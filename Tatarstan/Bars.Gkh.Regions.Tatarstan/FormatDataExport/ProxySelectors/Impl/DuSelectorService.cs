namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="DuProxy"/>
    /// </summary>
    public class DuSelectorService : BaseProxySelectorService<DuProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DuProxy> GetCache()
        {
            var contractOwnersRepository = this.Container.ResolveRepository<ManOrgContractOwners>();
            var contractTransferRepository = this.Container.ResolveRepository<ManOrgContractTransfer>();
            var manOrgAgrContractService = this.Container.ResolveRepository<ManOrgAgrContractService>();

            using (this.Container.Using(contractOwnersRepository,
                contractTransferRepository,
                manOrgAgrContractService))
            {
                var manOrgRealityObjectServices = this.FilterService
                    .FilterByContragent(manOrgAgrContractService.GetAll(),
                        x => x.Contract.ManagingOrganization.Contragent)
                    .Select(x => new
                    {
                        x.Contract.Id,
                        ServiceId = this.GetId(x.Service),
                        x.PaymentAmount,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault(y => y.ServiceId.HasValue));

                var manOrgRoDict = this.ProxySelectorFactory.GetSelector<RealityObjectByContract>()
                    .ProxyListCache.Values
                    .ToDictionary(x => x.Id, x => x.RealityObjectId);

                var manOrgContractOwnersList = this.FilterService
                    .FilterByContragent(contractOwnersRepository.GetAll(),
                        x => x.ManagingOrganization.Contragent)
                    .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today)
                    .WhereContainsBulked(x => x.Id, manOrgRoDict.Keys)
                    .Select(x => new
                    {
                        x.Id,
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
                        x.PaymentAmount,
                        x.PaymentProtocolFile
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var service = manOrgRealityObjectServices.Get(x.Id);
                        return new DuProxy
                        {
                            //DU
                            Id = x.Id,
                            ContragentId = x.ContragentId,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate ?? x.StartDate,
                            StartDate = x.StartDate,
                            PlannedEndDate = x.PlannedEndDate ?? x.StartDate.Value.AddYears(5),
                            Owner = 1,
                            ContractFoundation = 1,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            DrawingPaymentDocumentMonth = x.ThisMonthPaymentServiceDate ? 1 : 2,
                            Status = x.EndDate.HasValue ? 4 : 1,
                            TerminationDate = x.EndDate.HasValue ? x.TerminationDate : null,
                            TerminationReason = x.EndDate.HasValue ? this.GetTerminationReason(x.ContractStopReason) : null,
                            CancellationReason = x.TerminateReason,

                            //DUFILES
                            DuFile = x.FileInfo,
                            OssFile = x.ProtocolFileInfo,
                            OwnerFile = x.FileInfo,

                            //DUCHARGE
                            ChargeStatus = x.ContractStopReason ==  ContractStopReasonEnum.is_not_filled ? 1 : 2,
                            StartDatePaymentPeriod = x.StartDatePaymentPeriod,
                            EndDatePaymentPeriod = x.EndDatePaymentPeriod,
                            PaymentAmount = x.PaymentAmount,
                            PaymentProtocolFile = x.FileInfo,
                            ServiceId = service?.ServiceId,
                            ServicePayment = service?.PaymentAmount,
                            SetPaymentsFoundation = 1,
                            RealityObjectId = manOrgRoDict[x.Id]
                        };
                    })
                    .ToList();

                var manOrgContractTransferList = this.FilterService
                    .FilterByContragent(contractTransferRepository.GetAll(),
                        x => x.ManagingOrganization.Contragent)
                    .WhereContainsBulked(x => x.Id, manOrgRoDict.Keys)
                    .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today)
                    .Select(x => new
                    {
                        x.Id,
                        ContragentId = this.GetId(x.ManagingOrganization.Contragent),
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.StartDate,
                        x.PlannedEndDate,
                        ContragentOwnerId = this.GetId(x.ManOrgJskTsj.Contragent),
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
                        x.PaymentAmount,
                        x.PaymentProtocolFile
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var service = manOrgRealityObjectServices.Get(x.Id);
                        return new DuProxy
                        {
                            //DU
                            Id = x.Id,
                            ContragentId = x.ContragentId,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate ?? x.StartDate,
                            StartDate = x.StartDate,
                            PlannedEndDate = x.PlannedEndDate ?? x.StartDate.Value.AddYears(5),
                            Owner = 2,
                            ContragentOwnerId = x.ContragentOwnerId,
                            ContragentOwnerType = x.ContragentOwnerId.HasValue ? 1 : (int?)null,
                            ContractFoundation = 1,
                            InputMeteringDeviceValuesBeginDay = x.InputMeteringDeviceValuesBeginDate,
                            InputMeteringDeviceValuesEndDay = x.InputMeteringDeviceValuesEndDate,
                            DrawingPaymentDocumentDay = x.DrawingPaymentDocumentDate,
                            DrawingPaymentDocumentMonth = x.ThisMonthPaymentServiceDate ? 1 : 2,
                            Status = x.EndDate.HasValue ? 4 : 1,
                            TerminationDate = x.EndDate.HasValue ? x.TerminationDate : null,
                            TerminationReason = x.EndDate.HasValue ? this.GetTerminationReason(x.ContractStopReason) : null,
                            CancellationReason = x.TerminateReason,

                            //DUFILES
                            DuFile = x.FileInfo,
                            OssFile = x.ProtocolFileInfo,
                            OwnerFile = x.FileInfo,

                            //DUCHARGE
                            ChargeStatus = x.ContractStopReason == ContractStopReasonEnum.is_not_filled ? 1 : 2,
                            StartDatePaymentPeriod = x.StartDatePaymentPeriod,
                            EndDatePaymentPeriod = x.EndDatePaymentPeriod,
                            PaymentAmount = x.PaymentAmount,
                            PaymentProtocolFile = x.FileInfo,
                            ServiceId = service?.ServiceId,
                            ServicePayment = service?.PaymentAmount,
                            SetPaymentsFoundation = 1,
                            RealityObjectId = manOrgRoDict[x.Id]
                        };
                    })
                    .ToList();

                return manOrgContractOwnersList
                    .Union(manOrgContractTransferList)
                    .ToDictionary(x => x.Id);
            }
        }

        private int? GetTerminationReason(ContractStopReasonEnum reason)
        {
            switch (reason)
            {
                case ContractStopReasonEnum.added_by_error:
                    return 2;
                case ContractStopReasonEnum.finished_contract:
                case ContractStopReasonEnum.is_not_filled:
                    return 5;
                case ContractStopReasonEnum.is_excluded_decision:
                    return 7;
                case ContractStopReasonEnum.revocation_of_license:
                    return 1;
                case ContractStopReasonEnum.is_excluded_refusal:
                    return 6;
                default:
                    return null;
            }
        }
    }
}