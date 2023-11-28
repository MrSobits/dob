namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsAdmonitionMap : BaseEntityMap<AppealCitsAdmonition>
    {

        public AppealCitsAdmonitionMap() :
                base("Обращениям граждан - Предостережение", "GJI_CH_APPCIT_ADMONITION")
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
            Reference(x => x.AnswerFile, "Файл ответа").Column("ANSWERFILE_INFO_ID").Fetch();
            Reference(x => x.Inspector, "Должностное лицо").Column("INSPECTOR_ID").Fetch();
            Reference(x => x.Executor, "Исполнитель").Column("EXECUTOR_ID").Fetch();
        }
    }

}
