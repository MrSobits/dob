namespace Bars.Gkh.Gis.Reports
{
    using B4;
    using B4.IoC;
    using B4.Modules.Reports;
    using Reports;
    using SqlManager;
    using SqlManager.Impl;

    public class Module: AssemblyDefinedModule
    {
        public override void Install()
        {
            RegisterReport<Report_3_63_3>();
            //RegisterReport<Report_6_46_1>();
            //RegisterReport<Report_MKD>();
            RegisterReport<Report_5_37_1>();
            RegisterReport<Report_5_37_2>();
            RegisterReport<Report_5_37_3>();
            RegisterReport<Report_SZ_Charge>();
            RegisterReport<Report_SZ_Charge_MKD>();
            RegisterReport<Report_SZ_Collection>();
            RegisterReport<Report_SZ_Collection_Service>();
            RegisterReport<Report_SZ_Security>();
            RegisterReport<Report_SZ_Indicator>();
        }

        private void RegisterReport<T>() where T : StimulReportDynamicExcel
        {
            Container.RegisterTransient<IPrintForm, T>(string.Format("Bars.Gkh.Gis.Reports {0}", typeof(T).Name));
        }
    }
}