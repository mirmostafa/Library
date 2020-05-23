#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Library35.Windows.Properties;

namespace Library35.Windows.Forms.Internals
{
	internal class MsgBoxExDialogForm : Form
	{
		[AccessedThroughProperty("BevelButtons")]
		private Label _BevelButtons;

		[AccessedThroughProperty("BevelExpanderFooter")]
		private Label _BevelExpanderFooter;

		[AccessedThroughProperty("BevelFooter")]
		private Label _BevelFooter;

		[AccessedThroughProperty("Button1")]
		private Button _Button1;

		[AccessedThroughProperty("Button10")]
		private Button _Button10;

		[AccessedThroughProperty("Button11")]
		private Button _Button11;

		[AccessedThroughProperty("Button2")]
		private Button _Button2;

		[AccessedThroughProperty("Button3")]
		private Button _Button3;

		[AccessedThroughProperty("Button4")]
		private Button _Button4;

		[AccessedThroughProperty("Button5")]
		private Button _Button5;

		[AccessedThroughProperty("Button6")]
		private Button _Button6;

		[AccessedThroughProperty("Button7")]
		private Button _Button7;

		[AccessedThroughProperty("Button8")]
		private Button _Button8;

		[AccessedThroughProperty("Button9")]
		private Button _Button9;

		[AccessedThroughProperty("ButtonExpander")]
		private ChevronButton _ButtonExpander;

		private Button[] _Buttons;

		[AccessedThroughProperty("CheckBox")]
		private CheckBox _CheckBox;

		private string _CollapsedControlText;

		private Control _Control;

		private bool _DrawGradient;

		private bool _Expanded;

		private string _ExpandedControlText;

		[AccessedThroughProperty("FlowLayoutPanelButtons")]
		private FlowLayoutPanel _FlowLayoutPanelButtons;

		[AccessedThroughProperty("FlowLayoutPanelButtonsLeft")]
		private FlowLayoutPanel _FlowLayoutPanelButtonsLeft;

		private MsgBoxExDialogIcon _FooterIcon;

		private Color _GradientBegin;

		private Color _GradientEnd;

		[AccessedThroughProperty("LabelContent")]
		private Label _LabelContent;

		[AccessedThroughProperty("LabelExpandedContent")]
		private Label _LabelExpandedContent;

		[AccessedThroughProperty("LabelExpandedFooter")]
		private Label _LabelExpandedFooter;

		[AccessedThroughProperty("LabelFooter")]
		private Label _LabelFooter;

		[AccessedThroughProperty("LabelIcon")]
		private Label _LabelIcon;

		[AccessedThroughProperty("LabelIconFooter")]
		private Label _LabelIconFooter;

		[AccessedThroughProperty("LabelTitle")]
		private Label _LabelTitle;

		private MsgBoxExDialogIcon _MainIcon;

		[AccessedThroughProperty("PanelButtons")]
		private Panel _PanelButtons;

		[AccessedThroughProperty("PanelExpandedFooter")]
		private Panel _PanelExpandedFooter;

		[AccessedThroughProperty("PanelFooter")]
		private Panel _PanelFooter;

		private ISound _Sound;

		[AccessedThroughProperty("TableLayoutPanel")]
		private TableLayoutPanel _TableLayoutPanel;

		[AccessedThroughProperty("TableLayoutPanelButtons")]
		private TableLayoutPanel _TableLayoutPanelButtons;

		[AccessedThroughProperty("TableLayoutPanelContent")]
		private TableLayoutPanel _TableLayoutPanelContent;

		[AccessedThroughProperty("TableLayoutPanelContents")]
		private TableLayoutPanel _TableLayoutPanelContents;

		[AccessedThroughProperty("TableLayoutPanelExpanderFooter")]
		private TableLayoutPanel _TableLayoutPanelExpanderFooter;

		[AccessedThroughProperty("TableLayoutPanelFooter")]
		private TableLayoutPanel _TableLayoutPanelFooter;

		private MsgBoxExDialogButton[] _VButtons;

		private bool _CanCancel;

#pragma warning disable 649
		// ReSharper disable InconsistentNaming
		private readonly IContainer components;
		// ReSharper restore InconsistentNaming
#pragma warning restore 649

		internal virtual Label BevelButtons
		{
			[DebuggerNonUserCode] get { return this._BevelButtons; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._BevelButtons = value; }
		}

		internal virtual Label BevelExpanderFooter
		{
			[DebuggerNonUserCode] get { return this._BevelExpanderFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._BevelExpanderFooter = value; }
		}

		internal virtual Label BevelFooter
		{
			[DebuggerNonUserCode] get { return this._BevelFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._BevelFooter = value; }
		}

