namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class PublishedProgramController : B4.Alt.DataController<PublishedProgram>
    {
        public IFileManager FileManager { get; set; }
        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        public IPublishProgramService Service { get; set; }

        public ActionResult GetPublishedProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetPublishedProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetValidationForCreatePublishProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetValidationForCreatePublishProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Message) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetValidationForSignEcp(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetValidationForSignEcp(baseParams);
            return result.Success ? new JsonNetResult(result.Message) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetDataToSignEcp(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetDataToSignEcp(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetPdf(BaseParams baseParams)
        {
            var pdfId = baseParams.Params.GetAs<long>("pdfId");

            byte[] data;
            if (pdfId == 0)
            {
                data = new byte[0];
            }
            else
            {
                using (var file = FileManager.GetFile(FileInfoDomain.Get(pdfId)))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        file.CopyTo(tmpStream);
                        data = tmpStream.ToArray();
                    }
                }
            }

            Response.AddHeader("Content-Disposition", "inline; filename=PublishedProgram.pdf");
            Response.AddHeader("Content-Length", data.Length.ToString());
            return File(data, "application/pdf");
        }

        public ActionResult SaveSignedResult(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.SaveSignedResult(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult DeletePublishedProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.DeletePublishedProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}

