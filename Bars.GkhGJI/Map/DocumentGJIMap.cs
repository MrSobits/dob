namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Базовый документ ГЖИ"</summary>
    public class DocumentGjiMap : BaseEntityMap<DocumentGji>
    {
        
        public DocumentGjiMap() : 
                base("Базовый документ ГЖИ", "GJI_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOCUMENT").NotNull();
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(300);
            this.Property(x => x.CaseNumber, "Номер документа").Column("CASE_NUMBER").Length(300);
            this.Property(x => x.DocumentNum, "Номер документа (Целая часть)").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentSubNum, "Дополнительный номер документа (порядковый номер если документов одного типа неск" +
                    "олько)").Column("DOCUMENT_SUBNUM");
            this.Property(x => x.LiteralNum, "Буквенный подномер").Column("LITERAL_NUM");
            this.Property(x => x.DocumentYear, "Год документа").Column("DOCUMENT_YEAR");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Reference(x => x.Inspection, "Мероприятие комиссии").Column("INSPECTION_ID");
            this.Reference(x => x.Stage, "Этап проверки").Column("STAGE_ID");
            this.Reference(x => x.ComissionMeeting, "ComissionMeeting").Column("COMISSION_ID");
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAL_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}