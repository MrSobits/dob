namespace Bars.GkhCr.Initializers
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
    using Localizers;

    public class TypeContractGlossaryInitializer : EventHandlerBase<AppEventArgsBase>
    {
        private readonly IDomainService<MultipurposeGlossary> _glossaryDomainService;

        public TypeContractGlossaryInitializer()
        {
            _glossaryDomainService = ApplicationContext.Current.Container.ResolveDomain<MultipurposeGlossary>();
        }

        private const string GlossaryCode = TypeContractLocalizer.GlossaryCode;
        private const string GlossaryName = "Тип договора объекта КР";

        public override void OnEvent(AppEventArgsBase args)
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
                catch
                {
                }
            });
        }

        private void CreateGlossary()
        {
            var glossary = new MultipurposeGlossary(GlossaryCode, GlossaryName);
            var items = TypeContractLocalizer.GetDefaultItems();
            items.ForEach(x => glossary.AddItem(x.Key, x.Value));

            _glossaryDomainService.Save(glossary);
        }

        private void UpdateGlossary(MultipurposeGlossary glossary)
        {
            TypeContractLocalizer.GetDefaultItems().ForEach(x =>
            {
                if (glossary.Items.All(i => i.Key != x.Key))
                {
                    glossary.AddItem(x.Key, x.Value);
                }
            });

            _glossaryDomainService.Update(glossary);
        }
    }
}
