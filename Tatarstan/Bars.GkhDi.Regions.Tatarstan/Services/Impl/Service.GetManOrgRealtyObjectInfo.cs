namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Services.DataContracts;
    using Entities;
    using Enums;
    using GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;
    using GkhDi.Services.DataContracts.GetPeriods;
    using GkhDi.Entities;

    using InformationOnContracts = GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.InformationOnContracts;

    public partial class Service
    {
        public virtual GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = "."; 
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();

            var disclosureInfoRealObjService = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();

            // Сведения о договорах
            var informationOnContracts = Container.Resolve<IDomainService<GkhDi.Entities.InformationOnContracts>>()
                                .GetAll()
                                .Where(x => x.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.DisclosureInfo.PeriodDi.Id,
                                        x.Name,
                                        x.Cost,
                                        x.Number,
                                        x.PartiesContract,
                                        x.Comments,
                                        x.From,
                                        x.DateStart,
                                        x.DateEnd
                                    })
                                .AsEnumerable();

            // Сведения о случаях снижения платы
            var infoAboutReductionPayment = Container.Resolve<IDomainService<InfoAboutReductionPayment>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                        x.Id,
                                        x.BaseService.TemplateService.Name,
                                        x.BaseService.TemplateService.TypeGroupServiceDi,
                                        x.ReasonReduction,
                                        x.RecalculationSum,
                                        x.OrderDate,
                                        x.OrderNum,
                                        x.Description
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(
                                    x => x.Key,
                                    y => y.AsEnumerable().Select(
                                        z => new InfoAboutReductPaymentItem
                                                 {
                                                    Id = z.Id,
                                                    Name = z.Name.ToStr(),
                                                    TypeGroupServiceDi = z.TypeGroupServiceDi.GetEnumMeta().Display,
                                                    ReasonReduction = z.ReasonReduction.ToStr(),
                                                    RecalculationSum = z.RecalculationSum.HasValue ? z.RecalculationSum.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                    OrderDate = z.OrderDate.HasValue && z.OrderDate.Value.Date != DateTime.MinValue.Date ? z.OrderDate.Value.ToShortDateString() : string.Empty,
                                                    OrderNumber = z.OrderNum.ToStr(),
                                                    Description = z.Description.ToStr()
                                                })
                                    .ToArray());

            var worksDomain = Container.Resolve<IDomainService<PlanReductionExpenseWorks>>();

            var planReductionExpenseWorksQuery = worksDomain
                .GetAll()
                .Where(x => x.PlanReductionExpense.DisclosureInfoRealityObj.RealityObject.Id == idHouse);

            var measureNames = Container.Resolve<IDomainService<PlanReduceMeasureName>>().GetAll()
              .Where(x => planReductionExpenseWorksQuery.Select(y => y.Id).Contains(x.PlanReductionExpenseWorks.Id))
              .Select(x => new { x.PlanReductionExpenseWorks.Id, x.MeasuresReduceCosts.MeasureName })
              .ToDictionary(x => x.Id, x => x.MeasureName);

            // План работ и мер по снижению расходов
            var planReductionExpenseWorks = worksDomain
                                .GetAll()
                                .Where(x => x.PlanReductionExpense.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.PlanReductionExpense.DisclosureInfoRealityObj.PeriodDi.Id,
                                        x.Id,
                                        x.Name,
                                        x.DateComplete,
                                        x.PlannedReductionExpense,
                                        x.FactedReductionExpense,
                                        x.ReasonRejection
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable().Select(z => new PlanReductionExpItem
                                                                                                {
                                                                                                    Id = z.Id,
                                                                                                    Name = measureNames.ContainsKey(z.Id) ? measureNames[z.Id] : z.Name.ToStr(),
                                                                                                    DateComplete = z.DateComplete.HasValue && z.DateComplete.Value.Date != DateTime.MinValue.Date ? z.DateComplete.Value.ToShortDateString() : string.Empty,
                                                                                                    PlannedReductionExpense = z.PlannedReductionExpense.HasValue ? z.PlannedReductionExpense.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                                                    FactedReductionExpense = z.FactedReductionExpense.HasValue ? z.FactedReductionExpense.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                                                    ReasonRejection = z.ReasonRejection
                                                                                                })
                                                                                .ToArray());

            // Работы по содержанию и ремонту МКД
            var workPprRepairDetailService = Container.Resolve<IDomainService<WorkRepairDetailTat>>();
            var planWorkServiceRepWorksItem = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>()
                                .GetAll()
                                .Where(x => x.PlanWorkServiceRepair.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.PlanWorkServiceRepair.DisclosureInfoRealityObj.PeriodDi.Id,
                                        GroupWorkPprId = x.WorkRepairList.GroupWorkPpr.Id,
                                        x.WorkRepairList.GroupWorkPpr.Name,
                                        Periodicity = x.PeriodicityTemplateService.Name,
                                        x.DateStart,
                                        x.DateEnd,
                                        x.DateComplete,
                                        x.Cost,
                                        x.FactCost,
                                        x.ReasonRejection
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => new PlanWorkServiceRepWorksItem
                                {
                                    WorksPpr = y.AsEnumerable().Select(z => new GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.WorkPpr
                                    {
                                                        NamePpr = z.Name.ToStr(),
                                                        PeriodicityTemplateService = z.Periodicity.ToStr(),
                                                        DateEnd = z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date ? z.DateEnd.Value.ToShortDateString() : string.Empty,
                                                        DateComplete = z.DateComplete.HasValue && z.DateComplete.Value.Date != DateTime.MinValue.Date ?
                                                            z.DateStart.Value.ToShortDateString() + " - " + z.DateComplete.Value.ToShortDateString() : string.Empty,
                                                        Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2) : 0,
                                                        FactCost = z.FactCost.HasValue ? z.FactCost.Value.RoundDecimal(2) : 0,
                                                        ReasonRejection = z.ReasonRejection.ToStr(),

                                                        Details = workPprRepairDetailService.GetAll()
                                                                    .Where(x => x.WorkPpr.GroupWorkPpr.Id == z.GroupWorkPprId)
                                                                    .Where(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                                                    .Select(x => new DetailPpr
                                                                    {
                                                                        Name = x.WorkPpr.Name,
                                                                        FactSize = x.FactVolume.HasValue ? x.FactVolume.Value : 0M,
                                                                        PlanSize = x.PlannedVolume.HasValue ? x.PlannedVolume.Value : 0M,
                                                                        Izm = x.WorkPpr.Measure.ShortName
                                                                    }).ToArray()
                                                    }).ToArray()
                                });

            var worksTo = Container.Resolve<IDomainService<RepairService>>().GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                    .Select(x => x)
                    .ToDictionary(x => x.Id, y => y); 

            var workRepairTechServ = Container.Resolve<IDomainService<WorkRepairTechServ>>().GetAll()
                .Where(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                .Select(x => new
                {
                    x.BaseService.Id,
                    WorkGroupId = x.WorkTo.GroupWorkTo.Id,
                    PeriodId = x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id,
                    NameGroup = x.WorkTo.GroupWorkTo.Name,
                    DetailToName = x.WorkTo.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.PeriodId)
                .ToDictionary(x => x.Key, y => new PlanWorkServiceRepWorksItem
                {
                    WorksTo = y.AsEnumerable().Select(z => new GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.WorkTo
                    {
                        NameGroup = z.NameGroup,
                        DateComplete = (worksTo.ContainsKey(z.Id) && worksTo[z.Id].DateStart.HasValue && worksTo[z.Id].DateEnd.HasValue) ?
                                        worksTo[z.Id].DateStart.Value.ToShortDateString() + " - " + worksTo[z.Id].DateEnd.Value.ToShortDateString() : "",
                        DateEnd = (worksTo.ContainsKey(z.Id) && worksTo[z.Id].DateEnd.HasValue) ? worksTo[z.Id].DateEnd.Value.ToShortDateString() : "",
                        ReasonRejection = worksTo.ContainsKey(z.Id) ? worksTo[z.Id].RejectCause : "",
                        DetailsTo = new [] {new DetailTo {Name = z.DetailToName}}
                        
                    }).ToArray()
                });

            foreach (var period in workRepairTechServ)
            {
                if (planWorkServiceRepWorksItem.ContainsKey(period.Key))
                {
                    planWorkServiceRepWorksItem[period.Key].WorksTo = period.Value.WorksTo;
                }
                else
                {
                    planWorkServiceRepWorksItem.Add(period.Key, period.Value);
                }
            }

            // Документы
            var documents = Container.Resolve<IDomainService<DocumentsRealityObj>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                             {
                                                 PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                                 x.Id,
                                                 x.FileActState,
                                                 x.FileCatalogRepair,
                                                 x.FileReportPlanRepair
                                             })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // Протоколы
            var currentYear = DateTime.Now.Year;
            var protocols = Container.Resolve<IDomainService<DocumentsRealityObjProtocol>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == idHouse)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Year,
                            x.File
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Year)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // Прочие услуги
            var otherService = Container.Resolve<IDomainService<OtherService>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                        x.Id,
                                        x.Name,
                                        x.UnitMeasure,
                                        x.Tariff,
                                        x.Code
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                                .Select(z => new OtherServiceItem
                                                                {
                                                                    Id = z.Id,
                                                                    Name = z.Name.ToStr(),
                                                                    UnitMeasure = z.UnitMeasure.ToStr(),
                                                                    Tariff = z.Tariff.HasValue ? z.Tariff.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    Code = z.Code.ToStr()
                                                                })
                                                                .ToList());

            // Сведения об оплатах
            var infoAboutPaymentHousing = Container.Resolve<IDomainService<InfoAboutPaymentHousing>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                        x.Id,
                                        x.BaseService.TemplateService.Name,
                                        x.BaseService.TemplateService.TypeGroupServiceDi,
                                        ProviderName = x.BaseService.Provider.Name,
                                        x.CounterValuePeriodStart,
                                        x.CounterValuePeriodEnd,
                                        x.GeneralAccrual,
                                        x.Collection
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                                .Select(z => new InfoAboutPayItem
                                                                {
                                                                    Id = z.Id,
                                                                    Name = z.Name.ToStr(),
                                                                    TypeGroupServiceDi = z.TypeGroupServiceDi.GetEnumMeta().Display,
                                                                    Provider = z.ProviderName.ToStr(),
                                                                    CounterValuePeriodStart = z.CounterValuePeriodStart.HasValue ? z.CounterValuePeriodStart.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    CounterValuePeriodEnd = z.CounterValuePeriodEnd.HasValue ? z.CounterValuePeriodEnd.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    GeneralAccrual = z.GeneralAccrual.HasValue ? z.GeneralAccrual.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    Collection = z.Collection.HasValue ? z.Collection.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    Accrual = null,
                                                                    Debt = null,
                                                                    Payed = null
                                                                })
                                                                .ToList());

            var infoAboutPaymentCommunal = Container.Resolve<IDomainService<InfoAboutPaymentCommunal>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                        x.Id,
                                        x.BaseService.TemplateService.Name,
                                        x.BaseService.TemplateService.TypeGroupServiceDi,
                                        ProviderName = x.BaseService.Provider.Name,
                                        x.CounterValuePeriodStart,
                                        x.CounterValuePeriodEnd,
                                        x.Accrual,
                                        x.Payed,
                                        x.Debt
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                                .Select(z => new InfoAboutPayItem
                                                                {
                                                                    Id = z.Id,
                                                                    Name = z.Name.ToStr(),
                                                                    TypeGroupServiceDi = z.TypeGroupServiceDi.GetEnumMeta().Display,
                                                                    Provider = z.ProviderName.ToStr(),
                                                                    CounterValuePeriodStart = z.CounterValuePeriodStart.HasValue ? z.CounterValuePeriodStart.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    CounterValuePeriodEnd = z.CounterValuePeriodEnd.HasValue ? z.CounterValuePeriodEnd.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    Accrual = z.Accrual.HasValue ? z.Accrual.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    Debt = z.Debt.HasValue ? z.Debt.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    Payed = z.Payed.HasValue ? z.Payed.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                    GeneralAccrual = null,
                                                                    Collection = null
                                                                })
                                                                .ToList());

            // Сведения об использовании мест общего пользования
            var infoAboutCommonFacil = Container.Resolve<IDomainService<InfoAboutUseCommonFacilities>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                             {
                                                 PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                                 x.Id,
                                                 x.KindCommomFacilities,
                                                 x.Lessee,
                                                 x.DateStart,
                                                 x.DateEnd,
                                                 x.CostContract,
                                                 x.Number,
                                                 x.From,
                                                 x.TypeContract
                                             })
                                .AsEnumerable();

            // Сведения об исп нежилых помещений
            var nonResidentialPlacement = Container.Resolve<IDomainService<NonResidentialPlacement>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                        x.Id,
                                        x.ContragentName,
                                        x.TypeContragentDi,
                                        x.Area,
                                        x.DocumentDateApartment,
                                        x.DocumentDateCommunal,
                                        x.DocumentNumApartment,
                                        x.DocumentNumCommunal,
                                        x.DateStart,
                                        x.DateEnd
                                    })
                                .AsEnumerable();

            // Приборы учета нежилых помещений
            var nonResidentialPlaceMeteringDevice = Container.Resolve<IDomainService<NonResidentialPlacementMeteringDevice>>()
                                .GetAll()
                                .Where(x => x.NonResidentialPlacement.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => new
                                    {
                                        NonResidentialPlacementId = x.NonResidentialPlacement.Id,
                                        x.MeteringDevice.Name,
                                        x.MeteringDevice.AccuracyClass,
                                        x.MeteringDevice.TypeAccounting
                                    })
                                .AsEnumerable()
                                .GroupBy(x => x.NonResidentialPlacementId)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                                .Select(z => new NonResidentialPlaceMetering
                                                                {
                                                                    AccuracyClass = z.AccuracyClass,
                                                                    Name = z.Name.ToStr(),
                                                                    TypeAccounting = z.TypeAccounting.GetEnumMeta().Display
                                                                })
                                                                .ToList());
                                                                                

            // раскрытие по дому
            var disclosureInfoRealObj = disclosureInfoRealObjService.GetAll().FirstOrDefault(x => x.RealityObject.Id == idHouse);

            // Тарифы РСО
            var tariffRso = Container.Resolve<IDomainService<TariffForRso>>()
                                .GetAll()
                                .Where(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(x => new
                                                 {
                                                     BaseServiceId = x.BaseService.Id,
                                                     x.Cost,
                                                     x.NumberNormativeLegalAct,
                                                     x.DateNormativeLegalAct,
                                                     x.OrganizationSetTariff,
                                                     x.DateStart
                                                 })
                                .AsEnumerable()
                                .GroupBy(x => x.BaseServiceId)
                                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateStart).Select(z => new TariffForRsoItem
                                                                                                                     {
                                                                                                                         Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                                                                         DateNormativeLegalAct = z.DateNormativeLegalAct.HasValue && z.DateNormativeLegalAct.Value.Date != DateTime.MinValue.Date ? z.DateNormativeLegalAct.Value.ToShortDateString() : string.Empty,
                                                                                                                         DateStart = z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date ? z.DateStart.Value.ToShortDateString() : string.Empty,
                                                                                                                         NumberNormativeLegalAct = z.NumberNormativeLegalAct,
                                                                                                                         OrganizationSetTariff = z.OrganizationSetTariff
                                                                                                                     })
                                                                                                    .ToArray());

            // Тарифы потребителей
            var tariffConsumers = Container.Resolve<IDomainService<TariffForConsumers>>()
                                .GetAll()
                                .Where(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(x => new
                                {
                                    BaseServiceId = x.BaseService.Id,
                                    x.DateStart,
                                    x.TariffIsSetFor,
                                    x.OrganizationSetTariff,
                                    x.TypeOrganSetTariffDi,
                                    x.Cost
                                })
                                .AsEnumerable()
                                .GroupBy(x => x.BaseServiceId)
                                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateStart).Select(z => new TariffForConsumersItem
                                                                                                                    {
                                                                                                                        Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                                                                        TariffIsSetFor = z.TariffIsSetFor.GetEnumMeta().Display,
                                                                                                                        DateStart = z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date ? z.DateStart.Value.ToShortDateString() : string.Empty,
                                                                                                                        TypeOrganSetTariffDi = z.TypeOrganSetTariffDi.GetEnumMeta().Display,
                                                                                                                        OrganizationSetTariff = z.OrganizationSetTariff
                                                                                                                    })
                                                                                                    .ToArray());

            // Услуги
            var communalService = Container.Resolve<IDomainService<CommunalService>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x => 
                                        new
                                            {
                                                Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                                x.Id,
                                                x.TemplateService.Name,
                                                UnitMeasure = x.UnitMeasure.Name,
                                                ProviderName = x.Provider.Name,
                                                x.TemplateService.Code,
                                                x.TariffIsSetForDi,
                                                x.TemplateService.KindServiceDi,
                                                Price = x.TariffIsSetForDi,
                                                x.VolumePurchasedResources,
                                                x.TariffForConsumers

                                            })
                                .AsEnumerable()
                                .GroupBy(x => x.Periodid)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                    .Select(z => new ServiceItem
                                                    {
                                                        Id = z.Id,
                                                        Name = z.Name.ToStr(),
                                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                                        Provider = z.ProviderName.ToStr(),
                                                        Code = z.Code.ToStr(),
                                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                                        VolumePurchasedResources = z.VolumePurchasedResources.HasValue ? z.VolumePurchasedResources.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                        IsExecutor = string.IsNullOrEmpty(z.ProviderName) ? "Да" : "Нет",
                                                        Periodicity = null,
                                                        Price = null,
                                                        TypeProvision = null,
                                                        TypeProvisionUoTsjJsk = null,
                                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                                    })
                                                    .ToList());

            var housingService = Container.Resolve<IDomainService<HousingService>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                    .Select(
                        x =>
                            new
                            {
                                Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.TemplateService.Name,
                                UnitMeasure = x.UnitMeasure.Name,
                                PeriodicityName = x.Periodicity.Name,
                                ProviderName = x.Provider.Name,
                                x.TemplateService.Code,
                                x.TariffForConsumers,
                                x.TemplateService.KindServiceDi,
                                x.TypeOfProvisionService
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.Periodid)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                    .Select(z => new ServiceItem
                                                                    {
                                                                        Id = z.Id,
                                                                        Name = z.Name.ToStr(),
                                                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                                                        Periodicity = z.PeriodicityName.ToStr(),
                                                                        Provider = z.ProviderName.ToStr(),
                                                                        Code = z.Code.ToStr(),
                                                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                                                        TypeProvision = z.TypeOfProvisionService.GetEnumMeta().Display,
                                                                        TypeProvisionUoTsjJsk = z.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo ? " Да" : "Нет",
                                                                        IsExecutor = null,
                                                                        Price = null,
                                                                        VolumePurchasedResources = null,
                                                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                                                    })
                                                    .ToList());

            var repairService = Container.Resolve<IDomainService<RepairService>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x =>
                                        new
                                        {
                                            Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                            x.Id,
                                            x.TemplateService.Name,
                                            UnitMeasure = x.UnitMeasure.Name,
                                            ProviderName = x.Provider.Name,
                                            x.TemplateService.Code,
                                            x.TariffForConsumers,
                                            x.TemplateService.KindServiceDi,
                                            x.TypeOfProvisionService
                                        })
                                .AsEnumerable()
                                .GroupBy(x => x.Periodid)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                                    .Select(z => new ServiceItem
                                                    {
                                                        Id = z.Id,
                                                        Name = z.Name.ToStr(),
                                                        UnitMeasure = z.UnitMeasure.ToStr(),
                                                        Provider = z.ProviderName.ToStr(),
                                                        Code = z.Code.ToStr(),
                                                        KindService = z.KindServiceDi.GetEnumMeta().Display,
                                                        TypeProvision = z.TypeOfProvisionService.GetEnumMeta().Display,
                                                        TypeProvisionUoTsjJsk = z.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo ? " Да" : "Нет",
                                                        IsExecutor = null,
                                                        Periodicity = null,
                                                        Price = null,
                                                        VolumePurchasedResources = null,
                                                        TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                                        TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                                    })
                                                    .ToList());

            var capRepairService = Container.Resolve<IDomainService<CapRepairService>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                    .Select(
                        x =>
                            new
                            {
                                Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.TemplateService.Name,
                                UnitMeasure = x.UnitMeasure.Name,
                                ProviderName = x.Provider.Name,
                                x.TemplateService.Code,
                                x.TariffForConsumers,
                                x.TemplateService.KindServiceDi,
                                x.TypeOfProvisionService
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.Periodid)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                        .Select(z => new ServiceItem
                                        {
                                            Id = z.Id,
                                            Name = z.Name.ToStr(),
                                            UnitMeasure = z.UnitMeasure.ToStr(),
                                            Provider = z.ProviderName.ToStr(),
                                            Code = z.Code.ToStr(),
                                            KindService = z.KindServiceDi.GetEnumMeta().Display,
                                            TypeProvision = z.TypeOfProvisionService.GetEnumMeta().Display,
                                            TypeProvisionUoTsjJsk = z.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo ? " Да" : "Нет",
                                            IsExecutor = null,
                                            Periodicity = null,
                                            Price = null,
                                            VolumePurchasedResources = null,
                                            TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                            TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                        })
                                        .ToList());

            var additionalService = Container.Resolve<IDomainService<AdditionalService>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                    .Select(
                        x =>
                            new
                            {
                                Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                x.Id,
                                x.TemplateService.Name,
                                UnitMeasure = x.UnitMeasure.Name,
                                PeriodicityName = x.Periodicity.Name,
                                ProviderName = x.Provider.Name,
                                x.TemplateService.Code,
                                x.TariffForConsumers,
                                x.TemplateService.KindServiceDi
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.Periodid)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                        .Select(z => new ServiceItem
                                        {
                                            Id = z.Id,
                                            Name = z.Name.ToStr(),
                                            UnitMeasure = z.UnitMeasure.ToStr(),
                                            Provider = z.ProviderName.ToStr(),
                                            Code = z.Code.ToStr(),
                                            KindService = z.KindServiceDi.GetEnumMeta().Display,
                                            IsExecutor = null,
                                            Periodicity = null,
                                            Price = null,
                                            TypeProvision = null,
                                            TypeProvisionUoTsjJsk = null,
                                            VolumePurchasedResources = null,
                                            TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                            TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                        })
                                        .ToList());

            var controlService = Container.Resolve<IDomainService<ControlService>>()
                        .GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                        .Select(
                            x =>
                                new
                                {
                                    Periodid = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                    x.Id,
                                    x.TemplateService.Name,
                                    UnitMeasure = x.UnitMeasure.Name,
                                    ProviderName = x.Provider.Name,
                                    x.TemplateService.Code,
                                    x.TariffIsSetForDi,
                                    x.TariffForConsumers,
                                    x.TemplateService.KindServiceDi
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Periodid)
                        .ToDictionary(x => x.Key, y => y.AsEnumerable()
                                            .Select(z => new ServiceItem
                                            {
                                                Id = z.Id,
                                                Name = z.Name.ToStr(),
                                                UnitMeasure = z.UnitMeasure.ToStr(),
                                                Provider = z.ProviderName.ToStr(),
                                                Code = z.Code.ToStr(),
                                                KindService = z.KindServiceDi.GetEnumMeta().Display,
                                                VolumePurchasedResources = null,
                                                Price = null,
                                                Periodicity = null,
                                                IsExecutor = null,
                                                TypeProvision = null,
                                                TypeProvisionUoTsjJsk = null,
                                                TariffsForRso = tariffRso.ContainsKey(z.Id) ? tariffRso[z.Id] : null,
                                                TariffsForConsumers = tariffConsumers.ContainsKey(z.Id) ? tariffConsumers[z.Id] : null
                                            })
                                            .ToList());

            // Договора дома с ук
            var contractManOrg = Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                                .GetAll()
                                .Where(x => x.RealityObject.Id == idHouse)
                                .Select(x => new
                                                 {
                                                     ManOrgId = x.ManOrgContract.ManagingOrganization.Id,
                                                     x.ManOrgContract.StartDate,
                                                     x.ManOrgContract.EndDate,
                                                     x.ManOrgContract.TypeContractManOrgRealObj
                                                 })
                                .AsEnumerable()
                                .GroupBy(x => x.ManOrgId)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());
                                
            // Управляющие организации дома
            var mangingOrgs = Container.Resolve<IDomainService<DisclosureInfoRelation>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == idHouse)
                                .Select(
                                    x =>
                                        new 
                                        {
                                            PeriodId = x.DisclosureInfoRealityObj.PeriodDi.Id,
                                            PeriodDateStart = x.DisclosureInfoRealityObj.PeriodDi.DateStart,
                                            PeriodDateEnd = x.DisclosureInfoRealityObj.PeriodDi.DateEnd,
                                            x.DisclosureInfo.ManagingOrganization.Id,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.Name,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.JuridicalAddress,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.Ogrn,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.DateRegistration,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.OgrnRegistration,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.MailingAddress,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.Phone,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.Email,
                                            x.DisclosureInfo.ManagingOrganization.Contragent.OfficialWebsite,
                                            x.DisclosureInfo.ManagingOrganization.TypeManagement,
                                            x.DisclosureInfoRealityObj.RealityObject.AreaMkd
                                        })
                                .AsEnumerable()
                                .GroupBy(x => x.PeriodId)
                                .ToDictionary(x => x.Key, y => y.AsEnumerable());

            // Периоды раскрытия инф-ии
            var periods = disclosureInfoRealObjService
                            .GetAll()
                            .Where(x => x.RealityObject.Id == idHouse)
                            .Select(x => x.PeriodDi)
                            .AsEnumerable()
                            .Select(
                                x =>
                                new PeriodItem
                                    {
                                        Id = x.Id,
                                        Name = x.Name.ToStr(),
                                        Code = x.Id,
                                        DateStart = x.DateStart.HasValue && x.DateStart.Value.Date != DateTime.MinValue.Date ? x.DateStart.Value.ToShortDateString() : string.Empty,
                                        DateEnd = x.DateEnd.HasValue && x.DateEnd.Value.Date != DateTime.MinValue.Date ? x.DateEnd.Value.ToShortDateString() : string.Empty,
                                        ManagingOrgs = mangingOrgs.ContainsKey(x.Id) 
                                        ? mangingOrgs[x.Id].Select(y => new ManagingOrgItem
                                        {
                                            Id = y.Id,
                                            Name = y.Name.ToStr(),
                                            JuridicalAddress = y.JuridicalAddress.ToStr(),
                                            Ogrn = y.Ogrn.ToStr(),
                                            YearRegistration = y.DateRegistration.HasValue && y.DateRegistration.Value.Date != DateTime.MinValue.Date ? y.DateRegistration.Value.Year.ToStr() : string.Empty,
                                            OgrnRegistration = y.OgrnRegistration.ToStr(),
                                            PostAddress = y.MailingAddress.ToStr(),
                                            Phone = y.Phone.ToStr(),
                                            Email = y.Email.ToStr(),
                                            Suite = y.OfficialWebsite.ToStr(),
                                            MkdArea = y.AreaMkd.HasValue ? y.AreaMkd.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            ManagingType = y.TypeManagement.GetEnumMeta().Display,
                                            ContractStart = contractManOrg.ContainsKey(y.Id) && contractManOrg[y.Id].StartDate.HasValue && contractManOrg[y.Id].StartDate.Value.Date != DateTime.MinValue.Date ? contractManOrg[y.Id].StartDate.GetValueOrDefault().ToShortDateString() : string.Empty,
                                            ContractEnd = contractManOrg.ContainsKey(y.Id) && contractManOrg[y.Id].EndDate.HasValue && contractManOrg[y.Id].EndDate.Value.Date != DateTime.MinValue.Date ? contractManOrg[y.Id].EndDate.GetValueOrDefault().ToShortDateString() : string.Empty,
                                            ContractType = contractManOrg.ContainsKey(y.Id) ? contractManOrg[y.Id].TypeContractManOrgRealObj.GetEnumMeta().Display : string.Empty,
                                            DataByRealityObject = new DataByRealityObject
                                            {
                                                Services = MergeServices(y.PeriodId, new List<Dictionary<long, List<ServiceItem>>>
                                                {
                                                    communalService,
                                                    housingService,
                                                    additionalService,
                                                    controlService,
                                                    repairService,
                                                    capRepairService
                                                }),
                                                NonResidentialPlace = new NonResidentialPlace
                                                {
                                                NonResidentialPlacement = disclosureInfoRealObj != null ? disclosureInfoRealObj.NonResidentialPlacement == YesNoNotSet.Yes ? "Нет" : "Да" : "Не задано",
                                                NonResidentialPlaceItem = nonResidentialPlacement
                                                                            .Where(z =>
                                                                                (((x.DateStart.HasValue && y.PeriodDateStart.HasValue && (x.DateStart.Value >= y.PeriodDateStart.Value) || !y.PeriodDateStart.HasValue)
                                                                                    && (y.PeriodDateStart.HasValue && x.DateStart.HasValue && (y.PeriodDateStart.Value >= x.DateStart.Value) || !y.PeriodDateStart.HasValue))
                                                                                    || ((x.DateStart.HasValue && y.PeriodDateStart.HasValue && (y.PeriodDateStart.Value >= x.DateStart.Value) || !x.DateStart.HasValue)
                                                                                        && (x.DateEnd.HasValue && y.PeriodDateStart.HasValue
                                                                                            && (x.DateEnd.Value >= y.PeriodDateStart.Value) || !x.DateEnd.HasValue))))
                                                                            .Select(z => new NonResidentialPlaceItem
                                                                            {
                                                                                ContragentName = z.ContragentName,
                                                                                Area = z.Area.HasValue ? z.Area.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                                TypeContragentDi = z.TypeContragentDi.GetEnumMeta().Display.ToStr(),
                                                                                DocumentNumApartment = z.DocumentNumApartment,
                                                                                DocumentNumCommunal = z.DocumentNumCommunal,
                                                                                DateStart = z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date ? z.DateStart.Value.ToShortDateString() : string.Empty,
                                                                                DateEnd = z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date ? z.DateEnd.Value.ToShortDateString() : string.Empty,
                                                                                DocumentDateCommunal = z.DocumentDateCommunal.HasValue && z.DocumentDateCommunal.Value.Date != DateTime.MinValue.Date ? z.DocumentDateCommunal.Value.ToShortDateString() : string.Empty,
                                                                                DocumentDateApartment = z.DocumentDateApartment.HasValue && z.DocumentDateApartment.Value.Date != DateTime.MinValue.Date ? z.DocumentDateApartment.Value.ToShortDateString() : string.Empty,
                                                                                NonResidentialPlaceMetering = nonResidentialPlaceMeteringDevice.ContainsKey(z.Id) ? nonResidentialPlaceMeteringDevice[z.Id].ToArray() : null
                                                                            })
                                                                            .ToArray() 
                                                },
                                                UseCommonFacil = new UseCommonFacil
                                                {
                                                    PlaceGeneralUse = disclosureInfoRealObj != null ? disclosureInfoRealObj.PlaceGeneralUse == YesNoNotSet.Yes ? "Нет" : "Да" : "Не задано", 
                                                    InfoAboutUseCommonFacil = infoAboutCommonFacil
                                                                                .Where(z =>
                                                                                    (((y.PeriodDateStart.HasValue && z.DateStart >= y.PeriodDateStart.Value || !y.PeriodDateStart.HasValue)
                                                                                        && (y.PeriodDateEnd.HasValue && y.PeriodDateEnd.Value >= z.DateStart || !y.PeriodDateEnd.HasValue))
                                                                                        || ((y.PeriodDateStart.HasValue && y.PeriodDateStart.Value >= z.DateStart) && (y.PeriodDateStart.HasValue && z.DateEnd >= y.PeriodDateStart.Value || z.DateEnd <= DateTime.MinValue))))

                                                                                .Select(z => new InfoAboutUseCommonFacilItem
                                                                                {
                                                                                    Id = z.Id,
                                                                                    KindCommomFacilities = z.KindCommomFacilities.ToStr(),
                                                                                    Lessee = z.Lessee.ToStr(),
                                                                                    DateStart = z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date ? z.DateStart.Value.ToShortDateString() : string.Empty,
                                                                                    DateEnd = z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date ? z.DateEnd.Value.ToShortDateString() : string.Empty,
                                                                                    CostContract = z.CostContract.HasValue ? z.CostContract.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                                    Number = z.Number.ToStr(),
                                                                                    From = z.From.HasValue && z.From.Value.Date != DateTime.MinValue.Date ? z.From.Value.ToShortDateString() : string.Empty,
                                                                                    TypeContract = z.TypeContract.GetEnumMeta().Display.ToStr()
                                                                                })
                                                                                .ToArray() 
                                                },
                                                InfoAboutPay = MergeServices(y.PeriodId, new List<Dictionary<long, List<InfoAboutPayItem>>>
                                                {
                                                    infoAboutPaymentCommunal,
                                                    infoAboutPaymentHousing
                                                }),
                                                OtherService = otherService.ContainsKey(y.PeriodId) ? otherService[y.PeriodId].ToArray() : null,
                                                OrderAndConditionService = new OrderAndConditionService
                                                {
                                                    Act = documents.ContainsKey(y.PeriodId) && documents[y.PeriodId].FileActState != null
                                                    ? new DocumentRealObj
                                                    {
                                                        Id = documents[y.PeriodId].Id,
                                                        FileId = documents[y.PeriodId].FileActState.Id,
                                                        FileName = documents[y.PeriodId].FileActState.Name.ToStr()
                                                    }
                                                    : null,
                                                    WorksList = documents.ContainsKey(y.PeriodId) && documents[y.PeriodId].FileCatalogRepair != null
                                                    ? new DocumentRealObj
                                                    {
                                                        Id = documents[y.PeriodId].Id,
                                                        FileId = documents[y.PeriodId].FileCatalogRepair.Id,
                                                        FileName = documents[y.PeriodId].FileCatalogRepair.Name.ToStr()
                                                    }
                                                    : null,
                                                    Documents = new GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo.Documents
                                                    {
                                                        DocumentReportOfPlan = documents.ContainsKey(y.PeriodId) && documents[y.PeriodId].FileReportPlanRepair != null
                                                        ? new DocumentRealObj
                                                        {
                                                            Id = documents[y.PeriodId].Id,
                                                            FileId = documents[y.PeriodId].FileReportPlanRepair.Id,
                                                            FileName = documents[y.PeriodId].FileReportPlanRepair.Name.ToStr()
                                                        }
                                                        : null,
                                                        DocumentCurrentYear = protocols.ContainsKey(currentYear) && protocols[currentYear].File != null
                                                        ? new DocumentRealObj
                                                        {
                                                            Id = protocols[currentYear].Id,
                                                            FileId = protocols[currentYear].File.Id,
                                                            FileName = protocols[currentYear].File.Name.ToStr()
                                                        }
                                                        : null,
                                                        DocumentPrevYear = protocols.ContainsKey(currentYear - 1) && protocols[currentYear - 1].File != null
                                                        ? new DocumentRealObj
                                                        {
                                                            Id = protocols[currentYear - 1].Id,
                                                            FileId = protocols[currentYear - 1].File.Id,
                                                            FileName = protocols[currentYear - 1].File.Name.ToStr()
                                                        }
                                                        : null
                                                    },
                                                    PlanReductionExp = planReductionExpenseWorks.ContainsKey(y.PeriodId) ? planReductionExpenseWorks[y.PeriodId] : null,
                                                    PlanWorkServiceRepWorks = planWorkServiceRepWorksItem.ContainsKey(y.PeriodId) ? planWorkServiceRepWorksItem[y.PeriodId] : null,
                                                    ReductPayment = new ReductPayment
                                                    {
                                                        IsReductionPayment = disclosureInfoRealObj != null ? disclosureInfoRealObj.ReductionPayment.GetEnumMeta().Display : "Другое",
                                                        InfoAboutReductPayment = infoAboutReductionPayment.ContainsKey(y.PeriodId) ? infoAboutReductionPayment[y.PeriodId] : null
                                                    },
                                                    InformationOnContracts = new InformationOnContracts
                                                    {
                                                        Count = informationOnContracts.Count(z => ((z.DateStart.HasValue && z.DateStart >= y.PeriodDateStart.Value || !z.DateStart.HasValue)
                                                                            && (z.DateStart.HasValue && y.PeriodDateEnd.Value >= z.DateStart))
                                                                            || ((z.DateStart.HasValue && y.PeriodDateStart.Value >= z.DateStart || !z.DateStart.HasValue)
                                                                            && (z.DateEnd.HasValue && z.DateEnd >= y.PeriodDateStart.Value || !z.DateEnd.HasValue || z.DateEnd <= DateTime.MinValue))),
                                                        InformationOnCont = informationOnContracts
                                                                    .Where(z => ((z.DateStart.HasValue && z.DateStart >= y.PeriodDateStart.Value || !z.DateStart.HasValue)
                                                                            && (z.DateStart.HasValue && y.PeriodDateEnd.Value >= z.DateStart))
                                                                            || ((z.DateStart.HasValue && y.PeriodDateStart.Value >= z.DateStart || !z.DateStart.HasValue)
                                                                            && (z.DateEnd.HasValue && z.DateEnd >= y.PeriodDateStart.Value || !z.DateEnd.HasValue || z.DateEnd <= DateTime.MinValue)))
                                                                    .Select(
                                                                        z => new InformationOnContItem
                                                                        {
                                                                            Name = z.Name.ToStr(),
                                                                            Cost = z.Cost.HasValue ? z.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                                                            Number = z.Number.ToStr(),
                                                                            PartiesContract = z.PartiesContract.ToStr(),
                                                                            Comments = z.Comments.ToStr(),
                                                                            From = z.From.HasValue && z.From.Value.Date != DateTime.MinValue.Date ? z.From.Value.ToShortDateString() : string.Empty,
                                                                            DateStart = z.DateStart.HasValue && z.DateStart.Value.Date != DateTime.MinValue.Date ? z.DateStart.Value.ToShortDateString() : string.Empty,
                                                                            DateEnd = z.DateEnd.HasValue && z.DateEnd.Value.Date != DateTime.MinValue.Date ? z.DateEnd.Value.ToShortDateString() : string.Empty
                                                                        })
                                                                    .ToArray()
                                                    }
                                                }
                                            }
                                        })
                                        .ToArray()
                                        : null
                                    })
                            .ToArray();

            var realityObject = Container.Resolve<IDomainService<RealityObject>>().GetAll().FirstOrDefault(x => x.Id == idHouse);

            var realityObjectItem = new RealityObjectItem
                                        {
                                            Periods = periods,
                                            Municipality = realityObject.Municipality.Name.ToStr(),
                                            Address = realityObject.FiasAddress.AddressName.ToStr(),
                                            AddressKladr = realityObject.GkhCode.ToStr(),
                                            ExpluatationYear = realityObject.DateCommissioning.HasValue ? realityObject.DateCommissioning.Value.Year.ToStr() : string.Empty,
                                            Floor = realityObject.Floors.GetValueOrDefault(),
                                            ApartamentCount = realityObject.NumberApartments.GetValueOrDefault(),
                                            LivingPeople = realityObject.NumberLiving.GetValueOrDefault(),
                                            GeneralArea = realityObject.AreaLiving.HasValue ? realityObject.AreaLiving.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            Deterioration = realityObject.PhysicalWear.HasValue ? realityObject.PhysicalWear.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            YearCapRepair = realityObject.DateLastOverhaul.HasValue ? realityObject.DateLastOverhaul.Value.Year.ToStr() : string.Empty,
                                            SeriaHouse = realityObject.SeriesHome.ToStr(),
                                            Fasad = realityObject.HavingBasement.GetEnumMeta().Display,
                                            GatesCount = realityObject.NumberEntrances.GetValueOrDefault(),
                                            LivingNotLivingArea = realityObject.AreaLivingNotLivingMkd.HasValue ? realityObject.AreaLivingNotLivingMkd.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            LivingArea = realityObject.AreaLiving.HasValue ? realityObject.AreaLiving.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            LivingAreaPeople = realityObject.AreaLivingOwned.HasValue ? realityObject.AreaLivingOwned.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            LiftCount = realityObject.NumberLifts.GetValueOrDefault(),
                                            FasadArea = realityObject.AreaBasement.HasValue ? realityObject.AreaBasement.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                            RoofType = realityObject.TypeRoof.GetEnumMeta().Display,
                                            RoofMaterial = realityObject.RoofingMaterial != null ? realityObject.RoofingMaterial.Name.ToStr() : string.Empty,
                                            WallMaterial = realityObject.WallMaterial != null ? realityObject.WallMaterial.Name.ToStr() : string.Empty
                                        };

            var result = periods.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new GetManOrgRealtyObjectInfoResponse { Result = result, RealityObjectItem = realityObjectItem };
        }

        private T[] MergeServices<T>(long periodId, IEnumerable<Dictionary<long, List<T>>> serviceDict)
        {
            var list = new List<T>();

            foreach (var serviceDictitem in serviceDict)
            {
                if (serviceDictitem.ContainsKey(periodId))
                {
                    list.AddRange(serviceDictitem[periodId]);
                }                
            }

            return list.ToArray();
        }
    }
}