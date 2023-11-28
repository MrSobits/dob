namespace Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.IoC;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис получения актуальных управляющих организаций по домам
    /// </summary>
    public class ActualManOrgByRealityObjectSelectorService : BaseProxySelectorService<ActualManOrgByRealityObject>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ActualManOrgByRealityObject> GetCache()
        {
            var realityObjectManOrgService = this.Container.Resolve<IRealityObjectManOrgService>();

            using (this.Container.Using(realityObjectManOrgService))
            {
                return this.FilterService.FilterByContragent(
                        realityObjectManOrgService.GetActualManagingOrganizations(),
                        x => x.Value.Contragent)
                    .ToDictionary(x => x.Key, x => ActualManOrgByRealityObject.FromBase(x.Value));
            }
        }
    }

    public class ActualManOrgByRealityObject : ManagingOrganization
    {
        public static ActualManOrgByRealityObject FromBase(ManagingOrganization mo)
        {
            var result = new ActualManOrgByRealityObject();

            foreach (var prop in mo.GetType().GetProperties())
                typeof(ActualManOrgByRealityObject)
                    .GetProperty(prop.Name)
                    .SetValue(result, prop.GetValue(mo, null), null);

            return result;
        }
    }
}