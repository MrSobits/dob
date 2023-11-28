/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.Version
/// {
///     using B4.DataAccess;
///     using Hmao.Entities.Version;
///     using Hmao.Enum;
/// 
///     public class VersionActualizeLogMap : BaseImportableEntityMap<VersionActualizeLog>
///     {
///         public VersionActualizeLogMap()
///             : base("OVRHL_ACTUALIZE_LOG")
///         {
///             Map(x => x.CountActions, "COUNT_ACTIONS");
///             Map(x => x.UserName, "USER_NAME");
///             Map(x => x.DateAction, "DATE_ACTION").Not.Nullable();
///             Map(x => x.ActualizeType, "TYPE_ACTUALIZE").Not.Nullable().CustomType<VersionActualizeType>();
/// 
///             References(x => x.ProgramVersion, "VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.LogFile, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map.Version
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.Version.VersionActualizeLog"</summary>
    public class VersionActualizeLogMap : BaseImportableEntityMap<VersionActualizeLog>
    {
        
        public VersionActualizeLogMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.Version.VersionActualizeLog", "OVRHL_ACTUALIZE_LOG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CountActions, "количество выполненных действий").Column("COUNT_ACTIONS");
            Property(x => x.ProgramCrName, "Имя краткосрочной программы, в рамках которой запускалось действие").Column("PROGRAM_CR_NAME");
            Property(x => x.UserName, "Наименвоание пользователя").Column("USER_NAME");
            Property(x => x.DateAction, "Дата выполнения действия").Column("DATE_ACTION").NotNull();
            Property(x => x.ActualizeType, "Тип актуализации").Column("TYPE_ACTUALIZE").NotNull();
            Reference(x => x.ProgramVersion, "Версия").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.LogFile, "LogFile").Column("FILE_ID").Fetch();
        }
    }
}
