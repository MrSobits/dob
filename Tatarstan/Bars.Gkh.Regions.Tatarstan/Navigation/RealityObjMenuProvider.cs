namespace Bars.Gkh.Regions.Tatarstan.Navigation
{
    using Bars.B4;
    
    public class RealityObjMenuKey
    {
        public static string Key
        {
            get { return "RealityObj"; }
        }

        public static string Description
        {
            get
            {
                return "Меню карточки жилого дома";
            }
        }
    } 

    public class RealityObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return RealityObjMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return RealityObjMenuKey.Description;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Сведения о квартирах", "realityobjectedit/{0}/apartinfo").AddRequiredPermission("Gkh.RealityObject.Register.ApartInfo.View").WithIcon("icon-key");

            root.Add("Сведения по ЖКУ").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.View");
            root.Add("Сведения по ЖКУ").Add("Общие сведения по дому", "realityobjectedit/{0}/infoOverview").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.InfoOverview.View");
            root.Add("Сведения по ЖКУ").Add("Лицевые счета дома", "realityobjectedit/{0}/HouseAccount").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.Account.View");
            root.Add("Сведения по ЖКУ").Add("Показания общедомовых приборов учета", "realityobjectedit/{0}/meteringdevicevalue").AddRequiredPermission("Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.View");
        }
    }
}
