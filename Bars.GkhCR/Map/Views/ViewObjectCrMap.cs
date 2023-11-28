/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности " Объект капитального ремонта"
///     /// </summary>
///     public class ViewObjectCrMap : PersistentObjectMap<ViewObjectCr>
///     {
///         public ViewObjectCrMap() : base("VIEW_CR_OBJECT")
///         {
///             Map(x => x.ProgramNum, "PROGRAM_NUM");
///             Map(x => x.ProgramCrId, "PROGRAM_ID");
///             Map(x => x.ProgramCrName, "PROGRAM_NAME");
///             Map(x => x.BeforeDeleteProgramCrId, "BEFORE_DELETE_PROGRAM_ID");
///             Map(x => x.BeforeDeleteProgramCrName, "BEFORE_DEL_PROGRAM_NAME");
///             
///             Map(x => x.Municipality, "MUNICIPALITY_NAME");
///             Map(x => x.MunicipalityId, "MUNICIPALITY_ID");
///             
///             Map(x => x.RealityObjectId, "REALITY_OBJECT_ID");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.DateAcceptCrGji, "DATE_ACCEPT_GJI");
///             Map(x => x.AllowReneg, "ALLOW_RENEG");
///             Map(x => x.MonitoringSmrId, "SMR_ID");
///             Map(x => x.Settlement, "SETTLEMENT_NAME");
///             Map(x => x.SettlementId, "SETTLEMENT_ID");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///             References(x => x.SmrState, "SMR_STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на Объект КР"</summary>
    public class ViewObjectCrMap : PersistentObjectMap<ViewObjectCr>
    {
        
        public ViewObjectCrMap() : 
                base("Вьюха на Объект КР", "VIEW_CR_OBJECT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ProgramNum, "Номер по программе").Column("PROGRAM_NUM");
            this.Property(x => x.ProgramCrId, "Программа КР").Column("PROGRAM_ID");
            this.Property(x => x.ProgramCrName, "Программа КР").Column("PROGRAM_NAME");
            this.Property(x => x.BeforeDeleteProgramCrId, "Программа КР, на которую ссылался объект до удаления").Column("BEFORE_DELETE_PROGRAM_ID");
            this.Property(x => x.BeforeDeleteProgramCrName, "Программа КР, на которую ссылался объект до удаления").Column("BEFORE_DEL_PROGRAM_NAME");
            this.Property(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_NAME");
            this.Property(x => x.MunicipalityId, "Id Муниципального района").Column("MUNICIPALITY_ID");
            this.Property(x => x.RealityObjectId, "Объект недвижимости").Column("REALITY_OBJECT_ID");
            this.Property(x => x.Address, "Муниципальное образование").Column("ADDRESS");
            this.Property(x => x.Iscluttered, "Захламлено").Column("IS_CLUTTERED");
            this.Property(x => x.DeadlineMissed, "Срыв сроков").Column("DEADLINE_MISSED");
            this.Property(x => x.DateAcceptCrGji, "Дата принятия ГЖИ").Column("DATE_ACCEPT_GJI");
            this.Property(x => x.AllowReneg, "Разрешение на повторное согласование").Column("ALLOW_RENEG");
            this.Property(x => x.MonitoringSmrId, "Разрешение на повторное согласование").Column("SMR_ID");
            this.Property(x => x.PeriodName, "Наименование периода").Column("PERIOD_NAME");
            this.Property(x => x.Settlement, "Муниципальное образование").Column("SETTLEMENT_NAME");
            this.Property(x => x.SettlementId, "Id Муниципального образования").Column("SETTLEMENT_ID");
            this.Reference(x => x.State, "Статус объекта кр").Column("STATE_ID").Fetch();
            this.Reference(x => x.SmrState, "Статус мониторинга смр").Column("SMR_STATE_ID").Fetch();
        }
    }
}
