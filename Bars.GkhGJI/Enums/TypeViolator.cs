namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип нарушителя
    /// </summary>
    public enum TypeViolator
    {
        [Display("Индивидуальный предприниматель")]
        IP = 1,

        [Display("Юридическое лицо")]
        UridicalPerson = 2,

        [Display("Физическое лицо")]
        PhisicalPerson = 3,

        [Display("Должностное лицо")]
        OfficialPerson = 4,
    }
}