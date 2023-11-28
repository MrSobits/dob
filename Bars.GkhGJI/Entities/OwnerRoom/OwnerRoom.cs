namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities;
    /// <summary>
    /// Жилые дома нарушителей
    /// </summary>
    public class OwnerRoom : BaseGkhEntity
    {
        /// <summary>
        /// Связь с таблицей физ лиц
        /// </summary>
        public virtual IndividualPerson IndividualPerson { get; set; }

        /// <summary>
        /// Связь с таблицей нарушителей
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        /// <summary>
        /// Помещения 
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Дата начала владениея собственостью 
        /// </summary>
        public virtual DateTime DataOwnerStart { get; set; }

        /// <summary>
        /// Дата конца владения собственностью  
        /// </summary>
        public virtual DateTime DataOwnerEdit { get; set; }

        /// <summary>
        /// Тип нарушителя для адресов 
        /// </summary>
        public virtual TypeViolatorOwnerRoom TypeViolatorOwnerRoom { get; set; }
    }
}
