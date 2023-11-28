namespace Bars.GkhDi.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetContractsTsj/{houseId},{periodId}")]
        GetContractsTsjResponse GetContractsTsj(string houseId, string periodId);
    }
}
