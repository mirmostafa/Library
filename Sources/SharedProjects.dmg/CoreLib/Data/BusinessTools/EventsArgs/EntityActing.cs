using System.Collections.Generic;
using Mohammad.EventsArgs;

namespace Mohammad.Data.BusinessTools.EventsArgs
{
    public class EntityActed<TEntity> : ItemActedEventArgs<TEntity>
    {
        // Methods
        public EntityActed(TEntity item)
            : base(item) { }
    }

    public class EntityActing<TEntity> : ItemActingEventArgs<TEntity>
    {
        // Methods
        public EntityActing(TEntity item)
            : base(item) { }
    }

    public enum EntityAction
    {
        Insert,
        Update,
        Delete
    }

    public class EntityValidated<TEntity> : ItemActedEventArgs<TEntity>
    {
        // Properties
        public EntityAction Action { get; private set; }
        // Methods
        public EntityValidated(TEntity item, EntityAction action)
            : base(item) { this.Action = action; }
    }

    public class EntityValidating<TEntity> : ItemActingEventArgs<TEntity>
    {
        // Properties
        public EntityAction Action { get; private set; }
        // Methods
        public EntityValidating(TEntity item, EntityAction action)
            : base(item) { this.Action = action; }
    }

    public class EntityValidatingByIds : ItemActingEventArgs<IEnumerable<long>>
    {
        // Properties
        public EntityAction Action { get; private set; }
        // Methods
        public EntityValidatingByIds(IEnumerable<long> items, EntityAction action)
            : base(items) { this.Action = action; }
    }
}