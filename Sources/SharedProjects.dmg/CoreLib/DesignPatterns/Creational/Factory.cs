using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Collections.Generic;
using Mohammad.Helpers;

namespace Mohammad.DesignPatterns.Creational
{
    public static class Factory
    {
        private static readonly List<TripleValue<Type, Delegate, object>> _Manufacturares = new List<TripleValue<Type, Delegate, object>>();

        private static TripleValue<Type, Delegate, object> FindByType<T>()
        {
            return _Manufacturares.FirstOrDefault(manufacturare => manufacturare.Value1 == typeof(T));
        }

        public static void SetProducer<T>(Func<T> producer)
        {
            if (FindByType<T>() != null)
                throw new Exception();
            _Manufacturares.Add(new TripleValue<Type, Delegate, object>(typeof(T), producer, null));
        }

        public static T Get<T>(bool produceNew = false, bool overwrite = true)
        {
            var manufacturer = FindByType<T>();
            if (produceNew)
            {
                var value3 = manufacturer.Value2.As<Func<T>>()();
                if (overwrite)
                    manufacturer.Value3 = value3;
                return value3;
            }
            if (manufacturer.Value3 == null)
                manufacturer.Value3 = manufacturer.Value2.As<Func<T>>()();
            return (T) manufacturer.Value3;
        }
    }
}