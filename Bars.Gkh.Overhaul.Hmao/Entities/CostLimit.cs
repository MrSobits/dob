using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Dicts;
using System;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    /// <summary>
    /// Предельная стоимость услуги
    /// </summary>
    public class CostLimit : BaseEntity
    {
        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Cost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual short? FloorStart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual short? FloorEnd { get; set; }
    }
}
