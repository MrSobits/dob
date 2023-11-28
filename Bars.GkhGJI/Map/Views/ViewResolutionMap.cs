/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewResolutionMap : PersistentObjectMap<ViewResolution>
///     {
///         public ViewResolutionMap() : base("VIEW_GJI_RESOLUTION")
///         {
///             Map(x => x.SumPays, "SUM_PAYS");
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.OfficialName, "OFFICIAL_NAME");
///             Map(x => x.OfficialId, "OFFICIAL_ID");
///             Map(x => x.PenaltyAmount, "PENALTY_AMOUNT");
///             Map(x => x.InspectionId, "INSPECTION_ID");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///             Map(x => x.TypeDocumentGji, "TYPE_DOC").CustomType<TypeDocumentGji>();
///             Map(x => x.TypeExecutant, "TYPE_EXEC_NAME");
///             Map(x => x.Sanction, "SANCTION_NAME");
///             Map(x => x.ContragentMuName, "CTR_MU_NAME");
///             Map(x => x.ContragentMuId, "CTR_MU_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.ResolutionGjiId, "DOCUMENT_ID");
///             Map(x => x.DeliveryDate, "DELIVERY_DATE");
///             Map(x => x.Paided, "PAIDED");
///             Map(x => x.BecameLegal, "BECAME_LEGAL");
///             Map(x => x.RoAddress, "RO_ADDRESS");
/// 
///             References(x => x.State, "STATE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewResolution"</summary>
    public class ViewResolutionMap : PersistentObjectMap<ViewResolution>
    {
        
        public ViewResolutionMap() : 
                base("Bars.GkhGji.Entities.ViewResolution", "VIEW_GJI_RESOLUTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SumPays, "Сумма оплат штрафов").Column("SUM_PAYS");
            Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.OfficialName, "ФИО ДЛ вынесшего постановление").Column("OFFICIAL_NAME");
            Property(x => x.OfficialPosition, "Должность ДЛ вынесшего постановление").Column("OFFICIAL_POSITION");
            Property(x => x.OfficialId, "Идентификатор ДЛ вынесшего постановление").Column("OFFICIAL_ID");
            Property(x => x.PenaltyAmount, "Сумма штрафа").Column("PENALTY_AMOUNT");
            Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            Property(x => x.TypeInitiativeOrg, "Тип инициативного органа").Column("TYPE_INITIATIVE_ORG");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.TypeExecutant, "Тип исполнителя").Column("TYPE_EXEC_NAME");
            Property(x => x.Sanction, "Санкция").Column("SANCTION_NAME");
            Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            Property(x => x.ContragentName, "Контрагент (исполнитель)").Column("CONTRAGENT_NAME");
            Property(x => x.PhysicalPerson, "ФИО физлица").Column("PHYSICAL_PERSON");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номера документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.ResolutionGjiId, "Постановление").Column("DOCUMENT_ID");
            Property(x => x.DeliveryDate, "Дата вручения").Column("DELIVERY_DATE");
            Property(x => x.Paided, "Штраф оплачен").Column("PAIDED");
            Property(x => x.BecameLegal, "Вступило в законную силу").Column("BECAME_LEGAL");
            Property(x => x.RoAddress, "Адреса по нарушениям").Column("RO_ADDRESS");
            Property(x => x.ControlType, "Тип Контроля надзора").Column("CONTROL_TYPE");
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
            Property(x => x.InLawDate, "Дата вступления в силу").Column("INLAW_DATE");
            Property(x => x.DueDate, "Срок оплаты").Column("DUE_DATE");
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            Property(x => x.Protocol205Date, "Дата неоплаты").Column("notpaydate");
            Property(x => x.ArticleLaw, "Статьи закона").Column("ART_LAW");
            Property(x => x.ZonalInspectionId, "ZonalInspectionId").Column("ZONAL_ID");
            Property(x => x.ConcederationResult, "Результат рассмотрения").Column("CONSIDERATION_RESULT");
            Property(x => x.SentToOSP, "Направлено приставам").Column("SENT_TO_OSP");
        }
    }
}
