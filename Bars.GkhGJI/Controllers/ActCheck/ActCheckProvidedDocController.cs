namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActCheckProvidedDocController : B4.Alt.DataController<ActCheckProvidedDoc>
    {
        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckProvidedDocService>();
            try
            {
                var result = service.AddProvidedDocs(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
            
        }
    }
}