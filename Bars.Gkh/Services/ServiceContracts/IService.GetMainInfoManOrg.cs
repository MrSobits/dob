namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetMainInfoManOrg/{moId}")]
        GetMainInfoManOrgResponse GetMainInfoManOrg(string moId);
    }
}