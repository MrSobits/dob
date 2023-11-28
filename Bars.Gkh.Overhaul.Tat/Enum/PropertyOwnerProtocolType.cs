namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип протокола собственников жилых помещений
    /// </summary>
    public enum PropertyOwnerProtocolType
    {
        [Display("Протокол о формировании фонда капитального ремонта")]
        FormationFund = 0,

        [Display("Протокол о выборе управляющей организации")]
        SelectManOrg = 10,

        [Display("Постановление Исполкома МО")]
        ResolutionOfTheBoard = 20,

        [Display("Протокол о займе")]
        Loan = 30
    }
}