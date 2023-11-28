using GisGkhLibrary.Entities.Dictionaries;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.House
{
    /// <summary>
    /// Подъезд
    /// </summary>
    public class Entrance
    {
        /// <summary>
        /// Идентификатор подъезда. Нужен для изменения данных о подъезде
        /// </summary>
        public Guid EntranceGUID { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        public string EntranceNum { get; set; }

        /// <summary>
        /// ГУИД дочернего дома по ФИАС, к которому относится подъезд для группирующих домов
        /// </summary>
        public Guid FIASChildHouse { get; set; }

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
