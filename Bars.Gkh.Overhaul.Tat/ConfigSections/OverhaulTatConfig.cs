namespace Bars.Gkh.Overhaul.Tat.ConfigSections
{
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.Overhaul;

    /// <summary>
    /// Настройки версии ДПКР без МО
    /// </summary>
    [GkhConfigSection("Overhaul.OverhaulTat", DisplayName = "Настройки версии ДПКР (РТ)", UIParent = typeof(OverhaulConfig))]
    [Permissionable]
    [Navigation]
    public class OverhaulTatConfig : IGkhConfigSection
    {
        /// <summary>
        /// Период долгосрочной программы с
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период долгосрочной программы с")]
        [Group("Период программы")]
        public virtual int ProgrammPeriodStart { get; set; }

        /// <summary>
        /// Период долгосрочной программы по
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период долгосрочной программы по")]
        [Group("Период программы")]
        public virtual int ProgrammPeriodEnd { get; set; }

        /// <summary>
        /// Период группировки КЭ (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период группировки КЭ (год)")]
        [Group("Группировка")]
        public virtual int GroupByCeoPeriod { get; set; }

        /// <summary>
        /// Период группировки ООИ (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период группировки ООИ (год)")]
        [Group("Группировка")]
        public virtual int GroupByRoPeriod { get; set; }

        /// <summary>
        /// Стоимость услуг (% от стоимости СМР)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Стоимость услуг (% от стоимости СМР)")]
        public virtual decimal ServiceCost { get; set; }

        /// <summary>
        /// Годовой процент выплаты по займу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Годовой процент выплаты по займу")]
        public virtual decimal YearPercent { get; set; }

        /// <summary>
        /// Период планирования бюджета (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период планирования бюджета (год)")]
        public virtual int ShortTermProgPeriod { get; set; }

        /// <summary>
        /// Проверка признака фиксации скорректированного года
        /// </summary>
        [GkhConfigProperty(DisplayName = "Проверка признака фиксации скорректированного года")]
        public virtual bool CheckFixingFeatureCorrectionYear { get; set; }

        /// <summary>
        /// Период для публикации (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период для публикации (год)")]
        [Group("Публикация")]
        public virtual int PublicationPeriod { get; set; }

        /// <summary>
        /// Период актуализации программы с
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период актуализации программы с")]
        [Group("Актуализация")]
        public virtual int ActualizePeriodStart { get; set; }

        /// <summary>
        /// Период актуализации программы по
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период актуализации программы по")]
        [Group("Актуализация")]
        public virtual int ActualizePeriodEnd { get; set; }

        /// <summary>
        /// Ограничение на выбор скорректированного года
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничение на выбор скорректированного года")]
        public virtual YearCorrectionConfig YearCorrectionConfig { get; set; }
    }
}