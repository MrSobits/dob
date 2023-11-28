using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    public class CostLimitsMap : BaseEntityMap<CostLimit>
    {
        public CostLimitsMap() : base("Предельные стоимости работ или услуг", "OVRHL_KPKR_COST_LIMITS")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Work, "Работа").Column("WORK_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").Fetch();
            //
            Property(x => x.Cost, "Максимальная стоимость").Column("COST").NotNull();
            Property(x => x.DateStart, "Дата начала действия условий").Column("DATE_START");
            Property(x => x.DateEnd, "Дата прекращения действия условий").Column("DATE_END");
            Property(x => x.FloorStart, "Допустимый тип дома").Column("FLOOR_START");
            Property(x => x.FloorEnd, "Допустимое состояние дома").Column("FLOOR_END");
        }
    }
}
