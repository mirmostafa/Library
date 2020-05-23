#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mohammad.Dynamic;

namespace Mohammad.Helpers
{
    public static class DynamicHelper
    {
        /// <exception cref="Exception">Only library Expandos can be used.</exception>
        public static T As<T>(dynamic expando)
            where T : new()
        {
            if (expando is Expando)
            {
                var result = new T();
                foreach (var property in result.GetType().GetProperties())
                {
                    var value = expando[property.Name];
                    if (DBNull.Value.Equals(value))
                    {
                        value = null;
                    }

                    property.SetValue(result, value);
                }

                return result;
            }

            throw new Exception("Only library Expando can be used.");
        }

        /// <exception cref="Exception">Only library expandos can be used.</exception>
        public static IEnumerable<T> AsEnumerable<T>(dynamic expandos)
            where T : new()
        {
            if (expandos is List<Expando> || expandos is IEnumerable<Expando>)
            {
                foreach (var expando in expandos)
                {
                    var result = new T();
                    foreach (var property in result.GetType().GetProperties())
                    {
                        var value = expando[property.Name];
                        if (DBNull.Value.Equals(value))
                        {
                            value = null;
                        }

                        property.SetValue(result, value);
                    }

                    yield return result;
                }
            }
            else
            {
                throw new Exception("Only a list of library expandos can be used.");
            }
        }

        public static dynamic ToDynamic(this object obj)
        {
            var result = new ExpandoObject();
            result.AddMany(obj.GetType().GetProperties().Select(prop => new KeyValuePair<string, object>(prop.Name, prop.GetValue(obj, null))));
            return result;
        }

        public static IEnumerable<T> ToEnumerable<T>(dynamic dyn)
        {
            foreach (T item in dyn)
            {
                yield return item;
            }
        }

        //public static List<T> ToList<T>(dynamic dyn)
        //{
        //	return Enumerable.ToList(ToEnumerable<T>(dyn));
        //}

        public static List<T> ToList<T>(dynamic dyn)
        {
            var result = new List<T>();
            foreach (T item in dyn)
            {
                result.Add(item);
            }

            return result;
        }
    }
}