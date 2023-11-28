namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;

    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService.PersonalAccount;

    public class DebtorExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IDebtorService>();

            try
            {
                int totalCount;
                return service.GetList(baseParams, false, out totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}