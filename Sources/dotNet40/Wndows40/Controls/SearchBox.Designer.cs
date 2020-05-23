namespace Library40.Win.Controls
{
	partial class SearchBox
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
			this.SearchTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// SearchTextBox
			// 
			this.SearchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchTextBox.Location = new System.Drawing.Point(0, 0);
			this.SearchTextBox.Name = "SearchTextBox";
			this.SearchTextBox.Size = new System.Drawing.Size(132, 20);
			this.SearchTextBox.TabIndex = 0;
			this.SearchTextBox.Enter += new System.EventHandler(this.ParentTreeSerahBox_Enter);
			this.SearchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyUp);
			this.SearchTextBox.Leave += new System.EventHandler(this.ParentTreeSerahBox_Leave);
			// 
			// SearchBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.SearchTextBox);
			this.MaximumSize = new System.Drawing.Size(10000, 21);
			this.MinimumSize = new System.Drawing.Size(0, 21);
			this.Name = "SearchBox";
			this.Size = new System.Drawing.Size(135, 21);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox SearchTextBox;
	}
}
