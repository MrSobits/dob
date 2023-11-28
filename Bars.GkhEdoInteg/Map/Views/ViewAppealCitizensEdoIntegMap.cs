/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map.Views
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class ViewAppealCitizensEdoIntegMap : PersistentObjectMap<ViewAppealCitizensEdoInteg>
///     {
///         public ViewAppealCitizensEdoIntegMap()
///             : base("VIEW_GJI_APPEAL_CITS_EDO")
///         {
///             Map(x => x.ExecuteDate, "EXECUTE_DATE");
///             References(x => x.SuretyResolve, "SURETY_RESOLVE_ID").Fetch.Join();
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Tester, "TESTER_ID").Fetch.Join();
///             References(x => x.ZonalInspection, "ZONAINSP_ID").Fetch.Join();
/// 
///             Map(x => x.Municipality, "MUNICIPALITY");
///             Map(x => x.MunicipalityId, "MUNICIPALITY_ID");
///             Map(x => x.Number, "DOCUMENT_NUMBER");
///             Map(x => x.NumberGji, "GJI_NUMBER");
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.CheckTime, "CHECK_TIME");
///            
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.IsEdo, "IS_EDO");
///             Map(x => x.Correspondent, "CORRESPONDENT");
///             Map(x => x.RealObjAddresses, "RO_ADR");
///             Map(x => x.AddressEdo, "ADDRESS_EDO");
///             Map(x => x.CountSubject, "COUNT_SUBJECT");
/// 
///             //Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.QuestionsCount, "QUESTIONS_COUNT");
///             Map(x => x.CountRealtyObj, "COUNT_RO");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///             References(x => x.AppealCits, "ID").Not.Nullable().LazyLoad();
///             
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.ViewAppealCitizensEdoInteg"</summary>
    public class ViewAppealCitizensEdoIntegMap : PersistentObjectMap<ViewAppealCitizensEdoInteg>
    {
        
        public ViewAppealCitizensEdoIntegMap() : 
                base("Bars.GkhEdoInteg.Entities.ViewAppealCitizensEdoInteg", "VIEW_GJI_APPEAL_CITS_EDO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExecuteDate, "ExecuteDate").Column("EXECUTE_DATE");
            Property(x => x.Municipality, "Municipality").Column("MUNICIPALITY");
            Property(x => x.MunicipalityId, "MunicipalityId").Column("MUNICIPALITY_ID");
            Property(x => x.Number, "Number").Column("DOCUMENT_NUMBER");
            Property(x => x.NumberGji, "NumberGji").Column("GJI_NUMBER");
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.CheckTime, "CheckTime").Column("CHECK_TIME");
            Property(x => x.ContragentName, "ContragentName").Column("CONTRAGENT_NAME");
            Property(x => x.IsEdo, "IsEdo").Column("IS_EDO");
            Property(x => x.Correspondent, "Correspondent").Column("CORRESPONDENT");
            Property(x => x.RealObjAddresses, "RealObjAddresses").Column("RO_ADR");
            Property(x => x.AddressEdo, "AddressEdo").Column("ADDRESS_EDO");
            Property(x => x.CountSubject, "CountSubject").Column("COUNT_SUBJECT");
            Property(x => x.QuestionsCount, "QuestionsCount").Column("QUESTIONS_COUNT");
            Property(x => x.CountRealtyObj, "CountRealtyObj").Column("COUNT_RO");
            Reference(x => x.SuretyResolve, "SuretyResolve").Column("SURETY_RESOLVE_ID").Fetch();
            Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Tester, "Tester").Column("TESTER_ID").Fetch();
            Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAINSP_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            Reference(x => x.AppealCits, "AppealCits").Column("ID").NotNull();
        }
    }
}
