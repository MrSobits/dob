namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы решения собственников помещений МКД
    /// </summary>
    public enum PropertyOwnerDecisionType
    {
        [Display("Выбор способа формирования фонда кап.ремонта")]
        SelectMethodForming = 10,

        [Display("Установление минимального размера взноса на кап.ремонт")]
        SetMinAmount = 20,

        [Display("Установление фактических дат проведения КР")]
        ListOverhaulServices = 30,

        [Display("Установление минимального размера фонда КР")]
        MinCrFundSize = 60
    }
}