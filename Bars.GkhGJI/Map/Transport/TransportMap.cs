namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Таблица транспорта"</summary>
    public class TransportMap : BaseEntityMap<Transport>
    {
        
        public TransportMap() : 
                base("Правило проставления номера документа гжи", "GJI_TRANSPORT")
        {
        }
        
        protected override void Map()
        {
     
            Property(x => x.NameTransport, "Наименование транспорта").Column("NAME_TRANSPORT");
            Property(x => x.NamberTransport, "Номер транспорта").Column("NAMBER_TRANSPORT");
            Property(x => x.RegistrationNamberTransport, "Регистрационный номер транспорта").Column("REGISTRATION_NAMBER_TRANSPORT");
            Property(x => x.SeriesTransport, "Серия номера транспорта").Column("SERIES_TRANSPORT");
            Property(x => x.RegNamberTransport, "Регион регистрации номера транспорта").Column("REG_NAMBER_TRANSPORT");
        }
    }
}
