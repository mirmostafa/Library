namespace Library35.Windows.Controls
{
    partial class ProChart
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
            this.label_Resize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Resize
            // 
            this.label_Resize.BackColor = System.Drawing.Color.Black;
            this.label_Resize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Resize.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.label_Resize.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Resize.ForeColor = System.Drawing.Color.White;
            this.label_Resize.Location = new System.Drawing.Point(410, 284);
            this.label_Resize.Name = "label_Resize";
            this.label_Resize.Size = new System.Drawing.Size(20, 16);
            this.label_Resize.TabIndex = 0;
            this.label_Resize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Resize.Visible = false;
            // 
            // ProChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label_Resize);
            this.Name = "ProChart";
            this.Size = new System.Drawing.Size(430, 300);
            this.Load += new System.EventHandler(this.MyChart_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Resize;
    }
}
