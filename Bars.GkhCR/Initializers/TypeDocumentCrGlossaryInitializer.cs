namespace Bars.GkhCr.Initializers
{
	using Bars.B4;
	using Bars.B4.Application;
	using Bars.B4.DataAccess;
	using Bars.B4.Events;
	using Bars.B4.IoC.Lifestyles.SessionLifestyle;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities.Dicts.Multipurpose;
	using Bars.GkhCr.Localizers;
	using Castle.MicroKernel.Lifestyle.Scoped;
	using System.Linq;

	public class TypeDocumentCrGlossaryInitializer : EventHandlerBase<AppStartEventArgs>
	{
		private readonly IDomainService<MultipurposeGlossary> _glossaryDomainService;

		public TypeDocumentCrGlossaryInitializer()
		{
			_glossaryDomainService = ApplicationContext.Current.Container.ResolveDomain<MultipurposeGlossary>();
		}

		private const string GlossaryCode = TypeDocumentCrLocalizer.GlossaryCode;
		private const string GlossaryName = "Типы протоколов, актов объекта КР";

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
				catch
				{
				}
			});
		}

		private void CreateGlossary()
		{
			var glossary = new MultipurposeGlossary(GlossaryCode, GlossaryName);
			var items = TypeDocumentCrLocalizer.GetDefaultItems();
			items.ForEach(x => glossary.AddItem(x.Key, x.Value));

			_glossaryDomainService.Save(glossary);
		}

		private void UpdateGlossary(MultipurposeGlossary glossary)
		{
			TypeDocumentCrLocalizer.GetDefaultItems().ForEach(x =>
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