namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// 
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSources"></param>
        /// <param name="reportTemplate"></param>
        /// <param name="baseParams"></param>
        /// <param name="printFormat"></param>
        /// <param name="customArgs"></param>
        Stream Generate(IEnumerable<IDataSource> dataSources, Stream reportTemplate, BaseParams baseParams, ReportPrintFormat printFormat, IDictionary<string, object> customArgs);
    }
}
