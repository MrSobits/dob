﻿namespace Bars.GkhCr.Services.ServiceContracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetProgKr/{houseId=0}/{usedInExport=null}")]
        GetProgKrResponse GetProgKr(string houseId, string usedInExport);
    }
}