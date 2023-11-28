
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;


    /// <summary>Маппинг для "Ходатайства протокола ГЖИ"</summary>
    public class Protocol197PetitionMap : BaseEntityMap<Protocol197Petition>
    {
        
        public Protocol197PetitionMap() : 
                base("Ходатайства протокола ГЖИ", "GJI_PROTOCOL197_PETITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PetitionAuthorFIO, "ФИО").Column("AUTHOR_FIO");
            Property(x => x.PetitionAuthorDuty, "Должность").Column("AUTHOR_DUTY");
            Property(x => x.PetitionDate, "PetitionDate").Column("PETITION_DATE");
            Property(x => x.Workplace, "Место работы").Column("WORKPLACE").Length(300);
         //   Reference(x => x.Inspector, "Рассмотрел(-а)").Column("INSPECTOR_ID").NotNull();
            Property(x => x.PetitionText, "Текст ходатайства").Column("PETITION_TEXT").Length(3500);
            Property(x => x.PetitionDecisionText, "Текст ходатайства").Column("PETITION_DEC_TEXT").Length(3500);
            Property(x => x.Aprooved, "Aprooved").Column("APROOVED");
            Reference(x => x.Protocol197, "Протокол").Column("PROTOCOL197_ID").NotNull().Fetch();
        }
    }
}
