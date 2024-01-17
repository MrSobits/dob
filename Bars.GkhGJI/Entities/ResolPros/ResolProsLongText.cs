namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Описание дела
    /// </summary>
    public class ResolProsLongText : BaseEntity
    {
        /// <summary>
        /// Постановление
        /// </summary>
        public virtual ResolPros ResolPros { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }
    }
}