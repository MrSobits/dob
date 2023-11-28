namespace Bars.Gkh.RegOperator
{
    using Bars.B4;

    public class NavigationProvider: INavigationProvider
    {
        public string Key
        {
            get { return MainNavigationInfo.MenuName; }
        }

        public string Description
        {
            get { return MainNavigationInfo.MenuDescription; }
        }

        public void Init(MenuItem root)
        {
 

          
        }
    }
}