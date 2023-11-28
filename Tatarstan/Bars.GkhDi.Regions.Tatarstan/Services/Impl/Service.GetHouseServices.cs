namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Services.DataContracts;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    using Bars.GkhDi.Services;


    public partial class Service
    {
        public virtual GetHouseServicesResponse GetHouseServices(string houseId, string periodId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse != 0 && idPeriod != 0)
            {
                var diRealObj = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (diRealObj == null)
                {
                    return new GetHouseServicesResponse { Result = Result.DataNotFound };
                }

                var tariffRsoDict = this.Container.Resolve<IDomainService<TariffForRso>>().GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.Where(z => z.DateStart < DateTime.Now).OrderByDescending(x => x.DateStart).FirstOrDefault());

                var tariffForConsumers = this.Container.Resolve<IDomainService<TariffForConsumers>>().GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateStart).FirstOrDefault());

                var managServs = this.Container.Resolve<IDomainService<BaseService>>().GetAll()
                    .Where(x => x.TemplateService.KindServiceDi == KindServiceDi.Managing)
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Select(x => new ManagServ
                        {
                            Id = x.Id,
                            NameService = x.TemplateService.Name,
                            Provider = x.Provider.Name,
                            Measure = x.UnitMeasure.Name,
                            Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty
                        })
                    .ToArray();

                var comServs = this.Container.Resolve<IDomainService<CommunalService>>().GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Select(x => new ComServ
                    {
                        Id = x.Id,
                        NameService = x.TemplateService.Name,
                        Provider = x.Provider.Name,
                        Measure = x.UnitMeasure.Name,
                        Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty,
                        NameResOrg = x.Provider.Name,
                        TypeComServ = x.TemplateService.Name,
                        ResSize = x.VolumePurchasedResources.HasValue ? x.VolumePurchasedResources.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                        Tariff = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty,
                        DataNum = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null ? tariffRsoDict[x.Id].NumberNormativeLegalAct : string.Empty,
                        DataDate = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null ? tariffRsoDict[x.Id].DateNormativeLegalAct.ToDateTime().ToShortDateString() : string.Empty,
                        TarifSetOrg = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null ? tariffRsoDict[x.Id].OrganizationSetTariff : string.Empty,
                    })
                    .ToArray();
              
                var works = this.Container.Resolve<IDomainService<WorkRepairTechServ>>().GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Select(x => new
                        {
                            BaseServiceId = x.BaseService.Id,
                            x.Id,
                            GroupName = x.WorkTo.GroupWorkTo.Name,
                            x.WorkTo.Name
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.BaseServiceId)
                    .ToDictionary(
                        x => x.Key, 
                        y => new
                            {
                                NameGroup = String.Join(", ", y.Select(x => x.GroupName).Distinct()),
                                Works = y.Select(x => new Work { Id = x.Id, Name = x.Name }).ToArray()
                            });

                var worksToDict = this.Container.Resolve<IDomainService<RepairService>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Where(x => x.SumWorkTo != null)
                    .Select(x => new 
                        {
                            x.Id,
                            x.SumWorkTo,
                            x.SumFact,
                            x.DateStart,
                            x.DateEnd
                        })
                    .AsEnumerable()
                    .Where(x => works.ContainsKey(x.Id))
                    .Select(x => 
                        {
                            var dateStart = x.DateStart.HasValue ? x.DateStart.ToDateTime().ToShortDateString() : string.Empty;
                            var dateFinish = x.DateEnd.HasValue ? x.DateEnd.ToDateTime().ToShortDateString() : string.Empty;
                            var worksData = works[x.Id];

                            return new 
                            { 
                                x.Id, 
                                ServiceWorkToRt = new 
                                {
                                    planCost = x.SumWorkTo,
                                    factCost = x.SumFact,
                                    dateStart,
                                    dateFinish,
                                    worksData.NameGroup,
                                    worksData.Works
                                }
                            };
                        })
                    .ToDictionary(x => x.Id, x => x.ServiceWorkToRt);

                var worksRepairDetail = this.Container.Resolve<IDomainService<WorkRepairDetailTat>>().GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Select(x => new
                        {
                            GroupWorkPprId = x.WorkPpr.GroupWorkPpr.Id,
                            x.WorkPpr.Name,
                            UnitMeasure = x.UnitMeasure.Name,
                            x.PlannedVolume,
                            x.FactVolume
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.GroupWorkPprId)
                    .ToDictionary(
                        x => x.Key, 
                        y => y.Select(x => new Detail 
                        {
                            Name = x.Name,
                            FactSize = x.FactVolume.HasValue ? x.FactVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            PlanSize = x.PlannedVolume.HasValue ? x.PlannedVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            Izm = x.UnitMeasure ?? string.Empty
                        })
                        .ToArray());

                var worksPprByServiceDict = this.Container.Resolve<IDomainService<WorkRepairList>>().GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Select(x => new
                    {
                        x.Id,
                        BaseServiceId = x.BaseService.Id,
                        GroupWorkPprId = x.GroupWorkPpr.Id,
                        NameWork = x.GroupWorkPpr.Name,
                        x.PlannedCost,
                        x.FactCost,
                        x.DateStart,
                        x.DateEnd
                    })
                    .AsEnumerable()
                    .Select(x => new 
                        { 
                            x.BaseServiceId,
                            ServiceWorkPprRt = new 
                            {
                                x.Id,
                                x.NameWork,
                                PlanCost = x.PlannedCost,
                                FactSum = x.FactCost,
                                StartDate = x.DateStart.HasValue ? x.DateStart.ToDateTime().ToShortDateString() : string.Empty,
                                FinishDate = x.DateEnd.HasValue ? x.DateEnd.ToDateTime().ToShortDateString() : string.Empty,
                                Details = worksRepairDetail.ContainsKey(x.GroupWorkPprId) ? worksRepairDetail[x.GroupWorkPprId] : null
                            }
                        })
                    .GroupBy(x => x.BaseServiceId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ServiceWorkPprRt).ToList());

                var housingServ = this.Container.Resolve<IDomainService<BaseService>>().GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Where(x => x.TemplateService.KindServiceDi == KindServiceDi.Repair)
                    .Select(x => new { x.Id, x.TemplateService.Name })
                    .ToList();
                
                var housingServs = housingServ.Select(x =>
                {
                    var result = new HousingServ { Id = x.Id, NameService = x.Name };

                    var factCost = 0m;
                    var planCost = 0m;

                    if (worksToDict.ContainsKey(x.Id))
                    {
                        var worksTo = worksToDict[x.Id];

                        factCost = worksTo.factCost ?? 0;
                        planCost = worksTo.planCost ?? 0;

                        result.WorksTo = new ServiceWorkTo
                        {
                            DateFinish = worksTo.dateFinish,
                            DateStart = worksTo.dateStart,
                            FactCost = worksTo.factCost.HasValue ? worksTo.factCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            PlanCost = worksTo.planCost.HasValue ? worksTo.planCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            Works = worksTo.Works,
                            NameGroup = worksTo.NameGroup
                        };
                    }

                    if (worksPprByServiceDict.ContainsKey(x.Id))
                    {
                        var worksPpr = worksPprByServiceDict[x.Id];

                        worksPpr.ForEach(y =>
                            { 
                                factCost += y.FactSum ?? 0;
                                planCost += y.PlanCost ?? 0;
                            });

                        result.WorksPpr = worksPpr
                            .Select(y => new ServiceWorkPpr
                            {
                                Id = y.Id,
                                Details = y.Details,
                                NameWork = y.NameWork,
                                StartDate = y.StartDate,
                                FinishDate = y.FinishDate,
                                FactSum = y.FactSum.HasValue ? y.FactSum.Value.RoundDecimal(2).ToString(numberformat): string.Empty,
                                PlanCost = y.PlanCost.HasValue ? y.PlanCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty
                            })
                            .ToArray();
                    }

                    result.FactCost = factCost != 0 ? factCost.RoundDecimal(2).ToString(numberformat) : string.Empty;
                    result.PlanCost = planCost != 0 ? planCost.RoundDecimal(2).ToString(numberformat) : string.Empty;

                    return result;
                });

                return new GetHouseServicesResponse
                    {
                        ManagServs = managServs, 
                        HousingServs = housingServs.ToArray(), 
                        ComServs = comServs,
                        Result = Result.NoErrors
                    };
            }

            return new GetHouseServicesResponse { Result = Result.DataNotFound };
        }
    }
}
