namespace DocsViewer 
{
    partial class OnlineUsersForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxOnlineUsers;
        private System.Windows.Forms.Button btnUpdateList;
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
            this.btnUpdateList = new System.Windows.Forms.Button();
            this.btnLogoutUser = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxOnlineUsers
            // 
            this.listBoxOnlineUsers.FormattingEnabled = true;
            this.listBoxOnlineUsers.Location = new System.Drawing.Point(12, 12);
            this.listBoxOnlineUsers.Name = "listBoxOnlineUsers";
            this.listBoxOnlineUsers.Size = new System.Drawing.Size(300, 160);
            this.listBoxOnlineUsers.TabIndex = 0;
            this.listBoxOnlineUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxOnlineUsers_SelectedIndexChanged);
            // 
            // btnUpdateList
            // 
            this.btnUpdateList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateList.Location = new System.Drawing.Point(54, 178);
            this.btnUpdateList.Name = "btnUpdateList";
            this.btnUpdateList.Size = new System.Drawing.Size(90, 38);
            this.btnUpdateList.TabIndex = 1;
            this.btnUpdateList.Text = "Atualizar";
            this.btnUpdateList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateList.UseVisualStyleBackColor = true;
            this.btnUpdateList.Click += new System.EventHandler(this.BtnUpdateList_Click);
            // 
            // btnLogoutUser
            // 
            this.btnLogoutUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogoutUser.Location = new System.Drawing.Point(150, 178);
            this.btnLogoutUser.Name = "btnLogoutUser";
            this.btnLogoutUser.Size = new System.Drawing.Size(90, 38);
            this.btnLogoutUser.TabIndex = 2;
            this.btnLogoutUser.Text = "Deslogar";
            this.btnLogoutUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogoutUser.UseVisualStyleBackColor = true;
            this.btnLogoutUser.Click += new System.EventHandler(this.BtnLogoutUser_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(256, 181);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(38, 34);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // OnlineUsersForm
            // 
            this.ClientSize = new System.Drawing.Size(324, 221);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnLogoutUser);
            this.Controls.Add(this.btnUpdateList);
            this.Controls.Add(this.listBoxOnlineUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OnlineUsersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Usuários Online";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.PictureBox pictureBox1;
    }
}