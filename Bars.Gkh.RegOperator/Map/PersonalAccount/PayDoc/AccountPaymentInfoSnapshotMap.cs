/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount.PayDoc;
/// 
///     internal class AccountPaymentInfoSnapshotMap : BaseEntityMap<AccountPaymentInfoSnapshot>
///     {
///         public AccountPaymentInfoSnapshotMap() : base("regop_pers_paydoc_snap")
///         {
///             Map(x => x.AccountId, "account_id");
///             Map(x => x.AccountNumber, "acc_num");
///             Map(x => x.ChargeSum, "charge_sum");
///             Map(x => x.BaseTariffSum, "base_tariff_sum", true);
///             Map(x => x.DecisionTariffSum, "dec_tariff_sum", true);
///             Map(x => x.PenaltySum, "penalty_sum", true);
///             Map(x => x.Data, "raw_data");
///             Map(x => x.RoomAddress, "room_address");
///             Map(x => x.RoomType, "room_type");
///             Map(x => x.Services, "services");
///             Map(x => x.Tariff, "tariff");
///             Map(x => x.Area, "area");
///             
///             References(x => x.Snapshot, "snapshot_id");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    
    
    /// <summary>Маппинг для "Данные для документа на оплату по ЛС"</summary>
    public class AccountPaymentInfoSnapshotMap : BaseEntityMap<AccountPaymentInfoSnapshot>
    {
        
        public AccountPaymentInfoSnapshotMap() : 
                base("Данные для документа на оплату по ЛС", "REGOP_PERS_PAYDOC_SNAP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Snapshot, "Основная информация по документу").Column("SNAPSHOT_ID");
            Property(x => x.AccountId, "Id ЛС").Column("ACCOUNT_ID");
            Property(x => x.Data, "Данные для документа").Column("RAW_DATA").Length(250);
            Property(x => x.AccountNumber, "Номер ЛС").Column("ACC_NUM").Length(250);
            Property(x => x.RoomAddress, "Адрес до уровня квартиры. Т.е. Казань, Гаврилова 13, кв. 5").Column("ROOM_ADDRESS").Length(250);
            Property(x => x.RoomType, "Тип комнаты").Column("ROOM_TYPE");
            Property(x => x.Area, "Площадь помещения").Column("AREA");
            Property(x => x.Tariff, "Тариф").Column("TARIFF");
            Property(x => x.ChargeSum, "Сумма начисления Складывается из BaseTariffSum, DecisionTariffSum, PenaltySum").Column("CHARGE_SUM");
            Property(x => x.BaseTariffSum, "Служебное- К оплате по базовому тарифу").Column("BASE_TARIFF_SUM").NotNull();
            Property(x => x.DecisionTariffSum, "Служебное- К оплате по тарифу решения").Column("DEC_TARIFF_SUM").NotNull();
            Property(x => x.PenaltySum, "Служебное- Пени к оплате").Column("PENALTY_SUM").NotNull();
            Property(x => x.Services, "Оказанные услуги (в нашем случае пока только кап ремонт)").Column("SERVICES").Length(250);
        }
    }
}
