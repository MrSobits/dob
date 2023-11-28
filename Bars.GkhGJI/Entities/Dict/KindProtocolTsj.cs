﻿namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    ///Состояние протокола
    /// </summary>
    public class KindProtocolTsj : BaseGkhEntity
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