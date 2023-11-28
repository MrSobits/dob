namespace Bars.GkhGji.Regions.Chelyabinsk.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal;

    /// <summary>Маппинг для сущности "Задача проверки приказа ГЖИ"</summary>
	public class DisposalSurveyObjectiveMap : BaseEntityMap<DisposalSurveyObjective>
    {
        public DisposalSurveyObjectiveMap() : 
                base("Задача проверки приказа ГЖИ", "GJI_NSO_DISPOSAL_SURVEY_OBJ")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            this.Reference(x => x.SurveyObjective, "Задача проверки").Column("SURVEY_OBJ_ID").NotNull().Fetch();
        }
    }
}
