/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности Опубликованная программа
///     /// </summary>
///     public class PublishedProgramMap : BaseEntityMap<PublishedProgram>
///     {
///         public PublishedProgramMap()
///             : base("OVRHL_PUBLISH_PRG")
///         {
///             References(x => x.ProgramVersion, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PublishedProgram"</summary>
    public class PublishedProgramMap : BaseEntityMap<PublishedProgram>
    {
        
        public PublishedProgramMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.PublishedProgram", "OVRHL_PUBLISH_PRG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
