using System.Collections.ObjectModel;

namespace Mohammad.Collections.ObjectModel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    public abstract class CollectionContainer<T, TCollection>
        where TCollection : Collection<T>, new()
    {
        private TCollection? _Items;

        /// <summary>
        ///     Gets the items.
        /// </summary>
        /// <value>The items.</value>
        protected TCollection Items => this._Items ??= new TCollection();

        /// <summary>
        ///     Gets the <see cref="T" /> at the specified index.
        /// </summary>
        /// <value></value>
        public T this[int index] => this.Items[index];
    }
}