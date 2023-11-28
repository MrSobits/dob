namespace Bars.Gkh.Log.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.Gkh.Domain;

    using Castle.Windsor;

    /// <summary>
    /// Базовый класс для работы с <see cref="IAsyncLogger {T}"/>
    /// </summary>
    public abstract class AsyncLoggerBase<T> : IAsyncLogger<T> where T: class
    {
        private readonly object threadLock = new object();

        /// <summary>
        /// Кэш
        /// </summary>
        protected readonly ConcurrentQueue<T> LogCahe = new ConcurrentQueue<T>();

        /// <summary>
        /// Делегат события при добавлении элемента
        /// </summary>
        /// <param name="count">Размер очереди после добавления</param>
        protected delegate void QueueEvent(int count);

        /// <summary>
        /// Событие при добавлении элемента
        /// </summary>
        protected event QueueEvent OnEnqueue;

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Logger
        /// </summary>
        public ILogManager Logger { get; set; }

        /// <summary>
        /// Домен-сервис сохраняемой сущности
        /// </summary>
        public IDomainService<T> EntityDomainService { get; set; }

        /// <summary>
        /// Добавляет сущность в очередь для сохранения
        /// </summary>
        /// <param name="entity">Сохраняемая сущность</param>
        [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
        public virtual void Add(T entity)
        {
            this.LogCahe.Enqueue(entity);

            var onOnEnqueue = this.OnEnqueue;
            onOnEnqueue?.Invoke(this.LogCahe.Count);
        }

        /// <summary>
        /// Сохранить кэш
        /// </summary>
        public virtual void Flush()
        {
            try
            {
                var queueSize = this.LogCahe.Count;
                if (queueSize > 0)
                {
                    lock (this.threadLock)
                    {
                        ExplicitSessionScope.CallInNewScope(() =>
                        {
                            this.Container.InTransaction(() =>
                            {
                                for (int i = 0; i < queueSize; i++)
                                {
                                    T entity;
                                    if (this.LogCahe.TryDequeue(out entity))
                                    {
                                        this.EntityDomainService.Save(entity);
                                    }
                                    else
                                    {
                                        this.Logger?.Info($"Кэш сохранен не полностью ({i + 1}/{queueSize})");
                                        break;
                                    }
                                }
                            });
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.ErrorFormat(ex, "Ошибка при сохранении кэша");
            }
        }
    }
}