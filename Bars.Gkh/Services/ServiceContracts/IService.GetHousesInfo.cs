namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.HousesInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesInfo/{houseIds}")]
        GetHousesInfoResponse GetHousesInfo(string houseIds);
    }
}