#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Library40.Wpf.Windows.Controls
{
	/// <summary>
	///     Interaction logic for CicularMarque.xaml
	/// </summary>
	public partial class CicularMarque
	{
		public CicularMarque()
		{
			this.InitializeComponent();
		}

		public void Start()
		{
			this.DrawCanvas();
			this.canvas2.Visibility = Visibility.Visible;
			this.spin.BeginAnimation(RotateTransform.AngleProperty,
				new DoubleAnimation
				{
					From = 0,
					To = 360,
					RepeatBehavior = RepeatBehavior.Forever,
					SpeedRatio = 1
				});
		}

		public void Stop()
		{
			this.canvas2.Visibility = Visibility.Collapsed;
		}

		private void DrawCanvas()
		{
			for (var i = 0; i < 12; i++)
				this.canvas1.Children.Add(new Line
				                          {
					                          X1 = 50,
					                          X2 = 50,
					                          Y1 = 0,
					                          Y2 = 20,
					                          StrokeThickness = 5,
					                          Stroke = Brushes.SkyBlue,
					                          Width = this.Width,
					                          Height = this.Height,
					                          VerticalAlignment = VerticalAlignment.Center,
					                          HorizontalAlignment = HorizontalAlignment.Center,
					                          RenderTransformOrigin = new Point(.5, .5),
					                          RenderTransform = new RotateTransform(i * 30),
					                          Opacity = (double)i / 12
				                          });
		}
	}
}