namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Протоколы общего собрания собственников<para/>
    /// Не выгружаем, т.к. в Комплексе половины данных по указанному методу нет. Плюс непонятно что за типы протоколв имеются ввиду.<para/>
    /// Будем передавать сведения о протоколах в методе 2.15.2 Протоколы общего собрания собственников
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class ProtocolossExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PROTOCOLOSS";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<ProtocolossProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        x.ContragentId.ToStr(),
                        this.GetDate(x.DocumentDate),
                        x.DocumentNumber.Cut(50),
                        this.GetDate(x.StartDate),
                        x.MethodFormFundCr.ToStr(),
                        x.VotingForm.ToStr(),
                        this.GetDate(x.EndDate),
                        x.DecisionPlace.Cut(200),
                        x.MeetingPlace.Cut(200),
                        this.GetDateTime(x.MeetingDateTime),
                        this.GetDateTime(x.VoteStartDateTime),
                        this.GetDateTime(x.VoteEndDateTime),
                        x.ReceptionProcedure.Cut(2000),
                        x.ReviewProcedure.Cut(2000),
                        x.IsAnnualMeeting.ToStr(),
                        x.IsCompetencyMeeting.ToStr(),
                        x.Status.ToStr(),
                        x.ChangeReason.Cut(2000)
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
                case 2:
                case 3:
                case 4:
                case 7:
                case 9:
                case 11:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    if (row.Cells[7] == "1" || row.Cells[7] == "4")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 10:
                    if (row.Cells[7] == "2" || row.Cells[7] == "4")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 12:
                    if (row.Cells[7] == "3")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Уникальный идентификатор дома",
                "Владелец протокола",
                "Дата составления протокола",
                "Номер протокола",
                "Дата вступления в силу",
                "Способ формирования фонда КР",
                "Форма проведения голосования",
                "Дата окончания приема решений",
                "Место приема решений",
                "Место проведения собрания",
                "Дата и время проведения собрания",
                "Дата и время начала проведения голосования",
                "Дата и время окончания проведения голосования",
                "Порядок приема оформленных в письменной форме решений собственников",
                "Порядок ознакомления с информацией и материалами, которые будут представлены на данном собрании",
                "Ежегодное собрание",
                "Правомочность собрания",
                "Статус",
                "Основание изменения протокола"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOM",
                "CONTRAGENT",
                "VOTPROCONT"
            };
        }
    }
}