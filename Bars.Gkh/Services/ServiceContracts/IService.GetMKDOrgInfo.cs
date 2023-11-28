namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.GetMkdOrgInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetMkdOrgInfo/{roId}")]
        GetMkdOrgInfoResponse GetMkdOrgInfo(string roId);
    }
}