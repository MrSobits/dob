namespace Bars.Gkh.Entities.HousingInspection
{
    /// <summary>
    /// Административная комиссия
    /// </summary>
    public class HousingInspection : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}