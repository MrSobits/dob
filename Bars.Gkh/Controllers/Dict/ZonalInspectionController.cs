namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class ZonalInspectionController : B4.Alt.DataController<ZonalInspection>
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IZonalInspectionService>().AddInspectors(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddMunicipalites(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IZonalInspectionService>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получение по OKATO
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetByOkato(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IZonalInspectionService>().GetByOkato(baseParams);
            return new JsonGetResult(result.Data);
        }
    }
}