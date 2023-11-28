using GisGkhLibrary.Enums.HouseMgmt;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.House
{
    /// <summary>
    /// Регистрационные данные прав или ограничения прав
    /// </summary>
    public class Restriction
    {
        /// <summary>
        /// Тип ключа
        /// </summary>
        public RightOrEncumbrance Type { get; set; }

        /// <summary>
        /// Номер государственной регистрации
        /// </summary>
        public string RegNumber { get; set; }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        public DateTime RegDate { get; set; }
    }
}
