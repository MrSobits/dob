namespace Bars.Gkh.ViewModel.Administration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    public class FormatDataExportTaskViewModel : BaseViewModel<FormatDataExportTask>
    {
        public IGkhUserManager GkhUserManager { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroup { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<FormatDataExportTask> domainService, BaseParams baseParams)
        {
            var activeOperator = this.GkhUserManager.GetActiveOperator()?.Id ?? 0;

            var isAdministrator = this.GkhUserManager.GetActiveUser().Roles.Any(x => x.Role.Name == "Администратор");

            var entityGroupDict = this.ExportableEntityGroup
                .Select(x => new
                {
                    x.Code,
                    x.Description,
                })
                .ToDictionary(x => x.Code, x => x.Description);

            return domainService.GetAll()
                .Where(x => !x.IsDelete)
                .WhereIf(!isAdministrator, x => x.Operator.Id == activeOperator)
                .Select(x => new
                {
                    x.Id,
                    x.Operator.User.Login,
                    x.ObjectCreateDate,
                    x.StartDate,
                    x.EndDate,
                    x.StartTimeHour,
                    x.StartTimeMinutes,
                    x.PeriodType,
                    x.StartDayOfWeekList,
                    x.StartMonthList,
                    x.EntityGroupCodeList
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Login,
                    TriggerName = this.GetTriggerName(x.PeriodType, x.StartTimeHour, x.StartTimeMinutes, x.StartDayOfWeekList, x.StartMonthList),
                    CreateDate = x.ObjectCreateDate,
                    x.StartDate,
                    x.EndDate,
                    EntityGroupCodeList = this.GetGroupNameList(x.EntityGroupCodeList, entityGroupDict)
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<FormatDataExportTask> domainService, BaseParams baseParams)
        {
            var task = domainService.Get(baseParams.Params.GetAsId());
            return new BaseDataResult(new
            {
                task.Id,
                task.Operator,
                task.StartDate,
                task.EndDate,
                task.PeriodType,
                CreateDate = task.ObjectCreateDate,
                task.StartNow,
                task.StartTimeHour,
                task.StartTimeMinutes,
                EntityGroupCodeList = this.ExportableEntityGroup.Where(y => task.EntityGroupCodeList.Contains(y.Code))
                    .Select(x => new
                    {
                        x.Code,
                        x.Description
                    }),
                task.IsDelete,
                task.StartDayOfWeekList,
                task.StartMonthList,
                task.StartDaysList,
                task.BaseParams
            });
        }

        private string GetTriggerName(TaskPeriodType periodType,
            int startTimeHour,
            int startTimeMinutes,
            IList<byte> startDayOfWeekList,
            IList<byte> startMonthList)
        {
            switch (periodType)
            {
                case TaskPeriodType.NoPeriodicity:
                    return $"Одноразовый запуск. Время запуска: {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Daily:
                    return $"Ежедневно. Время запуска: {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Weekly:
                    return $"Еженедельно: {startDayOfWeekList.AggregateWithSeparator(this.GetDayOfWeeName, ", ")}. " +
                        $"Время запуска {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Monthly:
                    return $"Ежемесячно: {startMonthList.AggregateWithSeparator(this.GetMonthName, ", ")}. " +
                        $"Время запуска {startTimeHour:D2}:{startTimeMinutes:D2}";

                default:
                    throw new InvalidEnumArgumentException(nameof(periodType), (int)periodType, periodType.GetType());
            }
        }

        private string GetDayOfWeeName(byte dayOfWeek)
        {
            if (dayOfWeek > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            }

            // 1 января 1 г. н.э. был понедельник
            return new DateTime(1, 1, dayOfWeek).ToString("ddd");
        }

        private string GetMonthName(byte monthNumber)
        {
            return new DateTime(1, monthNumber, 1).ToString("MMMM");
        }

        private IList<string> GetGroupNameList(IList<string> entityGroupCodeList, IDictionary<string, string> entityGroupDict)
        {
            return entityGroupCodeList
                .Select(x => entityGroupDict[x])
                .ToList();
        }
    }
}