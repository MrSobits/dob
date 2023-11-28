namespace Bars.GkhDi.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetFundsInfo/{manOrgId},{periodId}")]
        GetFundsInfoResponse GetFundsInfo(string manOrgId, string periodId);
    }
}
