﻿namespace Bars.Gkh.Domain
{
    using B4.Application;
    using B4.IoC.Lifestyles.SessionLifestyle;
    using B4.Modules.Quartz;
    using B4.Utils;

    public class QuartzTaskWrapper<TTask> : ITask
        where TTask : ITask
    {
        /// <summary>
        /// Выполнение задачи
        /// </summary>
        /// <param name="params">Параметры исполнения задачи.
        ///             При вызове из планировщика передаются параметры из JobDataMap 
        ///             и контекст исполнения в параметре JobContext        
        ///             </param>
        public void Execute(DynamicDictionary @params)
        {
            var container = ApplicationContext.Current.Container;

            ExplicitSessionScope.CallInNewScope(
                () => container.Resolve<TTask>().Execute(@params));
        }
    }
}