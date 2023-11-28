namespace Bars.B4.Modules.Analytics.Reports.Extensions
{
    using System;
    using Bars.B4.Modules.Analytics.Reports.Sti;
    using Stimulsoft.Report;

    /// <summary>
    /// 
    /// </summary>
    public static class StiReportExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stiReport"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public static void AddDataSource(this StiReport stiReport, string name, Type type)
        {
            // TODO: Guard asserts!!!
            new StiDictBuilder().AddDictionaryDataSource(stiReport, name, type);
        }

        public static void RegData(this StiReport stiReport, string name, Type type, object data)
        {
            new StiReportDataRegistrator().RegData(stiReport, name, type, data);
        }
    }
}
