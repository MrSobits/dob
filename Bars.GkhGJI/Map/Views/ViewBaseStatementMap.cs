/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг View "Проверка по обращению граждан"
///     /// </summary>
///     public class ViewBaseStatementMap : PersistentObjectMap<ViewBaseStatement>
///     {
///         public ViewBaseStatementMap() : base("VIEW_GJI_INS_STATEMENT")
///         {
///             Map(x => x.IsDisposal, "IS_DISPOSAL");
///             Map(x => x.RealityObjectCount, "RO_COUNT");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.PersonInspection, "PERSON_INSPECTION").CustomType<PersonInspection>();
///             Map(x => x.TypeJurPerson, "TYPE_JUR_PERSON").CustomType<TypeJurPerson>();
///             Map(x => x.InspectionNumber, "INSPECTION_NUMBER");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.RealObjAddresses, "RO_ADR");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseStatement"</summary>
    public class ViewBaseStatementMap : PersistentObjectMap<ViewBaseStatement>
    {
        
        public ViewBaseStatementMap() : 
                base("Bars.GkhGji.Entities.ViewBaseStatement", "VIEW_GJI_INS_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IsDisposal, "Наличие распоряжения").Column("IS_DISPOSAL");
            Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION");
            Property(x => x.TypeJurPerson, "Тип контрагента").Column("TYPE_JUR_PERSON");
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            Property(x => x.ContragentName, "Контрагент (в отношении)").Column("CONTRAGENT_NAME");
            Property(x => x.DocumentNumber, "Номер обращения").Column("DOCUMENT_NUMBER");
            Property(x => x.RealObjAddresses, "Адреса домов").Column("RO_ADR");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
