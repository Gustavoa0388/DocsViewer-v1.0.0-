using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace DocsViewer 
{
    public partial class OnlineUsersForm : Form
    {
        private List<UserLoginDetail> currentUserDetails;
        private readonly string adminUsername;
        private string basePath => AppConfig.GetDatabasePath();


        public OnlineUsersForm(string adminUsername)
        {
            InitializeComponent();
            this.adminUsername = adminUsername;
            LoadCurrentUserDetails();
            PopulateOnlineUsersList();
            UpdateAllButtonIcons();
            LogoHelper.AplicarLogoComoIcon(this);
        }

    
        private void LoadCurrentUserDetails()
        {
            string currentUserDetailsFilePath = Path.Combine(basePath, "currentUsers.json");

            if (File.Exists(currentUserDetailsFilePath))
            {
                currentUserDetails = JsonConvert.DeserializeObject<List<UserLoginDetail>>(File.ReadAllText(currentUserDetailsFilePath));
            }
            else
            {
                currentUserDetails = new List<UserLoginDetail>();
            }
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnUpdateList.Image = GetButtonImage("updates");
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

        private void PopulateOnlineUsersList()
        {
            listBoxOnlineUsers.Items.Clear();
            foreach (var user in currentUserDetails)
            {
                var onlineTime = (DateTime.Now - DateTime.ParseExact(user.LoginTime, "dd-MM-yyyy HH:mm:ss", null)).ToString(@"hh\:mm\:ss");
                listBoxOnlineUsers.Items.Add($"{user.Username}, {user.IPAddress}, {user.LoginTime}, {onlineTime}");
            }
        }

        private void BtnUpdateList_Click(object sender, EventArgs e)
        {
            // Atualizar a lista de usuários manualmente
            LoadCurrentUserDetails();
            PopulateOnlineUsersList();
        }

        private void BtnLogoutUser_Click(object sender, EventArgs e)
        {
            // Abre o formulário de deslogar usuários
            var frm = new UserLogoutForm(adminUsername);
            frm.ShowDialog();
        }

        private void listBoxOnlineUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementação necessária
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (ThemeManager.IsDarkMode)
            {
                ThemeManager.ApplyDarkTheme(this);
            }
        }
    }
}