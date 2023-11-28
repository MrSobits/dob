namespace Bars.Gkh.Domain
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.Extensions;

    using Castle.Windsor;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Сервис для получения текущей версии сборки
    /// </summary>
    public class SystemVersionService : ISystemVersionService
    {
        private VersionInfo versionInfo;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод возвращает информацию о версии приложения
        /// </summary>
        /// <returns>Информация о версии</returns>
        public IDataResult<VersionInfo> GetVersionInfo()
        {
            if (this.versionInfo.IsNotNull())
            {
                return new GenericDataResult<VersionInfo>(this.versionInfo);
            }

            var executingAssembly = SystemVersionService.GetWebEntryAssembly();
            var assemblyVersion = executingAssembly.GetName().Version;
            var assemblyBranch = FileVersionInfo.GetVersionInfo(executingAssembly.Location).ProductVersion;

            var appVersion = $"release {assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";

            if (assemblyBranch == "test")
            {
                appVersion = "test";
            }

            this.versionInfo = new VersionInfo
            {
                AppVersion = appVersion,
                BuildNumber = assemblyVersion.Revision,
                Branch = assemblyBranch,
                BuildDate = executingAssembly.GetLinkerTime(TimeZoneInfo.Local),
                Region = this.GetRegionName()
            };

            return new GenericDataResult<VersionInfo>(this.versionInfo);
        }

        private static Assembly GetWebEntryAssembly()
        {
            if (System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                return null;
            }

            var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }

        private string GetRegionName()
        {
            var configFilePath = this.GetConfigFilePath("~/build-info.user.json");
            if (!File.Exists(configFilePath))
            {
                configFilePath = this.GetConfigFilePath("~/build-info.json");
            }

            if (File.Exists(configFilePath))
            {
                var fileData = File.ReadAllText(configFilePath);
                var regionData = JsonNetConvert.DeserializeObject<JObject>(this.Container, fileData);

                return regionData["region"].Value<string>();
            }

            return null;
        }

        private string GetConfigFilePath(string virtualFilePath)
        {
            var path = ApplicationContext.Current.MapPath(virtualFilePath);

            if (path.IsEmpty())
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, virtualFilePath.Replace("~/", string.Empty));
            }

            return path;
        }
    }
}