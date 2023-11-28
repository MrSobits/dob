using GisGkhLibrary.Entities.HouseMgmt.Person;

namespace GisGkhLibrary.Entities.HouseMgmt.Owners
{
    /// <summary>
    /// Единоличный собственник помещений в многоквартирном доме
    /// </summary>
    public class ApartmentBuildingSoleOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
