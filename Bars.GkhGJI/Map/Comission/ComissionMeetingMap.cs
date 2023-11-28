using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Обращение граждан"</summary>
    public class ComissionMeetingMap : BaseEntityMap<ComissionMeeting>
    {
        
        public ComissionMeetingMap() : 
                base("Заявка на внесение изменений в реестр лицензий", "GJI_COMISSION_MEETING")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAL_ID").Fetch();
            this.Property(x => x.ComissionName, "ComissionName").Column("COMISSION_NAME");
            this.Property(x => x.CommissionDate, "CommissionDate").Column("COMISSION_DATE");
            this.Property(x => x.CommissionNumber, "CommissionNumber").Column("COMISSION_NUMBER");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.TimeEnd, "TimeEnd").Column("TIME_END");
            this.Property(x => x.TimeStart, "TimeStart").Column("TIME_START");        
        }
    }
}
