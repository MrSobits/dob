namespace Bars.GkhDi.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPlanReductionExpense/{houseId},{periodId}")]
        GetPlanReductionExpenseResponse GetPlanReductionExpense(string houseId, string periodId);
    }
}
