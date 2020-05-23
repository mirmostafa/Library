using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

namespace Mohammad.Data.Ado
{
    public class ConvertType
    {
        public static Dictionary<SqlDbType, Type> SqlDbTypeMap;

        static ConvertType()
        {
            SqlDbTypeMap = new Dictionary<SqlDbType, Type>
                           {
                               {SqlDbType.BigInt, typeof(long)},
                               {SqlDbType.Binary, typeof(byte[])},
                               {SqlDbType.Bit, typeof(bool)},
                               {SqlDbType.Char, typeof(string)},
                               {SqlDbType.Date, typeof(DateTime)},
                               {SqlDbType.DateTime, typeof(DateTime)},
                               {SqlDbType.DateTime2, typeof(DateTime)},
                               {SqlDbType.DateTimeOffset, typeof(DateTimeOffset)},
                               {SqlDbType.Decimal, typeof(decimal)},
                               {SqlDbType.Float, typeof(double)},
                               {SqlDbType.Image, typeof(byte[])},
                               {SqlDbType.Int, typeof(int)},
                               {SqlDbType.Money, typeof(decimal)},
                               {SqlDbType.NChar, typeof(string)},
                               {SqlDbType.NText, typeof(string)},
                               {SqlDbType.NVarChar, typeof(string)},
                               {SqlDbType.Real, typeof(float)},
                               {SqlDbType.SmallDateTime, typeof(DateTime)},
                               {SqlDbType.SmallInt, typeof(short)},
                               {SqlDbType.SmallMoney, typeof(decimal)},
                               {SqlDbType.Structured, typeof(object)},
                               {SqlDbType.Text, typeof(string)},
                               {SqlDbType.Time, typeof(TimeSpan)},
                               {SqlDbType.Timestamp, typeof(byte[])},
                               {SqlDbType.TinyInt, typeof(byte)},
                               {SqlDbType.Udt, typeof(object)},
                               {SqlDbType.UniqueIdentifier, typeof(Guid)},
                               {SqlDbType.VarBinary, typeof(byte[])},
                               {SqlDbType.VarChar, typeof(string)},
                               {SqlDbType.Variant, typeof(object)},
                               {SqlDbType.Xml, typeof(SqlXml)}
                           };
        }

        public static Type SqlTypeToType(string sqlType, bool isNullable)
        {
            var result = (from item in SqlDbTypeMap
                          where item.Key.ToString().ToLower() == sqlType.ToLower()
                          select item).First().Value;
            if (isNullable)
                if (result.IsValueType)
                    result = GetNullableType(result);
            return result;
        }

        public static Type GetNullableType(Type type)
        {
            if (type == null)
                return null;
            //type = Nullable.GetUnderlyingType(type);
            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }
    }
}