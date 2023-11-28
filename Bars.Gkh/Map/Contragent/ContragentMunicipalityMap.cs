/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities;
/// 
///     public class ContragentMunicipalityMap : BaseImportableEntityMap<ContragentMunicipality>
///     {
///         public ContragentMunicipalityMap()
///             : base("GKH_CONTRAGENT_MUNICIPALITY")
///         {
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Муниципальные образования контрагенты"</summary>
    public class ContragentMunicipalityMap : BaseImportableEntityMap<ContragentMunicipality>
    {
        
        public ContragentMunicipalityMap() : 
                base("Муниципальные образования контрагенты", "GKH_CONTRAGENT_MUNICIPALITY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
