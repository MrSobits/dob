namespace Bars.Gkh.Map
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для Bars.Gkh.Entities.Administration.ExecutionActionEntity
    /// </summary>
    public class ExecutionActionHistoryMap : BaseEntityMap<ExecutionActionHistory>
    {
        #region Константы
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GKH_EXECUTION_ACTION";

        /// <summary>
        /// Код действия (<see cref="string"/>)
        /// </summary>
        public const string CodeColumnName = "CODE";

        /// <summary>
        /// Идентификатор задачи (<see cref="Guid"/>)
        /// </summary>
        public const string JobIdColumnName = "JOB_ID";

        /// <summary>
        /// Дата создания задачи (<see cref="DateTime"/>)
        /// </summary>
        public const string CreateDateColumnName = "CREATE_DATE";

        /// <summary>
        /// Дата запуска задачи (<see cref="DateTime"/>)
        /// </summary>
        public const string StartDateColumnName = "START_DATE";

        /// <summary>
        /// Дата завершения задачи (<see cref="DateTime"/>)
        /// </summary>
        public const string EndDateColumnName = "END_DATE";

        /// <summary>
        /// Результат (<see cref="string"/>)
        /// </summary>
        public const string DataResultColumnName = "DATA_RESULT";

        /// <summary>
        /// Статус (<see cref="ExecutionActionStatus"/>)
        /// </summary>
        public const string StatusColumnName = "STATUS";

        #endregion
        /// <summary></summary>
        public ExecutionActionHistoryMap()
            : base(typeof(ExecutionActionHistory).FullName, ExecutionActionHistoryMap.TableName)
        {
        }

        /// <summary></summary>
        protected override void Map()
        {
            this.Property(x => x.Code, "Код действия").Column(ExecutionActionHistoryMap.CodeColumnName).NotNull();
            this.Property(x => x.JobId, "Идентификатор задачи").Column(ExecutionActionHistoryMap.JobIdColumnName).NotNull();
            this.Property(x => x.CreateDate, "Дата создания задачи").Column(ExecutionActionHistoryMap.CreateDateColumnName);
            this.Property(x => x.StartDate, "Дата запуска задачи").Column(ExecutionActionHistoryMap.StartDateColumnName);
            this.Property(x => x.EndDate, "Дата завершения задачи").Column(ExecutionActionHistoryMap.EndDateColumnName);
            this.Property(x => x.DataResult, "Результат выполнения задачи").Column(ExecutionActionHistoryMap.DataResultColumnName);
            this.Property(x => x.Status, "Статус выполнения задачи").Column(ExecutionActionHistoryMap.StatusColumnName);

        }
    }

    /// <summary>Сериализация <see cref="BaseDataResult"/></summary>
    public class ExecutionActionMapNHibernateMapping : ClassMapping<ExecutionActionHistory>
    {
        /// <summary> </summary>
        public ExecutionActionMapNHibernateMapping()
        {
            this.Property(x => x.DataResult, m =>
            {
                m.Type<BinaryJsonType<BaseDataResult>>();
            });
        }
    }
}