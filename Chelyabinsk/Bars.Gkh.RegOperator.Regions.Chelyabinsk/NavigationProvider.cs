namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
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
            //root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Долевые ПИР", "partialclaimwork").AddRequiredPermission("Clw.FlattenedClaimWork.View");
        }
    }
}