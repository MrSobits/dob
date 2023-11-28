/// <mapping-converter-backup>
/// using FluentNHibernate.Mapping;
/// 
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Распоряжение"
///     /// </summary>
///     public class ChelyabinskProtocolMap : SubclassMap<ChelyabinskProtocol>
///     {
///         public ChelyabinskProtocolMap()
///         {
///             Table("GJI_NSO_PROTOCOL");
///             KeyColumn("ID");
///             Map(x => x.FormatDate, "FORMAT_DATE");
///             Map(x => x.FormatPlace, "FORMAT_PLACE").Length(500);
///             Map(x => x.FormatHour, "FORMAT_HOUR");
///             Map(x => x.FormatMinute, "FORMAT_MINUTE");
///             Map(x => x.NotifNumber, "NOTIF_NUM").Length(100);
/// 
///             Map(x => x.ProceedingsPlace, "PROCEEDINGS_PLACE").Length(100);
///             Map(x => x.Remarks, "REMARKS").Length(100);
///             Map(x => x.PersonRegistrationAddress, "PERSON_REG_ADDRESS").Length(250);
///             Map(x => x.PersonFactAddress, "PERSON_FACT_ADDRESS").Length(250);
///             Map(x => x.PersonJob, "PERSON_JOB");
///             Map(x => x.PersonPosition, "PERSON_POSITION");
///             Map(x => x.PersonBirthDatePlace, "PERSON_BIRTHDATE").Length(250);
///             Map(x => x.PersonDoc, "PERSON_DOC");
///             Map(x => x.PersonSalary, "PERSON_SALARY");
///             Map(x => x.PersonRelationship, "PERSON_RELAT");
/// 
///             Map(x => x.TypePresence, "TYPE_PRESENCE");
///             Map(x => x.Representative, "REPRESENTATIVE").Length(500);
///             Map(x => x.ReasonTypeRequisites, "REASON_TYPE_REQ").Length(1000);
/// 
///             Map(x => x.NotifDeliveredThroughOffice, "DELIV_THROUGH_OFFICE");
///             Map(x => x.ProceedingCopyNum, "PROCEEDING_COPY_NUM");
/// 
///             Map(x => x.DateOfViolation, "DATE_OF_VIOLATION");
///             Map(x => x.HourOfViolation, "HOUR_OF_VIOLATION");
///             Map(x => x.MinuteOfViolation, "MINUTE_OF_VIOLATION");
///             References(x => x.ResolveViolationClaim, "RESOLVE_VIOL_CLAIM_ID");
///             References(x => x.NormativeDoc, "NORMATIVE_DOC_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskProtocol"</summary>
    public class ChelyabinskProtocolMap : JoinedSubClassMap<ChelyabinskProtocol>
    {
        
        public ChelyabinskProtocolMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskProtocol", "GJI_NSO_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.PersonRegistrationAddress, "PersonRegistrationAddress").Column("PERSON_REG_ADDRESS").Length(250);
            this.Property(x => x.PersonFactAddress, "PersonFactAddress").Column("PERSON_FACT_ADDRESS").Length(250);
            this.Property(x => x.PersonJob, "PersonJob").Column("PERSON_JOB");
            this.Property(x => x.PersonPosition, "PersonPosition").Column("PERSON_POSITION");
            this.Property(x => x.PersonBirthDatePlace, "PersonBirthDatePlace").Column("PERSON_BIRTHDATE").Length(250);
            this.Property(x => x.PersonDoc, "PersonDoc").Column("PERSON_DOC");
            this.Property(x => x.PersonSalary, "PersonSalary").Column("PERSON_SALARY");
            this.Property(x => x.PersonRelationship, "PersonRelationship").Column("PERSON_RELAT");
            this.Property(x => x.TypePresence, "TypePresence").Column("TYPE_PRESENCE");
            this.Property(x => x.Representative, "Representative").Column("REPRESENTATIVE").Length(500);
            this.Property(x => x.ReasonTypeRequisites, "ReasonTypeRequisites").Column("REASON_TYPE_REQ").Length(1000);
            this.Property(x => x.DateOfViolation, "DateOfViolation").Column("DATE_OF_VIOLATION");
            this.Property(x => x.HourOfViolation, "HourOfViolation").Column("HOUR_OF_VIOLATION");
            this.Property(x => x.MinuteOfViolation, "MinuteOfViolation").Column("MINUTE_OF_VIOLATION");
            this.Reference(x => x.ResolveViolationClaim, "ResolveViolationClaim").Column("RESOLVE_VIOL_CLAIM_ID");
            this.Reference(x => x.NormativeDoc, "NormativeDoc").Column("NORMATIVE_DOC_ID");
        }
    }
}
