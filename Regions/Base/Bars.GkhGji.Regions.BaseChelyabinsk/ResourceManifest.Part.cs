namespace Bars.GkhGji.Regions.BaseChelyabinsk
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/AmirsService.svc", "Bars.GkhGji.Regions.BaseChelyabinsk.dll/Bars.GkhGji.Regions.BaseChelyabinsk.Services.AmirsService.svc");

            container.RegisterExtJsEnum<TypeDocumentGji>();
            container.RegisterExtJsEnum<TypeStage>();
            container.RegisterExtJsEnum<TypeReminder>();
            container.RegisterExtJsEnum<FNSLicDecisionType>();
            container.RegisterExtJsEnum<TypeRepresentativePresence>();
            container.RegisterExtJsEnum<LicenseActionType>();
            container.RegisterExtJsEnum<KNMType>();
            container.RegisterExtJsEnum<TypeComplainsRequest>();
            container.RegisterExtJsEnum<KNMStatus>();
            container.RegisterExtJsEnum<RequesterRole>();
            container.RegisterExtJsEnum<IdentityDocumentType>();
            container.Add("libs/B4/enums/TypeCorrespondent.js", new ExtJsEnumResource<TypeCorrespondent>("B4.enums.TypeCorrespondent"));
            container.Add("libs/B4/enums/ControlType.js", new ExtJsEnumResource<GkhGji.Enums.ControlType>("B4.enums.ControlType"));
        }
    }
}
