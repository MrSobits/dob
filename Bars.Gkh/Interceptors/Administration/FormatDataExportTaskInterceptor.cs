namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Scheduler;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;

    /// <summary>
    /// Задача для экспорта по формату
    /// </summary>
    public class FormatDataExportTaskInterceptor : EmptyDomainInterceptor<FormatDataExportTask>
    {
        public IGkhUserManager GkhUserManager { get; set; }
        public IFormatDataExportSchedulerService FormatDataExportScheduler { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            this.PrepareEntity(entity);

            var activeOperator = this.GkhUserManager.GetActiveOperator();
            var checkResult = this.CheckOperator(activeOperator);
            if (!checkResult.Success)
            {
                return checkResult;
            }

            entity.Operator = activeOperator;
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            this.FormatDataExportScheduler.ScheduleJob(new FormatDataExportJobInstance(entity));
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            var activeOperator = this.GkhUserManager.GetActiveOperator();

            var checkResult = this.CheckOperator(activeOperator);
            if (!checkResult.Success)
            {
                return checkResult;
            }

            this.PrepareEntity(entity);
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            this.FormatDataExportScheduler.UnScheduleJob(new FormatDataExportJobInstance(entity));
            this.FormatDataExportScheduler.ScheduleJob(new FormatDataExportJobInstance(entity));
            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<FormatDataExportTask> service, FormatDataExportTask entity)
        {
            this.FormatDataExportScheduler.UnScheduleJob(new FormatDataExportJobInstance(entity));
            return this.Success();
        }

        private void PrepareEntity(FormatDataExportTask entity)
        {
            if (entity.StartNow)
            {
                entity.StartTimeHour = DateTime.Now.Hour;
                entity.StartTimeMinutes = DateTime.Now.Minute;
            }
            if (entity.EntityGroupCodeList.Contains("All"))
            {
                entity.EntityGroupCodeList.Clear();
            }
        }

        private IDataResult CheckOperator(Operator gkhOperator)
        {
            if (gkhOperator.IsNull())
            {
                return this.Failure("К текущему пользователю не привязан оператор");
            }

            if (gkhOperator.Contragent.IsNull())
            {
                return this.Failure($"У оператора '{gkhOperator.User.Name}' не указан контрагент для заполнения");
            }

            this.Container.UsingForResolved<IFormatDataExportRoleService>((container, service) =>
            {
                // Выбросит исключение, если не сопоставлена роль оператора не сопоставлена с поставщиком информации
                service.GetCustomProviderFlags(gkhOperator);
            });

            return new BaseDataResult();
        }
    }
}