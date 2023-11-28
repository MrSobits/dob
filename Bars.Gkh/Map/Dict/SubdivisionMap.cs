namespace Bars.Gkh.Map.Dict
{
	using B4.Modules.Mapping.Mappers;
	using Bars.Gkh.Entities;

	/// <summary>Маппинг для "Справочник Подразделений"</summary>
	public class SubdivisionMap : BaseImportableEntityMap<Subdivision>
    {
        
        public SubdivisionMap() : 
                base("Справочник Подразделений", "GKH_DICT_SUBDIVISION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(500);
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
