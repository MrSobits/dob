namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Таблица транспорта"</summary>
    public class TransportDisposalMap : BaseEntityMap<TransportDisposal>
    {
        
        public TransportDisposalMap() : 
                base("Таблица связывающая транспорт с распоряжениями", "GJI_TRANSPORT_DISPOSAL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Transport, "Транспорт").Column("TRANSPORT_ID").Fetch();
            Reference(x => x.Disposal, "Распоряжения").Column("DISPOSAL_ID").Fetch();
        }
    }
}
