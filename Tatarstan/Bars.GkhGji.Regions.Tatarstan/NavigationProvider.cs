namespace Bars.GkhGji.Regions.Tatarstan
{
    using B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            var integration = root
                .Add("Жилищная инспекция")
                .Add("Интеграция с ГИС ГМП");

            integration
                .Add("Отправка начисленных штрафов", "gischarge")
                .AddRequiredPermission("GkhGji.GisCharge.View");

            integration
                .Add("Настройка параметров", "gisgmpparams")
                .AddRequiredPermission("GkhGji.GisCharge.ParamsView");
        }

        public string Key
        {
            get { return MainNavigationInfo.MenuName; }
        }

        public string Description
        {
            get { return MainNavigationInfo.MenuDescription; }
        }
    }
}