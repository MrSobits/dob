namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник "Категории риска УК"
    /// </summary>
    public class RiskCategory : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Показатель риска от
        /// </summary>
        public virtual int? RiskFrom{ get; set; }

        /// <summary>
        /// Показатель риска до
        /// </summary>
        public virtual int? RiskTo{ get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

    }
}