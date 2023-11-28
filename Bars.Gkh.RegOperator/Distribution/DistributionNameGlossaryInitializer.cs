namespace Bars.Gkh.RegOperator.Distribution
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Events;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Castle.Core.Internal;
    using Castle.MicroKernel.Lifestyle.Scoped;
    using NHibernate.Exceptions;

    public class DistributionNameGlossaryInitializer : EventHandlerBase<AppStartEventArgs>
    {
        private readonly IDomainService<MultipurposeGlossary> _glossaryDomainService;

        public DistributionNameGlossaryInitializer()
        {
            _glossaryDomainService = ApplicationContext.Current.Container.ResolveDomain<MultipurposeGlossary>();
        }

        private const string GlossaryCode = DistributionNameLocalizer.DistributionNameGlossaryCode;
        private const string GlossaryName = "Типы распределения счетов НВС";

        public override void OnEvent(AppStartEventArgs args)
        {
            ExplicitSessionScope.CallInScope(new DefaultLifetimeScope(), () =>
            {
                try
                {
                    var glossary = _glossaryDomainService.FirstOrDefault(x => x.Code == GlossaryCode);
                    if (glossary == null)
                    {
                        CreateGlossary();
                    }
                    else
                    {
                        UpdateGlossary(glossary);
                    }
                }
                catch (GenericADOException)
                {
                    // TODO: Log
                }
            });
        }

        private void CreateGlossary()
        {
            var glossary = new MultipurposeGlossary(GlossaryCode, GlossaryName);
            ApplicationContext.Current.Container.ResolveAll<IDistribution>().ForEach(x => glossary.AddItem(x.Code, x.Name));

            _glossaryDomainService.Save(glossary);
        }

        private void UpdateGlossary(MultipurposeGlossary glossary)
        {
            ApplicationContext.Current.Container.ResolveAll<IDistribution>().ForEach(x =>
            {
                if (glossary.Items.All(i => i.Key != x.Code))
                {
                    glossary.AddItem(x.Code, x.Name);
                }
            });

            _glossaryDomainService.Update(glossary);
        }
    }
}
