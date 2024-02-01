

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Зональная Административная комиссия"</summary>
    public class ZonalInspectionMap : BaseImportableEntityMap<ZonalInspection>
    {
        
        public ZonalInspectionMap() : 
                base("Зональная Административная комиссия", "GKH_DICT_ZONAINSP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.ZoneName, "Зональное наименование").Column("ZONE_NAME").Length(300);
            Property(x => x.BlankName, "Наименование для бланка").Column("BLANK_NAME").Length(300);
            Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(300);
            Property(x => x.Address, "Адрес").Column("ADDRESS").Length(300);
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(50);
            Property(x => x.Email, "E-mail").Column("EMAIL").Length(50);
            Property(x => x.NameSecond, "Наименование (2 гос.язык)").Column("NAME_SECOND").Length(300);
            Property(x => x.ZoneNameSecond, "Зональное наименование (2 гос.язык)").Column("ZONE_NAME_SECOND").Length(300);
            Property(x => x.BlankNameSecond, "Наименование для бланка (2 гос.язык)").Column("BLANK_NAME_SECOND").Length(300);
            Property(x => x.ShortNameSecond, "Краткое наименование (2 гос.язык)").Column("SHORT_NAME_SECOND").Length(300);
            Property(x => x.AddressSecond, "Адрес (2 гос.язык)").Column("ADDRESS_SECOND").Length(300);
            Property(x => x.Okato, "ОКАТО - Общероссийский классификатор объектов административно-территориального де" +
                    "ления. Пример: ОКАТО Татарстана 92000000000.").Column("OKATO").Length(30);
            Property(x => x.NameGenetive, "Наименование Родительный падеж").Column("NAME_GENETIVE").Length(300);
            Property(x => x.NameDative, "Наименование Дательный падеж").Column("NAME_DATIVE").Length(300);
            Property(x => x.NameAccusative, "Наименование Винительный падеж").Column("NAME_ACCUSATIVE").Length(300);
            Property(x => x.NameAblative, "Наименование Творительный падеж").Column("NAME_ABLATIVE").Length(300);
            Property(x => x.NamePrepositional, "Наименование Предложный падеж").Column("NAME_PREPOSITIONAL").Length(300);
            Property(x => x.ShortNameGenetive, "Краткое наименование Родительный падеж").Column("SHORT_NAME_GENETIVE").Length(300);
            Property(x => x.ShortNameDative, "Краткое наименование Дательный падеж").Column("SHORT_NAME_DATIVE").Length(300);
            Property(x => x.ShortNameAccusative, "Краткое наименование Винительный падеж").Column("SHORT_NAME_ACCUSATIVE").Length(300);
            Property(x => x.ShortNameAblative, "Краткое наименование Творительный падеж").Column("SHORT_NAME_ABLATIVE").Length(300);
            Property(x => x.ShortNamePrepositional, "Краткое наименование Предложный падеж").Column("SHORT_NAME_PREPOSITIONAL").Length(300);
            Property(x => x.IndexOfGji, "Индекс отдела гжи").Column("INDEX_OF_GJI").Length(100);
            Property(x => x.AppealCode, "Код нумерации обращения").Column("APPEAL_CODE");
            Property(x => x.Oktmo, "ОКТМО").Column("OKTMO").Length(30);
            Property(x => x.Locality, "Населенный пункт").Column("LOCALITY");
           // Property(x => x.IsNotGZHI, "Не является ГЖИ").Column("IS_NOT_GZHI").NotNull();
            Property(x => x.DepartmentCode, "Код отдела - используется при формировании документа ГЖИ").Column("DEPARTMENT_CODE");
            Property(x => x.bik, "БИК").Column("BIK");
            Property(x => x.inn, "ИНН").Column("INN");
            Property(x => x.kbk, "КБК").Column("KBK");
            Property(x => x.kpp, "КПП").Column("KPP");
            Property(x => x.uin, "УИН").Column("UIN");
            Property(x => x.ogrn, "ОГРН").Column("OGRN");
            Property(x => x.UFC, "UFC").Column("UFC");
            Property(x => x.PersonalAcc, "PersonalAcc").Column("PERSACC");
            Property(x => x.BankName, "BankName").Column("BANKNAME");
            Property(x => x.CorrespAcc, "CorrespAcc").Column("CORRACC");
            Property(x => x.GisGmpId, "GisGmpId").Column("GISGMPID");
            Property(x => x.UseUFC, "Использовать УФК в ГИС ГМП вместо названия комиссии").Column("USE_UFC");
        }
    }
}
