using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DocsViewer 
{
    public partial class ViewUsersForm : Form
    {
        private string basePath => AppConfig.GetDatabasePath();
        private readonly string loggedInUser;

        public ViewUsersForm(string loggedInUser)
        {
            this.loggedInUser = loggedInUser;

            // Inicialize o CategoryManager com as categorias e subcategorias
            var initialCategoriesWithSubmenus = GetCategoriesFromDirectory(AppConfig.GetDocumentsPath());
            CategoryManager.Initialize(initialCategoriesWithSubmenus);


            InitializeComponent();
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            LogoHelper.AplicarLogoComoIcon(this);
            UpdateAllButtonIcons();
            LoadUsers();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThemeManager.ApplyTheme(this);
        }


        private void LoadUsers()
        {
            string usersFilePath = Path.Combine(basePath, "users.json");
            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
            listBoxUsers.Items.Clear();
            foreach (var user in users)
            {
                if (user.Username != "dbadmin") // Filtro para não mostrar o dbadmin
                    listBoxUsers.Items.Add(user.Username);
            }
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnDeleteUser.Image = GetButtonImage("delete");
                btnSaveChanges.Image = GetButtonImage("save");

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

        private void ListBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedItem == null) return;

            string selectedUsername = listBoxUsers.SelectedItem.ToString();
            if (selectedUsername == "dbadmin")
            {
                MessageBox.Show("Usuário 'dbadmin' não pode ser editado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string usersFilePath = Path.Combine(basePath, "users.json");
            if (!File.Exists(usersFilePath))
                return;

            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
            var user = users.FirstOrDefault(u => u.Username == selectedUsername);
            if (user == null) return;

            txtUsername.Text = user.Username;
            txtPassword.Text = user.Password;
            cmbRole.SelectedItem = user.Role;

            string userPermissionsFilePath = Path.Combine(basePath, "userPermissions.json");
            if (File.Exists(userPermissionsFilePath))
            {
                var userPermissions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(userPermissionsFilePath));
                PopulateCheckedListBox();
                if (userPermissions.ContainsKey(selectedUsername))
                    CheckUserPermissions(userPermissions[selectedUsername]);
                else
                    CheckUserPermissions(new List<string>());
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

        private void CheckUserPermissions(List<string> userPermissions)
        {
            for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
            {
                string item = checkedListBoxCategories.Items[i].ToString().Trim();
                checkedListBoxCategories.SetItemChecked(i, userPermissions.Contains(item));
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            string selectedUsername = listBoxUsers.SelectedItem.ToString();
            string usersFilePath = Path.Combine(basePath, "users.json");
            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
            var user = users.First(u => u.Username == selectedUsername);

            user.Username = txtUsername.Text;
            user.Password = txtPassword.Text;
            user.Role = cmbRole.SelectedItem.ToString();

            var selectedPermissions = new List<string>();
            foreach (string item in checkedListBoxCategories.CheckedItems)
            {
                selectedPermissions.Add(item.Trim());
            }

            string userPermissionsFilePath = Path.Combine(basePath, "userPermissions.json");
            var userPermissions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(userPermissionsFilePath));
            userPermissions[selectedUsername] = selectedPermissions;

            File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
            File.WriteAllText(userPermissionsFilePath, JsonConvert.SerializeObject(userPermissions, Formatting.Indented));

            ActivityLogger.Log($"{loggedInUser} atualizou o usuário: {selectedUsername}", loggedInUser);

            MessageBox.Show("Usuário atualizado com sucesso!");
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedItem != null)
            {
                string selectedUsername = listBoxUsers.SelectedItem.ToString();
                string usersFilePath = Path.Combine(basePath, "users.json");
                var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
                string userPermissionsFilePath = Path.Combine(basePath, "userPermissions.json");
                var userPermissions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(userPermissionsFilePath));

                users.RemoveAll(u => u.Username == selectedUsername);
                userPermissions.Remove(selectedUsername);

                File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
                File.WriteAllText(userPermissionsFilePath, JsonConvert.SerializeObject(userPermissions, Formatting.Indented));

                LoadUsers();

                ActivityLogger.Log($"{loggedInUser}, excluiu o usuário: {selectedUsername}", loggedInUser);

                MessageBox.Show("Usuário excluído com sucesso!");
            }
            else
            {
                MessageBox.Show("Por favor, selecione um usuário para excluir.");
            }
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