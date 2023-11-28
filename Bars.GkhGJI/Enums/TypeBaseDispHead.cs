namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания проверки по поручению руководства
    /// </summary>
    public enum TypeBaseDispHead
    {
        [Display("Факт фиксации нарушения")]
        ExecutivePower = 10,

        [Display("Поручение руководителя ОМСУ")]
        FailureRemoveViolation = 20
    }
}