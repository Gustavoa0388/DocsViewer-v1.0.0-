using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DocsViewer 
{
    public partial class ChangePasswordForm : Form
    {
        private readonly User loggedUser;
        private string basePath => AppConfig.GetDatabasePath();
        private readonly bool forceChange;
        private bool passwordChangedSuccessfully = false;
        private bool isPasswordVisible = false;

        public ChangePasswordForm(User user, bool forceChange = false)
        {
            InitializeComponent();
            loggedUser = user;
            this.forceChange = forceChange;
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            UpdateAllButtonIcons();
            LogoHelper.AplicarLogoComoIcon(this);

            var logoConfig = LogoConfig.LoadLogoConfig();
            if (logoConfig != null && !string.IsNullOrEmpty(logoConfig.LogoBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(logoConfig.LogoBase64);
                using (var ms = new MemoryStream(imageBytes))
                {
                    pictureBoxLogo.Image?.Dispose();
                    pictureBoxLogo.Image = new Bitmap(ms);
                }
            }
        }

        private void PictureBoxTogglePassword1_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtNewPassword.UseSystemPasswordChar = !isPasswordVisible;
            pictureBoxTogglePassword1.Image = isPasswordVisible
                ? Properties.Resources.eye_open
                : Properties.Resources.eye_closed;
        }

        private void PictureBoxTogglePassword2_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtConfirmPassword.UseSystemPasswordChar = !isPasswordVisible;
            pictureBoxTogglePassword2.Image = isPasswordVisible
                ? Properties.Resources.eye_open
                : Properties.Resources.eye_closed;
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnChangePassword.Image = GetButtonImage("change");
            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao atualizar ícones: {ex.Message}");
            }
        }

        private Image GetButtonImage(string baseName)
        {
            try
            {
                var resourceName = ThemeManager.IsDarkMode ? $"{baseName}_dark" : $"{baseName}_light";
                var image = (Image)Properties.Resources.ResourceManager.GetObject(resourceName);

                // Fallback para ícone padrão se a imagem não existir
                return image ?? SystemIcons.Information.ToBitmap();
            }
            catch
            {
                return SystemIcons.Information.ToBitmap();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThemeManager.ApplyTheme(this);
        }


        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("As senhas não coincidem. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("A nova senha não pode estar vazia.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string usersFilePath = Path.Combine(AppConfig.GetDatabasePath(), "users.json");
                var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));

                // Use StringComparison para garantir busca correta, sem erro de maiúscula/minúscula
                var user = users.FirstOrDefault(u => u.Username.Equals(loggedUser.Username, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    user.Password = newPassword;
                    user.FirstLogin = false;
                    // Sincroniza instância de fora também, só para garantir
                    loggedUser.Password = newPassword;
                    loggedUser.FirstLogin = false;

                    File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));

                    ActivityLogger.Log("Alterou a senha.", loggedUser.Username);
                    MessageBox.Show("Senha alterada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    passwordChangedSuccessfully = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado no arquivo de usuários!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao alterar a senha: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            // Implementação necessária
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (forceChange && !passwordChangedSuccessfully)
            {
                MessageBox.Show("Você deve alterar sua senha antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
    }
}