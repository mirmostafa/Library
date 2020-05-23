using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library35.Windows.Controls
{
    partial class HBarChart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = (AutoScaleMode)1;
            base.Name = "HBarChart";
            base.MouseLeave += new EventHandler(this.HBarChart_MouseLeave);

            base.Paint += new PaintEventHandler(this.OnPaint);
            base.MouseMove += new MouseEventHandler(this.OnMouseMove);
            base.BackColorChanged += new EventHandler(this.HBarChart_BackColorChanged);

            base.MouseDoubleClick += new MouseEventHandler(this.HBarChart_MouseDoubleClick);

            base.MouseClick += new MouseEventHandler(this.HBarChart_MouseClick);

            base.Resize += new EventHandler(this.OnSize);
            base.ResumeLayout(false);
        }

        #endregion
    }
}