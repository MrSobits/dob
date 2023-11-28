namespace Bars.GkhDi.Regions.Tatarstan
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/DocumentsRealityObj.js");
            AddResource(container, "libs/B4/controller/InfoAboutUseCommonFacilities.js");
            AddResource(container, "libs/B4/controller/LoadWorkController.js");
            AddResource(container, "libs/B4/controller/PlanReductionExpense.js");
            AddResource(container, "libs/B4/controller/PlanWorkServiceRepair.js");
            AddResource(container, "libs/B4/controller/Service.js");
            AddResource(container, "libs/B4/controller/dict/MeasuresReduceCosts.js");
            AddResource(container, "libs/B4/model/PlanWorkServiceRepair.js");
            AddResource(container, "libs/B4/model/dict/MeasuresReduceCosts.js");
            AddResource(container, "libs/B4/model/service/LoadWorkPprRepair.js");
            AddResource(container, "libs/B4/model/service/ProviderService.js");
            AddResource(container, "libs/B4/model/service/Repair.js");
            AddResource(container, "libs/B4/model/service/WorkRepairDetail.js");
            AddResource(container, "libs/B4/model/service/WorkRepairList.js");
            AddResource(container, "libs/B4/store/MeasuresReduceCostsSelect.js");
            AddResource(container, "libs/B4/store/MeasuresReduceCostsSelected.js");
            AddResource(container, "libs/B4/store/dict/MeasuresReduceCosts.js");
            AddResource(container, "libs/B4/store/service/WorkRepairTechService.js");
            AddResource(container, "libs/B4/view/dict/measuresreducecosts/Grid.js");
            AddResource(container, "libs/B4/view/infoaboutusecommonfacilities/EditWindow.js");
            AddResource(container, "libs/B4/view/planreductionexpense/WorksGrid.js");
            AddResource(container, "libs/B4/view/planworkservicerepair/EditWindow.js");
            AddResource(container, "libs/B4/view/planworkservicerepair/RepairServicesGrid.js");
            AddResource(container, "libs/B4/view/planworkservicerepair/WorksEditWindow.js");
            AddResource(container, "libs/B4/view/planworkservicerepair/WorksGrid.js");
            AddResource(container, "libs/B4/view/realityobjectpassport/ViewPanel.js");
            AddResource(container, "libs/B4/view/service/Grid.js");
            AddResource(container, "libs/B4/view/service/LoadWorkEditPanel.js");
            AddResource(container, "libs/B4/view/service/repair/EditWindow.js");
            AddResource(container, "libs/B4/view/service/repair/ProviderServiceEditWindow.js");
            AddResource(container, "libs/B4/view/service/repair/ProviderServiceGrid.js");
            AddResource(container, "libs/B4/view/service/repair/WorkRepairDetailGrid.js");
            AddResource(container, "libs/B4/view/service/repair/WorkRepairListGrid.js");
            AddResource(container, "libs/B4/view/service/repair/WorkRepairTechServGrid.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhDi.Regions.Tatarstan.dll/Bars.GkhDi.Regions.Tatarstan.{0}", path.Replace("/", ".")));
        }
    }
}
