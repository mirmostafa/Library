#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;

namespace Library40.Win.Controls.BarChart
{
	public class CDataColumnItem
	{
		public int BoundIndex { get; set; }

		public TypeConverter Converter { get; set; }

		public string DisplayName { get; set; }

		public bool IsReadonly { get; set; }

		public string Name { get; set; }

		public Type ValueType { get; set; }
	}
}