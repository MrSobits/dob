namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Описание постановления прокуратуры"</summary>
    public class ResolProsLongTextMap : BaseEntityMap<ResolProsLongText>
    {
        
        public ResolProsLongTextMap() : 
                base("Описание постановления прокуратуры", "GJI_RESOLPROS_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ResolPros, "Постановление прокуратуры").Column("RESOLPROS_ID").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION");
        }

        public class ResolProsLongTextNHibernateMapping : ClassMapping<ResolProsLongText>
        {
            public ResolProsLongTextNHibernateMapping()
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
