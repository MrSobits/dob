using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Castle.Windsor;
using System.Collections.Generic;
using System.Linq;


namespace Bars.GkhGji.DomainService
{
    public class SubpoenaService : ISubpoena
    {

        public IWindsorContainer Container { get; set; }

        public IDataResult ComissionListSubpoena(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var comissionId = baseParams.Params.GetAs<int>("comissionId");

            var subpoenaservice = Container.Resolve<IDomainService<Subpoena>>();
            var comissionservice = Container.Resolve<IDomainService<ComissionMeeting>>();

            var datacomission = comissionservice.GetAll()
                .Where(x => x.Id == comissionId)
                .Select(x => x.CommissionDate).ToList();

            var data = subpoenaservice.GetAll()
                .Where(x => datacomission.Contains((System.DateTime)x.DateOfProceedings) )
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateOfProceedings,
                    x.HourOfProceedings,
                    x.ProceedingCopyNum,
                    x.ProceedingsPlace,
                    Comission = x.ComissionMeeting,
                    ComissionName = x.ComissionMeeting.ComissionName
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var subpoenaservice = Container.Resolve<IDomainService<Subpoena>>();

            var data = subpoenaservice.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateOfProceedings,
                    x.HourOfProceedings,
                    x.ProceedingCopyNum,
                    x.ProceedingsPlace,
                    Comission = x.ComissionMeeting,
                    ComissionName = x.ComissionMeeting.ComissionName
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
