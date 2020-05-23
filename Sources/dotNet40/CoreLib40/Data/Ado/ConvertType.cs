#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

namespace Library40.Data.Ado
{
	public class ConvertType
	{
		// Fields
		public static Dictionary<SqlDbType, Type> SqlDbTypeMap;

		// Methods
		static ConvertType()
		{
			var sqlDbTypeMap = new Dictionary<SqlDbType, Type>
			                   {
				                   {
					                   SqlDbType.BigInt, typeof (long)
				                   },
				                   {
					                   SqlDbType.Binary, typeof (byte[])
				                   },
				                   {
					                   SqlDbType.Bit, typeof (bool)
				                   },
				                   {
					                   SqlDbType.Char, typeof (string)
				                   },
				                   {
					                   SqlDbType.Date, typeof (DateTime)
				                   },
				                   {
					                   SqlDbType.DateTime, typeof (DateTime)
				                   },
				                   {
					                   SqlDbType.DateTime2, typeof (DateTime)
				                   },
				                   {
					                   SqlDbType.DateTimeOffset, typeof (DateTimeOffset)
				                   },
				                   {
					                   SqlDbType.Decimal, typeof (decimal)
				                   },
				                   {
					                   SqlDbType.Float, typeof (double)
				                   },
				                   {
					                   SqlDbType.Image, typeof (byte[])
				                   },
				                   {
					                   SqlDbType.Int, typeof (int)
				                   },
				                   {
					                   SqlDbType.Money, typeof (decimal)
				                   },
				                   {
					                   SqlDbType.NChar, typeof (string)
				                   },
				                   {
					                   SqlDbType.NText, typeof (string)
				                   },
				                   {
					                   SqlDbType.NVarChar, typeof (string)
				                   },
				                   {
					                   SqlDbType.Real, typeof (float)
				                   },
				                   {
					                   SqlDbType.SmallDateTime, typeof (DateTime)
				                   },
				                   {
					                   SqlDbType.SmallInt, typeof (short)
				                   },
				                   {
					                   SqlDbType.SmallMoney, typeof (decimal)
				                   },
				                   {
					                   SqlDbType.Structured, typeof (object)
				                   },
				                   {
					                   SqlDbType.Text, typeof (string)
				                   },
				                   {
					                   SqlDbType.Time, typeof (TimeSpan)
				                   },
				                   {
					                   SqlDbType.Timestamp, typeof (byte[])
				                   },
				                   {
					                   SqlDbType.TinyInt, typeof (byte)
				                   },
				                   {
					                   SqlDbType.Udt, typeof (object)
				                   },
				                   {
					                   SqlDbType.UniqueIdentifier, typeof (Guid)
				                   },
				                   {
					                   SqlDbType.VarBinary, typeof (byte[])
				                   },
				                   {
					                   SqlDbType.VarChar, typeof (string)
				                   },
				                   {
					                   SqlDbType.Variant, typeof (object)
				                   },
				                   {
					                   SqlDbType.Xml, typeof (SqlXml)
				                   }
			                   };
			SqlDbTypeMap = sqlDbTypeMap;
		}

		public static Type SqlTypeToType(string sqlType, bool isNullable)
		{
			var result = (from item in SqlDbTypeMap where item.Key.ToString().ToLower() == sqlType.ToLower() select item).First().Value;
			return result;
		}
	}
}