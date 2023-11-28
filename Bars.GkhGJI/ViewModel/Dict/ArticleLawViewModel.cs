namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ArticleLawViewModel : BaseViewModel<ArticleLawGji>
    {
        public override IDataResult List(IDomainService<ArticleLawGji> domainService, BaseParams baseParams)
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
                    x.OMS,
                    x.OmsRegion,
                    x.NameOMS,
                    x.KBK,
                    x.Part,
                    x.Article,
                    x.Bank
                })
                .Filter(loadParams, Container).OrderBy(x=> x.Article).ThenBy(x=> x.Part);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
