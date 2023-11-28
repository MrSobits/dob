using GisGkhLibrary.Entities.HouseMgmt.Person;

namespace GisGkhLibrary.Entities.HouseMgmt.Owners
{
    /// <summary>
    /// Собственник или пользователь жилого (нежилого) помещения в МКД
    /// </summary>
    public class ApartmentBuildingOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
