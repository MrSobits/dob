namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetHouseServicesResponse")]
    public class GetHouseServicesResponse
    {
        [DataMember]
        [XmlArray(ElementName = "ManagServs")]
        public ManagServ[] ManagServs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "HousingServs")]
        public HousingServ[] HousingServs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ComServs")]
        public ComServ[] ComServs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "WorksTo")]
        public ServiceWorkTo[] WorksTo { get; set; }

        [DataMember]
        [XmlArray(ElementName = "WorksPpr")]
        public ServiceWorkPpr[] WorksPpr { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}