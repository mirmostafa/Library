namespace Library40.Win.Controls
{
    partial class FormControlBox
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
            this.lblMinimize = new System.Windows.Forms.Label();
            this.lblMaximize = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMinimize
            // 
            this.lblMinimize.BackColor = System.Drawing.Color.Transparent;
            this.lblMinimize.Image = Properties.Resources.minimize;
            this.lblMinimize.Location = new System.Drawing.Point(4, 4);
            this.lblMinimize.Name = "lblMinimize";
            this.lblMinimize.Size = new System.Drawing.Size(19, 20);
            this.lblMinimize.TabIndex = 0;
            this.lblMinimize.MouseLeave += new System.EventHandler(this.lblMinimize_MouseLeave);
            this.lblMinimize.Click += new System.EventHandler(this.lblMinimize_Click);
            this.lblMinimize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblMinimize_MouseMove);
            // 
            // lblMaximize
            // 
            this.lblMaximize.BackColor = System.Drawing.Color.Transparent;
            this.lblMaximize.Image = Properties.Resources.maximize;
            this.lblMaximize.Location = new System.Drawing.Point(26, 4);
            this.lblMaximize.Name = "lblMaximize";
            this.lblMaximize.Size = new System.Drawing.Size(17, 20);
            this.lblMaximize.TabIndex = 0;
            this.lblMaximize.MouseLeave += new System.EventHandler(this.lblMaximize_MouseLeave);
            this.lblMaximize.Click += new System.EventHandler(this.lblMaximize_Click);
            this.lblMaximize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblMaximize_MouseMove);
            // 
            // lblClose
            // 
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Image = Properties.Resources.close;
            this.lblClose.Location = new System.Drawing.Point(47, 4);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(17, 20);
            this.lblClose.TabIndex = 0;
            this.lblClose.MouseLeave += new System.EventHandler(this.lblClose_MouseLeave);
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblClose_MouseMove);
            // 
            // FormControlBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = Properties.Resources.control_box;
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblMaximize);
            this.Controls.Add(this.lblMinimize);
            this.Name = "FormControlBox";
            this.Size = new System.Drawing.Size(70, 26);
            this.Load += new System.EventHandler(this.FormControlBox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMinimize;
        private System.Windows.Forms.Label lblMaximize;
        private System.Windows.Forms.Label lblClose;

    }
}