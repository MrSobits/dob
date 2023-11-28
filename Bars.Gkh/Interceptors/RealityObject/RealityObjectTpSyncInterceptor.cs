namespace Bars.Gkh.Interceptors
{
    using System.Web;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.DomainService;

    public class RealityObjectTpSyncInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        public IRealityObjectTpSyncService Service { get; set; }

        /*public override IDataResult BeforeUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            return HttpContext.Current.Session["noSync"].ToBool() || HttpContext.Current.Request["skipSyncValidation"].ToBool() ? new BaseDataResult() : this.Service.Validate(entity);
        }*/

        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            ///Добавил проверку HttpContext.Current == null, потому, что в тестах нет HttpContext.Current
            return HttpContext.Current == null || HttpContext.Current.Session["noSync"].ToBool() ? new BaseDataResult() : this.Service.Sync(entity);
        }
    }
}