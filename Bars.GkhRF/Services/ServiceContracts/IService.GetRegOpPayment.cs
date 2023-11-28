﻿namespace Bars.GkhRf.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.GkhRf.Services.DataContracts.GetRegOpPayment;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRegOpPayment/{houseId}")]
        GetRegOpPaymentResponse GetRegOpPayment(string houseId);
    }
}