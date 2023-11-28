/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности " Объект капитального ремонта"
///     /// </summary>
///     public class ViewObjCrEstimateCalcMap : PersistentObjectMap<ViewObjCrEstimateCalc>
///     {
///         public ViewObjCrEstimateCalcMap()
///             : base("VIEW_CR_OBJECT_EST_CALC")
///         {
///             Map(x => x.Id, "ID");
///             Map(x => x.RealityObjName, "ADDRESS");
///             Map(x => x.RealityObjectId, "REALITY_OBJECT_ID");
///             Map(x => x.ObjectCrId, "OBJECT_ID");
///             Map(x => x.MunicipalityId, "MUNICIPALITY_ID");
///             Map(x => x.Municipality, "MUNICIPALITY_NAME");
///             Map(x => x.SettlementId, "SETTLEMENT_ID");
///             Map(x => x.SettlementName, "SETTLEMENT_NAME");
///             Map(x => x.TypeWorkCrCount, "CNT_TW");
///             Map(x => x.EstimateCalculationsCount, "CNT_EST_CALC");
///             Map(x => x.ProgramCrId, "PROGRAM_ID");
///             Map(x => x.ProgramCrName, "PROGRAM_NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на Объект КР"</summary>
    public class ViewObjCrEstimateCalcMap : PersistentObjectMap<ViewObjCrEstimateCalc>
    {
        
        public ViewObjCrEstimateCalcMap() : 
                base("Вьюха на Объект КР", "VIEW_CR_OBJECT_EST_CALC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RealityObjName, "Жилой дом").Column("ADDRESS");
            Property(x => x.RealityObjectId, "Id жилого дома").Column("REALITY_OBJECT_ID");
            Property(x => x.ObjectCrId, "Объект КР").Column("OBJECT_ID");
            Property(x => x.MunicipalityId, "Муниципальное образование Id").Column("MUNICIPALITY_ID");
            Property(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_NAME");
            Property(x => x.SettlementId, "Муниципальный район Id").Column("SETTLEMENT_ID");
            Property(x => x.SettlementName, "Муниципальный район Название").Column("SETTLEMENT_NAME");
            Property(x => x.TypeWorkCrCount, "Количество Видов Работ по ОКР").Column("CNT_TW");
            Property(x => x.EstimateCalculationsCount, "Количество Сметных расчетов по ОКР").Column("CNT_EST_CALC");
            Property(x => x.ProgramCrId, "Программа КР").Column("PROGRAM_ID");
            Property(x => x.ProgramCrName, "Программа КР").Column("PROGRAM_NAME");
        }
    }
}
