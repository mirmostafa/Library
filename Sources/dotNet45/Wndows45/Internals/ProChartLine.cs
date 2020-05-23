using System.Drawing;
using System.Linq;

namespace Mohammad.Win.Internals
{
    public class ProChartLine
    {
        public float[] Values { get; }
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public float Avrage { get { return this.Values.Count() == 0 ? 0 : this.Values.Average(); } }

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
    }
}