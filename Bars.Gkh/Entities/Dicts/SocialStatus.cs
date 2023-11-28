namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Спарвочник "социальный статус"
    /// </summary>
    public class SocialStatus : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}
