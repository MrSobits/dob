namespace Bars.GkhGji.Regions.Smolensk.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;

    using Slepov.Russian.Morpher;
    using Stimulsoft.Report;

    public class ResolutionGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor
        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Resolution))
        {
        }
        #endregion


        #region Properties
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "Resolution"; }
        }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override string Name
        {
            get { return "Постановление"; }
        }

        public override string Description
        {
            get { return "Постановление"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        protected void GetCodeTemplate()
        {
            CodeTemplate = "Resolution";
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "Resolution",
                            Name = "Постановление",
                            Description = "Постановление",
                            Template = Properties.Resources.Resolution
                        }
                };
        }

        protected override string CodeTemplate { get; set; }

        #endregion


        #region Fields

        private long DocumentId { get; set; }

        #endregion


        #region DomainServices

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IDomainService<ResolutionLongDescription> ResolutionLongDescriptionDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }
        public IDomainService<ResolutionDefinitionSmol> ResolutionDefinitionSmolDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        public IDomainService<ContragentBank> ContragentBankDomain { get; set; }
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var resolution = ResolutionDomain.Load(DocumentId);

            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            if (resolution.Sanction == null)
            {
                throw new ReportProviderException("Не указана санкция");
            }

            FillCommonFields(resolution);

            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            Report["НомерПостановления"] = resolution.DocumentNumber;

            Report["ДатаПостановления"] = resolution.DocumentDate.HasValue
                    ? resolution.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

            Report["ДатаВручения"] = resolution.DeliveryDate.HasValue
                    ? resolution.DeliveryDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

            if (resolution.Official != null)
            {
                Report["КодДлВынесшегоПостановление"] = resolution.Official.Position;
                Report["ФИОДлВынесшегоПостановление"] = resolution.Official.Fio;
                Report["ФИОРуководителя"] = resolution.Official.ShortFio;
            }

            var resolLong =
                ResolutionLongDescriptionDomain.GetAll()
                    .FirstOrDefault(x => x.Resolution.Id == resolution.Id);

            var longDescription = string.Empty;

            if (resolLong != null)
            {
                longDescription = Encoding.UTF8.GetString(resolLong.Description);
            }

            Report["СоставАП"] = longDescription.IsNotEmpty()
                ? longDescription
                : resolution.Description;

            var protocol = (GkhGji.Entities.Protocol)GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, resolution, TypeDocumentGji.Protocol);

            if (protocol != null)
            {
                Report["ИсполнительныйДокумент"] = string.Format(
                    "{0} № {1}", 
                    protocol.DocumentDate.HasValue
                        ? protocol.DocumentDate.Value.ToString("d MMMM yyyy")
                        : string.Empty, 
                    protocol.DocumentNumber);
                var protocolArticleLawList = ProtocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == protocol.Id)
                    .Select(x => new
                    {
                        x.ArticleLaw.Name,
                        x.ArticleLaw.Description
                    })
                    .ToList();

                var firstProtocolArticleLaw = protocolArticleLawList.FirstOrDefault();

                Report["ОбластьПравонарушения"] = firstProtocolArticleLaw != null
                                                      ? firstProtocolArticleLaw.Name
                                                      : string.Empty;

                Report["СтатьяИсполнительногоДокумента"] =
                    protocolArticleLawList.Select(x => x.Name).AggregateWithSeparator(", ");

                Report["ОписаниеСтатьи"] =
                    protocolArticleLawList.Select(x => x.Name + " - " + x.Description).AggregateWithSeparator("; ");

                var protocolViolation =
                    ProtocolViolationDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);

                Report["НаселенныйПункт"] = protocolViolation != null
                    ? protocolViolation.InspectionViolation.RealityObject.FiasAddress.PlaceName
                    : string.Empty;
            }

            var resolDefinitionList =
                ResolutionDefinitionSmolDomain.GetAll()
                    .Where(x => x.Resolution.Id == DocumentId)
                    .Select(x => new
                    {
                        x.DocumentDate,
                        x.DocumentNum,
                        x.TypeDefinition
                    })
                    .ToList();

            var resolDefinitionFirst =
                resolDefinitionList.FirstOrDefault(
                    x => x.TypeDefinition == TypeDefinitionResolution.AppointmentPlaceTime);

            Report["Определение"] = resolDefinitionFirst != null
                                        ? string.Format(
                                            "{0} № {1}",
                                            resolDefinitionFirst.DocumentDate.HasValue
                                                ? resolDefinitionFirst.DocumentDate.Value.ToString("d MMMM yyyy")
                                                : string.Empty,
                                            resolDefinitionFirst.DocumentNum)
                                        : string.Empty;

            Report["Вотношении"] = resolution.Executant != null ? resolution.Executant.Code : string.Empty;

            Report["ВидСанкции"] = resolution.Sanction != null ? resolution.Sanction.Code : string.Empty;

            Report["СуммаШтрафа"] = resolution.PenaltyAmount.HasValue ? resolution.PenaltyAmount.Value.ToString("#.##") : string.Empty;

            Report["ФизЛицо"] = resolution.PhysicalPerson;
            if (!resolution.PhysicalPerson.IsEmpty())
            {
                var physPersonAllCases = склонятель.Проанализировать(resolution.PhysicalPerson);
                Report["ФизЛицоРП"] = physPersonAllCases.Родительный;
                Report["ФизЛицоДП"] = physPersonAllCases.Дательный;
            }

            var physPersonInfo =
                DocumentGjiPhysPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == DocumentId);

            if (physPersonInfo != null)
            {
                Report["АдресТелефон"] = physPersonInfo.PhysPersonAddress;
                Report["МестоРаботы"] = physPersonInfo.PhysPersonJob;
                Report["Должность"] = physPersonInfo.PhysPersonPosition;

                if (!physPersonInfo.PhysPersonPosition.IsEmpty())
                {
                    var physPersonInfoPositionAllCases = склонятель.Проанализировать(physPersonInfo.PhysPersonPosition);
                    Report["ДолжностьРП"] = physPersonInfoPositionAllCases.Родительный;
                }

                Report["ДатаМестоРождения"] = physPersonInfo.PhysPersonBirthdayAndPlace;
                Report["ДокументУдостовЛичность"] = physPersonInfo.PhysPersonDocument;
            }

            var contragent = resolution.Contragent;

            if (contragent != null)
            {
                var contragentContact =
                    ContragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                Report["Наименование"] = contragent.Name;
                Report["КонтрагентСокр"] = contragent.ShortName;
                if (contragentContact != null)
                {
                    Report["ДолжностьКонтр"] = contragentContact.Position.Name;
                    Report["ФИО"] = contragentContact.FullName;
                }
                Report["АдресЮР"] = contragent.FiasJuridicalAddress.AddressName;
                Report["АдресФакт"] = contragent.FiasFactAddress.AddressName;
                Report["ИНН"] = contragent.Inn;
                Report["КПП"] = contragent.Kpp;
                Report["ОГРН"] = contragent.Ogrn;

                var contragentBank = ContragentBankDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                if (contragentBank != null)
                {
                    Report["РасСчет"] = contragentBank.SettlementAccount;
                    Report["КорСчет"] = contragentBank.CorrAccount;
                    Report["Банк"] = contragentBank.Name;
                    Report["Бик"] = contragentBank.Bik;
                }
            }

            var disposal = (Disposal)GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, resolution, TypeDocumentGji.Disposal);

            if (disposal != null)
            {
                Report["ДатаПриказа"] = disposal.DocumentDate.HasValue
                    ? disposal.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;
                Report["НомерПриказа"] = disposal.DocumentNumber;

                Report["ПериодОбследования"] = string.Format(
                    "с {0} по {1}",
                    disposal.DateStart.HasValue ? disposal.DateStart.Value.ToShortDateString() : string.Empty,
                    disposal.DateEnd.HasValue ? disposal.DateEnd.Value.ToShortDateString() : string.Empty);

                if (disposal.KindCheck != null && !disposal.KindCheck.Name.IsEmpty())
                {
                    var kindCheckAllCases = склонятель.Проанализировать(disposal.KindCheck.Name);
                    Report["ВидПроверки"] = kindCheckAllCases.Родительный;
                }
            }
        }
    }
}