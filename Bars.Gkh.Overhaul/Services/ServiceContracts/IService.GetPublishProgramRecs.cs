﻿namespace Bars.Gkh.Overhaul.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.Gkh.Overhaul.Services.DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPublishProgramRecs/{muId=null}")]
        GetPublishProgramRecsResponse GetPublishProgramRecs(string muId);
    }
}