namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    using Castle.Windsor;

    public class SpecialAccountDecisionReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<SpecialAccountDecision> SpecialAccountDecisionDomain { get; set; }

        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }

        public IRealityObjectsProgramVersion RealObjProgramVersion { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        private YesNoNotSet _hasInDpkr;

        public SpecialAccountDecisionReport()
            : base(new ReportTemplateBinary(Properties.Resources.SpecialAccountDecision))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Реестр специальных счетов";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр специальных счетов";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.SpecialAccountDecision";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.SpecialAccountDecisionReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var date = baseParams.Params.GetAs<DateTime?>("dateTimeReport");

            dateTimeReport = date ?? DateTime.Now.Date;

            _hasInDpkr = baseParams.Params.GetAs("hasInDpkr", YesNoNotSet.NotSet);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roInDpkrQuery = RealObjProgramVersion.GetMainVersionRealityObjects();

            var ids = BasePropertyOwnerDecisionDomain.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.PropertyOwnerProtocol != null && x.PropertyOwnerProtocol.DocumentDate != null)
                .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Select(x => new
                {
                    x.PropertyOwnerProtocol.DocumentDate,
                    RoId = x.RealityObject.Id,
                    x.MethodFormFund,
                    x.Id
                })
                .ToList()
                .GroupBy(x => x.RoId)
                .Select(x => new
                {
                    x.Key,
                    x.OrderByDescending(z => z.DocumentDate).First().MethodFormFund,
                    x.OrderByDescending(z => z.DocumentDate).First().Id
                })
                .Where(x => x.MethodFormFund.HasValue && x.MethodFormFund.Value == MethodFormFundCr.SpecialAccount)
                .Select(x => x.Id)
                .ToArray();

            var records = SpecialAccountDecisionDomain.GetAll()
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Where(x => x.MethodFormFund == MethodFormFundCr.SpecialAccount)
                .Where(x => ids.Contains(x.Id))
                .WhereIf(_hasInDpkr == YesNoNotSet.Yes, x => roInDpkrQuery.Any(y => y.Id == x.RealityObject.Id))
                .WhereIf(_hasInDpkr == YesNoNotSet.No,
                    x =>
                        !roInDpkrQuery.Any(y => y.Id == x.RealityObject.Id) &&
                        (x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated || x.RealityObject.ConditionHouse == ConditionHouse.Serviceable))
                .WhereIf(_hasInDpkr == YesNoNotSet.NotSet, x => x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated || x.RealityObject.ConditionHouse == ConditionHouse.Serviceable)
                .Select(x => new
                {
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    x.TypeOrganization,
                    ManOrgName = x.ManagingOrganization.Contragent.Name,
                    RegOpName = x.RegOperator.Contragent.Name,
                    x.AccountNumber,
                    x.OpenDate,
                    x.CloseDate,
                    BankName = x.CreditOrg.Name,
                    MailingAddress = x.MailingAddress.AddressName,
                    x.Inn,
                    x.Kpp,
                    x.Ogrn,
                    x.Bik,
                    x.Okpo,
                    x.CorrAccount
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Address)
                .ToArray();

            reportParams.SimpleReportParams["DateTimeReport"] = dateTimeReport.Date.ToShortDateString();

            if (!records.Any())
            {
                return;
            }

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;
            foreach (var record in records)
            {
                sect.ДобавитьСтроку();

                var ownerAccountName = "Непосредственное управление";
                switch (record.TypeOrganization)
                {
                    case TypeOrganization.TSJ:
                    case TypeOrganization.JSK:
                        ownerAccountName = record.ManOrgName;
                        break;
                    case TypeOrganization.RegOperator:
                        ownerAccountName = record.RegOpName;
                        break;
                }

                sect["row"] = i++;
                sect["Mu"] = record.Municipality;
                sect["Address"] = record.Address;
                sect["OwnerAccountName"] = ownerAccountName;
                sect["AccountNumber"] = record.AccountNumber;
                sect["OpenDate"] = record.OpenDate;
                sect["CloseDate"] = record.CloseDate;
                sect["BankName"] = record.BankName;
                sect["MailingAddress"] = record.MailingAddress;
                sect["Inn"] = record.Inn;
                sect["Kpp"] = record.Kpp;
                sect["Ogrn"] = record.Ogrn;
                sect["Bik"] = record.Bik;
                sect["Okpo"] = record.Okpo;
                sect["CorrAccount"] = record.CorrAccount;
            }
        }
    }
}