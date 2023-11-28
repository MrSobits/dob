using GisGkhLibrary.Enums.HouseMgmt;

namespace GisGkhLibrary.Entities.HouseMgmt.ObjectAddress
{
    /// <summary>
    /// Данные об объекте жилищного фонда
    /// </summary>
    public class ObjectAddress
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        public ObjectAddressType? HouseType { get; set; }

        /// <summary>
        /// Адрес дома. Глобальный уникальный идентификатор дома по ФИАС
        /// </summary>
        public string FIASHouseGuid { get; set; }

        /// <summary>
        /// Номер квартиры (помещения) / Номер блока
        /// </summary>
        public string ApartmentNumber { get; set; }

        /// <summary>
        /// Номер комнаты (указывается в случае квартиры коммунального заселения)
        /// </summary>
        public string RoomNumber { get; set; }
    }
}
