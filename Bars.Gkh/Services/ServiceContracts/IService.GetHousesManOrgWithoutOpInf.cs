namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Services.DataContracts.GetHousesManOrgWithoutOpInf;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesManOrgWithoutOpInf/{moId},{startDateD}-{startDateM}-{startDateY},{endDateD}-{endDateM}-{endDateY}")]
        GetHousesManOrgWithoutOpResponse GetHousesManOrgWithoutOpInf(string moId, string startDateD, string startDateM, string startDateY, string endDateD, string endDateM, string endDateY);
    }
}