namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Помещения
    /// </summary>
    public class PremisesExportableEntity : BaseExportableEntity<Room>
    {
        /// <inheritdoc />
        public override string Code => "PREMISES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Oms |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PremisesProxy>()
                .ProxyListCache
                .Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        x.EntranceId.ToStr(),
                        x.RoomNum.Cut(255),
                        this.GetType(x.Type),
                        x.IsCommonProperty.ToStr(), // 6. Нежилое помещение является общим имуществом в МКД
                        this.GetTypeHouse(x.TypeHouse), // 7. Характеристика жилого помещения
                        this.GetDecimal(x.Area),
                        this.GetDecimal(x.LivingArea),
                        x.CadastralHouseNumber.Cut(40), // 10. Кадастровый номер в ГКН
                        string.Empty, // 11. Условный номер ЕГРП
                        x.Floor.ToStr(),
                        string.Empty, // 13. Дата прекращения существования объекта
                        string.Empty, // 14. Иные характеристики нежилого помещения
                        string.Empty, // 15. Иные характеристики квартиры
                        string.Empty, // 16. Наличие факта признания квартиры непригодной для проживания
                        string.Empty, // 17. Основание признания квартиры непригодной для проживания
                        string.Empty, // 18. Дата документа, содержащего решение о признании квартиры непригодной для проживания
                        string.Empty // 19. Номер документа, содержащего решение о признании квартиры непригодной для проживания
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
                case 4:
                case 5:
                case 6:
                case 7:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    if (row.Cells[4] == "1")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 9:
                case 10:
                    return row.Cells[9].IsEmpty() && row.Cells[10].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код помещения в системе отправителя ",
                "Уникальный идентификатор дома",
                "Уникальный идентификатор подъезда",
                "Номер помещения",
                "Тип помещения",
                "Нежилое помещение является общим имуществом в МКД",
                "Характеристика жилого помещения",
                "Общая площадь помещения по паспорту помещения",
                "Жилая площадь помещения по паспорту помещения",
                "Кадастровый номер в ГКН",
                "Условный номер ЕГРП",
                "Этаж",
                "Дата прекращения существования объекта",
                "Иные характеристики нежилого помещения",
                "Иные характеристики квартиры",
                "Наличие факта признания квартиры непригодной для проживания",
                "Основание признания квартиры непригодной для проживания",
                "Дата документа, содержащего решение о признании квартиры непригодной для проживания",
                "Номер документа, содержащего решение о признании квартиры непригодной для проживания"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOM",
                "ENTRANCE"
            };
        }

        private string GetType(RoomType roomType)
        {
            if (roomType == RoomType.Living)
            {
                return this.Yes;
            }

            if (roomType == RoomType.NonLiving)
            {
                return this.No;
            }

            return string.Empty;
        }

        private string GetTypeHouse(TypeHouse? typeHouse)
        {
            switch (typeHouse)
            {
                case TypeHouse.SocialBehavior:
                    return "3";
                default:
                    return "1";
            }
        }
    }
}