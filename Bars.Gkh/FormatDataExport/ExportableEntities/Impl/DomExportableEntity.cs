namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Дом
    /// </summary>
    public class DomExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOM";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.All ^
            FormatDataExportProviderFlags.RegOpWaste ^
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DomProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.City.Cut(40),
                        string.Empty, // 3. Населенный пункт районного подчинения (села, деревня, поселки и проч.)
                        this.GetValueOrDefault(x.Street, "-").Cut(40),
                        x.House.Cut(10),
                        x.Building.Cut(25),
                        x.Housing.Cut(10),
                        x.Letter.Cut(10),
                        x.ContragentId.ToStr(),
                        string.Empty, //10. Категория благоустроенности Код должен соответствовать базовому параметру с кодом 2001
                        this.GetNotZeroValue(x.MaximumFloors),
                        this.GetFirstDateYear(x.BuildYear),
                        this.GetDecimal(x.AreaMkd),
                        this.GetDecimal(x.AreaCommonUsage),
                        string.Empty, // 15. Полезная (отапливаемая площадь)
                        string.Empty, // 16. Количество строк - лицевой счет
                        string.Empty, // 17. Код Улицы КЛАДР
                        x.StreetGuid.ToStr(),
                        x.HouseGuid.ToStr(),
                        x.CadastralHouseNumber,
                        x.EgrpNumber, // 21. Условный номер ЕГРП
                        x.ConditionHouseId.ToStr(),
                        x.TypeHouse.ToStr(),
                        this.GetDecimal(x.AreaLiving),
                        this.GetFirstDateYear(x.CommissioningYear?.Year),
                        x.AccountFormationVariant.ToStr(),
                        x.UndergroundFloorCount.ToStr(),
                        x.MinimumFloors.ToStr(),
                        x.TimeZone.ToStr(),
                        this.No,
                        x.TypeManagement.ToStr(),
                        x.OktmoCode.ToStr().Cut(11),
                        x.LifeCycleStage,
                        this.GetDate(x.ReconstructionYear)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 3:
                case 10:
                case 12:
                case 18:
                case 22:
                case 23:
                case 24:
                case 26:
                case 28:
                case 29:
                case 31:
                    return row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код дома в системе отправителя",
                "Город/район",
                "Населенный пункт",
                "Улица",
                "Номер дома",
                "Строение (секция)",
                "Корпус",
                "Литера",
                "Код контрагента, передающего информацию по дому",
                "Категория благоустроенности",
                "Максимальная этажность",
                "Год постройки (указывается 1 число года, например 01.01.1900)",
                "Общая площадь (по техническому паспорту для расчета распределения расходов по площади дома)",
                "Площадь мест общего пользования",
                "Полезная (отапливаемая площадь)",
                "Количество строк - лицевой счет",
                "Код Улицы КЛАДР",
                "Код Улицы ФИАС",
                "Код дома ФИАС",
                "Кадастровый номер в ГКН",
                "Условный номер ЕГРП",
                "Состояние дома",
                "Тип дома",
                "Общая площадь жилых помещений по паспорту помещения",
                "Год ввода в эксплуатацию",
                "Способ формирования фонда капитального ремонта",
                "Количество подземных этажей",
                "Количество этажей наименьшее",
                "Часовая зона по Olsоn",
                "Наличие у дома статуса объекта культурного наследия",
                "Cпособ управления домом",
                "Код ОКТМО",
                "Стадия жизненного цикла",
                "Год проведения реконструкции"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "CONTRAGENT",
                "DICTPARAMVAL"
            };
        }
    }
}