#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Drawing;
using System.Linq;

namespace Library40.Win.Internals
{
	public class ProChartLine
	{
		public ProChartLine(float[] values, string name, Color color)
		{
			this.Values = values;
			this.Color = color;
			this.Name = name;
		}

		public ProChartLine(float[] values, string name)
		{
			this.Values = values;
			this.Name = name;
#warning check this out
			//this.Color = ColorHelper.RandomColor();
		}

		public float[] Values { get; private set; }
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public float Avrage
		{
			get { return this.Values.Count() == 0 ? 0 : this.Values.Average(); }
		}
	}
}