using GisGkhLibrary.Entities.Dictionaries;
using System.Collections.Generic;

namespace GisGkhLibrary.Entities.HouseMgmt.House
{
    /// <summary>
    /// Жилое помещение
    /// </summary>
    public class ResidentialPremise : Premise
    {
        /// <summary>
        /// Номер подъезда. null или пустая строка, если отсутствует
        /// </summary>
        public string EntranceNum { get; set; }

        /// <summary>
        /// Характеристика помещения (НСИ 30)
        /// </summary>
        public ResidentPremiseType PremisesCharacteristic { get; set; }

        /// <summary>
        /// Комнаты, которые нужно добавить
        /// </summary>
        public IEnumerable<LivingRoom> LivingRoomToCreate;

        /// <summary>
        /// Комнаты, которые нужно изменить
        /// </summary>
        public IEnumerable<LivingRoom> LivingRoomToUpdate;
    }
}
