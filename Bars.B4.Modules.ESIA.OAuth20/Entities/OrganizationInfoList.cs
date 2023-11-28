namespace Bars.B4.Modules.ESIA.OAuth20.Entities
{
    using System.Collections.Generic;
    using EsiaNET;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Список информации по организации
    /// </summary>
    public class OrganizationInfoList
    {
        /// <summary>
        /// Организации
        /// </summary>
        public List<OrganizationInfo> Organizations { get; }

        internal OrganizationInfoList(JObject organizationInfoList)
        {
            if (organizationInfoList == null)
                return;

            var organizationsDict = (IDictionary<string, JToken>)organizationInfoList;

            Organizations = new List<OrganizationInfo>();

            var elementsStr = EsiaHelpers.PropertyValueIfExists("elements", organizationsDict);
            var elementsArray = JsonNetConvert.DeserializeObject<List<OrganizationInfoDto>>(elementsStr);

            foreach (var element in elementsArray)
            {
                Organizations.Add(new OrganizationInfo
                {
                    Id = element.oid,
                    FullName = element.fullName,
                    Ogrn = element.ogrn,
                    Chief = element.chief == "true"
                });
            }
        }

        private class OrganizationInfoDto
        {
            public string oid { get; set; }
            public string fullName { get; set; }
            public string ogrn { get; set; }
            public string chief { get; set; }
        }
    }
}
