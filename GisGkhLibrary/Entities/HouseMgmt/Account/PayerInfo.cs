using GisGkhLibrary.Entities.HouseMgmt.Person;

namespace GisGkhLibrary.Entities.HouseMgmt.Account
{
    /// <summary>
    /// Сведения о плательщике
    /// </summary>
    public abstract class PayerInfo
    {
        /// <summary>
        /// Является нанимателем
        /// </summary>
        public bool? IsRenter { get; set; }

        /// <summary>
        /// Лицевые счета на помещение(я) разделены?
        /// </summary>
        public bool? IsAccountsDivided { get; set; }

        /// <summary>
        /// Плательщик
        /// </summary>
        public PersonBase Person { get; set; }
    }
}
