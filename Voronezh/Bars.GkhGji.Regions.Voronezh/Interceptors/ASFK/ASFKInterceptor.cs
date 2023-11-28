namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    class ASFKInterceptor : EmptyDomainInterceptor<ASFK>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ASFK> service, ASFK entity)
        {
            var vtoperDomain = this.Container.ResolveDomain<VTOPER>();
            var bdoperDomain = this.Container.ResolveDomain<BDOPER>();

            using (this.Container.Using(vtoperDomain))
            {
                //Удаляем связанные VTOPER-ы
                vtoperDomain.GetAll()
                    .Where(x => x.ASFK != null && x.ASFK.Id == entity.Id)
                    .ForEach(x => vtoperDomain.Delete(x.Id));
            }

            using (this.Container.Using(bdoperDomain))
            {
                //Удаляем связанные BDOPER-ы
                bdoperDomain.GetAll()
                    .Where(x => x.ASFK != null && x.ASFK.Id == entity.Id)
                    .ForEach(x => bdoperDomain.Delete(x.Id));
            }

            return this.Success();
        }
    }
}
