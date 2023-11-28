namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.GetEmergencyObjectList;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetEmergencyObjectList/{Id}")]
        GetEmergencyObjectListResponse GetEmergencyObjectList(string Id);
    }
}