		internal virtual Button Button1
		{
			[DebuggerNonUserCode] get { return this._Button1; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button1 != null)
					this._Button1.Click -= this.ButtonClick;
				this._Button1 = value;
				if (this._Button1 != null)
					this._Button1.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button10
		{
			[DebuggerNonUserCode] get { return this._Button10; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button10 != null)
					this._Button10.Click -= this.ButtonClick;
				this._Button10 = value;
				if (this._Button10 != null)
					this._Button10.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button11
		{
			[DebuggerNonUserCode] get { return this._Button11; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button11 != null)
					this._Button11.Click -= this.ButtonClick;
				this._Button11 = value;
				if (this._Button11 != null)
					this._Button11.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button2
		{
			[DebuggerNonUserCode] get { return this._Button2; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button2 != null)
					this._Button2.Click -= this.ButtonClick;
				this._Button2 = value;
				if (this._Button2 != null)
					this._Button2.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button3
		{
			[DebuggerNonUserCode] get { return this._Button3; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button3 != null)
					this._Button3.Click -= this.ButtonClick;
				this._Button3 = value;
				if (this._Button3 != null)
					this._Button3.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button4
		{
			[DebuggerNonUserCode] get { return this._Button4; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button4 != null)
					this._Button4.Click -= this.ButtonClick;
				this._Button4 = value;
				if (this._Button4 != null)
					this._Button4.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button5
		{
			[DebuggerNonUserCode] get { return this._Button5; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button5 != null)
					this._Button5.Click -= this.ButtonClick;
				this._Button5 = value;
				if (this._Button5 != null)
					this._Button5.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button6
		{
			[DebuggerNonUserCode] get { return this._Button6; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button6 != null)
					this._Button6.Click -= this.ButtonClick;
				this._Button6 = value;
				if (this._Button6 != null)
					this._Button6.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button7
		{
			[DebuggerNonUserCode] get { return this._Button7; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button7 != null)
					this._Button7.Click -= this.ButtonClick;
				this._Button7 = value;
				if (this._Button7 != null)
					this._Button7.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button8
		{
			[DebuggerNonUserCode] get { return this._Button8; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button8 != null)
					this._Button8.Click -= this.ButtonClick;
				this._Button8 = value;
				if (this._Button8 != null)
					this._Button8.Click += this.ButtonClick;
			}
		}

		internal virtual Button Button9
		{
			[DebuggerNonUserCode] get { return this._Button9; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._Button9 != null)
					this._Button9.Click -= this.ButtonClick;
				this._Button9 = value;
				if (this._Button9 != null)
					this._Button9.Click += this.ButtonClick;
			}
		}

		internal virtual ChevronButton ButtonExpander
		{
			[DebuggerNonUserCode] get { return this._ButtonExpander; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._ButtonExpander != null)
					this._ButtonExpander.Click -= this.ButtonExpanderClick;
				this._ButtonExpander = value;
				if (this._ButtonExpander != null)
					this._ButtonExpander.Click += this.ButtonExpanderClick;
			}
		}

		// ReSharper disable InconsistentNaming
		private Button[] Buttons // ReSharper restore InconsistentNaming
		{
			get
			{
				if (this._Buttons == null)
					this._Buttons = new[]
					                {
						                this.Button1, this.Button2, this.Button3, this.Button4, this.Button5, this.Button6, this.Button7, this.Button8, this.Button9, this.Button10, this.Button11
					                };
				return this._Buttons;
			}
		}

		public string Caption
		{
			get { return this.Text; }
			set { this.Text = value; }
		}

		internal virtual CheckBox CheckBox
		{
			[DebuggerNonUserCode] get { return this._CheckBox; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._CheckBox = value; }
		}

		public CheckState CheckBoxState
		{
			get { return this.CheckBox.CheckState; }
			set { this.CheckBox.CheckState = value; }
		}

		public string CheckBoxText
		{
			get { return this.CheckBox.Text; }
			set { this.CheckBox.Text = value; }
		}

		public string CollapsedControlText
		{
			get { return string.IsNullOrEmpty(this._CollapsedControlText) ? Resources.CollapsedText : this._CollapsedControlText; }
			set
			{
				this._CollapsedControlText = value;
				this.SetExpanderText();
			}
		}

		public string Content
		{
			get { return this.LabelContent.Text; }
			set
			{
				this.LabelContent.Text = value;
				if (string.IsNullOrEmpty(value))
				{
					this.LabelContent.Visible = false;
					this.TableLayoutPanelContent.RowStyles[1] = new RowStyle(SizeType.Absolute, 0f);
				}
				else
				{
					this.TableLayoutPanelContent.RowStyles[1] = new RowStyle(SizeType.AutoSize);
					this.LabelContent.Visible = true;
				}
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var createParams = base.CreateParams;
				if (!this._CanCancel)
					createParams.ClassStyle |= 0x200;
				createParams.ClassStyle |= 0x00020000;
				return createParams;
			}
		}

		public Control CustomControl
		{
			get { return this._Control; }
			set
			{
				this.TableLayoutPanelContents.Controls.Clear();
				if (value == null)
				{
					this.TableLayoutPanelContents.Visible = false;
					this.TableLayoutPanelContent.RowStyles[3] = new RowStyle(SizeType.Absolute, 0f);
				}
				else
				{
					this.TableLayoutPanelContents.Controls.Add(value, 0, 0);
					this.TableLayoutPanelContent.RowStyles[3] = new RowStyle(SizeType.AutoSize);
					this.TableLayoutPanelContents.Visible = true;
				}
				this._Control = value;
			}
		}

		public MsgBoxExDialogDefaultButton DefaultButton { get; set; }

		public bool Expanded
		{
			get { return this._Expanded; }
			set
			{
				this._Expanded = value;
				this.ButtonExpander.Expanded = value;
				this.SetExpanderText();
				if (this.ExpandFooterArea)
					if (!value)
					{
						this.TableLayoutPanelExpanderFooter.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
						this.TableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Absolute, 0f);
					}
					else
					{
						this.TableLayoutPanelExpanderFooter.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
						this.TableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.AutoSize);
					}
				else if (value)
				{
					this.LabelExpandedContent.Visible = true;
					this.TableLayoutPanelContent.RowStyles[2] = new RowStyle(SizeType.AutoSize);
				}
				else
				{
					this.TableLayoutPanelContent.RowStyles[2] = new RowStyle(SizeType.Absolute, 0f);
					this.LabelExpandedContent.Visible = false;
				}
			}
		}

		public string ExpandedControlText
		{
			get { return string.IsNullOrEmpty(this._ExpandedControlText) ? Resources.ExpandedText : this._ExpandedControlText; }
			set
			{
				this._ExpandedControlText = value;
				this.SetExpanderText();
			}
		}

		public string ExpandedInformation
		{
			get { return this.LabelExpandedContent.Text; }
			set
			{
				this.LabelExpandedContent.Text = value;
				this.LabelExpandedFooter.Text = value;
				this.ShowButtonExpander = !string.IsNullOrEmpty(value);
				this.Expanded = !string.IsNullOrEmpty(value);
			}
		}

		public bool ExpandFooterArea { get; set; }

		internal virtual FlowLayoutPanel FlowLayoutPanelButtons
		{
			[DebuggerNonUserCode] get { return this._FlowLayoutPanelButtons; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._FlowLayoutPanelButtons = value; }
		}

		internal virtual FlowLayoutPanel FlowLayoutPanelButtonsLeft
		{
			[DebuggerNonUserCode] get { return this._FlowLayoutPanelButtonsLeft; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._FlowLayoutPanelButtonsLeft = value; }
		}

		public MsgBoxExDialogIcon FooterIcon
		{
			get { return this._FooterIcon; }
			set
			{
				this._FooterIcon = value;
				switch (this._FooterIcon)
				{
					case MsgBoxExDialogIcon.Information:
						this.FooterImage = MsgBoxExDialogSmallIcon.Information;
						break;

					case MsgBoxExDialogIcon.Question:
						this.FooterImage = MsgBoxExDialogSmallIcon.Question;
						break;

					case MsgBoxExDialogIcon.Warning:
						this.FooterImage = MsgBoxExDialogSmallIcon.Warning;
						break;

					case MsgBoxExDialogIcon.Error:
						this.FooterImage = MsgBoxExDialogSmallIcon.Error;
						break;

					case MsgBoxExDialogIcon.SecuritySuccess:
						this.FooterImage = MsgBoxExDialogSmallIcon.SecuritySuccess;
						break;

					case MsgBoxExDialogIcon.SecurityQuestion:
						this.FooterImage = MsgBoxExDialogSmallIcon.SecurityQuestion;
						break;

					case MsgBoxExDialogIcon.SecurityWarning:
						this.FooterImage = MsgBoxExDialogSmallIcon.SecurityWarning;
						break;

					case MsgBoxExDialogIcon.SecurityError:
						this.FooterImage = MsgBoxExDialogSmallIcon.SecurityError;
						break;

					case MsgBoxExDialogIcon.SecurityShield:
						this.FooterImage = MsgBoxExDialogSmallIcon.Security;
						break;

					case MsgBoxExDialogIcon.SecurityShieldBlue:
						this.FooterImage = MsgBoxExDialogSmallIcon.Security;
						break;

					case MsgBoxExDialogIcon.SecurityShieldGray:
						this.FooterImage = MsgBoxExDialogSmallIcon.Security;
						break;

					default:
						this.FooterImage = null;
						break;
				}
			}
		}

		public Image FooterImage
		{
			get { return this.LabelIconFooter.Image; }
			set
			{
				this.LabelIconFooter.Image = value;
				if (value == null)
				{
					this.TableLayoutPanelFooter.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
					var padding4 = new Padding(0x10, this.LabelFooter.Margin.Top, this.LabelFooter.Margin.Right, this.LabelFooter.Margin.Bottom);
					this.LabelFooter.Margin = padding4;
				}
				else
				{
					this.TableLayoutPanelFooter.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
					var padding = new Padding(4, this.LabelFooter.Margin.Top, this.LabelFooter.Margin.Right, this.LabelFooter.Margin.Bottom);
					this.LabelFooter.Margin = padding;
				}
			}
		}

		public string FooterText
		{
			get { return this.LabelFooter.Text; }
			set
			{
				this.LabelFooter.Text = value;
				if (value == null)
				{
					this.TableLayoutPanelFooter.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
					this.TableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Absolute, 0f);
				}
				else
				{
					this.TableLayoutPanelFooter.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
					this.TableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.AutoSize);
				}
			}
		}

		public Image Image
		{
			get { return this.LabelIcon.Image; }
			set
			{
				this.LabelIcon.Image = value;
				this.TableLayoutPanelContent.ColumnStyles[0] = value == null ? new ColumnStyle(SizeType.Absolute, 0f) : new ColumnStyle(SizeType.AutoSize);
			}
		}

		internal virtual Label LabelContent
		{
			[DebuggerNonUserCode] get { return this._LabelContent; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._LabelContent = value; }
		}

		internal virtual Label LabelExpandedContent
		{
			[DebuggerNonUserCode] get { return this._LabelExpandedContent; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._LabelExpandedContent = value; }
		}

		internal virtual Label LabelExpandedFooter
		{
			[DebuggerNonUserCode] get { return this._LabelExpandedFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._LabelExpandedFooter = value; }
		}

		internal virtual Label LabelFooter
		{
			[DebuggerNonUserCode] get { return this._LabelFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._LabelFooter = value; }
		}

		internal virtual Label LabelIcon
		{
			[DebuggerNonUserCode] get { return this._LabelIcon; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._LabelIcon != null)
					this._LabelIcon.Paint -= this.LabelIconPaint;
				this._LabelIcon = value;
				if (this._LabelIcon != null)
					this._LabelIcon.Paint += this.LabelIconPaint;
			}
		}

		internal virtual Label LabelIconFooter
		{
			[DebuggerNonUserCode] get { return this._LabelIconFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._LabelIconFooter = value; }
		}

		internal virtual Label LabelTitle
		{
			[DebuggerNonUserCode] get { return this._LabelTitle; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._LabelTitle = value; }
		}

		public MsgBoxExDialogIcon MainIcon
		{
			get { return this._MainIcon; }
			set
			{
				this._MainIcon = value;
				this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 2);
				this.LabelTitle.ForeColor = SystemColors.HotTrack;
				this._DrawGradient = false;
				this.Sound = MsgBoxExDialogSound.Default;
				switch (this._MainIcon)
				{
					case MsgBoxExDialogIcon.Information:
						this.Image = MsgBoxExDialogBigIcon.Information;
						this.Sound = MsgBoxExDialogSound.Information;
						break;

					case MsgBoxExDialogIcon.Question:
						this.Image = MsgBoxExDialogBigIcon.Question;
						this.Sound = MsgBoxExDialogSound.Question;
						break;

					case MsgBoxExDialogIcon.Warning:
						this.Image = MsgBoxExDialogBigIcon.Warning;
						this.Sound = MsgBoxExDialogSound.Warning;
						break;

					case MsgBoxExDialogIcon.Error:
						this.Image = MsgBoxExDialogBigIcon.Error;
						this.Sound = MsgBoxExDialogSound.Error;
						break;

					case MsgBoxExDialogIcon.SecuritySuccess:
						this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 1);
						this.Image = MsgBoxExDialogBigIcon.SecuritySuccess;
						this._DrawGradient = true;
						this._GradientBegin = Color.FromArgb(0x15, 0x76, 0x15);
						this._GradientEnd = Color.FromArgb(0x39, 150, 0x3f);
						this.LabelTitle.ForeColor = Color.White;
						this.Sound = MsgBoxExDialogSound.Information;
						break;

					case MsgBoxExDialogIcon.SecurityQuestion:
						this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 1);
						this.Image = MsgBoxExDialogBigIcon.SecurityQuestion;
						this._DrawGradient = true;
						this._GradientBegin = Color.FromArgb(0, 0xb1, 0xf2);
						this._GradientEnd = Color.FromArgb(0x48, 0xcd, 0xfe);
						this.LabelTitle.ForeColor = Color.White;
						this.Sound = MsgBoxExDialogSound.Question;
						break;

					case MsgBoxExDialogIcon.SecurityWarning:
						this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 1);
						this.Image = MsgBoxExDialogBigIcon.SecurityWarning;
						this._DrawGradient = true;
						this._GradientBegin = Color.FromArgb(0xf2, 0xb1, 0);
						this._GradientEnd = Color.FromArgb(0xfe, 0xcd, 0x48);
						this.LabelTitle.ForeColor = Color.Black;
						this.Sound = MsgBoxExDialogSound.Warning;
						break;

