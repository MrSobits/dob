namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class ComissionMeetingDocumentMap : BaseEntityMap<ComissionMeetingDocument>
    {
        
        public ComissionMeetingDocumentMap() : 
                base("Приложение", "GJI_COMISSION_MEETING_DOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ComissionMeeting, "ComissionMeeting").Column("MEETING_ID").NotNull().Fetch();
            Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.ComissionDocumentDecision, "ComissionDocumentDecision").Column("DECISION");
        }
    }
}
