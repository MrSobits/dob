using System.Linq;
using Bars.B4;
using Bars.Gkh.Services.ServiceContracts;

namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using Castle.Windsor;
    using DomainService;
    using Entities;
    using Gkh.Utils;
    using GkhGji.Entities;

    public class ResolutionGisChargeRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_resolution_gis_charge_rule"; }
        }

        public string Name
        {
            get { return "Постановление - Формирование файла начисления для РИС ГМП"; }
        }

        public string TypeId
        {
            get { return "gji_document_resol"; }
        }

        public string Description
        {
            get { return "Формирует файл с начислением для ГИС ГМП"; }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var resolution = statefulEntity as Resolution;

            if (resolution == null)
            {
                return ValidateResult.No("Объект не является постановлением");
            }

            if (resolution.Executant == null || string.IsNullOrEmpty(resolution.Executant.Name))
            {
                return ValidateResult.Yes();
            }

            if (resolution.FineMunicipality == null)
            {
                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «МО получателя штрафа»");
            }

            if (resolution.Municipality == null)
            {
                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Местонахождение»");
            }

            var now = DateTime.Now;

            var gisGmpPattern =
                Container.Resolve<IDomainService<GisGmpPattern>>()
                    .GetAll()
                    .FirstOrDefault(x => x.Municipality.Id == resolution.FineMunicipality.Id && now < x.DateEnd && now > x.DateStart);

            var names = new List<string>
            {
                "исполнительный комитет",
                "тсж",
                "управляющая компания"
            };

            if (!names.Contains(resolution.Executant.Name.ToLower().Trim()))
            {
                return ValidateResult.Yes();
            }

            var isAbandoned = "аннулировано начисление".Equals(newState.Name.Return(x => x.ToLower().Trim()));

            if (isAbandoned && resolution.GisUin.IsEmpty())
            {
                return ValidateResult.No("Невозможно аннулировать неотправленное в ГМП начисление");
            }

            if (isAbandoned && resolution.AbandonReason.IsEmpty())
            {
                return ValidateResult.No("Для перевода статуса необходимо заполнить поле «Причина аннулирования»");
            }

            /*
             * GisChargeJson.ChargeStatus
             * Возможные значения:
             * 1–новое, если у документа не заполнено поле УИН
             * 2–изменение, если у документа заполнено поле УИН
             * 3–аннулирование, Если у документа статус «Аннулировано начисление» и заполнен УИН
             */

            var config = Container.Resolve<IGjiTatParamService>().GetConfig();

            var gisCharge = new GisChargeToSend
            {
                Resolution = resolution,
                JsonObject = new GisChargeJson
                {
                    PatternCode =
                        gisGmpPattern != null ? gisGmpPattern.PatternCode : config.GetAs<string>("GisGmpPatternCode"),
                    BillDate = resolution.DocumentDate.ToDateString(),
                    TotalAmount = resolution.PenaltyAmount.ToDecimal().ToString("##.00", new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    }),
                    SupplierBillId = resolution.GisUin,
                    ChargeStatus = resolution.GisUin.IsEmpty()
                        ? "1"
                        : isAbandoned
                            ? "3"
                            : "2",
                    Details =
                        "Оплата административного штрафа по постановлению \"{0}\" \"{1}\""
                            .FormatUsing(resolution.DocumentNumber, resolution.DocumentDate.ToDateString()),
                    Payer = new GisChargeJsonPayer
                    {
                        PayerType = "2",
                        PayerDocNumber = resolution.Contragent.Return(x => x.Inn),
                        Kpp = resolution.Contragent.Return(x => x.Kpp),
                        PayerCode = resolution.Contragent.Return(x => x.ShortName),
                        PayerCaption = resolution.Contragent.Return(x => x.Name),
                    },
                    addition_fields = new List<GisJsonAdditionalField>
                    {
                        new GisJsonAdditionalField
                        {
                            Name = "Номер документа санкции",
                            Value = resolution.DocumentNumSsp
                        }

                    },
                    BudgetIndex = new GisChargeBudgetIndex
                    {
                        Status = "01" 
                    },
                    OperationName = isAbandoned ? resolution.AbandonReason : null,
                    Oktmo = resolution.Municipality.Return(x => x.Oktmo).ToStr()
                }
            };

            Container.ResolveDomain<GisChargeToSend>().Save(gisCharge);

            return ValidateResult.Yes();
        }
    }
}