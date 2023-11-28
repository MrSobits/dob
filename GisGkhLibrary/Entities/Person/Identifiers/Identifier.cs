using System;
using GisGkhLibrary.Entities.Dictionaries;

namespace GisGkhLibrary.Entities.Person.Identifiers
{
    /// <summary>
    /// Удостоверение личности
    /// </summary>
    public class Identifier : IdentifierBase
    {
        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        public IdentifierType IdentifierType { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public DateTime IssueDate { get; set; }
    }
}
