namespace Bars.GkhRf.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.GkhRf.Services.DataContracts.GetObjectInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "ObjectInfo/{houseId}")]
        GetObjectInfoResponse GetObjectInfo(string houseId);
    }
}