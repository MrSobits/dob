namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Views
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views;

    public class ViewProtocol197Map : PersistentObjectMap<ViewProtocol197>
    {
		public ViewProtocol197Map() :
			base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ViewProtocol197", "VIEW_GJI_PROTOCOL197")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.CountViolation, "Количество нарушений").Column("COUNT_VIOL");
            this.Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            this.Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            this.Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            this.Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            this.Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            this.Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            this.Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            this.Property(x => x.ContragentName, "Контрагент (исполнитель)").Column("CONTRAGENT_NAME");
            Property(x => x.PhysicalPerson, "ФИО физлица").Column("PHYSICAL_PERSON");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNum, "Целая часть номер документа").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentNumber, "номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            this.Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            this.Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            this.Property(x => x.TypeExecutant, "Тип исполнителя").Column("TYPE_EXEC_NAME");
            this.Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            this.Property(x => x.ArticleLaw, "Статьи закона").Column("ART_LAW");
            this.Property(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
            Property(x => x.ZonalInspectionId, "ZonalInspectionId").Column("ZONAL_ID");
            this.Property(x => x.Fio, "ФИО физ лица ").Column("FIO");
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
            this.Property(x => x.TypeViolator, "Тип нарушителя").Column("TYPE_VIOLATOR");
            Property(x => x.Penalty, "Сумма штрафа").Column("PENALTY");
            Property(x => x.DateViolation, "Дата нарушения").Column("DATE_OF_VIOLATION");
            Property(x => x.InspectorPosition, "Должность инспектора").Column("INSPECTOR_POSITION");
            Property(x => x.PhoneNumber, "Контактный телефон физ лица").Column("PHONE_NUMBER");
        }
    }
}