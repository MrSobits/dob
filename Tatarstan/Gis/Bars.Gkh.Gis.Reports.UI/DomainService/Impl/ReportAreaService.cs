using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Gis.Reports.UI.DomainService.Impl
{
    using B4;
    using B4.Modules.Dapper.Repository;
    using Castle.Windsor;
    using Entities;

    public class ReportAreaService : IReportAreaService
    {
        public IWindsorContainer Container { get; set; }

        public IDapperRepository<ReportArea> DomainService { get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = DomainService.GetAll().AsQueryable().Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }
    }
}
