#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Library35.Helpers;
using Library35.Windows.Forms.Internals;

namespace Library35.Windows.Forms
{
	public partial class MsgBoxEx
	{
		private MsgBoxExDialogDefaultButton _DefaultButton = MsgBoxExDialogDefaultButton.Button1;

		private string _ExpandedInformation;

		private bool _LockSystem;

		private MsgBoxExDialogForm _MsgBoxExDialogForm;

		private ISound _Sound;

#pragma warning disable 649
		private static readonly IWin32Window _NullWindow;
#pragma warning restore 649

		public MsgBoxExDialogButton[] Buttons { get; set; }

		public string CollapsedControlText { get; set; }

		private string _Content;
		public string Content
		{
			get { return this._Content; }
			set
			{
				if (this._MsgBoxExDialogForm != null)
				{
					Action updateUi = null;
					var action = updateUi;
					updateUi = delegate
					           {
						           if (this._MsgBoxExDialogForm.InvokeRequired)
						           {
							           this._MsgBoxExDialogForm.Invoke(action);
							           return;
						           }
						           this._MsgBoxExDialogForm.Content = value;
					           };
					updateUi();
				}
				this._Content = value;
			}
		}

		public Control CustomControl { get; set; }

		public Image CustomFooterIcon { get; set; }

		public Image CustomMainIcon { get; set; }

		public MsgBoxExDialogDefaultButton DefaultButton
		{
			get { return this._DefaultButton; }
			set { this._DefaultButton = value; }
		}

		public bool ExpandedByDefault { get; set; }

		public string ExpandedControlText { get; set; }

		public string ExpandedInformation
		{
			get { return this._ExpandedInformation; }
			set
			{
				if (this._MsgBoxExDialogForm != null)
				{
					Action updateUi = null;
					var action = updateUi;
					updateUi = delegate
					           {
						           if (this._MsgBoxExDialogForm.InvokeRequired)
						           {
							           this._MsgBoxExDialogForm.Invoke(action);
							           return;
						           }
						           this._MsgBoxExDialogForm.ExpandedInformation = value;
					           };
					updateUi();
				}
				this._ExpandedInformation = value;
			}
		}

		public bool ExpandFooterArea { get; set; }

		public MsgBoxExDialogIcon FooterIcon { get; set; }

		public string FooterText { get; set; }

		public bool LockSystem
		{
			get { return this._LockSystem; }
			set { this._LockSystem = value; }
		}

		private MsgBoxExDialogIcon _MainIcon;
		public MsgBoxExDialogIcon MainIcon
		{
			get { return this._MainIcon; }
			set
			{
				if (this._MsgBoxExDialogForm != null)
				{
					Action updateUi = null;
					var action = updateUi;
					updateUi = delegate
					           {
						           if (this._MsgBoxExDialogForm.InvokeRequired)
						           {
							           this._MsgBoxExDialogForm.Invoke(action);
							           return;
						           }
						           this._MsgBoxExDialogForm.MainIcon = value;
					           };
					updateUi();
				}
				this._MainIcon = value;
			}
		}

		public string MainInstruction { get; set; }

		public IWin32Window Owner { get; set; }

		public MsgBoxExDialogResult Result { get; set; }

		public bool RightToLeftLayout { get; set; }

		public RightToLeft RightToLeft { get; set; }

		public ISound Sound
		{
			get
			{
				if (this._LockSystem && (this._Sound == null))
					return MsgBoxExDialogSound.Security;
				return this._Sound;
			}
			set { this._Sound = value; }
		}

		public CheckState VerificationFlagChecked { get; set; }

		public string VerificationText { get; set; }

		private string _WindowTitle;

		public string WindowTitle
		{
			get { return this._WindowTitle.IsNull(Application.ProductName); }
			set { this._WindowTitle = value; }
		}

