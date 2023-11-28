namespace Bars.GkhGji.Map.Dict
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities.Dict;

    public class InspectionBaseTypeMap : GkhBaseEntityMap<InspectionBaseType>
    {
        public InspectionBaseTypeMap()
            : base("GJI_DICT_INSPECTION_BASE_TYPE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
        }
    }
}