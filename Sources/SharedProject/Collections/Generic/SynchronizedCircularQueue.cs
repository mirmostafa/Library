#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using Mohammad.Helpers;

namespace Mohammad.Collections.Generic
{
    public class SynchronizedCircularQueue<T>
    {
        private static readonly Queue<T> _Queue = new Queue<T>();
        private static readonly object _LockObject = new object();
        private readonly int _TotalItems;

        public SynchronizedCircularQueue(int maxCount = int.MaxValue)
        {
            if (maxCount <= 0)
            {
                throw new ArgumentException("maxCount can not be less than or equal to zero");
            }

            this._TotalItems = maxCount;
        }

        /// <summary>
        ///     Get first object from queue or null if is empty.
        /// </summary>
        public T Peek() => CodeHelper.Lock(() => _Queue.Count > 0 ? _Queue.Peek() : default, _LockObject);

        /// <summary>
        ///     Get first object from queue and remove it from queue or return null if queue is empty.
        /// </summary>
        public T Dequeue() => CodeHelper.Lock(() => _Queue.Count > 0 ? _Queue.Dequeue() : default, _LockObject);

        /// <summary>
        ///     Add input to end of queue and remove the firs one if queue is full.
        /// </summary>
        /// <param name="input"></param>
        public void Enqueue(T input)
        {
            lock (_LockObject)
            {
                if (_Queue.Count >= this._TotalItems)
                {
                    _Queue.Dequeue();
                }

                _Queue.Enqueue(input);
            }
        }
    }
}