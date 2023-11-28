namespace Bars.Gkh.Entities.Suggestion
{
    /// <summary>
    /// Дополнительные атрибуты
    /// </summary>
    public class CategoryPosts : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
