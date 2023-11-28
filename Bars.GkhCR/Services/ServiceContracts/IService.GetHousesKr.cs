namespace Bars.GkhCr.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesKr/{programId}")]
        GetHousesKrResponse GetHousesKr(string programId);
    }
}