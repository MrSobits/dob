namespace Bars.Gkh.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Контроллер для работы с кодами субъектов РФ
    /// </summary>
    public class RegionCodeController : BaseController
    {
        /// <summary>
        /// Возвращает коллекцию код региона - наименование региона
        /// </summary>
        public ActionResult GetAll(BaseParams baseParams)
        {
            var data = this.Container.Resolve<IRegionCodeService>().GetAll()
                .Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value
                })
                .ToList();

            return new JsonNetResult(new ListDataResult(data, data.Count));
        }
    }
}