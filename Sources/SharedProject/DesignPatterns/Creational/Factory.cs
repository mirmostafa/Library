//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Mohammad.DesignPatterns.Creational
//{
//    public static class Factory
//    {
//        private static readonly List<Tuple<Type, Delegate, object>> _Manufacturares = new List<Tuple<Type, Delegate, object>>();

//        public static void SetProducer<T>(Func<T> producer)
//        {
//            if (FindByType<T>() != null)
//            {
//                throw new Exception();
//            }

//            _Manufacturares.Add(new Tuple<Type, Delegate, object>(typeof(T), producer, null));
//        }

//        public static T Get<T>(bool produceNew = false, bool overwrite = true)
//        {
//            var manufacturer = FindByType<T>();
//            if (produceNew)
//            {
//                var item3 = manufacturer.Item2 as Func<T>;
//                if (overwrite)
//                {
//                    manufacturer.Item3 = item3;
//                }

//                return item3;
//            }

//            if (manufacturer.Item3 == null)
//            {
//                manufacturer.Item3 = manufacturer.Item2.As<Func<T>>()();
//            }

//            return (T)manufacturer.Item3;
//        }

//        private static Tuple<Type, Delegate, object> FindByType<T>() => _Manufacturares.FirstOrDefault(manufacturare => manufacturare.Item1 == typeof(T));
//    }
//}

