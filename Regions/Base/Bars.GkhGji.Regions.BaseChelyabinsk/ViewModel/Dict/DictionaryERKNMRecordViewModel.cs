namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Dict.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class DictionaryERKNMRecordViewModel : BaseViewModel<DictionaryERKNMRecord>
    {
        public override IDataResult List(IDomainService<DictionaryERKNMRecord> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
          

            var data = domainService.GetAll()
				
                .Select(x => new
                {
                 x.DictionaryERKNM,
                 x.RecId,
                 x.Name,
                 x.Name1,
                 x.Name2,
                 x.EntityName,
                 x.EntityId
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}