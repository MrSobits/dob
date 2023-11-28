namespace Bars.Gkh.RegOperator.Imports.FsGorod
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Gkh.Enums.Import;
    using Import;
    using Entities;
    using Castle.Windsor;
    using Import.Impl;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;

    /// <summary>
    /// Импорт настроек импорта оплат
    /// </summary>
    public class FsGorodImportInfoSettingsImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private ILogImportManager LogManager { get; set; }

        private readonly IWindsorContainer _container = ApplicationContext.Current.Container;

        private ILogImport _logImport;

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Код импорта
        /// </summary>
        public override string CodeImport
        {
            get { return "FsGorodImportInfoSettings"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт настроек импорта оплат"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "json"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.FsGorodImportInfoSettings.Import"; }
        }

        /// <summary>
        /// Метод импорта
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var domain = _container.ResolveDomain<FsGorodImportInfo>();
            var itemDomain = _container.ResolveDomain<FsGorodMapItem>();
            using (_container.Using(domain, itemDomain))
            {
                var fileData = baseParams.Files["FileImport"];

                InitLog(fileData.FileName);

                using (var memoryStreamFile = new MemoryStream(fileData.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding("utf-8"));
                    var proxy = JsonConvert.DeserializeObject<FsGorodImportInfoProxy>(reader.ReadToEnd());

                    var importInfo = proxy.AsEntity();
                    var mapItems = proxy.MapItems != null
                        ? proxy.MapItems.Select(x => x.AsEntity(importInfo))
                        : new List<FsGorodMapItem>();

                    domain.Save(importInfo);
                    mapItems.ForEach(itemDomain.Save);
                }

                LogManager.Add(fileData, _logImport);
            }

            return new ImportResult(StatusImport.CompletedWithoutError);
        }

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns>результат проверки</returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];

            if (!PossibleFileExtensions.Equals(fileData.Extention))
            {
                message = "Недопустимое расширение файла";
                return false;
            }

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding("utf-8"));

                JsonSchema schema = new JsonSchema();
                schema.Type = JsonSchemaType.Object;
                schema.Properties = new Dictionary<string, JsonSchema>
                {
                    {"Code", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                    {"Name", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                    {"Description", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                    {"DataHeadIndex", new JsonSchema { Type = JsonSchemaType.Integer }},
                    {"Delimiter", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                    {"MapItems", new JsonSchema
                    {
                        Type = JsonSchemaType.Array,
                        Items = new List<JsonSchema>{new JsonSchema
                        {
                            Type = JsonSchemaType.Object,
                            Properties = new Dictionary<string, JsonSchema>
                            {
                                {"IsMeta", new JsonSchema { Type = JsonSchemaType.Boolean }},
                                {"Index", new JsonSchema { Type = JsonSchemaType.Integer }},
                                {"Regex", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                                {"PropertyName", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                                {"GetValueFromRegex", new JsonSchema { Type = JsonSchemaType.Boolean }},
                                {"RegexSuccessValue", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                                {"ErrorText", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                                {"UseFilename", new JsonSchema { Type = JsonSchemaType.Boolean }},
                                {"Format", new JsonSchema { Type = JsonSchemaType.String | JsonSchemaType.Null }},
                                {"PaymentAgent", new JsonSchema { Type = JsonSchemaType.Object | JsonSchemaType.Null }}

                            }
                        }}
                    }}
                };

                JObject importInfo = JObject.Parse(reader.ReadToEnd());
                if (!importInfo.IsValid(schema))
                {
                    message = "Файл не соответствует формату.";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Инициализация интерфейса лога импорта
        /// </summary>
        /// <param name="fileName"></param>
        public void InitLog(string fileName)
        {
            this.LogManager = this._container.Resolve<ILogImportManager>();
            if (this.LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogManager.FileNameWithoutExtention = fileName;
            this.LogManager.UploadDate = DateTime.Now;

            this._logImport = this._container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (this._logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this._logImport.SetFileName(fileName);
            this._logImport.ImportKey = this.Key;
        }

    }
}
