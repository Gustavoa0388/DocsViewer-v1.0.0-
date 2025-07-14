namespace DocsViewer 
{
    partial class UserLogoutForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxOnlineUsers;
        private System.Windows.Forms.Button btnLogoutUser;

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
            this.listBoxOnlineUsers = new System.Windows.Forms.ListBox();
            this.btnLogoutUser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxOnlineUsers
            // 
            this.listBoxOnlineUsers.FormattingEnabled = true;
            this.listBoxOnlineUsers.Location = new System.Drawing.Point(12, 12);
            this.listBoxOnlineUsers.Name = "listBoxOnlineUsers";
            this.listBoxOnlineUsers.Size = new System.Drawing.Size(200, 160);
            this.listBoxOnlineUsers.TabIndex = 0;
            // 
            // btnLogoutUser
            // 
            this.btnLogoutUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogoutUser.Location = new System.Drawing.Point(44, 180);
            this.btnLogoutUser.Name = "btnLogoutUser";
            this.btnLogoutUser.Size = new System.Drawing.Size(130, 38);
            this.btnLogoutUser.TabIndex = 1;
            this.btnLogoutUser.Text = "Deslogar Usuário";
            this.btnLogoutUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogoutUser.UseVisualStyleBackColor = true;
            this.btnLogoutUser.Click += new System.EventHandler(this.BtnLogoutUser_Click);
            // 
            // UserLogoutForm
            // 
            this.ClientSize = new System.Drawing.Size(224, 230);
            this.Controls.Add(this.btnLogoutUser);
            this.Controls.Add(this.listBoxOnlineUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserLogoutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deslogar Usuário";
            this.ResumeLayout(false);

        }
    }
}