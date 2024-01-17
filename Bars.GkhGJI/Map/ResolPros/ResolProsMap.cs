namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;


    /// <summary>Маппинг для "Постановление прокуратуры"</summary>
    public class ResolProsMap : JoinedSubClassMap<ResolPros>
    {
        
        public ResolProsMap() : 
                base("Постановление прокуратуры", "GJI_RESOLPROS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.UIN, "УИН").Column("UIN");
            Property(x => x.DateSupply, "Дата поступления в ГЖИ").Column("DATE_SUPPLY");
            Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.ProsecutorOffice, "Орган прокуратуры").Column("PROS_ID").Fetch();
            Reference(x => x.Municipality, "Муниципальное образование (Орган прокуратуры, вынесший постановление)").Column("MUNICIPALITY_ID").Fetch();

            //Данные по нарушителю
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.PhysicalPersonPosition, "Должность").Column("PP_POSITION");
            Property(x => x.PhysicalPersonDocumentNumber, "Номер документа").Column("PHYSICALPERSON_DOC_NUM").Length(500);
            Property(x => x.PhysicalPersonIsNotRF, "Гражданство").Column("PHYSICALPERSON_NOT_CITIZENSHIP").DefaultValue(false);
            Property(x => x.PhysicalPersonDocumentSerial, "Серия документа").Column("PHYSICALPERSON_DOC_SERIAL").Length(500);
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();
            Property(x => x.BirthPlace, "Место рождения физ лица").Column("BIRTH_PLACE");
            Property(x => x.Job, "Место работы").Column("JOB");
            Property(x => x.DateBirth, "Дата рождения").Column("DATE_BIRTH");
            Property(x => x.PassportIssued, "Паспорт выдан").Column("PASSPORT_ISSUED");
            Property(x => x.DepartmentCode, "Код подразделения").Column("DEPARTMENT_CODE");
            Property(x => x.DateIssue, "Дата выдачи паспорта физ лицу ").Column("DATE_ISSUE");
            Property(x => x.FamilyStatus, "Семейное положение").Column("FAMILY_STATUS");
            Property(x => x.DependentsNumber, "Количество иждевенцев").Column("DEPENDENTS_NUMBER");
            Reference(x => x.SocialStatus, "Социальный статус").Column("SOCIAL_STATE");
            Property(x => x.IsPlaceResidenceOutState, "Место регистрации физ лица").Column("IS_PLACE_RESIDENCE_OUTSTATE");
            Property(x => x.IsActuallyResidenceOutState, "Место фактического жительства физ лица").Column("IS_ACTUALLY_RESIDENCE_OUTSTATE");
            Property(x => x.PhoneNumber, "Контактный телефон физ лица").Column("PHONE_NUMBER");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.FiasRegistrationAddress, "Место выдачи документа").Column("FIAS_REG_ADDRESS").Fetch();
            Reference(x => x.FiasFactAddress, "Место выдачи документа").Column("FIAS_FACT_ADDRESS").Fetch();
        }
    }
}
