using System;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Пара: коммунальная услуга и коммунальный ресурс
    /// </summary>
    public class Pair
    {
        /// <summary>
        /// Ссылка на пару из коммунальной услуги и ресурса из предмета договора
        /// </summary>
        public Guid PairKey { get; set; }

        /// <summary>
        /// Дата начала поставки ресурса
        /// </summary>
        public DateTime StartSupplyDate { get; set; }

        /// <summary>
        /// Дата окончания поставки ресурса. Является обязательным, если указано значение в AutomaticRollOverOneYear
        /// </summary>
        public DateTime? EndSupplyDate { get; set; }

        /// <summary>
        /// Тип системы теплоснабжения (заполняется для коммунальных ресурсов "Тепловая энергия" и "Горячая вода")
        /// </summary>
        public HeatingSystemType HeatingSystemType { get; set; }
    }
}
