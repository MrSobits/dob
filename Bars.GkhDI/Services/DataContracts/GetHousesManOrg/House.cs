namespace Bars.GkhDi.Services.DataContracts.HousesManOrg
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "House", AnonymousType = true)]
    public class House
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember(IsRequired = false)]
        [XmlAttribute(AttributeName = "AreaRooms")]
        public string AreaRooms { get; set; }

        [XmlIgnore]
        public DateTime? _StartDate { get; set; }

        [XmlIgnore]
        public DateTime? _FinishDate { get; set; }
    }
}