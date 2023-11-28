namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;

    /// <summary>Маппинг для информации по исполненным операциям (раздел 2) и реквизитам ТОФК, по которым учтена операция</summary>
    public class VTOPERMap : BaseEntityMap<VTOPER>
    {
        
        public VTOPERMap() : 
                base("Информация по исполненным операциям (раздел 2) и реквизиты ТОФК, по которым учтена операция", "GJI_VR_ASFK_VTOPER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ASFK, "ASFK").Column("ASFK_ID").Fetch();
            Property(x => x.GUID, "GUID").Column("GUID");
            Property(x => x.KodDoc, "KodDoc").Column("KOD_DOC");
            Property(x => x.NomDoc, "NomDoc").Column("NOM_DOC");
            Property(x => x.DateDoc, "DateDoc").Column("DATE_DOC");
            Property(x => x.KodDocAdb, "KodDocAdb").Column("KOD_DOC_ADB");
            Property(x => x.NomDocAdb, "NomDocAdb").Column("NOM_DOC_ADB");
            Property(x => x.DateDocAdb, "DateDocAdb").Column("DATE_DOC_ADB");
            Property(x => x.SumIn, "SumIn").Column("SUM_IN");
            Property(x => x.SumOut, "SumOut").Column("SUM_OUT");
            Property(x => x.SumZach, "SumZach").Column("SUM_ZACH");
            Property(x => x.Note, "Note").Column("NOTE");
            Property(x => x.TypeKbk, "TypeKbk").Column("TYPE_KBK");
            Property(x => x.Kbk, "Kbk").Column("KBK");
            Property(x => x.AddKlass, "AddKlass").Column("ADD_KLASS");
            Property(x => x.Okato, "Okato").Column("OKATO");        
            Property(x => x.InnAdb, "InnAdb").Column("INN_ADB");
            Property(x => x.KppAdb, "KppAdb").Column("KPP_ADB");
        }
    }
}
