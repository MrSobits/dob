using GisGkhLibrary.Enums.HouseMgmt;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.Account
{
    /// <summary>
    /// Помещение
    /// </summary>
    public class Accommodation
    {
        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Тип идентификатора
        /// </summary>
        public AccommodationIdentifierType IdentifierType { get; set; }

        /// <summary>
        /// Доля внесения платы, размер доли в %
        /// </summary>
        public decimal? SharePercent { get; set; }
    }
}
