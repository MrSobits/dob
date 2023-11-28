namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    using Stimulsoft.Report;

    public class AdministrativeCaseDocStimulReport : GkhBaseStimulReport
    {
        private long adminCaseDocId;

        private AdministrativeCaseDoc adminCaseDoc;

        public AdministrativeCaseDocStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.DefinitionInformation))
        {
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        public override string Id
        {
            get
            {
                return "AdminCaseDoc";
            }
        }

        public override string CodeForm
        {
            get
            {
                return "AdminCaseDoc";
            }
        }

        public override string Name
        {
            get
            {
                return "Документ адм. дела";
            }
        }

        public override string Description
        {
            get
            {
                return "Документ административного дела";
            }
        }

        public IDomainService<AdministrativeCaseDescription> DescriptionDomain { get; set; }

        public IDomainService<AdministrativeCaseDoc> AdminCaseDocDomain { get; set; }

        public IDomainService<AdministrativeCaseArticleLaw> AdminCaseArticleLawDomain { get; set; }

        public IDomainService<AdministrativeCaseProvidedDoc> AdminCaseProvidedDocDomain { get; set; }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.adminCaseDocId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "TomskDefinitionInformation",
                                   Name = "AdminCaseDoc",
                                   Description = "Определение об истребовании сведений",
                                   Template = Properties.Resources.DefinitionInformation
                               },
                           new TemplateInfo
                               {
                                   Code = "TomskDefinitionProlongation",
                                   Name = "AdminCaseDoc",
                                   Description = "Определение о продлении срока",
                                   Template = Properties.Resources.DefinitionProlongation
                               },
                           new TemplateInfo
                               {
                                   Code = "TomskPetitionProlongation",
                                   Name = "AdminCaseDoc",
                                   Description = "Ходатайство о продлении срока",
                                   Template = Properties.Resources.PetitionProlongation
                               }
                       };
        }

        public override Stream GetTemplate()
        {
            adminCaseDoc = AdminCaseDocDomain.GetAll().First(x => x.Id == this.adminCaseDocId);

            // Поскольку в данном шаблоне может быть несколько шаблонов,
            // то определяем шаблон имеенно здесь
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "TomskDefinitionInformation";

            switch (adminCaseDoc.TypeAdminCaseDoc)
            {
                case TypeAdminCaseDoc.DefinitionInformation:
                    CodeTemplate = "TomskDefinitionInformation";
                    break;
                case TypeAdminCaseDoc.DefinitionProlongation:
                    CodeTemplate = "TomskDefinitionProlongation";
                    break;
                case TypeAdminCaseDoc.PetitionProlongation:
                    CodeTemplate = "TomskPetitionProlongation";
                    break;
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var articleLawName = AdminCaseArticleLawDomain.GetAll().Where(x => x.AdministrativeCase.Id == adminCaseDoc.AdministrativeCase.Id).Select(x => x.ArticleLaw.Name).FirstOrDefault();

            Report["Номер"] = adminCaseDoc.DocumentNumber;

            Report["ДатаДокумента"] = adminCaseDoc.DocumentDate.HasValue ? adminCaseDoc.DocumentDate.Value.ToShortDateString() : string.Empty;

            if (adminCaseDoc.EntitiedInspector != null)
            {
                Report["ДолжностьИнспектора"] = adminCaseDoc.EntitiedInspector.Position;
                Report["Инспектор"] = adminCaseDoc.EntitiedInspector.Fio;
                Report["ИнспекторХодСокр"] = adminCaseDoc.EntitiedInspector.ShortFio;
            }

            if (adminCaseDoc.AdministrativeCase.Inspector != null)
            {
                Report["ДолжностьИнспектораАдмДелаРП"] = adminCaseDoc.AdministrativeCase.Inspector.PositionGenitive;
                Report["ДолжностьИнспектораДела"] = adminCaseDoc.AdministrativeCase.Inspector.Position;
                Report["ИнспекторАдмДелаРП"] = adminCaseDoc.AdministrativeCase.Inspector.FioGenitive;
                Report["ИнспекторДелаСокр"] = adminCaseDoc.AdministrativeCase.Inspector.ShortFio;
            }

            Report["НомерДела"] = adminCaseDoc.AdministrativeCase.DocumentNumber;
            Report["ДатаДела"] = adminCaseDoc.AdministrativeCase.DocumentDate.HasValue ? adminCaseDoc.AdministrativeCase.DocumentDate.Value.ToShortDateString() : string.Empty;

            Report["СтатьяЗакона"] = articleLawName;

            FillContragentCase(adminCaseDoc.AdministrativeCase.Contragent);

            Report["Вопрос"] = adminCaseDoc.AdministrativeCase.DescriptionQuestion;

            var adminCaseDocPetition =
                AdminCaseDocDomain.GetAll()
                    .Where(x => x.AdministrativeCase.Id == adminCaseDoc.AdministrativeCase.Id)
                    .Where(x => x.TypeAdminCaseDoc == TypeAdminCaseDoc.PetitionProlongation)
                    .Select(x => x.DescriptionSet)
                    .FirstOrDefault();

            Report["Установил"] = adminCaseDocPetition;
            Report["УстановилАдмДело"] =
                this.DescriptionDomain.GetAll().FirstOrDefault(x => x.AdministrativeCase.Id == adminCaseDoc.AdministrativeCase.Id).Return(x => Encoding.UTF8.GetString(x.DescriptionSet))
                ?? adminCaseDoc.AdministrativeCase.DescriptionSet;

            Report["ПродленныйСрок"] = adminCaseDoc.RenewalTerm.HasValue ? adminCaseDoc.RenewalTerm.Value.ToShortDateString() : string.Empty;

            Report["ОбъектНедвижимости"] = adminCaseDoc.AdministrativeCase.RealityObject != null ? adminCaseDoc.AdministrativeCase.RealityObject.Address : string.Empty;

            var providedDocs = AdminCaseProvidedDocDomain.GetAll().Where(x => x.AdministrativeCase.Id == adminCaseDoc.AdministrativeCase.Id).Select(x => x.ProvidedDoc.Name).ToArray();

            var providedDocuments = providedDocs.Select(doc => new ProvidedDocument { ПредоставляемыйДокумент = doc + ";" }).ToList();
            var lastDocument = providedDocuments.LastOrDefault();
            if (lastDocument != null)
            {
                lastDocument.ПредоставляемыйДокумент = lastDocument.ПредоставляемыйДокумент.Remove(lastDocument.ПредоставляемыйДокумент.Length - 1) + ".";
            }

            Report.RegData("ПредоставляемыеДокументы", providedDocuments);
        }

        private void FillContragentCase(Contragent contragent)
        {
            if (contragent != null)
            {
                Report["КонтрагентРП"] = !contragent.NameGenitive.IsEmpty() ? contragent.NameGenitive : contragent.Name;

                Report["КонтрагентДП"] = !contragent.NameDative.IsEmpty() ? contragent.NameDative : contragent.Name;

                Report["КонтрагентВП"] = !contragent.NameAccusative.IsEmpty() ? contragent.NameAccusative : contragent.Name;

                Report["КонтрагентТП"] = !contragent.NameAblative.IsEmpty() ? contragent.NameAblative : contragent.Name;

                Report["КонтрагентПП"] = !contragent.NamePrepositional.IsEmpty() ? contragent.NamePrepositional : contragent.Name;

                Report["КонтрагентКр"] = !contragent.ShortName.IsEmpty() ? contragent.ShortName : contragent.Name;
            }
        }

        protected class ProvidedDocument
        {
            public string ПредоставляемыйДокумент { get; set; }
        }
    }
}