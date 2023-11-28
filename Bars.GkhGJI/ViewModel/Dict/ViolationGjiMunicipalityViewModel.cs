namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;


    public class ViolationGjiMunicipalityViewModel : BaseViewModel<ViolationGjiMunicipality>
    {
        public override IDataResult List(IDomainService<ViolationGjiMunicipality> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var violationid = baseParams.Params.GetAs<long>("violationId");

            var data = domainService.GetAll().Where(x => x.ViolationGji.Id == violationid).Select(x => new
            {
               x.Id,
               Municipality = x.Municipality.Name,
               MunicipalityRegion = x.Municipality.RegionName,
            }).Filter(loadParam, this.Container);

            var totalCount = data.Count();


            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }

}