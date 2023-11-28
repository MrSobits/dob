namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Физические лица
    /// </summary>
    public class IndExportableEntity : BaseExportableEntity<IndividualAccountOwner>
    {
        /// <inheritdoc />
        public override string Code => "IND";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var filter = new DynamicDictionary
            {
                ["IndFilterIds"] = this.ProxySelectorFactory.GetSelector<KvarProxy>()
                    .ProxyListCache
                    .Values
                    .Select(x => x.IndividualOwner.Id)
                    .ToList()
            };

            return this.ProxySelectorFactory.GetSelector<IndProxy>(filter, true)
                .ProxyListCache
                .Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Surname,
                        x.FirstName,
                        x.SecondName,
                        this.GetGender(x.Gender),
                        this.GetDate(x.BirthDate),
                        x.IdentityType,
                        string.Empty, //СНИЛС
                        x.IdentitySerial,
                        x.IdentityNumber,
                        this.GetDate(x.DateDocumentIssuance),
                        x.BirthPlace
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код физ.лица в системе отправителя",
                "Фамилия",
                "Имя",
                "Отчество",
                "Пол",
                "Дата рождения",
                "Документ, удостоверяющий личность",
                "СНИЛС",
                "Серия документа",
                "Номер документа",
                "Дата выдачи документа",
                "Место рождения"
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
                case 5:
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                case 7:
                    return row.Cells[6].IsEmpty() && row.Cells[7].IsEmpty();
                case 9:
                case 10:
                    return !row.Cells[6].IsEmpty() && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        private string GetGender(Gender? gender)
        {
            if (gender == Gender.Male)
            {
                return this.Yes;
            }

            if (gender == Gender.Female)
            {
                return this.No;
            }

            return string.Empty;
        }


    }
}