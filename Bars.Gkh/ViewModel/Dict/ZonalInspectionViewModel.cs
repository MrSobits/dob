namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class ZonalInspectionViewModel : BaseViewModel<ZonalInspection>
    {
        public override IDataResult List(IDomainService<ZonalInspection> domain, BaseParams baseParams)
        {
            var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            try
            {
                var ids = baseParams.Params.GetAs("Id", string.Empty);
                var listIds = !string.IsNullOrWhiteSpace(ids)
                    ? ids.Split(',').Select(id => id.ToLong()).ToArray()
                    : new long[0];

                var inspectorIds = baseParams.Params.GetAs("inspectorIds", string.Empty);
                var inspectorIdsList = !string.IsNullOrWhiteSpace(inspectorIds)
                    ? inspectorIds.Split(',').Select(id => id.ToLong()).ToArray()
                    : new long[0];

                var zonalInspectionInspectorsList = zonalInspectionInspectorDomain.GetAll()
                    .Where(x => inspectorIdsList.Contains(x.Inspector.Id))
                    .Select(x => x.ZonalInspection.Id).ToList();

                return domain.GetAll()
                    .WhereIf(listIds.IsNotEmpty(), x => listIds.Contains(x.Id))
                    .WhereIf(zonalInspectionInspectorsList.IsNotEmpty(), x => zonalInspectionInspectorsList.Contains(x.Id))
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
            finally
            {
                this.Container.Release(zonalInspectionInspectorDomain);
            }
        }
    }
}