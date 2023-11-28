namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "PeriodItem")]
    public class PeriodItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// КодПериода
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public long Code { get; set; }

        /// <summary>
        /// ДатаНачала 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        /// <summary>
        /// ДатаОкончания
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateEnd")]
        public string DateEnd { get; set; }

        /// <summary>
        /// УправляющиеОрганизации
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ManagingOrgs")]
        public ManagingOrgItem[] ManagingOrgs { get; set; }

        /// <summary>
        /// Сведения о коммунальных услугах
        /// </summary>
        [DataMember, XmlArray(ElementName = "HouseCommunalService")]
        public HouseCommunalService[] HouseCommunalService { get; set; }

        /// <summary>
        /// Сведения об использовании мест общего пользования
        /// </summary>
        [DataMember, XmlArray(ElementName = "HouseCommonProperty")]
        public HouseCommonProperty[] HouseCommonProperty { get; set; }

        /// <summary>
        /// Финансовые показатели
        /// </summary>
        [DataMember, XmlArray(ElementName = "HouseReportCommon")]
        public HouseReportCommon[] HouseReportCommon { get; set; }

        /// <summary>
        /// Лифты в доме
        /// </summary>
        [DataMember, XmlArray(ElementName = "HouseLift")]
        public HouseLift[] HouseLift { get; set; }


        [DataMember, XmlArray(ElementName = "HouseOverhaul")]
        public HouseOverhaul[] HouseOverhaul { get; set; }
    }
}