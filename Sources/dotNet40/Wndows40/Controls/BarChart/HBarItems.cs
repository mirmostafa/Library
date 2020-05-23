#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Library40.Win.Controls.BarChart
{
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class HBarItems : IList<HBarItem>, ICollection<HBarItem>, IEnumerable<HBarItem>, IEnumerable
	{
		#region DrawingModes enum
		public enum DrawingModes
		{
			Glass,
			Rubber,
			Solid
		}
		#endregion

		private readonly List<HBarItem> items = new List<HBarItem>();

		private double dABSMaximumValue;
		private double dABSMinimumValue;
		private double dABSTotalValue;
		private double dMaximumValue;
		private double dMinimumValue;
		private double dTotal;

		public HBarItems()
		{
			this.dTotal = this.dMaximumValue = this.dMinimumValue = 0.0;
			this.DrawingMode = DrawingModes.Glass;
		}

		public double ABSMaximum
		{
			get
			{
				if (this.ShouldReCalculate)
					this.ReCalculateAll();
				return this.dABSMaximumValue;
			}
		}

		public double ABSMinimum
		{
			get
			{
				if (this.ShouldReCalculate)
					this.ReCalculateAll();
				return this.dABSMinimumValue;
			}
		}

		public double ABSTotal
		{
			get
			{
				if (this.ShouldReCalculate)
					this.ReCalculateAll();
				return this.dABSTotalValue;
			}
		}

		[Browsable(true), Category("Bar Chart")]
		public int DefaultWidth { get; set; }

		[Browsable(true), Category("Bar Chart")]
		public DrawingModes DrawingMode { get; set; }

		public double Maximum
		{
			get
			{
				if (this.ShouldReCalculate)
					this.ReCalculateAll();
				return this.dMaximumValue;
			}
		}

		public double Minimum
		{
			get
			{
				if (this.ShouldReCalculate)
					this.ReCalculateAll();
				return this.dMinimumValue;
			}
		}

		[Browsable(false)]
		public bool ShouldReCalculate { get; set; }

		public double Total
		{
			get
			{
				if (this.ShouldReCalculate)
					this.ReCalculateAll();
				return this.dTotal;
			}
		}

		#region IList<HBarItem> Members
		public void Add(HBarItem item)
		{
			this.items.Add(item);
			item.Parent = this;
			this.ShouldReCalculate = true;
		}

		public void Clear()
		{
			foreach (var item in this.items)
				item.Parent = null;
			this.items.Clear();
			this.ShouldReCalculate = true;
		}

		public bool Contains(HBarItem item)
		{
			return this.items.Contains(item);
		}

		public void CopyTo(HBarItem[] array, int arrayIndex)
		{
			this.items.CopyTo(array, arrayIndex);
		}

		public IEnumerator<HBarItem> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		public int IndexOf(HBarItem item)
		{
			return this.items.IndexOf(item);
		}

		public void Insert(int index, HBarItem item)
		{
			item.Parent = this;
			this.items.Insert(index, item);
			this.ShouldReCalculate = true;
		}

		public bool Remove(HBarItem item)
		{
			item.Parent = null;
			var flag = this.items.Remove(item);
			this.ShouldReCalculate = true;
			return flag;
		}

		public void RemoveAt(int index)
		{
			this.items[index].Parent = null;
			this.items.RemoveAt(index);
			this.ShouldReCalculate = true;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		public int Count
		{
			get { return this.items.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public HBarItem this[int index]
		{
			get { return this.items[index]; }
			set
			{
				this.items[index].Parent = null;
				this.items[index] = value;
				this.items[index].Parent = this;
				this.ShouldReCalculate = true;
			}
		}
		#endregion

		private void ReCalculateAll()
		{
			if (this.items.Count <= 0)
			{
				this.dMaximumValue = this.dMinimumValue = this.dTotal = 0.0;
				this.dABSMaximumValue = this.dABSMinimumValue = this.dABSTotalValue = 0.0;
			}
			else
			{
				this.dTotal = this.dABSTotalValue = 0.0;
				this.dMaximumValue = this.dMinimumValue = this.items[0].Value;
				this.dABSMaximumValue = this.dABSMinimumValue = Math.Abs(this.items[0].Value);
				foreach (var item in this.items)
				{
					this.dTotal += item.Value;
					this.dABSTotalValue += Math.Abs(item.Value);
					if (item.Value > this.dMaximumValue)
						this.dMaximumValue = item.Value;
					else if (item.Value < this.dMinimumValue)
						this.dMinimumValue = item.Value;
					if (Math.Abs(item.Value) > this.dABSMaximumValue)
						this.dABSMaximumValue = Math.Abs(item.Value);
					else if (Math.Abs(item.Value) < this.dABSMinimumValue)
						this.dABSMinimumValue = Math.Abs(item.Value);
				}
			}
			this.ShouldReCalculate = false;
		}
	}
}