using GisGkhLibrary.Enums.HouseMgmt;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    public class HeatingSystemType
    {
        /// <summary>
        /// Централизованная/нецентрализованная
        /// </summary>
        public CentralizedOrNot CentralizedOrNot { get; set; }

        /// <summary>
        /// Открытая/Закрытая
        /// </summary>
        public OpenOrNot OpenOrNot { get; set; }
    }
}
