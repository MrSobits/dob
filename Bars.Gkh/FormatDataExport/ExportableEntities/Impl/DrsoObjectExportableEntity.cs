namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Предметы договора ресурсоснабжения
    /// </summary>
    public class DrsoObjectExportableEntity : BaseExportableEntity<PublicServiceOrgContractService>
    {
        /// <inheritdoc />
        public override string Code => "DRSOOBJECT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var services = this.ProxySelectorFactory.GetSelector<DictUslugaProxy>()
                .ProxyListCache.Values
                .Where(x => x.DrsoServiceId.HasValue)
                .ToDictionary(x => x.DrsoServiceId, y => y.Id);

            return this.GetFiltred(x => x.ResOrgContract.PublicServiceOrg.Contragent)
                .WhereContainsBulked(x => x.Id, services.Keys, 5000)
                .Select(x => new
                {
                    DrsoServiceId = x.Service.ExportId,
                    x.Id,
                    ResOrgContractId = x.ResOrgContract.Id,
                    x.CommunalResource,
                    x.SchemeConnectionType,
                    x.StartDate,
                    x.EndDate,
                    x.PlanVolume,
                    UnitMeasureId = x.UnitMeasure.Id
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.ResOrgContractId.ToStr(), // 2. Договор ресурсоснабжения
                        x.DrsoServiceId.ToStr(), // 3. Код коммунальной услуги
                        this.GetCommunalResource(x.CommunalResource), // 4. Тарифицируемый ресурс
                        this.GetSchemeConnectionType(x.SchemeConnectionType), // 5. Зависимая схема присоединения
                        this.GetDate(x.StartDate), // 6. Дата начала поставки ресурса
                        this.GetDate(x.EndDate), // 7. Дата окончания поставки ресурса
                        this.GetDecimal(x.PlanVolume), // 8. Плановый объем
                        x.UnitMeasureId.ToStr(), // 9. Код ОКЕИ
                        string.Empty // 10. Режим подачи
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2, 3, 5, 6 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код ",
                "Договор ресурсоснабжения",
                "Код коммунальной услуги",
                "Тарифицируемый ресурс",
                "Зависимая схема присоединения",
                "Дата начала поставки ресурса",
                "Дата окончания поставки ресурса",
                "Плановый объем",
                "Код ОКЕИ",
                "Режим подачи"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSO",
                "DICTUSLUGA"
            };
        }

        private string GetCommunalResource(CommunalResource communalResource)
        {
            switch (communalResource?.Name)
            {
                case "Питьевая вода":
                    return "1";

                case "Техническая вода":
                    return "2";

                case "Горячая вода ":
                    return "3";

                case "Тепловая энергия":
                    return "4";

                case "Теплоноситель":
                    return "5";

                case "Поддерживаемая мощность":
                    return "6";

                case "Сточные воды":
                    return "7";

                case "Электрическая энергия":
                    return "8";

                case "Природный газ (метан)":
                    return "9";

                case "Сжиженный газ (пропан-бутан)":
                    return "10";

                case "Топливо твердое":
                    return "11";

                case "Топливо печное бытовое":
                    return "12";

                case "Керосин":
                    return "13";

                default:
                    return string.Empty;
            }
        }

        private string GetSchemeConnectionType(SchemeConnectionType? schemeConnectionType)
        {
            switch (schemeConnectionType)
            {
                case SchemeConnectionType.Dependent:
                {
                    return "1";
                }
                case SchemeConnectionType.Independent:
                {
                    return "2";
                }
                default:
                {
                    return string.Empty;
                }
            }
        }
    }
}