namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.Utils;

    using MainSelectorNamespace = Gkh.FormatDataExport.ProxySelectors.Impl;

    /// <summary>
    /// Селектор для Дом
    /// </summary>
    public class DomSelectorService : MainSelectorNamespace.DomSelectorService
    {
        /// <inheritdoc />
        protected override IDictionary<long, DomProxy> GetCache()
        {
            var realityObjectRepository = this.Container.Resolve<IRepository<RealityObject>>();
            var municipalityFiasOktmoRepository = this.Container.Resolve<IRepository<MunicipalityFiasOktmo>>();
            var tehPassportCacheService = this.Container.Resolve<ITehPassportCacheService>();
            var calcAccountService = this.Container.Resolve<ICalcAccountService>();

            using (this.Container.Using(realityObjectRepository,
                municipalityFiasOktmoRepository,
                tehPassportCacheService,
                calcAccountService))
            {
                var tpBasementCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1_3_3", 2, 1, this.FilterService.RealityObjectIds)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var timeZone = this.SelectParams.GetAs("TimeZone", TimeZoneType.EuropeMoscow);

                var contragentRschets = this.ProxySelectorFactory.GetSelector<ContragentRschetProxy>()
                    .ExtProxyListCache;

                var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>()
                    .ProxyListCache
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            ContragentId = x.Value.Contragent.ExportId,
                            x.Value.TypeManagement
                        });

                var realityObjects = this.FilterService
                    .FilterByRealityObject(realityObjectRepository.GetAll()
                        .Where(x => x.ConditionHouse == ConditionHouse.Serviceable));

                var calcAccountDict = calcAccountService.GetRobjectsAccounts(realityObjects);

                var roCalcAccountDict = calcAccountDict
                    .Join(
                        contragentRschets,
                        x => x.Value.AccountNumber,
                        x => x.SettlementAccount,
                        (x, y) => new
                        {
                            x.Key,
                            y.Id
                        })
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).FirstOrDefault());

                var placeOktmoDict = municipalityFiasOktmoRepository.GetAll()
                    .WhereNotNull(x => x.Municipality)
                    .Select(x => new
                    {
                        x.Municipality.Id,
                        x.FiasGuid,
                        x.Oktmo
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => x.Select(y => new
                            {
                                y.FiasGuid,
                                y.Oktmo
                            })
                            .GroupBy(y => y.FiasGuid, y => y.Oktmo)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                return realityObjects
                    .Select(x => new
                    {
                        x.Id,
                        MuId = x.Municipality.Id,
                        City = x.Municipality.Name,
                        Settlement = x.FiasAddress.PlaceName,
                        x.FiasAddress.PlaceGuidId,
                        x.FiasAddress.StreetName,
                        x.FiasAddress.House,
                        x.FiasAddress.Building,
                        x.FiasAddress.Housing,
                        x.FiasAddress.Letter,
                        x.MaximumFloors,
                        x.BuildYear,
                        x.AreaMkd,
                        x.AreaCommonUsage,
                        x.FiasAddress.StreetGuidId,
                        x.FiasAddress.HouseGuid,
                        x.CadastralHouseNumber,
                        x.PhysicalWear,
                        x.TypeHouse,
                        x.AreaLiving,
                        x.DateCommissioning,
                        x.Floors,
                        x.IsCulturalHeritage,
                        x.Municipality.Oktmo,
                        x.ConditionHouse,
                    })
                    .AsEnumerable()
                    .Select(x => new DomProxy
                    {
                        Id = x.Id,
                        City = x.City,
                        Settlement = x.Settlement,
                        Street = x.StreetName,
                        House = x.House,
                        Building = x.Building,
                        Housing = x.Housing,
                        Letter = x.Letter,
                        ContragentId = manOrgDict.Get(x.Id)?.ContragentId,
                        MaximumFloors = x.MaximumFloors,
                        BuildYear = x.BuildYear,
                        AreaMkd = x.AreaMkd,
                        AreaCommonUsage = x.AreaCommonUsage,
                        StreetGuid = x.StreetGuidId,
                        HouseGuid = x.HouseGuid.ToStr(),
                        CadastralHouseNumber = x.CadastralHouseNumber,
                        EgrpNumber = x.CadastralHouseNumber,
                        ConditionHouseId = this.GetConditionId(x.ConditionHouse),
                        TypeHouse = this.GetTypeHouse(x.TypeHouse),
                        AreaLiving = x.AreaLiving,
                        CommissioningYear = x.DateCommissioning,
                        UndergroundFloorCount = tpBasementCache.Get(x.Id),
                        MinimumFloors = x.Floors,
                        TimeZone = timeZone.ToInt().ToString(),
                        IsCulturalHeritage = x.IsCulturalHeritage ? 1 : 2,
                        TypeManagement = this.GetTypeContract(manOrgDict.Get(x.Id)?.TypeManagement),
                        OktmoCode = placeOktmoDict.Get(x.MuId)?.Get(x.PlaceGuidId) ?? x.Oktmo,
                        CalcAccountId = roCalcAccountDict.Get(x.Id)
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}