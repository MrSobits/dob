namespace Bars.Gkh.Config.Impl
{
    using Bars.B4;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.Gkh.Config.Impl.Internal;
    using Bars.Gkh.Config.Impl.Internal.Serialization;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.SignalR;
    using Castle.Windsor;
    using Microsoft.AspNet.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Бэкенд хранения настроек в базе данных
    /// </summary>
    public class DatabaseConfigStorageBackend : IGkhConfigStorageBackend
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container"></param>
        /// <param name="domainService"></param>
        /// <param name="logManager"></param>
        public DatabaseConfigStorageBackend(
            IWindsorContainer container,
            IDomainService<GkhConfigParam> domainService,
            ILogManager logManager)
        {
            Container = container;
            DomainService = domainService;
            LogManager = logManager;
        }

        private IWindsorContainer Container { get; set; }

        private IDomainService<GkhConfigParam> DomainService { get; set; }

        private ILogManager LogManager { get; set; }

        public IDictionary<string, ValueHolder> GetConfig()
        {
            var scope = ExplicitSessionScope.EnterNewScope();
            try
            {
                return DomainService.GetAll().ToDictionary(x => x.Key, x => new ValueHolder(x.Value));
            }
            catch (Exception e)
            {
                LogManager.Error(
                    "Не удалось загрузить конфигурацию приложения из БД. Будут использованы стандартные значения",
                    e);
                return new Dictionary<string, ValueHolder>();
            }
            finally
            {
                ExplicitSessionScope.LeaveScope(scope);
            }
        }

        public void UpdateConfig(IDictionary<string, ValueHolder> map)
        {
            ExplicitSessionScope.CallInNewScope(() =>
            {
                Container.InTransaction(() =>
                {
                    var keys = map.Keys.ToArray();
                    var values = DomainService.GetAll()
                        // давайте договоримся, что не будем менять более 999 ключей за раз
                        .Where(x => keys.Contains(x.Key))
                        .ToDictionary(x => x.Key, x => x);
                    foreach (var pair in map)
                    {
                        GkhConfigParam entity;
                        if (!values.TryGetValue(pair.Key, out entity))
                        {
                            entity = new GkhConfigParam { Key = pair.Key };
                        }

                        entity.Value = ConfigSerializer.Serialize(pair.Value);

                        DomainService.SaveOrUpdate(entity);
                    }
                });

                UpdateClientConfig(map);
            });
        }

        private void UpdateClientConfig(IDictionary<string, ValueHolder> map)
        {
            GlobalHost.ConnectionManager.GetHubContext<GkhConfigHub>()
                .Clients.All.updateParams(ConfigSerializer.Serialize(map));
        }
    }
}