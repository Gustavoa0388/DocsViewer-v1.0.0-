using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace DocsViewer 
{
    public partial class ManagementForm : Form
    {
        private System.Windows.Forms.ToolTip toolTip3;
        private readonly string adminUsername;
        private string basePath => AppConfig.GetDatabasePath();
        private readonly Dictionary<string, List<string>> categoriesWithSubmenus = new Dictionary<string, List<string>>

        {
            { "Documentos Vigentes", new List<string> { "DT", "EC", "EMF", "GR", "NP", "RM", "RMP", "SF" } },
            { "Documentos Obsoletos", new List<string> { "DT", "EC", "EMF", "GR", "NP", "RM", "RMP", "SF" } },
            { "Validações", new List<string> { "Validações" } }
        };
        private Dictionary<string, List<string>> userPermissions;
        private User loggedUser;

        private readonly string loggedInUser;

        public ManagementForm(string loggedInUser)
        {
            this.loggedInUser = loggedInUser;
            InitializeComponent();
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            LogoHelper.AplicarLogoComoIcon(this);

            UpdateAllButtonIcons();

            toolTip3 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip3);

            // Tooltips para botões comuns
            toolTip3.SetToolTip(btnCreateUser, "Gerenciamento de Usuários(F2)");
            toolTip3.SetToolTip(btnOnlineUsers, "Usuários Online(F3)");
            toolTip3.SetToolTip(btnManageDocuments, "Gerenciamento de Documentos(F4)");
            //toolTip3.SetToolTip(btnGenerateLogReport, "Gera Log de Atividades do Sistema(F5)");
            toolTip3.SetToolTip(btnExportUserReport, "Gera Relatório de Usuários(F5)");
            toolTip3.SetToolTip(btnConfig, "Abre o Menu de Configurações(F6)");
            toolTip3.SetToolTip(btnDocReport, "Abre o Menu de Relatórios de Documentos (F7)");
            toolTip3.SetToolTip(btnLogReport, "Abre o Menu de Relatórios de Logs (F8)");

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
            ThemeManager.ApplyTheme(this); // Garante que o tema seja aplicado mesmo se o formulário for recriado
        }
            

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnCreateUser.Image = GetButtonImage("adduser");
                btnOnlineUsers.Image = GetButtonImage("onlineuser");
                btnManageDocuments.Image = GetButtonImage("doc_management");
                //btnGenerateLogReport.Image = GetButtonImage("log_report");
                btnExportUserReport.Image = GetButtonImage("report");
                btnConfig.Image = GetButtonImage("settings");
                btnDocReport.Image = GetButtonImage("exportreport");
                btnLogReport.Image = GetButtonImage("logreport");
                
            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao atualizar ícones: {ex.Message}");
            }
        }
        private void ManagementForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F2)
            {
                btnCreateUser.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
            }
            if (e.KeyCode == Keys.F3)
            {
                btnOnlineUsers.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
            }
            if (e.KeyCode == Keys.F4)
            {
                btnManageDocuments.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
            }
            //if (e.KeyCode == Keys.F5)
            //{
            //    btnGenerateLogReport.PerformClick(); // Simula um clique no botão
            //    e.Handled = true; // Indica que o evento foi tratado
            //}
            if (e.KeyCode == Keys.F5)
            {
                btnExportUserReport.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
            }
            if (e.KeyCode == Keys.F6)
            {
                btnConfig.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
            }
            if (e.KeyCode == Keys.F7)
            {
                btnDocReport.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
            }
            if (e.KeyCode == Keys.F7)
            {
                btnLogReport.PerformClick(); // Simula um clique no botão
                e.Handled = true; // Indica que o evento foi tratado
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


        private void BtnManageDocuments_Click(object sender, EventArgs e)
        {
            if (CurrentUserIsAdmin())
            {
                var docManagementForm = new DocumentManagementForm(AppConfig.GetDocumentsPath(), loggedInUser, categoriesWithSubmenus);

                docManagementForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Acesso negado. Apenas administradores podem acessar esta funcionalidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CurrentUserIsAdmin()
        {
            // Implementar a lógica para verificar se o usuário atual é administrador
            // Por exemplo, verificando um campo na classe de usuário atual
            return true; // Substituir pela lógica real
        }

        private void BtnCreateUser_Click(object sender, EventArgs e)
        {
            CreateUserForm createUserForm = new CreateUserForm(loggedInUser);
            if (createUserForm.ShowDialog() == DialogResult.OK)
            {
                ActivityLogger.Log("Criou um novo usuário.", loggedInUser);
            }
        }

        private void BtnGenerateLogReport_Click(object sender, EventArgs e)
        {
            GenerateLogReport();
        }

        private void GenerateLogReport()
        {
            try
            {
                // Abrir dialog para salvar o relatório
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string logReportPath = Path.Combine(folderBrowserDialog.SelectedPath, "log_report.txt");
                    string logFilePath = Path.Combine(basePath, "activity_log.txt");
                    string logContent = File.ReadAllText(logFilePath);
                    File.WriteAllText(logReportPath, logContent);
                    MessageBox.Show($"Relatório de logs gerado em: {logReportPath}");
                    ActivityLogger.Log("Gerou um relatório de logs.", loggedInUser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório de logs: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOnlineUsers_Click(object sender, EventArgs e)
        {
            var onlineUsersForm = new OnlineUsersForm(loggedInUser); // <-- Troque adminUsername por loggedInUser
            onlineUsersForm.ShowDialog();
            
        }


        //private void BtnExportUserReport_Click(object sender, EventArgs e)
        //{
        //    ExportUserReport();
        //}
        //
        //private void ExportUserReport()
        //{
        //    try
        //    {
        //        string usersFilePath = Path.Combine(basePath, "users.json");
        //        string userPermissionsFilePath = Path.Combine(basePath, "userPermissions.json");
        //
        //        var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
        //        var userPermissions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(userPermissionsFilePath));
        //
        //        // Abrir dialog para salvar o relatório
        //        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            string reportPath = Path.Combine(folderBrowserDialog.SelectedPath, "user_report.xls");
        //            IWorkbook workbook = new HSSFWorkbook();
        //            ISheet sheet = workbook.CreateSheet("User Report");
        //
        //            // Cabeçalho
        //            IRow headerRow = sheet.CreateRow(0);
        //            headerRow.CreateCell(0).SetCellValue("Username");
        //            headerRow.CreateCell(1).SetCellValue("Password");
        //            headerRow.CreateCell(2).SetCellValue("Role");
        //            headerRow.CreateCell(3).SetCellValue("Permissions");
        //
        //            // Dados dos usuários
        //            for (int i = 0; i < users.Count; i++)
        //            {
        //                var user = users[i];
        //                IRow row = sheet.CreateRow(i + 1);
        //                row.CreateCell(0).SetCellValue(user.Username);
        //                row.CreateCell(1).SetCellValue(user.Password);
        //                row.CreateCell(2).SetCellValue(user.Role);
        //                string permissions = string.Join(", ", userPermissions.ContainsKey(user.Username) ? userPermissions[user.Username] : new List<string>());
        //                row.CreateCell(3).SetCellValue(permissions);
        //            }
        //
        //            // Salvar o arquivo
        //            using (var fileData = new FileStream(reportPath, FileMode.Create))
        //            {
        //                workbook.Write(fileData);
        //            }
        //
        //            MessageBox.Show($"Relatório de usuários exportado em: {reportPath}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            ActivityLogger.Log("Exportou um relatório de usuários.", loggedInUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Erro ao exportar relatório de usuários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void BtnExportUserReport_Click(object sender, EventArgs e)
        {
            var relatoriousuarioform = new RelatorioUsuariosForm();
            relatoriousuarioform.ShowDialog();
        }
        

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        private void btnDocReport_Click(object sender, EventArgs e)
        {
            var relatorioForm = new RelatorioForm(loggedUser, userPermissions);
            relatorioForm.ShowDialog();
        }
             
       
        private void btnLogReport_Click(object sender, EventArgs e)
        {
            RelatorioLogForm relatorioLog = new RelatorioLogForm();
            relatorioLog.ShowDialog();
        }
    }
}
    
