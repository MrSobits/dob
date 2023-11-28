namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Программа капитального ремонта"</summary>
    public class ProgramCrMap : BaseImportableEntityMap<ProgramCr>
    {
        public ProgramCrMap()
            : base("Программа капитального ремонта", "CR_DICT_PROGRAM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeVisibilityProgramCr, "Видимость").Column("TYPE_VISIBILITY").NotNull();
            this.Property(x => x.TypeProgramCr, "Тип КР").Column("TYPE_PROGRAM_CR").NotNull();
            this.Property(x => x.TypeProgramStateCr, "Состояние").Column("TYPE_PROGRAM_STATE").NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").Length(200);
            this.Property(x => x.UsedInExport, "Используется при экспорте").Column("USED_IN_EXPORT").NotNull();
            this.Property(x => x.NotAddHome, "Не доступно добавление домов").Column("NOT_ADD_HOME").NotNull();
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.Name, "Наименование программы").Column("NAME").Length(300);
            this.Property(x => x.MatchFl, "Соответствует ФЗ").Column("MATCH_FL").NotNull();
            this.Property(x => x.AddWorkFromLongProgram, "Добавление видов работ из ДПКР").Column("ADD_WORK_FROM_LONG_PROG").NotNull();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.NormativeDoc, "Постановление об утверждении КП").Column("NORMATIVE_DOC_ID");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentDepartment, "Орган, принявший документ").Column("DOCUMENT_DEPARTMENT");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Property(x => x.GisGkhDocumentAttachmentGuid, "ГИС ЖКХ GUID вложения документа плана").Column("GIS_GKH_DOC_ATTACHMENT_GUID").Length(36);
            this.Property(x => x.GisGkhDocumentGuid, "ГИС ЖКХ GUID документа плана").Column("GIS_GKH_DOC_GUID").Length(36);
            this.Property(x => x.GisGkhDocumentTransportGuid, "ГИС ЖКХ Transport GUID документа плана").Column("GIS_GKH_DOC_TRANSPORT_GUID").Length(36);
        }
    }
}
