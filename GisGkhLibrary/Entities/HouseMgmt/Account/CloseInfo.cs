using System;
using GisGkhLibrary.Entities.Dictionaries;

namespace GisGkhLibrary.Entities.HouseMgmt.Account
{
    /// <summary>
    /// Информация о закрытии ЛС
    /// </summary>
    public class CloseInfo
    {
        /// <summary>
        /// Причина закрытия (НСИ 22)
        /// </summary>
        public AccountCloseReason CloseReason { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public DateTime CloseDate { get; internal set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Description { get; internal set; }
    }
}
