namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Сервис получения <see cref="UstavOuUslugaProxy"/>
    /// </summary>
    public class UstavOuUslugaSelectorService : BaseProxySelectorService<UstavOuUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, UstavOuUslugaProxy> GetCache()
        {
            throw new NotImplementedException("Экспорт секции USTAVOUUSLUGA не реализован");
        }
    }
}