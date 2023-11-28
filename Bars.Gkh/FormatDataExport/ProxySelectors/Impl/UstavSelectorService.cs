namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="UstavProxy"/>
    /// </summary>
    public class UstavSelectorService : BaseProxySelectorService<UstavProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, UstavProxy> GetCache()
        {
            var jskTsjContractRepository = this.Container.ResolveRepository<ManOrgJskTsjContract>();
            var roDirectManagContractRepository = this.Container.ResolveRepository<RealityObjectDirectManagContract>();
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();

            using (this.Container.Using(jskTsjContractRepository,
                roDirectManagContractRepository,
                contragentContactRepository))
            {
                var manOrgRoDict = this.ProxySelectorFactory.GetSelector<RealityObjectByContract>()
                    .ProxyListCache.Values
                    .ToDictionary(x => x.Id, x => x.RealityObjectId);

                var leaderId = this.SelectParams.GetAsId("LeaderPositionId");
                var tsjChairmanPositionId = this.SelectParams.GetAsId("TsjChairmanPosition");
                var tsjMemberPositionId = this.SelectParams.GetAsId("TsjMemberPositionId");

                var contactDict = this.FilterService
                    .FilterByContragent(contragentContactRepository.GetAll(), x => x.Contragent)
                    .WhereContains(x => x.Position.Id, new [] { leaderId, tsjChairmanPositionId, tsjMemberPositionId })
                    .Select(x => new
                    {
                        Id = this.GetId(x.Contragent),
                        x.FullName
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.FullName)
                    .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(", "));

                var jskTsjContractList = this.FilterService
                    .FilterByContragent(jskTsjContractRepository.GetAll(), x => x.ManagingOrganization.Contragent)
                    .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today)
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeContractManOrgRealObj,
                        ContragentId = this.GetId(x.ManagingOrganization.Contragent),
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.PlannedEndDate,
                        x.InputMeteringDeviceValuesBeginDate,
                        x.InputMeteringDeviceValuesEndDate,
                        x.DrawingPaymentDocumentDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.TerminationDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.ProtocolFileInfo,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.PaymentProtocolFile,
                        x.ThisMonthPaymentDocDate,
                        x.PaymentServicePeriodDate,
                        x.ManagingOrganization.Contragent.ContragentState,
                        x.CompanyReqiredPaymentAmount,
                        x.ReqiredPaymentAmount
                    })
                    .AsEnumerable()
                    .Select(x => new UstavProxy
                    {
                        //USTAV
                        Id = x.Id,
                        ContragentId = x.ContragentId,
                        DocumentNumber = x.DocumentNumber,
                        DocumentDate = x.DocumentDate ?? x.StartDate,
                        InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                        InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                        DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                        ThisMonthPaymentDocDate = x.ThisMonthPaymentDocDate ? 1 : 2,
                        PaymentServicePeriodDay = x.PaymentServicePeriodDate,
                        ThisMonthPaymentServiceDate = x.ThisMonthPaymentServiceDate ? 1 : 2,
                        LegalContragentId = x.ContragentId,
                        Management = contactDict.Get(x.ContragentId),
                        Status = x.ContragentState == ContragentState.Active ? 1 : 2,

                        //USTAVFILES
                        OssFile = x.FileInfo,
                        UstavFile = x.ProtocolFileInfo,

                        //USTAVOU
                        StartDate = x.StartDate,

                        //USTAVCHARGE
                        TypeContract = x.TypeContractManOrgRealObj,
                        PaymentInfo = 2,
                        StartDatePaymentPeriod = x.StartDatePaymentPeriod,
                        EndDatePaymentPeriod = x.EndDatePaymentPeriod,
                        CompanyReqiredPaymentAmount = x.CompanyReqiredPaymentAmount,
                        ReqiredPaymentAmount = x.ReqiredPaymentAmount,
                        PaymentProtocolFile = x.PaymentProtocolFile,

                        RealityObjectId = manOrgRoDict[x.Id]
                    })
                    .ToList();

                var roDirectManagContractList = this.FilterService
                    .FilterByContragent(roDirectManagContractRepository.GetAll(), x => x.ManagingOrganization.Contragent)
                    .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today)
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeContractManOrgRealObj,
                        ContragentId = this.GetId(x.ManagingOrganization.Contragent),
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.PlannedEndDate,
                        x.ThisMonthPaymentServiceDate,
                        x.EndDate,
                        x.ContractStopReason,
                        x.TerminateReason,
                        x.FileInfo,
                        x.StartDatePaymentPeriod,
                        x.EndDatePaymentPeriod,
                        x.ThisMonthPaymentDocDate,
                        x.PaymentServicePeriodDate,
                        x.ManagingOrganization.Contragent.ContragentState
                    })
                    .AsEnumerable()
                    .Select(x => new UstavProxy
                    {
                        //USTAV
                        Id = x.Id,
                        ContragentId = x.ContragentId,
                        DocumentNumber = x.DocumentNumber,
                        DocumentDate = x.DocumentDate ?? x.StartDate,
                        ThisMonthPaymentDocDate = x.ThisMonthPaymentDocDate ? 1 : 2,
                        PaymentServicePeriodDay = x.PaymentServicePeriodDate,
                        ThisMonthPaymentServiceDate = x.ThisMonthPaymentServiceDate ? 1 : 2,
                        LegalContragentId = x.ContragentId,
                        Management = contactDict.Get(x.ContragentId),
                        Status = x.ContragentState == ContragentState.Active ? 1 : 2,

                        //USTAVFILES
                        OssFile = x.FileInfo,

                        //USTAVOU
                        StartDate = x.StartDate,

                        //USTAVCHARGE
                        TypeContract = x.TypeContractManOrgRealObj,
                        PaymentInfo = 2,
                        StartDatePaymentPeriod = x.StartDatePaymentPeriod,
                        EndDatePaymentPeriod = x.EndDatePaymentPeriod,

                        RealityObjectId = manOrgRoDict[x.Id]
                    })
                    .ToList();

                return jskTsjContractList
                    .Union(roDirectManagContractList)
                    .ToDictionary(x => x.Id);
            }
        }
    }
}