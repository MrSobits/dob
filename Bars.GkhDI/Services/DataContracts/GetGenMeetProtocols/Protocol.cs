namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Protocol")]
    public class Protocol
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public long IdFile { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameFile")]
        public string NameFile { get; set; }
    }
}