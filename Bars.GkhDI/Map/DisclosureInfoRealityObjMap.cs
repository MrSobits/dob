/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Управляемый объект"
///     /// </summary>
///     public class DisclosureInfoRealityObjMap : BaseGkhEntityMap<DisclosureInfoRealityObj>
///     {
///         public DisclosureInfoRealityObjMap() : base("DI_DISINFO_REALOBJ")
///         {
///             Map(x => x.ReductionPayment, "REDUCTION_PAYMENT").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.NonResidentialPlacement, "NON_RESIDENT_PLACE").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.PlaceGeneralUse, "PLACE_GENERAL_USE").CustomType<YesNoNotSet>().Not.Nullable();
/// 
///             //Общие сведения о доме
///             Map(x => x.ClaimCompensationDamage, "CLAIM_COMPENSATION");
///             Map(x => x.ClaimReductionPaymentNonService, "CLAIM_NON_SERVICE");
///             Map(x => x.ClaimReductionPaymentNonDelivery, "CLAIM_NON_DELIVERY");
///             Map(x => x.ExecutionWork, "EXECUTION_WORK").Length(3000);
///             Map(x => x.ExecutionObligation, "EXECUTION_OBLIGATION").Length(3000);
///             Map(x => x.DescriptionServiceCatalogRepair, "DESCR_CATREP_SERV").Length(3000);
///             Map(x => x.DescriptionTariffCatalogRepair, "DESCR_CATREP_TARIF").Length(3000);
///             References(x => x.FileExecutionWork, "FILE_EXECUTION_WORK").Fetch.Join();
///             References(x => x.FileExecutionObligation, "FILE_ECXECUTION_OBLIG").Fetch.Join();
///             References(x => x.FileServiceCatalogRepair, "FILE_SERV_CAT_REPORT").Fetch.Join();
///             References(x => x.FileTariffCatalogRepair, "FILE_TARIF_CAT_REPORT").Fetch.Join();
/// 
///             //Фин показатели по ремонту содержанию дома
///             Map(x => x.WorkRepair, "WORK_REPAIR");
///             Map(x => x.WorkLandscaping, "WORK_LANDSCAPING");
///             Map(x => x.Subsidies, "SUBSIDIES");
///             Map(x => x.Credit, "CREDIT");
///             Map(x => x.FinanceLeasingContract, "FINANCE_LEASING");
///             Map(x => x.FinanceEnergServContract, "FINANCE_ENERGY");
///             Map(x => x.OccupantContribution, "OCCUPANT_CONTRIB");
///             Map(x => x.OtherSource, "OTHER_SOURCE");
/// 
///             Map(x => x.ReceivedPretensionCount, "RECEIVE_PRETENSION_CNT");
///             Map(x => x.ApprovedPretensionCount, "APROVE_PRETENSION_CNT");
///             Map(x => x.NoApprovedPretensionCount, "NO_APROVE_PRETENSION_CNT");
///             Map(x => x.PretensionRecalcSum, "PRETENSION_RECALC_SUM");
///             Map(x => x.SentPretensionCount, "SENT_PRETENSION_CNT");
///             Map(x => x.SentPetitionCount, "SENT_PETITION_CNT");
///             Map(x => x.ReceiveSumByClaimWork, "RECEIVE_SUM_BY_CLW");
/// 
///             Map(x => x.AdvancePayments, "ADVANCE_PAYMENTS").Nullable();
///             Map(x => x.CarryOverFunds, "CARRYOVER_FUNDS").Nullable();
///             Map(x => x.Debt, "DEBT").Nullable();
///             Map(x => x.ChargeForMaintenanceAndRepairsMaintanance, "CFMAR_MAINTANCE").Nullable();
///             Map(x => x.ChargeForMaintenanceAndRepairsRepairs, "CFMAR_REPAIRS").Nullable();
///             Map(x => x.ReceivedCashFromOwners, "RC_FOWNERS").Nullable();
///             Map(x => x.ReceivedCashFromOwnersTargeted, "RC_FOTARGETED").Nullable();
///             Map(x => x.ReceivedCashAsGrant, "RC_GRANT").Nullable();
///             Map(x => x.ReceivedCashFromUsingCommonProperty, "RC_FCOMMONPROP").Nullable();
///             Map(x => x.ReceivedCashFromOtherTypeOfPayments, "RC_FOTHER").Nullable();
///             Map(x => x.CashBalanceAdvancePayments, "CB_ADVANCE").Nullable();
///             Map(x => x.CashBalanceCarryOverFunds, "CB_CARRYOVER").Nullable();
///             Map(x => x.CashBalanceDebt, "CB_DEBT").Nullable();
/// 
///             Map(x => x.CashBalanceAll, "ALL_CASH_BALANCE").Nullable();
///             Map(x => x.ReceivedCashAll, "ALL_RECEIVED_CASH").Nullable();
/// 
///             References(x => x.PeriodDi, "PERIOD_DI_ID").Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DisclosureInfoRealityObj"</summary>
    public class DisclosureInfoRealityObjMap : BaseImportableEntityMap<DisclosureInfoRealityObj>
    {
        
        public DisclosureInfoRealityObjMap() : 
                base("Bars.GkhDi.Entities.DisclosureInfoRealityObj", "DI_DISINFO_REALOBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ReductionPayment, "ReductionPayment").Column("REDUCTION_PAYMENT").NotNull();
            Property(x => x.NonResidentialPlacement, "NonResidentialPlacement").Column("NON_RESIDENT_PLACE").NotNull();
            Property(x => x.PlaceGeneralUse, "PlaceGeneralUse").Column("PLACE_GENERAL_USE").NotNull();
            Property(x => x.ClaimCompensationDamage, "ClaimCompensationDamage").Column("CLAIM_COMPENSATION");
            Property(x => x.ClaimReductionPaymentNonService, "ClaimReductionPaymentNonService").Column("CLAIM_NON_SERVICE");
            Property(x => x.ClaimReductionPaymentNonDelivery, "ClaimReductionPaymentNonDelivery").Column("CLAIM_NON_DELIVERY");
            Property(x => x.ExecutionWork, "ExecutionWork").Column("EXECUTION_WORK").Length(3000);
            Property(x => x.ExecutionObligation, "ExecutionObligation").Column("EXECUTION_OBLIGATION").Length(3000);
            Property(x => x.DescriptionServiceCatalogRepair, "DescriptionServiceCatalogRepair").Column("DESCR_CATREP_SERV").Length(3000);
            Property(x => x.DescriptionTariffCatalogRepair, "DescriptionTariffCatalogRepair").Column("DESCR_CATREP_TARIF").Length(3000);
            Property(x => x.WorkRepair, "WorkRepair").Column("WORK_REPAIR");
            Property(x => x.WorkLandscaping, "WorkLandscaping").Column("WORK_LANDSCAPING");
            Property(x => x.Subsidies, "Subsidies").Column("SUBSIDIES");
            Property(x => x.Credit, "Credit").Column("CREDIT");
            Property(x => x.FinanceLeasingContract, "FinanceLeasingContract").Column("FINANCE_LEASING");
            Property(x => x.FinanceEnergServContract, "FinanceEnergServContract").Column("FINANCE_ENERGY");
            Property(x => x.OccupantContribution, "OccupantContribution").Column("OCCUPANT_CONTRIB");
            Property(x => x.OtherSource, "OtherSource").Column("OTHER_SOURCE");
            Property(x => x.ReceivedPretensionCount, "ReceivedPretensionCount").Column("RECEIVE_PRETENSION_CNT");
            Property(x => x.ApprovedPretensionCount, "ApprovedPretensionCount").Column("APROVE_PRETENSION_CNT");
            Property(x => x.NoApprovedPretensionCount, "NoApprovedPretensionCount").Column("NO_APROVE_PRETENSION_CNT");
            Property(x => x.PretensionRecalcSum, "PretensionRecalcSum").Column("PRETENSION_RECALC_SUM");
            Property(x => x.SentPretensionCount, "SentPretensionCount").Column("SENT_PRETENSION_CNT");
            Property(x => x.SentPetitionCount, "SentPetitionCount").Column("SENT_PETITION_CNT");
            Property(x => x.ReceiveSumByClaimWork, "ReceiveSumByClaimWork").Column("RECEIVE_SUM_BY_CLW");
            Property(x => x.AdvancePayments, "AdvancePayments").Column("ADVANCE_PAYMENTS");
            Property(x => x.CarryOverFunds, "CarryOverFunds").Column("CARRYOVER_FUNDS");
            Property(x => x.Debt, "Debt").Column("DEBT");
            Property(x => x.ChargeForMaintenanceAndRepairsMaintanance, "ChargeForMaintenanceAndRepairsMaintanance").Column("CFMAR_MAINTANCE");
            Property(x => x.ChargeForMaintenanceAndRepairsRepairs, "ChargeForMaintenanceAndRepairsRepairs").Column("CFMAR_REPAIRS");
            Property(x => x.ChargeForMaintenanceAndRepairsManagement, "ChargeForMaintenanceAndRepairsManagement").Column("CFMAR_MANAGMENT");
            Property(x => x.ChargeForMaintenanceAndRepairsAll, "ChargeForMaintenanceAndRepairsAll").Column("CFMAR_ALL");
            Property(x => x.ReceivedCashFromOwners, "ReceivedCashFromOwners").Column("RC_FOWNERS");
            Property(x => x.ReceivedCashFromOwnersTargeted, "ReceivedCashFromOwnersTargeted").Column("RC_FOTARGETED");
            Property(x => x.ReceivedCashAsGrant, "ReceivedCashAsGrant").Column("RC_GRANT");
            Property(x => x.ReceivedCashFromUsingCommonProperty, "ReceivedCashFromUsingCommonProperty").Column("RC_FCOMMONPROP");
            Property(x => x.ReceivedCashFromOtherTypeOfPayments, "ReceivedCashFromOtherTypeOfPayments").Column("RC_FOTHER");
            Property(x => x.CashBalanceAdvancePayments, "CashBalanceAdvancePayments").Column("CB_ADVANCE");
            Property(x => x.CashBalanceCarryOverFunds, "CashBalanceCarryOverFunds").Column("CB_CARRYOVER");
            Property(x => x.CashBalanceDebt, "CashBalanceDebt").Column("CB_DEBT");
            Property(x => x.CashBalanceAll, "CashBalanceAll").Column("ALL_CASH_BALANCE");
            Property(x => x.ReceivedCashAll, "ReceivedCashAll").Column("ALL_RECEIVED_CASH");

            Property(x => x.ComServStartAdvancePay, "ComServStartAdvancePay").Column("COM_SRV_START_ADVANCE_PAY");
            Property(x => x.ComServStartCarryOverFunds, "ComServStartCarryOverFunds").Column("COM_SRV_START_CARRYOV_FND");
            Property(x => x.ComServStartDebt, "ComServStartDebt").Column("COM_SRV_START_DEBT");
            Property(x => x.ComServEndAdvancePay, "ComServEndAdvancePay").Column("COM_SRV_END_ADVANCE_PAY");
            Property(x => x.ComServEndCarryOverFunds, "ComServEndCarryOverFunds").Column("COM_SRV_END_CARRYOV_FND");
            Property(x => x.ComServEndDebt, "ComServEndDebt").Column("COM_SRV_END_DEBT");
            Property(x => x.ComServReceivedPretensionCount, "ComServReceivedPretensionCount").Column("COM_SRV_RCV_PRETEN_CNT");
            Property(x => x.ComServApprovedPretensionCount, "ComServApprovedPretensionCount").Column("COM_SRV_APROVE_PRETEN_CNT");
            Property(x => x.ComServNoApprovedPretensionCount, "ComServNoApprovedPretensionCount").Column("COM_SRV_NO_APR_PRETEN_CNT");
            Property(x => x.ComServPretensionRecalcSum, "ComServPretensionRecalcSum").Column("COM_SRV_PRETEN_RECALC_SUM");

            Reference(x => x.FileExecutionWork, "FileExecutionWork").Column("FILE_EXECUTION_WORK").Fetch();
            Reference(x => x.FileExecutionObligation, "FileExecutionObligation").Column("FILE_ECXECUTION_OBLIG").Fetch();
            Reference(x => x.FileServiceCatalogRepair, "FileServiceCatalogRepair").Column("FILE_SERV_CAT_REPORT").Fetch();
            Reference(x => x.FileTariffCatalogRepair, "FileTariffCatalogRepair").Column("FILE_TARIF_CAT_REPORT").Fetch();
            Reference(x => x.PeriodDi, "PeriodDi").Column("PERIOD_DI_ID").Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJ_ID").NotNull().Fetch();
        }
    }
}
