using System.Windows.Forms;

namespace DocsViewer 
{
    partial class ProgressPopupForm
    {
        private System.ComponentModel.IContainer components = null;
        private ProgressBar progressBar;
        private Label labelStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 20);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(360, 25);
            this.progressBar.TabIndex = 0;
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(20, 55);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(360, 20);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "Aguardando...";
            // 
            // ProgressPopupForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 89);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProgressPopupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progresso";
            this.ResumeLayout(false);

        }
    }
}
