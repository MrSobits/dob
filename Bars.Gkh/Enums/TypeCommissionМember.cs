using Bars.B4.Utils;

namespace Bars.Gkh.Enums
{
    /// <summary>
    /// Тип члена совета многоквартироного дома (МКД)
    /// </summary>
    public enum TypeCommissionMember
    {
        [Display("Председатель")]
        Сhairman = 10,

        [Display("Заместитель председателя")]
        ViceChairman = 15,

        [Display("Ответственный секретарь")]
        Secretary = 20,        

        [Display("Члены комиссии")]
        Member = 40,
        
        [Display("Должностное лицо")]
        OfficialPerson = 50
    }
}
