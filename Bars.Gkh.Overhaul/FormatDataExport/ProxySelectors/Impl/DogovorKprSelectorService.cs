namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис получения <see cref="DogovorKprProxy"/>
    /// </summary>
    public class DogovorKprSelectorService : BaseProxySelectorService<DogovorKprProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DogovorKprProxy> GetCache()
        {
            var contractCrRepository = this.Container.ResolveRepository<ContractCr>();
            var buildContractRepository = this.Container.ResolveRepository<BuildContract>();

            using (this.Container.Using(contractCrRepository, buildContractRepository))
            {
                var contragentList = this.SelectParams.GetAs("OperatorContragentList", new List<long>());
                var isFiltred = this.SelectParams.GetAs<bool>("IsFiltred");

                var contractCrData = contractCrRepository.GetAll()
                    .WhereIf(isFiltred, x => contragentList.Contains(x.Contragent.Id))
                    .WhereNotNull(x => x.ObjectCr.ProgramCr)
                    .Select(x => new DogovorKprProxy
                    {
                        Id = x.ExportId,
                        ContractCrId = x.Id,
                        KprId = x.ObjectCr.ProgramCr.Id,
                        DocumentNumber = x.DocumentNum,
                        DocumentDate = x.DateFrom,
                        StartDate = x.DateStartWork,
                        EndDate = x.DateEndWork,
                        Sum = x.SumContract,
                        ExecutantContragentId = x.Contragent.Id,
                        IsGuaranteePeriod = 2, // Передавать всегда "2"

                        File = x.File,
                        FileType = 1,

                        ObjectCrId = x.ObjectCr.Id
                    })
                    .AsEnumerable();
                var buildContractData = buildContractRepository.GetAll()
                    .WhereIf(isFiltred, x => contragentList.Contains(x.Contragent.Id))
                    .WhereNotNull(x => x.ObjectCr.ProgramCr)
                    .Select(x => new DogovorKprProxy
                    {
                        Id = x.ExportId,
                        BuildContractId = x.Id,
                        KprId = x.ObjectCr.ProgramCr.Id,
                        DocumentNumber = x.DocumentNum,
                        DocumentDate = x.DocumentDateFrom,
                        StartDate = x.DateStartWork,
                        EndDate = x.DateEndWork,
                        Sum = x.Sum,
                        CustomerContragentId = x.Contragent.Id,
                        ExecutantContragentId = x.Builder.Contragent.Id,
                        IsGuaranteePeriod = 2, // Передавать всегда "2"

                        File = x.DocumentFile,
                        FileType = 1,

                        ObjectCrId = x.ObjectCr.Id
                    })
                    .AsEnumerable();

                return contractCrData.Union(buildContractData)
                    .ToDictionary(x => x.Id);
            }
        }
    }
}