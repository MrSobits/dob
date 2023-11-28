namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис получения <see cref="WorkDogovProxy"/>
    /// </summary>
    public class WorkDogovSelectorService : BaseProxySelectorService<WorkDogovProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, WorkDogovProxy> GetCache()
        {
            var buildContractTypeWorkRepository = this.Container.ResolveRepository<BuildContractTypeWork>();

            using (this.Container.Using(buildContractTypeWorkRepository))
            {
                var dogovorKprDict = this.ProxySelectorFactory.GetSelector<DogovorKprProxy>()
                    .ProxyListCache.Values
                    .Where(x => x.BuildContractId != null)
                    .Select(x => new
                    {
                        x.Id,
                        x.BuildContractId
                    })
                    .ToDictionary(x => x.BuildContractId, x => x.Id);

                return buildContractTypeWorkRepository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        BuildContractId = (long?) x.BuildContract.Id,
                        TypeWorkId = (long?) x.TypeWork.Id,
                        x.TypeWork.DateStartWork,
                        x.TypeWork.DateEndWork,
                        x.Sum,
                        TypeWorkSum = x.TypeWork.Sum,
                        x.TypeWork.Volume,
                        UnitMeasureName = x.TypeWork.Work.UnitMeasure.Name,
                        x.TypeWork.Work.Description
                    })
                    .AsEnumerable()
                    .Select(x => new WorkDogovProxy
                    {
                        Id = x.Id,
                        DogovorKprId = dogovorKprDict.Get(x.BuildContractId),
                        KprId = x.TypeWorkId,
                        StartDate = x.DateStartWork,
                        EndDate = x.DateEndWork,
                        ContractAmount = x.Sum,
                        KprAmount = x.TypeWorkSum,
                        WorkVolume = x.Volume,
                        AnotherUnit = x.UnitMeasureName,
                        Description = x.Description
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}