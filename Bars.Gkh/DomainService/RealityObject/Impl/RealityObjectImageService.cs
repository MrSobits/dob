namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для <see cref="RealityObjectImage" />
    /// </summary>
    public class RealityObjectImageService : IRealityObjectImageService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сохранить изображение
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Успешность</returns>
        public IDataResult SaveImage(BaseParams baseParams)
        {
            var roImageDomainService = this.Container.Resolve<IDomainService<RealityObjectImage>>();

            using (this.Container.Using(roImageDomainService))
            {
                var saveParam = baseParams
                    .Params
                    .Read<SaveParam<RealityObjectImage>>()
                    .Execute(container => Converter.ToSaveParam<RealityObjectImage>(container, true));

                var savedIds = new List<long>();

                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    var image = baseParams.Files.FirstOrDefault();

                    if (image.Value != null)
                    {
                        value.File = this.CreateFile(image.Value);
                    }

                    if (value.Id == 0)
                    {
                        roImageDomainService.Save(value);
                    }
                    else
                    {
                        roImageDomainService.Update(value);
                    }
                    savedIds.Add(value.Id);
                }

                return new BaseDataResult(savedIds);
            }
        }

        private FileInfo CreateFile(FileData fileData)
        {
            var fileManager = this.Container.Resolve<IFileManager>();

            // уменьшаем image до 5Mb
            const int allowedFileSizeInByte = 5 * 1024 * 1024;

            using (this.Container.Using(fileManager))
            {
                var imageConverter = new ImageConverter();
                var image = (Image) imageConverter.ConvertFrom(fileData.Data);

                image.RotateWithMeta();

                var bitmap = new Bitmap(image);

                using (var memoryStream = new MemoryStream())
                {
                    ImageResizeExtension.SaveTemporary(bitmap, memoryStream, 100, fileData.Extention);

                    if (fileData.Data.Length > allowedFileSizeInByte)
                    {
                        while (memoryStream.Length > allowedFileSizeInByte)
                        {
                            var scale = Math.Sqrt((double) allowedFileSizeInByte / (double) memoryStream.Length);
                            memoryStream.SetLength(0);
                            bitmap = ImageResizeExtension.ScaleImage(bitmap, scale);
                            ImageResizeExtension.SaveTemporary(bitmap, memoryStream, 100, fileData.Extention);
                        }
                    }

                    bitmap?.Dispose();

                    fileData.Data = memoryStream.ToArray();
                }

                return fileManager.SaveFile(fileData);
            }
        }
    }
}