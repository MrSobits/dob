using System;

namespace GisGkhLibrary.Entities.HouseMgmt.Volume
{
    /// <summary>
    /// Плановый объем и режим подачи за год в разрезе объекта жилищного фонда
    /// </summary>
    public class SupplyContractPlannedVolume : PlannedVolume
    {
        /// <summary>
        /// Ссылка на пару из коммунальной услуги и ресурса из предмета договора
        /// </summary>
        public Guid PairKey { get; set; }
    }
}
