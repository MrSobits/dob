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
    /// Селектор протоколов проверок
    /// </summary>
    public class ProtocolFilesSelectorService : BaseProxySelectorService<ProtocolFilesProxy>
    {
        #region Приложения к документам ГЖИ
        /// <summary>
        /// Приложение <see cref="ProtocolAnnex"/>
        /// </summary>
        public IRepository<ProtocolAnnex> ProtocolAnnexRepository { get; set; }
        #endregion

        /// <inheritdoc />
        protected override IDictionary<long, ProtocolFilesProxy> GetCache()
        {
            return this.ProtocolAnnexRepository.GetAll()
                .WhereNotNull(x => x.File)
                .Select(x => new ProtocolFilesProxy
                {
                    File = x.File,
                    DocumentGjiId = x.Protocol.Id,
                    Type = 1
                })
                .ToDictionary(x => x.Id);
        }
    }
}