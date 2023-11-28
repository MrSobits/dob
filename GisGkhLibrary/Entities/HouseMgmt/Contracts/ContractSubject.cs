using GisGkhLibrary.Entities.Dictionaries;
using GisGkhLibrary.Entities.HouseMgmt.Volume;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Предмет договора
    /// </summary>
    public class ContractSubject
    {
        /// <summary>
        /// Id пары услуги и ресурса
        /// </summary>
        public Guid PairKey { get; set; }

        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// Тарифицируемый ресурс
        /// </summary>
        public RatedResource RatedResource { get; set; }

        /// <summary>
        /// Дата начала поставки ресурса
        /// </summary>
        public DateTime StartSupplyDate { get; set; }

        /// <summary>
        /// Дата окончания поставки ресурса. Является обязательным, если указано значение в AutomaticRollOverOneYear
        /// </summary>
        public DateTime? EndSupplyDate { get; set; }

        /// <summary>
        /// Плановый объем и режим подачи за год
        /// </summary>
        public PlannedVolume PlannedVolume { get; set; }

    }
}
