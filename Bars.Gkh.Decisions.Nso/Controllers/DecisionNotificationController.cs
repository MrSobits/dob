namespace Bars.Gkh.Decisions.Nso.Controllers
{
    using System.IO;
    using System.Web.Mvc;

    using B4.Modules.FileStorage;
    using B4;
    using B4.Modules.DataExport.Domain;
    using Bars.B4.IoC;
    using Domain.Decisions;
    using Entities;

    public class DecisionNotificationController : FileStorageDataController<DecisionNotification>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("DecisionNotificationExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public void DownloadNotification(BaseParams baseParams)
        {
            var service = Container.Resolve<IDecisionNotificationService>();
            using (Container.Using(service))
            {
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"Уведомление о выборе способа формирования фонда капитального ремонта.xlsx\"");
                Response.StatusCode = 200;
                Response.BinaryWrite(
                    ((MemoryStream)service.DownloadNotification(baseParams)).ToArray()
                );
            }
        }
    }
}