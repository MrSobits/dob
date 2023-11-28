namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    /// <summary>
    /// Справочник Транспортных средств и их владельцы 
    /// </summary>
    public class Transport : BaseGkhEntity
    {
        /// <summary>
        /// Наименование транспортного средства  
        /// </summary>
        public virtual string  NameTransport { get; set; }

        /// <summary>
        ///  Номер транспортного средства  
        /// </summary>
        public virtual string NamberTransport { get; set; }

        /// <summary>
        /// Регистрционный номер транспортного средства  
        /// </summary>
        public virtual int? RegistrationNamberTransport { get; set; }

        /// <summary>
        /// Серия номера транспортного средства 
        /// </summary>
        public virtual string SeriesTransport { get; set; }

        /// <summary>
        /// Регион регистрации номера транспортного средства 
        /// </summary>
        public virtual int? RegNamberTransport { get; set; }
       

    }
}
