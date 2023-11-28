namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealityObj;

    public class RealityObjectTechnicalMonitoringMap : BaseEntityMap<RealityObjectTechnicalMonitoring>
    {
        public RealityObjectTechnicalMonitoringMap() : 
                base("Технический мониторинг", "GKH_OBJ_TECHNICAL_MONITORING")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE").NotNull();
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.MonitoringTypeDict, "Тип мониторинга").Column("TYPE_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").NotNull();
        }
    }
}
