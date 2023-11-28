using System;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Информация о температурном графике. Если показатели качества указываются в разрезе договора, то ссылка на ОЖФ в данном элементе не заполняется и элемент может заполняться только если предмете договора хотя бы раз встречается ресурс "Тепловая энергия". Если показатели качества указываются в разрезе ОЖФ, то ссылка на ОЖФ обязательна и элемент заполняется только если в рамках ОЖФ встречается ресурс "Тепловая энергия".
    /// </summary>
    public class TemperatureChart
    {
        /// <summary>
        /// Ссылка на ОЖФ, обязательно заполняется, если показатели качества ведутся в разрезе ОЖФ
        /// </summary>
        public Guid AddressObjectKey { get; set; }

        /// <summary>
        /// Температура наружного воздуха
        /// </summary>
        public int OutsideTemperature { get; set; }

        /// <summary>
        /// Температура теплоносителя в подающем трубопроводе
        /// </summary>
        public decimal FlowLineTemperature { get; set; }

        /// <summary>
        /// Температура теплоносителя в обратном трубопроводе
        /// </summary>
        public decimal OppositeLineTemperature { get; set; }
    }
}
