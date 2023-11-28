namespace Bars.Gkh.Utils
{
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Windsor;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Domain;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    /// <summary>
    /// Помощник регистрации сущностей в контейнере
    /// </summary>
    public static class ContainerHelper
    {
        private static IWindsorContainer Container => ApplicationContext.Current.Container;

        /// <summary>
        /// Зарегистрировать экспортируемую сущность как Transient
        /// </summary>
        /// <typeparam name="T">Тип экспортируемой сущности</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void RegisterExportableEntity<T>(IWindsorContainer container = null)
            where T : IExportableEntity
        {
            (container ?? ContainerHelper.Container).RegisterTransient<IExportableEntity, T>();
        }

        /// <summary>
        /// Подменить зарегистрированную экспортируемую сущность как Transient
        /// </summary>
        /// <typeparam name="TImpl">Заменяемый тип экспортируемой сущности</typeparam>
        /// <typeparam name="TNewImpl">Новый тип экспортируемой сущности</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void ReplaceExportableEntity<TImpl, TNewImpl>(IWindsorContainer container = null)
            where TImpl : IExportableEntity
            where TNewImpl : IExportableEntity
        {
            (container ?? ContainerHelper.Container).ReplaceComponent<IExportableEntity>(typeof(TImpl),
                Component.For<IExportableEntity>()
                    .ImplementedBy<TNewImpl>()
                    .LifestyleTransient());
        }

        /// <summary>
        /// Зарегистрировать частично экспортируемую сущность как Transient
        /// </summary>
        /// <typeparam name="TBaseImpl">Базовая сущность</typeparam>
        /// <typeparam name="TPartImpl">Частичная сущность</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void RegisterPartialExportableEntity<TBaseImpl, TPartImpl>(IWindsorContainer container = null)
            where TBaseImpl : IExportableEntity
            where TPartImpl : IPartialExportableEntity<TBaseImpl>
        {
            (container ?? ContainerHelper.Container)
                .RegisterTransient<IPartialExportableEntity<TBaseImpl>, TPartImpl>();
        }

        /// <summary>
        /// Зарегистрировать сервис селектора экспортируемой сущности как Transient
        /// </summary>
        /// <typeparam name="TProxy">Прокси-объект экспортируемой сущности</typeparam>
        /// <typeparam name="TSelector">Релизация селектора прокси-объекта</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void RegisterProxySelectorService<TProxy, TSelector>(IWindsorContainer container = null)
            where TSelector : IProxySelectorService<TProxy>
            where TProxy : IHaveId
        {
            (container ?? ContainerHelper.Container).RegisterTransient<IProxySelectorService<TProxy>, TSelector>();
        }

        /// <summary>
        /// Подменить зарегистрированный сервис селектора экспортируемой сущности как SessionScoped
        /// </summary>
        /// <typeparam name="TProxy">Прокси-сущность</typeparam>
        /// <typeparam name="TImpl">Заменяемый сервис-селектор</typeparam>
        /// <typeparam name="TNewImpl">Новый сервис-селектор</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void ReplaceProxySelectorService<TProxy, TImpl, TNewImpl>(IWindsorContainer container = null)
            where TImpl : IProxySelectorService<TProxy>
            where TNewImpl : IProxySelectorService<TProxy>
            where TProxy : IHaveId
        {
            (container ?? ContainerHelper.Container).ReplaceComponent<IProxySelectorService<TProxy>>(typeof(TImpl),
                Component.For<IProxySelectorService<TProxy>>()
                    .ImplementedBy<TNewImpl>()
                    .LifeStyle.Transient);
        }

        /// <summary>
        /// Зарегистрировать коллекцию экспортируемых файлов как Transient
        /// </summary>
        /// <typeparam name="T">Тип экспортируемой сущности</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void RegisterFileInfoExportableEntity<T>(IWindsorContainer container = null)
            where T : IFileEntityCollection, IExportableEntity
        {
            (container ?? ContainerHelper.Container).RegisterTransient<IFileEntityCollection, T>();
        }

        /// <summary>
        /// Подменить зарегистрированную коллекцию экспортируемых файлов как Transient
        /// </summary>
        /// <typeparam name="TImpl">Заменяемый тип экспортируемой сущности</typeparam>
        /// <typeparam name="TNewImpl">Новый тип экспортируемой сущности</typeparam>
        /// <param name="container">IoC контейнер</param>
        public static void ReplaceFileInfoExportableEntity<TImpl, TNewImpl>(IWindsorContainer container = null)
            where TImpl : IFileEntityCollection, IExportableEntity
            where TNewImpl : IFileEntityCollection, IExportableEntity
        {
            (container ?? ContainerHelper.Container).ReplaceComponent<IFileEntityCollection>(typeof(TImpl),
                Component.For<IFileEntityCollection>()
                    .ImplementedBy<TNewImpl>()
                    .LifestyleTransient());
        }

        /// <summary>
        /// Заменить ViewModel в контейнере
        /// </summary>
        /// <typeparam name="TEntity">Хранимая сущность</typeparam>
        /// <typeparam name="TImpl">Старая реализация</typeparam>
        /// <typeparam name="TNewImpl">Новая реализация</typeparam>
        public static void ReplaceViewModel<TEntity, TImpl, TNewImpl>(IWindsorContainer container = null)
            where TEntity : IEntity
            where TImpl : class, IViewModel<TEntity>
            where TNewImpl : class, IViewModel<TEntity>
        {
            (container ?? ContainerHelper.Container).ReplaceTransient<IViewModel<TEntity>, TImpl, TNewImpl>();
        }

        /// <summary>
        /// Зарегистрировать контроллер как <see cref="BaseGkhDataController{T}"/>
        /// </summary>
        /// <typeparam name="T">Хранимая сущность</typeparam>
        public static void RegisterGkhDataController<T>(IWindsorContainer container = null)
            where T : IEntity
        {
            var controllerType = typeof(T);
            var methods = typeof(WindsorHelper).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
            var name = methods.First(x => x.Name == "GetControllerRegistrationName" && x.GetParameters().Length == 1)
                .Invoke(null, new object[] { controllerType }) as string; // string WindsorHelper.GetControllerRegistrationName(Type t);

            (container ?? ContainerHelper.Container).Register(Component.For<IController>()
                .ImplementedBy(typeof(BaseGkhDataController<>).MakeGenericType(controllerType))
                .LifestylePerWebRequest()
                .Named(name));
        }

        /// <summary>
        /// Зарегистрировать контроллер как <see cref="FileStorageDataController{T}"/>
        /// </summary>
        /// <typeparam name="T">Хранимая сущность</typeparam>
        public static void RegisterFileDataController<T>(IWindsorContainer container = null)
            where T : class, IEntity
        {
            (container ?? ContainerHelper.Container).RegisterFileStorageDataController<T>();
            (container ?? ContainerHelper.Container).RegisterFileStorageDomainService<T>();
        }
    }
}