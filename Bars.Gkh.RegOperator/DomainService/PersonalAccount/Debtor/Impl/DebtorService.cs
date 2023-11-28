namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Debtor;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.Debtors;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;
    using NHibernate.Linq;

    using Dapper;
    using Gkh.Enums;
    using System;

    /// <summary>
    /// Сервис по должникам
    /// </summary>
    public class DebtorService : IDebtorService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Домен-сервис Объект капитального ремонта <see cref="ObjectCr"/>
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomainService { get; set; }

        /// <summary>
        /// Формирование реестра должников
        /// </summary>
        /// <param name="baseParams"> Базовый параметр </param>
        /// <returns></returns>
        public IDataResult Create(BaseParams baseParams)
        {
            var taskEntryDomain = this.Container.ResolveDomain<TaskEntry>();
            try
            {
                if (taskEntryDomain.GetAll()
                    .Any(
                        x => x.Status != TaskStatus.Error
                            && x.Status != TaskStatus.Succeeded
                            && x.Parent.TaskCode == DebtorsTaskProvider.Code))
                {
                    throw new ValidationException("Формирование реестра неплательщиков уже запущено");
                }

                return this.TaskManager.CreateTasks(new DebtorsTaskProvider(), baseParams);
            }
            finally
            {
                this.Container.Release(taskEntryDomain);
            }
        }

        /// <summary>
        /// Удаление неплательщиков
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <returns></returns>
        public IDataResult Clear(BaseParams baseParams)
        {
            var sessions = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(sessions))
            {
                sessions.GetCurrentSession().CreateQuery("delete from Debtor").ExecuteUpdate();
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <param name="paging">Постраничная навигация</param>
        /// <param name="totalCount">Общее количество записей</param>
        /// <returns></returns>
        public IList GetList(BaseParams baseParams, bool paging, out int totalCount)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            try
            {
                var loadParam = baseParams.GetLoadParam();

                var filter = new BaseParamsDebtorFilter(baseParams);

                filter.RealityObject = this.GetRealityObjectIdByProgramCr(filter.ProgramCrIds);

                var data = this.FilterByBaseParams(this.GetSelectDebtorDto(debtorDomain.GetAll(), claimWorkAccountDetailDomain.GetAll()), filter)
                    .Filter(loadParam, this.Container);

                totalCount = data.Count();

                data = paging ? data.Order(loadParam).Paging(loadParam) : data.Order(loadParam);

                return data.ToList();
            }
            finally
            {
                this.Container.Release(debtorDomain);
                this.Container.Release(claimWorkAccountDetailDomain);
            }
        }

        /// <summary>
        /// Создание работы
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <returns></returns>
        public IDataResult CreateClaimWorks(BaseParams baseParams)

        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var debtorClaimWorkUpdateService = this.Container.Resolve<IDebtorClaimWorkUpdateService>();

            try
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
                //Если нет отмеченых должников в списке, смотрим на массив AccNum
                if (ids.Length == 0 || ids == null)
                {
                    var acc_nums_temp = baseParams.Params.GetAs<string>("AccNum");

                    var ids_from_acc_num = GetIdsFromAccNum(acc_nums_temp);

                    //Роняем задачу при кривом вводе т.к. пустой массив id запустит формирование ПИР по всему реестру должников
                    if ((acc_nums_temp != null || acc_nums_temp != "") && (ids_from_acc_num.Length == 0))
                    {
                        return new BaseDataResult(false, "Строка со списком ЛС была пустая, либо по всем указанным лс нет записей в реестре должников");
                    }

                    //Передаем id, если найдены
                    if (ids.Length == 0 && ids_from_acc_num.Length > 0)
                    {
                        ids = ids_from_acc_num;
                    }
                }
                

                var loadParams = baseParams.GetLoadParam();

                var filter = new BaseParamsDebtorFilter(baseParams);

                filter.RealityObject = this.GetRealityObjectIdByProgramCr(filter.ProgramCrIds);

                var filterExist = (loadParams.DataFilter != null
                        && (!string.IsNullOrEmpty(loadParams.DataFilter.DataIndex) || loadParams.DataFilter.Filters.IsNotEmpty())
                        || loadParams.Filter.IsNotEmpty())
                    || filter.HasFilter;

                IQueryable<BasePersonalAccount> persAccQuery;

                if (filterExist && ids.IsEmpty())
                {
                    var debtorList = this.FilterByBaseParams(this.GetSelectDebtorDto(debtorDomain.GetAll(), claimWorkAccountDetailDomain.GetAll()), filter)
                        .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Id))
                        .Filter(loadParams, this.Container)
                        .Where(x => x.PersonalAccountId.HasValue)
                        .Select(x => x.PersonalAccountId.Value)
                        .AsEnumerable()
                        .Distinct()
                        .ToList();

                    persAccQuery = persAccDomain.GetAll()
                        .WhereContainsBulked(x => x.Id, debtorList);
                }
                else
                {
                    persAccQuery = debtorDomain.GetAll()
                        .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Id))
                        .Select(x => new BasePersonalAccount
                        {
                         Id = x.PersonalAccount.Id
                        });
                }

                return debtorClaimWorkUpdateService.CreateClaimWorks(persAccQuery);
            }
            finally
            {
                this.Container.Release(claimWorkAccountDetailDomain);
                this.Container.Release(debtorClaimWorkUpdateService);
                this.Container.Release(debtorDomain);
                this.Container.Release(persAccDomain);
            }
        }

        private long[] GetIdsFromAccNum(string accNumString)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            long[] res = new long[0];
            try
            {
                //Чистим строку от пробелов и разбиваем по запятым
                string trimmed = new string(accNumString.ToCharArray()
                                .Where(c => !Char.IsWhiteSpace(c))
                                .ToArray());
                string[] accNums = trimmed.Split(',');

                //var accDict = persAccDomain.GetAll().Select(x => new { x.Id, x.PersonalAccountNum }).ToDictionary(x=>x.PersonalAccountNum,x=>x.Id);

                var persAccIds = persAccDomain.GetAll().Where(x => accNums.Contains(x.PersonalAccountNum)).Select(x => x.Id);

                List<long> result = new List<long>();
                debtorDomain.GetAll().Where(x => persAccIds.Contains(x.PersonalAccount.Id)).Select(x => x.Id).ForEach(x => result.Add(x));
                res = result.ToArray();
            }
            catch
            {
                //Catch&Ignore
            }
            finally
            {
                Container.Release(debtorDomain);
                Container.Release(persAccDomain);
            }
            return res;
        }

        /// <summary>
        /// Обновление учреждения в судебной практике
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult UpdateJurInstitution(BaseParams baseParams)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var debtorJurInstitutionCache = this.Container.Resolve<IDebtorJurInstitutionCache>();

            try
            {
                var debtorsCount = debtorDomain.GetAll().Count();

                var step = 10000;

                for (int startIndex = 0; startIndex <= debtorsCount; startIndex += step)
                {
                    var debtors = debtorDomain.GetAll()
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.Room)
                        .ThenFetch(x => x.RealityObject)
                        .OrderBy(x => x.Id)
                        .Skip(startIndex)
                        .Take(step);

                    debtorJurInstitutionCache.InitCache(debtors.Select(x => x.PersonalAccount.Room.RealityObject.Id).ToArray());

                    var listToSave = new List<Debtor>();

                    foreach (var debtor in debtors)
                    {
                        if (debtorJurInstitutionCache.SetJurInstitution(debtor, debtor.PersonalAccount))
                        {
                            listToSave.Add(debtor);
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container, listToSave, listToSave.Count, true, true);
                }
            }
            finally
            {
                this.Container.Release(debtorDomain);
                this.Container.Release(debtorJurInstitutionCache);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Получаем список жилых домов отфильтрованных по МО и программе КР
        /// </summary>
        /// <param name="muncipalityIds">список "Муниципальное образование"</param>
        /// <param name="programCrIds">список "Программа капитального ремонта"</param>
        /// <returns>Список ID Жилых домов</returns>
        private long[] GetRealityObjectIdByProgramCr(long[] programCrIds)
        {
            if (programCrIds.Length > 0)
            {
                return this.ObjectCrDomainService.GetAll()
                    .Where(x => x.ProgramCr != null)
                    .Where(x => x.RealityObject != null)
                    .WhereNotNull(x => x.RealityObject.Municipality)
                    .WhereIf(programCrIds.IsNotEmpty(), x => programCrIds.Contains(x.ProgramCr.Id))
                    .Select(x => x.RealityObject.Id)
                    .ToArray();
            }
                
            return new long[0];
        }

        private IQueryable<DebtorDto> GetSelectDebtorDto(IQueryable<Debtor> debtorQuery, IQueryable<ClaimWorkAccountDetail> claimWorkAccountDetailQuery)
        {
            var persAccSerivce = Container.ResolveDomain<IndividualAccountOwner>();
            return debtorQuery.Select(
                 x => new DebtorDto
                 {
                     Id = x.Id,
                     PersonalAccountId = x.PersonalAccount.Id,
                     MunicipalityId = x.PersonalAccount.Room.RealityObject.Municipality.Id,
                     Municipality = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                     Settlement = x.PersonalAccount.Room.RealityObject.MoSettlement.Name,
                     RoomAddress = x.PersonalAccount.Room.RealityObject.Address + ", кв. " + x.PersonalAccount.Room.RoomNum,
                     State = x.PersonalAccount.State,
                     PersonalAccountNum = x.PersonalAccount.PersonalAccountNum,
                     AccountOwnerId = x.PersonalAccount.AccountOwner.Id,
                     AccountOwner = x.PersonalAccount.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal
                         ? (x.PersonalAccount.AccountOwner as LegalAccountOwner).Contragent.Name
                         : x.PersonalAccount.AccountOwner.Name,
                     OwnerType = x.PersonalAccount.AccountOwner.OwnerType,
                     DebtSum = x.DebtSum,
                     DebtBaseTariffSum = x.DebtBaseTariffSum,
                     DebtDecisionTariffSum = x.DebtDecisionTariffSum,
                     ExpirationDaysCount = x.ExpirationDaysCount,
                     ExpirationMonthCount = x.ExpirationMonthCount,
                     PenaltyDebt = x.PenaltyDebt,
                     HasClaimWork = claimWorkAccountDetailQuery.Any(y => y.PersonalAccount.Id == x.PersonalAccount.Id && !y.ClaimWork.State.FinalState),
                     CourtType = x.CourtType,
                     JurInstitution = x.JurInstitution.ShortName,
                     UserName = claimWorkAccountDetailQuery.Where(y => y.PersonalAccount.Id == x.PersonalAccount.Id && !y.ClaimWork.State.FinalState).Select(y => ((DebtorClaimWork)y.ClaimWork).User.Name).FirstOrDefault() ?? string.Empty,
                     RealityObjectId = x.PersonalAccount.Room.RealityObject.Id,
                     ExtractExists = x.ExtractExists,
                     AccountRosregMatched = x.AccountRosregMatched,
                     Underage = ((x.PersonalAccount.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual) 
                                &&(persAccSerivce.Get(x.PersonalAccount.AccountOwner.Id) != null) && (persAccSerivce.Get(x.PersonalAccount.AccountOwner.Id).BirthDate != null))
                                ? (DateTime.Now - persAccSerivce.Get(x.PersonalAccount.AccountOwner.Id).BirthDate) < TimeSpan.FromDays(6574) //365 дней * 18 лет + 4 дня на високосные года
                                : false,
                     OwnerArea = x.PersonalAccount.Room.Area * x.PersonalAccount.AreaShare
                  });
        }

        private IQueryable<DebtorDto> FilterByBaseParams(IQueryable<DebtorDto> debtorDtoQuery, BaseParamsDebtorFilter filter)
        {
            return debtorDtoQuery
                .WhereIf(filter.MuncipalityIds.Length > 0, x => filter.MuncipalityIds.Contains(x.MunicipalityId))
                .WhereIf(filter.OwnerIds.Length > 0, x => filter.OwnerIds.Contains(x.AccountOwnerId))
                .WhereIf(filter.StateIds.Length > 0, x => filter.StateIds.Contains(x.State.Id))
                .WhereIf(filter.RealityObject.Length > 0, x => filter.RealityObject.Contains(x.RealityObjectId));
        }

        private class DebtorDto
        {
            public long Id { get; set; }
            public long? PersonalAccountId { get; set; }
            public long MunicipalityId { get; set; }
            public string Municipality { get; set; }
            public string Settlement { get; set; }
            public string RoomAddress { get; set; }
            public State State { get; set; }
            public string PersonalAccountNum { get; set; }
            public long AccountOwnerId { get; set; }
            public string AccountOwner { get; set; }
            public PersonalAccountOwnerType OwnerType { get; set; }
            public decimal DebtSum { get; set; }
            public decimal DebtBaseTariffSum { get; set; }
            public decimal DebtDecisionTariffSum { get; set; }
            public int ExpirationDaysCount { get; set; }
            public int? ExpirationMonthCount { get; set; }
            public decimal PenaltyDebt { get; set; }
            public bool HasClaimWork { get; set; }
            public Gkh.Enums.ClaimWork.CourtType CourtType { get; set; }
            public string JurInstitution { get; set; }
            public string UserName { get; set; }
            public long RealityObjectId { get; set; }
            public string ProgramCr { get; set; }
            public decimal OwnerArea { get; set; }
            public bool Underage { get; set; }
            public YesNo? ExtractExists { get; set; }
            public YesNo? AccountRosregMatched { get; set; }
        }

        /// <summary>
        /// Класс, который содержит в себе вытащенные из параметра запроса
        /// </summary>
        private class BaseParamsDebtorFilter
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="baseParams">Параметры запроса</param>
            public BaseParamsDebtorFilter(BaseParams baseParams)
            {
                this.MuncipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
                this.StateIds = baseParams.Params.GetAs<long[]>("stateIds");
                this.OwnerIds = baseParams.Params.GetAs<long[]>("ownerIds");
                this.ProgramCrIds = baseParams.Params.GetAs<long[]>("programCrIds");
            }

            public long[] MuncipalityIds { get; private set; }

            public long[] StateIds { get; private set; }

            public long[] OwnerIds { get; private set; }

            public long[] ProgramCrIds { get; private set; }

            public long[] RealityObject { get; set; }

            public bool HasFilter => (this.MuncipalityIds != null && this.MuncipalityIds.Length > 0)
                || (this.StateIds != null && this.StateIds.Length > 0)
                || (this.OwnerIds != null && this.OwnerIds.Length > 0);
        }
    }
}