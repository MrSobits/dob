namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCommonInfoMkd/{year}")]
        CommonInfoMkd GetCommonInfoMkd(string year);
    }
}
