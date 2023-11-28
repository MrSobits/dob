namespace Bars.GkhDi.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Enums;
    using Gkh.Services.DataContracts;

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
                var diRealObj = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                    .GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (diRealObj == null)
                {
                    return new GetHouseServicesResponse { Result = Result.DataNotFound };
                }

                var tariffRsoDict = Container.Resolve<IDomainService<TariffForRso>>()
                             .GetAll()
                             .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .GroupBy(x => x.BaseService.Id)
                             .ToDictionary(x => x.Key, y => y.Where(z => z.DateStart < DateTime.Now).OrderByDescending(x => x.DateStart).FirstOrDefault());

                var tariffForConsumers = Container.Resolve<IDomainService<TariffForConsumers>>()
                             .GetAll()
                             .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .GroupBy(x => x.BaseService.Id)
                             .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateStart).FirstOrDefault());

                var managServs = Container.Resolve<IDomainService<BaseService>>().GetAll()
                              .Where(x => x.TemplateService.KindServiceDi == KindServiceDi.Managing && x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                              .Select(x => new ManagServ
                                               {
                                                   Id = x.Id,
                                                   Name = x.TemplateService.Name,
                                                   Provider = x.Provider.Name,
                                                   Measure = x.UnitMeasure.Name,
                                                   Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty
                                               })
                             .ToArray();

                var housingServs = new List<HousingServ>();

                housingServs.AddRange(Container.Resolve<IDomainService<BaseService>>().GetAll()
                              .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id 
                                  && (x.TemplateService.KindServiceDi == KindServiceDi.Repair || x.TemplateService.KindServiceDi == KindServiceDi.CapitalRepair)) 
                              .Select(x => new HousingServ
                              {
                                  Id = x.Id,
                                  Name = x.TemplateService.Name,
                                  Provider = x.Provider.Name,
                                  Measure = x.UnitMeasure.Name,
                                  Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty
                              })
                             .ToList());

                housingServs.AddRange(Container.Resolve<IDomainService<AdditionalService>>().GetAll()
                              .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                              .Select(x => new HousingServ
                              {
                                  Id = x.Id,
                                  Name = x.TemplateService.Name,
                                  Provider = x.Provider.Name,
                                  Measure = x.UnitMeasure.Name,
                                  Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty,
                                  Periodicity = x.Periodicity.Name
                              })
                             .ToList());

                housingServs.AddRange(Container.Resolve<IDomainService<HousingService>>().GetAll()
                              .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                              .Select(x => new HousingServ
                              {
                                  Id = x.Id,
                                  Name = x.TemplateService.Name,
                                  Provider = x.Provider.Name,
                                  Measure = x.UnitMeasure.Name,
                                  Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty,
                                  Periodicity = x.Periodicity.Name
                              })
                             .ToList());

                var comServs = Container.Resolve<IDomainService<CommunalService>>().GetAll()
                              .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                              .Select(x => new ComServ
                              {
                                  Id = x.Id,
                                  NameService = x.TemplateService.Name,
                                  Provider = x.Provider.Name,
                                  Measure = x.UnitMeasure.Name,
                                  Cost = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty,
                                  NameResOrg = x.Provider.Name,
                                  TypeUtilities = x.TemplateService.Name,
                                  ResSize = x.VolumePurchasedResources.HasValue ? x.VolumePurchasedResources.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                  Tariff = tariffForConsumers.ContainsKey(x.Id) ? tariffForConsumers[x.Id].Cost.ToDecimal().RoundDecimal(2).ToString(numberformat) : string.Empty,
                                  DataNum = tariffRsoDict.ContainsKey(x.Id)  && tariffRsoDict[x.Id] != null ? tariffRsoDict[x.Id].NumberNormativeLegalAct : string.Empty,
                                  DataDate = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null ? tariffRsoDict[x.Id].DateNormativeLegalAct.ToDateTime().ToShortDateString() : string.Empty,
                                  TarifSetOrg = tariffRsoDict.ContainsKey(x.Id) && tariffRsoDict[x.Id] != null ? tariffRsoDict[x.Id].OrganizationSetTariff : string.Empty,
                              })
                             .ToArray();

                var works =
                    Container.Resolve<IDomainService<WorkRepairTechServ>>()
                             .GetAll()
                             .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .GroupBy(x => x.BaseService.Id)
                             .ToDictionary(x => x.Key, y => y.Select(x => new Work { Name = x.WorkTo.Name }).ToArray());

                var worksTo = Container.Resolve<IDomainService<RepairService>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                    .Select(x => new ServiceWorkTo
                                     {
                                         Id = x.Id,
                                         NameServ = x.TemplateService.Name,
                                         Cost = x.SumWorkTo.HasValue ? x.SumWorkTo.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         Works = works.ContainsKey(x.Id) ? works[x.Id] : null
                                     })
                    .AsEnumerable()
                    .Where(x => !string.IsNullOrEmpty(x.Cost) || (x.Works != null && x.Works.Any()))
                    .ToArray();

                var worksRepairDetail = Container.Resolve<IDomainService<WorkRepairDetail>>()
                             .GetAll()
                             .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .GroupBy(x => x.WorkPpr.GroupWorkPpr.Id)
                             .ToDictionary(x => x.Key, y => y.Select(x => new TypeWork { Name = x.WorkPpr.Name }).ToArray());

                var worksPpr = Container.Resolve<IDomainService<WorkRepairList>>()
                             .GetAll()
                             .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .Select(x => new ServiceWorkPpr
                                              {
                                                  Id = x.Id,
                                                  NameWork = x.GroupWorkPpr.Name,
                                                  PlanSize = x.PlannedVolume.HasValue ? x.PlannedVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                  PlanCost = x.PlannedCost.HasValue ? x.PlannedCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                  FactSize = x.FactVolume.HasValue ? x.FactVolume.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                  FactSum = x.FactCost.HasValue ? x.FactCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                  StartDate = x.DateStart.HasValue ? x.DateStart.ToDateTime().ToShortDateString() : string.Empty,
                                                  FinishDate = x.DateEnd.HasValue ? x.DateEnd.ToDateTime().ToShortDateString() : string.Empty,
                                                  TypeWorks = worksRepairDetail.ContainsKey(x.GroupWorkPpr.Id) ? worksRepairDetail[x.GroupWorkPpr.Id] : null
                                              })
                             .ToArray();

                return new GetHouseServicesResponse
                           {
                               ManagServs = managServs, 
                               HousingServs = housingServs.ToArray(), 
                               ComServs = comServs,
                               WorksPpr = worksPpr,
                               WorksTo = worksTo,
                               Result = Result.NoErrors
                           };
            }

            return new GetHouseServicesResponse { Result = Result.DataNotFound };
        }
    }
}
