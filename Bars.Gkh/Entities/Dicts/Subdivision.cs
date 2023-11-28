namespace Bars.Gkh.Entities

{

    using Bars.Gkh.Enums;

    /// <summary>
    /// Члены комиссии
    /// </summary>
    public class Subdivision : BaseGkhEntity
    {

        /// <summary>
        /// Подразделение
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}
