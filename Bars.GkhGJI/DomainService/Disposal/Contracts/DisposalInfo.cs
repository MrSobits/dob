namespace Bars.GkhGji.DomainService.Contracts
{
    public class DisposalInfo
    {
        public string InspectorNames { get; protected set; }

        public string InspectorIds { get; protected set; }

        public string BaseName { get; protected set; }

        public string PlanName { get; protected set; }

        public bool ToAttracted { get; protected set; }

        public DisposalInfo(string inspectorNames, string inspectorIds, string baseName, string planName, bool toattracted)
        {
            InspectorIds = inspectorIds;
            InspectorNames = inspectorNames;
            BaseName = baseName;
            PlanName = planName;
            ToAttracted = toattracted;
        }
    }
}
