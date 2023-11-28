using GisGkhLibrary.Entities.Dictionaries;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.House
{
    /// <summary>
    /// Жилая комната
    /// </summary>
    public class LivingRoom : HouseIdentifiersBase
    {
        /// <summary>
        /// Идентификатор комнаты. Нужен для изменения данных о комнате
        /// </summary>
        public Guid LivingRoomGUID { get; set; }

        /// <summary>
        /// Номер комнаты
        /// </summary>
        public string RoomNumber { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public decimal? Square { get; set; }

        /// <summary>
        /// Причина аннулирования объекта жилищного фонда (НСИ 330)
        /// </summary>
        public AnnulmentReason AnnulmentReason { get; set; }

        /// <summary>
        /// Причина аннулирования. Дополнительная информация
        /// </summary>
        public string AnnulmentInfo { get; set; }
    }
}
