using GisGkhLibrary.Enums;

namespace GisGkhLibrary.Entities.HouseMgmt.Volume
{
    /// <summary>
    /// Плановый объем и режим подачи за год. 
    /// </summary>
    public class PlannedVolume
    {
        /// <summary>
        /// Плановый объем
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public OKEI Unit { get; set; }

        /// <summary>
        /// Режим подачи. Строка не более 250 символов.
        /// </summary>
        public string FeedingMode { get; set; }
    }
}
