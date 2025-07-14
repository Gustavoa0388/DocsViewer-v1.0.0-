using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NPOI.OpenXmlFormats.Dml.Diagram;
using NPOI.OpenXmlFormats.Wordprocessing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DocsViewer 
{
    public partial class MoveFileForm : Form
    {
        private readonly Dictionary<string, List<string>> categoryMap;
        private readonly List<string> selectedFiles;
        private System.Windows.Forms.ToolTip toolTip3;

        public string SelectedCategory => comboBoxCategory.SelectedItem?.ToString();
        public string SelectedSubCategory => comboBoxSubCategory.SelectedItem?.ToString();

        public MoveFileForm(Dictionary<string, List<string>> categoriesAndSubs, List<string> filesToMove)
        {
            InitializeComponent();
            this.categoryMap = categoriesAndSubs ?? new Dictionary<string, List<string>>();
            this.selectedFiles = filesToMove ?? new List<string>();

            ThemeManager.ApplyTheme(this);
            LoadCategories();
            UpdateAllButtonIcons();
            ConfigurePlaceholders();
            LogoHelper.AplicarLogoComoIcon(this);

            // Carrega os arquivos imediatamente
            if (selectedFiles != null && selectedFiles.Count > 0)
            {
                listBoxFiles.Items.Clear();
                listBoxFiles.Items.AddRange(selectedFiles.ToArray());
            }

            toolTip3 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip3);

            // Tooltips para botões comuns
            toolTip3.SetToolTip(comboBoxCategory, "Filtrar Categoria de Arquivos");
            toolTip3.SetToolTip(comboBoxSubCategory, "Filtrar Sub Categoria de Arquivos");
            toolTip3.SetToolTip(btnMove, "Mover o Documento (ENTER)");
            
        }

        private void DocumentManagementForm_KeyDown(object sender, KeyEventArgs e)
        {

            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnMove.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
               
                }
            }
        
        private void ConfigurePlaceholders()
        {
            // Configurar placeholder da categoria 
            comboBoxCategory.Text = "Selecione uma Categoria...";
            comboBoxCategory.ForeColor = SystemColors.GrayText;
            comboBoxCategory.GotFocus += (s, e) =>
            {
                if (comboBoxCategory.Text == "Selecione uma Categoria...")
                {
                    comboBoxCategory.Text = "";
                    comboBoxCategory.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            comboBoxCategory.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(comboBoxCategory.Text))
                {
                    comboBoxCategory.Text = "Selecione uma Categoria...";
                    comboBoxCategory.ForeColor = SystemColors.GrayText;
                }
            };

            // Configurar placeholder da sub categoria
            comboBoxSubCategory.Text = "Selecione uma Sub Categoria...";
            comboBoxSubCategory.ForeColor = SystemColors.GrayText;
            comboBoxSubCategory.GotFocus += (s, e) =>
            {
                if (comboBoxSubCategory.Text == "Selecione uma Sub Categoria...")
                {
                    comboBoxSubCategory.Text = "";
                    comboBoxSubCategory.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            comboBoxSubCategory.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(comboBoxSubCategory.Text))
                {
                    comboBoxSubCategory.Text = "Selecione uma Sub Categoria...";
                    comboBoxSubCategory.ForeColor = SystemColors.GrayText;
                }
            };
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnMove.Image = GetButtonImage("move");
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
            comboBoxCategory.Items.Clear();
            comboBoxCategory.Items.AddRange(categoryMap.Keys.ToArray());

            if (comboBoxCategory.Items.Count > 0)
                comboBoxCategory.SelectedIndex = 0;
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBoxCategory.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(selected)) return;

            comboBoxSubCategory.Items.Clear();
            if (categoryMap.TryGetValue(selected, out var subcats))
            {
                comboBoxSubCategory.Items.AddRange(subcats.ToArray());
                if (comboBoxSubCategory.Items.Count > 0)
                    comboBoxSubCategory.SelectedIndex = 0;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SelectedCategory == null || SelectedSubCategory == null)
            {
                MessageBox.Show("Selecione uma categoria e subcategoria.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}