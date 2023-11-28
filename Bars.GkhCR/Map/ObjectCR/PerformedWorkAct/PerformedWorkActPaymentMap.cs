/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Оплата в актах выполненных работ"
///     /// </summary>
///     public class PerformedWorkActPaymentMap : BaseImportableEntityMap<PerformedWorkActPayment>
///     {
///         public PerformedWorkActPaymentMap()
///             : base("CR_OBJ_PER_ACT_PAYMENT")
///         {
///             Map(x => x.DateDisposal, "DATE_DISPOSAL");
///             Map(x => x.DatePayment, "DATE_PAYMENT");
///             Map(x => x.Sum, "SUM", true);
///             Map(x => x.Paid, "SUM_PAID", true);
///             Map(x => x.Percent, "PERCENT", true);
///             Map(x => x.TransferGuid, "TRANSFER_GUID");
///             Map(x => x.TypeActPayment, "TYPE_ACT_PAYMENT", true);
/// 
///             References(x => x.PerformedWorkAct, "ACT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Распоряжение к оплате акта."</summary>
    public class PerformedWorkActPaymentMap : BaseImportableEntityMap<PerformedWorkActPayment>
    {
        
        public PerformedWorkActPaymentMap() : 
                base("Распоряжение к оплате акта.", "CR_OBJ_PER_ACT_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PerformedWorkAct, "Акт выполненных работ, по которому было создано данное распоряжение").Column("ACT_ID").Fetch();
            Property(x => x.DateDisposal, "Дата распоряжения").Column("DATE_DISPOSAL");
            Property(x => x.DatePayment, "Дата оплаты").Column("DATE_PAYMENT");
            Property(x => x.TypeActPayment, "Вид оплаты").Column("TYPE_ACT_PAYMENT").NotNull();
            Reference(x => x.Document, "Документ").Column("DOCUMENT_ID").Fetch();
            Property(x => x.Sum, "Сумма к оплате, руб.").Column("SUM").NotNull();
            Property(x => x.Percent, "Percent").Column("PERCENT").NotNull();
            Property(x => x.Paid, "Сумма оплачено, руб").Column("SUM_PAID").NotNull();
            Property(x => x.TransferGuid, "Guid трансфера денег для оплаты акта").Column("TRANSFER_GUID").Length(250);
        }
    }
}
