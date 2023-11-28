namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Windsor;
    using Bars.Gkh.DomainService;
    using ViewModel;
    using Castle.MicroKernel.Registration;
    using Controllers;
    using Domain;
    using Entities;
    using Import;
    using Imports;
    using Interceptors;
    using RegOperator.Entities;
    using RegOperator.Entities.Owner;
    using Utils;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterResourceManifest<ResourceManifest>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhRegopChelyabinsk navigation");
            this.RegisterControllers();
            this.RegisterDomainServices();
            this.RegisterViewModel();
            RegisterInterceptors();
        }

        private void RegisterControllers()
        {

            this.Container.RegisterAltDataController<LawSuitDebtWorkSSP>();
            this.Container.RegisterAltDataController<LawSuitDebtWorkSSPDocument>();
            this.Container.RegisterController<MassDebtWorkSSPController>();
            this.Container.RegisterController<RosRegExtractOperationsController>();
            this.Container.RegisterController<ArchivedClaimWorkController>();
        }
        private void RegisterInterceptors()
        {           
            this.Container.RegisterDomainInterceptor<LawSuitDebtWorkSSP, LawSuitDebtWorkSSPInterceptor>();          
        }

        private void RegisterDomainServices()
        {
            this.Container.RegisterTransient<IRosRegExtractOperationsService, RosRegExtractOperationsService>();
        }

        private void RegisterViewModel()
        {
            this.Container.RegisterViewModel<LawSuitDebtWorkSSP, LawSuitDebtWorkSSPViewModel>();
            this.Container.RegisterViewModel<LawSuitDebtWorkSSPDocument, LawSuitDebtWorkSSPDocumentViewModel>();

        }
    }
}