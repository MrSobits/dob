﻿namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class RegionCodeMVDViewModel : BaseViewModel<RegionCodeMVD>
    {
        public override IDataResult List(IDomainService<RegionCodeMVD> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            List<long> ids = null;

            if (baseParams.Params.ContainsKey("Id"))
            {
                ids = baseParams.Params.GetAs("Id", string.Empty).Split(',').Select(x => x.ToLong()).ToList();
            }

            var data = domainService.GetAll()
                .WhereIf(ids != null, x => ids.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
