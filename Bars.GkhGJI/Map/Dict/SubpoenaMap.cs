/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Мероприятия по устранению нарушений"
///     /// </summary>
///     public class ActionsRemovViolMap : BaseEntityMap<ActionsRemovViol>
///     {
///         public ActionsRemovViolMap()
///             : base("GJI_DICT_ACTREMOVVIOL")
///         {
///             Map(x => x.Name, "NAME").Length(500).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Повестка на Комиссию"</summary>
    public class SubpoenaMap : BaseEntityMap<Subpoena>
    {
        
        public SubpoenaMap() : 
                base("Мероприятия по устранению нарушений", "GJI_DICT_SUBPOENA")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(500).NotNull();
            Property(x => x.DateOfProceedings, "Дата рассмотрения дела").Column("DATE_OF_PROCEEDINGS");
            Property(x => x.HourOfProceedings, "Время рассмотрения дела(час)").Column("HOUR_OF_PROCEEDINGS");
            Property(x => x.MinuteOfProceedings, "Время рассмотрения дела(мин)").Column("MINUTE_OF_PROCEEDINGS");
            Property(x => x.ProceedingCopyNum, "Количество экземпляров").Column("PROCEEDING_COPY_NUM");
            Property(x => x.ProceedingsPlace, "Место рассмотрения дела").Column("PROCEEDINGS_PLACE");
            Reference(x => x.Protocol, "Связь с протоколом ").Column("PROTOCOL_ID").Fetch();
            Reference(x => x.ComissionMeeting, "Связь с таблицей Комиссий").Column("COMISSION_MEETING_ID").Fetch();
   
        }
    }
}