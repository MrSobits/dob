using GisGkhLibrary.Entities.HouseMgmt.Person;

namespace GisGkhLibrary.Entities.HouseMgmt.Owners
{
    /// <summary>
    /// Представитель собственников многоквартирного дома
    /// </summary>
    public class ApartmentBuildingRepresentativeOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
