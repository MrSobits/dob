namespace Bars.Gkh.Controllers.HousingInspection
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities.HousingInspection;

    public class HousingInspectionMunicipalityController : B4.Alt.DataController<HousingInspectionMunicipality>
    {
        /// <summary>
        /// Добавить муниципальные образования
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            return this.Container.Resolve<IHousingInspectionMunicipalityService>().AddMunicipalities(baseParams).ToJsonResult();
        }
    }
}