namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class ComissionMeetingInspectorMap : BaseEntityMap<ComissionMeetingInspector>
    {
        
        public ComissionMeetingInspectorMap() : 
                base("Приложение", "GJI_COMISSION_MEETING_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ComissionMeeting, "ComissionMeeting").Column("MEETING_ID").NotNull().Fetch();
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.YesNoNotSet, "YesNoNotSet").Column("PRESENT");
            Property(x => x.TypeCommissionMember, "TypeCommissionMember").Column("TYPE_COMISSION_MEMBER");
        }
    }
}
