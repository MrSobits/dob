namespace Bars.Gkh.Overhaul.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Работы в КПР
    /// </summary>
    public class WorkKprExportableEntity : BaseExportableEntity<TypeWorkCr>
    {
        /// <inheritdoc />
        public override string Code => "WORKKPR";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <summary>
        /// Средства источника финансирования
        /// </summary>
        public IRepository<FinanceSourceResource> FinanceSourceResourceRepository { get; set; }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var financeSourceDict = this.FinanceSourceResourceRepository.GetAll()
                .Select(x => new
                {
                    x.ObjectCr.Id,
                    x.FundResource,
                    x.BudgetSubject,
                    x.BudgetMu,
                    x.OwnerResource
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            return this.EntityRepository.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ProgramCrId = (long?) x.ObjectCr.ProgramCr.Id,
                    RealityObjectId = (long?) x.ObjectCr.RealityObject.Id,
                    WorkId = (long?) x.Work.Id,
                    x.DateEndWork,
                    x.ObjectCr.RealityObject.Municipality.Oktmo,
                    ObjectCrId = (long?) x.ObjectCr.Id
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var financeSource = financeSourceDict.Get(x.ObjectCrId ?? 0)?.LastOrDefault();

                    return new ExportableRow(x.Id,
                        new List<string>
                        {
                            x.Id.ToStr(),
                            x.ProgramCrId.ToStr(),
                            x.RealityObjectId.ToStr(),
                            x.WorkId.ToStr(),
                            this.GetDate(x.DateEndWork),
                            x.Oktmo.ToStr().Cut(11),
                            this.GetDecimal(financeSource?.FundResource),
                            this.GetDecimal(financeSource?.BudgetSubject),
                            this.GetDecimal(financeSource?.BudgetMu),
                            this.GetDecimal(financeSource?.OwnerResource)
                        });
                })
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
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                case 7:
                case 8:
                case 9:
                    return row.Cells[6].IsEmpty() && row.Cells[7].IsEmpty() && row.Cells[8].IsEmpty() && row.Cells[9].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Код КПР",
                "Код многоквартирного дома",
                "Вид капитального ремонта",
                "Месяц и год окончания работ",
                "ОКТМО",
                "Финансирование за счёт средств Фонда",
                "Финансирование за счёт средств бюджета субъекта РФ",
                "Финансирование за счёт средств местного бюджета",
                "Финансирование за счёт средств собственников"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "KPR",
                "DOM",
                "WORKKPRTYPE"
            };
        }
    }
}