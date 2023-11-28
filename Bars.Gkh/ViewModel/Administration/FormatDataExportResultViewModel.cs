namespace Bars.Gkh.ViewModel.Administration
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Utils;

    public class FormatDataExportResultViewModel : BaseViewModel<FormatDataExportResult>
    {
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<FormatDataExportResult> domainService, BaseParams baseParams)
        {
            var activeOperator = this.GkhUserManager.GetActiveOperator()?.Id ?? 0;

            var isAdministrator = this.GkhUserManager.GetActiveUser().Roles.Any(x => x.Role.Name == "Администратор");

            return domainService.GetAll()
                .WhereIf(!isAdministrator, x => x.Task.Operator.Id == activeOperator)
                .Select(x => new
                {
                    x.Id,
                    x.Task.Operator.User.Login,
                    x.Status,
                    x.StartDate,
                    x.EndDate,
                    x.Progress,
                    x.LogOperation.LogFile,
                    x.EntityCodeList
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}