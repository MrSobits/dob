namespace Bars.GkhGji.Regions.Tatarstan
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/permission/ActCheck.js");
            AddResource(container, "libs/B4/aspects/permission/ActRemoval.js");
            AddResource(container, "libs/B4/aspects/permission/BaseDispHead.js");
            AddResource(container, "libs/B4/aspects/permission/BaseJurPerson.js");
            AddResource(container, "libs/B4/aspects/permission/BaseProsClaim.js");
            AddResource(container, "libs/B4/aspects/permission/BaseStatement.js");
            AddResource(container, "libs/B4/aspects/permission/ProtocolGji.js");
            AddResource(container, "libs/B4/aspects/permission/TatDisposal.js");
            AddResource(container, "libs/B4/controller/ActCheck.js");
            AddResource(container, "libs/B4/controller/BaseStatement.js");
            AddResource(container, "libs/B4/controller/Disposal.js");
            AddResource(container, "libs/B4/controller/GisCharge.js");
            AddResource(container, "libs/B4/controller/GisGmpParams.js");
            AddResource(container, "libs/B4/controller/Prescription.js");
            AddResource(container, "libs/B4/controller/ProtocolGji.js");
            AddResource(container, "libs/B4/controller/Reminder.js");
            AddResource(container, "libs/B4/controller/Resolution.js");
            AddResource(container, "libs/B4/controller/basedisphead/Edit.js");
            AddResource(container, "libs/B4/controller/basejurperson/Edit.js");
            AddResource(container, "libs/B4/controller/baseprosclaim/Edit.js");
            AddResource(container, "libs/B4/controller/basestatement/Edit.js");
            AddResource(container, "libs/B4/controller/dict/InspectionBaseType.js");
            AddResource(container, "libs/B4/mixins/ActCheck.js");
            AddResource(container, "libs/B4/model/ActCheck.js");
            AddResource(container, "libs/B4/model/GisCharge.js");
            AddResource(container, "libs/B4/model/GisGmpPattern.js");
            AddResource(container, "libs/B4/model/ProtocolGji.js");
            AddResource(container, "libs/B4/model/actcheck/ProvidedDoc.js");
            AddResource(container, "libs/B4/model/basejurperson/Contragent.js");
            AddResource(container, "libs/B4/model/dict/InspectionBaseType.js");
            AddResource(container, "libs/B4/model/disposal/DisposalVerificationSubject.js");
            AddResource(container, "libs/B4/model/disposal/InspFoundationCheck.js");
            AddResource(container, "libs/B4/model/disposal/SurveyObjective.js");
            AddResource(container, "libs/B4/model/disposal/SurveyPurpose.js");
            AddResource(container, "libs/B4/store/ActCheck.js");
            AddResource(container, "libs/B4/store/GisCharge.js");
            AddResource(container, "libs/B4/store/GisGmpPattern.js");
            AddResource(container, "libs/B4/store/actcheck/ProvidedDoc.js");
            AddResource(container, "libs/B4/store/basejurperson/Contragent.js");
            AddResource(container, "libs/B4/store/dict/InspectionBaseType.js");
            AddResource(container, "libs/B4/store/disposal/DisposalVerificationSubject.js");
            AddResource(container, "libs/B4/store/disposal/InspFoundationCheck.js");
            AddResource(container, "libs/B4/store/disposal/SurveyObjective.js");
            AddResource(container, "libs/B4/store/disposal/SurveyPurpose.js");
            AddResource(container, "libs/B4/view/GisGmpParamsPanel.js");
            AddResource(container, "libs/B4/view/GisGmpPatternEditWindow.js");
            AddResource(container, "libs/B4/view/GisGmpPatternGrid.js");
            AddResource(container, "libs/B4/view/actcheck/EditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/ProvidedDocGrid.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/actremoval/EditPanel.js");
            AddResource(container, "libs/B4/view/actremoval/ViolationGrid.js");
            AddResource(container, "libs/B4/view/basedisphead/EditPanel.js");
            AddResource(container, "libs/B4/view/basejurperson/ContragentGrid.js");
            AddResource(container, "libs/B4/view/basejurperson/EditPanel.js");
            AddResource(container, "libs/B4/view/basejurperson/MainInfoTabPanel.js");
            AddResource(container, "libs/B4/view/baseprosclaim/EditPanel.js");
            AddResource(container, "libs/B4/view/basestatement/AddWindow.js");
            AddResource(container, "libs/B4/view/basestatement/EditPanel.js");
            AddResource(container, "libs/B4/view/desktop/portlet/TaskState.js");
            AddResource(container, "libs/B4/view/dict/inspectionbasetype/Grid.js");
            AddResource(container, "libs/B4/view/disposal/EditPanel.js");
            AddResource(container, "libs/B4/view/disposal/InspFoundationCheckGrid.js");
            AddResource(container, "libs/B4/view/disposal/SubjectVerificationGrid.js");
            AddResource(container, "libs/B4/view/disposal/SurveyObjectiveGrid.js");
            AddResource(container, "libs/B4/view/disposal/SurveyPurposeGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ResolutionGrid.js");
            AddResource(container, "libs/B4/view/gischarge/Grid.js");
            AddResource(container, "libs/B4/view/gischarge/JsonWindow.js");
            AddResource(container, "libs/B4/view/inspectiongji/ContragentGrid.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditGeneralInfoTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditViolCancelTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditWindow.js");
            AddResource(container, "libs/B4/view/prescription/CancelGrid.js");
            AddResource(container, "libs/B4/view/prescription/EditPanel.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/EditPanel.js");
            AddResource(container, "libs/B4/view/protocolgji/RequisitePanel.js");
            AddResource(container, "libs/B4/view/protocolmvd/EditPanel.js");
            AddResource(container, "libs/B4/view/reminder/FilterPanel.js");
            AddResource(container, "libs/B4/view/resolution/RequisitePanel.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhGji.Regions.Tatarstan.dll/Bars.GkhGji.Regions.Tatarstan.{0}", path.Replace("/", ".")));
        }
    }
}
