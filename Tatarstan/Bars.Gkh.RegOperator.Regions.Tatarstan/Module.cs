namespace Bars.Gkh.RegOperator.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Windsor;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Tat.PriorityParams;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Export.Reports;
    using Bars.Gkh.RegOperator.Export.Reports.Impl;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Controllers;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.DataProviders;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.ExecutionAction;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Export;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Imports;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Interceptors;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Interceptors.TransferFunds;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Permissions;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.PriorityParam;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels;
    using Bars.Gkh.RegOperator.Tasks.Reports;
    using Bars.Gkh.Utils;
    using Bars.GkhRf.Entities;
    using Castle.MicroKernel.Registration;
    using MainExportableEntities = Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.RegisterBaseComponents();

            this.RegisterImportsExports();

            this.RegisterServices();

            this.RegisterViewModel();

            this.RegisterControllers();

            this.RegisterInterceptors();

            this.RegisterDomainService();

            this.RegisterExecutionActions();

            this.RegisterTasks();

            this.RegisterFormatDataExport();
        }

        private void RegisterBaseComponents()
        {
            this.Container.RegisterPermissionMap<GkhRegOpRegionsTatarstanPermissionMap>();
            this.Container.RegisterNavigationProvider<NavigationProvider>();
            this.Container.Register(Component.For<IClientRouteMapRegistrar>()
                .ImplementedBy<ClientRouteMapRegistrar>()
                .LifestyleTransient());
            this.Container.Register(
                Component.For<IResourceManifest>().Named("Gkh.RegOperator.Regions.Tatarstan resources").ImplementedBy<ResourceManifest>()
                .LifestyleTransient());
            this.Container.Register(
                Component.For<IModuleDependencies>()
                    .Named(string.Format("{0} dependencies", this.AssemblyName))
                    .LifeStyle.Singleton.UsingFactoryMethod(
                        () => new ModuleDependencies(this.Container).Init()));
        }

        private void RegisterImportsExports()
        {
            this.Container.Register(
                Component.For<IDataExportService>()
                    .Named("ConfirmContributionExport")
                    .ImplementedBy<ConfirmContributionExport>()
                    .LifestyleTransient());
            this.Container.RegisterImport<ThirdPartyPersonalAccountImport>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IPersonalAccountInfoExportService, PersonalAccountInfoExportService>();
            this.Container.RegisterTransient<IDataProvider, ControlContractAndAccumFunds>();
            this.Container.RegisterTransient<IPriorityParams, AmountOfSavingsCrPriorityParam>();
            this.Container.RegisterTransient<IPriorityParams, PaymentToChargePercentPriorityParam>();
        }

        private void RegisterViewModel()
        {
            this.Container.RegisterViewModel<TransferHire, TransferHireViewModel>();
            this.Container.RegisterViewModel<ConfirmContribution, ConfirmContributionViewModel>();
            this.Container.RegisterViewModel<ConfirmContributionDoc, ConfirmContributionDocViewModel>();
            this.Container.ReplaceComponent<IViewModel<TransferObject>>(typeof(Gkh.RegOperator.ViewModels.TransferObjectViewModel),
                Component.For<IViewModel<TransferObject>>().ImplementedBy<TransferObjectViewModel>());
            this.Container.ReplaceComponent<IViewModel<RealityObjectChargeAccountOperation>>(typeof(Gkh.RegOperator.ViewModels.RealityObjectChargeAccountOperationViewModel),
                Component.For<IViewModel<RealityObjectChargeAccountOperation>>().ImplementedBy<ViewModel.RealityObjectChargeAccountOperationViewModel>());
            this.Container.ReplaceComponent<IViewModel<TransferRfRecord>>(
                typeof (Bars.Gkh.RegOperator.DomainService.TransferRf.TransferRfRecordViewModel),
                Component.For<IViewModel<TransferRfRecord>>().ImplementedBy<TransferRfRecordViewModel>());
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<TransferHireController>();
            this.Container.RegisterController<ConfirmContributionController>();
            this.Container.RegisterController<ConfirmContributionDocController>();
            this.Container.RegisterController<ConfirmContributionManagOrgController>();
            this.Container.RegisterController<ConfirmContributionRealityObjectController>();
        }

        private void RegisterInterceptors()
        {
            // регистрация Interceptors
            this.Container.RegisterDomainInterceptor<ConfirmContribution, ConfirmContributionInterceptor>();
            this.Container.RegisterDomainInterceptor<ConfirmContributionDoc, ConfirmContributionDocInterceptor>();
            this.Container.RegisterDomainInterceptor<TransferRfRecord, TransferRfRecordInterceptor>();
        }

        private void RegisterDomainService()
        {
            // регистрация DomainServices
            this.Container.RegisterDomainService<ConfirmContributionDoc, FileStorageDomainService<ConfirmContributionDoc>>();
            this.Container.RegisterTransient<IConfirmContributionService, ConfirmContributionService>();
            this.Container.ReplaceComponent<ITransferObjectService>(typeof(Gkh.RegOperator.DomainService.Impl.TransferObjectService),
                Component.For<ITransferObjectService>().ImplementedBy<TransferObjectService>());
            this.Container.RegisterTransient<ITransferHireService, TransferHireService>();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<DeleteDuplicatesTransferHireTat>();
        }

        private void RegisterTasks()
        {
            this.Container.RegisterTaskExecutor<PersonalAccountInfoExportTaskExecutor>(PersonalAccountInfoExportTaskExecutor.Id);
        }

        private void RegisterFormatDataExport()
        {
            //ContainerHelper.ReplaceExportableEntity<MainExportableEntities.DuVotproExportableEntity,
            //    DuVotproExportableEntity>(this.Container);
            //ContainerHelper.ReplaceExportableEntity<MainExportableEntities.UstavVotprotExportableEntity,
            //    UstavVotprotExportableEntity>(this.Container);
        }
    }
}