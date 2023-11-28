﻿namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using Bars.B4.Utils;

    public enum TypeVerificationResult
    {
        [Display("выявлены нарушения обязательных требований или требований, установленных муниципальными правовыми актами (с указанием положений (нормативных) правовых актов)")]
        Type10 = 10,

        [Display("выявлены несоответствия сведений, содержащихся в уведомлении о начале осуществления отдельных видов предпринимательской деятельности, обязательным требованиям (с указанием положений (нормативных) правовых актов)")]
        Type20 = 20,

        [Display("выявлены факты невыполнения предписаний органов государственного контроля (надзора), органов муниципального контроля (с указанием реквизитов выданных предписаний)")]
        Type30 = 30,

        [Display("нарушений не выявлено")]
        Type40 = 40,
    }
}