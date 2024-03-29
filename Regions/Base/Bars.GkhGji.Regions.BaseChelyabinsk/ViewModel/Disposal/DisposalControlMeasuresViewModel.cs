﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Disposal
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class DisposalControlMeasuresViewModel : BaseViewModel<DisposalControlMeasures>
    {
        public override IDataResult List(IDomainService<DisposalControlMeasures> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToInt()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.Disposal.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ControlMeasuresName = x.ControlActivity.Name,
                    x.DateStart,
                    x.DateEnd,
                    x.Description
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}