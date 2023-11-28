namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.EmergencyObjectInfo;
    using Bars.Gkh.Services.DataContracts.HousesInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetEmergencyObjectInfo/{Id=null}")]
        GetEmergencyObjectInfoResponse GetEmergencyObjectInfo(string Id);
    }
}