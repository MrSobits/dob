namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Общие
    /// </summary>
    [DisplayName("Общие")]
    public class FormatDataExportGeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Запускать экспорт на сервере расчетов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Запускать экспорт на сервере расчетов")]
        [DefaultValue(true)]
        public virtual bool StartInExecutor { get; set; }

        /// <summary>
        /// Не выгружать секции с ошибками
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не выгружать секции с ошибками")]
        [DefaultValue(true)]
        public virtual bool NoEmptyMandatoryFields { get; set; }

        /// <summary>
        /// Не выгружать ссылки на отсутствующие файлы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не выгружать ссылки на отсутствующие файлы")]
        [DefaultValue(true)]
        public virtual bool OnlyExistsFiles { get; set; }

        /// <summary>
        /// Экспорт только измененных данных
        /// </summary>
        [GkhConfigProperty(DisplayName = "Экспорт только измененных данных")]
        [DefaultValue(false)]
        public virtual bool UseIncrementalData { get; set; }

        /// <summary>
        /// Максимальный размер архива
        /// </summary>
        [GkhConfigProperty(DisplayName = "Максимальный размер архива (МБ)")]
        [DefaultValue(200)]
        [Range(1, int.MaxValue)]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual int? MaxArchiveSize { get; set; }

        /// <summary>
        /// Адрес удаленного сервера для передачи данных
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес удаленного сервера для передачи данных")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual string TransferServiceAddress { get; set; }
    }
}