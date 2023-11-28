namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    /// <summary>
    /// Справочник - Решения
    /// </summary>
    public class ConcederationResult: BaseEntity
    {
        /// <summary>
        /// Справочник - решения - Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}