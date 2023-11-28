using System;
using System.Collections.Generic;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Информация о применяемом тарифе
    /// </summary>
    public class Tariff
    {
        /// <summary>
        /// Ссылки на ОЖФ
        /// </summary>
        public List<Guid> AddressObjectKeys { get; set; }

        /// <summary>
        /// Ссылка на пару из коммунальной услуги и ресурса из предмета договора
        /// </summary>
        public Guid PairKey { get; internal set; }

        /// <summary>
        /// Идентификатор дифференцированной цены тарифа
        /// </summary>
        public Guid PriceGUID { get; internal set; }
    }
}
