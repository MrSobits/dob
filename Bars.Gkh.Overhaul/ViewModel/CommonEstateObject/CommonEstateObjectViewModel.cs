﻿namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class CommonEstateObjectViewModel : BaseViewModel<CommonEstateObject>
    {
        public override IDataResult List(IDomainService<CommonEstateObject> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var hideNotInlcuded = baseParams.Params.GetAs<bool>("hideNotIncluded");

            var ids = baseParams.Params.GetAs("Id", string.Empty).Split(',').Select(x => x.To<long>()).ToArray();

            var data = domainService.GetAll()
                .WhereIf(ids.Any(y => y != 0), x => ids.Contains(x.Id))
                .WhereIf(hideNotInlcuded, x => x.IncludedInSubjectProgramm)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
