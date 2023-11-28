﻿namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using DataContracts.GetDocument;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetDocument/{fileId}")]
        GetDocumentResponse GetDocument(string fileId);
    }
}
