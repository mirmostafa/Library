using System.Collections.Generic;
using Mohammad.EventsArgs;

namespace Mohammad.Data.BusinessTools.EventsArgs
{
    public class EntityActed<TEntity> : ItemActedEventArgs<TEntity>
    {
        // Methods
        public EntityActed(TEntity item)
            : base(item)
        {
        }
    }

    public class EntityActing<TEntity> : ItemActingEventArgs<TEntity>
    {
        // Methods
        public EntityActing(TEntity item)
            : base(item)
        {
        }
    }

    public enum EntityAction
    {
        Insert,
        Update,
        Delete
    }

    public class EntityValidated<TEntity> : ItemActedEventArgs<TEntity>
    {
        // Methods
        public EntityValidated(TEntity item, EntityAction action)
            : base(item) => this.Action = action;

        // Properties
        public EntityAction Action { get; }
    }

    public class EntityValidating<TEntity> : ItemActingEventArgs<TEntity>
    {
        // Methods
        public EntityValidating(TEntity item, EntityAction action)
            : base(item) => this.Action = action;

        // Properties
        public EntityAction Action { get; }
    }

    public class EntityValidatingByIds : ItemActingEventArgs<IEnumerable<long>>
    {
        // Methods
        public EntityValidatingByIds(IEnumerable<long> items, EntityAction action)
            : base(items) => this.Action = action;

        // Properties
        public EntityAction Action { get; }
    }
}