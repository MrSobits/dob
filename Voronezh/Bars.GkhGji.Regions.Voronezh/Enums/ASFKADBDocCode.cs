using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    /// <summary>
    /// Код документа АДБ
    /// </summary>
    public enum ASFKADBDocCode
    {
        [Display("Нет данных")]
        NoValue = 0,

        [Display("Заявка на возврат")]
        ZV = 1,

        [Display("Уведомление об уточнении")]
        UF = 2,

        [Display("Уведомление об уточнении, зачете (ФНС)")]
        UN = 3,

        [Display("Межрегиональный зачет")]
        UM = 4,

        [Display("Распоряжение налогового органа (зачет)")]
        UZ = 5,

        [Display("Прочие документы")]
        XX = 6
    }
}
