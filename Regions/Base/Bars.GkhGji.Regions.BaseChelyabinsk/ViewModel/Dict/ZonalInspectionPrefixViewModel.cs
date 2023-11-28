namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Entities.Dicts;

    public class ZonalInspectionPrefixViewModel : BaseViewModel<ZonalInspectionPrefix>
    {
        public override IDataResult Get(IDomainService<ZonalInspectionPrefix> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var zonalInspectionId = baseParams.Params.GetAsId("zonalInspectionId");

            if (id != 0)
            {
                return new BaseDataResult(domainService.GetAll().FirstOrDefault(x => x.Id == id));
            }

            return new BaseDataResult(domainService.GetAll().FirstOrDefault(x => x.ZonalInspection.Id == zonalInspectionId));
        }
    }
}