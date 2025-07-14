using iTextSharp.text;
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
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using Newtonsoft.Json.Linq; // Adiciona no topo
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharpPdfDocument = iTextSharp.text.pdf.PdfDocument;
using PdfiumViewerPdfDocument = PdfiumViewer.PdfDocument;
using WinFormsButton = System.Windows.Forms.Button;
using WinFormsComboBox = System.Windows.Forms.ComboBox;
using WinFormsToolTip = System.Windows.Forms.ToolTip;

namespace DocsViewer 
{


    public partial class Viewer1 : Form
    {
        private readonly User loggedUser;
        private string currentFilePath1;
        private string currentFilePath2;
        private System.Timers.Timer inactivityTimer;
        private System.Windows.Forms.ToolTip toolTip3;
        private readonly string loggedInUser;

        private Dictionary<string, List<string>> userPermissions;
        private string basePath => AppConfig.GetDatabasePath();
        private string documentsPath => AppConfig.GetDocumentsPath();

        public class BorderlessToolStripRenderer : System.Windows.Forms.ToolStripSystemRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // Não renderiza borda
            }
        }


        // Importa a função SetWindowLong da API do Windows
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // Importa a função GetWindowLong da API do Windows
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // Importa a função SystemParametersInfo da API do Windows
        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref RECT pvParam, uint fWinIni);

        private const uint SPI_GETWORKAREA = 0x0030;

        // Estrutura RECT usada para obter a área de trabalho
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public System.Drawing.Rectangle ToRectangle()
            {
                return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }


        private int rotationState1 = 0; // 0, 90, 180, 270
        private int rotationState2 = 0;

        // Constantes usadas para definir o comportamento da janela
        private const int GWL_STYLE = -16;
        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_SYSMENU = 0x00080000;

        public Viewer1(User user )
        {
            InitializeComponent();
            loggedUser = user;           
            LoadUserPreferences();
            DesabilitarPrintsPadraoPdfium();


            toolStrip1.Renderer = new BorderlessToolStripRenderer();
            toolStrip2.Renderer = new BorderlessToolStripRenderer();
            this.Shown += Viewer1_Shown; // Adiar inicialização pesada           
            this.ActiveControl = null;  //this = form
            this.KeyPreview = true; // Isso permite que o formulário capture os eventos de tecla primeiro
            AddInactivityHandlers(this);
            LogoHelper.AplicarLogoComoIcon(this);
            //this.loggedInUser = loggedInUser;
        }
        private void Viewer1_Shown(object sender, EventArgs e)

        {
            // Agora o controle tem seu handle criado
            LoadFormDataSafe();

        }
        private bool isListOpen1 = true; // true = aberta ao iniciar, mude se seu padrão for outro
        private bool isListOpen2 = true; // true = aberta ao iniciar, mude se seu padrão for outro
        private void LoadFormDataSafe()
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Focus();

                // 1. Carregue dados básicos
                userPermissions = LoadUserPermissions();                
                SetPdfViewer1ControlsVisible(false);
                // 2. Configure controles visíveis                
                btnManagement.Enabled = loggedUser.Role == "admin";
                btnPrintComCarimbo1.Visible = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                btnPrintComCarimbo2.Visible = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                //btnToggleListBoxes1.Visible = loggedUser.Role == "admin" || loggedUser.Role == "viewer" || loggedUser.Role == "editor";
                //btnToggleListBoxes2.Visible = loggedUser.Role == "admin" || loggedUser.Role == "viewer" || loggedUser.Role == "editor";
                btnPrintComCarimbo1.Enabled = false;
                btnPrintComCarimbo2.Enabled = false;
                // 3. Carregue combobox                
                PopulateCategoryComboBox(comboBoxCategory1);
                PopulateCategoryComboBox(comboBoxCategory2);
                SetupMaintenanceTimer();

                UpdateAllButtonIcons();  // Depois atualiza os demais

                // 5. Aplique o tema
                ThemeManager.ApplyTheme(this);

                // 6. Configure PDF Viewer (agora seguro)
                ConfigurePdfViewerPermissions();
                SetupInactivityTimer();

                // Define o comportamento da janela para aparecer na barra de tarefas
                int style = GetWindowLong(this.Handle, GWL_STYLE);
                SetWindowLong(this.Handle, GWL_STYLE, style | WS_MINIMIZEBOX | WS_SYSMENU);

                // Ajusta o formulário para caber corretamente na área de trabalho
                AdjustFormToFitScreen();

                // 4. Configure placeholders
                ConfigureSearchPlaceholder();


                //pdfViewer1.ShowToolbar = false; // Tente isso, se disponível

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao iniciar o programa: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Erro ao iniciar o programa: " + ex.Message);
                Application.Exit();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            toolTip3 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip3);

            // Tooltips para botões comuns
            toolTip3.SetToolTip(btnManagement, "Menu de Gerenciamento (F2)");
            toolTip3.SetToolTip(btnChangePassword, "Trocar Senha (F3)");
            toolTip3.SetToolTip(btnVisualizacaoSimples, "Mudar para Visualização Simples (F4)");
            toolTip3.SetToolTip(btnToggleDarkMode, "Mudar mode Escuro/Claro (F5)");
            toolTip3.SetToolTip(btnToggleAjust, "Ajustar Página (F6)");
            toolTip3.SetToolTip(btnLogout, "Sair do Sistema (F7)");
            toolTip3.SetToolTip(btnToggleListBoxes1, "Fechar a Lista de Arquivos (F8)");
            toolTip3.SetToolTip(btnToggleListBoxes2, "Fechar a Lista de Arquivos (F9)");
            toolTip3.SetToolTip(buttonSearch1, "Pesquisar Arquivos (ENTER)");
            toolTip3.SetToolTip(buttonSearch2, "Pesquisar Arquivos (INSERT)");
            toolTip3.SetToolTip(comboBoxCategory1, "Filtrar Categoria de Arquivos");
            toolTip3.SetToolTip(comboBoxCategory2, "Filtrar Categoria de Arquivos");
            toolTip3.SetToolTip(comboBoxSubCategory1, "Filtrar Sub Categoria de Arquivos");
            toolTip3.SetToolTip(comboBoxSubCategory2, "Filtrar Sub Categoria de Arquivos");
            toolTip3.SetToolTip(textBoxSearch1, "Digitar Texto para Buscar Arquivos");
            toolTip3.SetToolTip(textBoxSearch2, "Digitar Texto para Buscar Arquivos");
            

            // Tooltips para ToolStripButtons
            //toolBtnFirstPage1.ToolTipText = "Ir para a primeira página";
            //toolBtnResetRotation1.ToolTipText = "Restaurar rotação para 0°";

            textBoxSearch1.Enter += (s, e) => this.AcceptButton = buttonSearch1;
            textBoxSearch2.Enter += (s, e) => this.AcceptButton = buttonSearch2;
           
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
        private void Viewer1_KeyDown_1(object sender, KeyEventArgs e)
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
                    btnVisualizacaoSimples.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F5)
                {
                    btnToggleDarkMode.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F6)
                {
                    btnToggleAjust.PerformClick(); // Simula um clique no botão
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
                if (e.KeyCode == Keys.F9)
                {
                    btnToggleListBoxes2.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.Enter)
                {
                    buttonSearch1.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.Insert)
                {
                    buttonSearch2.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.Alt && e.KeyCode == Keys.H)
                {
                    btnVerifySignatures1.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.Alt && e.KeyCode == Keys.J)
                {
                    btnVerOriginal1.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.Alt && e.KeyCode == Keys.F)
                {
                    btnVerifySignatures2.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.Alt && e.KeyCode == Keys.G)
                {
                    btnVerOriginal2.PerformClick(); // Simula um clique no botão
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
            var ts1 = pdfViewer1.Controls.OfType<ToolStrip>().FirstOrDefault();
            var ts2 = pdfViewer2.Controls.OfType<ToolStrip>().FirstOrDefault();
            if (ts1 != null) DesabilitarBotaoPrintPadrao(ts1);
            if (ts2 != null) DesabilitarBotaoPrintPadrao(ts2);
        }

        private void ConfigureSearchPlaceholder()
        {
            // Configurar placeholder da busca 1
            textBoxSearch1.Text = "Pesquisar Arquivos...";
            textBoxSearch1.ForeColor = ThemeManager.TextBoxPlaceholderColor;
            textBoxSearch1.Tag = "HasPlaceholder_Pesquisar Arquivos..."; // Marca como TextBox com placeholder

            textBoxSearch1.GotFocus += (s, e) =>
            {
                if (textBoxSearch1.Text == "Pesquisar Arquivos...")
                {
                    textBoxSearch1.Text = "";
                    textBoxSearch1.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            textBoxSearch1.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBoxSearch1.Text))
                {
                    textBoxSearch1.Text = "Pesquisar Arquivos...";
                    textBoxSearch1.ForeColor = ThemeManager.TextBoxPlaceholderColor;
                }
            };

            // Configurar placeholder da busca 1
            textBoxSearch2.Text = "Pesquisar Arquivos...";
            textBoxSearch2.ForeColor = ThemeManager.TextBoxPlaceholderColor;
            textBoxSearch2.Tag = "HasPlaceholder_Pesquisar Arquivos..."; // Marca como TextBox com placeholder

            textBoxSearch2.GotFocus += (s, e) =>
            {
                if (textBoxSearch2.Text == "Pesquisar Arquivos...")
                {
                    textBoxSearch2.Text = "";
                    textBoxSearch2.ForeColor = ThemeManager.IsDarkMode ? Color.White : SystemColors.WindowText;
                }
            };

            textBoxSearch2.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBoxSearch2.Text))
                {
                    textBoxSearch2.Text = "Pesquisar Arquivos...";
                    textBoxSearch2.ForeColor = ThemeManager.TextBoxPlaceholderColor;
                }
            };

            // **Remove o foco do TextBox e define para outro controle**
            this.ActiveControl = null; // Remove o foco de qualquer controle
                                       // OU
            this.Focus(); // Foca em um Label (se tiver um)

        }



        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                buttonSearch1.Image = GetButtonImage("search");
                buttonSearch2.Image = GetButtonImage("search");
                btnToggleListBoxes1.Image = GetButtonImage("toggle_list_close");
                btnToggleListBoxes2.Image = GetButtonImage("toggle_list_close");
                btnVisualizacaoSimples.Image = GetButtonImage("simple_view");
                btnToggleDarkMode.Image = GetButtonImage("dark_mode");
                btnChangePassword.Image = GetButtonImage("password");
                btnManagement.Image = GetButtonImage("management");
                btnLogout.Image = GetButtonImage("logout1");
                btnToggleAjust.Image = GetButtonImage("adjust");

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

        private void UpdateToggleButtonIcon(ListBox listBox, WinFormsButton toggleButton)
        {
            // Se for a primeira vez, mantém o padrão "fechar lista"
            //if (toggleButton.Tag == null)
            // {
            //    toggleButton.Tag = "initialized"; // Marca como já inicializado
            //     return;
            //}

            // Lógica normal de alternância após a inicialização
            if (ThemeManager.IsDarkMode)
            {
                toggleButton.Image = listBox.Visible
                    ? Properties.Resources.toggle_list_close_dark
                    : Properties.Resources.toggle_list_open_dark;
            }
            else
            {
                toggleButton.Image = listBox.Visible
                    ? Properties.Resources.toggle_list_close_light
                    : Properties.Resources.toggle_list_open_light;
            }
        }


        private void AdjustFormToFitScreen()
        {
            RECT workArea = new RECT();
            if (SystemParametersInfo(SPI_GETWORKAREA, 0, ref workArea, 0))
            {
                System.Drawing.Rectangle workRectangle = workArea.ToRectangle();

                // Define o tamanho e a posição do formulário para caber na área de trabalho
                this.Location = new Point(workRectangle.Left, workRectangle.Top);
                this.Size = new Size(workRectangle.Width, workRectangle.Height);
            }
            else
            {
                MessageBox.Show("Não foi possível obter a área de trabalho.");
            }
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
        private void AddInactivityHandlers(Control parent)
        {
            parent.MouseMove += ResetInactivityTimer;
            parent.KeyPress += ResetInactivityTimer;

            foreach (Control ctrl in parent.Controls)
            {
                AddInactivityHandlers(ctrl); // recursivo, pega todos os filhos
            }
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
                            y = tamanhoPagina.Top - alturaCarimbo - 250;
                        }
                        else
                        {
                            // Canto superior esquerdo (retrato)
                            x = tamanhoPagina.Left - larguraCarimbo + 80;
                            y = tamanhoPagina.Top - alturaCarimbo - 20;
                        }
                        carimbo.SetAbsolutePosition(x, y);

                        var content = stamper.GetOverContent(page);
                        content.AddImage(carimbo);
                    }
                }
            }
        }

        private void btnPrintComCarimbo1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath1) || !File.Exists(currentFilePath1))
            {
                MessageBox.Show("Nenhum documento selecionado para imprimir.");
                return;
            }

            // VIEWER: botão já desabilitado no Load

            if (loggedUser.Role == "editor")
            {
                // Imprime COM carimbo
                string caminhoCarimbo1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "carimbo.png");
                string tempPdf = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
                AdicionarCarimboPrimeiraPagina(currentFilePath1, tempPdf, caminhoCarimbo1);
                ImprimirPDFComPdfium(tempPdf);
                LogActivity($"Imprimiu (com carimbo) o documento: {Path.GetFileName(currentFilePath1)}");
            }
            else if (loggedUser.Role == "admin")
            {
                // Imprime NORMAL (sem carimbo)
                ImprimirPDFComPdfium(currentFilePath1);
                LogActivity($"Imprimiu (sem carimbo) o documento: {Path.GetFileName(currentFilePath1)}");
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
    



        private void btnPrintComCarimbo2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath2) || !File.Exists(currentFilePath2))
            {
                MessageBox.Show("Nenhum documento selecionado para imprimir.");
                return;
            }

            // VIEWER: botão já desabilitado no Load

            if (loggedUser.Role == "editor")
            {
                // Imprime COM carimbo
                string caminhoCarimbo2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "carimbo.png");
                string tempPdf = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
                AdicionarCarimboPrimeiraPagina(currentFilePath2, tempPdf, caminhoCarimbo2);
                ImprimirPDFComPdfium(tempPdf);
                LogActivity($"Imprimiu (com carimbo) o documento: {Path.GetFileName(currentFilePath2)}");
            }
            else if (loggedUser.Role == "admin")
            {
                // Imprime NORMAL (sem carimbo)
                ImprimirPDFComPdfium(currentFilePath2);
                LogActivity($"Imprimiu (sem carimbo) o documento: {Path.GetFileName(currentFilePath2)}");
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


        private void ConfigurePdfViewerPermissions()
        {
            var toolStrip1 = pdfViewer1.Controls.OfType<ToolStrip>().FirstOrDefault();
            var toolStrip2 = pdfViewer2.Controls.OfType<ToolStrip>().FirstOrDefault();

            // Ajusta ToolStrip1
            if (toolStrip1 != null)
            {
                toolStrip1.SuspendLayout();
                foreach (ToolStripItem item in toolStrip1.Items)
                {
                    item.AutoSize = false;
                    item.Size = new Size(25, 25);
                    item.Padding = new Padding(2);
                }
                foreach (ToolStripItem item in toolStrip1.Items)
                {
                    if (item.Text == "Save")
                    {
                        item.Click += (s, e) =>
                        {
                            if (listBoxFiles1.SelectedItem != null)
                            {
                                string selectedFileName = listBoxFiles1.SelectedItem.ToString();
                                LogActivity($"Salvou o documento: {selectedFileName}");
                            }
                        };
                        item.Enabled = loggedUser.Role == "admin";
                    }
                    else if (item.Text == "Print")
                    {
                        item.Click -= null; // Remove handlers antigos (opcional)

                        item.Click += (s, e) =>
                        {
                            if (listBoxFiles1.SelectedItem != null && !string.IsNullOrEmpty(currentFilePath1))
                            {
                                string filePath = currentFilePath1;
                                if (File.Exists(filePath))
                                {
                                    ImprimirComCarimbo(filePath);
                                    LogActivity($"Imprimiu (com carimbo) o documento: {Path.GetFileName(filePath)}");
                                }
                                else
                                {
                                    MessageBox.Show("Arquivo não encontrado!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Nenhum documento selecionado para imprimir.");
                            }
                        };
                        item.Enabled = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                    }
                }
                toolStrip1.ResumeLayout(true);
                toolStrip1.PerformLayout();
            }

            // Ajusta ToolStrip2
            if (toolStrip2 != null)
            {
                toolStrip2.SuspendLayout();
                foreach (ToolStripItem item2 in toolStrip2.Items)
                {
                    item2.AutoSize = false;
                    item2.Size = new Size(25, 25);
                    item2.Padding = new Padding(2);
                }
                foreach (ToolStripItem item2 in toolStrip2.Items)
                {
                    if (item2.Text == "Save")
                    {
                        item2.Click += (s, e) =>
                        {
                            if (listBoxFiles2.SelectedItem != null)
                            {
                                string selectedFileName = listBoxFiles2.SelectedItem.ToString();
                                LogActivity($"Salvou o documento: {selectedFileName}");
                            }
                        };
                        item2.Enabled = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                    }
                    else if (item2.Text == "Print")
                    {
                        item2.Click -= null; // Remove handlers antigos (opcional)

                        item2.Click += (s, e) =>
                        {
                            if (listBoxFiles2.SelectedItem != null && !string.IsNullOrEmpty(currentFilePath2))
                            {
                                string filePath = currentFilePath2;
                                if (File.Exists(filePath))
                                {
                                    ImprimirComCarimbo(filePath);
                                    LogActivity($"Imprimiu (com carimbo) o documento: {Path.GetFileName(filePath)}");
                                }
                                else
                                {
                                    MessageBox.Show("Arquivo não encontrado!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Nenhum documento selecionado para imprimir.");
                            }
                        };
                        item2.Enabled = loggedUser.Role == "admin" || loggedUser.Role == "editor";
                    }
                }
                toolStrip2.ResumeLayout(true);
                toolStrip2.PerformLayout();
            }
        }
    
        private void ImprimirComCarimbo(string caminhoPdf)
        {
            if (string.IsNullOrEmpty(caminhoPdf) || !File.Exists(caminhoPdf))
            {
                MessageBox.Show("Nenhum PDF carregado!");
                return;
            }

            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
            AdicionarCarimboAoPdf(caminhoPdf, tempFile);

            // Usa o PdfiumViewer ou qualquer método que você já usa para imprimir:
            using (var document = PdfiumViewer.PdfDocument.Load(tempFile))
            {
                using (var printDoc = document.CreatePrintDocument())
                {
                    printDoc.PrinterSettings = new System.Drawing.Printing.PrinterSettings(); // Default, ou peça pro usuário escolher
                    printDoc.Print();
                }
            }
            File.Delete(tempFile);
        }

        private void AdicionarCarimboAoPdf(string inputPath, string outputPath)
        {
            using (var reader = new iTextSharp.text.pdf.PdfReader(inputPath))
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            using (var stamper = new iTextSharp.text.pdf.PdfStamper(reader, fs))
            {
                int n = reader.NumberOfPages;
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(@"C:\Users\Qualidade02\Desktop\DocsViewer - Versão final - Release  v1.0 imprimir carimbo\carimbo não controlado.png"); // Coloque o caminho do PNG do carimbo

                for (int i = 1; i <= n; i++)
                {
                    var pdfContentByte = stamper.GetOverContent(i);
                    img.ScalePercent(0f); // Ajuste o tamanho do carimbo!
                    img.SetAbsolutePosition(40, 40); // Ajuste a posição do carimbo
                    pdfContentByte.AddImage(img);
                }
                stamper.Close();
            }
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
            if (sender is WinFormsComboBox comboBox)
            {
                var subCategoryComboBox = comboBox == comboBoxCategory1 ? comboBoxSubCategory1 : comboBoxSubCategory2;
                PopulateSubCategoryComboBox(subCategoryComboBox, comboBox.SelectedItem.ToString());
            }
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

        private void ButtonSearch1_Click(object sender, EventArgs e)
        {
            LogActivity("Buscou na Categoria 1");
            string selectedCategory = comboBoxCategory1.SelectedItem.ToString();
            string selectedSubCategory = comboBoxSubCategory1.SelectedItem.ToString();
            string searchPattern = textBoxSearch1.Text;
            if (!isListOpen1)
            {
                MessageBox.Show("Abra a lista de Arquivos antes de buscar arquivos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "Pesquisar Arquivos...")
            {
                searchPattern = ""; // Define como vazio para buscar todos os PDFs
            }
            listBoxFiles1.Items.Clear();


            if (selectedCategory == "All Categories")
            {
                var categories = Directory.GetDirectories(documentsPath).Select(Path.GetFileName);
                foreach (var category in categories)
                {
                    if (UserCanAccessCategory(category))
                    {
                        SearchFiles(category, selectedSubCategory, searchPattern, listBoxFiles1);
                    }
                }
            }
            else
            {
                SearchFiles(selectedCategory, selectedSubCategory, searchPattern, listBoxFiles1);
            }
        }

        private void ButtonSearch2_Click(object sender, EventArgs e)
        {
            LogActivity("Buscou na Categoria 2");
            string selectedCategory = comboBoxCategory2.SelectedItem.ToString();
            string selectedSubCategory = comboBoxSubCategory2.SelectedItem.ToString();
            string searchPattern = textBoxSearch2.Text;
            if (!isListOpen2)
            {
                MessageBox.Show("Abra a lista de Arquivos antes de buscar arquivos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "Pesquisar Arquivos...")
            {
                searchPattern = ""; // Define como vazio para buscar todos os PDFs
            }
            listBoxFiles2.Items.Clear();

            if (selectedCategory == "All Categories")
            {
                var categories = Directory.GetDirectories(documentsPath).Select(Path.GetFileName);
                foreach (var category in categories)
                {
                    if (UserCanAccessCategory(category))
                    {
                        SearchFiles(category, selectedSubCategory, searchPattern, listBoxFiles2);
                    }
                }
            }
            else
            {
                SearchFiles(selectedCategory, selectedSubCategory, searchPattern, listBoxFiles2);
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

        private void ListBoxFiles1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFiles1.SelectedItem != null)
            {
                string selectedFileName = listBoxFiles1.SelectedItem.ToString();
                currentFilePath1 = GetFilePath(selectedFileName);
                DisplayPdf1(currentFilePath1);
                LogActivity($"Visualizou o arquivo {selectedFileName} na Categoria 1");

                // Habilita o botão de imprimir COM carimbo se o arquivo existir
                btnPrintComCarimbo1.Enabled = !string.IsNullOrEmpty(currentFilePath1) && File.Exists(currentFilePath1);
            }
            else
            {
                // Desabilita o botão se não tem arquivo selecionado
                btnPrintComCarimbo1.Enabled = false;
            }
        }

        private void ListBoxFiles2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFiles2.SelectedItem != null)
            {
                string selectedFileName = listBoxFiles2.SelectedItem.ToString();
                currentFilePath2 = GetFilePath(selectedFileName);
                DisplayPdf2(currentFilePath2);
                LogActivity($"Visualizou o arquivo {selectedFileName} na Categoria 2");

                // Habilita o botão de imprimir COM carimbo se o arquivo existir
                btnPrintComCarimbo2.Enabled = !string.IsNullOrEmpty(currentFilePath2) && File.Exists(currentFilePath2);
            }
            else
            {
                // Desabilita o botão se não tem arquivo selecionado
                btnPrintComCarimbo2.Enabled = false;
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

        private void DisplayPdf1(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            // Carrega o documento no visualizador
            var document = PdfiumViewer.PdfDocument.Load(filePath);
            pdfViewer1.Document = document;
            

            // Atualiza o caminho atual
            currentFilePath1 = filePath;

            // Verifica se há assinaturas digitais
            bool hasSignatures = HasSignatures(filePath);

            // Habilita ou desabilita o botão e o label de acordo com a presença de assinatura
            btnVerifySignatures1.Enabled = hasSignatures;
            lblSignatureStatus1.Enabled = hasSignatures;
            btnVerOriginal1.Enabled = hasSignatures;


            if (loggedUser.Role == "viewer" || loggedUser.Role == "editor")
            {
                // Desabilita os botão de acordo com a regra de usário
                btnVerifySignatures1.Enabled = false;
                lblSignatureStatus1.Enabled = true;
                btnVerOriginal1.Enabled = false;
                lblPage1.Enabled = true;
                toolBtnFirstPage1.Enabled = true;
                btnPrev1.Enabled = true;
                btnNext1.Enabled = true;
                btnRotateLeft1.Enabled = true;
                toolBtnResetRotation1.Enabled = true;
                btnRotateRight1.Enabled = true;
                btnFitPage1.Enabled = true;
            }
            else
            {

                btnVerifySignatures1.Enabled = true;
                lblSignatureStatus1.Enabled = true;
                btnVerOriginal1.Enabled = true;
                lblPage1.Enabled = true;
                toolBtnFirstPage1.Enabled = true;
                btnPrev1.Enabled = true;
                btnNext1.Enabled = true;
                btnRotateLeft1.Enabled = true;
                toolBtnResetRotation1.Enabled = true;
                btnRotateRight1.Enabled = true;
                btnFitPage1.Enabled = true;
            }

            lblSignatureStatus1.Text = hasSignatures ? "✔ Assinatura Digital" : "✘ Sem assinatura";
            lblSignatureStatus1.ForeColor = hasSignatures ? Color.Green : Color.Red;

            // Reinicia para a primeira página
            pdfViewer1.Invalidate();
            pdfViewer1.Refresh();
            pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;
            pdfViewer1.Renderer.Zoom = 1.0f; // zoom 100%
            SetPdfViewer1ControlsVisible(true);
            currentPage1 = 0;
            pdfViewer1.Renderer.Page = 0;
            UpdatePageUI1();
            
        }

        private void DisplayPdf2(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            // Carrega o documento no visualizador
            var document = PdfiumViewer.PdfDocument.Load(filePath);
            pdfViewer2.Document = document;

            // Atualiza o caminho atual
            currentFilePath2 = filePath;

            // Verifica se há assinaturas digitais
            bool hasSignatures = HasSignatures(filePath);

            // Habilita ou desabilita o botão e o label de acordo com a presença de assinatura
            btnVerifySignatures2.Enabled = hasSignatures;
            lblSignatureStatus2.Enabled = hasSignatures;
            btnVerOriginal2.Enabled = hasSignatures;


            if (loggedUser.Role == "viewer" || loggedUser.Role == "editor")
            {
                // Desabilita os botão de acordo com a regra de usário
                btnVerifySignatures2.Enabled = false;
                lblSignatureStatus2.Enabled = true;
                btnVerOriginal2.Enabled = false;
                lblPage2.Enabled = true;
                toolBtnFirstPage2.Enabled = true;
                btnPrev2.Enabled = true;
                btnNext2.Enabled = true;
                btnRotateLeft2.Enabled = true;
                toolBtnResetRotation2.Enabled = true;
                btnRotateRight2.Enabled = true;
                btnFitPage2.Enabled = true;
            }
            else
            {

                btnVerifySignatures2.Enabled = true;
                lblSignatureStatus2.Enabled = true;
                btnVerOriginal2.Enabled = true;
                lblPage2.Enabled = true;
                toolBtnFirstPage2.Enabled = true;
                btnPrev2.Enabled = true;
                btnNext2.Enabled = true;
                btnRotateLeft2.Enabled = true;
                toolBtnResetRotation2.Enabled = true; ;
                btnRotateRight2.Enabled = true;
                btnFitPage2.Enabled = true;
            }

            lblSignatureStatus2.Text = hasSignatures ? "✔ Assinatura Digital" : "✘ Sem assinatura";
            lblSignatureStatus2.ForeColor = hasSignatures ? Color.Green : Color.Red;

            // Reinicia para a primeira página
            pdfViewer1.Invalidate();
            pdfViewer1.Refresh();
            pdfViewer2.ZoomMode = PdfViewerZoomMode.FitBest;
            SetPdfViewer1ControlsVisible(true);
            currentPage2 = 0;
            pdfViewer2.Renderer.Page = 0;
            UpdatePageUI2();
            
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
            SaveUserPreferences(false);
            UpdateUserLoginStatus(loggedUser.Username, false);
            UpdateUserOnlineTime(loggedUser.Username);
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowPositionAndTheme(); // <-- só atualiza posição/tema
            UpdateUserLoginStatus(loggedUser.Username, false);
            UpdateUserOnlineTime(loggedUser.Username);

            CheckAndRemoveLoggedOutUsers();
            if (!loggedUser.LogoutRegistrado)
            {
                LogActivity($"Usuário {loggedUser.Username} deslogou do sistema");
                loggedUser.LogoutRegistrado = true;
            }
            Application.Exit();

        }
        private void SaveWindowPositionAndTheme()
        {
            string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
            var userPreferences = File.Exists(userPreferencesFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath))
                : new Dictionary<string, UserPreferences>();

            // Lê o valor atual salvo, para não mudar IsSimpleView
            var isSimpleView = userPreferences.ContainsKey(loggedUser.Username)
                ? userPreferences[loggedUser.Username].IsSimpleView
                : false; // ou true, conforme padrão

            bool isMaximized = this.WindowState == FormWindowState.Maximized;

            userPreferences[loggedUser.Username] = new UserPreferences
            {
                IsDarkMode = ThemeManager.IsDarkMode,
                IsSimpleView = isSimpleView, // NÃO MUDE!
                //WindowX = isMaximized ? this.RestoreBounds.X : this.Location.X,
                //WindowY = isMaximized ? this.RestoreBounds.Y : this.Location.Y,
                //WindowWidth = isMaximized ? this.RestoreBounds.Width : this.Size.Width,
                //WindowHeight = isMaximized ? this.RestoreBounds.Height : this.Size.Height
            };

            File.WriteAllText(userPreferencesFilePath, JsonConvert.SerializeObject(userPreferences, Formatting.Indented));
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
            UpdateAllButtonIcons();
            //ThemeManager.IsDarkMode = !ThemeManager.IsDarkMode;
            Console.WriteLine($"Antes: {ThemeManager.IsDarkMode}");
            ThemeManager.IsDarkMode = !ThemeManager.IsDarkMode;
            Console.WriteLine($"Depois: {ThemeManager.IsDarkMode}");
            UpdateAllButtonIcons();
            SaveUserPreferences(false);
            btnToggleDarkMode.Text = ThemeManager.IsDarkMode ? "Modo Claro" : "Modo Escuro";
        }

        private void LoadUserPreferences()
        {
            string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
            if (File.Exists(userPreferencesFilePath))
            {
                try // Adicionar try-catch para robustez na leitura/deserialização
                {
                    var userPreferences = JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath));
                    if (userPreferences.ContainsKey(loggedUser.Username))
                    {
                        var preferences = userPreferences[loggedUser.Username];
                        ThemeManager.IsDarkMode = preferences.IsDarkMode; // Carrega preferência de tema

                        // Se a preferência for por visualização simples, alterna o formulário
                        if (preferences.IsSimpleView)
                        {
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                this.Hide(); // Esconde o Viewer1
                                DocumentViewerForm documentViewerForm = new DocumentViewerForm(loggedUser);
                                documentViewerForm.ShowDialog(); // Mostra o DocumentViewerForm (simples)
                                this.Close(); // Fecha o Viewer1 original após o simples ser fechado
                            });
                            return; // Impede o resto do LoadFormDataSafe de executar para Viewer1
                        }

                        // --- RESTAURA POSIÇÃO E TAMANHO (só para Viewer1) ---
                        //if (preferences.WindowX >= 0 && preferences.WindowY >= 0 && preferences.WindowWidth > 0 && preferences.WindowHeight > 0)
                        //{
                        //    this.StartPosition = FormStartPosition.Manual;
                        //    this.Location = new Point(preferences.WindowX, preferences.WindowY);
                        //    this.Size = new Size(preferences.WindowWidth, preferences.WindowHeight);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    // Logar o erro ou informar o usuário sobre falha ao carregar preferências
                    Logger.Log($"Erro ao carregar preferências do usuário: {ex.Message}");
                    // Continuar com as configurações padrão ou tomar outra ação apropriada
                }
            }
            // Se não houver preferências ou IsSimpleView for false, continua carregando Viewer1 normalmente.
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

        //private void BtnVisualizacaoSimples_Click(object sender, EventArgs e)
        //{
        //    SaveUserPreferencesForSimpleView();
        //    DocumentViewerForm documentViewerForm = new DocumentViewerForm(loggedUser);
        //    documentViewerForm.Show(); // <--- use Show()
        //    this.Close(); // <--- fecha o formulário atual
        //}


        private void BtnVisualizacaoSimples_Click(object sender, EventArgs e)
      {
            SaveUserPreferences(true);
            // Usar BeginInvoke para operações de UI na thread correta
            this.BeginInvoke((MethodInvoker)delegate
          {
              this.Hide(); // Esconde o formulário atual (DocumentViewerForm)
      
              DocumentViewerForm documentViewerForm = new DocumentViewerForm(loggedUser);
              documentViewerForm.StartPosition = FormStartPosition.Manual;
              documentViewerForm.Location = this.Location;
              documentViewerForm.Size = this.Size; // Se quiser manter o mesmo tamanho também
              documentViewerForm.ShowDialog();
              // Após o usuário fechar o Viewer1, fecha o DocumentViewerForm original.
              this.Close();
          });
      }

        //private void SaveUserPreferencesForSimpleView()
        //{
        //    string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
        //    var userPreferences = File.Exists(userPreferencesFilePath)
        //        ? JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath))
        //        : new Dictionary<string, UserPreferences>();
        //
        //    userPreferences[loggedUser.Username] = new UserPreferences
        //    {
        //        IsDarkMode = ThemeManager.IsDarkMode,
        //        IsSimpleView = true, // Setting the preference for simple view
        //        WindowX = this.Location.X,
        //        WindowY = this.Location.Y,
        //        WindowWidth = this.Size.Width,
        //        WindowHeight = this.Size.Height
        //    };
        //
        //    File.WriteAllText(userPreferencesFilePath, JsonConvert.SerializeObject(userPreferences, Formatting.Indented));
        //}

        // Métodos adicionados para resolver os erros de método não encontrado
        private void ComboBoxSubCategory1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementação necessária
        }

        private void ComboBoxSubCategory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementação necessária
        }

        private void TextBoxSearch2_TextChanged(object sender, EventArgs e)
        {
            // Implementação necessária
        }

        private void pdfViewer1_Load(object sender, EventArgs e)
        {
            // Implementação necessária
        }

        // Variável para controlar o estado do ajuste
        private bool isAdjusted = false;

        // Botão da ListBox 1
        private void btnToggleListBoxes1_Click(object sender, EventArgs e)
        {

            ToggleListBoxVisibility(
                listBoxFiles1,
                pdfViewer1,
                btnToggleListBoxes1,
                posOriginalX: 238,
                posOcultoX: 12,
                larguraOriginal: 700,
                larguraOculto: 888
            );

        }

        // Botão da ListBox 2
        private void btnToggleListBoxes2_Click(object sender, EventArgs e)
        {
            ToggleListBoxVisibility(
                listBoxFiles2,
                pdfViewer2,
                btnToggleListBoxes2,
                posOriginalX: 1167,
                posOcultoX: 940,
                larguraOriginal: 700,
                larguraOculto: 888
            );

        }
        private void ToggleListBoxVisibility(
            ListBox listBox,
            PdfViewer pdfViewer,
            WinFormsButton toggleButton,
            int posOriginalX,
            int posOcultoX,
            int larguraOriginal,
            int larguraOculto)
        {
            // Verifica se a página está ajustada
            if (isAdjusted)
            {
                MessageBox.Show("Por favor, pressione 'Página Original' antes de ocultar as listas.");
                return;
            }

            
            // Atualiza SOMENTE o botão relacionado
            toggleButton.Image = listBox.Visible ? GetButtonImage("toggle_list_close") : GetButtonImage("toggle_list_open");

            // Alterna a visibilidade da ListBox
            listBox.Visible = !listBox.Visible;
            isListOpen1 = listBoxFiles1.Visible;
            isListOpen2 = listBoxFiles2.Visible;

            // Ajusta a posição e tamanho do PdfViewer
            if (listBox.Visible)
            {
                // ListBox VISÍVEL: PdfViewer volta para posição original
                pdfViewer.Location = new System.Drawing.Point(posOriginalX, 95);
                pdfViewer.Size = new System.Drawing.Size(larguraOriginal, 900);
            }
            else
            {
                // ListBox OCULTA: PdfViewer expande para a esquerda
                pdfViewer.Location = new System.Drawing.Point(posOcultoX, 95);
                pdfViewer.Size = new System.Drawing.Size(larguraOculto, 900);
            }

            // Atualiza o ícone do botão (Dark Mode ou Light Mode)
            
            UpdateToggleButtonIcon(listBox, toggleButton);
        }


        // Variáveis para armazenar os tamanhos e posições originais
        private System.Drawing.Point originalListBox1Location;
        private System.Drawing.Point originalListBox2Location;
        private System.Drawing.Size originalListBox1Size;
        private System.Drawing.Size originalListBox2Size;
        private System.Drawing.Point originalPdfViewer1Location;
        private System.Drawing.Point originalPdfViewer2Location;
        private System.Drawing.Size originalPdfViewer1Size;
        private System.Drawing.Size originalPdfViewer2Size;
        private System.Drawing.Point originalbtnToggleListBoxes1Location;
        private System.Drawing.Point originalbtnToggleListBoxes2Location;
        private System.Drawing.Point originaltoolStrip1Location;
        private System.Drawing.Point originaltoolStrip2Location;

        private void btnToggleAjust_Click(object sender, EventArgs e)
        {
            // Só permite ajustar se as duas listas estiverem visíveis!

            if (!listBoxFiles1.Visible || !listBoxFiles2.Visible)
            {
                MessageBox.Show("Abra as duas listas antes de ajustar a página.");
                return;
            }


            // Se for a primeira vez que o botão é clicado, armazene os valores originais
            if (!isAdjusted)
            {
                originalListBox1Location = listBoxFiles1.Location;
                originalListBox2Location = listBoxFiles2.Location;
                originalListBox1Size = listBoxFiles1.Size;
                originalListBox2Size = listBoxFiles2.Size;
                originalPdfViewer1Location = pdfViewer1.Location;
                originalPdfViewer2Location = pdfViewer2.Location;
                originalPdfViewer1Size = pdfViewer1.Size;
                originalPdfViewer2Size = pdfViewer2.Size;
                originalbtnToggleListBoxes1Location = btnToggleListBoxes1.Location;
                originalbtnToggleListBoxes2Location = btnToggleListBoxes2.Location;
                originaltoolStrip1Location = toolStrip1.Location;
                originaltoolStrip2Location = toolStrip2.Location;
            }

            // Alternar entre os tamanhos e posições originais e os ajustados
            if (isAdjusted)
            {
                // Voltar para as posições e tamanhos originais
                listBoxFiles1.Location = originalListBox1Location;
                listBoxFiles2.Location = originalListBox2Location;
                listBoxFiles1.Size = originalListBox1Size;
                listBoxFiles2.Size = originalListBox2Size;
                pdfViewer1.Location = originalPdfViewer1Location;
                pdfViewer2.Location = originalPdfViewer2Location;
                pdfViewer1.Size = originalPdfViewer1Size;
                pdfViewer2.Size = originalPdfViewer2Size;
                btnToggleListBoxes1.Location = originalbtnToggleListBoxes1Location;
                btnToggleListBoxes2.Location = originalbtnToggleListBoxes2Location;
                toolStrip1.Location = originaltoolStrip1Location;
                toolStrip2.Location = originaltoolStrip2Location;

            }
            else
            {
                // Modificar posição e tamanhos
                listBoxFiles1.Location = new System.Drawing.Point(11, 64);
                listBoxFiles2.Location = new System.Drawing.Point(680, 64);
                listBoxFiles1.Size = new System.Drawing.Size(150, 600);
                listBoxFiles2.Size = new System.Drawing.Size(150, 600);
                pdfViewer1.Location = new System.Drawing.Point(170, 64);
                pdfViewer2.Location = new System.Drawing.Point(840, 64);
                pdfViewer1.Size = new System.Drawing.Size(500, 600);
                pdfViewer2.Size = new System.Drawing.Size(500, 600);
                btnToggleListBoxes1.Location = new System.Drawing.Point(240, 31);
                btnToggleListBoxes2.Location = new System.Drawing.Point(1174, 31);
                toolStrip1.Location = new System.Drawing.Point(300, 68);
                toolStrip2.Location = new System.Drawing.Point(970, 68);

            }

            // Inverter o estado do ajuste
            isAdjusted = !isAdjusted;

            // Atualizar o texto do botão
            btnToggleAjust.Text = isAdjusted ? "Página Original" : "Ajustar Página";

            // Salvar preferências
            //SaveUserPreferences(false);
        }

        private void btnValidarOriginal1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath1) && File.Exists(currentFilePath1))
            {
                try
                {
                    System.Diagnostics.Process.Start(currentFilePath1);
                    string selectedFileName = listBoxFiles1.SelectedItem.ToString();
                    currentFilePath1 = GetFilePath(selectedFileName);
                    LogActivity($"Abriu o Arquivo Original {selectedFileName} na Categoria 1.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o Documento:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por Favor selecione um documento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnValidarOriginal2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath2) && File.Exists(currentFilePath2))
            {
                try
                {
                    System.Diagnostics.Process.Start(currentFilePath2);
                    string selectedFileName = listBoxFiles2.SelectedItem.ToString();
                    currentFilePath2 = GetFilePath(selectedFileName);
                    LogActivity($"Abriu o Arquivo Original {selectedFileName} na Categoria 2.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o Documento:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por Favor selecione um documento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private int currentPage1 = 0;
        private int currentPage2 = 0;

        private void btnPrev1_Click(object sender, EventArgs e) => PreviousPage1();
        private void btnNext1_Click(object sender, EventArgs e) => NextPage1();

        private void btnPrev2_Click(object sender, EventArgs e) => PreviousPage2();
        private void btnNext2_Click(object sender, EventArgs e) => NextPage2();


        private void PreviousPage1()
        {
            if (pdfViewer1.Document != null && currentPage1 > 0)
            {
                currentPage1--;
                pdfViewer1.Renderer.Page = currentPage1;
                UpdatePageUI1();
            }
        }

        private void NextPage1()
        {
            if (pdfViewer1.Document != null && currentPage1 < pdfViewer1.Document.PageCount - 1)
            {
                currentPage1++;
                pdfViewer1.Renderer.Page = currentPage1;
                UpdatePageUI1();
            }
        }

        private void PreviousPage2()
        {
            if (pdfViewer2.Document != null && currentPage2 > 0)
            {
                currentPage2--;
                pdfViewer2.Renderer.Page = currentPage2;
                UpdatePageUI2();
            }
        }

        private void NextPage2()
        {
            if (pdfViewer2.Document != null && currentPage2 < pdfViewer2.Document.PageCount - 1)
            {
                currentPage2++;
                pdfViewer2.Renderer.Page = currentPage2;
                UpdatePageUI2();
            }
        }

        private void UpdatePageUI1()
        {
            lblPage1.Text = $"Página {currentPage1 + 1} de {pdfViewer1.Document.PageCount}";
        }

        private void UpdatePageUI2()
        {
            lblPage2.Text = $"Página {currentPage2 + 1} de {pdfViewer2.Document.PageCount}";
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            Rotate1(false);
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            Rotate1(true);
        }

        private void Rotate1(bool clockwise)
        {
            if (pdfViewer1.Document == null)
                return;

            int delta = clockwise ? 90 : -90;
            rotationState1 = (rotationState1 + delta + 360) % 360;

            if (clockwise)
                pdfViewer1.Renderer.RotateRight();
            else
                pdfViewer1.Renderer.RotateLeft();

            pdfViewer1.Invalidate(); // Atualiza visual
        }

        private void btnRotateLeft1_Click(object sender, EventArgs e)
        {
            Rotate2(false);
        }

        private void btnRotateRight1_Click(object sender, EventArgs e)
        {
            Rotate2(true);
        }

        private void Rotate2(bool clockwise)
        {
            if (pdfViewer2.Document == null)
                return;

            int delta = clockwise ? 90 : -90;
            rotationState2 = (rotationState2 + delta + 360) % 360;

            if (clockwise)
                pdfViewer2.Renderer.RotateRight();
            else
                pdfViewer2.Renderer.RotateLeft();

            pdfViewer2.Invalidate(); // Atualiza visual
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
            string currentFilePath = !string.IsNullOrEmpty(currentFilePath1) ? currentFilePath1 :
                                   (!string.IsNullOrEmpty(currentFilePath2) ? currentFilePath2 : null);

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
            try
            {
                PdfReader reader = new PdfReader(filePath);
                AcroFields fields = reader.AcroFields;
                return fields.GetSignatureNames().Count > 0;
            }
            catch
            {
                return false;
            }
        }
        private void toolBtnFirstPage1_Click(object sender, EventArgs e)
        {
            toolBtnFirstPage1.Enabled = true;

            if (pdfViewer1.Document != null)
            {
                currentPage1 = 0;
                pdfViewer1.Renderer.Page = currentPage1;
                UpdatePageUI1();
            }
        }

        private void toolBtnResetRotation1_Click(object sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

            while (rotationState1 != 0)
            {
                pdfViewer1.Renderer.RotateLeft(); // desfaz 90°
                rotationState1 = (rotationState1 - 90 + 360) % 360;
            }

            pdfViewer1.Invalidate();
        }


        private void toolBtnFirstPage2_Click(object sender, EventArgs e)
        {
            if (pdfViewer2.Document != null)
            {
                currentPage2 = 0;
                pdfViewer2.Renderer.Page = currentPage2;
                UpdatePageUI2();
            }
        }

        private void toolBtnResetRotation2_Click(object sender, EventArgs e)
        {
            if (pdfViewer2.Document == null) return;

            while (rotationState2 != 0)
            {
                pdfViewer2.Renderer.RotateLeft(); // desfaz 90°
                rotationState2 = (rotationState2 - 90 + 360) % 360;
            }

            pdfViewer2.Invalidate();
        }



        private void btnVerOriginal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath1) && File.Exists(currentFilePath1))
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
                        System.Diagnostics.Process.Start(adobePath, $"\"{currentFilePath1}\"");
                        LogActivity($"Arquivo Orginal aberto: {Path.GetFileName(currentFilePath1)}");
                    }
                    else
                    {
                        // Se não for encontrado, abre com o programa padrão do sistema
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = currentFilePath1,
                            UseShellExecute = true
                        });
                        LogActivity($"Arquivo Orginal aberto: {Path.GetFileName(currentFilePath1)}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o arquivo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogActivity($"Erro ao tentar abrir o arquivo: {Path.GetFileName(currentFilePath1)} - {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Nenhum arquivo carregado ou caminho inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogActivity("Tentativa de abrir arquivo original falhou: nenhum arquivo carregado ou caminho inválido.");
            }
        }
        private void btnVerOriginal2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath2) && File.Exists(currentFilePath2))
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
                        System.Diagnostics.Process.Start(adobePath, $"\"{currentFilePath2}\"");
                        LogActivity($"Arquivo Orginal aberto: {Path.GetFileName(currentFilePath2)}");
                    }
                    else
                    {
                        // Se não for encontrado, abre com o programa padrão do sistema
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = currentFilePath2,
                            UseShellExecute = true
                        });
                        LogActivity($"Arquivo Orginal aberto: {Path.GetFileName(currentFilePath2)}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao abrir o arquivo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogActivity($"Erro ao tentar abrir o arquivo: {Path.GetFileName(currentFilePath2)} - {ex.Message}");
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

            lblPage1.Enabled = visible;
            toolBtnFirstPage1.Enabled = visible;
            btnPrev1.Enabled = visible;
            btnNext1.Enabled = visible;
            btnRotateLeft1.Enabled = visible;
            toolBtnResetRotation1.Enabled = visible;
            btnRotateRight1.Enabled = visible;
            btnFitPage1.Enabled = visible;
            lblPage2.Enabled = visible;
            toolBtnFirstPage2.Enabled = visible;
            btnPrev2.Enabled = visible;
            btnNext2.Enabled = visible;
            btnRotateLeft2.Enabled = visible;
            toolBtnResetRotation2.Enabled = visible;
            btnRotateRight2.Enabled = visible;
            btnFitPage2.Enabled = visible;

        }

        private void btnFitPage1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath1) && File.Exists(currentFilePath1))
            {
                try
                {
                    pdfViewer1.Document?.Dispose();

                    var document = PdfiumViewerPdfDocument.Load(currentFilePath1);
                    pdfViewer1.Document = document;

                    // Resetar zoom para melhor ajuste
                    pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;
                    pdfViewer1.Renderer.Zoom = 1.0f; // zoom 100%
                    pdfViewer1.Invalidate();
                    pdfViewer1.Refresh();

                    currentPage1 = 0;
                    pdfViewer1.Renderer.Page = currentPage1;
                    UpdatePageUI1();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao recarregar documento: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnFitPage2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath2) && File.Exists(currentFilePath2))
            {
                try
                {
                    pdfViewer2.Document?.Dispose();

                    var document = PdfiumViewerPdfDocument.Load(currentFilePath2);
                    pdfViewer2.Document = document;

                    // Resetar zoom para melhor ajuste
                    pdfViewer2.ZoomMode = PdfViewerZoomMode.FitBest;
                    pdfViewer2.Renderer.Zoom = 1.0f; // zoom 100%
                    pdfViewer2.Invalidate();
                    pdfViewer2.Refresh();

                    currentPage1 = 0;
                    pdfViewer2.Renderer.Page = currentPage1;
                    UpdatePageUI1();
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
        private void RealizarLogout()
        {
            SaveUserPreferences(false); // ou o que você quiser
            ActivityLogger.Log("deslogou do sistem por Inatividade", loggedUser?.Username ?? "?");
            UpdateUserLoginStatus(loggedUser.Username, false);
            UpdateUserOnlineTime(loggedUser.Username);
            CheckAndRemoveLoggedOutUsers();
            // Se quiser, fecha a aplicação
            Application.Exit();
        }
    }
}




