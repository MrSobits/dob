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
    /// Селектор предписаний проверок
    /// </summary>
    public class PreceptFilesSelectorService : BaseProxySelectorService<PreceptFilesProxy>
    {
        #region Приложения к документам ГЖИ
        /// <summary>
        /// Приложение <see cref="PrescriptionAnnex"/>
        /// </summary>
        public IRepository<PrescriptionAnnex> PrescriptionAnnexRepository { get; set; }
        #endregion

        /// <inheritdoc />
        protected override IDictionary<long, PreceptFilesProxy> GetCache()
        {
            return this.PrescriptionAnnexRepository.GetAll()
                .WhereNotNull(x => x.File)
                .Select(x => new PreceptFilesProxy
                {
                    File = x.File,
                    DocumentGjiId = x.Prescription.Id,
                    Type = 1
                })
                .ToDictionary(x => x.Id);
        }
    }
}