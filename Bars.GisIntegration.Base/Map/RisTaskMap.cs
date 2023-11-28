﻿namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.Base.Entities.RisTaskMap
    /// </summary>
    public class RisTaskMap : BaseEntityMap<RisTask>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RisTaskMap()
            : base("Bars.GisIntegration.Base.Entities.RisTaskMap", "GI_TASK")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            this.Property(x => x.StartTime, "StartTime").Column("START_TIME");
            this.Property(x => x.EndTime, "EndTime").Column("END_TIME");
            this.Property(x => x.UserName, "UserName").Column("USER_NAME").Length(200);
            this.Property(x => x.ClassName, "ClassName").Column("CLASS_NAME").Length(200);
            this.Property(x => x.TaskState, "TaskState").Column("TASK_STATE");
        }
    }

    public class NhRisTaskMap : ClassMapping<RisTaskTrigger>
    {
        public NhRisTaskMap()
        {
            this.Property(
                x => x.Message,
                m =>
                    {
                        m.Type<BinaryStringType>();
                        m.Column("MESSAGE");
                    });
        }
    }
}