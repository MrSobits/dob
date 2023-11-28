namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class ManagingOrgRealityObjectViewModel : BaseViewModel<ManagingOrgRealityObject>
    {    
        public IManagingOrgRealityObjectService ManagingOrgRealityObjectService { get; set; }
        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public override IDataResult List(IDomainService<ManagingOrgRealityObject> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var activeContractsQuery = this.ManagingOrgRealityObjectService.GetAllActive()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.ManagingOrgJskTsj);

            var manorgLicenceQuery = this.ManOrgLicenseDomain.GetAll();

            var data = domain.GetAll()
                 .Where(x => x.ManagingOrganization.Id == manorgId)
                 .Select(x => new
                 {
                     x.Id,
                     x.RealityObject.Address,
                     Municipality = x.RealityObject.Municipality.Name,

                     ActiveManagingOrganization = activeContractsQuery
                        .Where(y => y.RealityObject == x.RealityObject)
                        .Select(y => y.ManOrgContract.ManagingOrganization.Contragent.ShortName)
                        .FirstOrDefault(),

                     ActiveInn = activeContractsQuery
                        .Where(y => y.RealityObject == x.RealityObject)
                        .Select(y => y.ManOrgContract.ManagingOrganization.Contragent.Inn)
                        .FirstOrDefault(),

                     ActiveDateStart = activeContractsQuery
                        .Where(y => y.RealityObject == x.RealityObject)
                        .Select(y => y.ManOrgContract.StartDate)
                        .FirstOrDefault(),

                     ActiveLicenseDate = manorgLicenceQuery
                        .Where(z => activeContractsQuery
                                .Where(y => y.RealityObject == x.RealityObject).Any(y => y.ManOrgContract.ManagingOrganization.Contragent == z.Contragent))
                                .Select(y => y.DateRegister)
                                .FirstOrDefault()
                 })
                 .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                 .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                 .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}