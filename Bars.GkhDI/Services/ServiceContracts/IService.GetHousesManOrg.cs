namespace Bars.GkhDi.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.GkhDi.Services.DataContracts.HousesManOrg;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesManOrg/{moId},{periodId}")]
        GetHousesManOrgResponse GetHousesManOrg(string moId, string periodId);
    }
}