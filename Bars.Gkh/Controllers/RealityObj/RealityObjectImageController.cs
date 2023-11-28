namespace Bars.Gkh.Controllers.RealityObj
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Контроллер для фотографий жилого дома
    /// </summary>
    public class RealityObjectImageController : FileStorageDataController<RealityObjectImage>
    {
        public IFileManager FileManager { get; set; }

        public IRealityObjectImageService RealityObjectImageService { get; set; }

        /// <summary>
        /// Получение файла
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Файл в формате Base64String</returns>
        public ActionResult GetFileUrl(BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAs<long>("id");

            var objectImage = this.DomainService.GetAll()
                .FirstOrDefault(x => x.RealityObject.Id == objectId && x.ImagesGroup == ImagesGroup.Avatar);

            try
            {
                if (objectImage?.File == null || !this.FileManager.CheckFile(objectImage.File.Id).Success)
                {
                    return new JsonNetResult(new {success = false});
                }

                return new JsonNetResult(
                    new
                    {
                        success = true,
                        src = this.FileManager.GetBase64String(objectImage.File),
                        imageId = objectImage.Id
                    });
            }
            catch (Exception exception)
            {
                return JsonNetResult.Failure(exception.Message);
            }
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Успешность операции</returns>
        public override ActionResult Create(BaseParams baseParams)
        {
            return this.Js(this.RealityObjectImageService.SaveImage(baseParams));
        }

        /// <summary>
        /// Обновление записи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Успешность операции</returns>
        public override ActionResult Update(BaseParams baseParams)
        {
            return this.Js(this.RealityObjectImageService.SaveImage(baseParams));
        }
    }
}