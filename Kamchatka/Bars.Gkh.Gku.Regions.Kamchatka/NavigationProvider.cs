namespace Bars.Gkh.Gku.Regions.Kamchatka
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Реестр лицевых счетов", "gkuinfo").AddRequiredPermission("Gkh.GkuInfo.View");

            root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Модуль начисления", "billing").WithIcon("billing");

            root.Add("Справочники").Add("Жилищно-коммунальное хозяйство").Add("Тарифы ЖКУ", "gkutarif").AddRequiredPermission("GkhGji.Dict.GkuTariff.View");
        }
    }
}