using System;
using System.Collections.Generic;
using static Mohammad.Helpers.CodeHelper;

namespace Collections.Generic
{
    public class SynchronizedCircularQueue<T>
    {
        private readonly int _TotalItems;
        private static readonly Queue<T> _Queue = new Queue<T>();
        private static readonly object _LockObject = new object();

        public SynchronizedCircularQueue(int maxCount = int.MaxValue)
        {
            if (maxCount <= 0)
                throw new ArgumentException("maxCount can not be less than or equal to zero");
            this._TotalItems = maxCount;
        }

        /// <summary>
        ///     Get first object from queue or null if is empty.
        /// </summary>
        public T Peek() => Lock(() => _Queue.Count > 0 ? _Queue.Peek() : default(T), _LockObject);

        /// <summary>
        ///     Get first object from queue and remove it from queue or return null if queue is empty.
        /// </summary>
        public T Dequeue() => Lock(() => _Queue.Count > 0 ? _Queue.Dequeue() : default(T), _LockObject);

        /// <summary>
        ///     Add input to end of queue and remove the firs one if queue is full.
        /// </summary>
        /// <param name="input"></param>
        public void Enqueue(T input)
        {
            lock (_LockObject)
            {
                if (_Queue.Count >= this._TotalItems)
                    _Queue.Dequeue();
                _Queue.Enqueue(input);
            }
        }
    }
}