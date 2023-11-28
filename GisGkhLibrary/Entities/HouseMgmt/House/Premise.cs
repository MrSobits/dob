using GisGkhLibrary.Entities.Dictionaries;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.House
{
    /// <summary>
    /// Помещение
    /// </summary>
    public abstract class Premise : HouseIdentifiersBase
    {
        /// <summary>
        /// Идентификатор помещения. Нужен для изменения данных о помещении
        /// </summary>
        public Guid PremisesGUID { get; set; }

        /// <summary>
        /// ГУИД дочернего дома по ФИАС, к которому относится подъезд для группирующих домов
        /// </summary>
        public Guid FIASChildHouse { get; set; }

        /// <summary>
        /// Общая площадь помещения по паспорту помещения
        /// </summary>
        public decimal? TotalArea { get; set; }
        
        /// <summary>
        /// Номер помещения
        /// </summary>
        public string RoomNumber { get; set; }

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
