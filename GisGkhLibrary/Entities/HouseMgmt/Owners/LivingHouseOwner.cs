using GisGkhLibrary.Entities.HouseMgmt.Person;

namespace GisGkhLibrary.Entities.HouseMgmt.Owners
{
    /// <summary>
    /// Собственник или пользователь жилого дома (домовладения)
    /// </summary>
    public class LivingHouseOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
