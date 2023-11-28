namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    // <summary>
    /// Справочники - Коды типов документов
    /// </summary>

    public class PhysicalPersonDocType : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Справочники - Коды типов документов - Наименование
        /// </summary>
        public virtual string Name { get; set; }

    }
}
