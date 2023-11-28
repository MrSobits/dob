using GisGkhLibrary.Enums;
using System;

namespace GisGkhLibrary.Entities.OrganizationTypes
{
    /// <summary>
    /// Индивидуальный предприниматель
    /// </summary>
    public class Entrp : OrganizationTypeBase
    {
        public string OGRNIP { get; set; }

        public string FirstName { get; internal set; }

        public string Surname { get; internal set; }

        public string Patronymic { get; internal set; }

        public Gender? Sex { get; internal set; }

        public DateTime? StateRegistrationDate { get; internal set; }
    }
}
