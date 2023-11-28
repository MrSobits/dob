namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Оспаривание постановления ГЖИ"</summary>
    public class ComissionMeetingDocumentLongTextMap : BaseEntityMap<ComissionMeetingDocumentLongText>
    {

        public ComissionMeetingDocumentLongTextMap() :
                base("Оспаривание постановления ГЖИ", "GJI_COMISSION_MEETING_DOCUMENT_LTEXT")
        {
        }

        protected override void Map()
        {

            Reference(x => x.ComissionMeetingDocument, "постановление").Column("DOC_ID").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
        }

        public class ComissionMeetingDocumentLongTextNHibernateMapping : ClassMapping<ComissionMeetingDocumentLongText>
        {
            public ComissionMeetingDocumentLongTextNHibernateMapping()
            {
                this.Property(
                    x => x.Description,
                    mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            }
        }
    }
}
