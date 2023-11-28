namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;


    public class SubpoenaInterceptor : SubpoenaInterceptor<Subpoena>
    {
    }


    public class SubpoenaInterceptor<T> : EmptyDomainInterceptor<T>
        where T : Subpoena
    {

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var ComissionMeetingDomain = this.Container.Resolve<IDomainService<ComissionMeeting>>();

            var comission = ComissionMeetingDomain.GetAll()
                    .FirstOrDefault(x => x.CommissionDate == entity.DateOfProceedings);

            entity.ComissionMeeting = comission;

            return base.BeforeUpdateAction(service, entity);
        }
    }
}