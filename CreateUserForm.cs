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
    public partial class CreateUserForm : Form
    {
        private string basePath => AppConfig.GetDatabasePath();
        private readonly string loggedInUser;

        public CreateUserForm(string username)
        {
            InitializeComponent();
            loggedInUser = username;
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            UpdateAllButtonIcons();  // Depois atualiza os demais
            LogoHelper.AplicarLogoComoIcon(this);

            // Inicialize o CategoryManager com as categorias e subcategorias
            var initialCategoriesWithSubmenus = GetCategoriesFromDirectory(@"\\D4MDP574\Doc Viewer$\Documentos");
            CategoryManager.Initialize(initialCategoriesWithSubmenus);

            PopulateCheckedListBox();

            var logoConfig = LogoConfig.LoadLogoConfig();
            if (logoConfig != null && !string.IsNullOrEmpty(logoConfig.LogoBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(logoConfig.LogoBase64);
                using (var ms = new MemoryStream(imageBytes))
                {
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = new Bitmap(ms);
                }
            }
        }
        
        

    protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThemeManager.ApplyTheme(this);
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnCreateUser.Image = GetButtonImage("adduser");
                btnViewUsers.Image = GetButtonImage("view_user");
               
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

        private void PopulateCheckedListBox()
        {
            checkedListBoxCategories.Items.Clear();
            var categoriesWithSubmenus = CategoryManager.GetCategoriesWithSubmenus();
            foreach (var category in categoriesWithSubmenus.Keys)
            {
                checkedListBoxCategories.Items.Add(category);
                foreach (var subCategory in categoriesWithSubmenus[category])
                {
                    checkedListBoxCategories.Items.Add("  " + subCategory);
                }
            }
        }

        private void BtnCreateUser_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem.ToString();

            string usersFilePath = Path.Combine(basePath, "users.json");
            string userPermissionsFilePath = Path.Combine(basePath, "userPermissions.json");

            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
            if (username.ToLower() == "dbadmin")
            {
                MessageBox.Show("Não é permitido criar um usuário chamado 'dbadmin'.");
                return;
            }
            if (users.Any(u => u.Username == username))
            {
                MessageBox.Show("Usuário já existe!");
                return;
            }

            var selectedPermissions = new List<string>();
            foreach (string item in checkedListBoxCategories.CheckedItems)
            {
                selectedPermissions.Add(item.Trim());
            }

            var newUser = new User { Username = username, Password = password, Role = role };
            users.Add(newUser);

            var userPermissions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(userPermissionsFilePath));
            userPermissions[username] = selectedPermissions;

            File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
            File.WriteAllText(userPermissionsFilePath, JsonConvert.SerializeObject(userPermissions, Formatting.Indented));

            ActivityLogger.Log($"Usuário {username} criado com sucesso.", loggedInUser);
            MessageBox.Show("Usuário criado com sucesso!");
            this.Close();
        }

        private void BtnViewUsers_Click(object sender, EventArgs e)
        {
            ViewUsersForm viewUsersForm = new ViewUsersForm(loggedInUser);
            viewUsersForm.ShowDialog();
        }

        private Dictionary<string, List<string>> GetCategoriesFromDirectory(string path)
        {
            var categories = new Dictionary<string, List<string>>();

            if (Directory.Exists(path))
            {
                var categoryDirs = Directory.GetDirectories(path);
                foreach (var categoryDir in categoryDirs)
                {
                    var category = Path.GetFileName(categoryDir);
                    var subcategories = Directory.GetDirectories(categoryDir).Select(Path.GetFileName).ToList();
                    categories[category] = subcategories;
                }
            }

            return categories;
        }
    }
}