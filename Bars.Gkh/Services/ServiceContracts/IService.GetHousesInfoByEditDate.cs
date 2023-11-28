namespace Bars.Gkh.Services.ServiceContracts
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Bars.Gkh.Services.DataContracts.GetHousesInfoByEditDate;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesInfoByEditDate/{date}")]
        GetHousesInfoByEditDateResponse GetHousesInfoByEditDate(string date);
    }
}