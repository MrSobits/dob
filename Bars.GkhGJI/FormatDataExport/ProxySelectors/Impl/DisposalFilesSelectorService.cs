namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Селектор распоряжений проверок
    /// </summary>
    public class DisposalFilesSelectorService : BaseProxySelectorService<DisposalFilesProxy>
    {
        #region Приложения к документам ГЖИ
        /// <summary>
        /// Приложение <see cref="DisposalAnnex"/>
        /// </summary>
        public IRepository<DisposalAnnex> DisposalAnnexRepository { get; set; }
        #endregion

        /// <inheritdoc />
        protected override IDictionary<long, DisposalFilesProxy> GetCache()
        {
            return this.DisposalAnnexRepository.GetAll()
                .WhereNotNull(x => x.File)
                .Select(x => new DisposalFilesProxy
                {
                    File = x.File,
                    DocumentGjiId = x.Disposal.Id,
                    Type = 1
                })
                .ToDictionary(x => x.Id);
        }
    }
}