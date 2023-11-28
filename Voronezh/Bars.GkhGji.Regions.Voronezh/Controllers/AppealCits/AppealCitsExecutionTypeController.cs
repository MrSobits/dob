namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Entities;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class AppealCitsExecutionTypeController : B4.Alt.DataController<AppealCitsExecutionType>
    {
        /// <summary>
        /// Метод добавления связанных категорий заявителя
        /// </summary>
        public ActionResult AddAppealCitizens(BaseParams baseParams)
        {
            var recordIds = baseParams.Params.GetAs<long[]>("recordIds");
            var appealCitizensId = baseParams.Params.GetAsId("appealCitizensId");

            var existsIds = this.DomainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => x.AppealExecutionType.Id)
                .ToArray();

            this.Container.InTransaction(() =>
            {
                foreach (var id in recordIds.Except(existsIds))
                {
                    this.DomainService.Save(new AppealCitsExecutionType
                    {
                        AppealCits = new AppealCits {Id = appealCitizensId},
                        AppealExecutionType = new AppealExecutionType { Id = id}
                    });
                }
            });

            return this.JsSuccess();
        }
    }
}