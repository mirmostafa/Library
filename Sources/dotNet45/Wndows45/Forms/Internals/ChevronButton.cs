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
        private readonly IContainer components;

        private bool _Expanded;

        private bool _IsFocused;

        private bool _IsHovered;

        private bool _IsKeyDown;
        private bool _IsMouseDown;

        public ChevronButton()
        {
            this.InitializeComponent();
            this.Image = Resources.chevronmore;
        }

        public bool IsFocusedByKey { get; private set; }

        private bool _IsPressed => this._IsKeyDown || this._IsMouseDown && this._IsHovered;

        public override bool Focused => false;

        public bool Expanded
        {
            get => this._Expanded;
            set
            {
                this._Expanded = value;
                this.SetImage();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (this._IsMouseDown)
            {
                this._IsMouseDown = false;
                this.SetImage();
            }

            base.OnMouseUp(mevent);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            if (mevent.Button != MouseButtons.None)
            {
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
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this._IsHovered = false;
            this.SetImage();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this._IsHovered = true;
            this.SetImage();
            base.OnMouseEnter(e);
        }

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

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this._IsFocused = false;
            this.IsFocusedByKey = false;
            this._IsKeyDown = false;
            this._IsMouseDown = false;
            this.SetImage();
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (this._IsKeyDown && kevent.KeyCode == Keys.Space)
            {
                this._IsKeyDown = false;
                this.SetImage();
            }

            base.OnKeyUp(kevent);
        }

        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space)
            {
                this._IsKeyDown = true;
                this.SetImage();
            }

            base.OnKeyDown(kevent);
        }

        protected override void OnEnter(EventArgs e)
        {
            this._IsFocused = true;
            this.IsFocusedByKey = true;
            this.SetImage();
            base.OnEnter(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this._IsKeyDown = false;
            this._IsMouseDown = false;
            this._Expanded ^= true;
            this.SetImage();
            base.OnClick(e);
        }

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && this.components != null)
                {
                    this.components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void SetImage()
        {
            if (this._IsPressed)
            {
                this.Image = this._Expanded ? Resources.chevronlesspressed : Resources.chevronmorepressed;
            }
            else if (this._IsHovered || this._IsFocused)
            {
                this.Image = this._Expanded ? Resources.chevronlesshovered : Resources.chevronmorehovered;
            }
            else
            {
                this.Image = this._Expanded ? Resources.chevronless : Resources.chevronmore;
            }
        }

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
    }
}