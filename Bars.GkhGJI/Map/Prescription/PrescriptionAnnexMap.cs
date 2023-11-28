/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложения предписания ГЖИ"
///     /// </summary>
///     public class PrescriptionAnnexMap : BaseGkhEntityMap<PrescriptionAnnex>
///     {
///         public PrescriptionAnnexMap() : base("GJI_PRESCRIPTION_ANNEX")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
/// 
///             References(x => x.Prescription, "PRESCRIPTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения предписания ГЖИ"</summary>
    public class PrescriptionAnnexMap : BaseEntityMap<PrescriptionAnnex>
    {
        
        public PrescriptionAnnexMap() : 
                base("Приложения предписания ГЖИ", "GJI_PRESCRIPTION_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.TypePrescriptionAnnex, "Тип приложения").Column("ANNEX_TYPE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Number, "Номер документа").Column("NUMBER").Length(50);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
            Reference(x => x.Prescription, "Предписание").Column("PRESCRIPTION_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.TypeAnnex, "TypeAnnex").Column("TYPE_ANNEX").NotNull();
        }
    }
}
