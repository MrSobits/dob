namespace Bars.Gkh.Entities.Dicts
{
    using Enums;

    /// <summary>
    ///     Нормативно-правовой документ
    /// </summary>
    public class NormativeDoc : BaseGkhEntity
    {
        /// <summary>
        ///     Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Полное наименвоание
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        ///     Код
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        ///     Категория
        /// </summary>
        public virtual NormativeDocCategory Category { get; set; }
    }
}