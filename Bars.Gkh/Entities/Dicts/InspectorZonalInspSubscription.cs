namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Подписка инспекторов на комиссии
    /// </summary>
    public class InspectorZonalInspSubscription : BaseImportableEntity
    {
        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Комиссия
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }
    }
}
