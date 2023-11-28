/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class RealityObjectChargeAccountMap : BaseImportableEntityMap<RealityObjectChargeAccount>
///     {
///         public RealityObjectChargeAccountMap() : base("REGOP_RO_CHARGE_ACCOUNT")
///         {
///             Map(x => x.AccountNumber, "ACC_NUM", false);
///             Map(x => x.ChargeTotal, "CHARGE_TOTAL");
///             Map(x => x.PaidTotal, "PAID_TOTAL");
///             
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             Bag(x => x.Operations, mapper =>
///             {
///                 mapper.Access(Accessor.NoSetter);
///                 mapper.Fetch(CollectionFetchMode.Select);
///                 mapper.Lazy(CollectionLazy.Extra);
///                 mapper.Key(k => k.Column("ACC_ID"));
///                 mapper.Cascade(Cascade.DeleteOrphans);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Счет начислений дома"</summary>
    public class RealityObjectChargeAccountMap : BaseImportableEntityMap<RealityObjectChargeAccount>
    {
        
        public RealityObjectChargeAccountMap() : 
                base("Счет начислений дома", "REGOP_RO_CHARGE_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Объект недвижимости").Column("RO_ID").NotNull().Fetch();
            Property(x => x.AccountNumber, "Номер счета").Column("ACC_NUM").Length(250);
            Property(x => x.ChargeTotal, "Все начисления").Column("CHARGE_TOTAL");
            Property(x => x.PaidTotal, "Все оплаты").Column("PAID_TOTAL");
        }
    }

    public class RealityObjectChargeAccountNHibernateMapping : ClassMapping<RealityObjectChargeAccount>
    {
        public RealityObjectChargeAccountNHibernateMapping()
        {
            Bag(
                x => x.Operations,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Extra);
                        mapper.Key(k => k.Column("ACC_ID"));
                        mapper.Cascade(Cascade.DeleteOrphans);
                    },
                action => action.OneToMany());
        }
    }
}
