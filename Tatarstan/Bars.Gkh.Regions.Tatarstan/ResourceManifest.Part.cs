namespace Bars.Gkh.Regions.Tatarstan
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<ConstructionObjectDocumentType>();
            container.RegisterExtJsEnum<ConstructionObjectContractType>();
            container.RegisterExtJsEnum<ConstructionObjectParticipantType>();
            container.RegisterExtJsEnum<ConstructionObjectCustomerType>();
            container.RegisterExtJsEnum<ConstructionObjectPhotoGroup>();
            container.RegisterExtJsEnum<TerminationReasonType>();
            container.RegisterExtJsEnum<ExecutoryProcessDocumentType>();
            container.RegisterExtJsEnum<NormConsumptionType>();
            container.RegisterExtJsEnum<TypeHotWaterSystem>();
            container.RegisterExtJsEnum<TypeRisers>();
        }
    }
}