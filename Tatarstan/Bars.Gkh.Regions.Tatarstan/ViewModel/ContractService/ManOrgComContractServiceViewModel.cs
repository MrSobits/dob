﻿namespace Bars.Gkh.Regions.Tatarstan.ViewModel.ContractService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Вьюмодель для "ManOrgComContractService"
    /// </summary>
    public class ManOrgComContractServiceViewModel : BaseViewModel<ManOrgComContractService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ManOrgComContractService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var contractId = loadParams.Filter.GetAsId("contractId");

            return domainService.GetAll()
                .Where(x => x.Contract.Id == contractId)
                .Select(x => new
                {
                    x.Id,
                    x.Service.Name,
                    x.StartDate,
                    x.EndDate
                })
                .ToListDataResult(loadParams);
        }
    }
}