					case MsgBoxExDialogIcon.SecurityError:
						this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 1);
						this.Image = MsgBoxExDialogBigIcon.SecurityError;
						this._DrawGradient = true;
						this._GradientBegin = Color.FromArgb(0xac, 1, 0);
						this._GradientEnd = Color.FromArgb(0xe3, 1, 0);
						this.LabelTitle.ForeColor = Color.White;
						this.Sound = MsgBoxExDialogSound.Error;
						break;

					case MsgBoxExDialogIcon.SecurityShield:
						this.Image = MsgBoxExDialogBigIcon.Security;
						this.Sound = MsgBoxExDialogSound.Security;
						break;

					case MsgBoxExDialogIcon.SecurityShieldBlue:
						this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 1);
						this.Image = MsgBoxExDialogBigIcon.Security;
						this._DrawGradient = true;
						this._GradientBegin = Color.FromArgb(4, 80, 130);
						this._GradientEnd = Color.FromArgb(0x1c, 120, 0x85);
						this.LabelTitle.ForeColor = Color.White;
						this.Sound = MsgBoxExDialogSound.Security;
						break;

					case MsgBoxExDialogIcon.SecurityShieldGray:
						this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 1);
						this.Image = MsgBoxExDialogBigIcon.Security;
						this._DrawGradient = true;
						this._GradientBegin = Color.FromArgb(0x9d, 0x8f, 0x85);
						this._GradientEnd = Color.FromArgb(0xa4, 0x98, 0x90);
						this.LabelTitle.ForeColor = Color.White;
						this.Sound = MsgBoxExDialogSound.Security;
						break;

					default:
						this.Image = null;
						break;
				}
			}
		}

		internal virtual Panel PanelButtons
		{
			[DebuggerNonUserCode] get { return this._PanelButtons; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._PanelButtons = value; }
		}

		internal virtual Panel PanelExpandedFooter
		{
			[DebuggerNonUserCode] get { return this._PanelExpandedFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._PanelExpandedFooter = value; }
		}

		internal virtual Panel PanelFooter
		{
			[DebuggerNonUserCode] get { return this._PanelFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._PanelFooter = value; }
		}

		public bool ShowButtonExpander
		{
			get { return this.ButtonExpander.Visible; }
			set
			{
				if (value)
				{
					this.SetExpanderText();
					this.TableLayoutPanelButtons.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
					this.FlowLayoutPanelButtonsLeft.Visible = true;
					this.ButtonExpander.Visible = true;
				}
				else
				{
					this.ButtonExpander.Visible = false;
					if (!this.ShowCheckBox)
					{
						this.FlowLayoutPanelButtonsLeft.Visible = false;
						this.TableLayoutPanelButtons.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
					}
				}
			}
		}

		public bool ShowCheckBox
		{
			get { return this.CheckBox.Visible; }
			set
			{
				if (value)
				{
					this.TableLayoutPanelButtons.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
					this.FlowLayoutPanelButtonsLeft.Visible = true;
					this.CheckBox.Visible = true;
				}
				else
				{
					this.CheckBox.Visible = false;
					if (!this.ShowButtonExpander)
					{
						this.FlowLayoutPanelButtonsLeft.Visible = false;
						this.TableLayoutPanelButtons.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
					}
				}
			}
		}

		public ISound Sound
		{
			get { return this._Sound ?? MsgBoxExDialogSound.Default; }
			set { this._Sound = value; }
		}

		internal virtual TableLayoutPanel TableLayoutPanel
		{
			[DebuggerNonUserCode] get { return this._TableLayoutPanel; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._TableLayoutPanel = value; }
		}

		internal virtual TableLayoutPanel TableLayoutPanelButtons
		{
			[DebuggerNonUserCode] get { return this._TableLayoutPanelButtons; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._TableLayoutPanelButtons = value; }
		}

		internal virtual TableLayoutPanel TableLayoutPanelContent
		{
			[DebuggerNonUserCode] get { return this._TableLayoutPanelContent; }
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				if (this._TableLayoutPanelContent != null)
					this._TableLayoutPanelContent.CellPaint -= this.TableLayoutPanelContentCellPaint;
				this._TableLayoutPanelContent = value;
				if (this._TableLayoutPanelContent != null)
					this._TableLayoutPanelContent.CellPaint += this.TableLayoutPanelContentCellPaint;
			}
		}

		internal virtual TableLayoutPanel TableLayoutPanelContents
		{
			[DebuggerNonUserCode] get { return this._TableLayoutPanelContents; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._TableLayoutPanelContents = value; }
		}

		internal virtual TableLayoutPanel TableLayoutPanelExpanderFooter
		{
			[DebuggerNonUserCode] get { return this._TableLayoutPanelExpanderFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._TableLayoutPanelExpanderFooter = value; }
		}

		internal virtual TableLayoutPanel TableLayoutPanelFooter
		{
			[DebuggerNonUserCode] get { return this._TableLayoutPanelFooter; }
			[MethodImpl(MethodImplOptions.Synchronized)] [DebuggerNonUserCode] set { this._TableLayoutPanelFooter = value; }
		}

		public string Title
		{
			get { return this.LabelTitle.Text; }
			set
			{
				this.LabelTitle.Text = value;
				if (string.IsNullOrEmpty(value))
				{
					this.LabelTitle.Visible = false;
					this.TableLayoutPanelContent.RowStyles[0] = new RowStyle(SizeType.Absolute, 0f);
				}
				else
				{
					this.TableLayoutPanelContent.RowStyles[0] = new RowStyle(SizeType.AutoSize);
					this.LabelTitle.Visible = true;
				}
			}
		}

		public MsgBoxExDialogButton[] MsgBoxExButtons
		{
			get { return this._VButtons; }
			set
			{
				if (value == null)
					value = new MsgBoxExDialogButton[0];
				this._VButtons = value;
				if (value.Length == 0)
				{
					this.TableLayoutPanelButtons.ColumnStyles[1] = new ColumnStyle(SizeType.Absolute, 0f);
					if (this.TableLayoutPanelButtons.ColumnStyles[0].SizeType == SizeType.Absolute)
						this.TableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Absolute, 0f);
				}
				else
				{
					this.TableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.AutoSize);
					this.TableLayoutPanelButtons.ColumnStyles[1] = new ColumnStyle(SizeType.AutoSize);
				}
				var num2 = this.Buttons.Length - 1;
				for (var i = 0; i <= num2; i++)
				{
					var button = this.Buttons[i];
					button.Visible = i < value.Length;
					if (i >= value.Length)
						continue;
					if (value[i].MsgBoxExDialogResult == MsgBoxExDialogResult.Cancel)
					{
						this._CanCancel = true;
						this.CancelButton = this.Buttons[i];
					}
					button.Text = value[i].UseCustomText ? value[i].Text : MsgBoxExExtensions.GetButtonName(value[i].MsgBoxExDialogResult);
					button.Tag = value[i];
				}
				if (value.Length != 1)
					return;
				this._CanCancel = true;
				this.CancelButton = this.Buttons[0];
				this.AcceptButton = this.Buttons[0];
			}
		}

		public MsgBoxExDialogForm()
		{
			this.FormClosing += this.MsgBoxExDialogFormFormClosing;
			this.Shown += this.MsgBoxExDialogFormShown;
			this.InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;
			this.LabelTitle.Font = new Font(this.Font.FontFamily, this.Font.Size * 1.5f, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
			this.TableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Absolute, 0f);
			this.TableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Absolute, 0f);
			this.TableLayoutPanelButtons.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
			this.MsgBoxExButtons = null;
			this.FlowLayoutPanelButtonsLeft.Visible = false;
			this.TableLayoutPanelContent.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
			this.TableLayoutPanelContent.RowStyles[0] = new RowStyle(SizeType.Absolute, 0f);
			this.LabelTitle.Visible = false;
			this.TableLayoutPanelContent.RowStyles[1] = new RowStyle(SizeType.Absolute, 0f);
			this.LabelContent.Visible = false;
			this.TableLayoutPanelContent.RowStyles[2] = new RowStyle(SizeType.Absolute, 0f);
			this.LabelExpandedContent.Visible = false;
			this.TableLayoutPanelContent.RowStyles[3] = new RowStyle(SizeType.Absolute, 0f);
			this.TableLayoutPanelContents.Visible = false;
			this.CheckBox.Visible = false;
			this.ButtonExpander.Visible = false;
			this.TableLayoutPanelFooter.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
			this.TableLayoutPanelExpanderFooter.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
		}

		private void ButtonClick(object sender, EventArgs e)
		{
			var button = (Button)sender;
			var tag = (MsgBoxExDialogButton)button.Tag;
			this.Tag = tag;
			tag.RaiseClickEvent(RuntimeHelpers.GetObjectValue(sender), e);
			if (tag.MsgBoxExDialogResult != MsgBoxExDialogResult.None)
				this.DialogResult = DialogResult.OK;
		}

		private void ButtonExpanderClick(object sender, EventArgs e)
		{
			this.Expanded ^= true;
		}

		[DebuggerNonUserCode]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && (this.components != null))
					this.components.Dispose();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		private void InitializeComponent()
		{
#pragma warning disable 168
			var manager = new ComponentResourceManager(typeof (MsgBoxExDialogForm));
#pragma warning restore 168
			this.TableLayoutPanelContent = new TableLayoutPanel();
			this.LabelIcon = new Label();
			this.LabelTitle = new Label();
			this.LabelContent = new Label();
			this.TableLayoutPanelContents = new TableLayoutPanel();
			this.LabelExpandedContent = new Label();
			this.PanelExpandedFooter = new Panel();
			this.TableLayoutPanelExpanderFooter = new TableLayoutPanel();
			this.LabelExpandedFooter = new Label();
			this.BevelExpanderFooter = new Label();
			this.PanelFooter = new Panel();
			this.TableLayoutPanelFooter = new TableLayoutPanel();
			this.LabelIconFooter = new Label();
			this.LabelFooter = new Label();
			this.BevelFooter = new Label();
			this.PanelButtons = new Panel();
			this.TableLayoutPanelButtons = new TableLayoutPanel();
			this.FlowLayoutPanelButtons = new FlowLayoutPanel();
			this.Button1 = new Button();
			this.Button2 = new Button();
			this.Button3 = new Button();
			this.Button4 = new Button();
			this.Button5 = new Button();
			this.Button6 = new Button();
			this.Button7 = new Button();
			this.Button8 = new Button();
			this.Button9 = new Button();
			this.Button10 = new Button();
			this.Button11 = new Button();
			this.FlowLayoutPanelButtonsLeft = new FlowLayoutPanel();
			this.ButtonExpander = new ChevronButton();
			this.CheckBox = new CheckBox();
			this.BevelButtons = new Label();
			this.TableLayoutPanel = new TableLayoutPanel();
			this.TableLayoutPanelContent.SuspendLayout();
			this.PanelExpandedFooter.SuspendLayout();
			this.TableLayoutPanelExpanderFooter.SuspendLayout();
			this.PanelFooter.SuspendLayout();
			this.TableLayoutPanelFooter.SuspendLayout();
			this.PanelButtons.SuspendLayout();
			this.TableLayoutPanelButtons.SuspendLayout();
			this.FlowLayoutPanelButtons.SuspendLayout();
			this.FlowLayoutPanelButtonsLeft.SuspendLayout();
			this.TableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			this.TableLayoutPanelContent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.TableLayoutPanelContent.AutoSize = true;
			this.TableLayoutPanelContent.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanelContent.BackColor = SystemColors.ControlLightLight;
			this.TableLayoutPanelContent.ColumnCount = 2;
			this.TableLayoutPanelContent.ColumnStyles.Add(new ColumnStyle());
			this.TableLayoutPanelContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.TableLayoutPanelContent.Controls.Add(this.LabelIcon, 0, 0);
			this.TableLayoutPanelContent.Controls.Add(this.LabelTitle, 1, 0);
			this.TableLayoutPanelContent.Controls.Add(this.LabelContent, 1, 1);
			this.TableLayoutPanelContent.Controls.Add(this.TableLayoutPanelContents, 1, 3);
			this.TableLayoutPanelContent.Controls.Add(this.LabelExpandedContent, 1, 2);
			var point = new Point(0, 0);
			this.TableLayoutPanelContent.Location = point;
			var padding = new Padding(0);
			this.TableLayoutPanelContent.Margin = padding;
			this.TableLayoutPanelContent.Name = "TableLayoutPanelContent";
			this.TableLayoutPanelContent.RowCount = 4;
			this.TableLayoutPanelContent.RowStyles.Add(new RowStyle());
			this.TableLayoutPanelContent.RowStyles.Add(new RowStyle());
			this.TableLayoutPanelContent.RowStyles.Add(new RowStyle());
			this.TableLayoutPanelContent.RowStyles.Add(new RowStyle());
			var size = new Size(0x471, 0x75);
			this.TableLayoutPanelContent.Size = size;
			this.TableLayoutPanelContent.TabIndex = 0;
			this.LabelIcon.BackColor = Color.Transparent;
			point = new Point(12, 12);
			this.LabelIcon.Location = point;
			padding = new Padding(12, 12, 0, 12);
			this.LabelIcon.Margin = padding;
			this.LabelIcon.Name = "LabelIcon";
			this.TableLayoutPanelContent.SetRowSpan(this.LabelIcon, 2);
			size = new Size(0x20, 0x20);
			this.LabelIcon.Size = size;
			this.LabelIcon.TabIndex = 0;
			this.LabelTitle.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.LabelTitle.AutoSize = true;
			this.LabelTitle.BackColor = Color.Transparent;
			this.LabelTitle.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
			this.LabelTitle.ForeColor = SystemColors.HotTrack;
			point = new Point(0x35, 0x10);
			this.LabelTitle.Location = point;
			padding = new Padding(9, 0x10, 0x10, 0x10);
			this.LabelTitle.Margin = padding;
			this.LabelTitle.Name = "LabelTitle";
			size = new Size(0x4d, 0x13);
			this.LabelTitle.Size = size;
			this.LabelTitle.TabIndex = 1;
			this.LabelTitle.Text = "LabelTitle";
			this.LabelContent.Anchor = AnchorStyles.Left;
			this.LabelContent.AutoSize = true;
			point = new Point(0x38, 0x3b);
			this.LabelContent.Location = point;
			padding = new Padding(12, 8, 0x10, 8);
			this.LabelContent.Margin = padding;
			this.LabelContent.Name = "LabelContent";
			size = new Size(0x47, 13);
			this.LabelContent.Size = size;
			this.LabelContent.TabIndex = 2;
			this.LabelContent.Text = "LabelContent";
			this.TableLayoutPanelContents.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.TableLayoutPanelContents.AutoSize = true;
			this.TableLayoutPanelContents.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanelContents.ColumnCount = 1;
			this.TableLayoutPanelContents.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.TableLayoutPanelContents.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
			point = new Point(0x38, 0x6d);
			this.TableLayoutPanelContents.Location = point;
			padding = new Padding(12, 8, 0x10, 8);
			this.TableLayoutPanelContents.Margin = padding;
			this.TableLayoutPanelContents.Name = "TableLayoutPanelContents";
			this.TableLayoutPanelContents.RowCount = 1;
			this.TableLayoutPanelContents.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			this.TableLayoutPanelContents.RowStyles.Add(new RowStyle(SizeType.Absolute, 1f));
			size = new Size(0x429, 0);
			this.TableLayoutPanelContents.Size = size;
			this.TableLayoutPanelContents.TabIndex = 3;
			this.LabelExpandedContent.Anchor = AnchorStyles.Left;
			this.LabelExpandedContent.AutoSize = true;
			point = new Point(0x38, 80);
			this.LabelExpandedContent.Location = point;
			padding = new Padding(12, 0, 0x10, 8);
			this.LabelExpandedContent.Margin = padding;
			this.LabelExpandedContent.Name = "LabelExpandedContent";
			size = new Size(0x77, 13);
			this.LabelExpandedContent.Size = size;
			this.LabelExpandedContent.TabIndex = 2;
			this.LabelExpandedContent.Text = "LabelExpandedContent";
			this.PanelExpandedFooter.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.PanelExpandedFooter.AutoSize = true;
			this.PanelExpandedFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.PanelExpandedFooter.Controls.Add(this.TableLayoutPanelExpanderFooter);
			this.PanelExpandedFooter.Controls.Add(this.BevelExpanderFooter);
			point = new Point(0, 0xda);
			this.PanelExpandedFooter.Location = point;
			padding = new Padding(0);
			this.PanelExpandedFooter.Margin = padding;
			this.PanelExpandedFooter.Name = "PanelExpandedFooter";
			size = new Size(0x471, 0x1f);
			this.PanelExpandedFooter.Size = size;
			this.PanelExpandedFooter.TabIndex = 3;
			this.TableLayoutPanelExpanderFooter.AutoSize = true;
			this.TableLayoutPanelExpanderFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanelExpanderFooter.ColumnCount = 1;
			this.TableLayoutPanelExpanderFooter.ColumnStyles.Add(new ColumnStyle());
			this.TableLayoutPanelExpanderFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
			this.TableLayoutPanelExpanderFooter.Controls.Add(this.LabelExpandedFooter, 0, 0);
			this.TableLayoutPanelExpanderFooter.Dock = DockStyle.Fill;
			point = new Point(0, 2);
			this.TableLayoutPanelExpanderFooter.Location = point;
			padding = new Padding(0);
			this.TableLayoutPanelExpanderFooter.Margin = padding;
			this.TableLayoutPanelExpanderFooter.Name = "TableLayoutPanelExpanderFooter";
			this.TableLayoutPanelExpanderFooter.RowCount = 1;
			this.TableLayoutPanelExpanderFooter.RowStyles.Add(new RowStyle());
			this.TableLayoutPanelExpanderFooter.RowStyles.Add(new RowStyle(SizeType.Absolute, 29f));
			size = new Size(0x471, 0x1d);
			this.TableLayoutPanelExpanderFooter.Size = size;
			this.TableLayoutPanelExpanderFooter.TabIndex = 1;
			this.LabelExpandedFooter.AutoSize = true;
			point = new Point(0x10, 8);
			this.LabelExpandedFooter.Location = point;
			padding = new Padding(0x10, 8, 0x10, 8);
			this.LabelExpandedFooter.Margin = padding;
			this.LabelExpandedFooter.Name = "LabelExpandedFooter";
			size = new Size(0x70, 13);
			this.LabelExpandedFooter.Size = size;
			this.LabelExpandedFooter.TabIndex = 1;
			this.LabelExpandedFooter.Text = "LabelExpandedFooter";
			this.BevelExpanderFooter.BorderStyle = BorderStyle.Fixed3D;
			this.BevelExpanderFooter.Dock = DockStyle.Top;
			point = new Point(0, 0);
			this.BevelExpanderFooter.Location = point;
			this.BevelExpanderFooter.Name = "BevelExpanderFooter";
			size = new Size(0x471, 2);
			this.BevelExpanderFooter.Size = size;
			this.BevelExpanderFooter.TabIndex = 0;
			this.PanelFooter.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.PanelFooter.AutoSize = true;
			this.PanelFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.PanelFooter.Controls.Add(this.TableLayoutPanelFooter);
			this.PanelFooter.Controls.Add(this.BevelFooter);
			point = new Point(0, 0xba);
			this.PanelFooter.Location = point;
			padding = new Padding(0);
			this.PanelFooter.Margin = padding;
			this.PanelFooter.Name = "PanelFooter";
			size = new Size(0x471, 0x20);
			this.PanelFooter.Size = size;
			this.PanelFooter.TabIndex = 2;
			this.TableLayoutPanelFooter.AutoSize = true;
			this.TableLayoutPanelFooter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanelFooter.ColumnCount = 2;
			this.TableLayoutPanelFooter.ColumnStyles.Add(new ColumnStyle());
			this.TableLayoutPanelFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.TableLayoutPanelFooter.Controls.Add(this.LabelIconFooter, 0, 0);
			this.TableLayoutPanelFooter.Controls.Add(this.LabelFooter, 1, 0);
			this.TableLayoutPanelFooter.Dock = DockStyle.Fill;
			point = new Point(0, 2);
			this.TableLayoutPanelFooter.Location = point;
			padding = new Padding(0);
			this.TableLayoutPanelFooter.Margin = padding;
			this.TableLayoutPanelFooter.Name = "TableLayoutPanelFooter";
			this.TableLayoutPanelFooter.RowCount = 1;
			this.TableLayoutPanelFooter.RowStyles.Add(new RowStyle());
			size = new Size(0x471, 30);
			this.TableLayoutPanelFooter.Size = size;
			this.TableLayoutPanelFooter.TabIndex = 1;
			point = new Point(0x10, 6);
			this.LabelIconFooter.Location = point;
			padding = new Padding(0x10, 6, 0, 8);
			this.LabelIconFooter.Margin = padding;
			this.LabelIconFooter.Name = "LabelIconFooter";
			size = new Size(0x10, 0x10);
			this.LabelIconFooter.Size = size;
			this.LabelIconFooter.TabIndex = 0;
			this.LabelFooter.AutoSize = true;
			point = new Point(0x24, 8);
			this.LabelFooter.Location = point;
			padding = new Padding(4, 8, 0x10, 8);
			this.LabelFooter.Margin = padding;
			this.LabelFooter.Name = "LabelFooter";
			size = new Size(0x40, 13);
			this.LabelFooter.Size = size;
			this.LabelFooter.TabIndex = 0;
			this.LabelFooter.Text = "LabelFooter";
			this.BevelFooter.BorderStyle = BorderStyle.Fixed3D;
			this.BevelFooter.Dock = DockStyle.Top;
			point = new Point(0, 0);
			this.BevelFooter.Location = point;
			this.BevelFooter.Name = "BevelFooter";
			size = new Size(0x471, 2);
			this.BevelFooter.Size = size;
			this.BevelFooter.TabIndex = 0;
			this.PanelButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.PanelButtons.AutoSize = true;
			this.PanelButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.PanelButtons.Controls.Add(this.TableLayoutPanelButtons);
			this.PanelButtons.Controls.Add(this.BevelButtons);
			point = new Point(0, 0x75);
			this.PanelButtons.Location = point;
			padding = new Padding(0);
			this.PanelButtons.Margin = padding;
			this.PanelButtons.Name = "PanelButtons";
			size = new Size(0x471, 0x45);
			this.PanelButtons.Size = size;
			this.PanelButtons.TabIndex = 1;
			this.TableLayoutPanelButtons.AutoSize = true;
			this.TableLayoutPanelButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanelButtons.ColumnCount = 2;
			this.TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle());
			this.TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.TableLayoutPanelButtons.Controls.Add(this.FlowLayoutPanelButtons, 1, 0);
			this.TableLayoutPanelButtons.Controls.Add(this.FlowLayoutPanelButtonsLeft, 0, 0);
			this.TableLayoutPanelButtons.Dock = DockStyle.Fill;
			point = new Point(0, 2);
			this.TableLayoutPanelButtons.Location = point;
			padding = new Padding(0);
			this.TableLayoutPanelButtons.Margin = padding;
			this.TableLayoutPanelButtons.Name = "TableLayoutPanelButtons";
			this.TableLayoutPanelButtons.RowCount = 1;
			this.TableLayoutPanelButtons.RowStyles.Add(new RowStyle());
			size = new Size(0x471, 0x43);
			this.TableLayoutPanelButtons.Size = size;
			this.TableLayoutPanelButtons.TabIndex = 1;
			this.FlowLayoutPanelButtons.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			this.FlowLayoutPanelButtons.AutoSize = true;
			this.FlowLayoutPanelButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.FlowLayoutPanelButtons.Controls.Add(this.Button1);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button2);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button3);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button4);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button5);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button6);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button7);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button8);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button9);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button10);
			this.FlowLayoutPanelButtons.Controls.Add(this.Button11);
			point = new Point(0xd8, 4);
			this.FlowLayoutPanelButtons.Location = point;
			padding = new Padding(8, 4, 8, 4);
			this.FlowLayoutPanelButtons.Margin = padding;
			this.FlowLayoutPanelButtons.Name = "FlowLayoutPanelButtons";
			size = new Size(0x391, 0x21);
			this.FlowLayoutPanelButtons.Size = size;
			this.FlowLayoutPanelButtons.TabIndex = 0;
			this.FlowLayoutPanelButtons.WrapContents = false;
			this.Button1.AutoSize = true;
			this.Button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(4, 4);
			this.Button1.Location = point;
			padding = new Padding(4);
			this.Button1.Margin = padding;
			this.Button1.Name = "Button1";
			size = new Size(0x4b, 0x19);
			this.Button1.Size = size;
			this.Button1.TabIndex = 0;
			this.Button1.Visible = false;
			this.Button2.AutoSize = true;
			this.Button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x57, 4);
			this.Button2.Location = point;
			padding = new Padding(4);
			this.Button2.Margin = padding;
			this.Button2.Name = "Button2";
			size = new Size(0x4b, 0x19);
			this.Button2.Size = size;
			this.Button2.TabIndex = 1;
			this.Button2.Visible = false;
			this.Button3.AutoSize = true;
			this.Button3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(170, 4);
			this.Button3.Location = point;
			padding = new Padding(4);
			this.Button3.Margin = padding;
			this.Button3.Name = "Button3";
			size = new Size(0x4b, 0x19);
			this.Button3.Size = size;
			this.Button3.TabIndex = 2;
			this.Button3.Visible = false;
			this.Button4.AutoSize = true;
			this.Button4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0xfd, 4);
			this.Button4.Location = point;
			padding = new Padding(4);
			this.Button4.Margin = padding;
			this.Button4.Name = "Button4";
			size = new Size(0x4b, 0x19);
			this.Button4.Size = size;
			this.Button4.TabIndex = 3;
			this.Button4.Visible = false;
			this.Button5.AutoSize = true;
			this.Button5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x150, 4);
			this.Button5.Location = point;
			padding = new Padding(4);
			this.Button5.Margin = padding;
			this.Button5.Name = "Button5";
			size = new Size(0x4b, 0x19);
			this.Button5.Size = size;
			this.Button5.TabIndex = 4;
			this.Button5.Visible = false;
			this.Button6.AutoSize = true;
			this.Button6.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x1a3, 4);
			this.Button6.Location = point;
			padding = new Padding(4);
			this.Button6.Margin = padding;
			this.Button6.Name = "Button6";
			size = new Size(0x4b, 0x19);
			this.Button6.Size = size;
			this.Button6.TabIndex = 5;
			this.Button6.Visible = false;
			this.Button7.AutoSize = true;
			this.Button7.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x1f6, 4);
			this.Button7.Location = point;
			padding = new Padding(4);
			this.Button7.Margin = padding;
			this.Button7.Name = "Button7";
			size = new Size(0x4b, 0x19);
			this.Button7.Size = size;
			this.Button7.TabIndex = 6;
			this.Button7.Visible = false;
			this.Button8.AutoSize = true;
			this.Button8.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x249, 4);
			this.Button8.Location = point;
			padding = new Padding(4);
			this.Button8.Margin = padding;
			this.Button8.Name = "Button8";
			size = new Size(0x4b, 0x19);
			this.Button8.Size = size;
			this.Button8.TabIndex = 7;
			this.Button8.Visible = false;
			this.Button9.AutoSize = true;
			this.Button9.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x29c, 4);
			this.Button9.Location = point;
			padding = new Padding(4);
			this.Button9.Margin = padding;
			this.Button9.Name = "Button9";
			size = new Size(0x4b, 0x19);
			this.Button9.Size = size;
			this.Button9.TabIndex = 8;
			this.Button9.Visible = false;
			this.Button10.AutoSize = true;
			this.Button10.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x2ef, 4);
			this.Button10.Location = point;
			padding = new Padding(4);
			this.Button10.Margin = padding;
			this.Button10.Name = "Button10";
			size = new Size(0x4b, 0x19);
			this.Button10.Size = size;
			this.Button10.TabIndex = 9;
			this.Button10.Visible = false;
			this.Button11.AutoSize = true;
			this.Button11.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			point = new Point(0x342, 4);
			this.Button11.Location = point;
			padding = new Padding(4);
			this.Button11.Margin = padding;
			this.Button11.Name = "Button11";
			size = new Size(0x4b, 0x19);
			this.Button11.Size = size;
			this.Button11.TabIndex = 10;
			this.Button11.Visible = false;
			this.FlowLayoutPanelButtonsLeft.Anchor = AnchorStyles.Left;
			this.FlowLayoutPanelButtonsLeft.AutoSize = true;
			this.FlowLayoutPanelButtonsLeft.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.FlowLayoutPanelButtonsLeft.Controls.Add(this.ButtonExpander);
			this.FlowLayoutPanelButtonsLeft.Controls.Add(this.CheckBox);
			this.FlowLayoutPanelButtonsLeft.FlowDirection = FlowDirection.TopDown;
			point = new Point(8, 4);
			this.FlowLayoutPanelButtonsLeft.Location = point;
			padding = new Padding(8, 4, 4, 4);
			this.FlowLayoutPanelButtonsLeft.Margin = padding;
			this.FlowLayoutPanelButtonsLeft.Name = "FlowLayoutPanelButtonsLeft";
			size = new Size(0xc4, 0x3b);
			this.FlowLayoutPanelButtonsLeft.Size = size;
			this.FlowLayoutPanelButtonsLeft.TabIndex = 1;
			this.ButtonExpander.AutoSize = true;
			this.ButtonExpander.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.ButtonExpander.BackColor = Color.Transparent;
			this.ButtonExpander.Expanded = false;
			this.ButtonExpander.FlatAppearance.BorderColor = SystemColors.Control;
			this.ButtonExpander.FlatAppearance.BorderSize = 0;
			this.ButtonExpander.FlatAppearance.MouseDownBackColor = Color.Transparent;
			this.ButtonExpander.FlatAppearance.MouseOverBackColor = Color.Transparent;
			this.ButtonExpander.FlatStyle = FlatStyle.Flat;
			//this.ButtonExpander.Image = (Image)manager.GetObject("ButtonExpander.Image");
			this.ButtonExpander.ImageAlign = ContentAlignment.MiddleLeft;
			point = new Point(0, 4);
			this.ButtonExpander.Location = point;
			padding = new Padding(0, 4, 8, 4);
			this.ButtonExpander.Margin = padding;
			this.ButtonExpander.Name = "ButtonExpander";
			size = new Size(90, 0x1a);
			this.ButtonExpander.Size = size;
			this.ButtonExpander.TabIndex = 0;
			this.ButtonExpander.Text = "WaitFor More";
			this.ButtonExpander.TextAlign = ContentAlignment.MiddleLeft;
			this.ButtonExpander.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.ButtonExpander.UseVisualStyleBackColor = false;
			this.ButtonExpander.Visible = false;
			this.CheckBox.AutoSize = true;
			point = new Point(8, 0x26);
			this.CheckBox.Location = point;
			padding = new Padding(8, 4, 8, 4);
			this.CheckBox.Margin = padding;
			this.CheckBox.Name = "CheckBox";
			size = new Size(180, 0x11);
			this.CheckBox.Size = size;
			this.CheckBox.TabIndex = 1;
			this.CheckBox.Text = "Do not show this message again";
			this.CheckBox.UseVisualStyleBackColor = true;
			this.BevelButtons.BorderStyle = BorderStyle.Fixed3D;
			this.BevelButtons.Dock = DockStyle.Top;
			point = new Point(0, 0);
			this.BevelButtons.Location = point;
			this.BevelButtons.Name = "BevelButtons";
			size = new Size(0x471, 2);
			this.BevelButtons.Size = size;
			this.BevelButtons.TabIndex = 0;
			this.TableLayoutPanel.AutoSize = true;
			this.TableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanel.ColumnCount = 1;
			this.TableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this.TableLayoutPanel.Controls.Add(this.TableLayoutPanelContent, 0, 0);
			this.TableLayoutPanel.Controls.Add(this.PanelButtons, 0, 1);
			this.TableLayoutPanel.Controls.Add(this.PanelFooter, 0, 2);
			this.TableLayoutPanel.Controls.Add(this.PanelExpandedFooter, 0, 3);
			this.TableLayoutPanel.Dock = DockStyle.Fill;
			point = new Point(0, 0);
			this.TableLayoutPanel.Location = point;
			padding = new Padding(0);
			this.TableLayoutPanel.Margin = padding;
			this.TableLayoutPanel.Name = "TableLayoutPanel";
			this.TableLayoutPanel.RowCount = 4;
			this.TableLayoutPanel.RowStyles.Add(new RowStyle());
			this.TableLayoutPanel.RowStyles.Add(new RowStyle());
			this.TableLayoutPanel.RowStyles.Add(new RowStyle());
			this.TableLayoutPanel.RowStyles.Add(new RowStyle());
			size = new Size(0x25f, 0x20a);
			this.TableLayoutPanel.Size = size;
			this.TableLayoutPanel.TabIndex = 0;
			var ef = new SizeF(6f, 13f);
			this.AutoScaleDimensions = ef;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			size = new Size(0x25f, 0x20a);
			this.ClientSize = size;
			this.Controls.Add(this.TableLayoutPanel);
			this.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0xee);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MsgBoxExDialogForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "MsgBoxExDialogForm";
			this.TableLayoutPanelContent.ResumeLayout(false);
			this.TableLayoutPanelContent.PerformLayout();
			this.PanelExpandedFooter.ResumeLayout(false);
			this.PanelExpandedFooter.PerformLayout();
			this.TableLayoutPanelExpanderFooter.ResumeLayout(false);
			this.TableLayoutPanelExpanderFooter.PerformLayout();
			this.PanelFooter.ResumeLayout(false);
			this.PanelFooter.PerformLayout();
			this.TableLayoutPanelFooter.ResumeLayout(false);
			this.TableLayoutPanelFooter.PerformLayout();
			this.PanelButtons.ResumeLayout(false);
			this.PanelButtons.PerformLayout();
			this.TableLayoutPanelButtons.ResumeLayout(false);
			this.TableLayoutPanelButtons.PerformLayout();
			this.FlowLayoutPanelButtons.ResumeLayout(false);
			this.FlowLayoutPanelButtons.PerformLayout();
			this.FlowLayoutPanelButtonsLeft.ResumeLayout(false);
			this.FlowLayoutPanelButtonsLeft.PerformLayout();
			this.TableLayoutPanel.ResumeLayout(false);
			this.TableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private void LabelIconPaint(object sender, PaintEventArgs e)
		{
			if (!this._DrawGradient)
				return;
			var rect = new Rectangle(0, 0, this.TableLayoutPanel.Width, this.LabelIcon.Height);
			using (var brush = new LinearGradientBrush(rect, this._GradientBegin, this._GradientEnd, LinearGradientMode.Horizontal))
			{
				rect.X -= this.LabelIcon.Left;
				e.Graphics.FillRectangle(brush, rect);
			}
			e.Graphics.DrawImage(this.LabelIcon.Image, 0, 0);
		}

		private void SetExpanderText()
		{
			this.ButtonExpander.Text = this.Expanded ? this.ExpandedControlText : this.CollapsedControlText;
		}

		private void TableLayoutPanelContentCellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			if ((!this._DrawGradient || (e.Row != 0)) || (e.Column != 1))
				return;
			var rect = new Rectangle(0, 0, this.TableLayoutPanel.Width, e.CellBounds.Height);
			using (var brush = new LinearGradientBrush(rect, this._GradientBegin, this._GradientEnd, LinearGradientMode.Horizontal))
				e.Graphics.FillRectangle(brush, rect);
		}

		private void MsgBoxExDialogFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.Tag == null)
				if (((this.CancelButton == null) || !(this.CancelButton is Button)) || ((this.CancelButton as Button).Tag == null))
					this.Tag = new MsgBoxExDialogButton(MsgBoxExDialogResult.Cancel);
				else
					this.Tag = (this.CancelButton as Button).Tag;
		}

		private void MsgBoxExDialogFormShown(object sender, EventArgs e)
		{
			this.Tag = null;
			var defaultButton = (int)this.DefaultButton;
			if ((defaultButton > 0) && (defaultButton <= this.MsgBoxExButtons.Length))
			{
				this.AcceptButton = this.Buttons[defaultButton - 1];
				this.Buttons[defaultButton - 1].Focus();
			}
			else
				this.LabelContent.Focus();
			if (this.LabelTitle.Visible && this.LabelContent.Visible)
			{
				var padding4 = new Padding(this.LabelTitle.Margin.Left, this.LabelTitle.Margin.Top, this.LabelTitle.Margin.Right, 0);
				this.LabelTitle.Margin = padding4;
				var padding5 = new Padding(this.LabelContent.Margin.Left, this._DrawGradient ? 0x10 : this.LabelContent.Margin.Top, this.LabelContent.Margin.Right, 0x10);
				this.LabelContent.Margin = padding5;
			}
			if (!string.IsNullOrEmpty(this.ExpandedInformation) && this.Expanded)
				this.Expanded = true;
			if (this.Sound != null)
				this.Sound.Play();
		}
	}
}