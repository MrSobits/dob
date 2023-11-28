namespace Bars.Gkh.RegOperator.Quartz
{
    using System;
    using System.Linq;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Задача по синхронизации состояний аварийных домов с жилыми домами
    /// </summary>
    public class SyncEmergencyObjectsTask : BaseTask
    {
        /// <summary>
        /// Метод выполнения задачи
        /// </summary>
        /// <param name="paramDictionary">Параметры</param>
        public override void Execute(DynamicDictionary paramDictionary)
        {
            if (!this.Container.HasComponent<IEmergencyObjectSyncService>())
            {
                return;
            }

            ExplicitSessionScope.CallInNewScope(() =>
            {
                var logDomain = this.Container.ResolveDomain<EntityLogLight>();
                var realityObjectRepo = this.Container.ResolveRepository<RealityObject>();
                var logManager = this.Container.Resolve<ILogManager>();
                var emergencyObjSyncService = this.Container.Resolve<IEmergencyObjectSyncService>();
                var configProvider = this.Container.Resolve<IConfigProvider>();

                if (!configProvider.GetConfig().AppSettings.GetAs("RegOperator.ChangePersonalAccountState.Enabled", true))
                {
                    return;
                }

                try
                {
                    logManager.Info("Запуск синхронизации состояний аварийных домов с состояниями жилых");

                    try
                    {
                        this.Container.InTransaction(
                            () =>
                            {
                                var logList =
                                    logDomain.GetAll()
                                        .Where(x => x.ClassName == "RealityObject")
                                        .Where(x => x.PropertyName == "ConditionHouse")
                                        .Where(x => x.DateActualChange.Date == DateTime.Now.Date)
                                        .ToList();

                                var roIds = logList.Select(x => x.EntityId).ToArray();

                                var realityObjDict =
                                    realityObjectRepo.GetAll().Where(x => roIds.Contains(x.Id)).ToDictionary(x => x.Id);

                                foreach (var log in logList)
                                {
                                    var realityObject = realityObjDict[log.EntityId];
                                    emergencyObjSyncService.SyncEmergencyObjectCondition(realityObject, log);
                                }
                            });
                    }
                    catch (Exception e)
                    {
                        logManager.Error("Ошибка при синхронизации состояний аварийных домов с состояниями жилых", e);
                    }

                    logManager.Info("Завершение синхронизации состояний аварийных домов с состояниями жилых");
                }
                catch (Exception exception)
                {
                    logManager.Error(exception.Message, exception);
                }
                finally
                {
                    this.Container.Release(logDomain);
                    this.Container.Release(realityObjectRepo);
                    this.Container.Release(logManager);
                    this.Container.Release(emergencyObjSyncService);
                }
            });
        }
    }
}