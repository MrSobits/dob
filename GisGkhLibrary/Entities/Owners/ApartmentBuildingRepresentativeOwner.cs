using GisGkhLibrary.Entities.Person;

namespace GisGkhLibrary.Entities.Owners
{
    /// <summary>
    /// Представитель собственников многоквартирного дома
    /// </summary>
    public class ApartmentBuildingRepresentativeOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
