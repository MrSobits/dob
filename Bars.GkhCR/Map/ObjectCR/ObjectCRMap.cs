/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Gkh.Map;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности " Объект капитального ремонта"
///     /// </summary>
///     public class ObjectCrMap : BaseGkhEntityMap<ObjectCr>
///     {
///         public ObjectCrMap() : base("CR_OBJECT")
///         {
///             Map(x => x.GjiNum, "GJI_NUM").Length(300);
///             Map(x => x.ProgramNum, "PROGRAM_NUM").Length(300);
///             Map(x => x.DateEndBuilder, "DATE_END_BUILDER");
///             Map(x => x.DateStartWork, "DATE_START_WORK");
///             Map(x => x.DateEndWork, "DATE_END_WORK");
///             Map(x => x.DateStopWorkGji, "DATE_STOP_WORK_GJI");
///             Map(x => x.DateCancelReg, "DATE_CANCEL_REG");
///             Map(x => x.DateAcceptCrGji, "DATE_ACCEPT_GJI");
///             Map(x => x.DateAcceptReg, "DATE_ACCEPT_REG");
///             Map(x => x.DateGjiReg, "DATE_GJI_REG");
///             Map(x => x.SumDevolopmentPsd, "SUM_DEV_PSD");
///             Map(x => x.SumSmr, "SUM_SMR");
///             Map(x => x.SumSmrApproved, "SUM_SMR_APPROVED");
///             Map(x => x.SumTehInspection, "SUM_TECH_INSP");
///             Map(x => x.FederalNumber, "FEDERAL_NUM").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.AllowReneg, "ALLOW_RENEG");
/// 
///             References(x => x.ProgramCr, "PROGRAM_ID").Fetch.Join();
///             References(x => x.BeforeDeleteProgramCr, "BEFORE_DELETE_PROGRAM_ID").Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Объект капитального ремонта"</summary>
    public class ObjectCrMap : BaseImportableEntityMap<ObjectCr>
    {
        
        public ObjectCrMap() : 
                base("Объект капитального ремонта", "CR_OBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.GjiNum, "Номер комиссии").Column("GJI_NUM").Length(300);
            Property(x => x.ProgramNum, "Номер по программе").Column("PROGRAM_NUM").Length(300);
            Property(x => x.DateEndBuilder, "Дата завершения работ подрядчиком").Column("DATE_END_BUILDER");
            Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            Property(x => x.DateStopWorkGji, "Дата остановки работ ГЖИ").Column("DATE_STOP_WORK_GJI");
            Property(x => x.DateCancelReg, "Дата отклонения от регистрации").Column("DATE_CANCEL_REG");
            Property(x => x.DateAcceptCrGji, "Дата принятия КР ГЖИ").Column("DATE_ACCEPT_GJI");
            Property(x => x.DateAcceptReg, "Дата принятия на регистрацию").Column("DATE_ACCEPT_REG");
            Property(x => x.DateGjiReg, "Дата регистрации ГЖИ").Column("DATE_GJI_REG");
            Property(x => x.SumDevolopmentPsd, "Сумма на разработку экспертизы ПСД").Column("SUM_DEV_PSD");
            Property(x => x.SumSmr, "Сумма на СМР").Column("SUM_SMR");
            Property(x => x.DeadlineMissed, "Срыв сроков").Column("DEADLINE_MISSED");
            Property(x => x.SumSmrApproved, "Утвержденная сумма").Column("SUM_SMR_APPROVED");
            Property(x => x.SumTehInspection, "Сумма на технадзор").Column("SUM_TECH_INSP");
            Property(x => x.FederalNumber, "Федеральный номер").Column("FEDERAL_NUM").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Property(x => x.AllowReneg, "Разрешение на повторное согласование").Column("ALLOW_RENEG");
            Property(x => x.MaxKpkrAmount, "Предельная сумма из КПКР").Column("MAX_KPKR_AMOUNT");
            Property(x => x.FactAmountSpent, "Фактически освоенная сумма").Column("FACT_AMOUNT_SPENT");
            Property(x => x.FactStartDate, "Фактическая дата начала работ").Column("FACT_START_DATE");
            Property(x => x.FactEndDate, "Фактическая дата окончания работ").Column("FACT_END_DATE");
            Property(x => x.WarrantyEndDate, "Дата окончания гарантийных обязательств").Column("WARRANTY_END_DATE");
            Reference(x => x.ProgramCr, "Программа").Column("PROGRAM_ID").Fetch();
            Reference(x => x.BeforeDeleteProgramCr, "Программа, на которую ссылался объект до удаления").Column("BEFORE_DELETE_PROGRAM_ID").Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
