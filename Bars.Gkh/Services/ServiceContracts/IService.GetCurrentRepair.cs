namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.CurrentRepair;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCurrentRepair/{houseId}")]
        GetCurrentRepairResponse GetCurrentRepair(string houseId);
    }
}