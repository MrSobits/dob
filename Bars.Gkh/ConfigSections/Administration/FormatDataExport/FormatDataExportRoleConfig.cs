namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;

    using Bars.B4.Modules.Security;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Dto;

    /// <summary>
    /// Сопоставление ролей с поставщиками информации
    /// </summary>
    [DisplayName(@"Сопоставление ролей с поставщиками информации")]
    public class FormatDataExportRoleConfig : IGkhConfigSection
    {
        // TODO Переделать грязь
        /// <summary>
        /// УО
        /// </summary>
        [GkhConfigProperty(DisplayName = "УО")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Uo { get; set; }

        /// <summary>
        /// РСО
        /// </summary>
        [GkhConfigProperty(DisplayName = "РСО")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Rso { get; set; }

        /// <summary>
        /// ГЖИ
        /// </summary>
        [GkhConfigProperty(DisplayName = "ГЖИ")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Gji { get; set; }

        /// <summary>
        /// ОМЖК
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОМЖК")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Omjk { get; set; }

        /// <summary>
        /// ФСТ
        /// </summary>
        [GkhConfigProperty(DisplayName = "ФСТ")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Fst { get; set; }

        /// <summary>
        /// ОГВ субъекта РФ
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОГВ субъекта РФ")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Ogv { get; set; }

        /// <summary>
        /// ОМС
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОМС")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Oms { get; set; }

        /// <summary>
        /// ОИВ субъекта РФ по регулированию тарифов
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОИВ субъекта РФ по регулированию тарифов")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Oiv { get; set; }

        /// <summary>
        /// Администратор общего собрания собственников помещений в многоквартирном доме
        /// </summary>
        [GkhConfigProperty(DisplayName = "Администратор общего собрания собственников помещений в многоквартирном доме")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> AdminOss { get; set; }

        /// <summary>
        /// ОГВ субъекта РФ по энергосбережению
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОГВ субъекта РФ по энергосбережению")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> OgvEnergo { get; set; }

        /// <summary>
        /// Орган или организация, уполномоченная на осуществление государственного учета жилищного фонда
        /// </summary>
        [GkhConfigProperty(DisplayName = "Орган или организация, уполномоченная на осуществление государственного учета жилищного фонда")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> GjiAccounting { get; set; }

        /// <summary>
        /// Региональный оператор капитального ремонта
        /// </summary>
        [GkhConfigProperty(DisplayName = "Региональный оператор капитального ремонта")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> RegOpCr { get; set; }

        /// <summary>
        /// Фонд содействия реформированию жилищно-коммунального хозяйства
        /// </summary>
        [GkhConfigProperty(DisplayName = "Фонд содействия реформированию жилищно-коммунального хозяйства")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> GkhFound { get; set; }

        /// <summary>
        /// Уполномоченный орган субъекта РФ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Уполномоченный орган субъекта РФ")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Uos { get; set; }

        /// <summary>
        /// Минстрой России
        /// </summary>
        [GkhConfigProperty(DisplayName = "Минстрой России")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> MinStroy { get; set; }

        /// <summary>
        /// Региональный оператор по обращению с твердыми коммунальными отходами
        /// </summary>
        [GkhConfigProperty(DisplayName = "Региональный оператор по обращению с твердыми коммунальными отходами")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> RegOpWaste { get; set; }

        /// <summary>
        /// Расчетный центр
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчетный центр")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Rc { get; set; }
    }
}