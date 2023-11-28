﻿namespace Bars.Gkh.Overhaul.Tat
{     
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/aspects/permission/realityobj/OwnerProtocol.js");
            AddResource(container, "libs/B4/controller/DecisionNoticeRegister.js");
            AddResource(container, "libs/B4/controller/LongTermPrObject.js");
            AddResource(container, "libs/B4/controller/PriorityParam.js");
            AddResource(container, "libs/B4/controller/RealEstateType.js");
            AddResource(container, "libs/B4/controller/RealEstateTypeRate.js");
            AddResource(container, "libs/B4/controller/ShortProgram.js");
            AddResource(container, "libs/B4/controller/Subsidy.js");
            AddResource(container, "libs/B4/controller/administration/MassDeleteRoSe.js");
            AddResource(container, "libs/B4/controller/dict/AccountOperation.js");
            AddResource(container, "libs/B4/controller/dict/YearCorrection.js");
            AddResource(container, "libs/B4/controller/import/RealityObjectImport.js");
            AddResource(container, "libs/B4/controller/longtermprobject/AccrualsAccount.js");
            AddResource(container, "libs/B4/controller/longtermprobject/Edit.js");
            AddResource(container, "libs/B4/controller/longtermprobject/Navigation.js");
            AddResource(container, "libs/B4/controller/longtermprobject/PaymentAccount.js");
            AddResource(container, "libs/B4/controller/longtermprobject/PropertyOwnerDecision.js");
            AddResource(container, "libs/B4/controller/longtermprobject/PropertyOwnerProtocols.js");
            AddResource(container, "libs/B4/controller/longtermprobject/RealAccount.js");
            AddResource(container, "libs/B4/controller/longtermprobject/SpecialAccount.js");
            AddResource(container, "libs/B4/controller/program/CorrectionResult.js");
            AddResource(container, "libs/B4/controller/program/Dpkr.js");
            AddResource(container, "libs/B4/controller/program/FirstStage.js");
            AddResource(container, "libs/B4/controller/program/Publication.js");
            AddResource(container, "libs/B4/controller/program/SecondStage.js");
            AddResource(container, "libs/B4/controller/program/ThirdStage.js");
            AddResource(container, "libs/B4/controller/realityobj/OwnerProtocol.js");
            AddResource(container, "libs/B4/controller/realityobj/StructElement.js");
            AddResource(container, "libs/B4/controller/report/CertificationControlValues.js");
            AddResource(container, "libs/B4/controller/report/ConsolidatedCertificationReport.js");
            AddResource(container, "libs/B4/controller/report/ControlCertificationOfBuild.js");
            AddResource(container, "libs/B4/controller/report/CountRoByMuInPeriod.js");
            AddResource(container, "libs/B4/controller/report/CtrlCertOfBuildConsiderMissingCeo.js");
            AddResource(container, "libs/B4/controller/report/FillingControlRepairReport.js");
            AddResource(container, "libs/B4/controller/report/FormFundNotSetMkdInfo.js");
            AddResource(container, "libs/B4/controller/report/GisuRealObjContract.js");
            AddResource(container, "libs/B4/controller/report/LongProgramByTypeWork.js");
            AddResource(container, "libs/B4/controller/report/LongProgramReport.js");
            AddResource(container, "libs/B4/controller/report/PublishedDpkr.js");
            AddResource(container, "libs/B4/controller/report/RoomAreaControl.js");
            AddResource(container, "libs/B4/controller/report/ShortProgramByTypeWork.js");
            AddResource(container, "libs/B4/controller/report/SpecialAccountDecision.js");
            AddResource(container, "libs/B4/controller/version/ProgramVersion.js");
            AddResource(container, "libs/B4/model/BasePropertyOwnerDecision.js");
            AddResource(container, "libs/B4/model/CreditOrganizationDecision.js");
            AddResource(container, "libs/B4/model/CurrentPrioirityParams.js");
            AddResource(container, "libs/B4/model/FormulaParam.js");
            AddResource(container, "libs/B4/model/ListServicesDecision.js");
            AddResource(container, "libs/B4/model/LongTermPrObject.js");
            AddResource(container, "libs/B4/model/MinAmountDecision.js");
            AddResource(container, "libs/B4/model/MinFundSizeDecision.js");
            AddResource(container, "libs/B4/model/OwnerAccountDecision.js");
            AddResource(container, "libs/B4/model/PrevAccumulatedAmountDecision.js");
            AddResource(container, "libs/B4/model/PropertyOwnerDecisionWork.js");
            AddResource(container, "libs/B4/model/PropertyOwnerProtocols.js");
            AddResource(container, "libs/B4/model/RealEstateTypeCommonParam.js");
            AddResource(container, "libs/B4/model/RealEstateTypePriorityParam.js");
            AddResource(container, "libs/B4/model/RealEstateTypeRate.js");
            AddResource(container, "libs/B4/model/RealEstateTypeStructElement.js");
            AddResource(container, "libs/B4/model/RegOpAccountDecision.js");
            AddResource(container, "libs/B4/model/ShortProgramRecord.js");
            AddResource(container, "libs/B4/model/SpecialAccountDecision.js");
            AddResource(container, "libs/B4/model/SpecialAccountDecisionNotice.js");
            AddResource(container, "libs/B4/model/account/Accruals.js");
            AddResource(container, "libs/B4/model/account/BankStatement.js");
            AddResource(container, "libs/B4/model/account/Operation.js");
            AddResource(container, "libs/B4/model/account/Payment.js");
            AddResource(container, "libs/B4/model/account/Real.js");
            AddResource(container, "libs/B4/model/account/Special.js");
            AddResource(container, "libs/B4/model/account/operation/Accruals.js");
            AddResource(container, "libs/B4/model/account/operation/Real.js");
            AddResource(container, "libs/B4/model/account/operation/Special.js");
            AddResource(container, "libs/B4/model/dict/AccountOperation.js");
            AddResource(container, "libs/B4/model/dict/HouseType.js");
            AddResource(container, "libs/B4/model/dict/TypeOwnership.js");
            AddResource(container, "libs/B4/model/dict/YearCorrection.js");
            AddResource(container, "libs/B4/model/longtermprobject/ListServicesWorksModel.js");
            AddResource(container, "libs/B4/model/priorityparam/Multi.js");
            AddResource(container, "libs/B4/model/priorityparam/Quality.js");
            AddResource(container, "libs/B4/model/priorityparam/Quant.js");
            AddResource(container, "libs/B4/model/program/CorrectionResult.js");
            AddResource(container, "libs/B4/model/program/DpkrGroupedYear.js");
            AddResource(container, "libs/B4/model/program/FirstStage.js");
            AddResource(container, "libs/B4/model/program/PriorityParam.js");
            AddResource(container, "libs/B4/model/program/PublishedProgram.js");
            AddResource(container, "libs/B4/model/program/PublishedProgramRecord.js");
            AddResource(container, "libs/B4/model/program/SecondStage.js");
            AddResource(container, "libs/B4/model/program/ThirdStage.js");
            AddResource(container, "libs/B4/model/program/thirddetails/CommonEstate.js");
            AddResource(container, "libs/B4/model/program/thirddetails/WorkType.js");
            AddResource(container, "libs/B4/model/shortprogram/DefectList.js");
            AddResource(container, "libs/B4/model/shortprogram/Protocol.js");
            AddResource(container, "libs/B4/model/shortprogram/RealityObject.js");
            AddResource(container, "libs/B4/model/shortprogram/Record.js");
            AddResource(container, "libs/B4/model/shortprogram/Work.js");
            AddResource(container, "libs/B4/model/subsidy/SubsidyMunicipality.js");
            AddResource(container, "libs/B4/model/subsidy/SubsidyMunicipalityRecord.js");
            AddResource(container, "libs/B4/model/subsidy/SubsidyRecord.js");
            AddResource(container, "libs/B4/model/version/Params.js");
            AddResource(container, "libs/B4/model/version/ProgramVersion.js");
            AddResource(container, "libs/B4/model/version/VersionActualizeLog.js");
            AddResource(container, "libs/B4/model/version/VersionRecord.js");
            AddResource(container, "libs/B4/plugin/TreeFilter.js");
            AddResource(container, "libs/B4/store/BasePropertyOwnerDecision.js");
            AddResource(container, "libs/B4/store/CurrentPrioirityParams.js");
            AddResource(container, "libs/B4/store/DecisionNoticeRegister.js");
            AddResource(container, "libs/B4/store/FormulaParam.js");
            AddResource(container, "libs/B4/store/LongTermPrObject.js");
            AddResource(container, "libs/B4/store/MinAmountDecision.js");
            AddResource(container, "libs/B4/store/OwnerAccountContragent.js");
            AddResource(container, "libs/B4/store/PropertyOwnerDecisionWork.js");
            AddResource(container, "libs/B4/store/PropertyOwnerProtocols.js");
            AddResource(container, "libs/B4/store/PublishedProgramMunicipality.js");
            AddResource(container, "libs/B4/store/RealEstateTypeCommonParam.js");
            AddResource(container, "libs/B4/store/RealEstateTypePriorityParam.js");
            AddResource(container, "libs/B4/store/RealEstateTypeRate.js");
            AddResource(container, "libs/B4/store/RealEstateTypeStructElement.js");
            AddResource(container, "libs/B4/store/RegOpAccountDecision.js");
            AddResource(container, "libs/B4/store/ShortProgramRecord.js");
            AddResource(container, "libs/B4/store/ShortProgramYear.js");
            AddResource(container, "libs/B4/store/SpecialAccountDecision.js");
            AddResource(container, "libs/B4/store/SpecialAccountDecisionNotice.js");
            AddResource(container, "libs/B4/store/account/Accruals.js");
            AddResource(container, "libs/B4/store/account/BankStatement.js");
            AddResource(container, "libs/B4/store/account/ContragentForSpecial.js");
            AddResource(container, "libs/B4/store/account/Operation.js");
            AddResource(container, "libs/B4/store/account/Payment.js");
            AddResource(container, "libs/B4/store/account/Real.js");
            AddResource(container, "libs/B4/store/account/Special.js");
            AddResource(container, "libs/B4/store/account/operation/Accruals.js");
            AddResource(container, "libs/B4/store/account/operation/Real.js");
            AddResource(container, "libs/B4/store/account/operation/Special.js");
            AddResource(container, "libs/B4/store/administration/massdelete/RealityObject.js");
            AddResource(container, "libs/B4/store/administration/massdelete/RealityObjectStructuralElement.js");
            AddResource(container, "libs/B4/store/dict/AccountOperation.js");
            AddResource(container, "libs/B4/store/dict/AccountOperationNoPaging.js");
            AddResource(container, "libs/B4/store/dict/ConditionHouseForSelect.js");
            AddResource(container, "libs/B4/store/dict/ConditionHouseForSelected.js");
            AddResource(container, "libs/B4/store/dict/MunicipalityForSelect.js");
            AddResource(container, "libs/B4/store/dict/MunicipalityForSelected.js");
            AddResource(container, "libs/B4/store/dict/TypeOwnershipForSelect.js");
            AddResource(container, "libs/B4/store/dict/TypeOwnershipForSelected.js");
            AddResource(container, "libs/B4/store/dict/YearCorrection.js");
            AddResource(container, "libs/B4/store/longtermprobject/ListServicesWorksStore.js");
            AddResource(container, "libs/B4/store/longtermprobject/NavigationMenu.js");
            AddResource(container, "libs/B4/store/priorityparam/Multi.js");
            AddResource(container, "libs/B4/store/priorityparam/Quality.js");
            AddResource(container, "libs/B4/store/priorityparam/Quant.js");
            AddResource(container, "libs/B4/store/priorityparam/multi/Select.js");
            AddResource(container, "libs/B4/store/priorityparam/multi/Selected.js");
            AddResource(container, "libs/B4/store/program/CorrectionHistory.js");
            AddResource(container, "libs/B4/store/program/CorrectionHistoryDetail.js");
            AddResource(container, "libs/B4/store/program/CorrectionResult.js");
            AddResource(container, "libs/B4/store/program/CorrectResultForMassChangeSelect.js");
            AddResource(container, "libs/B4/store/program/CorrectResultForMassChangeSelected.js");
            AddResource(container, "libs/B4/store/program/DpkrGroupedYear.js");
            AddResource(container, "libs/B4/store/program/FirstStage.js");
            AddResource(container, "libs/B4/store/program/PriorityParam.js");
            AddResource(container, "libs/B4/store/program/Publication.js");
            AddResource(container, "libs/B4/store/program/SecondStage.js");
            AddResource(container, "libs/B4/store/program/ThirdStage.js");
            AddResource(container, "libs/B4/store/program/thirddetails/CommonEstate.js");
            AddResource(container, "libs/B4/store/program/thirddetails/WorkType.js");
            AddResource(container, "libs/B4/store/realityobj/NavigationMenu.js");
            AddResource(container, "libs/B4/store/regoperator/Account.js");
            AddResource(container, "libs/B4/store/regoperator/Municipality.js");
            AddResource(container, "libs/B4/store/regoperator/Navigation.js");
            AddResource(container, "libs/B4/store/shortprogram/DefectList.js");
            AddResource(container, "libs/B4/store/shortprogram/DefectListWork.js");
            AddResource(container, "libs/B4/store/shortprogram/Protocol.js");
            AddResource(container, "libs/B4/store/shortprogram/RealityObject.js");
            AddResource(container, "libs/B4/store/shortprogram/RealObjSelect.js");
            AddResource(container, "libs/B4/store/shortprogram/RealObjSelected.js");
            AddResource(container, "libs/B4/store/shortprogram/Record.js");
            AddResource(container, "libs/B4/store/shortprogram/WorkSelect.js");
            AddResource(container, "libs/B4/store/shortprogram/WorkSelected.js");
            AddResource(container, "libs/B4/store/subsidy/SubsidyMunicipalityRecord.js");
            AddResource(container, "libs/B4/store/subsidy/SubsidyRecord.js");
            AddResource(container, "libs/B4/store/version/ActualizeDeletedEntries.js");
            AddResource(container, "libs/B4/store/version/Params.js");
            AddResource(container, "libs/B4/store/version/ProgramVersion.js");
            AddResource(container, "libs/B4/store/version/VersionActualizeLog.js");
            AddResource(container, "libs/B4/store/version/VersionRecord.js");
            AddResource(container, "libs/B4/ux/config/YearCorrection.js");
            AddResource(container, "libs/B4/view/RealEstateTypeRateGrid.js");
            AddResource(container, "libs/B4/view/administration/massdelete/Panel.js");
            AddResource(container, "libs/B4/view/administration/massdelete/SelectGrid.js");
            AddResource(container, "libs/B4/view/decisionnoticereg/Grid.js");
            AddResource(container, "libs/B4/view/dict/accountoperation/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/accountoperation/Grid.js");
            AddResource(container, "libs/B4/view/dict/yearcorrection/Grid.js");
            AddResource(container, "libs/B4/view/import/realityobj/Panel.js");
            AddResource(container, "libs/B4/view/longtermprobject/AddWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/EditPanel.js");
            AddResource(container, "libs/B4/view/longtermprobject/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/NavigationPanel.js");
            AddResource(container, "libs/B4/view/longtermprobject/accrualsaccount/EditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/accrualsaccount/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/accrualsaccount/OperationEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/accrualsaccount/OperationGrid.js");
            AddResource(container, "libs/B4/view/longtermprobject/paymentaccount/BankStatEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/paymentaccount/BankStatGrid.js");
            AddResource(container, "libs/B4/view/longtermprobject/paymentaccount/EditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/paymentaccount/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/paymentaccount/OperationEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/paymentaccount/OperationGrid.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/AddWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/CreditOrgEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/ListServicesEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/MinAmountEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/MinFundSizeEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/OwnerAccountEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/PreAmountEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/RegOpEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/SpecAccEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/SpecAccNoticePanel.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/work/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerdecision/work/ListServicesWorksGrid.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerprotocols/EditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/propertyownerprotocols/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/realaccount/EditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/realaccount/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/realaccount/OperationEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/realaccount/OperationGrid.js");
            AddResource(container, "libs/B4/view/longtermprobject/specialaccount/EditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/specialaccount/Grid.js");
            AddResource(container, "libs/B4/view/longtermprobject/specialaccount/OperationEditWindow.js");
            AddResource(container, "libs/B4/view/longtermprobject/specialaccount/OperationGrid.js");
            AddResource(container, "libs/B4/view/priorityparam/Grid.js");
            AddResource(container, "libs/B4/view/priorityparam/Panel.js");
            AddResource(container, "libs/B4/view/priorityparam/QualityEditWindow.js");
            AddResource(container, "libs/B4/view/priorityparam/QualityGrid.js");
            AddResource(container, "libs/B4/view/priorityparam/QuantEditWindow.js");
            AddResource(container, "libs/B4/view/priorityparam/QuantGrid.js");
            AddResource(container, "libs/B4/view/priorityparam/multi/EditWindow.js");
            AddResource(container, "libs/B4/view/priorityparam/multi/Grid.js");
            AddResource(container, "libs/B4/view/priorityparam/multi/SelectedGrid.js");
            AddResource(container, "libs/B4/view/priorityparam/multi/SelectGrid.js");
            AddResource(container, "libs/B4/view/program/CopyPricesWindow.js");
            AddResource(container, "libs/B4/view/program/CorrectionActualizeYearsWindow.js");
            AddResource(container, "libs/B4/view/program/CorrectionHistoryDetailGrid.js");
            AddResource(container, "libs/B4/view/program/CorrectionHistoryDetailWindow.js");
            AddResource(container, "libs/B4/view/program/CorrectionHistoryGrid.js");
            AddResource(container, "libs/B4/view/program/CorrectionResultDetailsGrid.js");
            AddResource(container, "libs/B4/view/program/CorrectionResultGrid.js");
            AddResource(container, "libs/B4/view/program/CorrectionResultPanel.js");
            AddResource(container, "libs/B4/view/program/CurrentPriorityGrid.js");
            AddResource(container, "libs/B4/view/program/DpkrGroupedYearGrid.js");
            AddResource(container, "libs/B4/view/program/EditCorrectionResultWindow.js");
            AddResource(container, "libs/B4/view/program/EditOrderWindow.js");
            AddResource(container, "libs/B4/view/program/FirstStageGrid.js");
            AddResource(container, "libs/B4/view/program/MassCorrectYearChangeWindow.js");
            AddResource(container, "libs/B4/view/program/NewVersionWindow.js");
            AddResource(container, "libs/B4/view/program/PublicationGrid.js");
            AddResource(container, "libs/B4/view/program/SecondStageGrid.js");
            AddResource(container, "libs/B4/view/program/ThirdStageGrid.js");
            AddResource(container, "libs/B4/view/program/ThirdStagePanel.js");
            AddResource(container, "libs/B4/view/program/thirddetails/CommonEstateGrid.js");
            AddResource(container, "libs/B4/view/program/thirddetails/ThirdStageDetails.js");
            AddResource(container, "libs/B4/view/program/thirddetails/WorkTypeGrid.js");
            AddResource(container, "libs/B4/view/realestatetype/CommonParamGrid.js");
            AddResource(container, "libs/B4/view/realestatetype/Edit.js");
            AddResource(container, "libs/B4/view/realestatetype/Grid.js");
            AddResource(container, "libs/B4/view/realestatetype/PriorityParamGrid.js");
            AddResource(container, "libs/B4/view/realestatetype/StructElemGrid.js");
            AddResource(container, "libs/B4/view/realestatetype/TreeMultiSelect.js");
            AddResource(container, "libs/B4/view/realityobj/protocol/DecisionGrid.js");
            AddResource(container, "libs/B4/view/realityobj/protocol/Grid.js");
            AddResource(container, "libs/B4/view/realityobj/protocol/Panel.js");
            AddResource(container, "libs/B4/view/realityobj/structelement/Panel.js");
            AddResource(container, "libs/B4/view/report/CertificationControlValuesPanel.js");
            AddResource(container, "libs/B4/view/report/ConsolidatedCertificationReportPanel.js");
            AddResource(container, "libs/B4/view/report/ControlCertificationOfBuildPanel.js");
            AddResource(container, "libs/B4/view/report/CountRoByMuInPeriodPanel.js");
            AddResource(container, "libs/B4/view/report/CtrlCertOfBuildMissingCeoPanel.js");
            AddResource(container, "libs/B4/view/report/FillingControlRepairReportPanel.js");
            AddResource(container, "libs/B4/view/report/FormFundNotSetMkdInfoPanel.js");
            AddResource(container, "libs/B4/view/report/GisuRealObjContractPanel.js");
            AddResource(container, "libs/B4/view/report/LongProgramByTypeWorkPanel.js");
            AddResource(container, "libs/B4/view/report/LongProgramReportPanel.js");
            AddResource(container, "libs/B4/view/report/PublishedDpkrPanel.js");
            AddResource(container, "libs/B4/view/report/RoomAreaControlPanel.js");
            AddResource(container, "libs/B4/view/report/ShortProgramByTypeWorkPanel.js");
            AddResource(container, "libs/B4/view/report/SpecialAccountDecisionPanel.js");
            AddResource(container, "libs/B4/view/shortprogram/DefectListEditWindow.js");
            AddResource(container, "libs/B4/view/shortprogram/DefectListGrid.js");
            AddResource(container, "libs/B4/view/shortprogram/Grid.js");
            AddResource(container, "libs/B4/view/shortprogram/MassStateChangeWindow.js");
            AddResource(container, "libs/B4/view/shortprogram/Panel.js");
            AddResource(container, "libs/B4/view/shortprogram/ProtocolEditWindow.js");
            AddResource(container, "libs/B4/view/shortprogram/ProtocolGrid.js");
            AddResource(container, "libs/B4/view/shortprogram/RealityObjectGrid.js");
            AddResource(container, "libs/B4/view/shortprogram/RecordEditWindow.js");
            AddResource(container, "libs/B4/view/shortprogram/RecordGrid.js");
            AddResource(container, "libs/B4/view/subsidy/SubsidyMuChart.js");
            AddResource(container, "libs/B4/view/subsidy/SubsidyMuPanel.js");
            AddResource(container, "libs/B4/view/subsidy/SubsidyMuRecordGrid.js");
            AddResource(container, "libs/B4/view/subsidy/SubsidyMuTabPanel.js");
            AddResource(container, "libs/B4/view/subsidy/SubsidyPanel.js");
            AddResource(container, "libs/B4/view/subsidy/SubsidyRecordGrid.js");
            AddResource(container, "libs/B4/view/version/ActualizeDelGrid.js");
            AddResource(container, "libs/B4/view/version/ActualizeDelWindow.js");
            AddResource(container, "libs/B4/view/version/ActualizeLogGrid.js");
            AddResource(container, "libs/B4/view/version/ActualizePeriodWindow.js");
            AddResource(container, "libs/B4/view/version/ActualizeProgramCrWindow.js");
            AddResource(container, "libs/B4/view/version/CopyWindow.js");
            AddResource(container, "libs/B4/view/version/Grid.js");
            AddResource(container, "libs/B4/view/version/OrderWindow.js");
            AddResource(container, "libs/B4/view/version/Panel.js");
            AddResource(container, "libs/B4/view/version/RecordsGrid.js");
            AddResource(container, "libs/B4/view/version/VersionParamsGrid.js");
            AddResource(container, "content/css/b4GkhNso.css");
            AddResource(container, "content/images/checked.gif");
            AddResource(container, "content/images/longProgram.png");
            AddResource(container, "content/images/longProgramRegistry.png");
            AddResource(container, "content/images/paramsSetup.png");
            AddResource(container, "content/images/programVersion.png");
            AddResource(container, "content/images/publishPrograms.png");
            AddResource(container, "content/images/shortProgram.png");
            AddResource(container, "content/images/subsidy.png");
            AddResource(container, "content/images/unchecked.gif");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Overhaul.Tat.dll/Bars.Gkh.Overhaul.Tat.{0}", path.Replace("/", ".")));
        }
    }
}
