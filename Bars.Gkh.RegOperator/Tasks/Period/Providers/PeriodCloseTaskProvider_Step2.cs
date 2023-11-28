﻿namespace Bars.Gkh.RegOperator.Tasks.Period.Providers
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Tasks.Period.Callbacks;
    using Bars.Gkh.RegOperator.Tasks.Period.Executors;

    /// <summary>
    /// Провайдер задачи закрытия периода (Этап 2)
    /// </summary>
    public class PeriodCloseTaskProvider_Step2 : ITaskProvider
    {
        #region Implementation of ITaskProvider
        
        /// <summary>
        /// Создать задачи
        /// </summary>
        /// <param name="params">Базовые параметры</param>
        /// <returns>Результат</returns>
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor("Закрытие периода", PeriodCloseTaskExecutor_Step2.Id, @params)
                    {
                        SuccessCallback = PeriodCloseSuccessCallback_Step2.Id,
                        FailCallback = PeriodCloseFailCallback.Id
                    }
                });
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode => "PeriodClose";

        #endregion
    }
}