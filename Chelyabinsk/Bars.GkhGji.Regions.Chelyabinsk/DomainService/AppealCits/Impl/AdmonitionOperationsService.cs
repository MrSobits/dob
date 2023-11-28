namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;  

    using System.Linq;
    using Bars.B4;
    using Castle.Windsor;

    public class AdmonitionOperationsService : IAdmonitionOperationsService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListDocsForSelect(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var docsServ = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();

            var data = docsServ.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentName,
                    x.DocumentDate
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();
            Container.Release(docsServ);

            return new ListDataResult(data.ToArray(), totalCount);
        }



    }
}