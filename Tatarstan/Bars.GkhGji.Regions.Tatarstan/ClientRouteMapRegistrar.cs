namespace Bars.GkhGji.Regions.Tatarstan
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("gischarge", "B4.controller.GisCharge", requiredPermission: "GkhGji.GisCharge.View"));
            map.AddRoute(new ClientRoute("gisgmpparams", "B4.controller.GisGmpParams", requiredPermission: "GkhGji.GisCharge.ParamsView"));
        }
    }
}