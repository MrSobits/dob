namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Protocol197
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197Controller : B4.Alt.DataController<Protocol197>
	{
		public IBlobPropertyService<Protocol197, Protocol197LongText> LongTextService { get; set; }
		public IProtocol197Service Protocol197Service { get; set; }

		public ActionResult Export(BaseParams baseParams)
		{
			var export = this.Container.Resolve<IDataExportService>("Protocol197DataExport");
			return export != null ? export.ExportData(baseParams) : null;
		}

		public ActionResult ListView(BaseParams baseParams)
		{
			var result = (ListDataResult)this.Container.Resolve<IProtocol197Service>().ListView(baseParams);
			return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
		}
		public ActionResult GetNameList(BaseParams baseParams)
		{
			var result = Protocol197Service.GetNameList(baseParams);
			return new JsonListResult(result, result.Count);
		}

		public ActionResult UpdateComissionDocumentState(BaseParams baseParams)
		{
			var result = Protocol197Service.UpdateComissionDocumentState(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult UpdateComissionDocumentsState(BaseParams baseParams)
		{
			var result = Protocol197Service.UpdateComissionDocumentsState(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult GetInfo(long? documentId)
		{
			var result = this.Container.Resolve<IProtocol197Service>().GetInfo(documentId);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult GetRepeatInfo(long? documentId)
		{
			var result = this.Container.Resolve<IProtocol197Service>().GetRepeatInfo(documentId);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddRequirements(BaseParams baseParams)
		{
			var result = this.Container.Resolve<IProtocol197Service>().AddRequirements(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddDirections(BaseParams baseParams)
		{
			var result = this.Container.Resolve<IProtocol197Service>().AddDirections(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public virtual ActionResult GetDescription(BaseParams baseParams)
		{
			var result = this.LongTextService.Get(baseParams);
			return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public virtual ActionResult SaveDescription(BaseParams baseParams)
		{
			var result = this.LongTextService.Save(baseParams);
			return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult GetFiasProperty(BaseParams baseParams)
		{
			
			try
			{
				var data = this.Container.Resolve<IProtocol197Service>().GetFiasProperty(baseParams);
				return JsSuccess(data);
			}
			finally
			{

			}
		}
	}
}