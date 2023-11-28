/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhCr.Entities;
/// 	using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Вьюха на сметный расчет объекта КР"
///     /// </summary>
///     public class ViewObjectCrEstimateCalcDetailMap : PersistentObjectMap<ViewObjCrEstimateCalcDetail>
///     {
///         public ViewObjectCrEstimateCalcDetailMap()
///             : base("VIEW_CR_OBJ_EST_CALC_DET")
///         {
///             Map(x => x.ObjectCrId, "OBJECT_ID");
///             Map(x => x.WorkName, "WORK_NAME");
///             Map(x => x.FinSourceName, "FIN_SOURCE_NAME");
///             Map(x => x.SumEstimate, "SUM_ESTIMATE");
///             Map(x => x.SumResource, "SUM_RESOURCE");
///             Map(x => x.TotalEstimate, "TOTAL_ESTIMATE");
///             Map(x => x.EstimationType, "ESTIMATION_TYPE").CustomType<EstimationType>();
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на сметный расчет объекта КР"</summary>
    public class ViewObjCrEstimateCalcDetailMap : PersistentObjectMap<ViewObjCrEstimateCalcDetail>
    {
        
        public ViewObjCrEstimateCalcDetailMap() : 
                base("Вьюха на сметный расчет объекта КР", "VIEW_CR_OBJ_EST_CALC_DET")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ObjectCrId, "Объект кап ремонта").Column("OBJECT_ID");
            Property(x => x.WorkName, "Наименование работы").Column("WORK_NAME");
            Property(x => x.FinSourceName, "Наименование источника финансировния").Column("FIN_SOURCE_NAME");
            Property(x => x.SumEstimate, "Сумма по сметам").Column("SUM_ESTIMATE");
            Property(x => x.SumResource, "Сумма по ведомостям ресурсов").Column("SUM_RESOURCE");
            Property(x => x.TotalEstimate, "Итоги по смете").Column("TOTAL_ESTIMATE");
            Property(x => x.EstimationType, "Тип сметы").Column("ESTIMATION_TYPE");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
