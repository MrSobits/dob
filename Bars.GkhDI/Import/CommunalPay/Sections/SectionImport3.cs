namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class SectionImport3 : ISectionImport
    {
        #region Properties

        public string Name
        {
            get { return "Импорт из комплат секция #3"; }
        }

        public IWindsorContainer Container { get; set; }

        public IRepository<OtherService> DomainServiceOther { get; set; }

        public IRepository<CommunalService> DomainServiceCommunal { get; set; }

        public IRepository<HousingService> DomainServiceHousing { get; set; }

        public IRepository<RepairService> DomainServiceRepair { get; set; }

        public IRepository<CapRepairService> DomainServiceCapRepair { get; set; }

        public IRepository<ControlService> DomainServiceControl { get; set; }

        public IRepository<AdditionalService> DomainServiceAdditional { get; set; }

        public IRepository<TariffForConsumers> DomainServiceTariff { get; set; }

        public IRepository<RealityObject> DomainServiceRobject { get; set; }

        public IRepository<PeriodDi> DomainServicePeriod { get; set; }

        public IRepository<DisclosureInfoRealityObj> DomainServiceDiRo { get; set; }

        public IRepository<ProviderService> DomainServiceProvider { get; set; }

        public IRepository<Contragent> ContragentRepository { get; set; }

        #endregion

        public void ImportSection(ImportParams importParams)
        {
            var resultOther = new List<OtherService>();
            var resultCommunal = new List<CommunalService>();
            var resultTariffConsummers = new List<TariffForConsumers>();
            var resultHousing = new List<HousingService>();
            var resultRepair = new List<RepairService>();
            var resultCapRepair = new List<CapRepairService>();
            var resultAdditional = new List<AdditionalService>();
            var resultControl = new List<ControlService>();
            var listActiveProvider = new List<ProviderService>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;
            var realityObjectDict = importParams.RealObjsImportInfo;

            if (sectionsData.Section3.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds.ToDictionary(x => x);

            var tmpImportParams = importParams;
            var otherServiceDict = DomainServiceOther.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));                

            var communalServiceDict = DomainServiceCommunal.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));

            var housingServiceDict = DomainServiceHousing.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));

            
            var codeErcList = sectionsData.Section3.Select(x => x.CodeErc).Distinct().ToList();
            var realityObjectIdList = realityObjectDict
                .Where(x => codeErcList.Contains(x.Key))
                .Select(x => x.Value.Id)
                .ToList();
            var diRoDict = DomainServiceDiRo.GetAll()
                .Where(x =>
                    x.PeriodDi.Id == importParams.PeriodDiId
                    &&
                    realityObjectIdList.Contains(x.RealityObject.Id)
                )
                .AsEnumerable()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.First());

            var disclosureInfoRealityObjIdList = diRoDict.Values.Select(x => x.Id);
            var repairServiceDict = DomainServiceRepair.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .Where(x => disclosureInfoRealityObjIdList.Contains(x.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));

            var capRepairServiceDict = DomainServiceCapRepair.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));

            var controlServiceDict = DomainServiceControl.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .Where(x => disclosureInfoRealityObjIdList.Contains(x.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));

            var additionalServiceDict = DomainServiceAdditional.GetAll()
                .Where(x => x.DisclosureInfoRealityObj != null && x.DisclosureInfoRealityObj.PeriodDi != null)
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == tmpImportParams.PeriodDiId)
                .Where(x => disclosureInfoRealityObjIdList.Contains(x.DisclosureInfoRealityObj.Id))
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x));


            var tariffForConsumersServiceDict =
                DomainServiceTariff.GetAll()
                    .Where(x => x.BaseService != null)
                    .Where(x => x.BaseService.DisclosureInfoRealityObj != null)
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi != null)
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)

                    .Where(
                        x =>
                            DomainServiceCommunal.GetAll()
                                .Where(y => y.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                                .Select(y => y.Id)
                                .Contains(x.BaseService.Id)
                            ||

                            DomainServiceHousing.GetAll()
                                .Where(y => y.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                                .Select(y => y.Id)
                                .Contains(x.BaseService.Id))
                    .Select(x => new TariffForConsumersProxy
                    {
                        BaseServiceId = x.BaseService.Id,
                        Cost = x.Cost
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.BaseServiceId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x));
                                  

            var templateServiceDict = Container.Resolve<IDomainService<TemplateService>>().GetAll()
                .Where(x => x.Code != null)
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First());

            var contragentDict = Container.Resolve<IDomainService<Contragent>>().GetAll()
                .Where(x => x.Inn != null && x.Kpp != null)
                .Select(x => new
                {
                    x.Inn,
                    x.Kpp,
                    x.Id
                })
                .AsEnumerable()
                .GroupBy(x => string.Format("{0}_{1}", x.Inn, x.Kpp))
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());


            var providerDict =
                ContragentRepository.GetAll()
                    .Where(x => contragentDict.Values.Contains(x.Id))
                    .ToDictionary(x => x.Id);

            var serviceProviderDict = DomainServiceProvider.GetAll()
                .AsEnumerable()
                .GroupBy(x => string.Format("{0}_{1}", x.BaseService.Id, x.Provider.Id))
                .ToDictionary(x => x.Key, y => y.First());

            foreach (var section3Record in sectionsData.Section3)
            {
                // Получаем дом по коду ЕРЦ
                var realityObject = realityObjectDict.ContainsKey(section3Record.CodeErc) ? realityObjectDict[section3Record.CodeErc] : null;
                if (realityObject == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section3Record.CodeErc));
                    continue;
                }

                if (!realityObjects.ContainsKey(realityObject.Id))
                {
                    logImport.Warn(this.Name, string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде", section3Record.CodeErc, inn));
                    continue;
                }

                var diRo = GetDisclosureInfoRealityObj(importParams, diRoDict, realityObject);                

                // По коду получаем шаблонную услугу
                var templateService = templateServiceDict.ContainsKey(section3Record.Code) ? templateServiceDict[section3Record.Code] : null;
                if (templateService == null)
                {
                    // В справочнике шаблонных услуг нету данной услуги. Сажаем ее в прочие, имя для нее берем из секции #11
                    var serviceSection11 = sectionsData.Section11.FirstOrDefault(x => x.CodeService == section3Record.CodeCommunalPay);
                    if (serviceSection11 == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить услугу с кодом {0} в секции 11", section3Record.CodeCommunalPay));
                        continue;
                    }

                    var other = otherServiceDict.ContainsKey(diRo.Id) ? otherServiceDict[diRo.Id].FirstOrDefault(x => x.Code == section3Record.CodeCommunalPay) : null;
                    if (other == null)
                    {
                        logImport.Info(this.Name, string.Format("Добавлена запись с кодом {0} в раздел прочие услуги", section3Record.CodeCommunalPay));

                        other = new OtherService
                        {
                            DisclosureInfoRealityObj = diRo,
                            Code = section3Record.CodeCommunalPay
                        };
                    }

                    other.Name = serviceSection11.NameService;
                    other.Provider = section3Record.Provider;
                    other.Tariff = section3Record.Tariff;
                    other.UnitMeasure = section3Record.UnitMeasure;

                    if (other.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    resultOther.Add(other);
                }
                else
                {
                    var providerKey = string.Format("{0}_{1}", section3Record.ProviderInn, section3Record.ProviderKpp);

                    var providerId = contragentDict.ContainsKey(providerKey)
                                    ? contragentDict[providerKey]
                                    : 0;
                    
                    var provider = providerDict.ContainsKey(providerId)
                        ? providerDict[providerId]
                        : null;

                    // По виду шаблонной услуги создаем услугу
                    switch (templateService.KindServiceDi)
                    {
                        case KindServiceDi.Communal:
                            {
                                var service = communalServiceDict.ContainsKey(diRo.Id)
                                                   ? communalServiceDict[diRo.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                                                   : null;

                                if (service == null)
                                {
                                    logImport.Info(Name, string.Format("Добавлена запись с кодом {0} в раздел сведения об услугах", templateService.Name));

                                    service = new CommunalService
                                    {
                                        DisclosureInfoRealityObj = diRo,
                                        TemplateService = templateService,
                                        TypeOfProvisionService = TypeOfProvisionServiceDi.ServiceProvidedMo
                                    };
                                }

                                if (providerId > 0)
                                {
                                    var key = string.Format("{0}_{1}", service.Id, provider.Id);

                                    var providerService = serviceProviderDict.ContainsKey(key)
                                        ? serviceProviderDict[key]
                                        : null;

                                    if (providerService != null)
                                    {
                                        providerService.DateStartContract = section3Record.DogDate;
                                        providerService.NumberContract = section3Record.DogNum;
                                    }
                                    else
                                    {
                                        providerService = new ProviderService
                                        {
                                            BaseService = service,
                                            DateStartContract = section3Record.DogDate,
                                            Provider = this.ContragentRepository.Load(providerId),
                                            NumberContract = section3Record.DogNum
                                        };
                                    }

                                    listActiveProvider.Add(providerService);

                                    service.Provider = provider;
                                }

                                service.VolumePurchasedResources = section3Record.VolumeFull;

                                if (service.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                resultCommunal.Add(service);

                                // Для коммунальной услуги заполняем или добавляем тарифы
                                var tariffExist = tariffForConsumersServiceDict.ContainsKey(service.Id) &&
                                                  tariffForConsumersServiceDict[service.Id].Any(
                                                      x => x.Cost == section3Record.Tariff);

                                if (!tariffExist)
                                {
                                    logImport.Info(this.Name, string.Format("Добавлена запись тарифа для услуги {0} ", service.TemplateService.Name));

                                    var tariffForConsumers = new TariffForConsumers
                                    {
                                        BaseService = service,
                                        TariffIsSetFor = TariffIsSetForDi.ForOwnersResidentialAndNonResidentialPremises,
                                        TypeOrganSetTariffDi = TypeOrganSetTariffDi.Organ,
                                        Cost = section3Record.Tariff,
                                        DateStart = section3Record.TariffBegin
                                    };

                                    resultTariffConsummers.Add(tariffForConsumers);
                                }

                                break;
                            }

                        case KindServiceDi.Housing:
                            {
                                var service = housingServiceDict.ContainsKey(diRo.Id)
                                                   ? housingServiceDict[diRo.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                                                   : null;

                                if (service == null)
                                {
                                    logImport.Info(this.Name, string.Format("Добавлена запись с кодом {0} в раздел сведения об услугах", templateService.Name));

                                    service = new HousingService
                                    {
                                        DisclosureInfoRealityObj = diRo,
                                        TemplateService = templateService,
                                        TypeOfProvisionService = TypeOfProvisionServiceDi.ServiceProvidedMo 
                                    };
                                }

                                if (providerId > 0)
                                {
                                    var key = string.Format("{0}_{1}", service.Id, provider.Id);

                                    var providerService = serviceProviderDict.ContainsKey(key)
                                        ? serviceProviderDict[key]
                                        : null;

                                    if (providerService != null)
                                    {
                                        providerService.DateStartContract = section3Record.DogDate;
                                        providerService.NumberContract = section3Record.DogNum;
                                    }
                                    else
                                    {
                                        providerService = new ProviderService
                                        {
                                            BaseService = service,
                                            DateStartContract = section3Record.DogDate,
                                            Provider = this.ContragentRepository.Load(providerId),
                                            NumberContract = section3Record.DogNum
                                        };
                                    }

                                    listActiveProvider.Add(providerService);

                                    service.Provider = provider;
                                }

                                if (service.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                resultHousing.Add(service);

                                // Для жилищной услуги заполняем или добавляем тарифы
                                var tariffExist = tariffForConsumersServiceDict.ContainsKey(service.Id) &&
                                                  tariffForConsumersServiceDict[service.Id].Any(
                                                      x => x.Cost == section3Record.Tariff);

                                if (!tariffExist)
                                {
                                    logImport.Info(this.Name, string.Format("Добавлена запись тарифа для услуги {0} ", service.TemplateService.Name));

                                    var tariffForConsumers = new TariffForConsumers
                                    {
                                        BaseService = service,
                                        TariffIsSetFor = TariffIsSetForDi.ForOwnersResidentialAndNonResidentialPremises,
                                        TypeOrganSetTariffDi = TypeOrganSetTariffDi.Organ,
                                        Cost = section3Record.Tariff,
                                        DateStart = section3Record.TariffBegin
                                    };

                                    resultTariffConsummers.Add(tariffForConsumers);
                                }

                                break;
                            }

                        case KindServiceDi.Repair:
                            {
                                var service = repairServiceDict.ContainsKey(diRo.Id)
                                                   ? repairServiceDict[diRo.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                                                   : null;

                                if (service == null)
                                {
                                    logImport.Info(Name, string.Format("Добавлена запись с кодом {0} в раздел сведения об услугах", templateService.Name));

                                    service = new RepairService
                                    {
                                        DisclosureInfoRealityObj = diRo,
                                        TemplateService = templateService,
                                        TypeOfProvisionService = TypeOfProvisionServiceDi.ServiceProvidedMo
                                    };
                                }

                                if (providerId > 0)
                                {
                                    var key = string.Format("{0}_{1}", service.Id, provider.Id);

                                    var providerService = serviceProviderDict.ContainsKey(key)
                                        ? serviceProviderDict[key]
                                        : null;

                                    if (providerService != null)
                                    {
                                        providerService.DateStartContract = section3Record.DogDate;
                                        providerService.NumberContract = section3Record.DogNum;
                                    }
                                    else
                                    {
                                        providerService = new ProviderService
                                        {
                                            BaseService = service,
                                            DateStartContract = section3Record.DogDate,
                                            Provider = this.ContragentRepository.Load(providerId),
                                            NumberContract = section3Record.DogNum
                                        };
                                    }

                                    listActiveProvider.Add(providerService);

                                    service.Provider = provider;
                                }

                                if (provider != null)
                                {
                                    service.Provider = provider;
                                }

                                if (service.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                resultRepair.Add(service);

                                break;
                            }

                        case KindServiceDi.CapitalRepair:
                            {
                                var service = capRepairServiceDict.ContainsKey(diRo.Id)
                                                   ? capRepairServiceDict[diRo.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                                                   : null;

                                if (service == null)
                                {
                                    logImport.Info(this.Name, string.Format("Добавлена запись с кодом {0} в раздел сведения об услугах", templateService.Name));

                                    service = new CapRepairService
                                    {
                                        DisclosureInfoRealityObj = diRo,
                                        TemplateService = templateService,
                                        TypeOfProvisionService = TypeOfProvisionServiceDi.ServiceProvidedMo
                                    };
                                }

                                if (providerId > 0)
                                {
                                    var key = string.Format("{0}_{1}", service.Id, provider.Id);

                                    var providerService = serviceProviderDict.ContainsKey(key)
                                        ? serviceProviderDict[key]
                                        : null;

                                    if (providerService != null)
                                    {
                                        providerService.DateStartContract = section3Record.DogDate;
                                        providerService.NumberContract = section3Record.DogNum;
                                    }
                                    else
                                    {
                                        providerService = new ProviderService
                                        {
                                            BaseService = service,
                                            DateStartContract = section3Record.DogDate,
                                            Provider = this.ContragentRepository.Load(providerId),
                                            NumberContract = section3Record.DogNum
                                        };
                                    }

                                    listActiveProvider.Add(providerService);

                                    service.Provider = provider;
                                }

                                if (provider != null)
                                {
                                    service.Provider = provider;
                                }

                                if (service.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                resultCapRepair.Add(service);

                                break;
                            }

                        case KindServiceDi.Additional:
                            {
                                var service = additionalServiceDict.ContainsKey(diRo.Id)
                                                   ? additionalServiceDict[diRo.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                                                   : null;

                                if (service == null)
                                {
                                    logImport.Info(Name, string.Format("Добавлена запись с кодом {0} в раздел сведения об услугах", templateService.Name));

                                    service = new AdditionalService
                                    {
                                        DisclosureInfoRealityObj = diRo,
                                        TemplateService = templateService,
                                        DateStart = DateTime.Now,
                                        DateEnd = DateTime.Now,
                                        Total = 0m
                                    };
                                }

                                if (providerId > 0)
                                {
                                    var key = string.Format("{0}_{1}", service.Id, provider.Id);

                                    var providerService = serviceProviderDict.ContainsKey(key)
                                        ? serviceProviderDict[key]
                                        : null;

                                    if (providerService != null)
                                    {
                                        providerService.DateStartContract = section3Record.DogDate;
                                        providerService.NumberContract = section3Record.DogNum;
                                    }
                                    else
                                    {
                                        providerService = new ProviderService
                                        {
                                            BaseService = service,
                                            DateStartContract = section3Record.DogDate,
                                            Provider = this.ContragentRepository.Load(providerId),
                                            NumberContract = section3Record.DogNum
                                        };
                                    }

                                    listActiveProvider.Add(providerService);

                                    service.Provider = provider;
                                }

                                if (provider != null)
                                {
                                    service.Provider = provider;
                                }

                                if (service.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                resultAdditional.Add(service);

                                break;
                            }

                        case KindServiceDi.Managing:
                            {
                                var service = controlServiceDict.ContainsKey(diRo.Id)
                                                   ? controlServiceDict[diRo.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                                                   : null;

                                if (service == null)
                                {
                                    logImport.Info(Name, string.Format("Добавлена запись с кодом {0} в раздел сведения об услугах", templateService.Name));

                                    service = new ControlService
                                    {
                                        DisclosureInfoRealityObj = diRo,
                                        TemplateService = templateService
                                    };
                                }

                                if (providerId > 0)
                                {
                                    var key = string.Format("{0}_{1}", service.Id, provider.Id);

                                    var providerService = serviceProviderDict.ContainsKey(key)
                                        ? serviceProviderDict[key]
                                        : null;

                                    if (providerService != null)
                                    {
                                        providerService.DateStartContract = section3Record.DogDate;
                                        providerService.NumberContract = section3Record.DogNum;
                                    }
                                    else
                                    {
                                        providerService = new ProviderService
                                        {
                                            BaseService = service,
                                            DateStartContract = section3Record.DogDate,
                                            Provider = this.ContragentRepository.Load(providerId),
                                            NumberContract = section3Record.DogNum
                                        };
                                    }

                                    listActiveProvider.Add(providerService);

                                    service.Provider = provider;
                                }

                                if (provider != null)
                                {
                                    service.Provider = provider;
                                }

                                if (service.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                resultControl.Add(service);

                                break;
                            }
                    }
                }
            }
            
            if (resultOther.Count > 0)
                this.InTransaction(resultOther, DomainServiceOther);
            if (resultCommunal.Count > 0)
                this.InTransaction(resultCommunal, DomainServiceCommunal);
            if (resultHousing.Count > 0)
                this.InTransaction(resultHousing, DomainServiceHousing);
            if (resultRepair.Count > 0)
                this.InTransaction(resultRepair, DomainServiceRepair);
            if (resultCapRepair.Count > 0)
                this.InTransaction(resultCapRepair, DomainServiceCapRepair);
            if (resultAdditional.Count > 0)
                this.InTransaction(resultAdditional, DomainServiceAdditional);
            if (resultControl.Count > 0)
                this.InTransaction(resultControl, DomainServiceControl);
            if (resultTariffConsummers.Count > 0)
                this.InTransaction(resultTariffConsummers, DomainServiceTariff);
            if (listActiveProvider.Count > 0)
                this.InTransaction(listActiveProvider, DomainServiceProvider);

            resultOther.Clear();
            resultCommunal.Clear();
            resultTariffConsummers.Clear();
            resultHousing.Clear();
            resultRepair.Clear();
            resultCapRepair.Clear();
            resultAdditional.Clear();
            resultControl.Clear();

            otherServiceDict.Clear();
            communalServiceDict.Clear();
            housingServiceDict.Clear();
            repairServiceDict.Clear();
            capRepairServiceDict.Clear();
            controlServiceDict.Clear();
            additionalServiceDict.Clear();
            tariffForConsumersServiceDict.Clear();
            realityObjectIdList.Clear();
            diRoDict.Clear();
            templateServiceDict.Clear();
            contragentDict.Clear();
            providerDict.Clear();
            serviceProviderDict.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// // Получаем раскрытие в доме (или если нету создаем его)
        /// </summary>
        /// <param name="importParams"></param>
        /// <param name="diRoDict"></param>
        /// <param name="realityObject"></param>
        /// <returns></returns>
        private DisclosureInfoRealityObj GetDisclosureInfoRealityObj(            
            ImportParams importParams,
            Dictionary<long, DisclosureInfoRealityObj> diRoDict,                        
            RealObjImportInfo realityObject)
        {
            var disclosureInfoRealityObj = diRoDict.ContainsKey(realityObject.Id)
                                       ? diRoDict[realityObject.Id]
                                       : null;

            if (disclosureInfoRealityObj == null)
            {
                disclosureInfoRealityObj = new DisclosureInfoRealityObj
                {
                    PeriodDi = new PeriodDi { Id = importParams.PeriodDiId },
                    RealityObject = new RealityObject { Id = realityObject.Id }
                };
                DomainServiceDiRo.Save(disclosureInfoRealityObj);
                diRoDict.Add(realityObject.Id, disclosureInfoRealityObj);                
            }

            return disclosureInfoRealityObj;            
        }

        /// <summary>
        /// Транзакция
        /// </summary>
        /// <param name="list"></param>
        /// <param name="repos"></param>
        private void InTransaction(IEnumerable<PersistentObject> list, IRepository repos)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }

    /// <summary>
    /// Прокси класс Тарифы для потребителей
    /// </summary>
    internal class TariffForConsumersProxy
    {
        /// <summary>
        /// Базовая услуга
        /// </summary>
        public long BaseServiceId { get; set; }
        /// <summary>
        /// Стоимость тарифа
        /// </summary>
        public decimal? Cost { get; set; }
    }
}
