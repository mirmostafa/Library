namespace Library35.Windows.Forms.Internals
{
	partial class SqlConnectionStringDialogForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.GroupBox groupBox1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlConnectionStringDialogForm));
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.GroupBox groupBox2;
			System.Windows.Forms.TabPage tabPage1;
			System.Windows.Forms.TabControl tabControl1;
			this.userInfoPanel = new System.Windows.Forms.Panel();
			this.savePassCheckBox = new System.Windows.Forms.CheckBox();
			this.userNameTextBox = new System.Windows.Forms.TextBox();
			this.showPassCheckBox = new System.Windows.Forms.CheckBox();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.authUserPassRadioButton = new System.Windows.Forms.RadioButton();
			this.authWindowsRadioButton = new System.Windows.Forms.RadioButton();
			this.serversComboBox = new System.Windows.Forms.ComboBox();
			this.refreshButton = new System.Windows.Forms.Button();
			this.selectDbGroupBox = new System.Windows.Forms.GroupBox();
			this.dbsComboBox = new System.Windows.Forms.ComboBox();
			this.promptLabel = new System.Windows.Forms.Label();
			this.testConnectionButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			tabPage1 = new System.Windows.Forms.TabPage();
			tabControl1 = new System.Windows.Forms.TabControl();
			groupBox1.SuspendLayout();
			this.userInfoPanel.SuspendLayout();
			groupBox2.SuspendLayout();
			tabPage1.SuspendLayout();
			this.selectDbGroupBox.SuspendLayout();
			tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			resources.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Controls.Add(this.userInfoPanel);
			groupBox1.Controls.Add(this.authUserPassRadioButton);
			groupBox1.Controls.Add(this.authWindowsRadioButton);
			groupBox1.Name = "groupBox1";
			groupBox1.TabStop = false;
			// 
			// userInfoPanel
			// 
			resources.ApplyResources(this.userInfoPanel, "userInfoPanel");
			this.userInfoPanel.Controls.Add(label1);
			this.userInfoPanel.Controls.Add(this.savePassCheckBox);
			this.userInfoPanel.Controls.Add(this.userNameTextBox);
			this.userInfoPanel.Controls.Add(this.showPassCheckBox);
			this.userInfoPanel.Controls.Add(label2);
			this.userInfoPanel.Controls.Add(this.passwordTextBox);
			this.userInfoPanel.Name = "userInfoPanel";
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// savePassCheckBox
			// 
			resources.ApplyResources(this.savePassCheckBox, "savePassCheckBox");
			this.savePassCheckBox.Checked = true;
			this.savePassCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.savePassCheckBox.Name = "savePassCheckBox";
			this.savePassCheckBox.UseVisualStyleBackColor = true;
			// 
			// userNameTextBox
			// 
			resources.ApplyResources(this.userNameTextBox, "userNameTextBox");
			this.userNameTextBox.Name = "userNameTextBox";
			this.userNameTextBox.TextChanged += new System.EventHandler(this.userNameTextBox_TextChanged);
			// 
			// showPassCheckBox
			// 
			resources.ApplyResources(this.showPassCheckBox, "showPassCheckBox");
			this.showPassCheckBox.Name = "showPassCheckBox";
			this.showPassCheckBox.UseVisualStyleBackColor = true;
			this.showPassCheckBox.CheckedChanged += new System.EventHandler(this.showPassCheckBox_CheckedChanged);
			// 
			// label2
			// 
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			// 
			// passwordTextBox
			// 
			resources.ApplyResources(this.passwordTextBox, "passwordTextBox");
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.UseSystemPasswordChar = true;
			// 
			// authUserPassRadioButton
			// 
			resources.ApplyResources(this.authUserPassRadioButton, "authUserPassRadioButton");
			this.authUserPassRadioButton.Checked = true;
			this.authUserPassRadioButton.Name = "authUserPassRadioButton";
			this.authUserPassRadioButton.TabStop = true;
			this.authUserPassRadioButton.UseVisualStyleBackColor = true;
			this.authUserPassRadioButton.CheckedChanged += new System.EventHandler(this.authUserPassRadioButton_CheckedChanged);
			// 
			// authWindowsRadioButton
			// 
			resources.ApplyResources(this.authWindowsRadioButton, "authWindowsRadioButton");
			this.authWindowsRadioButton.Name = "authWindowsRadioButton";
			this.authWindowsRadioButton.UseVisualStyleBackColor = true;
			this.authWindowsRadioButton.CheckedChanged += new System.EventHandler(this.authWindowsRadioButton_CheckedChanged);
			// 
			// groupBox2
			// 
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Controls.Add(this.serversComboBox);
			groupBox2.Controls.Add(this.refreshButton);
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			// 
			// serversComboBox
			// 
			resources.ApplyResources(this.serversComboBox, "serversComboBox");
			this.serversComboBox.FormattingEnabled = true;
			this.serversComboBox.Name = "serversComboBox";
			this.serversComboBox.DropDown += new System.EventHandler(this.serversComboBox_DropDown);
			// 
			// refreshButton
			// 
			resources.ApplyResources(this.refreshButton, "refreshButton");
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// tabPage1
			// 
			resources.ApplyResources(tabPage1, "tabPage1");
			tabPage1.Controls.Add(groupBox2);
			tabPage1.Controls.Add(groupBox1);
			tabPage1.Controls.Add(this.selectDbGroupBox);
			tabPage1.Name = "tabPage1";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// selectDbGroupBox
			// 
			resources.ApplyResources(this.selectDbGroupBox, "selectDbGroupBox");
			this.selectDbGroupBox.Controls.Add(this.dbsComboBox);
			this.selectDbGroupBox.Name = "selectDbGroupBox";
			this.selectDbGroupBox.TabStop = false;
			// 
			// dbsComboBox
			// 
			resources.ApplyResources(this.dbsComboBox, "dbsComboBox");
			this.dbsComboBox.FormattingEnabled = true;
			this.dbsComboBox.Name = "dbsComboBox";
			this.dbsComboBox.DropDown += new System.EventHandler(this.dbsComboBox_DropDown);
			// 
			// tabControl1
			// 
			resources.ApplyResources(tabControl1, "tabControl1");
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			// 
			// promptLabel
			// 
			resources.ApplyResources(this.promptLabel, "promptLabel");
			this.promptLabel.Name = "promptLabel";
			// 
			// testConnectionButton
			// 
			resources.ApplyResources(this.testConnectionButton, "testConnectionButton");
			this.testConnectionButton.Name = "testConnectionButton";
			this.testConnectionButton.UseVisualStyleBackColor = true;
			this.testConnectionButton.Click += new System.EventHandler(this.testConnectionButton_Click);
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// SqlConnectionStringDialogForm
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.Controls.Add(tabControl1);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.testConnectionButton);
			this.Controls.Add(this.promptLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SqlConnectionStringDialogForm";
			this.ShowInTaskbar = false;
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			this.userInfoPanel.ResumeLayout(false);
			this.userInfoPanel.PerformLayout();
			groupBox2.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			this.selectDbGroupBox.ResumeLayout(false);
			tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.CheckBox savePassCheckBox;
		private System.Windows.Forms.CheckBox showPassCheckBox;
		private System.Windows.Forms.Button testConnectionButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		internal System.Windows.Forms.Label promptLabel;
		internal System.Windows.Forms.ComboBox serversComboBox;
		internal System.Windows.Forms.RadioButton authWindowsRadioButton;
		internal System.Windows.Forms.RadioButton authUserPassRadioButton;
		internal System.Windows.Forms.TextBox passwordTextBox;
		internal System.Windows.Forms.TextBox userNameTextBox;
		internal System.Windows.Forms.ComboBox dbsComboBox;
		private System.Windows.Forms.Panel userInfoPanel;
		private System.Windows.Forms.GroupBox selectDbGroupBox;
	}
}