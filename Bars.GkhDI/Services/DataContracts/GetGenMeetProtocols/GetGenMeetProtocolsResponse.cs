namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetGenMeetProtocolsResponse")]
    public class GetGenMeetProtocolsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Protocols")]
        public Protocol[] Protocols { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}