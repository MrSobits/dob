namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Analytics.Reports.Sti;
    using Stimulsoft.Report;

    /// <summary>
    /// 
    /// </summary>
    public class EmptyTemplateService
    {

        /// <summary>
        /// Пустой mrt-шаблон.
        /// </summary>
        public static byte[] EmptyTemplate
        {
            get { return Properties.Resources.EmptyReport; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSources"></param>
        /// <param name="addConn"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public Stream GetTemplateWithMeta(IEnumerable<IDataSource> dataSources, bool addConn = false, IEnumerable<IParam> paramsList = null)
        {
            var emptyReportStream = new MemoryStream(Properties.Resources.EmptyReport);

            var stiReport = new StiReport();
            stiReport.Load(emptyReportStream);
            stiReport.Dictionary.Clear();

            foreach (var dataSource in dataSources)
            {
                stiReport.AddDataSource(dataSource.Name, dataSource.GetMetaData());
            }

            if (addConn)
            {
                // TODO: сделать абстрактной, чтобы работало с другими базами
                StiConnection.PostgreConnection.AddTo(stiReport);
            }

            if (paramsList != null)
            {
                AddReportVars(stiReport, paramsList);                
            }

            var result = new MemoryStream();
            result.Seek(0, SeekOrigin.Begin);
            stiReport.Save(result);
            return result;
        }

        private void AddReportVars(StiReport stiReport, IEnumerable<IParam> paramsList)
        {
            foreach (var dpParam in paramsList)
            {
                stiReport.Dictionary.Variables.Add(dpParam.Name, dpParam.CLRType);
            }
        }
    }
}
