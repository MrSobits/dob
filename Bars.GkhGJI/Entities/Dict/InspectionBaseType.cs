namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Справочник Основание проверки
    /// </summary>
    public class InspectionBaseType : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}