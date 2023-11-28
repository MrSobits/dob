namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Метод расчета эталонного начисления
    /// </summary>
    public enum DebtCalcMethod
    {
        /// <summary>
        /// Суд
        /// </summary>
        [Display("По дате открытия ЛС")]
        OpenDate = 10,

        /// <summary>
        /// Служба судебных приставов
        /// </summary>
        [Display("По первому периоду начисления")]
        FirstPeriod = 20,

        /// <summary>
        /// Служба судебных приставов
        /// </summary>
        [Display("Указаны фактические начисления")]
        None = 30
    }
}