namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.ClaimWork;

    /// <summary>
    /// Основание ПИР для неплательщиков - юр. лиц
    /// </summary>
    public class LegalClaimWork : DebtorClaimWork
    {
        /// <summary>
        /// Адрес зоны подсудности
        /// </summary>
        public virtual RealityObject JurisdictionAddress { get; set; }

        public LegalClaimWork()
        {
            this.DebtorType = DebtorType.Legal;
        }
    }
}