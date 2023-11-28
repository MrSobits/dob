namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Региональный оператор
    /// </summary>
    public class RegOperator : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}