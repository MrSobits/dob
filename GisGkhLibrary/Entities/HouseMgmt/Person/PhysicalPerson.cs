using GisGkhLibrary.Entities.HouseMgmt.Person.Identifiers;
using GisGkhLibrary.Enums;
using System;

namespace GisGkhLibrary.Entities.HouseMgmt.Person
{
    public class PhysicalPerson : PersonBase
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Пол 
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Удостоверение личности
        /// </summary>
        public IdentifierBase Identifier { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public string PlaceBirth { get; internal set; }        
    }
}
