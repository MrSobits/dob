namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;
    
    /// <summary>
    /// Тип нарушителя для адресов 
    /// </summary>
    public enum TypeViolatorOwnerRoom
    {
        [Display("Юридическое лицо")]
        UridicalPerson = 2,

        [Display("Физическое лицо")]
        PhisicalPerson = 3,
    }
}