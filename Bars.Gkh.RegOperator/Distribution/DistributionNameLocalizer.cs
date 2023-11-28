namespace Bars.Gkh.RegOperator.Distribution
{
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Castle.Windsor;

    public static class DistributionNameLocalizer
    {
        public const string DistributionNameGlossaryCode = "distribution_type";
        private static readonly IWindsorContainer Container = ApplicationContext.Current.Container;

        public static string GetLocalizedName(IDistribution distribution, string defaultName)
        {
            ArgumentChecker.NotNull(distribution, nameof(distribution));
            var glossaryDomain = Container.ResolveDomain<MultipurposeGlossary>();
            using (Container.Using(glossaryDomain))
            {
                var glossary = glossaryDomain.FirstOrDefault(x => x.Code == DistributionNameGlossaryCode);
                if (glossary == null)
                {
                    return defaultName;
                }

                return glossary.Contains(distribution.Code) ? glossary.GetItemValue(distribution.Code) : defaultName;
            }
        }
    }
}
