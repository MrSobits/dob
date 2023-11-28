using GisGkhLibrary.Enums.HouseMgmt;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Срок помесячного представления чего-либо
    /// </summary>
    public class MonthDate
    {
        /// <summary>
        /// День месяца. -1, если последний денб месяца
        /// </summary>
        public sbyte DayOfMonth { get; set; }

        /// <summary>
        /// Текущего или следющего месяца
        /// </summary>
        public MonthDateType Type { get; set; }
    }
}
