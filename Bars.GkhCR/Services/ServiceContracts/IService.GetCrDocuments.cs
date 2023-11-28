namespace Bars.GkhCr.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.GkhCr.Services.DataContracts.GetCrDocuments;

    public partial interface IService
    {
        /// <summary>
        /// Получить документы объекта КР
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="usedInExport"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCrDocuments/{houseId}/{programId}")]
        CrDocumentsResponse GetCrDocuments(string houseId, string programId);
    }
}