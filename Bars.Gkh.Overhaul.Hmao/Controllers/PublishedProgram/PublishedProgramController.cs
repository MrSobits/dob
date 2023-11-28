namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using B4;
    using System.Linq;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Hmao.DomainService;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PublishedProgramController : B4.Alt.DataController<Entities.PublishedProgram>
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

        public ActionResult GetDataForInfoPanel(BaseParams baseParams, long muId)
        {
            var versionContainer = this.Container.Resolve<IDomainService<VersionRecord>>();
            var allVersionRecords = versionContainer.GetAll()
                .Where(x => x.ProgramVersion.IsMain && x.Show);
            var versionRecords = versionContainer.GetAll()
                .Where(x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == muId && x.Show);

            var allTotalCount = allVersionRecords.Select(x => x.RealityObject.Id).Distinct().Count();
            var allMainCount = allVersionRecords.Where(x => !x.SubProgram)
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allSubCount = allVersionRecords.Where(x => x.SubProgram)
                .Select(x => x.RealityObject.Id).Distinct().Count();

            var totalCount = versionRecords.Select(x => x.RealityObject.Id).Distinct().Count();
            var mainCount = versionRecords.Where(x => !x.SubProgram)
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var subCount = versionRecords.Where(x => x.SubProgram)
                .Select(x => x.RealityObject.Id).Distinct().Count();


            //ToDo привести это к нормальному виду
            var comObjElect = versionRecords.Where(x => x.CommonEstateObjects == "Электроснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjLift = versionRecords.Where(x => x.CommonEstateObjects == "Лифтовое хозяйство")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjWaterO = versionRecords.Where(x => x.CommonEstateObjects == "Водоотведение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjWaterS = versionRecords.Where(x => x.CommonEstateObjects == "Водоснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjRoof = versionRecords.Where(x => x.CommonEstateObjects == "Крыша")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjGas = versionRecords.Where(x => x.CommonEstateObjects == "Газоснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjPodv = versionRecords.Where(x => x.CommonEstateObjects == "Подвальные помещения")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjWarm = versionRecords.Where(x => x.CommonEstateObjects == "Теплоснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjFundam = versionRecords.Where(x => x.CommonEstateObjects == "Фундамент")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var comObjFas = versionRecords.Where(x => x.CommonEstateObjects == "Фасад")
                .Select(x => x.RealityObject.Id).Distinct().Count();

            var allComObjElect = allVersionRecords.Where(x => x.CommonEstateObjects == "Электроснабжение")
               .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjLift = allVersionRecords.Where(x => x.CommonEstateObjects == "Лифтовое хозяйство")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjWaterO = allVersionRecords.Where(x => x.CommonEstateObjects == "Водоотведение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjWaterS = allVersionRecords.Where(x => x.CommonEstateObjects == "Водоснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjRoof = allVersionRecords.Where(x => x.CommonEstateObjects == "Крыша")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjGas = allVersionRecords.Where(x => x.CommonEstateObjects == "Газоснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjPodv = allVersionRecords.Where(x => x.CommonEstateObjects == "Подвальные помещения")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjWarm = allVersionRecords.Where(x => x.CommonEstateObjects == "Теплоснабжение")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjFundam = allVersionRecords.Where(x => x.CommonEstateObjects == "Фундамент")
                .Select(x => x.RealityObject.Id).Distinct().Count();
            var allComObjFas = allVersionRecords.Where(x => x.CommonEstateObjects == "Фасад")
                .Select(x => x.RealityObject.Id).Distinct().Count();

            var data = new { totalCount = totalCount, mainCount = mainCount, subCount = subCount, comObjElect = comObjElect, allTotalCount = allTotalCount, allMainCount = allMainCount,
                allSubCount = allSubCount, comObjLift = comObjLift,  comObjWaterO = comObjWaterO, comObjWaterS = comObjWaterS, comObjRoof = comObjRoof, comObjGas = comObjGas,
                comObjPodv = comObjPodv, comObjWarm = comObjWarm,
                comObjFundam = comObjFundam,
                comObjFas = comObjFas,
                allComObjElect = allComObjElect,
                allComObjLift = allComObjLift,
                allComObjWaterO = allComObjWaterO,
                allComObjWaterS = allComObjWaterS,
                allComObjRoof = allComObjRoof,
                allComObjGas = allComObjGas,
                allComObjPodv = allComObjPodv,
                allComObjWarm = allComObjWarm,
                allComObjFundam = allComObjFundam,
                allComObjFas = allComObjFas
            };
            return JsSuccess(data);
        }
    }
}