namespace Bars.Gkh.Interceptors
{
    using System.Web;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class TpRealityObjectSyncInterceptor : EmptyDomainInterceptor<TehPassportValue>
    {
        public IRealityObjectTpSyncService Service { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<TehPassportValue> service, TehPassportValue entity)
        {
            return this.Sync(entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<TehPassportValue> service, TehPassportValue entity)
        {
            return this.Sync(entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<TehPassportValue> service, TehPassportValue entity)
        {
            return this.Sync(entity);
        }

        private IDataResult Sync(TehPassportValue entity)
        {
            return HttpContext.Current == null || HttpContext.Current.Session["noSync"].ToBool() ? new BaseDataResult() : this.Service.Sync(entity);
        }
    }
}