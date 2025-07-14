using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DocsViewer 
{
    public partial class CategoryManagementForm : Form
    {
        private readonly string documentsPath;
        private readonly string loggedInUser;

        public CategoryManagementForm(string documentsPath, string loggedInUser)
        {
            InitializeComponent();
            this.documentsPath = documentsPath;
            this.loggedInUser = loggedInUser;
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            LoadCategories();
            UpdateAllButtonIcons();
            LogoHelper.AplicarLogoComoIcon(this);
        }



        private void UpdateAllButtonIcons()
        {
            try
        {
        btnAddCategory.Image = GetButtonImage("add");
        btnEditCategory.Image = GetButtonImage("rename");
        btnDeleteCategory.Image = GetButtonImage("delete");
        btnAddSubcategory.Image = GetButtonImage("add");
        btnEditSubcategory.Image = GetButtonImage("rename");
        btnDeleteSubcategory.Image = GetButtonImage("delete");
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




        private void LoadCategories()
        {
            listBoxCategories.Items.Clear();
            listBoxCategories.Items.AddRange(CategoryManager.LoadCategoriesFromDirectory(documentsPath).Keys.ToArray());
        }

        private void listBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = listBoxCategories.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(selectedCategory))
            {
                var subcategories = CategoryManager.GetSubcategories(selectedCategory);
                listBoxSubcategories.Items.Clear();
                listBoxSubcategories.Items.AddRange(subcategories.ToArray());
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string newCategory = DocumentManagementForm.Prompt.ShowDialog("Nova categoria:", "Adicionar Categoria");
            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                CategoryManager.AddCategory(documentsPath, newCategory);
                ActivityLogger.Log($"Adicionou a categoria '{newCategory}'", loggedInUser);
                LoadCategories();
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string selectedCategory = listBoxCategories.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCategory)) return;

            string newCategory = DocumentManagementForm.Prompt.ShowDialog("Novo nome da categoria:", "Editar Categoria", selectedCategory);
            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                CategoryManager.EditCategory(documentsPath, selectedCategory, newCategory);
                ActivityLogger.Log($"Renomeou categoria '{selectedCategory}' para '{newCategory}'", loggedInUser);
                LoadCategories();
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            string selectedCategory = listBoxCategories.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedCategory)) return;

            if (MessageBox.Show($"Excluir categoria '{selectedCategory}'?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CategoryManager.DeleteCategory(documentsPath, selectedCategory);
                ActivityLogger.Log($"Excluiu categoria '{selectedCategory}'", loggedInUser);
                LoadCategories();
            }
        }

        private void btnAddSubcategory_Click(object sender, EventArgs e)
        {
            string selectedCategory = listBoxCategories.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedCategory)) return;

            string subcategory = DocumentManagementForm.Prompt.ShowDialog("Nova subcategoria:", "Adicionar Subcategoria");
            if (!string.IsNullOrWhiteSpace(subcategory))
            {
                CategoryManager.AddSubcategory(documentsPath, selectedCategory, subcategory);
                ActivityLogger.Log($"Adicionou subcategoria '{subcategory}' à categoria '{selectedCategory}'", loggedInUser);
                listBoxCategories_SelectedIndexChanged(null, null);
            }
        }

        private void btnEditSubcategory_Click(object sender, EventArgs e)
        {
            string selectedCategory = listBoxCategories.SelectedItem?.ToString();
            string selectedSubcategory = listBoxSubcategories.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedCategory) || string.IsNullOrWhiteSpace(selectedSubcategory)) return;

            string newSub = DocumentManagementForm.Prompt.ShowDialog("Novo nome da subcategoria:", "Editar Subcategoria", selectedSubcategory);
            if (!string.IsNullOrWhiteSpace(newSub))
            {
                CategoryManager.EditSubcategory(documentsPath, selectedCategory, selectedSubcategory, newSub);
                ActivityLogger.Log($"Renomeou subcategoria '{selectedSubcategory}' para '{newSub}' na categoria '{selectedCategory}'", loggedInUser);
                listBoxCategories_SelectedIndexChanged(null, null);
            }
        }

        private void btnDeleteSubcategory_Click(object sender, EventArgs e)
        {
            string selectedCategory = listBoxCategories.SelectedItem?.ToString();
            string selectedSubcategory = listBoxSubcategories.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selectedCategory) || string.IsNullOrWhiteSpace(selectedSubcategory)) return;

            if (MessageBox.Show($"Excluir subcategoria '{selectedSubcategory}'?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CategoryManager.DeleteSubcategory(documentsPath, selectedCategory, selectedSubcategory);
                ActivityLogger.Log($"Excluiu subcategoria '{selectedSubcategory}' da categoria '{selectedCategory}'", loggedInUser);
                listBoxCategories_SelectedIndexChanged(null, null);
            }
        }
    }
}
