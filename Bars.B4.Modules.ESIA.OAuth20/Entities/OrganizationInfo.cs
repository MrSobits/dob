namespace Bars.B4.Modules.ESIA.OAuth20.Entities
{
    using System.Collections.Generic;
    using EsiaNET;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Информация по организации
    /// </summary>
    public class OrganizationInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// Наименование подразделения
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Является шефом
        /// </summary>
        public bool Chief { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public string Kpp { get; set; }

        internal OrganizationInfo() { }

        internal OrganizationInfo(JObject organizationInfo)
        {
            if (organizationInfo == null)
                return;

            var organizationsDict = (IDictionary<string, JToken>)organizationInfo;

            this.Inn = EsiaHelpers.PropertyValueIfExists("inn", organizationsDict);
            this.Kpp = EsiaHelpers.PropertyValueIfExists("kpp", organizationsDict);
        }
    }
}