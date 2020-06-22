using System.Collections.Generic;
using Mohammad.Helpers;

namespace Mohammad.DesignPatterns.Behavioral
{
    public static class Memento
    {
        private static readonly Dictionary<object, object> _ObjectRepository = new Dictionary<object, object>();

        public static T Set<T>(object id, T value)
        {
            if (_ObjectRepository.ContainsKey(id))
                _ObjectRepository[id] = value;
            else
                _ObjectRepository.Add(id, value);
            return value;
        }

        public static object Get(object id) => _ObjectRepository.ContainsKey(id) ? _ObjectRepository[id] : null;
        public static T Get<T>(object id) => Get(id).To<T>();
    }
}