namespace DocsViewer 
{
    partial class ManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.Button btnOnlineUsers;
        private System.Windows.Forms.Button btnManageDocuments;        
        private System.Windows.Forms.Button btnExportUserReport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog; // Dialog para escolher onde salvar os relatórios

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagementForm));
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnOnlineUsers = new System.Windows.Forms.Button();
            this.btnManageDocuments = new System.Windows.Forms.Button();
            this.btnExportUserReport = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnDocReport = new System.Windows.Forms.Button();
            this.btnLogReport = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateUser.Location = new System.Drawing.Point(27, 158);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(120, 38);
            this.btnCreateUser.TabIndex = 0;
            this.btnCreateUser.Text = "Criar Usuário";
            this.btnCreateUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCreateUser.UseVisualStyleBackColor = true;
            this.btnCreateUser.Click += new System.EventHandler(this.BtnCreateUser_Click);
            // 
            // btnOnlineUsers
            // 
            this.btnOnlineUsers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOnlineUsers.Location = new System.Drawing.Point(27, 202);
            this.btnOnlineUsers.Name = "btnOnlineUsers";
            this.btnOnlineUsers.Size = new System.Drawing.Size(120, 38);
            this.btnOnlineUsers.TabIndex = 2;
            this.btnOnlineUsers.Text = "Usuários Online";
            this.btnOnlineUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOnlineUsers.UseVisualStyleBackColor = true;
            this.btnOnlineUsers.Click += new System.EventHandler(this.BtnOnlineUsers_Click);
            // 
            // btnManageDocuments
            // 
            this.btnManageDocuments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageDocuments.Location = new System.Drawing.Point(91, 301);
            this.btnManageDocuments.Name = "btnManageDocuments";
            this.btnManageDocuments.Size = new System.Drawing.Size(130, 38);
            this.btnManageDocuments.TabIndex = 3;
            this.btnManageDocuments.Text = "Gerenciamento\nde Documentos";
            this.btnManageDocuments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnManageDocuments.UseVisualStyleBackColor = true;
            this.btnManageDocuments.Click += new System.EventHandler(this.BtnManageDocuments_Click);
            // 
            // btnExportUserReport
            // 
            this.btnExportUserReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportUserReport.Location = new System.Drawing.Point(165, 158);
            this.btnExportUserReport.Name = "btnExportUserReport";
            this.btnExportUserReport.Size = new System.Drawing.Size(120, 38);
            this.btnExportUserReport.TabIndex = 5;
            this.btnExportUserReport.Text = "Relatório\n de Usuários";
            this.btnExportUserReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportUserReport.UseVisualStyleBackColor = true;
            this.btnExportUserReport.Click += new System.EventHandler(this.BtnExportUserReport_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfig.Location = new System.Drawing.Point(27, 252);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(120, 38);
            this.btnConfig.TabIndex = 6;
            this.btnConfig.Text = "Configurações";
            this.btnConfig.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnDocReport
            // 
            this.btnDocReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDocReport.Location = new System.Drawing.Point(165, 252);
            this.btnDocReport.Name = "btnDocReport";
            this.btnDocReport.Size = new System.Drawing.Size(120, 38);
            this.btnDocReport.TabIndex = 7;
            this.btnDocReport.Text = "Relatório de Documentos";
            this.btnDocReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDocReport.UseVisualStyleBackColor = true;
            this.btnDocReport.Click += new System.EventHandler(this.btnDocReport_Click);
            // 
            // btnLogReport
            // 
            this.btnLogReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogReport.Location = new System.Drawing.Point(165, 202);
            this.btnLogReport.Name = "btnLogReport";
            this.btnLogReport.Size = new System.Drawing.Size(120, 38);
            this.btnLogReport.TabIndex = 8;
            this.btnLogReport.Text = "Relatório \nde Logs";
            this.btnLogReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogReport.UseVisualStyleBackColor = true;
            this.btnLogReport.Click += new System.EventHandler(this.btnLogReport_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(88, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 122);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // ManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(313, 351);
            this.Controls.Add(this.btnLogReport);
            this.Controls.Add(this.btnDocReport);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnExportUserReport);
            this.Controls.Add(this.btnManageDocuments);
            this.Controls.Add(this.btnOnlineUsers);
            this.Controls.Add(this.btnCreateUser);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = resources.GetString("$this.Text");
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManagementForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnDocReport;
        private System.Windows.Forms.Button btnLogReport;
    }
}