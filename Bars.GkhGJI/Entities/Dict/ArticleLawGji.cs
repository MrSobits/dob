namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// статья закона ГЖИ
    /// </summary>
    public class ArticleLawGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Принадлежность к ОМС/Регион
        /// </summary>
        public virtual string OMS { get; set; }

        /// <summary>
        /// Принадлежность к ОМС/Регион (енум)
        /// </summary>
        public virtual OmsRegionBelonging OmsRegion { get; set; }

        /// <summary>
        /// Наименование ОМС для ОМС/Регион
        /// </summary>
        public virtual string NameOMS { get; set; }

        /// <summary>
        /// Часть
        /// </summary>
        public virtual string Part { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        public virtual string Article { get; set; }

        /// <summary>
        /// КБК Региона
        /// </summary>
        public virtual string KBK { get; set; }

        /// <summary>
        /// Банк получателя для  ОМС/РЕГИОН
        /// </summary>
        public virtual string Bank { get; set; }
    }
}