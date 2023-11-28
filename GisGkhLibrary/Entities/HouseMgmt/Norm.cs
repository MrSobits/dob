using System;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Информация о нормативе потребления коммунальной услуги
    /// </summary>
    public class Norm
    {
        /// <summary>
        /// Ссылка на ОЖФ. Если null, норматив применяется для всех ОЖФ договора
        /// </summary>
        public Guid? AddressObjectKey { get; set; }

        /// <summary>
        /// Ссылка на пару из коммунальной услуги и ресурса из предмета договора
        /// </summary>
        public Guid PairKey { get; internal set; }

        /// <summary>
        /// Идентификатор норматива потребления коммунальной услуги
        /// </summary>
        public Guid NormGUID { get; internal set; }
    }
}
