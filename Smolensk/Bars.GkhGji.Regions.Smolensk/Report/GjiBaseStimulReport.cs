namespace Bars.GkhGji.Regions.Smolensk.Report
{
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Gkh.Entities;
    using Gkh.Report;
    using GkhGji.Entities;
    using GkhGji.Enums;

    public abstract class GjiBaseStimulReport : GkhBaseStimulReport
    {
        public IDomainService<DocumentGjiChildren> DocChildDomain { get; set; }

        protected GjiBaseStimulReport(IReportTemplate reportTemplate)
            : base(reportTemplate)
        {
        }

        protected DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji seachingTypeDoc)
        {
            var result = document;

            if (document.TypeDocumentGji != seachingTypeDoc)
            {
                var docs =
                    DocChildDomain.GetAll()
                        .Where(x => x.Children.Id == document.Id)
                        .Select(x => x.Parent)
                        .ToList();

                foreach (var doc in docs)
                {
                    result = GetParentDocument(doc, seachingTypeDoc);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == seachingTypeDoc ? result : null;
            }

            return null;
        }

        protected DocumentGji GetChildDocument(DocumentGji document, TypeDocumentGji seachingTypeDoc)
        {
            var result = document;

            if (document.TypeDocumentGji != seachingTypeDoc)
            {
                var docs = DocChildDomain.GetAll()
                    .Where(x => x.Parent.Id == document.Id)
                    .Select(x => x.Children)
                    .ToList();

                foreach (var doc in docs)
                {
                    result = GetChildDocument(doc, seachingTypeDoc);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == seachingTypeDoc ? result : null;
            }

            return null;
        }

        protected Disposal GetMainDisposal(InspectionGji inspection)
        {
            if (inspection == null)
            {
                return null;
            }

            var disposalDomain = Container.ResolveDomain<Disposal>();

            using (Container.Using(disposalDomain))
            {
                return disposalDomain.GetAll()
                    .Where(x => x.TypeDisposal == TypeDisposalGji.Base)
                    .FirstOrDefault(x => x.Inspection.Id == inspection.Id);
            }
        }

        protected Inspector FillFirstInspector(DocumentGji document)
        {
            if (document == null)
            {
                return null;
            }

            Inspector inspector = null;

            var docinspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();

            using (Container.Using(docinspectorDomain))
            {
                var inspectors = docinspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    inspector = inspectors.First();
                    FillInspectorFio(inspector);
                    FillInspectorPosition(inspector);
                }
            }

            return inspector;
        }

        protected void FillInspectorFio(Inspector inspector, string prefix = "ИнспекторФио")
        {
            if (inspector == null || string.IsNullOrEmpty(prefix))
            {
                return;
            }

            Report[prefix + "Сокр"] = inspector.ShortFio;

            Report[prefix] = inspector.Fio;
            Report[prefix + "Рп"] = inspector.FioGenitive;
            Report[prefix + "Дп"] = inspector.FioDative;
            Report[prefix + "Вп"] = inspector.FioAccusative;
            Report[prefix + "Тп"] = inspector.FioAblative;
            Report[prefix + "Пп"] = inspector.FioPrepositional;
        }

        protected void FillInspectorPosition(Inspector inspector, string prefix = "ИнспекторДолжность")
        {
            if (inspector == null || string.IsNullOrEmpty(prefix))
            {
                return;
            }

            Report[prefix] = inspector.Position;
            Report[prefix + "Рп"] = inspector.PositionGenitive;
            Report[prefix + "Дп"] = inspector.PositionDative;
            Report[prefix + "Вп"] = inspector.PositionAccusative;
            Report[prefix + "Тп"] = inspector.PositionAblative;
            Report[prefix + "Пп"] = inspector.PositionPrepositional;
        }

        protected void FillContragent(Contragent contragent, string prefix = "Контрагент")
        {
            if (contragent == null || string.IsNullOrEmpty(prefix))
            {
                return;
            }

            Report["УправОргСокр"] = contragent.ShortName;
            Report["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.AddressName;

            Report[prefix] = contragent.Name;
            Report[prefix + "Рп"] = contragent.NameGenitive;
            Report[prefix + "Дп"] = contragent.NameDative;
            Report[prefix + "Вп"] = contragent.NameAccusative;
            Report[prefix + "Тп"] = contragent.NameAblative;
            Report[prefix + "Пп"] = contragent.NamePrepositional;

            Report[prefix + "Сокр"] = contragent.ShortName;

            Report[prefix + "ЮрАдрес"] = contragent.JuridicalAddress;
            Report[prefix + "ПочтАдрес"] = contragent.MailingAddress;
            Report[prefix + "ФактАдрес"] = contragent.FactAddress;
            Report[prefix + "АдресЗаПределами"] = contragent.AddressOutsideSubject;

            Report[prefix + "ЮрАдресПолный"] = contragent.FiasJuridicalAddress.Return(x => x.AddressName);
            Report[prefix + "ПочтАдресПолный"] = contragent.FiasMailingAddress.Return(x => x.AddressName);
            Report[prefix + "ФактАдресПолный"] = contragent.FiasFactAddress.Return(x => x.AddressName);
            Report[prefix + "АдресЗаПределамиПолный"] = contragent.FiasOutsideSubjectAddress.Return(x => x.AddressName);

            Report[prefix + "ИНН"] = contragent.Inn;
            Report[prefix + "КПП"] = contragent.Kpp;
            Report[prefix + "ДатаРегистрации"] =
                contragent.DateRegistration.HasValue
                    ? contragent.DateRegistration.Value.ToShortDateString()
                    : string.Empty;
        }

        protected void FillDocument(DocumentGji document, string prefix = "Документ", string dateFormat = "dd.MM.yyyy")
        {
            var docdate = document.DocumentDate.HasValue
                ? document.DocumentDate.Value.ToString(dateFormat, new CultureInfo("ru-RU"))
                : null;

            Report[prefix + "Номер"] = document.DocumentNumber;
            Report[prefix + "Дата"] = docdate;
            Report[prefix + "НомерДата"] = string.Format("№ {0} от {1}", document.DocumentNumber, docdate);
        }

        protected void FillCommonFields(DocumentGji doc)
        {
            Report["ИдентификаторДокументаГЖИ"] = doc.Id.ToString();
            Report["СтрокаПодключениякБД"] = Container.Resolve<IDbConfigProvider>().ConnectionString;
        }
    }
}
