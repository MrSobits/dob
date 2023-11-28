/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.Version
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Bars.Gkh.Overhaul.Tat.Entities;
///     using Bars.Gkh.Overhaul.Tat.Enum;
/// 
///     public class VersionRecordMap : BaseEntityMap<VersionRecord>
///     {
///         public VersionRecordMap()
///             : base("OVRHL_VERSION_REC")
///         {
///             Map(x => x.Year, "YEAR").Not.Nullable();
///             Map(x => x.FixedYear, "FIXED_YEAR").Not.Nullable();
///             Map(x => x.CorrectYear, "CORRECT_YEAR");
///             Map(x => x.Sum, "SUM").Not.Nullable();
///             Map(x => x.CommonEstateObjects, "CEO_STRING");
///             Map(x => x.Point, "POINT").Not.Nullable();
///             Map(x => x.IndexNumber, "INDEX_NUM").Not.Nullable();
///             Map(x => x.TypeDpkrRecord, "TYPE_DPKR_RECORD").Not.Nullable().CustomType<TypeDpkrRecord>();
/// 
///             References(x => x.ProgramVersion, "VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.VersionRecord"</summary>
    public class VersionRecordMap : BaseEntityMap<VersionRecord>
    {
        
        public VersionRecordMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.VersionRecord", "OVRHL_VERSION_REC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.FixedYear, "FixedYear").Column("FIXED_YEAR").NotNull();
            Property(x => x.CorrectYear, "CorrectYear").Column("CORRECT_YEAR");
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.CommonEstateObjects, "CommonEstateObjects").Column("CEO_STRING");
            Property(x => x.Point, "Point").Column("POINT").NotNull();
            Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUM").NotNull();
            Property(x => x.TypeDpkrRecord, "TypeDpkrRecord").Column("TYPE_DPKR_RECORD").NotNull();
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
        }
    }
}
