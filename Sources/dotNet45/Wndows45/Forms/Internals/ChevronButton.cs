using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Win.Properties;

namespace Mohammad.Win.Forms.Internals
{
    internal class ChevronButton : Button
    {
        #region Fields

        #region _IsMouseDown

        private bool _IsMouseDown;

        #endregion

        #region _IsKeyDown

        private bool _IsKeyDown;

        #endregion

        #region _IsHovered

        private bool _IsHovered;

        #endregion

        #region _IsFocusedByKey

        #endregion

        #region _IsFocused

        private bool _IsFocused;

        #endregion

        #region components

        private readonly IContainer components;

        #endregion

        #region _Expanded

        private bool _Expanded;

        #endregion

        #endregion

        #region Properties

        #region _IsPressed

        public bool IsFocusedByKey { get; private set; }

        private bool _IsPressed { get { return this._IsKeyDown || this._IsMouseDown && this._IsHovered; } }

        #endregion

        #region Focused

        public override bool Focused { get { return false; } }

        #endregion

        #region Expanded

        public bool Expanded
        {
            get { return this._Expanded; }
            set
            {
                this._Expanded = value;
                this.SetImage();
            }
        }

        #endregion

        #endregion

        #region Methods

        #region SetImage

        private void SetImage()
        {
            if (this._IsPressed)
                this.Image = this._Expanded ? Resources.chevronlesspressed : Resources.chevronmorepressed;
            else if (this._IsHovered || this._IsFocused)
                this.Image = this._Expanded ? Resources.chevronlesshovered : Resources.chevronmorehovered;
            else
                this.Image = this._Expanded ? Resources.chevronless : Resources.chevronmore;
        }

        #endregion

        #region OnMouseUp

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (this._IsMouseDown)
            {
                this._IsMouseDown = false;
                this.SetImage();
            }
            base.OnMouseUp(mevent);
        }

        #endregion

        #region OnMouseMove

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            if (mevent.Button != MouseButtons.None)
                if (!this.ClientRectangle.Contains(mevent.X, mevent.Y))
                {
                    if (this._IsHovered)
                    {
                        this._IsHovered = false;
                        this.SetImage();
                    }
                }
                else if (!this._IsHovered)
                {
                    this._IsHovered = true;
                    this.SetImage();
                }
        }

        #endregion

        #region OnMouseLeave

        protected override void OnMouseLeave(EventArgs e)
        {
            this._IsHovered = false;
            this.SetImage();
            base.OnMouseLeave(e);
        }

        #endregion

        #region OnMouseEnter

        protected override void OnMouseEnter(EventArgs e)
        {
            this._IsHovered = true;
            this.SetImage();
            base.OnMouseEnter(e);
        }

        #endregion

        #region OnMouseDown

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (!this._IsMouseDown && mevent.Button == MouseButtons.Left)
            {
                this._IsMouseDown = true;
                this.IsFocusedByKey = false;
                this.SetImage();
            }
            base.OnMouseDown(mevent);
        }

        #endregion

        #region OnLeave

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this._IsFocused = false;
            this.IsFocusedByKey = false;
            this._IsKeyDown = false;
            this._IsMouseDown = false;
            this.SetImage();
        }

        #endregion

        #region OnKeyUp

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (this._IsKeyDown && kevent.KeyCode == Keys.Space)
            {
                this._IsKeyDown = false;
                this.SetImage();
            }
            base.OnKeyUp(kevent);
        }

        #endregion

        #region OnKeyDown

        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space)
            {
                this._IsKeyDown = true;
                this.SetImage();
            }
            base.OnKeyDown(kevent);
        }

        #endregion

        #region OnEnter

        protected override void OnEnter(EventArgs e)
        {
            this._IsFocused = true;
            this.IsFocusedByKey = true;
            this.SetImage();
            base.OnEnter(e);
        }

        #endregion

        #region OnClick

        protected override void OnClick(EventArgs e)
        {
            this._IsKeyDown = false;
            this._IsMouseDown = false;
            this._Expanded ^= true;
            this.SetImage();
            base.OnClick(e);
        }

        #endregion

        #region InitializeComponent

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = Color.Transparent;
            this.FlatAppearance.BorderColor = SystemColors.Control;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatStyle = FlatStyle.Flat;
            this.ImageAlign = ContentAlignment.MiddleLeft;
            this.TextAlign = ContentAlignment.MiddleLeft;
            this.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Visible = false;
            this.ResumeLayout(false);
        }

        #endregion

        #region Dispose

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && this.components != null)
                    this.components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region ChevronButton

        public ChevronButton()
        {
            this.InitializeComponent();
            this.Image = Resources.chevronmore;
        }

        #endregion

        #endregion
    }
}