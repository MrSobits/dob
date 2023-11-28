namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class ViolationGjiViewModel : ViolationGjiViewModel<ViolationGji>
    {
    }

    public class ViolationGjiViewModel<T> : BaseViewModel<T>
        where T : ViolationGji
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var violFeatureDomain = Container.ResolveDomain<ViolationFeatureGji>();
            var violActionsRemovDomain = Container.ResolveDomain<ViolationActionsRemovGji>();
            var violNormativeDocItemDomain = Container.ResolveDomain<ViolationNormativeDocItemGji>();

            try
            {
                var featDict = violFeatureDomain
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        Name = x.FeatureViolGji.FullName ?? x.FeatureViolGji.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

                var actRemViolDict = violActionsRemovDomain
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.ActionsRemovViol.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

                var data =
                    domainService.GetAll()
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.PpRf25,
                            x.PpRf307,
                            x.PpRf491,
                            x.CodePin,
                            x.PpRf170,
                            x.OtherNormativeDocs,
                            x.NormativeDocNames,
                            x.Municipality,
                            x.TypeMunicipality,
                            x.ArticleLaw,
                            x.ArticleLawRepeatative
                        })
                        .AsEnumerable()
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    Name = x.Name ?? string.Empty,
                                    PpRf25 = x.PpRf25 ?? string.Empty,
                                    PpRf307 = x.PpRf307 ?? string.Empty,
                                    PpRf491 = x.PpRf491 ?? string.Empty,
                                    CodePin = x.CodePin ?? string.Empty,
                                    FeatViol = featDict.Get(x.Id) ?? string.Empty,
                                    ActRemViol = actRemViolDict.Get(x.Id) ?? string.Empty,
                                    NormDocNum = x.NormativeDocNames ?? string.Empty,
                                    Municipality = x.Municipality,
                                    TypeMunicipality = x.TypeMunicipality,
                                    x.ArticleLaw,
                                    x.ArticleLawRepeatative
                                })
                        .AsQueryable()
                        .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(violFeatureDomain);
                Container.Release(violActionsRemovDomain);
                Container.Release(violNormativeDocItemDomain);
            }
        }
    }
}