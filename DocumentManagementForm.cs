using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;


namespace DocsViewer
{
    public partial class DocumentManagementForm : Form
    {
        private readonly string basePath;
        private readonly string loggedInUser;
        private bool isDarkMode = false;
        private readonly User loggedUser;
        private System.Timers.Timer inactivityTimer;
        private readonly Dictionary<string, List<string>> categoriesWithSubmenus;
        private System.Windows.Forms.ToolTip toolTip;

        private string documentsPath => AppConfig.GetDocumentsPath();



        public DocumentManagementForm(string basePath, string loggedInUser, Dictionary<string, List<string>> categoriesWithSubmenus)
        {
            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new PdfHelper.SimpleFontResolver();

            this.basePath = basePath;
            this.loggedInUser = loggedInUser;
            this.categoriesWithSubmenus = categoriesWithSubmenus;
            InitializeComponent();
            InitializeComboBoxes(cmbCategory, cmbSubCategory);
            InitializeComboBoxes(comboBoxCategorySearch, comboBoxSubCategorySearch);
            LoadCategoriesIntoComboBoxes();
            UpdateAllButtonIcons();  // Depois atualiza os demais
            ConfigureSearchPlaceholder();
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            LogoHelper.AplicarLogoComoIcon(this);
            //listBoxFiles.SelectedIndexChanged += ListBoxFiles_SelectedIndexChanged;
            this.listBoxFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxFiles_MouseDoubleClick);
            lstFiles.SelectedIndexChanged += LstFiles_SelectedIndexChanged;

            this.AcceptButton = null;
            //this.AcceptButton = btnSearch;
            toolTip = new System.Windows.Forms.ToolTip();

            ThemeManager.ApplyToolTipTheme(toolTip);

            // Tooltips para botões comuns
            toolTip.SetToolTip(btnSaveFiles, "Salva os Arquivos (F2)");
            toolTip.SetToolTip(btnLoadFiles, "Carrega Arquivos(F3)");
            toolTip.SetToolTip(btnClearFiles, "Limpa a Lista de Arquivos(F4)");
            toolTip.SetToolTip(btnDeleteFiles, "Apaga os Arquivos Selecionados(F5)");
            toolTip.SetToolTip(btnMoveFile, "Move os Arquivos Selecionados(F6)");
            toolTip.SetToolTip(btnRenameFile1, "Renomeia o Arquivo Selecionado(F7)");
            toolTip.SetToolTip(btnSearch, "Pesquisar Arquivos(ENTER)");

            textBoxSearch.Enter += (s, e) => this.AcceptButton = btnSearch;
        }
        private void DocumentManagementForm_KeyDown(object sender, KeyEventArgs e)
        {

            {
                if (e.KeyCode == Keys.F2)
                {
                    btnSaveFiles.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F3)
                {
                    btnLoadFiles.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F4)
                {
                    btnClearFiles.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F5)
                {
                    btnDeleteFiles.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F6)
                {
                    btnMoveFile.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F7)
                {
                    btnRenameFile1.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.Enter)
                {
                    btnSearch.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
            }
        }

        private string GetMetadataPath(string nomeArquivo)
        {
            string metadataDir = AppConfig.GetMetadataPath();
            Directory.CreateDirectory(metadataDir); // Garante que existe
            string nomeJson = Path.GetFileNameWithoutExtension(nomeArquivo) + ".json";
            return Path.Combine(metadataDir, nomeJson);
        }

        // Salva ou atualiza o metadado JSON de um documento
        private void SalvarMetadadoDocumento(Documento doc)
        {
            string jsonPath = GetMetadataPath(doc.Nome);
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(doc, Formatting.Indented));
        }

        // Exclui o metadado JSON de um documento
        private void ExcluirMetadadoDocumento(string nomeArquivo)
        {
            string jsonPath = GetMetadataPath(nomeArquivo);
            if (File.Exists(jsonPath))
                File.Delete(jsonPath);
        }

        // Atualiza nome do metadado quando renomear arquivo
        private void RenomearMetadadoDocumento(string nomeAntigo, string nomeNovo)
        {
            string oldJson = GetMetadataPath(nomeAntigo);
            string newJson = GetMetadataPath(nomeNovo);
            if (File.Exists(oldJson))
                File.Move(oldJson, newJson);

            // Atualiza o nome dentro do JSON
            if (File.Exists(newJson))
            {
                var doc = JsonConvert.DeserializeObject<Documento>(File.ReadAllText(newJson));
                doc.Nome = nomeNovo;
                File.WriteAllText(newJson, JsonConvert.SerializeObject(doc, Formatting.Indented));
            }
        }

