namespace Bars.GkhGji.Regions.Chelyabinsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.IoC;
    using B4.Utils;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Export;
    using Gkh.Authentification;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report;

    /// <summary>
    /// Отчет для приказа
    /// </summary>
    public class GjiChelyabinskDisposalStimulReport : ChelyabinskDisposalStimulReport
    {

        #region Properties

      


        // <summary>
		// Формат экспорта
		// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Odt;
            }
        }

        /// <summary>
        /// Настройки экспорта
        /// </summary>
        public override StiExportSettings ExportSettings
        {
            get
            {
                return new StiOdtExportSettings
                {
                    RemoveEmptySpaceAtBottom = false,

                };
            }
        }
        #endregion Properties
    }
  
}