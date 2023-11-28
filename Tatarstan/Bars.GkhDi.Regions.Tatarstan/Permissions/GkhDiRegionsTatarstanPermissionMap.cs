namespace Bars.GkhDi.Regions.Tatarstan.Permissions
{
    using B4;
    
    public class GkhDiRegionsTatarstanPermissionMap : PermissionMap
    {
        public GkhDiRegionsTatarstanPermissionMap()
        {
            Namespace("GkhDi.Dict.MeasuresReduceCosts", "Меры по снижению расходов");
            CRUDandViewPermissions("GkhDi.Dict.MeasuresReduceCosts");
        }
    }
}