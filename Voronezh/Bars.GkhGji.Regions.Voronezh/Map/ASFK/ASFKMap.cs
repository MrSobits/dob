namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;

    /// <summary>Маппинг для обмена информацией с АСФК (фед. казначейство)</summary>
    public class ASFKMap : BaseEntityMap<ASFK>
    {
        public ASFKMap() : 
                base("Обмен информацией с АСФК (фед. казначейство)", "GJI_VR_ASFK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NumVer, "NumVer").Column("NUM_VER");
            Property(x => x.Former, "Former").Column("FORMER");
            Property(x => x.FormVer, "FormVer").Column("FORM_VER");
            Property(x => x.NormDoc, "NormDoc").Column("NORM_DOC");
            Property(x => x.KodTofkFrom, "KodTofkTo").Column("KOD_TOFK_FROM");
            Property(x => x.NameTofkFrom, "NameTofkTo").Column("NAME_TOFK_FROM");
            Property(x => x.BudgetLevel, "BudgetLevel").Column("BUDGET_LEVEL");
            Property(x => x.KodUbp, "KodUbp").Column("KOD_UBP");
            Property(x => x.NameUbp, "NameUbp").Column("NAME_UBP");
            Property(x => x.GuidVT, "GuidVT").Column("GUID_VT");
            Property(x => x.LsAdb, "LsAdb").Column("LS_ADB");
            Property(x => x.DateOtch, "DateOtch").Column("DATE_OTCH");
            Property(x => x.DateOld, "DateOld").Column("DATE_OLD");
            Property(x => x.VidOtch, "VidOtch").Column("VID_OTCH");
            Property(x => x.KodTofkVT, "KodTofkVT").Column("KOD_TOFK_VT");        
            Property(x => x.NameTofkVT, "NameTofkVT").Column("NAME_TOFK_VT");
            Property(x => x.KodUbpAdb, "KodUbpAdb").Column("KOD_UBP_ADB");
            Property(x => x.NameUbpAdb, "NameUbpAdb").Column("NAME_UBP_ADB");
            Property(x => x.KodGadb, "KodGadb").Column("KOD_GADB");
            Property(x => x.NameGadb, "NameGadb").Column("NAME_GADB");
            Property(x => x.NameBud, "NameBud").Column("NAME_BUD");
            Property(x => x.Oktmo, "Oktmo").Column("OKTMO");
            Property(x => x.OkpoFo, "OkpoFo").Column("OKPO_FO");
            Property(x => x.NameFo, "NameFo").Column("NAME_FO");
            Property(x => x.DolIsp, "DolIsp").Column("DOL_ISP");
            Property(x => x.NameIsp, "NameIsp").Column("NAME_ISP");
            Property(x => x.TelIsp, "TelIsp").Column("TEL_ISP");
            Property(x => x.DatePod, "DatePod").Column("DATE_POD");
            Property(x => x.SumInItogV, "SumInItogV").Column("SUM_IN_ITOG_V");
            Property(x => x.SumOutItogV, "SumOutItogV").Column("SUM_OUT_ITOG_V");
            Property(x => x.SumZachItogV, "SumZachItogV").Column("SUM_ZACH_ITOG_V");
            Property(x => x.SumNOutItogV, "SumNOutItogV").Column("SUM_N_OUT_ITOG_V");
            Property(x => x.SumNZachItogV, "SumNZachItogV").Column("SUM_N_ZACH_ITOG_V");
            Property(x => x.SumBeginIn, "SumBeginIn").Column("SUM_BEGIN_IN");
            Property(x => x.SumBeginOut, "SumBeginOut").Column("SUM_BEGIN_OUT");
            Property(x => x.SumBeginZach, "SumBeginZach").Column("SUM_BEGIN_ZACH");
            Property(x => x.SumBeginNOut, "SumBeginNOut").Column("SUM_BEGIN_N_OUT");
            Property(x => x.SumBeginNZach, "SumBeginNZach").Column("SUM_BEGIN_N_ZACH");
            Property(x => x.SumEndIn, "SumEndIn").Column("SUM_END_IN");
            Property(x => x.SumEndOut, "SumEndOut").Column("SUM_END_OUT");
            Property(x => x.SumEndZach, "SumEndZach").Column("SUM_END_ZACH");
            Property(x => x.SumEndNOut, "SumEndNOut").Column("SUM_END_N_OUT");
            Property(x => x.SumEndNZach, "SumEndNZach").Column("SUM_END_N_ZACH");
        }
    }
}
