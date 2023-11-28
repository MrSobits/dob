using GisGkhLibrary.Entities.HouseMgmt.Volume;
using GisGkhLibrary.Enums.HouseMgmt;
using System.Collections.Generic;

namespace GisGkhLibrary.Entities.HouseMgmt.ObjectAddress
{
    /// <summary>
    /// Данные об объекте жилищного фонда в разрезе договора ресурсоснабжения
    /// </summary>
    public class ObjectAddressContract : ObjectAddress
    {
        /// <summary>
        /// Пара: коммунальная услуга и коммунальный ресурс
        /// </summary>
        public List<Pair> Pairs { get; set; }

        /// <summary>
        /// Плановый объем и режим подачи за год. Может быть заполнено только если плановый объем и режим подачи ведется в разрезе ОЖФ
        /// </summary>
        public List<SupplyContractPlannedVolume> PlannedVolumes { get; set; }

        /// <summary>
        /// Размещение информации о начислениях за коммунальные услуги осуществляет:
        ///R(SO)- РСО.
        ///P(roprietor)-Исполнитель коммунальных услуг.
        ///Заполняется, если порядок размещения информации о начислениях за коммунальные услуги ведется в разрезе договора
        /// </summary>
        public CountingResource? CountingResource { get; set; }

        /// <summary>
        /// Размещение информации об индивидуальных приборах учета и их показаниях осуществляет ресурсоснабжающая организация. Обязательно для заполнения, если в tns:CountingResource указано"РСО" . В остальных случаях не заполняется.
        /// </summary>
        public bool? MeteringDeviceInformation { get; set; }
    }
}
