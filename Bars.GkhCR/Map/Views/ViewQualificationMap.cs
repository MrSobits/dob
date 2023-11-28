/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "реестр квалификационного отбора"
///     /// </summary>
///     public class ViewQualificationMap : PersistentObjectMap<ViewQualification>
///     {
///         public ViewQualificationMap()
///             : base("VIEW_CR_OBJECT_QUALIFICATION")
///         {
/// 
///             Map(x => x.ProgrammName, "PROGRAM");
///             Map(x => x.MunicipalityName, "MUNICIPALITY");
///             Map(x => x.BuilderName, "BUILDER");
///             Map(x => x.Sum, "TYPE_WORK_SUM");
///             Map(x => x.Rating, "RATING");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.QualMemberCount, "QUAL_MEMBER_COUNT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на реест квалификационного отбора"</summary>
    public class ViewQualificationMap : PersistentObjectMap<ViewQualification>
    {
        
        public ViewQualificationMap() : 
                base("Вьюха на реест квалификационного отбора", "VIEW_CR_OBJECT_QUALIFICATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ProgrammName, "Программа КР").Column("PROGRAM");
            Property(x => x.MunicipalityName, "Муниципальное образование").Column("MUNICIPALITY");
            Property(x => x.BuilderName, "Подрядчик").Column("BUILDER");
            Property(x => x.Sum, "Сумма").Column("TYPE_WORK_SUM");
            Property(x => x.Rating, "Рейтинг").Column("RATING");
            Property(x => x.Address, "Рейтинг").Column("ADDRESS");
            Property(x => x.QualMemberCount, "Кол-во участников квал отбора").Column("QUAL_MEMBER_COUNT");
        }
    }
}
