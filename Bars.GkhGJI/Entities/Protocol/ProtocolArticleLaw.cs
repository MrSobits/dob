namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Статьи закона в протоколе ГЖИ
    /// </summary>
    public class ProtocolArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol Protocol { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}