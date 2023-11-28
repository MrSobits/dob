
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class InspectorMap : BaseImportableEntityMap<Inspector>
    {
        
        public InspectorMap() : 
                base("Инспектор", "GKH_DICT_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Position, "Должность").Column("POSITION").Length(300);
            Property(x => x.Fio, "ФИО").Column("FIO").Length(300).NotNull();
            Property(x => x.ShortFio, "Фамилия И.О.").Column("SHORTFIO").Length(100);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.IsHead, "Начальник").Column("IS_HEAD").NotNull();
            Property(x => x.Active, "Работает").Column("IS_ACTIVE").NotNull();
            Property(x => x.FioGenitive, "ФИО Родительный падеж").Column("FIO_GENITIVE").Length(300);
            Property(x => x.FioDative, "ФИО Дательный падеж").Column("FIO_DATIVE").Length(300);
            Property(x => x.FioAccusative, "ФИО Винительный падеж").Column("FIO_ACCUSATIVE").Length(300);
            Property(x => x.FioAblative, "ФИО Творительный падеж").Column("FIO_ABLATIVE").Length(300);
            Property(x => x.FioPrepositional, "ФИО Предложный падеж").Column("FIO_PREPOSITIONAL").Length(300);
            Property(x => x.PositionGenitive, "Должность Родительный падеж").Column("POSITION_GENITIVE").Length(300);
            Property(x => x.PositionDative, "Должность Дательный падеж").Column("POSITION_DATIVE").Length(300);
            Property(x => x.PositionAccusative, "Должность Винительный падеж").Column("POSITION_ACCUSATIVE").Length(300);
            Property(x => x.PositionAblative, "Должность Творительный падеж").Column("POSITION_ABLATIVE").Length(300);
            Property(x => x.PositionPrepositional, "Должность Предложный падеж").Column("POSITION_PREPOSITIONAL").Length(300);
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(300);
            Property(x => x.Email, "Электронный адрес").Column("EMAIL").Length(100);
            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.TypeCommissionMember, "TypeCommissionMember").Column("TYPE_COMMISSION_MEMBER");
            Reference(x => x.Subdivision, "Подразделение").Column("SUBDIVISION_ID").Fetch();
            Reference(x => x.NotMemberPosition, "Подразделение").Column("POSITION_ID").Fetch();
            Reference(x => x.ZonalInspection, "Административная комиссия").Column("ZON_INSP_ID").Fetch();
        }
    }
}
