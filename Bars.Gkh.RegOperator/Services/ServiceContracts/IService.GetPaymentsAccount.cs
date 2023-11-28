namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.RegOperator.Services.DataContracts.PaymentsAccount;

    public partial interface IService
    {
        /// <summary>
        /// Оплаты по лицеовму счету
        /// </summary>
        /// <param name="accountNum"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPaymentsAccount/{ExNum}")]
        PaymentsAccountResponse GetPaymentsAccount(string accountNum);
    }
}