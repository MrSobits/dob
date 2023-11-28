namespace Bars.GkhDi.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.GkhDi.Services.DataContracts.GetPeriods;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPeriods")]
        GetPeriodsResponse GetPeriods();
    }
}