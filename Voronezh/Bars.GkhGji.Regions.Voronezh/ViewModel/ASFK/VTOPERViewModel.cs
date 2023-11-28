namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using System.Linq;

    public class VTOPERViewModel : BaseViewModel<VTOPER>
    {
        public override IDataResult List(IDomainService<VTOPER> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new 
                { 
                    x.Id,
                    x.GUID,
                    x.KodDoc,
                    x.NomDoc,
                    x.DateDoc,
                    x.KodDocAdb,
                    x.NomDocAdb,
                    x.DateDocAdb,
                    x.SumIn,
                    x.SumOut,
                    x.SumZach,
                    x.Note,                  
                    x.TypeKbk,
                    x.Kbk,
                    x.AddKlass,
                    x.Okato,
                    x.InnAdb,
                    x.KppAdb
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
