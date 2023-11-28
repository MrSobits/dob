using System.ServiceModel;

namespace Bars.GkhGji.Regions.Voronezh.Services.ServiceContracts
{
    using Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncAppealCit;

    /// <summary>
    /// Сервис обмена данными с АС ДОУ
    /// </summary>
    [ServiceContract]
    public interface IAppealCitService
    {
        /// <summary>
        /// Получение обращений граждан
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        AppealCitResult GetAppealCit(AppealCitRecord appealCitRecord, string token);

        [OperationContract]
        [XmlSerializerFormat]
        AppealCitPortalResult ImportPortalAppeal(PortalAppeal appealCitRecord, string token);

        [OperationContract]
        [XmlSerializerFormat]
        ReportResult SetReportState(ReportState reportState, string token);

        [OperationContract]
        [XmlSerializerFormat]
        AppealCitStateResult GetState(string Id, string token);
    }
}
