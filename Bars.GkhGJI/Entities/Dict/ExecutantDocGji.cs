namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник - Составители
    /// </summary>
    public class ExecutantDocGji : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Справочник - Составители - Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}