namespace DocsViewer 
{
    partial class ChangePasswordForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.PictureBox pictureBoxLogo; // PictureBox para a logo

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordForm));
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pictureBoxTogglePassword1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxTogglePassword2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTogglePassword1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTogglePassword2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(65, 130);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(70, 13);
            this.lblNewPassword.TabIndex = 0;
            this.lblNewPassword.Text = "Nova Senha:";
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(56, 178);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(88, 13);
            this.lblConfirmPassword.TabIndex = 2;
            this.lblConfirmPassword.Text = "Confirmar Senha:";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(18, 148);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(164, 20);
            this.txtNewPassword.TabIndex = 1;
            this.txtNewPassword.UseSystemPasswordChar = true;
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(18, 198);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(164, 20);
            this.txtConfirmPassword.TabIndex = 3;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChangePassword.Location = new System.Drawing.Point(60, 226);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(110, 38);
            this.btnChangePassword.TabIndex = 4;
            this.btnChangePassword.Text = "Alterar Senha";
            this.btnChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.BtnChangePassword_Click);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(55, 12);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(121, 100);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 5;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Click += new System.EventHandler(this.pictureBoxLogo_Click);
            // 
            // pictureBoxTogglePassword1
            // 
            this.pictureBoxTogglePassword1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxTogglePassword1.ErrorImage = global::DocsViewer.Properties.Resources.eye_closed1;
            this.pictureBoxTogglePassword1.Image = global::DocsViewer.Properties.Resources.eye_closed;
            this.pictureBoxTogglePassword1.Location = new System.Drawing.Point(188, 148);
            this.pictureBoxTogglePassword1.Name = "pictureBoxTogglePassword1";
            this.pictureBoxTogglePassword1.Size = new System.Drawing.Size(24, 20);
            this.pictureBoxTogglePassword1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTogglePassword1.TabIndex = 6;
            this.pictureBoxTogglePassword1.TabStop = false;
            this.pictureBoxTogglePassword1.Click += new System.EventHandler(this.PictureBoxTogglePassword1_Click);
            // 
            // pictureBoxTogglePassword2
            // 
            this.pictureBoxTogglePassword2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxTogglePassword2.ErrorImage = global::DocsViewer.Properties.Resources.eye_closed1;
            this.pictureBoxTogglePassword2.Image = global::DocsViewer.Properties.Resources.eye_closed;
            this.pictureBoxTogglePassword2.Location = new System.Drawing.Point(188, 198);
            this.pictureBoxTogglePassword2.Name = "pictureBoxTogglePassword2";
            this.pictureBoxTogglePassword2.Size = new System.Drawing.Size(24, 20);
            this.pictureBoxTogglePassword2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTogglePassword2.TabIndex = 7;
            this.pictureBoxTogglePassword2.TabStop = false;
            this.pictureBoxTogglePassword2.Click += new System.EventHandler(this.PictureBoxTogglePassword2_Click);
            // 
            // ChangePasswordForm
            // 
            this.AcceptButton = this.btnChangePassword;
            this.ClientSize = new System.Drawing.Size(231, 275);
            this.Controls.Add(this.pictureBoxTogglePassword2);
            this.Controls.Add(this.pictureBoxTogglePassword1);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblNewPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alterar Senha";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTogglePassword1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTogglePassword2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox pictureBoxTogglePassword1;
        private System.Windows.Forms.PictureBox pictureBoxTogglePassword2;
    }
}