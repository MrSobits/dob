namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Обстоятельства дела
    /// </summary>
    public class ResolutionLongText : BaseEntity
    {
        /// <summary>
        /// Постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }
    }
}