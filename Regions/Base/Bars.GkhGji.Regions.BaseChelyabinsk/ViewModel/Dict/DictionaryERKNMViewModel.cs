namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Dict.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class DictionaryERKNMViewModel : BaseViewModel<DictionaryERKNM>
    {
        public override IDataResult List(IDomainService<DictionaryERKNM> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
          

            var data = domainService.GetAll()
				
                .Select(x => new
                {
                 x.DictionaryERKNMGuid,
                 x.Name,
                 x.Type,
                 x.Description,
                 x.Order,
                 x.Required,
                 x.DateLastUpdate,
                 x.EntityName,
                 x.EntityId
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}