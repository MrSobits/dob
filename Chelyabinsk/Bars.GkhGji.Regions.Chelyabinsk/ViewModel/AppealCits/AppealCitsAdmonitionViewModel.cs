namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Entities;
    using Bars.GkhGji.Entities;

    public class AppealCitsAdmonitionViewModel : BaseViewModel<AppealCitsAdmonition>
    {
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public override IDataResult List(IDomainService<AppealCitsAdmonition> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var isFiltered = baseParams.Params.GetAs<bool>("isFiltered");

            if (!isFiltered)
            {
                var data = domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    x.PerfomanceDate,
                    x.PerfomanceFactDate,
                    Contragent = x.Contragent.Name,
                    x.File,
                    Inspector = x.Inspector.Fio,
                    x.AnswerFile,
                    Executor = x.Executor.Fio,
                    x.DocumentNumber
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else
            {
                var appealCitsRealityObject = AppealCitsRealityObjectDomain.GetAll();

                var dateStart2 = loadParams.Filter.GetAs("dateStart", new DateTime());
                var dateEnd2 = loadParams.Filter.GetAs("dateEnd", new DateTime());

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");

                var data = domainService.GetAll()
                    .Join(
                        appealCitsRealityObject.AsEnumerable(),
                        x => x.AppealCits.Id,
                        y => y.AppealCits.Id,
                        (x, y) => new
                        {
                            x.Id,
                            x.DocumentName,
                            x.PerfomanceDate,
                            x.PerfomanceFactDate,
                            Contragent = x.Contragent.Name,
                            x.File,
                            Inspector = x.Inspector.Fio,
                            Municipality = y.RealityObject.Municipality.Name,
                            Address = y.RealityObject.Address,
                            x.AnswerFile,
                            Executor = x.Executor.Fio,
                            x.DocumentNumber
                        })
                    .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}