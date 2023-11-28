namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;

    /// <summary>Маппинг для информации из расчетных документов (BDPD/BDPL)</summary>
    public class BDOPERMap : BaseEntityMap<BDOPER>
    {
        
        public BDOPERMap() : 
                base("Информация из расчетных документов (BDPD/BDPL)", "GJI_VR_ASFK_BDOPER")
        {
        }

        protected override void Map()
        {
            Reference(x => x.ASFK, "ASFK").Column("ASFK_ID").Fetch();
            Reference(x => x.Resolution, "Постановление").Column("RESOLUTION_ID").Fetch();
            Property(x => x.IsPayFineAdded, "IsPayFineAdded").Column("IS_PAYFINE_ADDED");
            Property(x => x.GUID, "GUID").Column("GUID");
            Property(x => x.Sum, "Sum").Column("SUM");
            Property(x => x.InnPay, "InnPay").Column("INN_PAY");
            Property(x => x.KppPay, "KppPay").Column("KPP_PAY");
            Property(x => x.NamePay, "NamePay").Column("NAME_PAY");
            Property(x => x.Purpose, "Purpose").Column("PURPOSE");
        }
    }
}
