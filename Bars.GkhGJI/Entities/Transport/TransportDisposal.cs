namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Таблица транспорта который участвует в протоколе 
    /// </summary>
    public class TransportDisposal : BaseGkhEntity
    {
        /// <summary>
        /// Связь с таблицей транспорта  
        /// </summary>
        public virtual Transport Transport { get; set; }
        /// <summary>
        /// Связь с таблицей распоряжения 
        /// </summary>
        public virtual Disposal Disposal { get; set; }
    }
}
