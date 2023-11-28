namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum ExecutorType
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        [Display("Административная комиссия")]
        Mo = 10,

        /// <summary>
        /// Администрация МО
        /// </summary>
        [Display("-")]
        Mu = 20,

        /// <summary>
        /// ГЖИ
        /// </summary>
        [Display("-")]
        Gji = 30,

        /// <summary>
        /// Фонд КР
        /// </summary>
        [Display("-")]
        CrFund = 40,

        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("-")]
        None = 50
    }
}