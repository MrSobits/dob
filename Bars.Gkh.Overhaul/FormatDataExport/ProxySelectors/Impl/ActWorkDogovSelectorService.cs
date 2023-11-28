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
    /// Сервис получения <see cref="ActWorkDogovProxy"/>
    /// </summary>
    public class ActWorkDogovSelectorService : BaseProxySelectorService<ActWorkDogovProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ActWorkDogovProxy> GetCache()
        {
            var performedWorkActRepository = this.Container.ResolveRepository<PerformedWorkAct>();

            using (this.Container.Using(performedWorkActRepository))
            {
                var municipalityList = this.SelectParams.GetAs("OperatorMunicipalityList", new List<long>());
                var isFiltred = this.SelectParams.GetAs<bool>("IsFiltred");

                return performedWorkActRepository.GetAll()
                    .WhereIf(isFiltred, x => municipalityList.Contains(x.Realty.Municipality.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        WorkName = x.TypeWorkCr.Work.Name,
                        FinanceSourceName = x.TypeWorkCr.FinanceSource.Name,
                        x.DocumentNum,
                        x.DateFrom,
                        x.Sum
                    })
                    .AsEnumerable()
                    .Select(x => new ActWorkDogovProxy
                    {
                        Id = x.Id,
                        Status = 1,
                        Name = !string.IsNullOrWhiteSpace(x.WorkName)
                            ? !string.IsNullOrWhiteSpace(x.FinanceSourceName)
                                ?$"{x.WorkName} ({x.FinanceSourceName})"
                                : x.WorkName
                            : string.Empty,
                        Number = x.DocumentNum,
                        Date = x.DateFrom,
                        Sum = x.Sum,
                        IsInstallments = 2
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}