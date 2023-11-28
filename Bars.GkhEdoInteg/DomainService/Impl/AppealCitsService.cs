namespace Bars.GkhEdoInteg.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Utils;
    using Bars.GkhEdoInteg.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Utils;

    /// <summary>
	/// Сервис для работы с Обращения граждан
	/// </summary>
    public class AppealCitsService : GkhGji.DomainService.AppealCitsService
    {
        /// <summary>
        /// Вернуть список обращений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="totalCount">Количество записей (выходной параметр)</param>
        /// <param name="usePaging">Маркер использования постраничного вывода</param>
        /// <returns>Результирующий список</returns>
        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceView = this.Container.Resolve<IDomainService<ViewAppealCitizensEdoInteg>>();
            var serviceAppealRo = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();

            try
            {
                var revenueSourceNamesFilter = baseParams.GetValueFromComplexFilter("RevenueSourceNames") as string;
                var revenueSourceNumbersFilter = baseParams.GetValueFromComplexFilter("RevenueSourceNumbers") as string;
                var revenueSourceDatesFilter = baseParams.GetValueFromComplexFilter("RevenueSourceDates") as DateTime?;

                var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

                var ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;

                var listIds = new List<long>();
                if (!string.IsNullOrEmpty(ids))
                {
                    if (ids.Contains(','))
                    {
                        listIds.AddRange(ids.Split(',').Select(id => id.ToLong()).ToList());
                    }
                    else
                    {
                        listIds.Add(ids.ToLong());
                    }
                }

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

                var dateFromStart = baseParams.Params.GetAs<DateTime>("dateFromStart");
                var dateFromEnd = baseParams.Params.GetAs<DateTime>("dateFromEnd");

                var checkTimeStart = baseParams.Params.GetAs<DateTime>("checkTimeStart");
                var checkTimeEnd = baseParams.Params.GetAs<DateTime>("checkTimeEnd");
                var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);

                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();

                var appealCitsIds = new List<long>();

                if (realityObjectId > 0)
                {
                    appealCitsIds.AddRange(serviceAppealRo.GetAll()
                        .Where(x => x.RealityObject.Id == realityObjectId)
                        .Select(x => x.AppealCits.Id)
                        .ToArray());
                }

                IQueryable<long> appealIdsFromSourceFilter = null;
                if (revenueSourceDatesFilter.HasValue || revenueSourceNamesFilter.IsNotEmpty() || revenueSourceNumbersFilter.IsNotEmpty())
                {
                    // здесь реализуется фильтрация по источникам жалобы
                    appealIdsFromSourceFilter = appealCitsSourceService.GetAll()
                        .WhereIf(revenueSourceNamesFilter.IsNotEmpty(), s => s.RevenueSource.Name.Contains(revenueSourceNamesFilter))
                        .WhereIf(revenueSourceNumbersFilter.IsNotEmpty(), s => s.RevenueSourceNumber.Contains(revenueSourceNumbersFilter))
                        .WhereIf(
                            revenueSourceDatesFilter > DateTime.MinValue, 
                            s => s.RevenueDate >= revenueSourceDatesFilter && s.RevenueDate < revenueSourceDatesFilter.Value.AddDays(1))
                        .Select(s => s.AppealCits.Id)
                        .Distinct();
                }

                // Фильтрация по инспектору была изменена по задаче 33244
                var query = serviceView.GetAll()
                    .WhereIf(
                        municipalityList.Count > 0,
                        x => (x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value)) || !x.MunicipalityId.HasValue)
                    .WhereIf(
                        inspectorsList.Count > 0,
                        x => inspectorsList.Contains(x.Executant.Id) || inspectorsList.Contains(x.Tester.Id) || inspectorsList.Contains(x.AppealCits.Surety.Id))
                    .WhereIf(appealCitsIds.Count > 0, x => appealCitsIds.Contains(x.Id))
                    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                    .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom >= dateFromStart)
                    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime >= dateFromStart)
                    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                    .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                    .WhereIf(appealIdsFromSourceFilter != null, x => appealIdsFromSourceFilter.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        Name = string.Format("{0} ({1})", x.Number, x.NumberGji),
                        // Для отображения в строке масового выбора
                        ManagingOrganization = x.ContragentName,
                        x.Number,
                        x.NumberGji,
                        x.DateFrom,
                        x.CheckTime,
                        x.QuestionsCount,
                        x.Municipality,
                        x.CountRealtyObj,
                        x.IsEdo,
                        Executant = x.Executant.Fio,
                        Tester = x.Tester.Fio,
                        SuretyResolve = x.SuretyResolve.Name,
                        x.ExecuteDate,
                        x.ZonalInspection,
                        x.RealObjAddresses,
                        x.Correspondent,
                        x.AddressEdo,
                        x.CountSubject,

                        //как бы все значения удовлетворяют фильтр
                        RevenueSourceNames = revenueSourceNamesFilter,
                        RevenueSourceNumbers = revenueSourceNumbersFilter,
                        RevenueSourceDates = revenueSourceDatesFilter
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParams, this.Container);

                if (usePaging)
                {
                    // Для скорости, если нет этих фильтров то считаем количество просто от таблицы, а не от вьюхи
                    // такое происходит при первом открытии реестра.
                    // проверка необходима, так как не делаем Select, через который мы получаем простой объект
                    if (loadParams.DataFilter == null
                        && loadParams.GetRuleValue("ManagingOrganization") == null
                        && loadParams.GetRuleValue("Municipality") == null
                        && loadParams.GetRuleValue("CountRealtyObj") == null
                        && loadParams.GetRuleValue("IsEdo") == null
                        && loadParams.GetRuleValue("RealObjAddresses") == null
                        && loadParams.GetRuleValue("AddressEdo") == null
                        && loadParams.GetRuleValue("CountSubject") == null
                        && loadParams.GetRuleValue("ManagingOrganization") == null
                        && loadParams.GetRuleValue("Executant") == null
                        && loadParams.GetRuleValue("SuretyResolve") == null
                        && loadParams.GetRuleValue("Tester") == null
                        && municipalityList.Count == 0)
                    {
                        var appealCitsDomainService = this.Container.ResolveDomain<AppealCits>();
                        using (this.Container.Using(appealCitsDomainService))
                        {
                            totalCount = appealCitsDomainService.GetAll()
                                .WhereIf(
                                    inspectorsList.Count > 0,
                                    x => inspectorsList.Contains(x.Executant.Id) || inspectorsList.Contains(x.Tester.Id) || inspectorsList.Contains(x.Surety.Id))
                                .WhereIf(appealCitsIds.Count > 0, x => appealCitsIds.Contains(x.Id))
                                .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                                .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                                .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom >= dateFromStart)
                                .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                                .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime >= dateFromStart)
                                .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                                .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                                .WhereIf(appealIdsFromSourceFilter != null, x => appealIdsFromSourceFilter.Contains(x.Id))
                                .Filter(loadParams, this.Container)
                                .Count();
                        }

                    }
                    else
                    {
                        totalCount = query.Count();
                    }
                    
                    query = query.Order(loadParams).Paging(loadParams);
                }
                else
                {
                    query = query.Order(loadParams);
                    totalCount = query.Count();
                }

                var data = query.ToList();

                var dataIds = data.Select(x => x.Id);

                var sources = appealCitsSourceService.GetAll()
                    .Where(s => dataIds.Contains(s.AppealCits.Id))
                    .Select(x => new
                    {
                        AppealCitsId = x.AppealCits.Id,
                        RevenueSourceName = x.RevenueSource.Name,
                        x.RevenueSourceNumber,
                        x.RevenueDate
                    })
                    .ToList();

                const string separator = ", ";

                var result = data.Select(ac => new
                {
                    ac.Id,
                    ac.State,
                    ac.Name,
                    // Для отображения в строке масового выбора
                    ac.ManagingOrganization,
                    ac.Number,
                    ac.NumberGji,
                    ac.DateFrom,
                    ac.CheckTime,
                    ac.QuestionsCount,
                    ac.Municipality,
                    ac.CountRealtyObj,
                    ac.IsEdo,
                    ac.Executant,
                    ac.Tester,
                    ac.SuretyResolve,
                    ac.ExecuteDate,
                    ac.ZonalInspection,
                    ac.RealObjAddresses,
                    ac.Correspondent,
                    ac.AddressEdo,
                    ac.CountSubject,
                    //прицепляем источники жалоб к новым колонкам в результате
                    RevenueSourceNames = sources.Where(s => s.AppealCitsId == ac.Id).AggregateWithSeparator(s => s.RevenueSourceName, separator),
                    RevenueSourceNumbers = sources.Where(s => s.AppealCitsId == ac.Id).AggregateWithSeparator(s => s.RevenueSourceNumber, separator),
                    RevenueSourceDates = sources.Where(s => s.AppealCitsId == ac.Id).AggregateWithSeparator(s => s.RevenueDate.ToDateString(), separator)
                }).ToList();

                return result;
            }
            finally 
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceView);
                this.Container.Release(serviceAppealRo);
                this.Container.Release(appealCitsSourceService);
            }
            
        }
    }
}