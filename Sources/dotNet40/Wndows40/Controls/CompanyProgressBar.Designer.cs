namespace Library40.Win.Controls
{
    partial class CompanyProgressBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CompanyProgressBar
            // 
            this.Name = "CompanyProgressBar";
            this.Size = new System.Drawing.Size(264, 32);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ProgressBar_Paint);
            this.ResumeLayout(false);

        } 

    }
}