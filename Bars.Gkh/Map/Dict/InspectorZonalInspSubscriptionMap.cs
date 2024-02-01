namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Подписка инспекторов на комиссии"</summary>
    public class InspectorZonalInspSubscriptionMap : BaseImportableEntityMap<InspectorZonalInspSubscription>
    {
        
        public InspectorZonalInspSubscriptionMap() : 
                base("Подписка инспекторов на комиссии", "GKH_DICT_INSP_ZONALINSP_SUBSCRIP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSP_ID").Fetch();
            Reference(x => x.ZonalInspection, "Комиссия").Column("ZONAL_INSP_ID").Fetch();
        }
    }
}
