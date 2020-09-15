using System;
using System.Data;
using System.Reflection;

namespace Mohammad.Helpers
{
    public partial class AdoHelper
    {
        public static void AddRange(this DataColumnCollection columns, params string[] colNames)
        {
            foreach (var colName in colNames)
            {
                columns.Add(new DataColumn(colName));
            }
        }

        public static T ValueOf<T>(this DataRow row, string columnName) => UnboxT<T>.Unbox(row[columnName]);
        public static T ValueOf<T>(this DataRow row, DataColumn column) => UnboxT<T>.Unbox(row[column]);
        public static T ValueOf<T>(this DataRow row, int columnIndex) => UnboxT<T>.Unbox(row[columnIndex]);
        public static T ValueOf<T>(this DataRow row, int columnIndex, DataRowVersion version) => UnboxT<T>.Unbox(row[columnIndex, version]);
        public static T ValueOf<T>(this DataRow row, string columnName, DataRowVersion version) => UnboxT<T>.Unbox(row[columnName, version]);
        public static T ValueOf<T>(this DataRow row, DataColumn column, DataRowVersion version) => UnboxT<T>.Unbox(row[column, version]);

        private static class UnboxT<T>
        {
            internal static readonly Converter<object, T> Unbox = Create(typeof(T));

            private static Converter<object, T> Create(Type type)
            {
                if (type.IsValueType)
                {
                    if (type.IsGenericType && !type.IsGenericTypeDefinition && typeof(Nullable<>) == type.GetGenericTypeDefinition())
                    {
                        return (Converter<object, T>)Delegate.CreateDelegate(typeof(Converter<object, T>),
                            typeof(UnboxT<T>)
                                .GetMethod("NullableField", BindingFlags.Static | BindingFlags.NonPublic)
                                .MakeGenericMethod(type.GetGenericArguments()[0]));
                    }

                    return ValueField;
                }

                return ReferenceField;
            }

            private static TElem? NullableField<TElem>(object value)
                where TElem : struct
            {
                if (DBNull.Value == value)
                {
                    return default;
                }

                return (TElem)Convert.ChangeType(value, typeof(TElem));
            }

            private static T ReferenceField(object value) => DBNull.Value == value ? default : (T)Convert.ChangeType(value, typeof(T));

            /// <exception cref="InvalidCastException"></exception>
            private static T ValueField(object value)
            {
                if (DBNull.Value == value)
                {
                    throw new InvalidCastException();
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }
}