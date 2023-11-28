/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class ProgramVersionMap : BaseImportableEntityMap<ProgramVersion>
///     {
///         public ProgramVersionMap()
///             : base("OVRHL_PRG_VERSION")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable();
///             Map(x => x.VersionDate, "VERSION_DATE");
///             Map(x => x.IsMain, "IS_MAIN");
///             Map(x => x.ActualizeDate, "ACTUALIZE_DATE");
/// 
///             References(x => x.Municipality, "MU_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.ProgramVersion"</summary>
    public class ProgramVersionMap : BaseImportableEntityMap<ProgramVersion>
    {  
        public ProgramVersionMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.ProgramVersion", "OVRHL_PRG_VERSION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").NotNull();
            this.Property(x => x.VersionDate, "VersionDate").Column("VERSION_DATE");
            this.Property(x => x.IsMain, "IsMain").Column("IS_MAIN");
            this.Property(x => x.ActualizeDate, "Дата последнего добавления актуальных записей").Column("ACTUALIZE_DATE");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ ГУИД").Column("GIS_GKH_GUID");
            this.Reference(x => x.Municipality, "Municipality").Column("MU_ID").NotNull().Fetch();
            this.Reference(x => x.ParentVersion, "Версия, с которой сделано копирование").Column("PARENT_VERSION_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
