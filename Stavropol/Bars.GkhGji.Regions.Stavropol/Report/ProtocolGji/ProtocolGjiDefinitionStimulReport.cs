using System.Text.RegularExpressions;

namespace Bars.GkhGji.Regions.Stavropol.Report.ProtocolGji
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Stavropol.Properties;
    using Bars.GkhGji.Report;

    using Stimulsoft.Report;

    public class ProtocolGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        public ProtocolGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Resources.Stavrapol_Protocol_Definition_1))
        {
        }

        #region Fields

        private long _definitionId;
        private IList<DocumentGji> _documents;
        private Protocol _protocol;
        private ProtocolDefinition _definition;
        private Disposal _disposal;
        private ActCheck _actCheck;
        private Resolution _resolution;

        private const string StavrapolProtocolDefinition1 = "Stavrapol_Protocol_Definition_1";
        private const string StavrapolProtocolDefinition2 = "Stavrapol_Protocol_Definition_2";
        private const string StavrapolProtocolDefinition3 = "Stavrapol_Protocol_Definition_3";
        private const string BlockGjiDefinition1 = "BlockGJI_Definition_1";
        private const string BlockGjiDefinition2 = "BlockGJI_Definition_2";
        private const string BlockGjiDefinition3 = "BlockGJI_Definition_3";
        private const string BlockGjiDefinition4 = "BlockGJI_Definition_4";
        private const string BlockGjiDefinition5 = "BlockGJI_Definition_5";

        private readonly IEnumerable<string> _executantTypesForPerson = new[] { "16", "10", "12", "13", "1", "3", "5", "19", "21", "22" };
        private readonly IEnumerable<string> _executantTypesForPhysPerson = new[] { "14", "6", "7" };
        private readonly IEnumerable<string> _executantTypesForJurPerson = new[] { "15", "9", "0", "2", "4", "8", "11", "18" };

        #endregion Fields

        #region Properties

        public override string Id
        {
            get { return "ProtocolDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override string Name
        {
            get { return "Определение протокола"; }
        }

        public override string Description
        {
            get { return "Определение протокола"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _definitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            var protocolDefinitionDomain = Container.ResolveDomain<ProtocolDefinition>();
            using (Container.Using(protocolDefinitionDomain))
            {
                _definition = protocolDefinitionDomain.Load(_definitionId);
            }

            if (_definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            _protocol = _definition.Protocol;

            var documentDomain = Container.ResolveDomain<DocumentGji>();
            using (Container.Using(documentDomain))
            {
                // Получаем все документы в дереве
                _documents = documentDomain.GetAll()
                    .Where(x => x.Stage.Inspection.Id == _protocol.Inspection.Id)
                    .ToList();

                _disposal = _documents.FirstOrDefault(x => x.TypeDocumentGji == TypeDocumentGji.Disposal) as Disposal;
                _actCheck = _documents.FirstOrDefault(x => x.TypeDocumentGji == TypeDocumentGji.ActCheck) as ActCheck;
                _resolution = _documents.FirstOrDefault(x => x.TypeDocumentGji == TypeDocumentGji.Resolution) as Resolution;

	            if (_actCheck != null)
	            {
		            // Получаем поличество домов в акте
		            var actRealityObjectDomain = Container.ResolveDomain<ActCheckRealityObject>();
		            using (Container.Using(actRealityObjectDomain))
		            {
			            var cnt = actRealityObjectDomain.GetAll().Count(x => x.ActCheck.Id == _actCheck.Id);
			            if (cnt == 1)
			            {
				            _actCheck.ActCheckGjiRealityObject =
					            actRealityObjectDomain.GetAll().FirstOrDefault(x => x.ActCheck.Id == _actCheck.Id);
			            }
		            }
	            }
            }
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1027:TabsMustNotBeUsed", Justification = "Reviewed. Suppression is OK here.")]
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = StavrapolProtocolDefinition1,
                    Code = StavrapolProtocolDefinition1,
                    Description = "Определение об отказе в удовлетворении ходатайства",
					Template = Resources.Stavrapol_Protocol_Definition_1
				},
				new TemplateInfo
				{
					Name = StavrapolProtocolDefinition2,
					Code = StavrapolProtocolDefinition2,
					Description = "Определение об исправлении опечатки",
					Template = Resources.Stavrapol_Protocol_Definition_2
				},
				new TemplateInfo
				{
					Name = StavrapolProtocolDefinition3,
					Code = StavrapolProtocolDefinition3,
					Description =
						"Определение об истребовании сведений, необходимых для разрешения дела об административном правонарушении",
					Template = Resources.Stavrapol_Protocol_Definition_3
				},
				new TemplateInfo
				{
					Name = BlockGjiDefinition1,
					Code = BlockGjiDefinition1,
					Description =
						"Определение о возвращении протокола об административном правонарушении и других материалов дела должностному лицу",
					Template = Resources.BlockGJI_Definition_1
				},
				new TemplateInfo
				{
					Name = BlockGjiDefinition2,
					Code = BlockGjiDefinition2,
					Description = "Определение о приводе, необходимых для разрешения дела об административном правонарушении",
					Template = Resources.BlockGJI_Definition_2
				},
				new TemplateInfo
				{
					Name = BlockGjiDefinition3,
					Code = BlockGjiDefinition3,
					Description =
						"Определение об отложении рассмотрения дела об административном правонарушении ДлЛица",
					Template = Resources.BlockGJI_Definition_3
				},
				new TemplateInfo
				{
					Name = BlockGjiDefinition4,
					Code = BlockGjiDefinition4,
					Description =
						"Определение об отложении рассмотрения дела об административном правонарушении ЮрЛица",
					Template = Resources.BlockGJI_Definition_4
				},
				new TemplateInfo
				{
					Name = BlockGjiDefinition5,
					Code = BlockGjiDefinition5,
					Description =
						"Определение об отложении рассмотрения дела об административном правонарушении ФизЛица",
					Template = Resources.BlockGJI_Definition_5
				}
			};
        }

        public override Stream GetTemplate()
        {
            switch (_definition.TypeDefinition)
            {
                case TypeDefinitionProtocol.DenialPetition:
                    CodeTemplate = StavrapolProtocolDefinition1;
                    break;
                case TypeDefinitionProtocol.CorrectionMisprint:
                    CodeTemplate = StavrapolProtocolDefinition2;
                    break;
                case TypeDefinitionProtocol.ReclamationInformation:
                    CodeTemplate = StavrapolProtocolDefinition3;
                    break;
                case TypeDefinitionProtocol.ReturnProtocol:
                    CodeTemplate = BlockGjiDefinition1;
                    break;
                case TypeDefinitionProtocol.About:
                    CodeTemplate = BlockGjiDefinition2;
                    break;

                case TypeDefinitionProtocol.PostponeCase:
                    if (_executantTypesForPerson.Contains(_protocol.Executant.Code))
                    {
                        CodeTemplate = BlockGjiDefinition3;
                    }
                    else if (_executantTypesForJurPerson.Contains(_protocol.Executant.Code))
                    {
                        CodeTemplate = BlockGjiDefinition4;
                    }
                    else if (_executantTypesForPhysPerson.Contains(_protocol.Executant.Code))
                    {
                        CodeTemplate = BlockGjiDefinition5;
                    }
                    break;
            }

            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            FillCommonInfo();
            switch (_definition.TypeDefinition)
            {
                case TypeDefinitionProtocol.DenialPetition:
                    FillDenialPetitionInfo();
                    break;
                case TypeDefinitionProtocol.CorrectionMisprint:
                    FillCorrectionMisprintInfo();
                    break;
                case TypeDefinitionProtocol.ReclamationInformation:
                    FillReclamationInformationInfo();
                    break;
                case TypeDefinitionProtocol.ReturnProtocol:
                    FillReturnProtocolInfo();
                    break;
                case TypeDefinitionProtocol.About:
                    FillAboutInfo();
                    break;

                case TypeDefinitionProtocol.PostponeCase:
                    if (_executantTypesForPerson.Contains(_protocol.Executant.Code))
                    {
                        FillPostponeCaseDefinitionPersonInfo();
                    }
                    else if (_executantTypesForJurPerson.Contains(_protocol.Executant.Code))
                    {
                        FillPostponeCaseDefinitionJurInfo();
                    }
                    else if (_executantTypesForPhysPerson.Contains(_protocol.Executant.Code))
                    {
                        FillPostponeCaseDefinitionPhysInfo();
                    }
                    break;
            }
        }

        /// <summary>
        /// Общая информация для всех определений
        /// </summary>
        private void FillCommonInfo()
        {
            Report["НомерОпределения"] = _definition.DocumentNumber != null 
                ? _definition.DocumentNumber.ToString() 
                : _definition.DocumentNum;
            Report["ДатаОпределения"] = _definition.DocumentDate.HasValue
                ? _definition.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            if (_definition.IssuedDefinition != null)
            {
                Report["Руководитель"] = _definition.IssuedDefinition.Fio;
                Report["РуководительФИО"] = _definition.IssuedDefinition.ShortFio;
                Report["ДолжностьРуководителя"] = _definition.IssuedDefinition.Position;
            }
        }

        /// <summary>
        /// Определение об отказе в удовлетворении ходатайства
        /// </summary>
        private void FillDenialPetitionInfo()
        {
            FillContragentInfo();

            if (_protocol.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var insAppCitsDomain = Container.ResolveDomain<InspectionAppealCits>();
                using (Container.Using(insAppCitsDomain))
                {
                    var appeals = insAppCitsDomain.GetAll()
                        .Where(x => x.Inspection.Id == _protocol.Inspection.Id)
                        .ToList();

                    if (appeals.Count > 0)
                    {
                        Report["Корреспондент"] = string.Join(",", appeals.Select(x => x.AppealCits.Correspondent));
                    }
                }
            }

            Report["АдресПравонарушения"] = GetProtocolViolationAddress();
	        Report["ДатаИВремяРассмотренияДела"] = GetDateAndTimeForExecutionDate();

            Report["СтатьяЗакона"] = GetArticleLaw();
            Report["Описание"] = _definition.Description;

            if (_actCheck != null)
            {
                Report["ДатаАкта"] = _actCheck.DocumentDate != null
                    ? _actCheck.DocumentDate.Value.ToShortDateString()
                    : string.Empty;
	            
				if (_actCheck.ActCheckGjiRealityObject != null)
	            {
					Report["ОписаниеАкт"] = _actCheck.ActCheckGjiRealityObject.Description;    
	            }
            }

            if (_disposal != null && _disposal.KindCheck != null)
            {
                Report["ВидПроверки"] = _disposal.KindCheck.Name;
            }
        }

        /// <summary>
        /// Определение об исправлении опечатки
        /// </summary>
        private void FillCorrectionMisprintInfo()
        {
            if (_resolution != null)
            {
                Report["ДатаПостановления"] = _resolution.DocumentDate != null
                    ? _resolution.DocumentDate.ToDateTime().ToShortDateString()
                    : string.Empty;

                Report["НомерПостановления"] = _resolution.DocumentNumber;
            }

            Report["СписокМатериалов"] = GetMaterialList();
        }

        /// <summary>
        /// Определение об истребовании сведений, необходимых для разрешения дела об административном правонарушении
        /// </summary>
        private void FillReclamationInformationInfo()
        {
            FillContragentInfo();

            Report["АдресПравонарушения"] = GetProtocolViolationAddress();
            Report["СписокМатериалов"] = GetMaterialList();
        }

        /// <summary>
        /// Определение о возвращении протокола
        /// </summary>
        private void FillReturnProtocolInfo()
        {
            FillContragentInfo();

            Report["ДатаПротокола"] = _protocol.DocumentDate.HasValue
                ? _protocol.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            Report["НомерПротокола"] = _protocol.DocumentNumber;

            var docInsDomain = Container.ResolveDomain<DocumentGjiInspector>();
            using (Container.Using(docInsDomain))
            {
                Report["Инспектор"] = docInsDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == _protocol.Id)
                    .Select(x => x.Inspector.Fio)
                    .FirstOrDefault();
            }

            Report["Описание"] = _definition.Description;
            Report["СтатьяЗакона"] = GetArticleLaw();
            Report["АдресПравонарушения"] = GetProtocolViolationAddress();
            Report["СписокМатериалов"] = GetMaterialList();
        }

        /// <summary>
        /// Определение о приводе
        /// </summary>
        private void FillAboutInfo()
        {
            FillContragentInfo();

	        Report["ДатаИВремяРассмотренияДела"] = GetDateAndTimeForExecutionDate();

            if (_actCheck != null)
            {
                Report["ДатаАкта"] = _actCheck.DocumentDate != null
                    ? _actCheck.DocumentDate.Value.ToShortDateString()
                    : string.Empty;

                Report["НомерАкта"] = _actCheck.DocumentNumber;
            }

            Report["ДатаПротокола"] = _protocol.DocumentDate != null
                ? _protocol.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            Report["НомерПротокола"] = _protocol.DocumentNumber;
            Report["АдресПравонарушения"] = GetProtocolViolationAddress();
            Report["СтатьяЗакона"] = GetArticleLaw();
            Report["НомерДома"] = GetProtocolViolationHouseNum();
        }

        /// <summary>
        /// Определение об отложении рассмотрения дела для исполнителей с кодом 16,10,12,13,1,3,5,19,21,22 (делаДлЛица)
        /// </summary>
        private void FillPostponeCaseDefinitionPersonInfo()
        {
            if (_resolution != null)
            {
                Report["ДатаПостановления"] = _resolution.DocumentDate != null
                    ? _resolution.DocumentDate.ToDateTime().ToShortDateString()
                    : string.Empty;

	            if (_protocol != null)
	            {
					Report["ФизическоеЛицо"] = _protocol.PhysicalPerson;
	            }
	            if (_resolution.Official != null)
	            {
					Report["ФИОВынесшего"] = _resolution.Official.Fio;
					Report["ДолжностьВынесшего"] = _resolution.Official.Position;
					Report["ФИОВынесшегоТП"] = _resolution.Official.FioAblative;    
					Report["ДолжностьВынесшегоТП"] = _resolution.Official.PositionAblative;
	            }
            }

            Report["СтатьяЗакона"] = GetArticleLaw();
	        Report["ДатаИВремяРассмотренияДела"] = GetDateAndTimeForExecutionDate();
        }

        /// <summary>
        /// Определение об отложении рассмотрения дела для исполнителей с кодом 15,9,0,2,4,8,11,18 (делаЮрЛица)
        /// </summary>
        private void FillPostponeCaseDefinitionJurInfo()
        {
            FillContragentInfo();

            Report["ДатаПротокола"] = _protocol.DocumentDate.HasValue
                ? _protocol.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            Report["НомерПротокола"] = _protocol.DocumentNumber;
            Report["СтатьяЗакона"] = GetArticleLaw();
	        Report["ДатаИВремяРассмотренияДела"] = GetDateAndTimeForExecutionDate();

	        if (_protocol != null)
	        {
				Report["ФизическоеЛицо"] = _protocol.PhysicalPerson;
	        }

			Report["ОпределенияОВремИместе"] = GetTimeAndPlaceHearingDefinitions();
        }

        /// <summary>
        /// Определение об отложении рассмотрения дела для исполнителей с кодом 14,6,7 (делаФизЛица)
        /// </summary>
        private void FillPostponeCaseDefinitionPhysInfo()
        {
            Report["ДатаПротокола"] = _protocol.DocumentDate.HasValue
                ? _protocol.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            Report["НомерПротокола"] = _protocol.DocumentNumber;

            if (_protocol != null)
            {
				Report["ФизическоеЛицо"] = _protocol.PhysicalPerson;
				Report["АдресФЛ"] = _protocol.PhysicalPersonInfo;
            }

			Report["ОпределенияОВремИместе"] = GetTimeAndPlaceHearingDefinitions();
	        Report["ДатаИВремяРассмотренияДела"] = GetDateAndTimeForExecutionDate();
        }

        private void FillContragentInfo()
        {
            if (_protocol.Contragent != null)
            {
                Report["Контрагент"] = _protocol.Contragent.Name;
                Report["КонтрагентСокр"] = _protocol.Contragent.ShortName;
                Report["АдресКонтрагента"] = _protocol.Inspection.Contragent.FiasJuridicalAddress != null
                    ? _protocol.Inspection.Contragent.FiasJuridicalAddress.AddressName
                    : string.Empty;

                var contact = GetContragentContact(new[] { "1", "4" });
                if (contact != null)
                {
                    Report["ФИОРукОрг"] = string.Format("{0} {1}", contact.Position.Name, contact.FullName);
                }
            }
        }

        private ContragentContact GetContragentContact(IEnumerable<string> positionCodes)
        {
            var contragentContactDomain = Container.ResolveDomain<ContragentContact>();
            using (Container.Using(contragentContactDomain))
            {
                return
                    contragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == _protocol.Contragent.Id &&
                                                                         positionCodes.Contains(x.Position.Code));
            }
        }

        private string GetMaterialList()
        {
            var docStr = string.Join("; ",
                _documents.Select(
                    x => string.Format("{0} от {1} №{2}", x.TypeDocumentGji.GetEnumMeta().Display, x.DocumentDate.ToDateString(),
                        x.DocumentNum)));

            var sb = new StringBuilder(docStr);
            if (_protocol.Inspection.Contragent != null)
            {
                var contact = GetContragentContact(new[] { "1", "4" });
                if (contact != null)
                {
                    sb.AppendFormat(", составленные {0} {1};", contact.Position.NameAblative, contact.NameAblative);
                }
            }

            if (_actCheck != null)
            {
                var actCheckDefinitionDomain = Container.ResolveDomain<ActCheckDefinition>();
                using (Container.Using(actCheckDefinitionDomain))
                {
                    var actCheckDefinitions = actCheckDefinitionDomain.GetAll().Where(x => x.ActCheck.Id == _actCheck.Id).ToList();
                    foreach (var definition in actCheckDefinitions)
                    {
                        sb.Append(GetDefinitionString(definition.DocumentDate.ToDateString(), definition.DocumentNumber,
                            definition.TypeDefinition.GetEnumMeta().Display));
                    }
                }
            }

            if (_protocol != null)
            {
                var protocolDefinitionDomain = Container.ResolveDomain<ProtocolDefinition>();
                using (Container.Using(protocolDefinitionDomain))
                {
                    var protocolDefinitions = protocolDefinitionDomain.GetAll().Where(x => x.Protocol.Id == _protocol.Id).ToList();
                    foreach (var definition in protocolDefinitions)
                    {
						sb.Append(GetDefinitionString(definition.DocumentDate.ToDateString(), definition.DocumentNumber,
                            definition.TypeDefinition.GetEnumMeta().Display));
                    }
                }
            }

            if (_resolution != null)
            {
                var protocolDefinitionDomain = Container.ResolveDomain<ResolutionDefinition>();
                using (Container.Using(protocolDefinitionDomain))
                {
                    var resolutionDefinitions = protocolDefinitionDomain.GetAll().Where(x => x.Resolution.Id == _resolution.Id).ToList();
                    foreach (var definition in resolutionDefinitions)
                    {
						sb.Append(GetDefinitionString(definition.DocumentDate.ToDateString(), definition.DocumentNumber,
                            definition.TypeDefinition.GetEnumMeta().Display));
                    }
                }
            }

            return sb.ToString();
        }

        private string GetDefinitionString(string date, int? num, string type)
        {
	        return string.Format("; определение от {0} №{1} {2}",
		        date, num.HasValue ? num.Value.ToString() : string.Empty, type);
        }

        private string GetArticleLaw()
        {
            var articleDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            using (Container.Using(articleDomain))
            {
                var articles = articleDomain.GetAll()
                    .Where(x => x.Protocol.Id == _protocol.Id)
                    .Select(x => x.ArticleLaw)
                    .AggregateWithSeparator(x => x.Name, ", ");

                return articles;
            }
        }

        private string GetProtocolViolationAddress()
        {
            var protocolViolationDomain = Container.ResolveDomain<ProtocolViolation>();
            using (Container.Using(protocolViolationDomain))
            {
                var realitiObjects = protocolViolationDomain.GetAll()
                    .Where(x => x.Document.Id == _protocol.Id)
                    .Select(x => new
                    {
                        Municipality = x.InspectionViolation.RealityObject != null
                            ? x.InspectionViolation.RealityObject.Municipality.Name
                            : string.Empty,
                        RealityObject = x.InspectionViolation.RealityObject != null
                            ? x.InspectionViolation.RealityObject.Address
                            : string.Empty,
                        RealityObjectId = x.InspectionViolation.RealityObject != null
                            ? x.InspectionViolation.RealityObject.Id
                            : 0,
                    }).ToList()
                    .GroupBy(x => new { x.RealityObjectId, x.Municipality, x.RealityObject })
                    .Select(x => new
                    {
                        x.Key.RealityObject
                    })
                    .AggregateWithSeparator(x => x.RealityObject, ", ");

                return realitiObjects;
            }
        }

        private string GetProtocolViolationHouseNum()
        {
	        var address = GetProtocolViolationAddress();
	        var match = Regex.Match(address, @"д\.\s(\d+)");
	        
	        return match.Groups[1].Value;
        }

	    private string GetDateAndTimeForExecutionDate()
	    {
		    var date = _definition.ExecutionDate.HasValue
			    ? _definition.ExecutionDate.Value.ToShortDateString()
			    : null;

		    var time = _definition.TimeDefinition.HasValue
			    ? _definition.TimeDefinition.Value.ToString("HH:mm", new CultureInfo("ru-RU"))
			    : null;

		    return string.Format("{0} {1}", date, time);
	    }

	    private string GetTimeAndPlaceHearingDefinitions()
	    {
			if (_protocol != null)
			{
				var protocolDefinitionDomain = Container.ResolveDomain<ProtocolDefinition>();
				using (Container.Using(protocolDefinitionDomain))
				{
					var definitions = protocolDefinitionDomain.GetAll()
						.Where(e => e.Protocol.Id == _protocol.Id && e.TypeDefinition == TypeDefinitionProtocol.TimeAndPlaceHearing)
						.ToList()
						.Select(e => string.Format("от {0} № {1}", e.DocumentDate.ToDateString(), e.DocumentNumber));

					return string.Join(", ", definitions);
				}
			}

		    return null;
	    }
    }
}