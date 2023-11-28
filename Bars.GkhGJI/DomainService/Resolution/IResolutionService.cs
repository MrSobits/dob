namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IResolutionService
    {
        IDataResult GetInfo(long? documentId);

        IDataResult ListIndividualPerson(BaseParams baseParams);

        IDataResult ListResolutionIndividualPerson(BaseParams baseParams);
        IDataResult ListView(BaseParams baseParams);

        IQueryable<ViewResolution> GetViewList();

        /// <summary>
        /// Получение распоряжений для раскрытия
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetResolutionInfo(BaseParams baseParams);

        string GetTakingDecisionAuthorityName();
    }
}