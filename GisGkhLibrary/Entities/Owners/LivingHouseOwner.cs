using GisGkhLibrary.Entities.Person;

namespace GisGkhLibrary.Entities.Owners
{
    /// <summary>
    /// Собственник или пользователь жилого дома (домовладения)
    /// </summary>
    public class LivingHouseOwner : OwnerBase
    {
        public PersonBase Person { get; set; }
    }
}
