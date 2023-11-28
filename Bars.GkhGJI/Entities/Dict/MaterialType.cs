namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Содержание ответа
    /// </summary>
    public class MaterialType : BaseGkhEntity
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
        /// является доказательством
        /// </summary>
        public virtual bool IsProof { get; set; }
    }
}