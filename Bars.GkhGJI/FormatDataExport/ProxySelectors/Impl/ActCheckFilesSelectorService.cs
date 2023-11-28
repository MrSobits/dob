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
    /// Селектор актов проверки
    /// </summary>
    public class ActCheckFilesSelectorService : BaseProxySelectorService<ActCheckFilesProxy>
    {
        #region Приложения к документам ГЖИ
        /// <summary>
        /// Приложение <see cref="ActCheckAnnex"/>
        /// </summary>
        public IRepository<ActCheckAnnex> ActCheckAnnexRepository { get; set; }
        #endregion

        /// <inheritdoc />
        protected override IDictionary<long, ActCheckFilesProxy> GetCache()
        {
            return this.ActCheckAnnexRepository.GetAll()
                .WhereNotNull(x => x.File)
                .Select(x => new ActCheckFilesProxy
                {
                    File = x.File,
                    DocumentGjiId = x.ActCheck.Id,
                    Type = 1
                })
                .ToDictionary(x => x.Id);
        }
    }
}