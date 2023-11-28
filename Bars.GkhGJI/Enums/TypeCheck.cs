namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ рассмотрения
    /// </summary>
    public enum TypeCheck
    {
        [Display("Плановая")]
        PlannedExit = 1,

        [Display("Внеплановая")]
        NotPlannedExit = 2,
    }
}