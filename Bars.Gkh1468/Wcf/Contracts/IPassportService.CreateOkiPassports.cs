namespace Bars.Gkh1468.Wcf
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IPassportService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "CreateOkiPassports/{year},{month}")]
        string CreateOkiPassports(string year, string month);
    }
}