using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Helpers;
using Mohammad.Win.Helpers;

namespace Mohammad.Win.Controls
{
    /// <summary>
    ///     Represents a window or dialog box that makes up an application's user interface.
    /// </summary>
    public partial class LibraryForm : Form
    {
        private bool _IsDataChanged;
        private bool _IsFormLoading;
        private static readonly Font _DefaultFont = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 178, false);

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (value == null)
                    base.Font = _DefaultFont;
                else if (value == DefaultFont)
                    base.Font = _DefaultFont;
                else
                    base.Font = value;
            }
        }

        [Browsable(false)]
        public bool IsFormLoading
        {
            get { return this._IsFormLoading; }
            private set
            {
                if (this._IsFormLoading == value)
                    return;
                this._IsFormLoading = value;
                this.IsFormLoadingChanged.RaiseAsync(this);
            }
        }

        [Browsable(false)]
        public bool IsDataChanged
        {
            get { return this._IsDataChanged; }
            set
            {
                if (this._IsDataChanged == value)
                    return;
                this._IsDataChanged = value;
                this.DataChanged.RaiseAsync(this);
            }
        }

        [DefaultValue(200)]
        public int AnimateTime { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                if (!this.DesignMode && this.DropShadow)
                    createParams.ClassStyle |= 0x00020000;
                return createParams;
            }
        }

        [DefaultValue(false)]
        public bool DropShadow { get; set; }

        [DefaultValue(false)]
        public bool AutoSetText { get; set; }

        /// <summary>
        ///     Initializes a new instance of the Library.Windows.LibraryForm class.
        /// </summary>
        public LibraryForm()
        {
            this.IsFormLoading = true;
            this.InitializeComponent();
            this.AnimateTime = 200;
            this.Font = _DefaultFont;
        }

        public override void ResetFont() { this.Font = null; }
        private bool ShouldSerializeFont() { return !this.Font.Equals(_DefaultFont); }
        public event EventHandler LoadSettings;
        public event EventHandler SaveSettings;

        protected virtual void OnLoadSettings(EventArgs e)
        {
            this.GetControls<IInitializable>().ForEach(ctrl => ctrl.Initialize());
            this.LoadSettings.Raise(this, e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            this.OnSaveSettings(EventArgs.Empty);
        }

        protected virtual void OnSaveSettings(EventArgs e) { this.SaveSettings.Raise(this, e); }

        protected override void OnLoad(EventArgs e)
        {
            this.OnLoadSettings(EventArgs.Empty);
            base.OnLoad(e);
        }

        public event EventHandler IsFormLoadingChanged;
        public event EventHandler DataChanged;

        private void SetText()
        {
            if (!this.DesignMode && this.AutoSetText && !this.Text.EndsWith(Application.ProductVersion))
                this.Text = this.Text.IsNullOrEmpty()
                    ? string.Format("{0} ({1})", Application.ProductName, Application.ProductVersion)
                    : string.Format("{0} - {1} ({2})", this.Text, Application.ProductName, Application.ProductVersion);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.IsFormLoading = false;
            this.SetText();
            this.CallOnDataChanged(delegate { this.IsDataChanged = true; });
        }
    }
}