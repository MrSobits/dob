namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="KvarProxy"/>
    /// </summary>
    public class KvarSelectorService : BaseProxySelectorService<KvarProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, KvarProxy> GetCache()
        {
            var personalAccountRepository = this.Container.ResolveRepository<BasePersonalAccount>();
            var legalAccountOwnerRepository = this.Container.ResolveRepository<LegalAccountOwner>();

            var cashPaymentCenterPersAccRepository = this.Container.ResolveRepository<CashPaymentCenterPersAcc>();
            var cashPaymentCenterRealObjRepository = this.Container.ResolveRepository<CashPaymentCenterRealObj>();
            var personalAccountChangeRepository = this.Container.ResolveRepository<PersonalAccountChange>();

            using (this.Container.Using(personalAccountRepository,
                legalAccountOwnerRepository,
                cashPaymentCenterPersAccRepository,
                cashPaymentCenterRealObjRepository,
                personalAccountChangeRepository))
            {
                var indDict = this.ProxySelectorFactory.GetSelector<IndProxy>().ProxyListCache;
                var premisesDict = this.ProxySelectorFactory.GetSelector<PremisesProxy>().ProxyListCache;
                var roomIds = this.ProxySelectorFactory.GetSelector<RoomProxy>().ProxyListCache.Keys;
                var contragentDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>().ProxyListCache
                    .ToDictionary(x => x.Key, x => this.GetId(x.Value.Contragent));

                //достаем связку юр лицо-контрагент
                var legalDict = this.FilterService
                    .FilterByContragent(legalAccountOwnerRepository.GetAll(), x => x.Contragent)
                    .Select(x => new
                    {
                        x.Id,
                        ContragentId = (long?) x.Contragent.ExportId
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.ContragentId)
                    .ToDictionary(x => x.Key, x => x.Single());

                var cachPaymentCenterConnectionType = this.Container
                    .GetGkhConfig<RegOperatorConfig>()
                    .GeneralConfig
                    .CachPaymentCenterConnectionType;

                var accountList = this.FilterService
                    .FilterByRealityObject(personalAccountRepository.GetAll(), x => x.Room.RealityObject)
                    .Select(x => new AccountProxy
                    {
                        Id = x.Id,
                        PersonalAccountNum = x.PersonalAccountNum,
                        OwnerId = x.AccountOwner.Id,
                        RoomId = x.Room.Id,
                        RoId = x.Room.RealityObject.Id,
                        Area = x.AreaShare * x.Room.Area,
                        OpenDate = x.OpenDate,
                        CloseDate = x.CloseDate.IsValid() ? (DateTime?) x.CloseDate : null,
                        AreaShare = x.AreaShare
                    })
                    .AsEnumerable()
                    .Where(x => indDict.ContainsKey(x.OwnerId) || legalDict.ContainsKey(x.OwnerId))
                    .ToList();

                var proxyList = this.GetKvarProxyList(new AccountParams
                {
                    AccountList = accountList,
                    CachPaymentCenterConnectionType = cachPaymentCenterConnectionType,
                    IndDict = indDict,
                    LegalDict = legalDict,
                    RoomIds = roomIds,
                    PremisesDict = premisesDict,
                    ContragentDict = contragentDict,
                    CashPaymentCenterPersAccRepository = cashPaymentCenterPersAccRepository,
                    CashPaymentCenterRealObjRepository = cashPaymentCenterRealObjRepository,
                    PersonalAccountChangeRepository = personalAccountChangeRepository
                });

                return proxyList.ToDictionary(x => x.Id);
            }
        }

        private string GetAccountType(bool conductsAccrual)
        {
            return conductsAccrual ? "4" : "3";
        }

        private IList<KvarProxy> GetKvarProxyList(AccountParams accountParams)
        {
            var cachPaymentCenterConnectionType = accountParams.CachPaymentCenterConnectionType;
            var indDict = accountParams.IndDict;
            var premisesDict = accountParams.PremisesDict;
            var roomIds = accountParams.RoomIds;
            var contragentDict = accountParams.ContragentDict;
            var legalDict = accountParams.LegalDict;

            var count = accountParams.AccountList.Count;
            var take = 5000;
            var proxyList = new List<KvarProxy>();

            for (var skip = 0; skip < count; skip += take)
            {
                var accountPart = accountParams.AccountList
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                var accountsIds = accountPart.Select(x => x.Id).ToList();
                var accountsRoIds = accountPart.Select(x => x.RoId).ToList();
                var accountsCloseIds = accountPart.Where(y => y.CloseDate.HasValue).Select(x => x.Id).ToList();

                var accountTypeDict = new Dictionary<long, string>();
                var realObjTypeDict = new Dictionary<long, string>();

                // в зависимости от настроек тянем РКЦ через связь ЛС-РКЦ или Дом-РКЦ
                if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
                {
                    accountTypeDict = accountParams.CashPaymentCenterPersAccRepository.GetAll()
                        .Where(x => x.DateStart <= DateTime.Now.Date && (!x.DateEnd.HasValue || DateTime.Now.Date <= x.DateEnd))
                        .WhereContainsBulked(x => x.PersonalAccount.Id, accountsIds, take)
                        .GroupBy(x => x.PersonalAccount.Id, x => this.GetAccountType(x.CashPaymentCenter.ConductsAccrual))
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault());
                }
                else
                {
                    realObjTypeDict = accountParams.CashPaymentCenterRealObjRepository.GetAll()
                        .Where(x => x.DateStart <= DateTime.Now.Date && (!x.DateEnd.HasValue || DateTime.Now.Date <= x.DateEnd))
                        .WhereContainsBulked(x => x.RealityObject.Id, accountsRoIds, take)
                        .GroupBy(x => x.RealityObject.Id, x => this.GetAccountType(x.CashPaymentCenter.ConductsAccrual))
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault());
                }

                //для закрытых лс ищем причину закрытия
                var closeInfoList = accountParams.PersonalAccountChangeRepository.GetAll()
                    .Where(x => x.ChangeType == PersonalAccountChangeType.Close || x.ChangeType == PersonalAccountChangeType.MergeAndClose)
                    .WhereContainsBulked(x => x.PersonalAccount.Id, accountsCloseIds, take)
                    .Select(x => new
                    {
                        x.PersonalAccount.Id,
                        x.ActualFrom,
                        CloseDate = (DateTime?)x.PersonalAccount.CloseDate,
                        ChangeType = (PersonalAccountChangeType?)x.ChangeType,
                        ConditionHouse = (ConditionHouse?)x.PersonalAccount.Room.RealityObject.ConditionHouse,
                        x.Description,
                        Date = (DateTime?)x.Date
                    })
                    .ToList();

                var closeInfoDict = closeInfoList
                    .Select(x => new
                    {
                        x.Id,
                        CloseDate = x.ActualFrom ?? x.CloseDate,
                        CloseReasonType = x.ChangeType == PersonalAccountChangeType.MergeAndClose
                            ? "8"
                            : x.ConditionHouse == ConditionHouse.Razed
                                ? "7"
                                : "6",
                        x.Description,
                        x.Date
                    })
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Date).First());

                proxyList.AddRange(accountPart.Select(x => new KvarProxy
                {
                    Id = x.Id,
                    PersonalAccountNum = x.PersonalAccountNum,
                    OpenDate = x.OpenDate,

                    CloseDate = closeInfoDict.Get(x.Id)?.CloseDate,
                    CloseReasonType = closeInfoDict.Get(x.Id)?.CloseReasonType,
                    CloseReason = closeInfoDict.Get(x.Id)?.Description,
                    PrincipalContragentId = contragentDict.Get(x.RoId),
                    Area = x.Area,
                    PersonalAccountType =
                        (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount
                            ? accountTypeDict.Get(x.Id)
                            : realObjTypeDict.Get(x.RoId))
                        ?? "3",

                    IndividualOwner = indDict.Get(x.OwnerId),
                    ContragentId = legalDict.Get(x.OwnerId),
                    Premises = premisesDict.Get(x.RoomId),
                    RoomId = roomIds.Contains(x.RoomId) ? (long?)x.RoomId : null,
                    Share = x.AreaShare * 100
                }));
            }

            return proxyList;
        }

        private class AccountParams
        {
            public IList<AccountProxy> AccountList { get; set; }
            public CachPaymentCenterConnectionType CachPaymentCenterConnectionType { get; set; }
            public IRepository<CashPaymentCenterPersAcc> CashPaymentCenterPersAccRepository { get; set; }
            public IRepository<CashPaymentCenterRealObj> CashPaymentCenterRealObjRepository { get; set; }
            public IRepository<PersonalAccountChange> PersonalAccountChangeRepository { get; set; }
            public IDictionary<long, IndProxy> IndDict { get; set; }
            public IDictionary<long, PremisesProxy> PremisesDict { get; set; }
            public ICollection<long> RoomIds { get; set; }
            public IDictionary<long, long?> ContragentDict { get; set; }
            public IDictionary<long, long?> LegalDict { get; set; }
        }

        private class AccountProxy
        {
            public long Id { get; set; }
            public string PersonalAccountNum { get; set; }
            public long OwnerId { get; set; }
            public long RoomId { get; set; }
            public long RoId { get; set; }
            public decimal Area { get; set; }
            public DateTime OpenDate { get; set; }
            public DateTime? CloseDate { get; set; }
            public decimal AreaShare { get; set; }
        }
    }
}
