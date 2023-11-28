using System;
using GisGkhLibrary.Enums;

namespace GisGkhLibrary.Entities.HouseMgmt.Account
{
    public class IndividualPayerInfo : PayerInfo
    {
        /// <summary>
        /// Пол 
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? DateOfBirth { get; set; }


    }
}
