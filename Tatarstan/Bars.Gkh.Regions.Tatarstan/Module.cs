namespace Bars.Gkh.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Modules.ClaimWork.Controllers;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Extension;
    using Bars.Gkh.Regions.Tatarstan.Controller;
    using Bars.Gkh.Regions.Tatarstan.Controller.ChargeSplitting;
    using Bars.Gkh.Regions.Tatarstan.DocCreateRules;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.DomainService.Impl;
    using Bars.Gkh.Regions.Tatarstan.DomainService.States;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;
    using Bars.Gkh.Regions.Tatarstan.ExecutionAction;
    using Bars.Gkh.Regions.Tatarstan.Import;
    using Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.Regions.Tatarstan.Import.UtilityDebtor;
    using Bars.Gkh.Regions.Tatarstan.Interceptors;
    using Bars.Gkh.Regions.Tatarstan.Interceptors.ConstructionObject;
    using Bars.Gkh.Regions.Tatarstan.Interceptors.GasEquipmentOrg;
    using Bars.Gkh.Regions.Tatarstan.Interceptors.PubServContractPeriodSumm;
    using Bars.Gkh.Regions.Tatarstan.Interceptors.UtilityDebtor;
    using Bars.Gkh.Regions.Tatarstan.Navigation;
    using Bars.Gkh.Regions.Tatarstan.Reports;
    using Bars.Gkh.Regions.Tatarstan.ViewModel;
    using Bars.Gkh.Regions.Tatarstan.ViewModel.ContractService;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;

    using MainProxySelectorService = Bars.Gkh.FormatDataExport.ProxySelectors.Impl;

    using Castle.MicroKernel.Registration;

    using EmergencyObjMenuProvider = Bars.Gkh.Regions.Tatarstan.Navigation.EmergencyObjMenuProvider;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterPermissionMap<PermissionMap>();
            this.Container.RegisterResourceManifest<ResourceManifest>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.RegisterNavigations();
            this.RegisterDomainServices();
            this.RegisterInterceptors();
            this.RegisterViewModels();
            this.RegisterServices();
            this.RegisterControllers();
            this.RegisterExecuteActions();
            this.RegisterBundlers();
            this.RegisterExports();
            this.RegisterImports();
       
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhRT statefulentity");

            this.RegisterGhku();

            this.RegisterFormatDataExport();

            ApplicationContext.Current.Events.GetEvent<AppStartEvent>()
                .Subscribe(
                    _ =>
                    {
                        if (this.Container.GetGkhConfig<ClaimWorkConfig>().Enabled)
                        {
                            this.CreateStatesForPir();
                        }
                    });
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<RoleTypeHousePermissionController>();
            this.Container.RegisterController<ConstructionObjectController>();
            this.Container.RegisterFileStorageDataController<ConstructionObjectDocument>();
            this.Container.RegisterFileStorageDataController<ConstructionObjectPhoto>();
            this.Container.RegisterFileStorageDataController<ConstructionObjectContract>();
            this.Container.RegisterAltDataController<ConstructionObjectTypeWork>();
            this.Container.RegisterAltDataController<ConstructionObjectParticipant>();
            this.Container.RegisterController<ConstructObjMonitoringSmrController>();

            this.Container.RegisterAltDataController<GasEquipmentOrg>();
            this.Container.RegisterFileStorageDataController<GasEquipmentOrgRealityObj>();

            //Неплательщики ЖКУ
            this.Container.RegisterController<BaseClaimWorkController<UtilityDebtorClaimWork>>();
            this.Container.RegisterController<FileStorageDataController<ExecutoryProcessDocument>>();
            this.Container.RegisterController<ExecutoryProcessController>();
            this.Container.RegisterAltDataController<SeizureOfProperty>();
            this.Container.RegisterAltDataController<DepartureRestriction>();
            
            this.Container.RegisterController<ContractPeriodController>();
            this.Container.RegisterAltDataController<ContractPeriodSummDetail>();
            this.Container.RegisterAltDataController<PeriodNormConsumption>();
            this.Container.RegisterAltDataController<NormConsumption>();
            this.Container.RegisterController<NormConsumptionFiringController>();
            this.Container.RegisterController<NormConsumptionColdWaterController>();
            this.Container.RegisterController<NormConsumptionHeatingController>();
            this.Container.RegisterController<NormConsumptionHotWaterController>();
            this.Container.RegisterController<PubServContractPeriodSummController>();
            this.Container.RegisterController<BudgetOrgContractPeriodSummController>();
            this.Container.RegisterController<FuelEnergyOrgContractDetailController>();
            this.Container.RegisterAltDataController<FuelEnergyOrgContractInfo>();
            this.Container.RegisterAltDataController<ServiceOrgFuelEnergyResourcePeriodSumm>();
            this.Container.RegisterAltDataController<PlanPaymentsPercentage>();

            //ContractService
            this.Container.RegisterAltDataController<ManOrgContractService>();
            this.Container.RegisterAltDataController<ManOrgAddContractService>();
            this.Container.RegisterAltDataController<ManOrgAgrContractService>();
            this.Container.RegisterAltDataController<ManOrgComContractService>();
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterFileStorageDomainService<ConstructionObjectDocument>();
            this.Container.RegisterFileStorageDomainService<ConstructionObjectPhoto>();
            this.Container.RegisterFileStorageDomainService<ConstructionObjectContract>();
            this.Container.RegisterFileStorageDomainService<GasEquipmentOrgRealityObj>();

            //Неплательщики ЖКУ
            this.Container.RegisterDomainService<ExecutoryProcessDocument, FileStorageDomainService<ExecutoryProcessDocument>>();
            this.Container.RegisterDomainService<ExecutoryProcess, FileStorageDomainService<ExecutoryProcess>>();
            this.Container.RegisterDomainService<UtilityDebtorClaimWork, FileStorageDomainService<UtilityDebtorClaimWork>>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<RoleTypeHousePermission, RoleTypeHousePermissionViewModel>();
            this.Container.RegisterViewModel<ConstructionObject, ConstructionObjectViewModel>();
            this.Container.RegisterViewModel<ConstructionObjectDocument, ConstructionObjectDocumentViewModel>();
            this.Container.RegisterViewModel<ConstructionObjectPhoto, ConstructionObjectPhotoViewModel>();
            this.Container.RegisterViewModel<ConstructionObjectContract, ConstructionObjectContractViewModel>();
            this.Container.RegisterViewModel<ConstructionObjectParticipant, ConstructionObjectParticipantViewModel>();
            this.Container.RegisterViewModel<ConstructionObjectTypeWork, ConstructionObjectTypeWorkViewModel>();
            this.Container.RegisterViewModel<NormConsumption, NormConsumptionViewModel>();
            this.Container.RegisterViewModel<PubServContractPeriodSumm, PubServContractPeriodSummViewModel>();
            this.Container.RegisterViewModel<ContractPeriodSummDetail, ContractPeriodSummDetailViewModel>();
            this.Container.RegisterViewModel<BudgetOrgContractPeriodSumm, BudgetOrgContractPeriodSummViewModel>();
            this.Container.RegisterViewModel<FuelEnergyOrgContractDetail, FuelEnergyOrgContractDetailViewModel>();
            this.Container.RegisterViewModel<FuelEnergyOrgContractInfo, FuelEnergyOrgContractInfoViewModel>();
            this.Container.RegisterViewModel<ServiceOrgFuelEnergyResourcePeriodSumm, ServiceOrgFuelEnergyResourcePeriodSummViewModel>();
            this.Container.RegisterViewModel<PlanPaymentsPercentage, PlanPaymentsPercentageViewModel>();

            this.Container.RegisterViewModel<GasEquipmentOrg, GasEquipmentOrgViewModel>();
            this.Container.RegisterViewModel<GasEquipmentOrgRealityObj, GasEquipmentOrgRealityObjViewModel>();
            
            // Неплательщики ЖКУ
            this.Container.RegisterViewModel<UtilityDebtorClaimWork, UtilityDebtorClaimWorkViewModel>();
            this.Container.RegisterViewModel<ExecutoryProcessDocument, ExecutoryProcessDocumentViewModel>();

            //ContractService
            this.Container.RegisterViewModel<ManOrgAddContractService, ManOrgAddContractServiceViewModel>();
            this.Container.RegisterViewModel<ManOrgAgrContractService, ManOrgAgrContractServiceViewModel>();
            this.Container.RegisterViewModel<ManOrgComContractService, ManOrgComContractServiceViewModel>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<ConstructionObject, ConstructionObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<ConstructionObjectContract, ConstructionObjectContractInterceptor>();
            this.Container.RegisterDomainInterceptor<ConstructObjMonitoringSmr, ConstructObjMonitoringSmrInterceptor>();
            this.Container.RegisterDomainInterceptor<NormConsumption, NormConsumptionInterceptor>();
            this.Container.RegisterDomainInterceptor<ContractPeriodSummDetail, ContractPeriodSummDetailInterceptor>();
            this.Container.RegisterDomainInterceptor<PeriodNormConsumption, PeriodNormConsumptionInterceptor>();
            this.Container.RegisterDomainInterceptor<GasEquipmentOrg, GasEquipmentOrgInterceptor>();
            this.Container.RegisterDomainInterceptor<GasEquipmentOrgRealityObj, GasEquipmentOrgRealityObjInterceptor>();

            //Неплательщики ЖКУ
            this.Container.RegisterDomainInterceptor<UtilityDebtorClaimWork, UtilityDebtorClaimWorkInterceptor>();
            this.Container.RegisterDomainInterceptor<ExecutoryProcess, ExecutoryProcessInterceptor>();

            this.Container.RegisterDomainReadInterceptor<PubServContractPeriodSumm, PubServContractPeriodSummReadInterceptor>();

            this.Container.RegisterDomainInterceptor<ManOrgContractService, ManOrgContractServiceInterceptor>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IRoleTypeHousePermissionService, RoleTypeHousePermissionService>();
            this.Container.RegisterTransient<IConstructObjMonitoringSmrService, ConstructObjMonitoringSmrService>();
            this.Container.RegisterTransient<IConstructionObjectService, ConstructionObjectService>();
            this.Container.RegisterTransient<INormConsumptionService, NormConsumptionService>();

            Component.For<IChargeSplittingService>()
                .Forward<IPublicServiceOrgExportService>()
                .Forward<IBudgetOrgContractExportService>()
                .LifeStyle.Transient
                .ImplementedBy<ChargeSplittingService>()
                .RegisterIn(this.Container);
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterNavigationProvider<NavigationProvider>();
            this.Container.RegisterNavigationProvider<EmergencyObjMenuProvider>();
            this.Container.RegisterNavigationProvider<ConstructionObjMenuProvider>();
            this.Container.RegisterNavigationProvider<GasEquipmentOrgMenuProvider>();
        }

        private void RegisterExecuteActions()
        {
            this.Container.RegisterExecutionAction<ChangeHasPrivatizedFlats>();
            this.Container.RegisterExecutionAction<ExtractAlreadyMatchedAddressesAction>();
            this.Container.RegisterExecutionAction<StoredProcedureExecutionAction>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportReport, NormConsumptionDataExportReport>("NormConsumptionDataExport");

            this.Container.RegisterTransient<IGkhBaseReport, ContractPeriodSummDetailReport>(ContractPeriodSummDetailReport.Code);
        }

        private void RegisterImports()
        {
            this.Container.RegisterImport<PubServContractImport>();
            this.Container.RegisterImport<BudgetOrgContractImport>();
            this.Container.RegisterImport<FuelEnergyOrgContractImport>();
        }

        private void RegisterGhku()
        {
            this.Container.RegisterTransient<IClaimWorkPermission, UtilityDebtorClaimWorkPermission>();

            this.Container.RegisterSessionScoped<IClwStateProvider, UtilityDebtorClwStateProvider>();

            this.Container.Register(
                Component.For<IBaseClaimWorkService>()
                    .Forward<IBaseClaimWorkService<UtilityDebtorClaimWork>>()
                    .ImplementedBy<UtilityDebtorClaimWorkService>()
                    .DependsOn(Dependency.OnComponent<IClwStateProvider, UtilityDebtorClwStateProvider>())
                    .LifeStyle.SessionScoped());

            this.Container.RegisterTransient<INavigationProvider, UtilityDebtorClaimWorkMenuProvider>();
            this.Container.RegisterTransient<IClaimWorkNavigation, UtilityDebtClaimWorkNavProvider>();

            this.Container.RegisterTransient<IClaimWorkDocRule, ExecutoryProcessRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, SeizureOfPropertyRule>();
            this.Container.RegisterTransient<IClaimWorkDocRule, DepartureRestrictionRule>();

            this.Container.RegisterTransient<IUtilityDebtorClaimWorkInfoService, UtilityDebtorClaimWorkInfoService>();

            this.Container.RegisterImport<UtilityDebtorImport>(UtilityDebtorImport.Id);
        }

        private void CreateStatesForPir()
        {
            this.Container.CreateStates<UtilityDebtorClaimWork>(predicate: x => x.Name.StartsWith("Utility"));
            this.Container.CreateStates<ExecutoryProcess>(predicate: x => x.Name.StartsWith("Utility"));
        }

        private void RegisterFormatDataExport()
        {

            ContainerHelper.ReplaceProxySelectorService<DuProxy,
                MainProxySelectorService.DuSelectorService,
                DuSelectorService>(this.Container);

            ContainerHelper.ReplaceProxySelectorService<DuOuUslugaProxy,
                MainProxySelectorService.DuOuUslugaSelectorService,
                DuOuUslugaSelectorService>(this.Container);

            ContainerHelper.ReplaceProxySelectorService<UstavProxy,
                MainProxySelectorService.UstavSelectorService,
                UstavSelectorService>(this.Container);

            ContainerHelper.ReplaceProxySelectorService<UstavOuUslugaProxy,
                MainProxySelectorService.UstavOuUslugaSelectorService,
                UstavOuUslugaSelectorService>(this.Container);
        }
 }
}
