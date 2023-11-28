/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.CommonEstateObject
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.CommonEstateObject;
/// 
///     public class CommonEstateObjectMap : BaseImportableEntityMap<CommonEstateObject>
///     {
///         public CommonEstateObjectMap()
///             : base("OVRHL_COMMON_ESTATE_OBJECT")
///         {
///             Map(x => x.Code, "CEO_CODE", true, 200);
///             References(x => x.GroupType, "CEO_GROUP_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Name, "NAME", true, 500);
///             Map(x => x.ShortName, "SHORT_NAME", true, 300);
///             Map(x => x.IsMatchHc, "IS_MATCH_HC", true, false);
///             Map(x => x.IncludedInSubjectProgramm, "INC_IN_SUBJ_PRG", true, false);
///             Map(x => x.IsEngineeringNetwork, "IS_ENG_NETWORK", true, false);
///             Map(x => x.MultipleObject, "MULT_OBJECT", true, false);
///             Map(x => x.Weight, "WEIGHT", true, 0);
///             Map(x => x.IsMain, "IS_MAIN", true, false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.CommonEstateObject
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.CommonEstateObject;

    /// <summary>Маппинг для "Объект общего имущества"</summary>
    public class CommonEstateObjectMap : BaseImportableEntityMap<CommonEstateObject>
    {

        /// <summary>
        /// .ctor
        /// </summary>
        public CommonEstateObjectMap() :
                base("Объект общего имущества", "OVRHL_COMMON_ESTATE_OBJECT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.GroupType, "Тип группы").Column("CEO_GROUP_TYPE_ID").NotNull().Fetch();
            this.Property(x => x.Code, "Код").Column("CEO_CODE").Length(200).NotNull();
            this.Property(x => x.ReformCode, "Код реформы").Column("REFORM_CODE").Length(10);
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            this.Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(300).NotNull();
            this.Property(x => x.IsMatchHc, "Флаг: Соответствует ЖК РФ").Column("IS_MATCH_HC").DefaultValue(false).NotNull();
            this.Property(x => x.IncludedInSubjectProgramm, "Флаг: Включен в программу субъекта").Column("INC_IN_SUBJ_PRG").DefaultValue(false).NotNull();
            this.Property(x => x.IsEngineeringNetwork, "Флаг: Является инженерной сетью").Column("IS_ENG_NETWORK").DefaultValue(false).NotNull();
            this.Property(x => x.MultipleObject, "Множественный объект").Column("MULT_OBJECT").DefaultValue(false).NotNull();
            this.Property(x => x.Weight, "Вес").Column("WEIGHT").NotNull();
            this.Property(x => x.IsMain, "Является основным").Column("IS_MAIN").DefaultValue(false).NotNull();
        }
    }
}
