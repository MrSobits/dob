namespace Bars.GkhGji.Regions.Stavropol.Report.DisposalGji
{
	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using B4.Modules.Reports;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Report;
	using Bars.GkhGji.Contracts;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Regions.Stavropol.Properties;
	using Bars.GkhGji.Report;
	using Stimulsoft.Report;
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;

	public class DisposalGjiNotificationStimulReport : GjiBaseStimulReport
	{
		public DisposalGjiNotificationStimulReport()
			: base(new ReportTemplateBinary(Resources.Notification_V_S))
		{
		}

		#region Properties

		public override string Id
		{
			get { return "DisposalNotification"; }
		}

		public override string CodeForm
		{
			get { return "Disposal"; }
		}

		public override string Name
		{
			get { return "Уведомление о проверке"; }
		}

		public override string Description
		{
			get { return "Уведомление о проверке (из приказа)"; }
		}

		public override StiExportFormat ExportFormat
		{
			get { return StiExportFormat.Word2007; }
		}

		protected override string CodeTemplate { get; set; }

		#endregion

		#region Fields

		private long _documentId;
		private Disposal _disposal;
		private DocumentGji _actCheck;

		#endregion

		public override void SetUserParams(UserParamsValues userParamsValues)
		{
			_documentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

			var disposalDomain = Container.ResolveDomain<Disposal>();
			using (Container.Using(disposalDomain))
			{
				_disposal = disposalDomain.Load(_documentId);
				if (_disposal == null)
				{
					var disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
					throw new ReportProviderException(string.Format("Не удалось получить {0}", disposalText.ToLower()));
				}

				_actCheck = GetChildDocument(_disposal, TypeDocumentGji.ActCheck);
			}
		}

		public override List<TemplateInfo> GetTemplateInfo()
		{
			return new List<TemplateInfo>
			{
				new TemplateInfo
				{
					Code = "Notification_V_S",
					Name = "Notification_V_S",
					Description = "Уведомление о проверке",
					Template = Properties.Resources.Notification_V_S
				}
			};
		}

		public override Stream GetTemplate()
		{
			CodeTemplate = "Notification_V_S";
			return base.GetTemplate();
		}

		public override void PrepareReport(ReportParams reportParams)
		{
            FillCommonFields(_disposal);
			// зональную инспекцию получаем через муниципальное образование первого дома
			var insGjiRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
			using (Container.Using(insGjiRoDomain))
			{
				var realityObjs = insGjiRoDomain.GetAll()
					.Where(x => x.Inspection.Id == _disposal.Inspection.Id)
					.Select(x => x.RealityObject)
					.ToArray();

				var firstRo = realityObjs.FirstOrDefault();
				if (firstRo != null)
				{
					var zonalInsDomain = Container.ResolveDomain<ZonalInspectionMunicipality>();
					using (Container.Using(zonalInsDomain))
					{
						var zonalInspection = zonalInsDomain.GetAll()
							.Where(x => x.Municipality.Id == firstRo.Municipality.Id)
							.Select(x => x.ZonalInspection)
							.FirstOrDefault();

						if (zonalInspection != null)
						{
							Report["Адрес1ГосЯзык"] = zonalInspection.Address;
							Report["Телефон"] = zonalInspection.Phone;
							Report["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
						}
					}

					if (realityObjs.Count() == 1 && firstRo.FiasAddress != null)
					{
						Report["АдресОбъектаПравонарушения"] = firstRo.FiasAddress.AddressName;
					}
				}
			}

			Report["ДатаРаспоряжения"] = _disposal.DocumentDate.HasValue
				? _disposal.DocumentDate.Value.ToString("«dd» MMMM yyyy", new CultureInfo("ru-RU"))
				: string.Empty;
			Report["ВидОбследования"] = _disposal.KindCheck != null ? _disposal.KindCheck.Name : "";

			var contragent = _disposal.Inspection.Contragent;
			if (contragent != null)
			{
				Report["УправОрг"] = contragent.Name;
				Report["ФактАдрес"] = contragent.FiasFactAddress != null
					? contragent.FiasFactAddress.AddressName
					: contragent.FactAddress;

				var contragentContactDomain = Container.ResolveDomain<ContragentContact>();
				using (Container.Using(contragentContactDomain))
				{
					var headContragent = contragentContactDomain.GetAll()
						.Where(x => x.Contragent.Id == contragent.Id && x.DateStartWork.HasValue &&
						            (x.DateStartWork.Value <= DateTime.Now &&
						             (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
						.FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

					if (headContragent != null)
					{
						Report["ФИОРукОрг"] = string.Format("{0} - {1}", headContragent.Position.Name, headContragent.FullName);
					}
				}
			}

			if (_actCheck != null)
			{
				Report["ДатаПроверки"] = _actCheck.DocumentDate.HasValue
					? _actCheck.DocumentDate.Value.ToString("dd MMMM yyyy", new CultureInfo("ru-RU"))
					: string.Empty;
			}

			Report["НомерРаспоряжения"] = _disposal.DocumentNumber;

			var insDomain = Container.ResolveDomain<DocumentGjiInspector>();
			var inspector = insDomain.GetAll()
				.Where(x => x.DocumentGji.Id == _disposal.Id)
				.Select(x => x.Inspector)
				.FirstOrDefault();

			if (inspector != null)
			{
				Report["Должность"] = inspector.Position;
				Report["Исполнитель"] = inspector.FioAblative;
				Report["ТелефонИнспектора"] = inspector.Phone;
			}
		}
	}
}