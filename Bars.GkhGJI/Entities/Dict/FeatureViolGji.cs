namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Группа нарушений
    /// </summary>
    public class FeatureViolGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код реформы ЖКХ
        /// </summary>
        public virtual string GkhReformCode { get; set; }

        /// <summary>
        /// Полное наименование включая и родительские группы разделенные через '/'
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// Код характеристики в Тематическом классификаторе
        /// </summary>
        public virtual string QuestionCode { get; set; }

        /// <summary>
        /// Родительская группа нарушений
        /// </summary>
        public virtual FeatureViolGji Parent { get; set; }
    }
}