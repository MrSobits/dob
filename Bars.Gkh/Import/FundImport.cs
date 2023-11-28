namespace Bars.Gkh.Import
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;

    using Bars.Gkh.Enums.Import;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    public class FundImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private ILogImport logImport;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "FundImport"; }
        }

        public override string Name
        {
            get { return "Импорт по форме фонда"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.FundImport.View"; }
        }

        /// <summary>
        /// Менеджер управляющий логами
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];
            InitLog(fileData.FileName);

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
            }

            LogManager.Add(fileData, logImport);
            LogManager.Save();

            message += LogManager.GetInfo();
            var status = LogManager.CountError > 0 ? StatusImport.CompletedWithError : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;
        }
    }
}
