/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
///     using Bars.Gkh.Overhaul.Tat.Enum;
/// 
///     public class ProgramVersionMap : BaseEntityMap<ProgramVersion>
///     {
///         public ProgramVersionMap()
///             : base("OVRHL_PRG_VERSION")
///         {
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.Fetch);
///             Map(x => x.Name, "NAME", true);
///             Map(x => x.VersionDate, "VERSION_DATE", true);
///             Map(x => x.IsMain, "IS_MAIN", true, false);
///             Map(x => x.CopyingState, "COPYING_STATE", true, ProgramVersionCopyingState.NotCopied);
/// 
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ProgramVersion"</summary>
    public class ProgramVersionMap : BaseEntityMap<ProgramVersion>
    {
        
        public ProgramVersionMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ProgramVersion", "OVRHL_PRG_VERSION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(250).NotNull();
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").Fetch();
            Property(x => x.VersionDate, "VersionDate").Column("VERSION_DATE").NotNull();
            Property(x => x.IsMain, "IsMain").Column("IS_MAIN").DefaultValue(false).NotNull();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            Property(x => x.CopyingState, "CopyingState").Column("COPYING_STATE").DefaultValue(ProgramVersionCopyingState.NotCopied).NotNull();
        }
    }
}
