using iTextSharp.text;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Newtonsoft.Json;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json.Linq; // Adiciona no topo
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharpPdfDocument = iTextSharp.text.pdf.PdfDocument;
using PdfiumViewerPdfDocument = PdfiumViewer.PdfDocument;
//using WinFormsButton = System.Windows.Forms.Button;
using WinFormsComboBox = System.Windows.Forms.ComboBox;



namespace DocsViewer 
{
    public partial class DocumentViewerForm : Form
    {
        private readonly User loggedUser; // Mantém como readonly
        private string currentFilePath; // Remove readonly se precisar modificar depois
        private System.Timers.Timer inactivityTimer; // Remove readonly se precisar modificar depois
        private System.Windows.Forms.ToolTip toolTip4;

        private  Dictionary<string, List<string>> userPermissions;
        private string basePath => AppConfig.GetDatabasePath();
        private string documentsPath => AppConfig.GetDocumentsPath();
        private void btnPrev_Click(object sender, EventArgs e) => PreviousPage();
        private void btnNext_Click(object sender, EventArgs e) => NextPage();

        public class BorderlessToolStripRenderer : System.Windows.Forms.ToolStripSystemRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // Não renderiza borda
            }
        }



        private bool _highlightMode = false;
        private Point? _highlightStart = null;
        private System.Drawing.Rectangle? _highlightRect = null;
        public DocumentViewerForm(User user)
        {
            InitializeComponent();
            loggedUser = user;
            LoadUserPreferences();
            toolStrip.Renderer = new BorderlessToolStripRenderer();
            LogoHelper.AplicarLogoComoIcon(this);
            this.Shown += DocumentViewerForm_Shown; // Adiar inicialização pesada
            this.ActiveControl = buttonSearch3;  // Define o foco em outro controle ao iniciar
            ConfigurePdfViewerPermissions();
            AddInactivityHandlers(this);
            DesabilitarPrintsPadraoPdfium();

            SetupInactivityTimer();
        }
            private void DocumentViewerForm_Shown(object sender, EventArgs e)
        {
            // Agora o controle tem seu handle criado
            LoadFormDataSafe();

        }


        private void LoadFormDataSafe()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 1. Carregue dados básicos
                userPermissions = LoadUserPermissions();
                //LoadUserPreferences();

                // 2. Configure controles visíveis               
                btnManagement.Enabled = loggedUser.Role == "admin";
                btnPrintComCarimbo.Visible = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                //btnChangePassword.Visible = loggedUser.Role == "viewer" || loggedUser.Role == "editor";
                //btnVerifySignatures.Enabled = !(loggedUser.Role == "viewer" || loggedUser.Role == "editor");
                //btnVerOriginal.Enabled = !(loggedUser.Role == "viewer" || loggedUser.Role == "editor");
                //btnVerOriginal.Enabled = false; // Desabilita o botão de validação original para visualizadores e editores
                //btnVerifySignatures.Enabled = false;
                btnPrintComCarimbo.Enabled = false;
                SetupMaintenanceTimer();

                //btnVerifySignatures.Enabled = loggedUser.Role != "viewer" && loggedUser.Role != "editor";



                // 3. Configure placeholders
                ConfigureSearchPlaceholder();

                // 4. Carregue combobox
                PopulateCategoryComboBox(comboBoxCategory3);

                // 5. Atualiza os Icones dos botões
                UpdateAllButtonIcons();

                // 6. Aplique o tema
                ThemeManager.ApplyTheme(this);

                // 7. Configure PDF Viewer (agora seguro)
                ConfigurePdfViewerPermissions();
                SetupInactivityTimer();
                SetPdfViewer1ControlsVisible(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário: {ex.Message}");
                Logger.Log($"LoadFormDataSafe error: {ex.ToString()}");
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            toolTip4 = new System.Windows.Forms.ToolTip();
            // Definindo aparência personalizada
            toolTip4.BackColor = Color.AliceBlue;
            toolTip4.ForeColor = Color.Black;
            toolTip4.OwnerDraw = true;
            toolTip4.Draw += toolTip4_Draw;
            toolTip4.Popup += toolTip4_Popup;

            // Tooltips para botões comuns
            toolTip4.SetToolTip(btnManagement, "Menu de Gerenciamento (F2)");
            toolTip4.SetToolTip(btnChangePassword, "Trocar Senha(F3)");
            toolTip4.SetToolTip(btnVisualizacaoDupla, "Mudar para Visualização Dupla(F4)");
            toolTip4.SetToolTip(btnToggleDarkMode, "Mudar mode Escuro/Claro(F5)");
            toolTip4.SetToolTip(btnToggleAjust1, "Ajustar Página(F6)");
            toolTip4.SetToolTip(btnLogout, "Sair do Sistema(F7)");
            toolTip4.SetToolTip(btnToggleListBoxes1, "Fechar a Lista de Arquivos(F8)");
            toolTip4.SetToolTip(buttonSearch3, "Pesquisar Arquivos");            
            toolTip4.SetToolTip(comboBoxCategory3, "Filtrar Categoria de Arquivos");            
            toolTip4.SetToolTip(comboBoxSubCategory3, "Filtrar Sub Categoria de Arquivos");           
            toolTip4.SetToolTip(textBoxSearch3, "Digitar Texto para Buscar Arquivos");

            toolStripOffsetX = toolStrip.Location.X - pdfViewer3.Location.X;
            toolStripOffsetY = toolStrip.Location.Y - pdfViewer3.Location.Y;
        }
        private System.Timers.Timer maintenanceTimer;

        private void SetupMaintenanceTimer()
        {
            maintenanceTimer = new System.Timers.Timer(60 * 1000); // 1 minuto
            maintenanceTimer.Elapsed += MaintenanceTimer_Elapsed;
            maintenanceTimer.AutoReset = true;
            maintenanceTimer.Start();

            // Já verifica logo na abertura do form!
            MaintenanceTimer_Elapsed(null, null);
        }

        private void MaintenanceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string mensagem;
                // AQUI, use a role do usuário logado:
                if (IsInMaintenance(out mensagem, loggedUser.Role))
                {
                    ActivityLogger.Log("Deslogado automaticamente por manutenção", loggedUser?.Username ?? "?");
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(mensagem, "Manutenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }));
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Manutenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
            }
            catch { }
        }
        private void DocumentViewerForm_KeyDown(object sender, KeyEventArgs e)
        {
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnManagement.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F3)
                {
                    btnChangePassword.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F4)
                {
                    btnVisualizacaoDupla.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F5)
                {
                    btnToggleDarkMode.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F6)
                {
                    btnToggleAjust1.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F7)
                {
                    btnLogout.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F8)
                {
                    btnToggleListBoxes1.PerformClick(); // Simula um clique no botão if (e.Shift && e.KeyCode == Keys.Enter)
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.Alt && e.KeyCode == Keys.H)
                {
                    btnVerifySignatures.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.Alt && e.KeyCode == Keys.J)
                {
                    btnVerOriginal.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
            }
        }
        private void DesabilitarBotaoPrintPadrao(ToolStrip toolStrip)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item.Text == "Print")
                {
                    item.Enabled = false;
                    item.Visible = false;
                }
            }
        }
        private void DesabilitarPrintsPadraoPdfium()
        {
            var ts1 = pdfViewer3.Controls.OfType<ToolStrip>().FirstOrDefault();            
            if (ts1 != null) DesabilitarBotaoPrintPadrao(ts1);
           
        }

        public void AdicionarCarimboPrimeiraPagina(string arquivoPdfOriginal, string arquivoPdfDestino, string caminhoCarimbo)
        {
            using (var reader = new PdfReader(arquivoPdfOriginal))
            using (var fs = new FileStream(arquivoPdfDestino, FileMode.Create, FileAccess.Write))
            using (var stamper = new PdfStamper(reader, fs))
            {
                int page = 1;
                var tamanhoPagina = reader.GetPageSize(page);

                // Descobre a rotação da página: 0, 90, 180, 270
                int rotation = reader.GetPageRotation(page);

                // Considera paisagem se rotação 90 ou 270, ou se largura > altura
                bool isLandscape = rotation == 90 || rotation == 270 || tamanhoPagina.Width > tamanhoPagina.Height;

                using (var img = System.Drawing.Image.FromFile(caminhoCarimbo))
                {
                    // Gira o carimbo se quiser (opcional, já está no seu código)
                    if (!isLandscape)
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);

                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var imgBytes = ms.ToArray();
                        iTextSharp.text.Image carimbo = iTextSharp.text.Image.GetInstance(imgBytes);

                        float larguraCarimbo, alturaCarimbo;

                        if (isLandscape)
                        {
                            // Exemplo: carimbo mais largo, menos alto (ajuste a gosto)
                            larguraCarimbo = Utilities.MillimetersToPoints(40); // 40mm de largura
                            alturaCarimbo = Utilities.MillimetersToPoints(20); // 20mm de altura
                        }
                        else
                        {
                            // Exemplo: carimbo mais alto, menos largo (ajuste a gosto)
                            larguraCarimbo = Utilities.MillimetersToPoints(20); // 20mm de largura
                            alturaCarimbo = Utilities.MillimetersToPoints(40); // 40mm de altura
                        }
                        carimbo.ScaleAbsolute(larguraCarimbo, alturaCarimbo);
                        float x, y;
                        if (isLandscape)
                        {
                            // Canto superior esquerdo (paisagem, mesmo se rotação 90 ou 270)
                            x = tamanhoPagina.Left + 40;
                            y = tamanhoPagina.Top - alturaCarimbo - 260;
                        }
                        else
                        {
                            // Canto superior esquerdo (retrato)
                            x = tamanhoPagina.Left - larguraCarimbo + 70;
                            y = tamanhoPagina.Top - alturaCarimbo - 20;
                        }
                        carimbo.SetAbsolutePosition(x, y);

                        var content = stamper.GetOverContent(page);
                        content.AddImage(carimbo);
                    }
                }
            }
        }

        private void btnPrintComCarimbo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath) || !File.Exists(currentFilePath))
            {
                MessageBox.Show("Nenhum documento selecionado para imprimir.");
                return;
            }

            // VIEWER: botão já desabilitado no Load

            if (loggedUser.Role == "editor")
            {
                // Imprime COM carimbo
                string caminhoCarimbo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "carimbo.png"); 
                string tempPdf = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
                AdicionarCarimboPrimeiraPagina(currentFilePath, tempPdf, caminhoCarimbo);
                ImprimirPDFComPdfium(tempPdf);
                LogActivity($"Imprimiu (com carimbo) o documento: {Path.GetFileName(currentFilePath)}");
            }
            else if (loggedUser.Role == "admin")
            {
                // Imprime NORMAL (sem carimbo)
                ImprimirPDFComPdfium(currentFilePath);
                LogActivity($"Imprimiu (sem carimbo) o documento: {Path.GetFileName(currentFilePath)}");
            }
        }

        private void ImprimirPDFComPdfium(string pdfPath)
        {
            try
            {
                using (var pdfDoc = PdfiumViewer.PdfDocument.Load(pdfPath))
                {
                    // Cria o PrintDocument já no modo Fit to Page (ShrinkToMargin)
                    using (var printDoc = pdfDoc.CreatePrintDocument(PdfiumViewer.PdfPrintMode.ShrinkToMargin))
                    {
                        // Ajuste de margens (aumente aqui para mais folga)
                        printDoc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

                        // Permite escolher intervalo de páginas
                        PrintDialog dlg = new PrintDialog();
                        dlg.Document = printDoc;
                        dlg.AllowSomePages = true;
                        dlg.AllowSelection = true;

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            printDoc.PrinterSettings = dlg.PrinterSettings;
                            // Não precisa mexer em DefaultPageSettings aqui (ele já pega do dialog se necessário)
                            printDoc.Print();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao imprimir o PDF: " + ex.Message);
            }
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                buttonSearch3.Image = GetButtonImage("search");
                btnToggleListBoxes1.Image = GetButtonImage("toggle_list_close");
                btnVisualizacaoDupla.Image = GetButtonImage("double_view");
                btnToggleDarkMode.Image = GetButtonImage("dark_mode");
                btnChangePassword.Image = GetButtonImage("password");
                btnManagement.Image = GetButtonImage("management");
                btnLogout.Image = GetButtonImage("logout1");
                btnToggleAjust1.Image = GetButtonImage("adjust");
                
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
                return image ?? SystemIcons.Information.ToBitmap();
            }
            catch
            {
                return SystemIcons.Information.ToBitmap();
            }
        }

        private void ConfigureSearchPlaceholder()
        {
            // Configurar placeholder da busca 1
            textBoxSearch3.Text = "Pesquisar Arquivos...";
            textBoxSearch3.ForeColor = ThemeManager.TextBoxPlaceholderColor;
            textBoxSearch3.Tag = "HasPlaceholder_Pesquisar Arquivos..."; // Marca como TextBox com placeholder

            textBoxSearch3.GotFocus += (s, e) => {
                if (textBoxSearch3.Text == "Pesquisar Arquivos...")
                {
                    textBoxSearch3.Text = "";
                    textBoxSearch3.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            textBoxSearch3.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(textBoxSearch3.Text))
                {
                    textBoxSearch3.Text = "Pesquisar Arquivos...";
                    textBoxSearch3.ForeColor = ThemeManager.TextBoxPlaceholderColor;
                }
            };

            // **Remove o foco do TextBox e define para outro controle**
            this.ActiveControl = null; // Remove o foco de qualquer controle
                                       // OU
            this.Focus(); // Foca em um Label (se tiver um)

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThemeManager.ApplyTheme(this);
        }

   
        private Dictionary<string, List<string>> LoadUserPermissions()
        {
            string userPermissionsFilePath = Path.Combine(basePath, "userPermissions.json");
            if (!File.Exists(userPermissionsFilePath))
            {
                string errorMessage = "Arquivo de permissões não encontrado: " + userPermissionsFilePath;
                Logger.Log(errorMessage);
                throw new FileNotFoundException(errorMessage);
            }

            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(userPermissionsFilePath));
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

        private void OnInactivity(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Close()));
            }
            else
            {
                ActivityLogger.Log("deslogou do sistem por Inatividade", loggedUser?.Username ?? "?");                               
                Close();
            }
        }

        private void ResetInactivityTimer(object sender, EventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
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

        private void ConfigurePdfViewerPermissions()
        {
            var toolStrip = pdfViewer3.Controls.OfType<ToolStrip>().FirstOrDefault();


            // Ajusta a altura e os ícones da ToolStrip1 (se existir)
            if (toolStrip != null)
            {
                toolStrip.SuspendLayout();
                //toolStrip1.Height = 50;
                //toolStrip1.ImageScalingSize = new Size(32, 32);
                //toolStrip1.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                toolStrip.ResumeLayout(true);
                toolStrip.PerformLayout(); // Força o redesenho

                foreach (ToolStripItem item in toolStrip.Items)
                {
                    item.AutoSize = false; // Desativa auto-size
                    item.Size = new Size(25, 25); // Tamanho customizado
                    item.Padding = new Padding(2); // Espaçamento interno
                }

                foreach (ToolStripItem item in toolStrip.Items)
                {
                    // Seus eventos e lógica de permissão...
                    if (item.Text == "Save")
                    {
                        item.Click += (s, e) =>
                        {
                            if (listBoxFiles3.SelectedItem != null)
                            {
                                string selectedFileName = listBoxFiles3.SelectedItem.ToString();
                                LogActivity($"Salvou o documento: {selectedFileName}");
                            }
                        };
                        item.Enabled = loggedUser.Role == "admin"; //|| loggedUser.Role == "editor";
                    }
                    else if (item.Text == "Print")
                    {
                        item.Click += (s, e) =>
                        {
                            if (listBoxFiles3.SelectedItem != null)
                            {
                                string selectedFileName = listBoxFiles3.SelectedItem.ToString();
                                LogActivity($"Imprimiu o documento: {selectedFileName}");
                            }
                        };
                        item.Enabled = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                    }
                }
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Carrega o restante após o formulário estar visível
            this.BeginInvoke(new Action(() => {
                try
                {
                    // Forçar uma atualização do tema
                    ThemeManager.ApplyTheme(this);

                    // Atualizar o PDF Viewer se já houver arquivo carregado
                    if (!string.IsNullOrEmpty(currentFilePath))
                    {
                        DisplayPdf(currentFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Erro pós-carregamento: {ex.ToString()}");
                }
            }));
        }

        private void PopulateCategoryComboBox(WinFormsComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("All Categories");

            if (Directory.Exists(documentsPath))
            {
                var categories = Directory.GetDirectories(documentsPath).Select(Path.GetFileName);
                foreach (var category in categories)
                {
                    if (UserCanAccessCategory(category))
                    {
                        comboBox.Items.Add(category);
                    }
                }
            }
            else
            {
                string errorMessage = "Diretório de documentos não encontrado: " + documentsPath;
                Logger.Log(errorMessage);
                throw new DirectoryNotFoundException(errorMessage);
            }

            comboBox.SelectedIndex = 0;
        }

        private void ComboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSubCategoryComboBox(comboBoxSubCategory3, comboBoxCategory3.SelectedItem.ToString());
        }

        private void PopulateSubCategoryComboBox(WinFormsComboBox comboBox, string selectedCategory)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("All Subcategories");

            var subCategoryPath = Path.Combine(documentsPath, selectedCategory);
            if (Directory.Exists(subCategoryPath))
            {
                var subCategories = Directory.GetDirectories(subCategoryPath).Select(Path.GetFileName);
                foreach (var subCategory in subCategories)
                {
                    if (UserCanAccessSubCategory(selectedCategory, subCategory))
                    {
                        comboBox.Items.Add(subCategory);
                    }
                }
            }

            comboBox.SelectedIndex = 0;
        }

        

        private void ButtonSearch3_Click(object sender, EventArgs e)
        {
            LogActivity("Buscou na Categoria 3");           
            string selectedCategory = comboBoxCategory3.SelectedItem.ToString();
            string selectedSubCategory = comboBoxSubCategory3.SelectedItem.ToString();
            string searchPattern = textBoxSearch3.Text;
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "Pesquisar Arquivos...")
            {
                searchPattern = ""; // Define como vazio para buscar todos os PDFs
            }

            listBoxFiles3.Items.Clear();

            if (selectedCategory == "All Categories")
            {
                var categories = Directory.GetDirectories(documentsPath).Select(Path.GetFileName);
                foreach (var category in categories)
                {
                    if (UserCanAccessCategory(category))
                    {
                        SearchFiles(category, selectedSubCategory, searchPattern, listBoxFiles3);
                    }
                }
            }
            else
            {
                SearchFiles(selectedCategory, selectedSubCategory, searchPattern, listBoxFiles3);
            }
        }

        private void SearchFiles(string category, string subCategory, string searchPattern, ListBox listBox)
        {
            var categoryPath = Path.Combine(documentsPath, category);

            if (subCategory == "All Subcategories")
            {
                var subCategories = Directory.GetDirectories(categoryPath).Select(Path.GetFileName);
                foreach (var subCat in subCategories)
                {
                    if (UserCanAccessSubCategory(category, subCat))
                    {
                        string subCategoryPath = Path.Combine(categoryPath, subCat);
                        string[] files = Directory.GetFiles(subCategoryPath, "*" + searchPattern + "*.pdf", SearchOption.AllDirectories);

                        foreach (string file in files)
                        {
                            listBox.Items.Add(Path.GetFileName(file));
                        }
                    }
                }
            }
            else
            {
                if (UserCanAccessSubCategory(category, subCategory))
                {
                    string subCategoryPath = Path.Combine(categoryPath, subCategory);
                    string[] files = Directory.GetFiles(subCategoryPath, "*" + searchPattern + "*.pdf", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        listBox.Items.Add(Path.GetFileName(file));
                    }
                }
            }
        }

        private void ListBoxFiles3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFiles3.SelectedItem != null)
            {
                string selectedFileName = listBoxFiles3.SelectedItem.ToString();
                currentFilePath = GetFilePath(selectedFileName);
                DisplayPdf(currentFilePath);
                LogActivity($"Visualizou o arquivo {selectedFileName} na Categoria 3");

                // Habilita o botão de imprimir COM carimbo se o arquivo existir
                btnPrintComCarimbo.Enabled = !string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath);
            }
            else
            {
                // Desabilita o botão se não tem arquivo selecionado
                btnPrintComCarimbo.Enabled = false;
            }
        }

        private string GetFilePath(string fileName)
        {
            var categories = Directory.GetDirectories(documentsPath);
            foreach (var categoryPath in categories)
            {
                var files = Directory.GetFiles(categoryPath, fileName, SearchOption.AllDirectories);
                if (files.Any())
                {
                    return files.First();
                }
                var subCategories = Directory.GetDirectories(categoryPath);
                foreach (var subCategoryPath in subCategories)
                {
                    files = Directory.GetFiles(subCategoryPath, fileName, SearchOption.AllDirectories);
                    if (files.Any())
                    {
                        return files.First();
                    }
                }
            }
            return null;
        }
        private void DisplayPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            // Carrega o documento no visualizador
            var document = PdfiumViewer.PdfDocument.Load(filePath);
            pdfViewer3.Document = document;

            // Atualiza o caminho atual
            currentFilePath = filePath;

            // Verifica se há assinaturas digitais
            bool hasSignatures = HasSignatures(filePath);

            // Habilita ou desabilita o botão e o label de acordo com a presença de assinatura
            btnVerifySignatures.Enabled = hasSignatures;
            lblSignatureStatus1.Enabled = hasSignatures;
            btnVerOriginal.Enabled = hasSignatures;


            if (loggedUser.Role == "viewer" || loggedUser.Role == "editor")
            {
                // Desabilita os botão de acordo com a regra de usário
                btnVerifySignatures.Enabled = false;
                lblSignatureStatus1.Enabled = true;
                btnVerOriginal.Enabled = false;
                lblPage.Enabled = true;
                toolBtnFirstPage1.Enabled = true;
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
                btnRotateLeft.Enabled = true;
                toolBtnResetRotation1.Enabled = true;
                btnRotateRight.Enabled = true;
                btnFitPage1.Enabled = true;
            }
            else
            {

                //btnVerifySignatures.Enabled = true;
                lblSignatureStatus1.Enabled = true;
                btnVerOriginal.Enabled = true;
                lblPage.Enabled = true;
                toolBtnFirstPage1.Enabled = true;
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
                btnRotateLeft.Enabled = true;
                toolBtnResetRotation1.Enabled = true;
                btnRotateRight.Enabled = true;
                btnFitPage1.Enabled = true;
            }

            lblSignatureStatus1.Text = hasSignatures ? "✔ Assinatura Digital" : "✘ Sem assinatura";
            lblSignatureStatus1.ForeColor = hasSignatures ? Color.Green : Color.Red;

            // Reinicia para a primeira página
            pdfViewer3.Invalidate();
            pdfViewer3.Refresh();
            pdfViewer3.ZoomMode = PdfViewerZoomMode.FitBest;
            pdfViewer3.Renderer.Zoom = 1.5f; // zoom 100%
            SetPdfViewer1ControlsVisible(true);
            currentPage = 0;
            pdfViewer3.Renderer.Page = 0;
            UpdatePageUI();
        }

        private bool UserCanAccessCategory(string category)
        {
            return userPermissions.ContainsKey(loggedUser.Username) && userPermissions[loggedUser.Username].Contains(category);
        }

        private bool UserCanAccessSubCategory(string category, string subCategory)
        {
            return userPermissions.ContainsKey(loggedUser.Username) && userPermissions[loggedUser.Username].Contains(subCategory);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm changePasswordForm = new ChangePasswordForm(loggedUser);
            changePasswordForm.ShowDialog();
            LogActivity("Abriu a tela de alteração de senha.");
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {
            ManagementForm settingsForm = new ManagementForm(loggedUser.Username);
            settingsForm.ShowDialog();
            LogActivity("Abriu o painel de configurações.");
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            SaveUserPreferences(true);
            UpdateUserLoginStatus(loggedUser.Username, false);
            UpdateUserOnlineTime(loggedUser.Username);
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }

        private void DocumentViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveUserPreferences(true);
            UpdateUserLoginStatus(loggedUser.Username, false);
            UpdateUserOnlineTime(loggedUser.Username);
            

            // Verificar e remover usuários com status false
            CheckAndRemoveLoggedOutUsers();

            // Registrar a saída do usuário
            if (!loggedUser.LogoutRegistrado)
            {
                LogActivity($"Usuário {loggedUser.Username} deslogou do sistema");
                loggedUser.LogoutRegistrado = true;
            }

            // Fechar o aplicativo completamente
            Application.Exit();
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

        private void LogActivity(string activity)
        {
            try
            {
                string logFilePath = Path.Combine(basePath, "activity_log.txt");
                string logMessage = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - {GetLocalIPAddress()} - {loggedUser.Username} - {activity}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logMessage);
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

        private void BtnToggleDarkMode_Click(object sender, EventArgs e)
        {
            ThemeManager.IsDarkMode = !ThemeManager.IsDarkMode;
            UpdateAllButtonIcons();
            SaveUserPreferences(true); // <-- sempre true aqui!
        }
        private void LoadUserPreferences()
        {
            string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
            if (File.Exists(userPreferencesFilePath))
            {
                try
                {
                    var userPreferences = JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath));
                    if (userPreferences.ContainsKey(loggedUser.Username))
                    {
                        var preferences = userPreferences[loggedUser.Username];
                        ThemeManager.IsDarkMode = preferences.IsDarkMode;

                        //if (preferences.WindowX >= 0 && preferences.WindowY >= 0 && preferences.WindowWidth > 0 && preferences.WindowHeight > 0)
                        //{
                        //    this.StartPosition = FormStartPosition.Manual;
                        //    this.WindowState = FormWindowState.Normal;
                        //    this.Location = new Point(preferences.WindowX, preferences.WindowY);
                        //    this.Size = new Size(preferences.WindowWidth, preferences.WindowHeight);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Erro ao carregar preferências do usuário: {ex.Message}");
                }
            }
        }


        private void SaveUserPreferences(bool isSimpleView)
        {
            string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
            var userPreferences = File.Exists(userPreferencesFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath))
                : new Dictionary<string, UserPreferences>();

            bool isMaximized = this.WindowState == FormWindowState.Maximized;

            userPreferences[loggedUser.Username] = new UserPreferences
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
        private void BtnVisualizacaoDupla_Click(object sender, EventArgs e)
        {
            SaveUserPreferences(false); // false = modo dupla;
            // Usar BeginInvoke para operações de UI na thread correta
            this.BeginInvoke((MethodInvoker)delegate
          {
              this.Hide(); // Esconde o formulário atual (DocumentViewerForm)

              Viewer1 form1 = new Viewer1(loggedUser);
              form1.StartPosition = FormStartPosition.Manual;
              form1.Location = this.Location;
              form1.Size = this.Size;
              form1.ShowDialog();
              // Após o usuário fechar o Viewer1, fecha o DocumentViewerForm original.
              this.Close();
    });
      }

//private void BtnVisualizacaoDupla_Click(object sender, EventArgs e)
//{
//    SaveUserPreferencesForDualView();
//    Viewer1 form1 = new Viewer1(loggedUser);
//    form1.Show(); // <--- use Show() em vez de ShowDialog()
//    this.Close(); // <--- fecha o formulário atual
//}

        //private void SaveUserPreferencesForDualView()
        //{
        //    string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
        //    var userPreferences = File.Exists(userPreferencesFilePath)
        //        ? JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath))
        //        : new Dictionary<string, UserPreferences>();
        //
        //    userPreferences[loggedUser.Username] = new UserPreferences
        //    {
        //        IsDarkMode = ThemeManager.IsDarkMode,
        //        IsSimpleView =  false,
        //        WindowX = this.Location.X,
        //        WindowY = this.Location.Y,
        //        WindowWidth = this.Size.Width,
        //        WindowHeight = this.Size.Height
        //    };
        //
        //    File.WriteAllText(userPreferencesFilePath, JsonConvert.SerializeObject(userPreferences, Formatting.Indented));
        //}

        // Variável para controlar o estado do ajuste
        private bool isAdjusted = false;
        private int toolStripOffsetX;
        private int toolStripOffsetY;

        // Variáveis para armazenar os tamanhos e posições originais
        private System.Drawing.Point originalistBoxFiles3Location;
        private System.Drawing.Size originalListBox3Size;
        private System.Drawing.Point originalPdfViewer3Location;
        private System.Drawing.Size originalPdfViewer3Size;
        private System.Drawing.Point originalbtnToggleListBoxes1Location;
        private System.Drawing.Point originaltoolStripLocation;

        private void btnToggleListBoxes1_Click(object sender, EventArgs e)
        {
            // Verificar se o botão "Ajustar Página" está pressionado
            if (isAdjusted)
            {
                MessageBox.Show("Por favor, pressione 'Página Original' antes de ocultar as listas.");
                return;
            }

            // Alternar a visibilidade das ListBox
            listBoxFiles3.Visible = !listBoxFiles3.Visible;
            
            // Ajustar a posição dos pdfViewer
            if (listBoxFiles3.Visible)
            {
                // Mostrar Listas: voltar para as posições originais
                pdfViewer3.Location = new System.Drawing.Point(317, 97);
                pdfViewer3.Size = new System.Drawing.Size(1300, 875);
                toolStrip.Location = new Point(
                pdfViewer3.Location.X + toolStripOffsetX,
                pdfViewer3.Location.Y + toolStripOffsetY
                );

            }
            else
            {
                // Ocultar Listas: mover os pdfViewer para a esquerda
                pdfViewer3.Location = new System.Drawing.Point(12, 97);                
                pdfViewer3.Size = new System.Drawing.Size(1300, 875);
                toolStrip.Location = new Point(
                pdfViewer3.Location.X + toolStripOffsetX,
                pdfViewer3.Location.Y + toolStripOffsetY
                );
            }

            // Atualizar o ícone do botão
            btnToggleListBoxes1.Image = listBoxFiles3.Visible ? GetButtonImage("toggle_list_close") : GetButtonImage("toggle_list_open");
            //SaveUserPreferences();

            // Atualizar o texto do botão
            //btnToggleListBoxes1.Text = listBoxFiles3.Visible ? "Ocultar Listas" : "Mostrar Listas";
        }

                   

        private void btnToggleAjust1_Click_1(object sender, EventArgs e)
            {
            // Verificar se o botão "Ocultar Listas" está pressionado
            if (!listBoxFiles3.Visible)
            {
                MessageBox.Show("Por favor, mostre as listas antes de ajustar a página.");
                return;
            }

            // Se for a primeira vez que o botão é clicado, armazene os valores originais
            if (!isAdjusted)
            {
               originalistBoxFiles3Location = listBoxFiles3.Location;
               originalListBox3Size = listBoxFiles3.Size;
               originalPdfViewer3Location = pdfViewer3.Location;
               originalPdfViewer3Size = pdfViewer3.Size;
               originalbtnToggleListBoxes1Location = btnToggleListBoxes1.Location;
               originaltoolStripLocation = toolStrip.Location;


            }

            // Alternar entre os tamanhos e posições originais e os ajustados
            if (isAdjusted)
            {
                // Voltar para as posições e tamanhos originais
                listBoxFiles3.Location = originalistBoxFiles3Location;
                
                listBoxFiles3.Size = originalListBox3Size;
               
                pdfViewer3.Location = originalPdfViewer3Location;
                
                pdfViewer3.Size = originalPdfViewer3Size;

                btnToggleListBoxes1.Location = originalbtnToggleListBoxes1Location;
                toolStrip.Location = originaltoolStripLocation;

            }
            else
            {
                // Modificar posição e tamanhos
                listBoxFiles3.Location = new System.Drawing.Point(12, 71);                
                listBoxFiles3.Size = new System.Drawing.Size(150, 600);         
                pdfViewer3.Location = new System.Drawing.Point(179, 71);                
                pdfViewer3.Size = new System.Drawing.Size(700, 600);
                btnToggleListBoxes1.Location = new System.Drawing.Point(233, 39);
                toolStrip.Location = new Point(
                pdfViewer3.Location.X + toolStripOffsetX,
                pdfViewer3.Location.Y + toolStripOffsetY
                );
            }

            // Inverter o estado do ajuste
            isAdjusted = !isAdjusted;

            // Atualizar o texto do botão
            btnToggleAjust1.Text = isAdjusted ? "Página Original" : "Ajustar Página";
        }

        private void btnChangePassword_Click_1(object sender, EventArgs e)
        {
         {
            ChangePasswordForm changePasswordForm = new ChangePasswordForm(loggedUser);
            changePasswordForm.ShowDialog();
            LogActivity("Abriu a tela de alteração de senha.");
        }
    }
        

        private int currentPage = 0;

        private void PreviousPage()
        {
            if (pdfViewer3?.Document == null)
                return;

            if (currentPage > 0)
            {
                currentPage--;
                pdfViewer3.Renderer.Page = currentPage;
                UpdatePageUI();
            }
        }


        private void NextPage()
        {
            if (pdfViewer3?.Document == null)
                return;

            int totalPages = pdfViewer3.Document.PageCount;

            if (currentPage < totalPages - 1)
            {
                currentPage++;
                pdfViewer3.Renderer.Page = currentPage;
                UpdatePageUI();
            }
        }

        private void UpdatePageUI()
        {
            if (pdfViewer3?.Document == null || pdfViewer3.Document.PageCount == 0)
            {
                lblPage.Text = "Nenhum documento carregado";
                return;
            }

            int pageCount = pdfViewer3.Document.PageCount;

            if (currentPage < 0)
                currentPage = 0;
            else if (currentPage >= pageCount)
                currentPage = pageCount - 1;

            lblPage.Text = $"Página {currentPage + 1} de {pageCount}";
        }


        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            Rotate(false);
        }
        private void btnRotateRight_Click_1(object sender, EventArgs e)
        {
            Rotate(true);
        }
        private int rotationState1 = 0; // 0, 90, 180, 270
        private int rotationState2 = 0;


        private void Rotate(bool clockwise)
        {
            if (pdfViewer3.Document == null)
                return;

            int delta = clockwise ? 90 : -90;
            rotationState1 = (rotationState1 + delta + 360) % 360;

            if (clockwise)
                pdfViewer3.Renderer.RotateRight();
            else
                pdfViewer3.Renderer.RotateLeft();

            pdfViewer3.Invalidate(); // Atualiza visual
        }

        private string CheckDigitalSignatures(string filePath)
        {
            try
            {
                PdfReader reader = new PdfReader(filePath);
                AcroFields fields = reader.AcroFields;
                List<string> signatures = fields.GetSignatureNames();

                if (signatures.Count > 0)
                {
                    StringBuilder signatureInfo = new StringBuilder();
                    signatureInfo.AppendLine("Assinaturas digitais encontradas:");

                    foreach (string name in signatures)
                    {
                        signatureInfo.AppendLine($"\nAssinatura: {name}");

                        PdfPKCS7 pkcs7 = fields.VerifySignature(name);
                        if (pkcs7 != null)
                        {
                            // Tenta extrair o nome do CN manualmente
                            var subject = pkcs7.SigningCertificate.SubjectDN.ToString();
                            var cnMatch = Regex.Match(subject, @"CN=([^,]+)");
                            if (cnMatch.Success)
                                signatureInfo.AppendLine($"Assinante: {cnMatch.Groups[1].Value}");
                            else
                                signatureInfo.AppendLine($"Assinante: (Nome não encontrado)");

                            signatureInfo.AppendLine($"Data da assinatura: {pkcs7.SignDate}");
                            signatureInfo.AppendLine($"Válida: {pkcs7.Verify()}");

                            signatureInfo.AppendLine("\nDetalhes do certificado:");
                            signatureInfo.AppendLine($"Emissor: {pkcs7.SigningCertificate.IssuerDN}");
                            signatureInfo.AppendLine($"Válido de: {pkcs7.SigningCertificate.NotBefore}");
                            signatureInfo.AppendLine($"Válido até: {pkcs7.SigningCertificate.NotAfter}");
                        }
                    }

                    return signatureInfo.ToString();
                }
                else
                {
                    return "Nenhuma assinatura digital encontrada neste documento.";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao verificar assinaturas digitais: {ex.Message}";
            }
        }


        private void btnVerifySignatures_Click(object sender, EventArgs e)
        {
            //string currentFilePath = !string.IsNullOrEmpty(currentFilePath) ? currentFilePath :null;
                        

            if (currentFilePath != null && File.Exists(currentFilePath))
            {
                string signatureInfo = CheckDigitalSignatures(currentFilePath);
                MessageBox.Show(signatureInfo, "Assinaturas Digitais",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogActivity($"Verificou a Assinatura Digital no Arquivo: {Path.GetFileName(currentFilePath)}");
            }
            else
            {
                MessageBox.Show("Nenhum documento carregado para verificar assinaturas.",
                               "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool HasSignatures(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    AcroFields fields = reader.AcroFields;
                    return fields.GetSignatureNames().Count > 0;
                }
            }
            catch (Exception ex)
            {
                // Opcional: logar o erro ou debugar
                // Console.WriteLine("Erro ao verificar assinaturas: " + ex.Message);
                return false;
            }
        }

        private void btnVerOriginal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
            {
                try
                {
                    string adobePath = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";

                    // Tenta o caminho alternativo (versão 64 bits)
                    if (!File.Exists(adobePath))
                    {
                        adobePath = @"C:\Program Files\Adobe\Acrobat DC\Acrobat\Acrobat.exe";
                    }

                    if (File.Exists(adobePath))
                    {
                        // Se o Adobe Reader for encontrado, abre com ele
                        System.Diagnostics.Process.Start(adobePath, $"\"{currentFilePath}\"");
                        LogActivity($"Arquivo Orginal aberto: {Path.GetFileName(currentFilePath)}");
                    }
                    else
                    {
                        // Se não for encontrado, abre com o programa padrão do sistema
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = currentFilePath,
                            UseShellExecute = true
                        });
                        LogActivity($"Arquivo Orginal aberto: {Path.GetFileName(currentFilePath)}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o arquivo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogActivity($"Erro ao tentar abrir o arquivo: {Path.GetFileName(currentFilePath)} - {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nenhum arquivo carregado ou caminho inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogActivity("Tentativa de abrir arquivo original falhou: nenhum arquivo carregado ou caminho inválido.");
            }
        }



        private void SetPdfViewer1ControlsVisible(bool visible)
        {
            lblPage.Enabled = visible;
            toolBtnFirstPage1.Enabled = visible;
            btnPrev.Enabled = visible;
            btnNext.Enabled = visible;
            btnRotateLeft.Enabled = visible;
            toolBtnResetRotation1.Enabled = visible;
            btnRotateRight.Enabled = visible;
            btnFitPage1.Enabled = visible;
                      
        }

        private void toolBtnFirstPage1_Click(object sender, EventArgs e)
        {
            toolBtnFirstPage1.Enabled = true;

            if (pdfViewer3.Document != null)
            {
                currentPage = 0;
                pdfViewer3.Renderer.Page = currentPage;
                UpdatePageUI();
            }
        }

        private void toolBtnResetRotation1_Click(object sender, EventArgs e)
        {
            if (pdfViewer3.Document == null) return;

            while (rotationState1 != 0)
            {
                pdfViewer3.Renderer.RotateLeft(); // desfaz 90°
                rotationState1 = (rotationState1 - 90 + 360) % 360;
            }

            pdfViewer3.Invalidate();
        }

        private void toolTip4_Draw(object sender, DrawToolTipEventArgs e)
        {
            System.Drawing.Font tooltipFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);

            // Desenha o fundo e a borda
            e.Graphics.FillRectangle(new SolidBrush(toolTip4.BackColor), e.Bounds);
            e.Graphics.DrawRectangle(Pens.Gray, e.Bounds);

            // Desenha o texto
            TextRenderer.DrawText(
                e.Graphics,
                e.ToolTipText,
                tooltipFont,
                e.Bounds,
                toolTip4.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left
            );
        }

        private void toolTip4_Popup(object sender, PopupEventArgs e)
        {
            System.Drawing.Font tooltipFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            Size textSize = TextRenderer.MeasureText(toolTip4.GetToolTip(e.AssociatedControl), tooltipFont);

            // Ajuste manual de margem (opcional)
            e.ToolTipSize = new Size(textSize.Width + 1, textSize.Height + 1);
        }

        private void btnFitPage1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
            {
                try
                {
                    pdfViewer3.Document?.Dispose();

                    var document = PdfiumViewerPdfDocument.Load(currentFilePath);
                    pdfViewer3.Document = document;

                    // Resetar zoom para melhor ajuste
                    pdfViewer3.ZoomMode = PdfViewerZoomMode.FitBest;
                    pdfViewer3.Renderer.Zoom = 1.5f; // zoom 100%
                    pdfViewer3.Invalidate();
                    pdfViewer3.Refresh();

                    currentPage = 0;
                    pdfViewer3.Renderer.Page = currentPage;
                    UpdatePageUI();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao recarregar documento: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool IsInMaintenance(out string mensagem, string userRole = null)
        {
            mensagem = "";
            try
            {
                string manutencaoFilePath = Path.Combine(AppConfig.GetDatabasePath(), "manutencao.json");
                if (File.Exists(manutencaoFilePath))
                {
                    var json = JObject.Parse(File.ReadAllText(manutencaoFilePath));
                    bool status = json["status"]?.Value<bool>() ?? false;
                    var msgOriginal = json["mensagem"]?.ToString() ?? "O sistema está em manutenção. Tente novamente mais tarde.";

                    // Para admin, mostra um aviso diferente mas não bloqueia
                    if (status && userRole != null && userRole.ToLower() == "admin")
                    {
                        mensagem = "[MODO MANUTENÇÃO ATIVO]\n" + msgOriginal;
                        return false; // Não bloqueia!
                    }

                    // bloqueio com vários usuários liberados
                    //var rolesLiberados = new[] { "admin", "superadmin" };
                    //if (status && userRole != null && !rolesLiberados.Contains(userRole.ToLower()))
                    // return true;
                    // Só bloqueia se não for admin
                    if (status && userRole != null && userRole.ToLower() != "admin")
                    {
                        mensagem = msgOriginal;
                        return true;
                    }

                    // Se não informar role, bloqueia geral
                    if (status && string.IsNullOrEmpty(userRole))
                    {
                        mensagem = msgOriginal;
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }             
                   
                    
    }
}
    
