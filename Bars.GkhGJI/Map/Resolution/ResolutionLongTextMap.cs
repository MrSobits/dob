namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Оспаривание постановления ГЖИ"</summary>
    public class ResolutionLongTextMap : BaseEntityMap<ResolutionLongText>
    {
        
        public ResolutionLongTextMap() : 
                base("Оспаривание постановления ГЖИ", "GJI_RESOLUTION_LTEXT")
        {
        }
        
        protected override void Map()
        {
      
            Reference(x => x.Resolution, "постановление").Column("RESOLUTION_ID").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
        }

        public class ResolutionLongTextNHibernateMapping : ClassMapping<ResolutionLongText>
        {
            public ResolutionLongTextNHibernateMapping()
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
