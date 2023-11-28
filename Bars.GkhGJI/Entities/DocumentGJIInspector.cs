namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспекторы документа ГЖИ
    /// </summary>
    public class DocumentGjiInspector : BaseGkhEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Порядковый номер инспектора для текущего документа
        /// </summary>
        public virtual int Order { get; set; }
    }
}