namespace Bars.Gkh.Controllers.Dict
{
    using System.Linq;
    using System.Web.Mvc;
    using B4;
    using Entities.Dicts;
    using Enums;

    public class NormativeDocController : Bars.B4.Alt.DataController<NormativeDoc>
    {
        public ActionResult ListOverhaul(StoreLoadParams storeParams)
        {
            var service = Resolve<IDomainService<NormativeDoc>>();

            var data = service.GetAll()
                .Where(x => x.Category == NormativeDocCategory.Overhaul)
                .Filter(storeParams, Container);

            int totalCount = data.Count();

            data = data.Order(storeParams).Paging(storeParams);

            return new JsonListResult(data.ToList(), totalCount);
        }
    }
}