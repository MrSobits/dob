namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.NetworkWorker;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис экспорта в формате 4.0.X
    /// </summary>
    public class FormatDataExportService : IFormatDataExportService
    {
        public IWindsorContainer Container { get;set; }

        public IFormatDataTransferService FormatDataTransferService { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroup { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }

        /// <inheritdoc />
        public IDataResult ListAvailableSection(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var providerFlag =
                this.FormatDataExportRoleService.GetCustomProviderFlags(this.GkhUserManager.GetActiveOperator());

            return this.ExportableEntityGroup.Where(x => providerFlag.CheckFlags(x.AllowProviderFlags))
                .Select(x => new
                {
                    x.Code,
                    x.Description,
                    x.InheritedEntityCodeList
                })
                .ToListDataResult(loadParams, this.Container);
        }

        /// <inheritdoc />
        public IDataResult GetRemoteStatus(BaseParams baseParams)
        {
            var statusId = baseParams.Params.GetAsId();
            if (statusId == 0)
            {
                return BaseDataResult.Error("Отсутствует идентификатор загрузки");
            }

            return this.FormatDataTransferService.GetStatus(statusId);
        }

        /// <inheritdoc />
        public IDataResult StartRemoteImport(BaseParams baseParams)
        {
            var fileId = baseParams.Params.GetAsId();
            if (fileId == 0)
            {
                return BaseDataResult.Error("Отсутствует идентификатор загрузки");
            }

            return this.FormatDataTransferService.StartImport(fileId, CancellationToken.None);
        }

        /// <inheritdoc />
        public IDataResult GetRemoteFile(BaseParams baseParams)
        {
            var fileId = baseParams.Params.GetAsId();
            if (fileId == 0)
            {
                return BaseDataResult.Error("Отсутствует идентификатор удаленного файла");
            }

            return this.FormatDataTransferService.GetFile(fileId);
        }

        /// <inheritdoc />
        public IDataResult UpdateRemoteStatus(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }
    }
}