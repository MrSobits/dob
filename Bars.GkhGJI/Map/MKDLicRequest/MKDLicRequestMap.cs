using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Обращение граждан"</summary>
    public class MKDLicRequestMap : BaseEntityMap<MKDLicRequest>
    {
        
        public MKDLicRequestMap() : 
                base("Заявка на внесение изменений в реестр лицензий", "GJI_MKD_LIC_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент заявитель").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.ExecutantDocGji, "Тип заявителя").Column("EXECUTANT_TYPE_ID").NotNull().Fetch();
            this.Reference(x => x.Inspector, "Исполнитель").Column("INSPECTOR_ID").Fetch();
            this.Reference(x => x.MKDLicTypeRequest, "Тип запроса").Column("TYPE_REQUEST_ID").NotNull().Fetch();
            this.Reference(x => x.StatmentContragent, "Контрагент в заявлении").Column("STMT_CONTRAGENT_ID").Fetch();

            this.Property(x => x.ConclusionDate, "Дата заключения").Column("CONCLUSION_DATE");
            this.Property(x => x.ConclusionNumber, "Номер заключения").Column("CONCLUSION_NUMBER");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            this.Property(x => x.LicStatementResult, "Результат рассмотрения").Column("STATEMENT_RESULT");
            this.Property(x => x.LicStatementResultComment, "Комментарий к результату").Column("RESULT_COMMENT").Length(1000);
            this.Property(x => x.Objection, "Поступило возражение").Column("OBJECTION");
            this.Property(x => x.ObjectionResult, "Результат обжалования").Column("OBJECTION_RESULT");
            this.Property(x => x.PhysicalPerson, "ФИО заявителя").Column("FIO").Length(300);
            this.Property(x => x.StatementDate, "Дата заявления").Column("STATEMENT_DATE").NotNull();
            this.Property(x => x.StatementNumber, "Номер заявления").Column("STATEMENT_NUMBER").Length(255);          
        }
    }
}
