/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// 
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Контрагент"
///     /// </summary>
///     public class PersonQualificationCertificateMap : BaseImportableEntityMap<PersonQualificationCertificate>
///     {
///         public PersonQualificationCertificateMap()
///             : base("GKH_PERSON_CERTIFICATE")
///         {
///             References(x => x.Person, "PERSON_ID").Not.Nullable().Fetch.Join();
/// 
///             Map(x => x.Number, "CERT_NUMBER").Length(100);
///             Map(x => x.BlankNumber, "BLANK_NUMBER").Length(100);
///             Map(x => x.IssuedDate, "ISSUED_DATE");
///             Map(x => x.EndDate, "END_DATE");
///             References(x => x.File, "FILE_ID").Fetch.Join();
/// 
///             Map(x => x.HasDuplicate, "HAS_DUPLICATE").Not.Nullable().Default("0");
///             Map(x => x.DuplicateNumber, "DUPLICATE_NUMBER").Length(100);
///             Map(x => x.DuplicateIssuedDate, "DUPLICATE_ISSUED_DATE");
///             References(x => x.DuplicateFile, "DUPLICATE_FILE_ID").Fetch.Join();
/// 
///             Map(x => x.HasCancelled, "HAS_CANCELLED").Not.Nullable().Default("0");
///             Map(x => x.TypeCancelation, "TYPE_CANCELATION").Not.Nullable().CustomType<TypeCancelationQualCertificate>();
///             Map(x => x.CancelationDate, "CANCEL_DATE");
///             Map(x => x.CancelNumber, "CANCEL_NUMBER").Length(100);
///             Map(x => x.CancelProtocolDate, "CANCEL_PROTOCOL_DATE");
///             References(x => x.CancelFile, "CANCEL_FILE_ID").Fetch.Join();
/// 
///             Map(x => x.HasRenewed, "HAS_RENEWED").Not.Nullable().Default("0");
///             Map(x => x.CourtName, "COURT_NAME").Length(1000);
///             Map(x => x.CourtActNumber, "COURT_ACT_NUMBER").Length(100);
///             Map(x => x.CourtActDate, "COURT_ACT_DATE");
///             References(x => x.ActFile, "ACT_FILE_ID").Fetch.Join();
/// 
///             References(x => x.RequestToExam, "REQUEST_EXAM_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using B4.Modules.Mapping.Mappers;
    using Entities;
    using System;
    
    
    /// <summary>Маппинг для "Квалификационный аттестат"</summary>
    public class PersonQualificationCertificateMap : BaseImportableEntityMap<PersonQualificationCertificate>
    {
        
        public PersonQualificationCertificateMap() : 
                base("Квалификационный аттестат", "GKH_PERSON_CERTIFICATE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Number, "Номер - якобы неиспользуется просто навсякий случай").Column("CERT_NUMBER").Length(100);
            this.Property(x => x.BlankNumber, "Номер бланка").Column("BLANK_NUMBER").Length(100);
            this.Property(x => x.IssuedDate, "Дата выдачи").Column("ISSUED_DATE");
            this.Property(x => x.EndDate, "Дата окончания действия").Column("END_DATE");
            this.Property(x => x.RecieveDate, "Дата получения").Column("RECIEVE_DATE");
            this.Property(x => x.HasDuplicate, "Выдан дубликат").Column("HAS_DUPLICATE").DefaultValue(false).NotNull();
            this.Property(x => x.DuplicateNumber, "Номер дубликата").Column("DUPLICATE_NUMBER").Length(100);
            this.Property(x => x.DuplicateIssuedDate, "Дата выдачи дубликата").Column("DUPLICATE_ISSUED_DATE");
            this.Property(x => x.HasCancelled, "Аттестат аннулирован").Column("HAS_CANCELLED").DefaultValue(false).NotNull();
            this.Property(x => x.TypeCancelation, "Основание отмены квалификационоого аттестата").Column("TYPE_CANCELATION").NotNull();
            this.Property(x => x.CancelationDate, "Дата анулирования").Column("CANCEL_DATE");
            this.Property(x => x.CancelNumber, "Номер протокола аннулирования").Column("CANCEL_NUMBER").Length(100);
            this.Property(x => x.CancelProtocolDate, "Дата протокола аннулирования").Column("CANCEL_PROTOCOL_DATE");
            this.Property(x => x.HasRenewed, "Аннулирование отменено").Column("HAS_RENEWED").DefaultValue(false).NotNull();
            this.Property(x => x.CourtName, "Наименование суда").Column("COURT_NAME").Length(1000);
            this.Property(x => x.CourtActNumber, "Номер судебного акта").Column("COURT_ACT_NUMBER").Length(100);
            this.Property(x => x.CourtActDate, "Дата судебного акта").Column("COURT_ACT_DATE");
            this.Property(x => x.ApplicationDate, "Дата судебного акта").Column("APPLICATION_DATE");
            this.Property(x => x.IsFromAnotherRegion, "Квалификационный аттестат получен в другом регионе").Column("IS_FROM_ANOTHER_REGION");
            this.Property(x => x.RegionCode, "Код региона где был выдан аттестат").Column("REGION_CODE").Length(2);
            this.Property(x => x.IssuedBy, "Кем выдан").Column("ISSUED_BY").Length(500);
            this.Reference(x => x.Person, "Person").Column("PERSON_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Reference(x => x.DuplicateFile, "Фаил дубликата").Column("DUPLICATE_FILE_ID").Fetch();
            this.Reference(x => x.CancelFile, "Протокол аннулирования").Column("CANCEL_FILE_ID").Fetch();
            this.Reference(x => x.ActFile, "Судебный акт").Column("ACT_FILE_ID").Fetch();
            this.Reference(x => x.RequestToExam, "Заявка на доступ к экзамену").Column("REQUEST_EXAM_ID").Fetch();
            this.Reference(x => x.FileNotificationOfExamResults, "Дата судебного акта").Column("FAIL_NOTIFICATION_OF_EXAM_RESULTS_ID").Fetch();
            this.Reference(x => x.FileIssueApplication, "Дата судебного акта").Column("FILE_ISSUE_APPLICATION_ID").Fetch();
        }
    }
}
