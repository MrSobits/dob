namespace Bars.GkhGji.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities.Dict;

    public class CategoryRiskMap : GkhBaseEntityMap<RiskCategory>
    {
        public CategoryRiskMap()
            : base("GJI_CATEGORY_RISK_UK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            this.Property(x => x.RiskFrom, "Показатель риска от").Column("RISK_FROM").NotNull();
            this.Property(x => x.RiskTo, "Показатель риска до").Column("RISK_TO").NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
        }
    }
}