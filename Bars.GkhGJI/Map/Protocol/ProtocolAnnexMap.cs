/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложения протокола ГЖИ"
///     /// </summary>
///     public class ProtocolAnnexMap : BaseGkhEntityMap<ProtocolAnnex>
///     {
///         public ProtocolAnnexMap() : base("GJI_PROTOCOL_ANNEX")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
/// 
///             References(x => x.Protocol, "PROTOCOL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения протокола ГЖИ"</summary>
    public class ProtocolAnnexMap : BaseEntityMap<ProtocolAnnex>
    {
        
        public ProtocolAnnexMap() : 
                base("Приложения протокола ГЖИ", "GJI_PROTOCOL_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.IsProof, "Доказательство").Column("IS_PROOF");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.TypeAnnex, "TypeAnnex").Column("ANNEX_TYPE").NotNull();
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
        }
    }
}
