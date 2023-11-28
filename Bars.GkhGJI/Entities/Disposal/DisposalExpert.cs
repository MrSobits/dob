namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Эксперт рапоряжения ГЖИ
    /// </summary>
    public class DisposalExpert : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Эксперт
        /// </summary>
        public virtual ExpertGji Expert { get; set; }
    }
}