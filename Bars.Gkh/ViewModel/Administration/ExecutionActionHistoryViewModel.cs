namespace Bars.Gkh.ViewModel.Administration
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Журнал выполнения действий
    /// </summary>
    public class ExecutionActionHistoryViewModel : BaseViewModel<ExecutionActionHistory>
    {
        public IExecutionActionInfoService ExecutionActionInfoService { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<ExecutionActionHistory> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    Name = this.ExecutionActionInfoService.GetInfo(x.Code)?.Name ?? x.Code,
                    this.ExecutionActionInfoService.GetInfo(x.Code)?.Description,
                    CreateDate = x.CreateDate.ToUniversalTime(),
                    StartDate = x.StartDate.ToUniversalTime(),
                    EndDate = x.EndDate.ToUniversalTime(),
                    x.DataResult,
                    x.Status,
                    Duration = x.EndDate.IsValid() && x.StartDate.IsValid()
                        ? x.EndDate - x.StartDate
                        : new TimeSpan(0)
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}