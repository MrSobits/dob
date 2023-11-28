namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;

    /// <summary>
    /// Селектор для Дом
    /// </summary>
    public class DomSelectorService : BaseProxySelectorService<DomProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DomProxy> GetCache()
        {
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject> ();
            var municipalityFiasOktmoRepository = this.Container.ResolveRepository<MunicipalityFiasOktmo>();
            var tehPassportCacheService = this.Container.Resolve<ITehPassportCacheService>();

            using (this.Container.Using(realityObjectRepository,
                municipalityFiasOktmoRepository,
                tehPassportCacheService))
            {
                var tpBasementCache = tehPassportCacheService.GetCacheByRealityObjects("Form_1_3_3", 2, 1)
                    .ToDictionary(x => x.Key, x => x.Value.ToInt());

                var timeZone = this.SelectParams.GetAs("TimeZone", TimeZoneType.EuropeMoscow);

                var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>()
                    .ProxyListCache
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            ContragentId = x.Value.Contragent.ExportId,
                            x.Value.TypeManagement
                        });

                var placeOktmoDict = municipalityFiasOktmoRepository.GetAll()
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

                var realityObjects = this.FilterService
                    .FilterByRealityObject(realityObjectRepository.GetAll()
                    .Where(x => x.ConditionHouse == ConditionHouse.Serviceable));

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
                        OktmoCode = placeOktmoDict.Get(x.MuId)?.Get(x.PlaceGuidId) ?? x.Oktmo
                    })
                    .ToDictionary(x => x.Id);
            }
        }

        protected int? GetTypeHouse(TypeHouse? typeHouse)
        {
            switch (typeHouse)
            {
                case TypeHouse.ManyApartments:
                    return 1;
                case TypeHouse.Individual:
                case TypeHouse.SocialBehavior:
                    return 2;
                case TypeHouse.BlockedBuilding:
                    return 3;
                default:
                    return null;
            }
        }

        protected int? GetConditionId(ConditionHouse? conditionHouse)
        {
            switch (conditionHouse)
            {
                case ConditionHouse.Emergency:
                    return 1;
                case ConditionHouse.Serviceable:
                    return 2;
                case ConditionHouse.Dilapidated:
                    return 3;
                default:
                    return null;
            }
        }

        protected int? GetTypeContract(TypeManagementManOrg? typeValue)
        {
            switch (typeValue)
            {
                case TypeManagementManOrg.TSJ:
                    return 2; // ТСЖ
                case TypeManagementManOrg.JSK:
                    return 3; // ЖСК
                case TypeManagementManOrg.Other:
                    return 4; // Иной кооператив
                case TypeManagementManOrg.UK:
                    return 5; // УО
                default:
                    return 6; // Не выбран
            }
        }
    }
}