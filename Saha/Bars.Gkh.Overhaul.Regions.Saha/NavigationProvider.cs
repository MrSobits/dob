namespace Bars.Gkh.Overhaul.Regions.Saha
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
        }

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
    }
}