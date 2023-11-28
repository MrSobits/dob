namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис по работе с основаниями претензионно-исковой работы для неплательщиков
    /// </summary>
    public class IndividualClaimWorkService : DebtorClaimWorkService<IndividualClaimWork>
    {
        private readonly IUserIdentity userIdentity;

        public IndividualClaimWorkService(IUserIdentity userIdentity)
        {
            this.userIdentity = userIdentity;
        }

        /// <summary>
        /// Вернуть список ПИР
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="isPaging">Флаг пагинации</param>
        /// <param name="totalCount">Общее количество записей</param>
        /// <returns>Результат запроса</returns>
        public override IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var loadParam = baseParams.GetLoadParam();
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var paused = baseParams.Params.GetAs<bool>("showPaused", false);
            var config = this.Container.GetGkhConfig<DebtorClaimWorkConfig>();
            var domainService = this.Container.ResolveDomain<ViewIndividualClaimWork>();

            try
            {
                if (paused)
                {
                    var configfilterByUser = config.General.FilterByUser;

                    var data = domainService.GetAll()
                        .WhereIf(configfilterByUser, x => x.User.Id == this.userIdentity.UserId)
                        .Where(x=> x.DebtorState == DebtorState.PausedChangeAcc || x.DebtorState == DebtorState.PausedGetInfoUnderage)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.State,
                                x.DebtorState,
                                x.Municipality,
                                x.AccountOwnerName,
                                x.RegistrationAddress,
                                x.AccountsAddress,
                                x.AccountsNumber,
                                x.CurrChargeBaseTariffDebt,
                                x.CurrChargeDecisionTariffDebt,
                                x.CurrChargeDebt,
                                x.CurrPenaltyDebt,
                                x.IsDebtPaid,
                                x.DebtPaidDate,
                                x.PIRCreateDate,
                                x.FirstDocCreateDate,
                                x.HasCharges185FZ,
                                x.MinShare,
                                x.Underage,
                                UserName = x.User.Name ?? string.Empty
                            })
                        .Filter(loadParam, this.Container);

                    totalCount = data.Count();

                    data = isPaging ? data.Order(loadParam).Paging(loadParam) : data.Order(loadParam);

                    return data.ToList();
                }
                else
                {
                    var configfilterByUser = config.General.FilterByUser;

                    var data = domainService.GetAll()
                       .Where(x => !x.FirstDocCreateDate.HasValue || x.FirstDocCreateDate.HasValue && (x.FirstDocCreateDate.Value >= dateStart2 && x.FirstDocCreateDate.Value <= dateEnd2))
                       .Where(x => x.DebtorState != DebtorState.PausedChangeAcc && x.DebtorState != DebtorState.PausedGetInfoUnderage)
                        .WhereIf(configfilterByUser, x => x.User.Id == this.userIdentity.UserId)
                        .Select(
                            x => new
                            {
                                x.Id,
                                x.State,
                                x.DebtorState,
                                x.Municipality,
                                x.AccountOwnerName,
                                x.RegistrationAddress,
                                x.AccountsAddress,
                                x.AccountsNumber,
                                x.CurrChargeBaseTariffDebt,
                                x.CurrChargeDecisionTariffDebt,
                                x.CurrChargeDebt,
                                x.CurrPenaltyDebt,
                                x.IsDebtPaid,
                                x.DebtPaidDate,
                                x.PIRCreateDate,
                                x.FirstDocCreateDate,
                                x.HasCharges185FZ,
                                x.MinShare,
                                x.Underage,
                                UserName = x.User.Name ?? string.Empty
                            })
                        .Filter(loadParam, this.Container);

                    totalCount = data.Count();

                    data = isPaging ? data.Order(loadParam).Paging(loadParam) : data.Order(loadParam);

                    return data.ToList();
                }
            }
            finally
            {
                this.Container.Release(domainService);
                this.Container.Release(config);
            }
        }

        /// <inheritdoc />
        public override IDataResult UpdateStates(BaseParams baseParams, bool inTask = false)
        {
            if (inTask)
            {
                var debtorType = DebtorType.Individual;
                baseParams.Params["debtorType"] = debtorType;
                return this.CreateTask(baseParams);
            }

            return this.DebtorClaimWorkUpdateService.UpdateStates(baseParams);
        }

        public override IDataResult PauseState(BaseParams baseParams)//, DebtorState newDebtorState
        {
            return BaseDataResult.Error("Отсутствует конечный статус");
        }
    }
}