namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Bars.Gkh.RegOperator.Services.DataContracts.GetChargePeriods;

    public partial interface IService
    {

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPaymentDocument/{accountNumber},{periodId},{saveSnapshot}")]
        PaymentDocumentResponse GetPaymentDocument(string accountNumber, string periodId, bool saveSnapshot);
    }
}
