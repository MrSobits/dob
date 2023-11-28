namespace Bars.Gkh.Gis.Reports.UI.Entities
{
    using Billing.Core.BillingEntity;

    /// <summary>
    /// Список районов для отчета
    /// </summary>
    public class ReportArea : BillingEntity
    {
        /// <summary>
        /// Название района
        /// </summary>
        public virtual string Name { get; set; }
    }
}
