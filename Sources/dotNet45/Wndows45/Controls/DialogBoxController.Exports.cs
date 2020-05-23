using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Win.Internals;

namespace Mohammad.Win.Controls
{
    partial class DialogBoxController
    {
        #region Fade

        /// <summary>
        ///     Gets or sets that the dialog box must be faded in start of showing
        /// </summary>
        [Description("Gets or sets that the dialog box must be faded in start of showing")]
        [DefaultValue(false)]
        public bool Fade { get; set; }

        #endregion

        #region Enable

        private bool enable;
        private CallBack_WinProc myWndProc;

        /// <summary>
        ///     Enables or disabled tracking dialog boxes
        /// </summary>
        [Description("Enables or disabled tracking dialog boxes")]
        [DefaultValue(false)]
        public bool Enable
        {
            get { return this.enable; }
            set
            {
                this.enable = value;
                if (this.DesignMode)
                    return;
                if (this.Enable)
                {
                    if (this.hHook != 0)
                        UnhookWindowsHookEx(this.hHook);

                    int hInst;
                    int Thread;

                    this.myWndProc = this.WinProc;

                    hInst = GetWindowLong(0, GWL_HINSTANCE);
                    Thread = GetCurrentThreadId();
                    this.hHook = SetWindowsHookEx(WH_CBT, this.myWndProc, hInst, Thread);
                }
                else
                {
                    UnhookWindowsHookEx(this.hHook);
                    this.hHook = 0;
                    this.myWndProc = null;
                    this.myEnumProc = null;
                }
            }
        }

        #endregion

        #region RightToLeft

        /// <summary>
        ///     Gets or sets a value indicating whether the text in controlled dialogs appears from right to left
        /// </summary>
        [Description("Gets or sets a value indicating whether the text in controlled dialogs appears from right to left")]
        [Localizable(true)]
        [DefaultValue(RightToLeft.Yes)]
        public RightToLeft RightToLeft { get; set; } = RightToLeft.Yes;

        #endregion

        #region Translate

        /// <summary>
        ///     Gets or sets that the buttons must be translated to persian language.
        /// </summary>
        [Description("Gets or sets that the buttons must be trabslated to persian language.")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool Translate { get; set; } = true;

        #endregion

        #region Font

        /// <summary>
        ///     Gets or sets the font of the text displayed by the controlled dialog box.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:System.Drawing.Font"></see> to apply to the text displayed by the control. The default is the
        ///     value of the
        ///     <see
        ///         cref="P:System.Windows.Forms.Control.DefaultFont">
        ///     </see>
        ///     property.
        /// </returns>
        [Description("Gets or sets the font of the text displayed by the controlled dialog box.")]
        [Localizable(true)]
        [DefaultValue(typeof(Font), "Tahoma")]
        public virtual Font Font { get; set; } = new Font("Tahoma", 8);

        #endregion

        #region Dictionary

        private DialogBoxControllerDict _Dictionary;

        /// <summary>
        ///     Gets or set the dictionary which will be used to translate the controls found in dialog boxes.
        /// </summary>
        public DialogBoxControllerDict Dictionary
        {
            get
            {
                if (this._Dictionary == null)
                    this._Dictionary = new DialogBoxControllerDict();
                return this._Dictionary;
            }
            set { this._Dictionary = value; }
        }

        #endregion
    }
}