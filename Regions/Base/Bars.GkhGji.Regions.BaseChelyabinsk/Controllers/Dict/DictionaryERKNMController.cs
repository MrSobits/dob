namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Protocol197
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Dict.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class DictionaryERKNMController : B4.Alt.DataController<DictionaryERKNM>
	{
		//public IBlobPropertyService<Protocol197, Protocol197LongText> LongTextService { get; set; }

		//public ActionResult Export(BaseParams baseParams)
		//{
		//	var export = this.Container.Resolve<IDataExportService>("Protocol197DataExport");
		//	return export != null ? export.ExportData(baseParams) : null;
		//}

		public ActionResult ListView(BaseParams baseParams)
		{
			var result = (ListDataResult)this.Container.Resolve<IDictionaryERKNMService>().ListView(baseParams);
			return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult GetInfo(long? documentId)
		{
			var result = this.Container.Resolve<IDictionaryERKNMService>().GetInfo(documentId);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddRequirements(BaseParams baseParams)
		{
			var result = this.Container.Resolve<IDictionaryERKNMService>().AddRequirements(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddDirections(BaseParams baseParams)
		{
			var result = this.Container.Resolve<IDictionaryERKNMService>().AddDirections(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		//public virtual ActionResult GetDescription(BaseParams baseParams)
		//{
		//	var result = this.LongTextService.Get(baseParams);
		//	return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
		//}

		//public virtual ActionResult SaveDescription(BaseParams baseParams)
		//{
		//	var result = this.LongTextService.Save(baseParams);
		//	return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
		//}
	}
}