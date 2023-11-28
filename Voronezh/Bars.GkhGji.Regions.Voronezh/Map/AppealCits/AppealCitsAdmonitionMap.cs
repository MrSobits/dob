namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsAdmonitionMap : BaseEntityMap<AppealCitsAdmonition>
    {
        
        public AppealCitsAdmonitionMap() : 
                base("Обращениям граждан - Предостережение", "GJI_APPCIT_ADMONITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(30);
            Property(x => x.PerfomanceDate, "Дата исполнения").Column("PERFORMANCE_DATE");
            Property(x => x.PerfomanceFactDate, "Дата фактического исполнения").Column("PERFORMANCE_FACT_DATE");
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_INFO_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Reference(x => x.AnswerFile, "Файл ответа").Column("ANSWERFILE_INFO_ID").Fetch();
            Reference(x => x.SignedAnswerFile, "Подписанный файл ответа").Column("SIGNED_ANSWERFILE_ID").Fetch();
            Reference(x => x.AnswerSignature, "Подпись ответа").Column("SIGNATURE_ANSWERFILE_ID").Fetch();
            Reference(x => x.AnswerCertificate, "Сертификат ответа").Column("CERTIFICATE_ANSWERFILE_ID").Fetch();
            Reference(x => x.Inspector, "Должностное лицо").Column("INSPECTOR_ID").Fetch();
            Reference(x => x.Executor, "Исполнитель").Column("EXECUTOR_ID").Fetch();
            Property(x => x.KindKNDGJI, "Вид контроля/надзора").Column("KIND_KND").NotNull();
        }
    }
    
}
