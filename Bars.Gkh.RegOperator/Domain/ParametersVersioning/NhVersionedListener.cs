namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System.Linq;
    using B4.Utils;

    using Castle.Windsor;
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;
    using NHibernate;
    using NHibernate.Event;

    /// <summary>
    /// Следит за создаваемыми сущностями, если сущность версионируется,
    /// то создаем первую версию для параметра сущности
    /// </summary>
    public class NhVersionedListener : IPostInsertEventListener, IPostDeleteEventListener
    {
        private LogsHolder Holder
        {
            get { return this.container.Resolve<LogsHolder>(); }
        }

        public NhVersionedListener(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Called after inserting an item in the datastore
        /// </summary>
        /// <param name="event"></param>
        public void OnPostInsert(PostInsertEvent @event)
        {
            var entity = @event.Entity;

            if (!VersionedEntityHelper.IsUnderVersioning(entity)) return;

            var parameters =
                VersionedEntityHelper.GetCreator(entity)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                    .ToList();

            var holder = this.Holder;

            parameters.ForEach(x =>
            {
                var save = new EntityLogLight
                {
                    EntityId = @event.Id.ToLong(),
                    ClassName = x.ClassName,
                    PropertyName = x.PropertyName,
                    PropertyValue = x.PropertyValue,
                    ParameterName = x.ParameterName
                };
                holder.Add(save, entity);
            });
        }

        /// <summary>
        /// Called after deleting an item from the datastore
        /// </summary>
        /// <param name="event"></param>
        public void OnPostDelete(PostDeleteEvent @event)
        {
            var entity = @event.Entity;
            var childSession = @event.Session.GetSession(EntityMode.Poco);

            if (!VersionedEntityHelper.IsUnderVersioning(entity)) return;

            var parameters =
                VersionedEntityHelper.GetCreator(entity)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                    .ToList();

            var keys = parameters.Select(x => "{0}|{1}".FormatUsing(@event.Id, x.ParameterName)).ToList();

            childSession.CreateQuery(
                "delete from EntityLogLight where concat(EntityId, concat('|', ParameterName)) in (:keys)")
                .SetParameterList("keys", keys)
                .ExecuteUpdate();

            childSession.Flush();

            this.Holder.Remove(keys.ToArray());
        }

        private readonly IWindsorContainer container;
    }
}