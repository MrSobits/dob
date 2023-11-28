namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/claimwork/EditIndividualClaimWork.js");
            AddResource(container, "libs/B4/controller/claimwork/IndividualClaimWork.js");
            AddResource(container, "libs/B4/controller/claimwork/Lawsuit.js");
            AddResource(container, "libs/B4/controller/claimwork/PartialClaimWork.js");
            AddResource(container, "libs/B4/controller/regop/personal_account/BasePersonalAccount.js");
            AddResource(container, "libs/B4/controller/regop/personal_account/Debtor.js");
            AddResource(container, "libs/B4/model/LawsuitReferenceCalculation.js");
            AddResource(container, "libs/B4/model/claimwork/LawSuitDebtWorkSSP.js");
            AddResource(container, "libs/B4/model/claimwork/LawSuitDebtWorkSSPDocument.js");
            AddResource(container, "libs/B4/model/claimwork/LawsuitOwnerRepresentative.js");
            AddResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitAddInfo.js");
            AddResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitOwnerInfoAddInfo.js");
            AddResource(container, "libs/B4/model/regop/personal_account/Debtor.js");
            AddResource(container, "libs/B4/store/LawsuitReferenceCalculation.js");
            AddResource(container, "libs/B4/store/claimwork/LawSuitDebtWorkSSP.js");
            AddResource(container, "libs/B4/store/claimwork/LawSuitDebtWorkSSPDocument.js");
            AddResource(container, "libs/B4/store/claimwork/LawsuitOwnerInfoByDocId.js");
            AddResource(container, "libs/B4/store/claimwork/LawsuitOwnerRepresentative.js");
            AddResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitAddInfo.js");
            AddResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitOwnerInfoAddInfo.js");
            AddResource(container, "libs/B4/view/claimwork/IndividualGrid.js");
            AddResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPAddWindow.js");
            AddResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPDocumentEditWindow.js");
            AddResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPDocumentGrid.js");
            AddResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPEditWindow.js");
            AddResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPGrid.js");
            AddResource(container, "libs/B4/view/claimwork/LawsuitOwnerRepresentativeGrid.js");
            AddResource(container, "libs/B4/view/claimwork/LawsuitOwnerRepresentativeWindow.js");
            AddResource(container, "libs/B4/view/claimwork/lawsuit/CourtClaimInfoPanel.js");
            AddResource(container, "libs/B4/view/claimwork/lawsuit/EditPanel.js");
            AddResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitOwnerInfoGrid.js");
            AddResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitOwnerInfoWindow.js");
            AddResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitReferenceCalculationGrid.js");
            AddResource(container, "libs/B4/view/claimwork/lawsuit/MainInfoPanel.js");
            AddResource(container, "libs/B4/view/regop/personal_account/DebtorGrid.js");
            AddResource(container, "libs/B4/view/regop/personal_account/PersonalAccountGrid.js");
        }
		        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.RegOperator.Regions.Chelyabinsk.dll/Bars.Gkh.RegOperator.Regions.Chelyabinsk.{0}", path.Replace("/", ".")));
        }
    }
}
