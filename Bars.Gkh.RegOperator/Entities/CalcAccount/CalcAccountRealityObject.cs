namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Жилой дом расчетного счета
    /// </summary>
    public class CalcAccountRealityObject : BaseImportableEntity
    {
        /// <summary>
        /// Расчетный счет
        /// </summary>
        public virtual CalcAccount Account { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}