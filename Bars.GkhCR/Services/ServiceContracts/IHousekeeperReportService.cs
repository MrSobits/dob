using System.ServiceModel;

namespace Bars.GkhCr.Services.ServiceContracts
{
    using Bars.Gkh.Services.DataContracts.RealityObjectHousekeeper;

    public partial interface IService
    {
        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        InsertHousekeeperReportResponce InsertHousekeeperReport(HousekeeperReportProxy report, string token);

        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        InsertBuildControlReportResponce InsertBuildControlReport(BuildControlReportProxy report, string token);

        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetHousekeeperReportResponce GetHousekeeperReport(string Login, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        CheckBuildControlUserResponce CheckBuildControlUser(string Login, string PasswordHash, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetBuildControlObjectsResponce GetBuildControlObjects(long userId, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetBuildControlObjectResponce GetBuildControlObject(long objectId, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetBuildControlReportListResponce GetBuildControlReportList(long objectId, string token);

        /// <summary>
        /// Импорт сведений об обращении граждан по объекту капитального ремонта
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetHousekeeperReportResponce GetCrObjectReport(long crId, string token);

        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        ObjectCRMobileResponce GetObjectsCRMobile(long RealityId, string token);
    }
}
