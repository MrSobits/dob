﻿namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;

    using Bars.GkhDi.Services.DataContracts.GetPeriods;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "ManOrgRealtyObjectInfo/{houseId}")]
        GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId);
    }
}