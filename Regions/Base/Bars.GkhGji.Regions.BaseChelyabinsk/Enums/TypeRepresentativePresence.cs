namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///     Протокол - Реквизиты - В присуствии/отсутствии
    /// </summary>
    public enum TypeRepresentativePresence
    {
        /// <summary>
        ///     в отсутствии
        /// </summary>
        [Display("Заочно")]
        None = 0,

        /// <summary>
        ///     в присутствии законного представителя
        /// </summary>
        [Display("в присутствии законного представителя")]
        LegalRepresentative = 10,

        /// <summary>
        ///     в присутствии уполномоченного представителя
        /// </summary>
        [Display("в присутствии уполномоченного представителя")]
        AuthorizedRepresentative = 20,


        /// <summary>
        ///     в присутствии правонарушителя"
        /// </summary>
        [Display("в присутствии правонарушителя")]
        PresentOffender = 30,



        /// <summary>
        ///     в отсутствии правонарушителя
        /// </summary>
        [Display("в отсутствии правонарушителя")]
        NoPresentOffender = 40

    }
}