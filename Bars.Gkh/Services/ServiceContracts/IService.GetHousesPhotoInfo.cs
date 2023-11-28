namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.GetHousesPhotoInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesPhotoInfo/{houseIds}")]
        GetHousesPhotoInfoResponse GetHousesPhotoInfo(string houseIds);
    }
}