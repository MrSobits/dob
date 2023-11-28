using GisGkhLibrary.Entities.Person;

namespace GisGkhLibrary.Entities.Owners
{
    /// <summary>
    /// Единоличный собственник помещений в многоквартирном доме
    /// </summary>
    public class ApartmentBuildingSoleOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