        // Atualiza categoria/subcategoria do metadado (em caso de mover)
        private void AtualizarCategoriaMetadado(string nomeArquivo, string novaCategoria, string novaSubcategoria)
        {
            string jsonPath = GetMetadataPath(nomeArquivo);
            if (File.Exists(jsonPath))
            {
                var doc = JsonConvert.DeserializeObject<Documento>(File.ReadAllText(jsonPath));
                doc.Categoria = novaCategoria;
                doc.Subcategoria = novaSubcategoria;
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(doc, Formatting.Indented));
            }
        }

        private void SetupInactivityTimer()
        {
            inactivityTimer = new System.Timers.Timer(15 * 60 * 1000); // 15 minutes
            inactivityTimer.Elapsed += OnInactivity;
            inactivityTimer.AutoReset = false;
            inactivityTimer.Start();

            this.MouseMove += ResetInactivityTimer;
            this.KeyPress += ResetInactivityTimer;
        }
        private void AddInactivityHandlers(Control parent)
        {
            parent.MouseMove += ResetInactivityTimer;
            parent.KeyPress += ResetInactivityTimer;

            foreach (Control ctrl in parent.Controls)
            {
                AddInactivityHandlers(ctrl); // recursivo, pega todos os filhos
            }
        }

        private void OnInactivity(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => RealizarLogout()));
            }
            else
            {
                RealizarLogout();
            }
        }
        private void ResetInactivityTimer(object sender, EventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void DocumentManagementForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);

            if (ThemeManager.IsDarkMode)
            {
                ApplyDarkMenuTheme();
            }
        }
        private void UpdateAllButtonIcons()
        {
            try
            {
                btnSearch.Image = GetButtonImage("search");
                btnLoadFiles.Image = GetButtonImage("load_File");
                btnSaveFiles.Image = GetButtonImage("save");
                btnClearFiles.Image = GetButtonImage("clean");
                btnDeleteFiles.Image = GetButtonImage("delete");
                btnMoveFile.Image = GetButtonImage("move");
                btnRenameFile1.Image = GetButtonImage("rename");

            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao atualizar ícones: {ex.Message}");
            }
        }

        private System.Drawing.Image GetButtonImage(string baseName)
        {
            try
            {
                var resourceName = ThemeManager.IsDarkMode ? $"{baseName}_dark" : $"{baseName}_light";
                var image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject(resourceName);

                // Fallback para ícone padrão se a imagem não existir
                return image ?? SystemIcons.Information.ToBitmap();
            }
            catch
            {
                return SystemIcons.Information.ToBitmap();
            }
        }


        private void ConfigureSearchPlaceholder()
        {

            // Configurar placeholder da busca
            textBoxSearch.Text = "Pesquisar Arquivos...";
            textBoxSearch.ForeColor = SystemColors.GrayText;
            textBoxSearch.GotFocus += (s, e) =>
            {
                if (textBoxSearch.Text == "Pesquisar Arquivos...")
                {
                    textBoxSearch.Text = "";
                    textBoxSearch.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            textBoxSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
                {
                    textBoxSearch.Text = "Pesquisar Arquivos...";
                    textBoxSearch.ForeColor = SystemColors.GrayText;
                }
            };

            // Configurar placeholder da categoria add
            cmbCategory.Text = "Selecione uma Categoria...";
            cmbCategory.ForeColor = SystemColors.GrayText;
            cmbCategory.GotFocus += (s, e) =>
            {
                if (cmbCategory.Text == "Selecione uma Categoria...")
                {
                    cmbCategory.Text = "";
                    cmbCategory.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            cmbCategory.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cmbCategory.Text))
                {
                    cmbCategory.Text = "Selecione uma Categoria...";
                    cmbCategory.ForeColor = SystemColors.GrayText;
                }
            };

            // Configurar placeholder da sub categoria add
            cmbSubCategory.Text = "Selecione uma Sub Categoria...";
            cmbSubCategory.ForeColor = SystemColors.GrayText;
            cmbSubCategory.GotFocus += (s, e) =>
            {
                if (cmbSubCategory.Text == "Selecione uma Sub Categoria...")
                {
                    cmbSubCategory.Text = "";
                    cmbSubCategory.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            cmbSubCategory.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cmbSubCategory.Text))
                {
                    cmbSubCategory.Text = "Selecione uma Sub Categoria...";
                    cmbSubCategory.ForeColor = SystemColors.GrayText;
                }
            };

            // Configurar placeholder da categoria edit
            comboBoxCategorySearch.Text = "Selecione uma Sub Categoria...";
            comboBoxCategorySearch.ForeColor = SystemColors.GrayText;
            comboBoxCategorySearch.GotFocus += (s, e) =>
            {
                if (comboBoxCategorySearch.Text == "Selecione uma Sub Categoria...")
                {
                    comboBoxCategorySearch.Text = "";
                    comboBoxCategorySearch.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            comboBoxCategorySearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(comboBoxCategorySearch.Text))
                {
                    comboBoxCategorySearch.Text = "Selecione uma Sub Categoria...";
                    comboBoxCategorySearch.ForeColor = SystemColors.GrayText;
                }
            };


            // Configurar placeholder da sub categoria edit
            comboBoxSubCategorySearch.Text = "Selecione uma Sub Categoria...";
            comboBoxSubCategorySearch.ForeColor = SystemColors.GrayText;
            comboBoxSubCategorySearch.GotFocus += (s, e) =>
            {
                if (comboBoxSubCategorySearch.Text == "Selecione uma Sub Categoria...")
                {
                    comboBoxSubCategorySearch.Text = "";
                    comboBoxSubCategorySearch.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            comboBoxSubCategorySearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(comboBoxSubCategorySearch.Text))
                {
                    comboBoxSubCategorySearch.Text = "Selecione uma Sub Categoria...";
                    comboBoxSubCategorySearch.ForeColor = SystemColors.GrayText;
                }
            };
        }
        private void CriarCategoriaMenuItem_Click(object sender, EventArgs e)
        {

            using (var form = new CategoryManagementForm(documentsPath, loggedInUser))
            {
                //ThemeManager.ApplyTheme(form);
                form.ShowDialog();
            }
        }

        private void InitializeComboBoxes(ComboBox comboBoxCategory, ComboBox comboBoxSubCategory)
        {
            comboBoxCategory.SelectedIndexChanged += (sender, e) =>
            {
                var selectedCategory = comboBoxCategory.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    UpdateSubCategoryComboBox(comboBoxSubCategory, selectedCategory);
                }
            };
        }

        private void LoadCategoriesIntoComboBoxes()
        {
            {
                // Carrega categorias nas ComboBoxes
                var categories = GetCategories();

                cmbCategory.Items.Clear();
                comboBoxCategorySearch.Items.Clear();

                cmbCategory.Items.AddRange(categories.ToArray());
                comboBoxCategorySearch.Items.Add("All Categories");
                comboBoxCategorySearch.Items.AddRange(categories.ToArray());

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;

                if (comboBoxCategorySearch.Items.Count > 0)
                    comboBoxCategorySearch.SelectedIndex = 0;

                // Carrega subcategorias correspondentes à primeira categoria
                if (cmbCategory.SelectedItem != null)
                {
                    UpdateSubCategoryComboBox(cmbSubCategory, cmbCategory.SelectedItem.ToString());
                }

                if (comboBoxCategorySearch.SelectedItem != null)
                {
                    UpdateSubCategoryComboBox(comboBoxSubCategorySearch, comboBoxCategorySearch.SelectedItem.ToString());
                }
            }
        }
        private void SalvarNovoDocumento(string filePath, string categoria, string subcategoria, string usuario)
        {
            var doc = new Documento
            {
                Nome = Path.GetFileName(filePath),
                Categoria = categoria,
                Subcategoria = subcategoria,
                Data = DateTime.Now,
                Usuario = usuario,
                TamanhoMb = Math.Round(new FileInfo(filePath).Length / 1024.0 / 1024.0, 2)
            };
            SalvarMetadadoDocumento(doc);
        }

        // 2. RENOMEAR DOCUMENTO
        private void RenomearDocumento(string caminhoAntigo, string caminhoNovo)
        {
            string nomeAntigo = Path.GetFileName(caminhoAntigo);
            string nomeNovo = Path.GetFileName(caminhoNovo);
            RenomearMetadadoDocumento(nomeAntigo, nomeNovo);
        }

        // 3. MOVER DOCUMENTO
        private void MoverDocumento(string filePath, string novaCategoria, string novaSubcategoria)
        {
            string nomeArquivo = Path.GetFileName(filePath);
            AtualizarCategoriaMetadado(nomeArquivo, novaCategoria, novaSubcategoria);
        }

        // 4. EXCLUIR DOCUMENTO
        private void ExcluirDocumento(string filePath)
        {
            string nomeArquivo = Path.GetFileName(filePath);
            ExcluirMetadadoDocumento(nomeArquivo);
        }

        private void SaveUserPreferences()
        {
            string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
            var userPreferences = File.Exists(userPreferencesFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath))
                : new Dictionary<string, UserPreferences>();

            userPreferences[loggedInUser] = new UserPreferences
            {
                IsDarkMode = isDarkMode,
                IsSimpleView = true // As this is the DocumentViewerForm, it's the simple view
            };

            File.WriteAllText(userPreferencesFilePath, JsonConvert.SerializeObject(userPreferences, Formatting.Indented));
        }

        private void BtnToggleDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            ToggleDarkMode(isDarkMode);
            SaveUserPreferences();
        }

        private void ToggleDarkMode(bool darkMode)
        {
            var backColor = darkMode ? System.Drawing.Color.FromArgb(45, 45, 48) : System.Drawing.Color.White;
            var foreColor = darkMode ? System.Drawing.Color.White : System.Drawing.Color.Black;

            this.BackColor = backColor;
            this.ForeColor = foreColor;

            foreach (Control control in this.Controls)
            {
                ToggleControlDarkMode(control, darkMode);
            }
        }

        private void ToggleControlDarkMode(Control control, bool darkMode)
        {
            var backColor = darkMode ? System.Drawing.Color.FromArgb(45, 45, 48) : System.Drawing.Color.White;
            var foreColor = darkMode ? System.Drawing.Color.White : System.Drawing.Color.Black;

            control.BackColor = backColor;
            control.ForeColor = foreColor;

            if (control is ComboBox || control is TextBox || control is ListBox)
            {
                control.BackColor = darkMode ? System.Drawing.Color.FromArgb(30, 30, 30) : System.Drawing.Color.White;
            }

            foreach (Control childControl in control.Controls)
            {
                ToggleControlDarkMode(childControl, darkMode);
            }
        }
        private void BtnLoadFiles_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "PDF (*.pdf)|*.pdf"; // Só mostra PDF no diálogo

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    lstFiles.Items.Clear();
                    var arquivosInvalidos = new List<string>();

                    foreach (var file in openFileDialog.FileNames)
                    {
                        if (Path.GetExtension(file).ToLower() == ".pdf")
                        {
                            lstFiles.Items.Add(file);
                        }
                        else
                        {
                            arquivosInvalidos.Add(Path.GetFileName(file));
                        }
                    }

                    if (arquivosInvalidos.Any())
                    {
                        MessageBox.Show(
                            $"Os seguintes arquivos não são PDF e foram ignorados:\n{string.Join("\n", arquivosInvalidos)}",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Só registra no log os PDFs válidos
                    if (lstFiles.Items.Count > 0)
                    {
                        ActivityLogger.Log(
                            $"Carregou arquivos: {string.Join(", ", lstFiles.Items.Cast<string>().Select(Path.GetFileName))}",
                            loggedInUser);
                    }
                }
            }
        }

        private void BtnSaveFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedItem == null || cmbSubCategory.SelectedItem == null)
                {
                    MessageBox.Show("Selecione uma categoria e subcategoria.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedCategory = cmbCategory.SelectedItem.ToString();
                var selectedSubCategory = cmbSubCategory.SelectedItem.ToString();
                var targetPath = Path.Combine(documentsPath, selectedCategory, selectedSubCategory);

                if (!Directory.Exists(targetPath))
                {
                    MessageBox.Show($"O caminho de destino não existe: {targetPath}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int total = lstFiles.Items.Count;
                using (var popup = new ProgressPopupForm())
                {
                    popup.Show();
                    popup.UpdateProgress(0, total, "Iniciando cópia...");

                    int count = 0;
                    foreach (var file in lstFiles.Items)
                    {
                        string filePath = file.ToString();
                        string fileName = Path.GetFileName(filePath);
                        string destPath = Path.Combine(targetPath, fileName);

                        if (File.Exists(destPath))
                        {
                            var result = MessageBox.Show($"'{fileName}' já existe. Deseja substituir?", "Confirmar", MessageBoxButtons.YesNo);
                            if (result == DialogResult.No)
                                continue;
                        }

                        var resposta = MessageBox.Show(
                            $"Deseja adicionar marca d'água ao arquivo '{fileName}'?",
                            "Adicionar Marca d'Água",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        string logMsg;

                        if (resposta == DialogResult.Yes)
                        {
                            // Salva com marca d'água
                            string tempPdf = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
                            File.Copy(filePath, tempPdf, true);
                            PdfHelper.AdicionarMarcaDagua(tempPdf, destPath);
                            File.Delete(tempPdf);

                            logMsg = $"Salvou o arquivo '{fileName}' COM marca d'água em '{selectedCategory}\\{selectedSubCategory}'";
                        }
                        else
                        {
                            // Salva SEM marca d'água
                            File.Copy(filePath, destPath, true);

                            logMsg = $"Salvou o arquivo '{fileName}' SEM marca d'água em '{selectedCategory}\\{selectedSubCategory}'";
                        }

                        count++;
                        popup.UpdateProgress(count, total, $"Copiado: {fileName}");
                        SalvarNovoDocumento(destPath, selectedCategory, selectedSubCategory, loggedInUser);

                        // Loga o resultado
                        ActivityLogger.Log(logMsg, loggedInUser);
                    }

                    popup.Close();
                }

                MessageBox.Show("Arquivos salvos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar arquivos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void BtnDeleteFiles_Click(object sender, EventArgs e)
        {

            if (listBoxFiles.SelectedItems.Count > 0)
            {
                var confirmResult = MessageBox.Show("Tem certeza de que deseja excluir os documentos selecionados?", "Confirmação", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    foreach (var selectedItem in listBoxFiles.SelectedItems)
                    {
                        var fileName = selectedItem.ToString();
                        var filePath = GetFilePath(fileName);

                        if (filePath != null)
                        {
                            File.Delete(filePath);
                        }
                        ExcluirDocumento(filePath);
                    }
                    ActivityLogger.Log($"Excluiu documentos: {string.Join(", ", listBoxFiles.SelectedItems.Cast<string>())}", loggedInUser);
                    MessageBox.Show("Documentos excluídos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listBoxFiles.Items.Clear();
                }
            }
            else
            {
                MessageBox.Show("Nenhum documento selecionado para exclusão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearFiles_Click(object sender, EventArgs e)
        {
            lstFiles.Items.Clear();
            ActivityLogger.Log("Limpou a lista de arquivos.", loggedInUser);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            {

                var searchPattern = textBoxSearch.Text;
                var selectedCategory = comboBoxCategorySearch.SelectedItem?.ToString() ?? "All Categories";
                var selectedSubCategory = comboBoxSubCategorySearch.SelectedItem?.ToString() ?? "All Subcategories";
                if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "Pesquisar Arquivos...")
                {
                    searchPattern = ""; // Define como vazio para buscar todos os PDFs
                }
                listBoxFiles.Items.Clear();


                if (selectedCategory == "All Categories")
                {
                    var categories = Directory.GetDirectories(documentsPath).Select(Path.GetFileName);
                    foreach (var category in categories)
                    {

                        {
                            SearchFiles(category, selectedSubCategory, searchPattern, listBoxFiles);
                        }
                    }
                }
                else
                {
                    SearchFiles(selectedCategory, selectedSubCategory, searchPattern, listBoxFiles);
                }
                ActivityLogger.Log($"Realizou pesquisa por '{searchPattern}' na categoria '{selectedCategory}' e subcategoria '{selectedSubCategory}'", loggedInUser);
            }
        }




        //var searchPattern = textBoxSearch.Text;
        //var selectedCategory = comboBoxCategorySearch.SelectedItem?.ToString() ?? "All Categories";
        //var selectedSubCategory = comboBoxSubCategorySearch.SelectedItem?.ToString() ?? "All Subcategories";
        //
        //listBoxFiles.Items.Clear();
        //
        //if (selectedCategory == "All Categories")
        //{
        //    foreach (var category in GetCategories())
        //    {
        //        SearchFiles(category, selectedSubCategory, searchPattern, listBoxFiles);
        //    }
        //}
        //else
        //{
        //    SearchFiles(selectedCategory, selectedSubCategory, searchPattern, listBoxFiles);
        //}
        //
        //ActivityLogger.Log($"Realizou pesquisa por '{searchPattern}' na categoria '{selectedCategory}' e subcategoria '{selectedSubCategory}'", loggedInUser);

                private void SearchFiles(string category, string subCategory, string searchPattern, ListBox listBox)
        {
            var categoryPath = Path.Combine(documentsPath, category);

            if (subCategory == "All Subcategories")
            {
                var subCategories = Directory.GetDirectories(categoryPath).Select(Path.GetFileName);
                foreach (var subCat in subCategories)
                {
                    var subCategoryPath = Path.Combine(categoryPath, subCat);
                    string[] files = Directory.GetFiles(subCategoryPath, "*" + searchPattern + "*.*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        listBox.Items.Add(Path.GetFileName(file));
                    }
                }
            }
            else
            {
                var subCategoryPath = Path.Combine(categoryPath, subCategory);
                string[] files = Directory.GetFiles(subCategoryPath, "*" + searchPattern + "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    listBox.Items.Add(Path.GetFileName(file));
                }
            }
        }

        private string GetFilePath(string fileName)
        {
            foreach (var category in GetCategories())
            {
                string categoryPath = Path.Combine(documentsPath, category);
                var files = Directory.GetFiles(categoryPath, fileName, SearchOption.AllDirectories);
                if (files.Any())
                {
                    return files.First();
                }
                foreach (var subCategory in GetSubcategories(category))
                {
                    var subCategoryPath = Path.Combine(categoryPath, subCategory);
                    files = Directory.GetFiles(subCategoryPath, fileName, SearchOption.AllDirectories);
                    if (files.Any())
                    {
                        return files.First();
                    }
                }
            }
            return null;
        }

        //private void BtnUpdateCategories_Click(object sender, EventArgs e)
        //{
        //    LoadCategoriesFromDirectory();
        //    LoadCategoriesIntoComboBoxes();
        //    MessageBox.Show("Categorias e subcategorias atualizadas com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    ActivityLogger.Log("Atualizou categorias e subcategorias", loggedInUser);
        //}

        private List<string> GetCategories()
        {
            if (Directory.Exists(documentsPath))
            {
                return Directory.GetDirectories(documentsPath).Select(Path.GetFileName).ToList();
            }
            return new List<string>();
        }

        private List<string> GetSubcategories(string category)
        {
            var subCategoryPath = Path.Combine(documentsPath, category);
            if (Directory.Exists(subCategoryPath))
            {
                return Directory.GetDirectories(subCategoryPath).Select(Path.GetFileName).ToList();
            }
            return new List<string>();
        }

        private void UpdateComboBoxes(params ComboBox[] comboBoxes)
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.Items.Clear();
                var categories = GetCategories();
                comboBox.Items.AddRange(categories.ToArray());
                if (categories.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
        }

        private void UpdateSubCategoryComboBox(ComboBox comboBox, string selectedCategory)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("All Subcategories");
            var subcategories = GetSubcategories(selectedCategory);
            if (subcategories != null)
            {
                comboBox.Items.AddRange(subcategories.ToArray());
            }
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void btnRenameFile1_Click(object sender, EventArgs e)
        {

            if (listBoxFiles.SelectedItem == null)
            {
                MessageBox.Show("Selecione um arquivo para renomear.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldFileName = listBoxFiles.SelectedItem.ToString();
            string oldPath = GetFilePath(oldFileName);

            if (string.IsNullOrEmpty(oldPath))
            {
                MessageBox.Show("Arquivo não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (pdfViewer2.Document != null)
            {
                pdfViewer2.Document.Dispose();
                pdfViewer2.Document = null;
            }


            string newFileName = Prompt.ShowDialog("Digite o novo nome do arquivo:", "Renomear Arquivo", oldFileName);
            if (string.IsNullOrWhiteSpace(newFileName)) return;

            string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newFileName);
            // Libera o documento atual do viewer

            RenomearDocumento(oldPath, newPath);

            try
            {
                File.Move(oldPath, newPath);
                MessageBox.Show("Arquivo renomeado com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActivityLogger.Log($"Renomeou arquivo '{oldFileName}' para '{newFileName}'", loggedInUser);
                BtnSearch_Click(null, null); // Atualiza a lista
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao renomear: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnMoveFile_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione um ou mais arquivos para mover.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Converte os arquivos selecionados em lista
            var selectedFiles = listBoxFiles.SelectedItems.Cast<string>().ToList();

            // Mapeia as categorias e subcategorias
            var categoryMap = new Dictionary<string, List<string>>();
            var categories = Directory.GetDirectories(documentsPath);


            foreach (var categoryPath in categories)
            {
                string category = Path.GetFileName(categoryPath);
                var subcategories = Directory.GetDirectories(categoryPath)
                                             .Select(Path.GetFileName)
                                             .ToList();

                categoryMap[category] = subcategories;
            }

            // Abre o formulário com os arquivos selecionados visíveis
            using (var form = new MoveFileForm(categoryMap, selectedFiles))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string targetCategory = form.SelectedCategory;
                    string targetSubCategory = form.SelectedSubCategory;
                    string destinationDir = Path.Combine(documentsPath, targetCategory, targetSubCategory);
                    Directory.CreateDirectory(destinationDir);

                    using (var popup = new ProgressPopupForm())
                    {
                        popup.Show();
                        popup.UpdateProgress(0, selectedFiles.Count, "Iniciando movimentação...");

                        int count = 0;
                        foreach (var fileName in selectedFiles)
                        {
                            string sourcePath = GetFilePath(fileName);
                            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                                continue;

                            string destinationPath = Path.Combine(destinationDir, fileName);
                            popup.UpdateProgress(count, selectedFiles.Count, $"Movendo: {fileName}");

                            try
                            {
                                File.Move(sourcePath, destinationPath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Erro ao mover {fileName}: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            MoverDocumento(destinationPath, targetCategory, targetSubCategory);

                            count++;
                            popup.UpdateProgress(count, selectedFiles.Count, $"Movido: {fileName}");
                        }



                        popup.Close();
                    }

                    ActivityLogger.Log($"Moveu {selectedFiles.Count} arquivo(s) para {targetCategory}\\{targetSubCategory}", loggedInUser);
                    MessageBox.Show("Arquivos movidos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        private void listBoxFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxFiles.SelectedItem != null)
            {
                string selectedFileName = listBoxFiles.SelectedItem.ToString();
                string filePath = GetFilePath(selectedFileName);

                if (filePath != null && File.Exists(filePath))
                {
                    try
                    {
                        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        var pdfDoc = PdfiumViewer.PdfDocument.Load(stream);
                        pdfViewer2.Document?.Dispose(); // Descarta o anterior
                        pdfViewer2.Document = pdfDoc;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Arquivo não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyDarkMenuTheme()
        {
            menuStrip.BackColor = Color.FromArgb(45, 45, 48);
            menuStrip.ForeColor = Color.White;
            menuStrip.Renderer = new ThemeManager.DarkMenuStripRenderer();

            // Aplica a cor branca em todos os itens
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                ApplyWhiteTextToMenu(item);
            }
        }

        private void ApplyWhiteTextToMenu(ToolStripMenuItem menuItem)
        {
            menuItem.ForeColor = Color.White;

            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenu)
                {
                    ApplyWhiteTextToMenu(subMenu);
                }
                else
                {
                    subItem.ForeColor = Color.White;
                }
            }
        }




        private void ApplyMenuTheme()
        {
            if (ThemeManager.IsDarkMode)
            {
                menuStrip.Renderer = new ThemeManager.DarkMenuStripRenderer();
                foreach (ToolStripMenuItem item in menuStrip.Items)
                {
                    SetMenuTextColor(item, Color.White);
                }
            }
        }

        private void SetMenuTextColor(ToolStripMenuItem item, Color color)
        {
            item.ForeColor = color;
            foreach (ToolStripMenuItem subItem in item.DropDownItems.OfType<ToolStripMenuItem>())
            {
                SetMenuTextColor(subItem, color);
            }
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption, string defaultValue = "")
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, Text = defaultValue };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;
                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        private void LstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem != null)
            {
                string filePath = lstFiles.SelectedItem.ToString();

                if (File.Exists(filePath))
                {
                    try
                    {
                        pdfViewer1.Document?.Dispose(); // Libera PDF anterior
                        pdfViewer1.Document = null;

                        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        var pdfDoc = PdfiumViewer.PdfDocument.Load(stream);
                        pdfViewer1.Document = pdfDoc;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RealizarLogout()
        {
            SaveUserPreferences(false); // ou o que você quiser            
            UpdateUserLoginStatus(loggedInUser, false);
            UpdateUserOnlineTime(loggedInUser);
            CheckAndRemoveLoggedOutUsers();
            // Se quiser, fecha a aplicação
            Application.Exit();
        }
        private void UpdateUserLoginStatus(string username, bool status)
        {
            string userLoginStatusFilePath = Path.Combine(basePath, "userLoginStatus.json");
            var userLoginStatus = File.Exists(userLoginStatusFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, bool>>(File.ReadAllText(userLoginStatusFilePath))
                : new Dictionary<string, bool>();

            userLoginStatus[username] = status;
            File.WriteAllText(userLoginStatusFilePath, JsonConvert.SerializeObject(userLoginStatus, Formatting.Indented));
        }

        private void UpdateUserOnlineTime(string username)
        {
            string userLoginDetailsFilePath = Path.Combine(basePath, "userLoginDetails.json");
            if (File.Exists(userLoginDetailsFilePath))
            {
                var userLoginDetails = JsonConvert.DeserializeObject<List<UserLoginDetail>>(File.ReadAllText(userLoginDetailsFilePath));
                var userDetail = userLoginDetails.FirstOrDefault(u => u.Username == username && u.OnlineTime == "00:00:00");

                if (userDetail != null)
                {
                    userDetail.OnlineTime = (DateTime.Now - DateTime.ParseExact(userDetail.LoginTime, "dd-MM-yyyy HH:mm:ss", null)).ToString(@"hh\:mm\:ss");
                    File.WriteAllText(userLoginDetailsFilePath, JsonConvert.SerializeObject(userLoginDetails, Formatting.Indented));
                }
            }
        }
        private void CheckAndRemoveLoggedOutUsers()
        {
            string userLoginStatusFilePath = Path.Combine(basePath, "userLoginStatus.json");
            string currentUserDetailsFilePath = Path.Combine(basePath, "currentUsers.json");

            if (File.Exists(userLoginStatusFilePath) && File.Exists(currentUserDetailsFilePath))
            {
                var userLoginStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(File.ReadAllText(userLoginStatusFilePath));
                var currentUserDetails = JsonConvert.DeserializeObject<List<UserLoginDetail>>(File.ReadAllText(currentUserDetailsFilePath));
                var usersToRemove = currentUserDetails.Where(u => userLoginStatus.ContainsKey(u.Username) && !userLoginStatus[u.Username]).ToList();

                if (usersToRemove.Any())
                {
                    foreach (var userDetail in usersToRemove)
                    {
                        userDetail.OnlineTime = (DateTime.Now - DateTime.ParseExact(userDetail.LoginTime, "dd-MM-yyyy HH:mm:ss", null)).ToString(@"hh\:mm\:ss");
                        SaveUserLoginDetails(userDetail);
                        currentUserDetails.Remove(userDetail);
                    }
                    File.WriteAllText(currentUserDetailsFilePath, JsonConvert.SerializeObject(currentUserDetails, Formatting.Indented));
                }
            }
        }
        private void SaveUserLoginDetails(UserLoginDetail userDetail)
        {
            string userLoginDetailsFilePath = Path.Combine(basePath, "userLoginDetails.json");
            var userLoginDetails = File.Exists(userLoginDetailsFilePath)
                ? JsonConvert.DeserializeObject<List<UserLoginDetail>>(File.ReadAllText(userLoginDetailsFilePath))
                : new List<UserLoginDetail>();

            userLoginDetails.Add(userDetail);

            File.WriteAllText(userLoginDetailsFilePath, JsonConvert.SerializeObject(userLoginDetails, Formatting.Indented));
        }
        private void SaveUserPreferences(bool isSimpleView)
        {
            string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
            var userPreferences = File.Exists(userPreferencesFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath))
                : new Dictionary<string, UserPreferences>();

            bool isMaximized = this.WindowState == FormWindowState.Maximized;

            userPreferences[loggedInUser] = new UserPreferences
            {
                IsDarkMode = ThemeManager.IsDarkMode,
                IsSimpleView = isSimpleView,
                //WindowX = isMaximized ? this.RestoreBounds.X : this.Location.X,
                //WindowY = isMaximized ? this.RestoreBounds.Y : this.Location.Y,
                //WindowWidth = isMaximized ? this.RestoreBounds.Width : this.Size.Width,
                //WindowHeight = isMaximized ? this.RestoreBounds.Height : this.Size.Height
            };

            File.WriteAllText(userPreferencesFilePath, JsonConvert.SerializeObject(userPreferences, Formatting.Indented));
        }

        private void moverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (listBoxFiles.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Selecione um ou mais arquivos para mover.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // Converte os arquivos selecionados em lista
            var selectedFiles = listBoxFiles.SelectedItems.Cast<string>().ToList();

            // Mapeia as categorias e subcategorias
            var categoryMap = new Dictionary<string, List<string>>();
            var categories = Directory.GetDirectories(documentsPath);

            foreach (var categoryPath in categories)
            {
                string category = Path.GetFileName(categoryPath);
                var subcategories = Directory.GetDirectories(categoryPath)
                                             .Select(Path.GetFileName)
                                             .ToList();

                categoryMap[category] = subcategories;
            }

            // Abre o formulário com os arquivos selecionados visíveis
            using (var form = new MoveFileForm(categoryMap, selectedFiles))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string targetCategory = form.SelectedCategory;
                    string targetSubCategory = form.SelectedSubCategory;
                    string destinationDir = Path.Combine(documentsPath, targetCategory, targetSubCategory);
                    Directory.CreateDirectory(destinationDir);

                    using (var popup = new ProgressPopupForm())
                    {
                        popup.Show();
                        popup.UpdateProgress(0, selectedFiles.Count, "Iniciando movimentação...");

                        int count = 0;
                        foreach (var fileName in selectedFiles)
                        {
                            string sourcePath = GetFilePath(fileName);
                            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                                continue;

                            string destinationPath = Path.Combine(destinationDir, fileName);
                            popup.UpdateProgress(count, selectedFiles.Count, $"Movendo: {fileName}");

                            try
                            {
                                File.Move(sourcePath, destinationPath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Erro ao mover {fileName}: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            count++;
                            popup.UpdateProgress(count, selectedFiles.Count, $"Movido: {fileName}");
                        }

                        popup.Close();
                    }

                    ActivityLogger.Log($"Moveu {selectedFiles.Count} arquivo(s) para {targetCategory}\\{targetSubCategory}", loggedInUser);
                    MessageBox.Show("Arquivos movidos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static class PdfHelper
        {
            public static void AdicionarMarcaDagua(string pdfInput, string pdfOutput)
            {
                iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(pdfInput);
                using (FileStream fs = new FileStream(pdfOutput, FileMode.Create, FileAccess.Write, FileShare.None))
                using (PdfStamper stamper = new PdfStamper(pdfReader, fs))
                {
                    int totalPaginas = pdfReader.NumberOfPages;
                    BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.EMBEDDED);

                    for (int i = 1; i <= totalPaginas; i++)
                    {
                        var pageSize = pdfReader.GetPageSize(i);
                        int rotation = pdfReader.GetPageRotation(i);
                        float width = pageSize.Width;
                        float height = pageSize.Height;

                        // Se a página está rotacionada (90 ou 270), inverta width e height
                        bool paisagem = (width > height) ^ (rotation == 90 || rotation == 270);

                        PdfContentByte canvas = stamper.GetOverContent(i);

                        // Transparência
                        PdfGState gState = new PdfGState();
                        gState.FillOpacity = 0.3f; // Opacidade ajustável
                        canvas.SaveState();
                        canvas.SetGState(gState);

                        // Cor e fonte
                        canvas.SetColorFill(BaseColor.RED);
                        canvas.BeginText();
                        canvas.SetFontAndSize(font, 40);

                        // Lógica de deslocamento e rotação conforme orientação:
                        float deslocaX, deslocaY, angulo;

                        if (paisagem) // Página paisagem (real)
                        {
                            deslocaX = 130;
                            deslocaY = 120;
                            angulo = 0;
                        }
                        else // Retrato
                        {
                            deslocaX = -240;
                            deslocaY = +10;
                            angulo = 90;
                        }

                        float x = (width / 2) + deslocaX;
                        float y = (height / 2) + deslocaY;

                        canvas.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "SISTEMA DA QUALIDADE", x, y, angulo);

                        canvas.EndText();
                        canvas.RestoreState();
                    }
                    stamper.Close();
                }
                pdfReader.Close();
            }


            public class SimpleFontResolver : IFontResolver
            {
                private static readonly byte[] FontData = LoadFont();

                private static byte[] LoadFont()
                {
                    string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fontes", "Arial.ttf");
                    if (!File.Exists(fontPath))
                    {
                        MessageBox.Show(
                            "Arquivo de fonte não encontrado!\n\nCaminho esperado:\n" + fontPath,
                            "Erro ao carregar fonte", MessageBoxButtons.OK, MessageBoxIcon.Error
                        );
                        // Retorna null para evitar crash, mas GetFont abaixo vai dar erro controlado
                        return null;
                    }
                    return File.ReadAllBytes(fontPath);
                }

                public byte[] GetFont(string faceName)
                {
                    if (FontData == null)
                        throw new Exception("Fonte Arial não foi carregada corretamente.");
                    return FontData;
                }

                public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
                {
                    // Não importa bold/italic, sempre retorna Arial normal
                    return new FontResolverInfo("Arial#");
                }
            }

        }
    }
}


