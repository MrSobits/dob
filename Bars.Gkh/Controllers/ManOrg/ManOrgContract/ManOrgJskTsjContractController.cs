namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using B4;
    using B4.Modules.FileStorage;
    using Bars.B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;

    /// <summary>
    /// Контролер "Управление домами (ТСЖ / ЖСК)"
    /// </summary>
    public class ManOrgJskTsjContractController : FileStorageDataController<ManOrgJskTsjContract>
    {
        /// <summary>
        /// Возвращает объект недвижимости договора
        /// </summary>
        /// <returns></returns>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Жилой дом</returns>
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IManOrgJskTsjContractService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Проверка на дату договора управления 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат проверки</returns>
        public ActionResult VerificationDate(BaseParams baseParams)
        {
            var result = Container.Resolve<IManOrgJskTsjContractService>().VerificationDate(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ManOrgContactDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}