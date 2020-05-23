#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Library40.Win.Internals
{
	/// <summary>
	///     Tab is a specialized ToolStripButton with extra padding
	/// </summary>
	[DesignerCategory("code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.None)]
	// turn off the ability to add this in the DT, the TabPageSwitcher designer will provide this.
	public class Tab : ToolStripButton
	{
		private readonly Timer timer = new Timer();
		public bool b_active;
		public bool b_fading = true;
		public bool b_on;
		public bool b_selected;
		public int e_opacity = 40;
		public int i_opacity;
		public int o_opacity = 180;

		/// <summary>
		///     Constructor for tab - support all overloads.
		/// </summary>
		public Tab()
		{
			this.Initialize();
		}

		public Tab(string text)
			: base(text, null, null)
		{
			this.Initialize();
		}

		public Tab(Image image)
			: base(null, image, null)
		{
			this.Initialize();
		}

		public Tab(string text, Image image)
			: base(text, image, null)
		{
			this.Initialize();
		}

		public Tab(string text, Image image, EventHandler onClick)
			: base(text, image, onClick)
		{
			this.Initialize();
		}

		public Tab(string text, Image image, EventHandler onClick, string name)
			: base(text, image, onClick, name)
		{
			this.Initialize();
		}

		/// <summary>
		///     Hide the CheckOnClick from the Property Grid so that we can use it for our own purposes.
		/// </summary>
		[DefaultValue(true)]
		public new bool CheckOnClick
		{
			get { return base.CheckOnClick; }
			set { base.CheckOnClick = value; }
		}

		/// <summary>
		///     Specify the default display style to be ImageAndText
		/// </summary>
		protected override ToolStripItemDisplayStyle DefaultDisplayStyle
		{
			get { return ToolStripItemDisplayStyle.ImageAndText; }
		}

		/// <summary>
		///     Add extra internal spacing so we have enough room for our curve.
		/// </summary>
		protected override Padding DefaultPadding
		{
			get { return new Padding(35, 0, 6, 0); }
		}

		/// <summary>
		///     The associated TabStripPage - when Tab is clicked, this TabPage will be selected.
		/// </summary>
		[DefaultValue("null")]
		public TabStripPage TabStripPage { get; set; }

		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;

				var bmpdummy = new Bitmap(100, 100);
				var g = Graphics.FromImage(bmpdummy);
				var textwidth = g.MeasureString(this.Text, this.Font).Width;
				this.Width = Convert.ToInt16(textwidth) + 26;
			}
		}

		/// <summary>
		///     Common initialization code between all CTORs.
		/// </summary>
		private void Initialize()
		{
			this.AutoSize = false;
			this.Width = 150;
			this.CheckOnClick = true;
			// Tab will use the "checked" property in order to represent the "selected tab".
			this.ForeColor = Color.FromArgb(44, 90, 154);
			this.Font = new Font("tahoma", 9);
			this.Margin = new Padding(6, this.Margin.Top, this.Margin.Right, this.Margin.Bottom);
			this.i_opacity = this.o_opacity;
			this.timer.Interval = 1;
			this.timer.Tick += this.timer_Tick;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			this.b_on = true;
			this.b_fading = true;
			this.b_selected = true;
			this.timer.Start();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			this.b_on = false;
			this.b_fading = true;
			this.timer.Start();
			base.OnMouseLeave(e);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (this.b_on)
				if (this.i_opacity > this.e_opacity)
				{
					this.i_opacity -= 20;
					this.Invalidate();
				}
				else
				{
					this.i_opacity = this.e_opacity;
					this.Invalidate();
					this.timer.Stop();
				}
			if (!this.b_on)
				if (this.i_opacity < this.o_opacity)
				{
					this.i_opacity += 8;
					this.Invalidate();
				}
				else
				{
					this.i_opacity = this.o_opacity;
					this.b_fading = false;
					this.Invalidate();
					this.b_selected = false;
					this.timer.Stop();
				}
		}
	}
}