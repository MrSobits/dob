namespace Bars.Gkh.Overhaul.Hmao.ViewModel.Version
{
    using System.Linq;
    using B4;
    using Entities.Version;

    public class VersionActualizeLogViewModel : BaseViewModel<VersionActualizeLog>
    {
        public override IDataResult List(IDomainService<VersionActualizeLog> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var versionId = loadParams.Filter.GetAs<long>("versionId");

           var data =
                domainService.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserName,
                        x.ActualizeType,
                        DateAction = x.DateAction.ToUniversalTime(),
                        x.CountActions,
                        x.ProgramCrName,
                        x.LogFile
                    })
                    .ToList()
                    .AsQueryable()
                    .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}