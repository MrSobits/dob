namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Владелец 
    /// </summary>
    public class Owner : BaseGkhEntity
    {

        /// <summary>
        /// Связь с контрагентом 
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Связь с транспортом  
        /// </summary>
        public virtual Transport Transport { get; set; }
       
        /// <summary>
        /// Связь с должностными лицами  
        /// </summary>
        public virtual ContragentContact ContragentContact { get; set; }

        /// <summary>
        /// Связь с физическими лицами  
        /// </summary>
        public virtual IndividualPerson IndividualPerson { get; set; }
      
        /// <summary>
        /// Дата начала владениея собственостью 
        /// </summary>
        public virtual DateTime DataOwnerStart { get; set; }
        
        /// <summary>
        /// Дата конца владения собственностью  
        /// </summary>
        public virtual DateTime DataOwnerEdit { get; set; }

        /// <summary>
        /// Тип нарушителя
        /// </summary>
        public virtual TypeViolator TypeViolator { get; set; }
        /// <summary>
        /// Наименование транспортного средства  
        /// </summary>
        public virtual string NameTransport { get; set; }

        /// <summary>
        ///  Номер транспортного средства  
        /// </summary>
        public virtual string NamberTransport { get; set; }

    }
}
