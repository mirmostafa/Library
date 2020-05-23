#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;

namespace Library35.Windows.Controls.BarChart
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