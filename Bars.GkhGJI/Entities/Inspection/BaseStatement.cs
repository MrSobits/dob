namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание обращение граждан ГЖИ
    /// </summary>
    public class BaseStatement : InspectionGji
    {
        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual FormCheck FormCheck { get; set; }

#warning Здесь УО ненужно, неправильно перенесли через конвертацию. Для хранения юр лица есть поле Contragent
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }
    }
}