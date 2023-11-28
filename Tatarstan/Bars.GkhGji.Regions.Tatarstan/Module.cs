namespace Bars.GkhGji.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Events;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Controller;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Impl;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Integration;
    using Bars.GkhGji.Regions.Tatarstan.Integration.Impl;
    using Bars.GkhGji.Regions.Tatarstan.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Permissions;
    using Bars.GkhGji.Regions.Tatarstan.Quartz;
    using Bars.GkhGji.Regions.Tatarstan.Quartz.Impl;
    using Bars.GkhGji.Regions.Tatarstan.StateChange;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict;

    using Castle.MicroKernel.Registration;

    using ActRemovalValidationRule = Bars.GkhGji.StateChange.ActRemovalValidationRule;

    using MainViewModel = Bars.GkhGji.ViewModel;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IReminderRule, AppealCitsReminderRule>("GkhGji.Regions.Tatarstan.Reminder.AppealCitsReminderRule");
            this.Container.RegisterTransient<IReminderRule, InspectionReminderRule>("GkhGji.Regions.Tatarstan.Reminder.InspectionReminderRule");
            this.Container.ReplaceTransient<IRuleChangeStatus, ActRemovalValidationRule, StateChange.ActRemovalValidationRule>();

            this.Container.ReplaceTransient<IReminderService, GkhGji.DomainService.ReminderService, DomainService.ReminderService>();

            this.Container.ReplaceTransient<IBaseStatementService, GkhGji.DomainService.BaseStatementService, DomainService.BaseStatementService>();

            this.RegisterCommonComponents();

            this.RegisterControllers();

            this.RegisterStateChange();

            this.RegisterInterceptors();

            this.RegistrationInspectionRules();

            this.RegisterIntegrations();

            this.RegisterViewModels();

            this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Bars.GkhGji.Report.ResolutionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Bars.GkhGji.Regions.Tatarstan.Report.ResolutionGji.ResolutionGjiReport>().LifeStyle.Transient);

            this.RegistrationQuartz();
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhGji.Regions.Tatarstan resources");

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiPermissionMap>());

            this.Container.RegisterTransient<IGjiTatParamService, GjiTatParamService>();

            this.Container.RegisterTransient<IFieldRequirementSource, GkhGjiFieldRequirementMap>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<GisChargeController>();
            this.Container.RegisterController<GisGmpParamsController>();
            this.Container.RegisterController<GmpInnCheckerController>();
            this.Container.RegisterController<GisGmpPatternController>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<Disposal, DisposalValidateInterceptor>();

            this.Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionCancel>>(
                typeof(GkhGji.Interceptors.PrescriptionCancelInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionViol>>(
                typeof(GkhGji.Interceptors.PrescriptionViolInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionViol>>().ImplementedBy<PrescriptionViolInterceptor>().LifestyleTransient());

            this.Container.ReplaceComponent<IDomainServiceInterceptor<ProtocolMvd>>(
                typeof(GkhGji.Interceptors.ProtocolMvdServiceInterceptor),
                Component.For<IDomainServiceInterceptor<ProtocolMvd>>().ImplementedBy<ProtocolMvdServiceInterceptor>().LifestyleTransient());

            this.Container.ReplaceComponent<IDomainServiceInterceptor<ActCheck>>(
                typeof(GkhGji.Interceptors.ActCheckServiceInterceptor),
                Component.For<IDomainServiceInterceptor<ActCheck>>().ImplementedBy<ActCheckServiceInterceptor>().LifestyleTransient());
        }

        private void RegisterIntegrations()
        {
            this.Container.RegisterTransient<IGisGmpIntegration, GisGmpIntegration>();
        }

        private void RegisterStateChange()
        {
            this.Container.RegisterTransient<IRuleChangeStatus, ResolutionGisChargeRule>();
            this.Container.ReplaceComponent<IRuleChangeStatus>(typeof(GkhGji.StateChange.InspectionValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.InspectionValidationRule>().LifeStyle.Transient);
        }

        public void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
            {
                this.Container.RegisterTransient<IReminderResolution, ReminderResolution>("Reminder Resolution Quartz integration");
                this.Container.RegisterTransient<ITask, ResolutionQuartzTask>();

                ApplicationContext.Current.Events.GetEvent<NhStartEvent>().Subscribe<InitQuartz>();
            }
        }

        public void RegistrationInspectionRules()
        {
            this.Container.ReplaceTransient<IDocumentGjiRule, DisposalToActSurveyRule, TatDisposalToActSurveyRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, DisposalToActCheckRule, TatDisposalToActCheckRule>();
            this.Container.ReplaceTransient<IDocumentGjiRule, DisposalToActCheckByRoRule, TatDisposalToActCheckByRoRule>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<GisChargeToSend, GisChargeViewModel>();
            this.Container.RegisterViewModel<GisGmpPattern, GisGmpPatternViewModel>();

            ContainerHelper.ReplaceViewModel<ActCheck, MainViewModel.ActCheckViewModel, ActCheckViewModel>(this.Container);
            ContainerHelper.ReplaceViewModel<Disposal, MainViewModel.DisposalViewModel, DisposalViewModel>(this.Container);
            ContainerHelper.ReplaceViewModel<SurveySubject, MainViewModel.Dict.SurveySubjectViewModel, SurveySubjectViewModel>(this.Container);
        }
    }
}