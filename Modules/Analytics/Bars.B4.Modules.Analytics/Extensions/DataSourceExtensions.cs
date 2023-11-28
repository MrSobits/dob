namespace Bars.B4.Modules.Analytics.Extensions
{
    using System;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Domain;

    /// <summary>
    /// 
    /// </summary>
    public static class DataSourceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public static object GetData(this IDataSource dataSource, BaseParams baseParams)
        {
            var container = ApplicationContext.Current.Container;
            var provider = container.Resolve<IDataProviderService>().Get(dataSource.ProviderKey);
            return provider != null ? provider.ProvideData(dataSource.DataFilter, baseParams) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetMetaInfo(this IDataSource dataSource)
        {
            var container = ApplicationContext.Current.Container;
            var provider = container.Resolve<IDataProviderService>().Get(dataSource.ProviderKey);
            return provider.GetMeta();
        }

    }
}
