namespace Bars.GkhRf.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class GisuRealObjContract : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        protected List<long> municipalityIds = new List<long>();

        public GisuRealObjContract(ReportTemplateBinary tpl) : base(tpl) { }

        public GisuRealObjContract()
            : base(new ReportTemplateBinary(Properties.Resources.GisuRObjectContract))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.GisuRealObjContract";
            }
        }

        public override string Name
        {
            get { return "Отчет по домам, включенным в договор с ГИСУ"; }
        }

        public override string Desciption
        {
            get { return "Отчет по домам, включенным в договор с ГИСУ"; }
        }

        public override string GroupName
        {
            get { return "Отчеты Рег.Фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.GisuRealObjContract"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityIds.Clear();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (!string.IsNullOrEmpty(municipalityStr))
            {
                var mcp = municipalityStr.Split(',');
                foreach (var id in mcp)
                {
                    long mcpId = 0;
                    if (long.TryParse(id, out mcpId))
                    {
                        if (!municipalityIds.Contains(mcpId))
                        {
                            municipalityIds.Add(mcpId);
                        }
                    }
                }
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            var houses = this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    MunName = x.RealityObject.Municipality.Name,
                    ContrName = x.ContractRf.ManagingOrganization.Contragent.Name,
                    x.RealityObject.Address,
                    x.ContractRf.DocumentNum,
                    x.ContractRf.DocumentDate,
                    x.IncludeDate,
                    x.TypeCondition,
                    x.ExcludeDate
                })
                .ToList();

            foreach (var house in houses)
            {
                section.ДобавитьСтроку();

                section["Район"] = house.MunName;
                section["УправляющаяОрганизация"] = house.ContrName ?? string.Empty;
                section["Адрес"] = house.Address;
                section["НомерДоговора"] = house.DocumentNum;
                section["ДатаДоговора"] = house.DocumentDate;
                section["ДатаВключенияВДоговор"] = house.IncludeDate;
                section["ДатаИсключенияИзДоговора"] = house.TypeCondition == TypeCondition.Exclude
                    ? house.ExcludeDate.HasValue ? house.ExcludeDate.Value.ToShortDateString() : string.Empty
                    : string.Empty;
            }

        }
    }
}