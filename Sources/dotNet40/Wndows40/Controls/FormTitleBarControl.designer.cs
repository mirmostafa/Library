namespace Library40.Win.Controls
{
    partial class FormTitleBarControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTitleBarControl));
            this.pbTitle = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.frmControlBox = new FormControlBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTitle
            // 
            this.pbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pbTitle.BackgroundImage = Properties.Resources.bkg;
            this.pbTitle.Image = Properties.Resources.bar;
            this.pbTitle.Location = new System.Drawing.Point(0, 0);
            this.pbTitle.Name = "pbTitle";
            this.pbTitle.Size = new System.Drawing.Size(249, 24);
            this.pbTitle.TabIndex = 0;
            this.pbTitle.TabStop = false;
            this.pbTitle.DoubleClick += new System.EventHandler(this.PbTitleDoubleClick);
            this.pbTitle.Click += new System.EventHandler(this.PbTitleClick);
            this.pbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbTitleMouseDown);
            this.pbTitle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PbTitleMouseDoubleClick);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(6, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(166, 15);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "My Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.DoubleClick += new System.EventHandler(this.LblTitleDoubleClick);
            this.lblTitle.Click += new System.EventHandler(this.LblTitleClick);
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            this.lblTitle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LblTitleMouseDoubleClick);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblTitleMouseUp);
            // 
            // frmControlBox
            // 
            this.frmControlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.frmControlBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("frmControlBox.BackgroundImage")));
            this.frmControlBox.Close = true;
            this.frmControlBox.Location = new System.Drawing.Point(247, 0);
            this.frmControlBox.Maximize = true;
            this.frmControlBox.Minimize = true;
            this.frmControlBox.Name = "frmControlBox";
            this.frmControlBox.Size = new System.Drawing.Size(70, 26);
            this.frmControlBox.TabIndex = 2;
            // 
            // FormTitleBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = Properties.Resources.bkg;
            this.Controls.Add(this.frmControlBox);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbTitle);
            this.DoubleBuffered = true;
            this.Name = "FormTitleBarControl";
            this.Size = new System.Drawing.Size(317, 25);
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTitle;
        public System.Windows.Forms.Label lblTitle;
        private FormControlBox frmControlBox;


    }
}