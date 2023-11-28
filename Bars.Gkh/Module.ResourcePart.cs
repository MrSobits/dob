namespace Bars.Gkh
{
    using Bars.B4.ResourceBundling;

    public partial class Module
    {
        /// <summary>
        /// Метод регистрации бандлов
        /// </summary>
        private void RegisterBundlers()
        {
            var bundler = this.Container.Resolve<IResourceBundler>();
            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/content/css/b4GkhMain.css"
                });

            bundler.RegisterScriptsBundle("external-libs", new[]
                {
                    "~/libs/B4/AjaxRequestOverrides.js",
                    "~/libs/B4/ToolTip.js",
                    "~/libs/Yandex/YandexMap.js",
                    "~/libs/SignalR/jquery.signalR-1.2.2.min.js",
                    "~/libs/SignalR/Hubs.js",
                    "~/libs/B4/AjaxRequestOverrides.js",
                    "~/libs/B4/CountCacheQueue.js",
                    "~/libs/B4/GkhConfig.js",
                    "~/libs/B4/DateFix.js",
                    "~/libs/B4/WindowOverride.js",
                    "~/libs/B4/TabPanelOverride.js",
                    "~/libs/Highcharts/highcharts.js",
                    "~/libs/Highcharts/exporting.js",
                    "~/libs/Highcharts/offline-exporting.js",
                    "~/libs/Highcharts/plugins.js"
                });
        }
    }
}