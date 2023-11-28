namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Лицевые счета
    /// </summary>
    public class KvarExportableEntity : BaseExportableEntity<BasePersonalAccount>
    {
        /// <inheritdoc />
        public override string Code => "KVAR";

        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<KvarProxy>()
                .ProxyListCache
                .Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код лицевого счета в системе отправителя
                        x.PersonalAccountNum.Cut(30), // 2. Уникальный номер лицевого счета в системе отправителя
                        x.RealityObjectId.ToStr(), // 3. Уникальный код дома
                        x.IndividualOwner?.Surname.ToStr(), // 4. Фамилия абонента
                        x.IndividualOwner?.FirstName.ToStr(), // 5. Имя абонента
                        x.IndividualOwner?.SecondName.ToStr(), // 6. Отчество абонента
                        this.GetDate(x.IndividualOwner?.BirthDate), // 7. Дата рождения абонента
                        this.GetDate(x.OpenDate), // 8. Дата открытия ЛС
                        string.Empty, // 9. Основание открытия ЛС
                        this.GetDate(x.CloseDate), // 10.Дата закрытия ЛС
                        x.CloseReasonType, // 11.Причина закрытия ЛС
                        x.CloseReason.Cut(100), // 12.Основание закрытия ЛС
                        x.Premises?.AccountsNum.ToStr(), // 13.Количество проживающих
                        string.Empty, // 14.Количество временно прибывших жильцов
                        string.Empty, // 15.Количество временно убывших жильцов
                        this.GetNotZeroValue(x.Premises?.RoomsCount), // 16.Количество комнат
                        this.GetDecimal(x.Area).Cut(11), // 17.Общая площадь (площадь применяемая для расчета большинства площадных услуг)
                        this.GetDecimal(x.Premises?.LivingArea).Cut(11), // 18.Жилая площадь
                        string.Empty, // 19.Отапливаемая площадь
                        string.Empty, // 20.Площадь для найма
                        string.Empty, // 21.Наличие эл. Плиты
                        string.Empty, // 22.Наличие газовой плиты
                        string.Empty, // 23.Наличие газовой колонки
                        string.Empty, // 24.Наличие огневой плиты
                        string.Empty, // 25.Код типа жилья по газоснабжению
                        string.Empty, // 26.Код типа жилья по водоснабжению
                        string.Empty, // 27.Код типа жилья по горячей воде
                        string.Empty, // 28.Код типа жилья по канализации
                        string.Empty, // 29.Наличие забора из открытой системы отопления
                        string.Empty, // 30.Участок (ЖЭУ)
                        x.PrincipalContragentId.ToStr(), // 31.Код контрагента, с которым у потребителя ЖКУ заключен договор на оказание услуг (принципал)
                        x.PersonalAccountType.ToStr(), // 32.Тип лицевого счета
                        this.GetStrId(x.IndividualOwner), // 33.Плательщик – Физ.лицо
                        x.ContragentId.ToStr(), // 34.Плательщик – Организация
                        string.Empty // 35.Плательщик является нанимателем
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код лицевого счета в системе отправителя",
                "Уникальный номер лицевого счета в системе отправителя",
                "Уникальный код дома",
                "Фамилия абонента",
                "Имя абонента",
                "Отчество абонента",
                "Дата рождения абонента",
                "Дата открытия ЛС",
                "Основание открытия ЛС",
                "Дата закрытия ЛС",
                "Причина закрытия ЛС",
                "Основание закрытия ЛС",
                "Количество проживающих",
                "Количество временно прибывших жильцов",
                "Количество временно убывших жильцов",
                "Количество комнат",
                "Общая площадь (площадь применяемая для расчета большинства площадных услуг)",
                "Жилая площадь",
                "Отапливаемая площадь",
                "Площадь для найма",
                "Наличие эл. Плиты",
                "Наличие газовой плиты",
                "Наличие газовой колонки",
                "Наличие огневой плиты",
                "Код типа жилья по газоснабжению",
                "Код типа жилья по водоснабжению",
                "Код типа жилья по горячей воде",
                "Код типа жилья по канализации",
                "Наличие забора из открытой системы отопления",
                "Участок (ЖЭУ)",
                "Код контрагента, с которым у потребителя ЖКУ заключен договор на оказание услуг (принципал)",
                "Тип лицевого счета",
                "Плательщик – Физ.лицо",
                "Плательщик – Организация",
                "Плательщик является нанимателем"
            };
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                case 7:
                case 16:
                case 30:
                case 31:
                    return row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "CONTRAGENT",
                "IND",
                "PREMISES"
            };
        }
    }
}
