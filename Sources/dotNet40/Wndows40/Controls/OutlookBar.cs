#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library40.Win.Controls
{
	public partial class OutlookBar : UserControl
	{
		#region OutlookBarButtons list
		public class OutlookBarButtons : CollectionBase
		{
			//protected ArrayList List;
			protected OutlookBar parent;

			internal OutlookBarButtons(OutlookBar parent)
			{
				this.parent = parent;
			}

			public OutlookBar Parent
			{
				get { return this.parent; }
			}

			public OutlookBarButton this[int index]
			{
				get { return (OutlookBarButton)this.List[index]; }
			}

			public void Add(OutlookBarButton item)
			{
				if (this.List.Count == 0)
					this.Parent.SelectedButton = item;
				this.List.Add(item);
				item.Parent = this.Parent;
				this.Parent.ButtonlistChanged();
			}

			public OutlookBarButton Add(string text, Image image)
			{
				var b = new OutlookBarButton(this.parent);
				b.Text = text;
				b.Image = image;
				this.Add(b);
				return b;
			}

			public OutlookBarButton Add(string text)
			{
				return this.Add(text, null);
			}

			public OutlookBarButton Add()
			{
				return this.Add();
			}

			public void Remove(OutlookBarButton button)
			{
				this.List.Remove(button);
				this.Parent.ButtonlistChanged();
			}

			public int IndexOf(object value)
			{
				return this.List.IndexOf(value);
			}

			#region handle CollectionBase events
			protected override void OnInsertComplete(int index, object value)
			{
				var b = (OutlookBarButton)value;
				b.Parent = this.parent;
				this.Parent.ButtonlistChanged();
				base.OnInsertComplete(index, value);
			}

			protected override void OnSetComplete(int index, object oldValue, object newValue)
			{
				var b = (OutlookBarButton)newValue;
				b.Parent = this.parent;
				this.Parent.ButtonlistChanged();
				base.OnSetComplete(index, oldValue, newValue);
			}

			protected override void OnClearComplete()
			{
				this.Parent.ButtonlistChanged();
				base.OnClearComplete();
			}
			#endregion handle CollectionBase events
		}
		#endregion OutlookBarButtons list

		#region OutlookBar property definitions
		/// <summary>
		///     property to set the buttonHeigt
		///     default is 30
		/// </summary>
		protected int buttonHeight;

		/// <summary>
		///     buttons contains the list of clickable OutlookBarButtons
		/// </summary>
		protected OutlookBarButtons buttons;

		protected Color gradientButtonDark = Color.FromArgb(178, 193, 140);
		protected Color gradientButtonHoverDark = Color.FromArgb(247, 192, 91);
		protected Color gradientButtonHoverLight = Color.FromArgb(255, 255, 220);
		protected Color gradientButtonLight = Color.FromArgb(234, 240, 207);
		protected Color gradientButtonSelectedDark = Color.FromArgb(239, 150, 21);
		protected Color gradientButtonSelectedLight = Color.FromArgb(251, 230, 148);

		/// <summary>
		///     this variable remembers the button index over which the mouse is moving
		/// </summary>
		protected int hoveringButtonIndex = -1;

		/// <summary>
		///     this variable remembers which button is currently selected
		/// </summary>
		protected OutlookBarButton selectedButton;

		[Description("Specifies the height of each button on the OutlookBar"), Category("Layout")]
		public int ButtonHeight
		{
			get { return this.buttonHeight; }
			set
			{
				if (value > 18)
					this.buttonHeight = value;
				else
					this.buttonHeight = 18;
			}
		}

		[Description("Dark gradient color of the button"), Category("Appearance")]
		public Color GradientButtonNormalDark
		{
			get { return this.gradientButtonDark; }
			set { this.gradientButtonDark = value; }
		}

		[Description("Light gradient color of the button"), Category("Appearance")]
		public Color GradientButtonNormalLight
		{
			get { return this.gradientButtonLight; }
			set { this.gradientButtonLight = value; }
		}

		[Description("Dark gradient color of the button when the mouse is moving over it"), Category("Appearance")]
		public Color GradientButtonHoverDark
		{
			get { return this.gradientButtonHoverDark; }
			set { this.gradientButtonHoverDark = value; }
		}

		[Description("Light gradient color of the button when the mouse is moving over it"), Category("Appearance")]
		public Color GradientButtonHoverLight
		{
			get { return this.gradientButtonHoverLight; }
			set { this.gradientButtonHoverLight = value; }
		}

		[Description("Dark gradient color of the seleced button"), Category("Appearance")]
		public Color GradientButtonSelectedDark
		{
			get { return this.gradientButtonSelectedDark; }
			set { this.gradientButtonSelectedDark = value; }
		}

		[Description("Light gradient color of the seleced button"), Category("Appearance")]
		public Color GradientButtonSelectedLight
		{
			get { return this.gradientButtonSelectedLight; }
			set { this.gradientButtonSelectedLight = value; }
		}

		/// <summary>
		///     when a button is selected programatically, it must update the control
		///     and repaint the buttons
		/// </summary>
		[Browsable(false)]
		public OutlookBarButton SelectedButton
		{
			get { return this.selectedButton; }
			set
			{
				// assign new selected button
				this.PaintSelectedButton(this.selectedButton, value);

				// assign new selected button
				this.selectedButton = value;
			}
		}

		/// <summary>
		///     readonly list of buttons
		/// </summary>
		//[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public OutlookBarButtons Buttons
		{
			get { return this.buttons; }
		}
		#endregion OutlookBar property definitions

		#region OutlookBar events

		#region Delegates
		public delegate void ButtonClickEventHandler(object sender, ButtonClickEventArgs e);
		#endregion

		public new event ButtonClickEventHandler Click;

		[Serializable]
		public class ButtonClickEventArgs : MouseEventArgs
		{
			public readonly OutlookBarButton SelectedButton;

			public ButtonClickEventArgs(OutlookBarButton button, MouseEventArgs evt)
				: base(evt.Button, evt.Clicks, evt.X, evt.Y, evt.Delta)
			{
				this.SelectedButton = button;
			}
		}
		#endregion OutlookBar events

		#region OutlookBar functions
		public OutlookBar()
		{
			this.InitializeComponent();
			this.buttons = new OutlookBarButtons(this);
			this.buttonHeight = 30; // set default to 30
		}

		private void PaintSelectedButton(OutlookBarButton prevButton, OutlookBarButton newButton)
		{
			if (prevButton == newButton)
				return; // no change so return immediately

			var selIdx = -1;
			var valIdx = -1;

			// find the indexes of the previous and new button
			selIdx = this.buttons.IndexOf(prevButton);
			valIdx = this.buttons.IndexOf(newButton);

			// now reset selected button
			// mouse is leaving control, so unhighlight anythign that is highlighted
			var g = Graphics.FromHwnd(this.Handle);
			if (selIdx >= 0)
				// un-highlight current hovering button
				this.buttons[selIdx].PaintButton(g, 1, selIdx * (this.buttonHeight + 1) + 1, false, false);

			if (valIdx >= 0)
				// highlight newly selected button
				this.buttons[valIdx].PaintButton(g, 1, valIdx * (this.buttonHeight + 1) + 1, true, false);
			g.Dispose();
		}

		/// <summary>
		///     returns the button given the coordinates relative to the Outlookbar control
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public OutlookBarButton HitTest(int x, int y)
		{
			var index = (y - 1) / (this.buttonHeight + 1);
			if (index >= 0 && index < this.buttons.Count)
				return this.buttons[index];
			return null;
		}

		/// <summary>
		///     this function will setup the control to cope with changes in the buttonlist
		///     that is, addition and removal of buttons
		/// </summary>
		private void ButtonlistChanged()
		{
			if (!this.DesignMode) // only set sizes automatically at runtime
				this.MaximumSize = new Size(0, this.buttons.Count * (this.buttonHeight + 1) + 1);

			this.Invalidate();
		}
		#endregion OutlookBar functions

		#region OutlookBar control event handlers
		/// <summary>
		///     isResizing is used as a signal, so this method is not called recusively
		///     this prevents a stack overflow
		/// </summary>
		private bool isResizing;

		private void OutlookBar_Load(object sender, EventArgs e)
		{
			// initiate the render style flags of the control
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.Selectable | ControlStyles.UserMouse, true);
		}

		private void OutlookBar_Paint(object sender, PaintEventArgs e)
		{
			var top = 1;
			foreach (OutlookBarButton b in this.Buttons)
			{
				b.PaintButton(e.Graphics, 1, top, b.Equals(this.selectedButton), false);
				top += b.Height + 1;
			}
		}

		private void OutlookBar_Click(object sender, EventArgs e)
		{
			if (!(e is MouseEventArgs))
				return;

			// case to MouseEventArgs so position and mousebutton clicked can be used
			var mea = (MouseEventArgs)e;

			// only continue if left mouse button was clicked
			if (mea.Button != MouseButtons.Left)
				return;

			var index = (mea.Y - 1) / (this.buttonHeight + 1);

			if (index < 0 || index >= this.buttons.Count)
				return;

			var button = this.buttons[index];
			if (button == null)
				return;
			if (!button.Enabled)
				return;

			// ok, all checks passed so assign the new selected button
			// and raise the event
			this.SelectedButton = button;

			var bce = new ButtonClickEventArgs(this.selectedButton, mea);
			if (this.Click != null) // only invoke on left mouse click
				this.Click.Invoke(this, bce);
		}

		private void OutlookBar_DoubleClick(object sender, EventArgs e)
		{
			//TODO: only if you intend to support a doubleclick
			// this can be implemented exactly like the click event
		}

		private void OutlookBar_MouseLeave(object sender, EventArgs e)
		{
			// mouse is leaving control, so unhighlight anything that is highlighted
			if (this.hoveringButtonIndex >= 0)
			{
				// so we need to change the hoveringButtonIndex to the new index
				var g = Graphics.FromHwnd(this.Handle);
				var b1 = this.buttons[this.hoveringButtonIndex];

				// un-highlight current hovering button
				b1.PaintButton(g, 1, this.hoveringButtonIndex * (this.buttonHeight + 1) + 1, b1.Equals(this.selectedButton), false);
				this.hoveringButtonIndex = -1;
				g.Dispose();
			}
		}

		private void OutlookBar_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.None)
			{
				// determine over which button the mouse is moving
				var index = (e.Location.Y - 1) / (this.buttonHeight + 1);
				if (index >= 0 && index < this.buttons.Count)
				{
					if (this.hoveringButtonIndex == index)
						return; // nothing changed so we're done, current button stays highlighted

					// so we need to change the hoveringButtonIndex to the new index
					var g = Graphics.FromHwnd(this.Handle);

					if (this.hoveringButtonIndex >= 0)
					{
						var b1 = this.buttons[this.hoveringButtonIndex];

						// un-highlight current hovering button
						b1.PaintButton(g, 1, this.hoveringButtonIndex * (this.buttonHeight + 1) + 1, b1.Equals(this.selectedButton), false);
					}

					// highlight new hovering button
					var b2 = this.buttons[index];
					b2.PaintButton(g, 1, index * (this.buttonHeight + 1) + 1, b2.Equals(this.selectedButton), true);
					this.hoveringButtonIndex = index; // set to new index
					g.Dispose();
				}
				else
					// no hovering button, so un-highlight all.
					if (this.hoveringButtonIndex >= 0)
					{
						// so we need to change the hoveringButtonIndex to the new index
						var g = Graphics.FromHwnd(this.Handle);
						var b1 = this.buttons[this.hoveringButtonIndex];

						// un-highlight current hovering button
						b1.PaintButton(g, 1, this.hoveringButtonIndex * (this.buttonHeight + 1) + 1, b1.Equals(this.selectedButton), false);
						this.hoveringButtonIndex = -1;
						g.Dispose();
					}
			}
		}

		private void OutlookBar_Resize(object sender, EventArgs e)
		{
			// only set sizes automatically at runtime
			if (!this.DesignMode)
				if (!this.isResizing)
				{
					this.isResizing = true;
					if ((this.Height - 1) % (this.buttonHeight + 1) > 0)
						this.Height = ((this.Height - 1) / (this.buttonHeight + 1)) * (this.buttonHeight + 1) + 1;
					this.Invalidate();
					this.isResizing = false;
				}
		}
		#endregion OutlookBar control event handlers

		/// <summary>
		/// the OutlookBarButtons class contains the list of buttons
		/// it manages adding and removing buttons, and updates the Outlookbar control
		/// respectively. Note that this is a class, not a control!
		/// </summary>
	}

	public class OutlookBarButton // : IComponent
	{
		private bool enabled = true;
		protected int height;

		protected Image image;
		protected OutlookBar parent;

		protected object tag;
		protected string text;

		public OutlookBarButton()
		{
			this.parent = new OutlookBar(); // set it to a dummy outlookbar control
			this.text = "";
		}

		public OutlookBarButton(OutlookBar parent)
		{
			this.parent = parent;
			this.text = "";
		}

		[Description("Indicates wether the button is enabled"), Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		[Description("The image that will be displayed on the button"), Category("Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Image Image
		{
			get { return this.image; }
			set
			{
				this.image = value;
				this.parent.Invalidate();
			}
		}

		[Description("User-defined data to be associated with the button"), Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public object Tag
		{
			get { return this.tag; }
			set { this.tag = value; }
		}

		internal OutlookBar Parent
		{
			get { return this.parent; }
			set { this.parent = value; }
		}

		[Description("The text that will be displayed on the button"), Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Text
		{
			get { return this.text; }
			set
			{
				this.text = value;
				this.parent.Invalidate();
			}
		}

		public int Height
		{
			get { return this.parent == null ? 30 : this.parent.ButtonHeight; }
		}

		public int Width
		{
			get { return this.parent == null ? 60 : this.parent.Width - 2; }
		}

		/// <summary>
		///     the outlook button will paint itself on its container (the OutlookBar)
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="IsSelected"></param>
		/// <param name="IsHovering"></param>
		public void PaintButton(Graphics graphics, int x, int y, bool IsSelected, bool IsHovering)
		{
			Brush br;
			var rect = new Rectangle(0, y, this.Width, this.Height);
			if (this.enabled)
				if (IsSelected)
					if (IsHovering)
						br = new LinearGradientBrush(rect, this.parent.GradientButtonSelectedDark, this.parent.GradientButtonSelectedLight, 90f);
					else
						br = new LinearGradientBrush(rect, this.parent.GradientButtonSelectedLight, this.parent.GradientButtonSelectedDark, 90f);
				else if (IsHovering)
					br = new LinearGradientBrush(rect, this.parent.GradientButtonHoverLight, this.parent.GradientButtonHoverDark, 90f);
				else
					br = new LinearGradientBrush(rect, this.parent.GradientButtonNormalLight, this.parent.GradientButtonNormalDark, 90f);
			else
				br = new LinearGradientBrush(rect, this.parent.GradientButtonNormalLight, this.parent.GradientButtonNormalDark, 90f);

			graphics.FillRectangle(br, x, y, this.Width, this.Height);
			br.Dispose();

			if (this.text.Length > 0)
				graphics.DrawString(this.Text, this.parent.Font, Brushes.Black, 36, y + this.Height / 2 - this.parent.Font.Height / 2);

			if (this.image != null)
				graphics.DrawImage(this.image, 36 / 2 - this.image.Width / 2, y + this.Height / 2 - this.image.Height / 2, this.image.Width, this.image.Height);
		}
	}
}