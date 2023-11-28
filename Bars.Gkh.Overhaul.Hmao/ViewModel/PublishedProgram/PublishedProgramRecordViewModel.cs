namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Gkh.Utils;

    /// <summary>
    /// Представление для сущности Запись Опубликованной программы
    /// </summary>
    public class PublishedProgramRecordViewModel : BaseViewModel<PublishedProgramRecord>
    {
        /// <summary>
        /// Домен-сервис для сущности Запись в версии программы
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        /// <inheritdoc/>
        public override IDataResult List(IDomainService<PublishedProgramRecord> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var moId = 0M;
            var versionId = loadParam.Filter.GetAs<long>("versionId");
            if (versionId == 0)
            {
                moId = baseParams.Params.GetAs<long>("mo_id");
            }

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            decimal summary;
            int cnt;

            if (groupByRoPeriod == 0)
            {
                // Поулчаем опубликованную программу по основной версии
                var newData =
                    domainService.GetAll()
                        .WhereIf(moId > 0, x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                        .WhereIf(versionId > 0, x => x.PublishedProgram.ProgramVersion.Id == versionId)
                        .Select(x => new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address, 
                            x.PublishedYear,
                            x.Sum,
                            x.CommonEstateobject, 
                            x.IndexNumber
                        })
                        .Filter(loadParam, this.Container);

                summary = newData.Select(x => x.Sum).AsEnumerable().Sum();

                cnt = newData.Count();

                newData = newData
                    .OrderBy(x => x.IndexNumber)
                    .ThenBy(x => x.PublishedYear)
                    .Order(loadParam)
                    .Paging(loadParam);

                return new ListSummaryResult(newData, cnt, new { Sum = summary });
            }
            else
            {
                var dataPublished =
                    domainService.GetAll()
                        .WhereIf(moId > 0, x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                        .WhereIf(versionId > 0, x => x.PublishedProgram.ProgramVersion.Id == versionId)
                        .Where(x => x.Stage2 != null)
                        .Select(x => new
                        {
                            x.Stage2.Stage3Version.Id,
                            x.Sum,
                            x.IndexNumber,
                            x.PublishedYear
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => new {
                            PublishedYear = y.Select(z => z.PublishedYear).FirstOrDefault(),
                            Sum = y.Select(z => z.Sum).Sum(),
                            IndexNumber = y.Select(z => z.IndexNumber).FirstOrDefault(),
                        });

                var query = this.VersionRecordDomain.GetAll()
                        .WhereIf(moId > 0, x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == moId)
                        .WhereIf(versionId > 0, x => x.ProgramVersion.Id == versionId)
                        .Where(x => domainService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(x => new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            CommonEstateobject = x.CommonEstateObjects
                        })
                        .AsEnumerable();

                var newData =
                    query
                        .Select(x => new
                        {
                            x.Id,
                            x.Municipality,
                            x.RealityObject,
                            PublishedYear = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id].PublishedYear : 0,
                            Sum = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id].Sum : 0m,
                            x.CommonEstateobject,
                            IndexNumber = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id].IndexNumber : 0
                        })
                        .AsQueryable()
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.PublishedYear)
                        .Filter(loadParam, this.Container);

                summary = newData.Sum(x => x.Sum);
                cnt = newData.Count();

                return new ListSummaryResult(newData.Order(loadParam).Paging(loadParam), cnt, new { Sum = summary });
            }
        }
    }
}