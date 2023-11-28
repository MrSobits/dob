﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using global::Quartz.Util;


namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    /// <summary>
    /// Провайдер задачи на заполнение оплат штрафов
    /// </summary>
    public class CreateResolutionPayFinesTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "CreateResolutionPayFinesTaskProvider";

        #endregion

        #region Constructors

        public CreateResolutionPayFinesTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на заполнение оплат штрафов
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Заполнение оплат штрафов",
                        CreateResolutionPayFinesTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
