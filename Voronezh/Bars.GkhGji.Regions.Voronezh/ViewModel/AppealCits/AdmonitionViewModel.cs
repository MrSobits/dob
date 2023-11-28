namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;

    public class AdmonitionViewModel : BaseViewModel<AppealCitsAdmonition>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public override IDataResult List(IDomainService<AppealCitsAdmonition> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitsRealityObject = AppealCitsRealityObjectDomain.GetAll();

            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
            {
                var contragent = thisOperator.Contragent;
                var contragentList = OperatorContragentDomain.GetAll()
                 .Where(x => x.Contragent != null)
                 .Where(x => x.Operator == thisOperator)
                 .Select(x => x.Contragent.Id).Distinct().ToList();
                if (contragent != null)
                {
                    if (!contragentList.Contains(contragent.Id))
                    {
                        contragentList.Add(contragent.Id);
                    }
                }

                var data = domainService.GetAll()
                    .Where(x=> contragentList.Contains(x.Contragent.Id))
               .Join(appealCitsRealityObject.AsEnumerable(), x => x.AppealCits.Id, y => y.AppealCits.Id, (x, y) => new
               {
                   x.Id,
                   x.DocumentName,
                   x.PerfomanceDate,
                   x.PerfomanceFactDate,
                   Contragent = x.Contragent.Name,
                   x.File,
                   x.SignedFile,
                   x.AppealCits.Number,
                   x.Signature,
                   Inspector = x.Inspector.Fio,
                   Municipality = y.RealityObject.Municipality.Name,
                   Address = y.RealityObject.Address,
                   x.AnswerFile,
                   x.KindKNDGJI,
                   x.SignedAnswerFile,
                   x.AnswerSignature,
                   Executor = x.Executor.Fio,
                   x.DocumentNumber,
                   x.DocumentDate
               })
               .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {

                var data = domainService.GetAll()
                .Join(appealCitsRealityObject.AsEnumerable(), x => x.AppealCits.Id, y => y.AppealCits.Id, (x, y) => new
                {
                    x.Id,
                    x.DocumentName,
                    x.PerfomanceDate,
                    x.PerfomanceFactDate,
                    Contragent = x.Contragent.Name,
                    x.File,
                    x.SignedFile,
                    x.Signature,
                    Inspector = x.Inspector.Fio,
                    Municipality = y.RealityObject.Municipality.Name,
                    Address = y.RealityObject.Address,
                    x.AnswerFile,
                    x.SignedAnswerFile,
                    x.AnswerSignature,
                    x.KindKNDGJI,
                    Executor = x.Executor.Fio,
                    x.DocumentNumber,
                    x.DocumentDate
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}