namespace Bars.GkhDi.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "ManOrgInfo/{manOrgId}")]
        GetManOrgInfoResponse GetManOrgInfo(string manOrgId);
    }
}
