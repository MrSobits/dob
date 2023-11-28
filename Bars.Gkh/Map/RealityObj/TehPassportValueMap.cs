namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Значение ТехПаспорта"</summary>
    public class TehPassportValueMap : BaseImportableEntityMap<TehPassportValue>
    {
        
        public TehPassportValueMap() : 
                base("Значение ТехПаспорта", "TP_TEH_PASSPORT_VALUE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.FormCode, "Код Формы").Column("FORM_CODE");
            Property(x => x.CellCode, "Код Ячейки").Column("CELL_CODE");
            Property(x => x.Value, "Значение").Column("VALUE");
            Reference(x => x.TehPassport, "ТехПаспорт").Column("TEH_PASSPORT_ID").NotNull().Fetch();
        }
    }
}