		private MsgBoxExDialogResult LockSystemAndShow()
		{
			var owner = this.Owner as Control;
			var screen = owner == null ? Screen.PrimaryScreen : Screen.FromControl(owner);
			var image = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
			var ptr4 = IntPtr.Zero;
			using (var graphics = Graphics.FromImage(image))
			{
				var bounds = screen.Bounds;
				graphics.CopyFromScreen(0, 0, 0, 0, bounds.Size);
				using (Brush brush = new SolidBrush(Color.FromArgb(0xc0, Color.Black)))
					graphics.FillRectangle(brush, screen.Bounds);
				if (owner != null)
				{
					var form = owner.FindForm();
					if (form != null)
					{
						graphics.CopyFromScreen(form.Location, form.Location, form.Size);
						using (Brush brush2 = new SolidBrush(Color.FromArgb(0x80, Color.Black)))
						{
							bounds = new Rectangle(form.Location, form.Size);
							graphics.FillRectangle(brush2, bounds);
						}
					}
				}
				var threadDesktop = MsgBoxExNative.GetThreadDesktop(Thread.CurrentThread.ManagedThreadId);
				var desktop = MsgBoxExNative.OpenInputDesktop(0, false, 0x100);
				string device = null;
				var desktopStr = "Desktop" + Guid.NewGuid();
				var ptr = MsgBoxExNative.CreateDesktop(ref desktopStr, ref device, ptr4, 0, 0x10000000, ptr4);
				MsgBoxExNative.SetThreadDesktop(ptr);
				MsgBoxExNative.SwitchDesktop(ptr);
				var thread = new Thread(this.NewThreadMethod);
				thread.Start(new MsgBoxExDialogLockSystemParameters(ptr, image));
				thread.Join();
				MsgBoxExNative.SwitchDesktop(desktop);
				MsgBoxExNative.SetThreadDesktop(threadDesktop);
				MsgBoxExNative.CloseDesktop(ptr);
				return this.Result;
			}
		}

		public MsgBoxEx()
		{
			this.RightToLeft = Thread.CurrentThread.CurrentUICulture.Name.EqualsTo("fa-ir") ? RightToLeft.Yes : RightToLeft.Inherit;
		}

		private void NewThreadMethod(object args)
		{
			var parameters = (MsgBoxExDialogLockSystemParameters)args;
			MsgBoxExNative.SetThreadDesktop(parameters.NewDesktop);
			using (Form form = new MsgBoxExBackgroundForm(parameters.Background))
			{
				form.Show();
				this.ShowInternal(form);
				form.BackgroundImage = null;
				Application.DoEvents();
				Thread.Sleep(250);
			}
		}

		private static void SetStartPosition(Form f, IWin32Window o)
		{
			f.StartPosition = o == null ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
		}

		public MsgBoxExDialogResult Show()
		{
			return this.LockSystem ? this.LockSystemAndShow() : this.ShowInternal(this.Owner);
		}

		private MsgBoxExDialogResult ShowInternal(IWin32Window owner)
		{
			this._MsgBoxExDialogForm = new MsgBoxExDialogForm
			                           {
				                           Content = this.Content,
				                           Caption = this.WindowTitle,
				                           Title = this.MainInstruction,
				                           MsgBoxExButtons = this.Buttons,
				                           MainIcon = this.MainIcon,
				                           FooterText = this.FooterText,
				                           DefaultButton = this.DefaultButton,
				                           RightToLeft = this.RightToLeft,
				                           RightToLeftLayout = this.RightToLeftLayout,
				                           ShowCheckBox = !string.IsNullOrEmpty(this.VerificationText),
				                           CheckBoxState = this.VerificationFlagChecked,
				                           CheckBoxText = this.VerificationText,
				                           CustomControl = this.CustomControl,
				                           ExpandFooterArea = this.ExpandFooterArea,
				                           ExpandedInformation = this.ExpandedInformation,
				                           Expanded = this.ExpandedByDefault,
				                           ExpandedControlText = this.ExpandedControlText,
				                           CollapsedControlText = this.CollapsedControlText
			                           };
			if (this.CustomMainIcon != null)
				this._MsgBoxExDialogForm.Image = this.CustomMainIcon;
			this._MsgBoxExDialogForm.FooterIcon = this.FooterIcon;
			if (this.CustomFooterIcon != null)
				this._MsgBoxExDialogForm.FooterImage = this.CustomFooterIcon;
			if (this.Sound != null)
				this._MsgBoxExDialogForm.Sound = this.Sound;
			SetStartPosition(this._MsgBoxExDialogForm, owner);
			this._MsgBoxExDialogForm.ShowDialog(owner);
			this._MsgBoxExDialogForm.CustomControl = null;
			this.VerificationFlagChecked = this._MsgBoxExDialogForm.CheckBoxState;
			this.Result = ((MsgBoxExDialogButton)this._MsgBoxExDialogForm.Tag).MsgBoxExDialogResult;
			return this.Result;
		}

		public override string ToString()
		{
			var text = new StringBuilder(this.WindowTitle);
			text.AppendLine();
			text.AppendLine("=========================");
			text.AppendLine(string.Format("ContentText  : {0}", this.Content));
			text.AppendLine(string.Format("MoreInfo     : {0}", this.ExpandedInformation));
			text.AppendLine("=========================");
			text.AppendLine(string.Format("FooterText   : {0}", this.FooterText));
			return text.ToString();
		}
	}
}