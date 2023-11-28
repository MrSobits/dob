namespace Bars.Gkh.Services.ServiceContracts
{
    using Bars.Gkh.Services.DataContracts.GetManOrgsByDate;
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRoManOrgsByDate/{date}")]
        GetRoManOrgsResponseByDate GetRoManOrgsByDate(string date);
    }
}