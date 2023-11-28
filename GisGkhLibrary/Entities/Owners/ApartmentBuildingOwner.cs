using GisGkhLibrary.Entities.Person;

namespace GisGkhLibrary.Entities.Owners
{
    /// <summary>
    /// Собственник или пользователь жилого (нежилого) помещения в МКД
    /// </summary>
    public class ApartmentBuildingOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
