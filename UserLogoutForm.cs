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
    public partial class UserLogoutForm : Form
    {
        private Dictionary<string, bool> userLoginStatus;
        private readonly User loggedUser;
        private readonly string adminUsername;
        private string basePath => AppConfig.GetDatabasePath();

        public UserLogoutForm(string adminUsername)
        {
            InitializeComponent();
            this.adminUsername = adminUsername;
            LoadUserLoginStatus();
            PopulateOnlineUsersList();
            UpdateAllButtonIcons();
            LogoHelper.AplicarLogoComoIcon(this);
            ThemeManager.ApplyTheme(this); // Adicione esta linha
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (ThemeManager.IsDarkMode)
            {
                ThemeManager.ApplyDarkTheme(this);
            }
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnLogoutUser.Image = GetButtonImage("logout1");


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

        private void LoadUserLoginStatus()
        {
            string userLoginStatusFilePath = Path.Combine(basePath, "userLoginStatus.json");

            if (File.Exists(userLoginStatusFilePath))
            {
                userLoginStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(File.ReadAllText(userLoginStatusFilePath));
            }
            else
            {
                userLoginStatus = new Dictionary<string, bool>();
                MessageBox.Show("Arquivo de status de login não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateOnlineUsersList()
        {
            listBoxOnlineUsers.Items.Clear();

            // Filtra os usuários que estão online (onde o valor é true)
            var onlineUsers = userLoginStatus.Where(u => u.Value).Select(u => u.Key).ToList();

            foreach (var user in onlineUsers)
            {
                listBoxOnlineUsers.Items.Add(user);
            }
        }

        private void BtnLogoutUser_Click(object sender, EventArgs e)
        {
            if (listBoxOnlineUsers.SelectedItem != null)
            {
                string selectedUser = listBoxOnlineUsers.SelectedItem.ToString();
             

                // Confirma se o administrador deseja deslogar o usuário
                var confirmResult = MessageBox.Show($"Tem certeza que deseja deslogar o usuário {selectedUser}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    // Altera o status do usuário para false
                    userLoginStatus[selectedUser] = false;
                    SaveUserLoginStatus();
                    ActivityLogger.Log($"{adminUsername} deslogou o usuário {selectedUser}", adminUsername);

                    // Atualiza a lista de usuários online
                    PopulateOnlineUsersList();
                    
                    MessageBox.Show($"Usuário {selectedUser} foi deslogado com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um usuário para deslogar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUserLoginStatus()
        {
            try
            {
                string userLoginStatusFilePath = Path.Combine(basePath, "userLoginStatus.json");
                File.WriteAllText(userLoginStatusFilePath, JsonConvert.SerializeObject(userLoginStatus, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar status de login: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogActivity(string activity, User loggedUser)
        {
            string logMessage = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - {GetLocalIPAddress()} - {loggedUser.Username} - {activity}";
            try
            {
                string logFilePath = Path.Combine(basePath, "activity_log.txt");
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar atividade: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP Address Not Found!";
        }
    }
}