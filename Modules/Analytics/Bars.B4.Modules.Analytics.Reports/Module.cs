namespace Bars.B4.Modules.Analytics.Reports
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Controllers;
    using Bars.B4.Modules.Analytics.Reports.Controllers.History;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Domain.History;
    using Bars.B4.Modules.Analytics.Reports.DomainServices;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Modules.Analytics.Reports.Interceptors;
    using Bars.B4.Modules.Analytics.Reports.Permissions;
    using Bars.B4.Modules.Analytics.Reports.Tasks;
    using Bars.B4.Modules.Analytics.Reports.ViewModels;
    using Bars.B4.Modules.Analytics.Reports.ViewModels.History;
    using Bars.B4.Modules.ReportPanel.Entities;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Windsor;

    using Castle.MicroKernel.Registration;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;

    using IReportGenerator = Bars.B4.Modules.Analytics.Reports.Generators.IReportGenerator;

    /// <summary>
    /// Модуль
    /// </summary>
    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// Инициализировать модуль
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();

            // PermissionMap
            this.Container.RegisterPermission<ReportsPermissionMap>();

            // StimulConfigs
            StiConfig.Services.Add(new StiPostgreSQLAdapterService());
            StiConfig.Services.Add(new StiPostgreSQLDatabase());

            // Domain services
            this.Container.RegisterDomainService<StoredReport, StoredReportDomainService>();
            this.Container.RegisterDomainService<ReportCustom, ReportCustomDomainService>();

            // Services
            this.Container.RegisterTransient<ICodedReportManager, CodedReportManager>();
            this.Container.RegisterTransient<IReportGenerator, StimulReportGenerator>();
            this.Container.RegisterTransient<IReportPanelService, ReportPanelService>();
            this.Container.RegisterTransient<ICodedReportService, CodedReportService>();
            this.Container.RegisterTransient<IReportGeneratorService, ReportGeneratorService>();
            this.Container.RegisterTransient<IReportHistoryService, ReportHistoryService>();

            //заменяем PrintFormService, чтобы изменить поведение для отчетов на стимуле
            this.Container.ReplaceComponent<IPrintFormService>(typeof(ReportPanel.Domain.PrintFormService),
                Component.For<IPrintFormService>()
                    .ImplementedBy<PrintFormService>()
                    .LifeStyle.Transient);
            
            // Interceptors
            this.Container.RegisterDomainInterceptor<ReportCustom, ReportCustomInterceptor>();
            this.Container.RegisterDomainInterceptor<PrintFormCategory, PrintFormCategoryInterceptor>();

            // Taks
            this.Container.RegisterTransient<ITaskExecutor, ReportGeneratorTask>(ReportGeneratorTask.Id);

            //ViewModel
            this.Container.RegisterViewModel<ReportHistory, ReportHistoryViewModel>();
            this.Container.RegisterViewModel<PrintFormCategory, PrintFormCategoryViewModel>();

            //Controllers
            this.Container.RegisterController<ReportHistoryController>();

            this.Container.ReplaceController<PrintFormController>("printform");
        }
    }
}