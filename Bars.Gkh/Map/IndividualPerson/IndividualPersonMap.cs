namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.IndividualPerson"</summary>
    public class IndividualPersonMap : BaseEntityMap<IndividualPerson>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public IndividualPersonMap()
            :
                base("Физическое лицо ", "GKH_INDIVIDUAL_PERSON")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Fio, "ФИО физ лица ").Column("NAME").NotNull();
            this.Property(x => x.PlaceResidence, "Место регистрации физ лица").Column("PLACE_RESIDENCE");
            this.Property(x => x.ActuallyResidence, "Место фактического жительства физ лица").Column("ACTUALLY_RESIDENCE");
            this.Property(x => x.BirthPlace, "Место рождения физ лица").Column("BIRTH_PLACE");
            this.Property(x => x.Job, "Место работы").Column("JOB");
            this.Property(x => x.DateBirth, "Дата рождения").Column("DATE_BIRTH");
            this.Property(x => x.PassportNumber, "Номер паспорта").Column("PASSPORT_NUMBER");
            this.Property(x => x.PassportSeries, "Серия паспорта").Column("PASSPORT_SERIES");
            this.Property(x => x.PassportIssued, "Паспорт выдан").Column("PASSPORT_ISSUED");
            this.Property(x => x.DepartmentCode, "Код подразделения").Column("DEPARTMENT_CODE");
            this.Property(x => x.DateIssue, "Дата выдачи паспорта физ лицу ").Column("DATE_ISSUE");
            this.Property(x => x.INN, "ИНН").Column("INN");
            this.Property(x => x.FamilyStatus, "Семейное положение").Column("FAMILY_STATUS");
            this.Reference(x => x.FiasRegistrationAddress, "Место выдачи документа").Column("FIAS_REG_ADDRESS").Fetch();
            this.Reference(x => x.FiasFactAddress, "Место выдачи документа").Column("FIAS_FACT_ADDRESS").Fetch();
            this.Property(x => x.DependentsNumber, "Количество иждевенцев").Column("DEPENDENTS_NUMBER");
            this.Reference(x => x.SocialStatus, "Социальный статус").Column("SOCIAL_STATE");
            this.Property(x => x.PlaceResidenceOutState, "Место регистрации физ лица").Column("PLACE_RESIDENCE_OUTSTATE");
            this.Property(x => x.ActuallyResidenceOutState, "Место фактического жительства физ лица").Column("ACTUALLY_RESIDENCE_OUTSTATE");
            this.Property(x => x.IsPlaceResidenceOutState, "Место регистрации физ лица").Column("IS_PLACE_RESIDENCE_OUTSTATE");
            this.Property(x => x.IsActuallyResidenceOutState, "Место фактического жительства физ лица").Column("IS_ACTUALLY_RESIDENCE_OUTSTATE");
            this.Property(x => x.PhoneNumber, "Контактный телефон физ лица").Column("PHONE_NUMBER");
        }
    }
}