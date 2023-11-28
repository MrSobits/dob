using GisGkhLibrary.Entities.Dictionaries;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.House
{
    /// <summary>
    /// Жилой дом
    /// </summary>
    public class ApartmentHouse : HouseIdentifiersBase
    {
        /// <summary>
        /// Глобальный уникальный идентификатор дома по ФИАС
        /// </summary>
        public Guid FIAS { get; set; }

        /// <summary>
        /// ОКТМО (обязательное для всех территорий, за исключением города и космодрома "Байконур"). Значение из ФИАС при наличии.
        /// </summary>
        public string OKTMO { get; set; }
        public string OKTMOName { get; set; }

        /// <summary>
        /// Часовая зона
        /// </summary>
        public OlsonTZ OlsonTZ { get; set; }
    }
}
