using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Adiciona no topo
using NPOI.OpenXmlFormats.Wordprocessing;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
namespace DocsViewer
{
    public partial class LoginForm : Form
    {
        private Dictionary<string, bool> userLoginStatus;
        private User loggedUser;
        private string basePath => AppConfig.GetDatabasePath();
        private readonly string localAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DocsViewer");
        private bool isPasswordVisible = false;
        private System.Windows.Forms.ToolTip toolTip;

        public LoginForm()
        {
            InitializeComponent();
            try
            {
                LoadUserLoginStatus();
                LoadLastLogin();
                ThemeManager.ApplyTheme(this);
                LogoHelper.AplicarLogoComoIcon(this);
                this.ActiveControl = txtPassword;
                lblVersao.Text = ObterVersaoSistema();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar status de login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Erro ao carregar status de login: " + ex.Message);
                Application.Exit();
            }

            toolTip = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip);
            toolTip.SetToolTip(txtUsername, "Digite seu Nome de Usuário");
            toolTip.SetToolTip(txtPassword, "Digite sua Senha de Acesso");
            toolTip.SetToolTip(pictureBoxTogglePassword, "Clique para Visualizar Senha");
            toolTip.SetToolTip(btnNotasVersao, "notas da versão");

            var logoConfig = LogoConfig.LoadLogoConfig();
            if (logoConfig != null && !string.IsNullOrEmpty(logoConfig.LogoBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(logoConfig.LogoBase64);
                using (var ms = new MemoryStream(imageBytes))
                {
                    pictureBoxLogo.Image?.Dispose();
                    pictureBoxLogo.Image = new Bitmap(ms);
                }
            }
        }



        private void PictureBoxTogglePassword_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtPassword.UseSystemPasswordChar = !isPasswordVisible;
            pictureBoxTogglePassword.Image = isPasswordVisible
                ? Properties.Resources.eye_open
                : Properties.Resources.eye_closed;
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
                Logger.Log("Arquivo de status de login não encontrado. Criando novo.");
            }
        }

        private void SaveUserLoginStatus()
        {
            try
            {
                string userLoginStatusFilePath = Path.Combine(basePath, "userLoginStatus.json");
                File.WriteAllText(userLoginStatusFilePath, JsonConvert.SerializeObject(userLoginStatus, Formatting.Indented));
                Logger.Log("Status de login salvo com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar status de login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Erro ao salvar status de login: " + ex.Message);
            }
        }

        private void LoadLastLogin()
        {
            string lastLoginFilePath = Path.Combine(localAppDataPath, "lastLogin.json");
            if (File.Exists(lastLoginFilePath))
            {
                var lastLogin = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(lastLoginFilePath));
                if (lastLogin.ContainsKey("LastUsername"))
                {
                    txtUsername.Text = lastLogin["LastUsername"];
                }
            }
        }

        private void SaveLastLogin(string username)
        {
            try
            {
                if (!Directory.Exists(localAppDataPath))
                {
                    Directory.CreateDirectory(localAppDataPath);
                }
                string lastLoginFilePath = Path.Combine(localAppDataPath, "lastLogin.json");
                var lastLogin = new Dictionary<string, string>
                {
                    { "LastUsername", username }
                };
                File.WriteAllText(lastLoginFilePath, JsonConvert.SerializeObject(lastLogin, Formatting.Indented));
                Logger.Log("Último login salvo com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar último login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Erro ao salvar último login: " + ex.Message);
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

                        
            int tentativasPermitidas = 3;   // Se quiser, coloque isso no AppConfig
            int tempoBloqueioMin = 1;

            // Checa se está bloqueado antes de qualquer coisa
            if (LoginLockoutHelper.EstaBloqueado(username, out var tempoRestante))
            {
                MessageBox.Show($"Usuário bloqueado por mais {(int)tempoRestante?.TotalSeconds} segundos.", "Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string usersFilePath = Path.Combine(basePath, "users.json");
            if (!File.Exists(usersFilePath))
            {
                MessageBox.Show("Arquivo de usuários não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Arquivo de usuários não encontrado: " + usersFilePath);
                return;
            }
            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));

            // Garante dbadmin (hardcoded)
            if (!users.Any(u => u.Username == "dbadmin"))
            {
                users.Add(new User { Username = "dbadmin", Password = "dbadmin", Role = "admin", FirstLogin = true });
                File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
            }


            // Procura o usuário
            var userFound = users.FirstOrDefault(u => u.Username == username);

            if (userFound != null && userFound.Password == password)
            {
                if (userLoginStatus.ContainsKey(userFound.Username) && userLoginStatus[userFound.Username])
                {
                    MessageBox.Show("O usuário já está logado em outro terminal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Log("Tentativa de login múltiplo para o usuário: " + userFound.Username);
                    return;
                }
                string mensagem;
                if (IsInMaintenance(out mensagem, userFound.Role))
                {
                    Logger.Log($"Tentativa de login bloqueada pelo modo manutenção. Usuário: {userFound.Username} / Classe: {userFound.Role}");
                    MessageBox.Show(mensagem, "Manutenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (userFound.Role.ToLower() == "admin" && mensagem.StartsWith("[MODO MANUTENÇÃO ATIVO]"))
                {
                    Logger.Log($"Entrou no sistema durante modo manutenção. Usuário: {userFound.Username} / Classe: {userFound.Role}");
                    ActivityLogger.Log("Entrou no sistema durante modo manutenção", userFound.Username);
                    MessageBox.Show(mensagem, "Atenção (Admin)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                

                // Login correto → reseta bloqueio
                LoginLockoutHelper.ResetUser(username);
                


                // Agora sim: login válido e senha atualizada
                userLoginStatus[userFound.Username] = true;
                    SaveUserLoginStatus();

                    userFound.LastLogin = DateTime.Now;
                    SaveUserDetails(users);

                    SaveCurrentUserDetails(userFound.Username, GetLocalIPAddress(), DateTime.Now);
                    SaveLastLogin(userFound.Username);
              
                if (userFound.FirstLogin)
                {
                    MessageBox.Show("Este é seu primeiro acesso. Você deve alterar sua senha.", "Troca de Senha Obrigatória", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Chama o formulário de troca de senha obrigatória
                    using (var frmChange = new ChangePasswordForm(userFound))
                    {
                        frmChange.StartPosition = FormStartPosition.CenterScreen;
                        frmChange.ShowDialog();
                        // Recarrega os dados do usuário após a troca da senha
                        var usersReload = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
                        userFound = usersReload.FirstOrDefault(u => u.Username == userFound.Username);
                        if (userFound != null && userFound.FirstLogin)
                        {
                            MessageBox.Show("Você deve trocar a senha para continuar.", "Troca de Senha Obrigatória", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.Exit();
                            return;
                        }
                    }
                }

                VerificarAtualizacao();

                this.Hide();

                    // Lógica para abrir a tela principal (adapte ao seu sistema)
                    string userPreferencesFilePath = Path.Combine(basePath, "userPreferences.json");
                    bool isSimpleView = true;

                    if (File.Exists(userPreferencesFilePath))
                    {
                        var userPreferences = JsonConvert.DeserializeObject<Dictionary<string, UserPreferences>>(File.ReadAllText(userPreferencesFilePath));
                        if (userPreferences.ContainsKey(userFound.Username))
                        {
                            isSimpleView = userPreferences[userFound.Username].IsSimpleView;
                        }
                    }


                    Form mainForm;
                    if (isSimpleView)
                        mainForm = new DocumentViewerForm(userFound); // Visualização Simples
                    else
                        mainForm = new Viewer1(userFound);            // Visualização Dupla


                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();

                    Logger.Log($"Usuário {userFound.Username} fez login com sucesso.");
                    LogActivity($"Usuário {userFound.Username} fez login com sucesso.");
            }
            else
            {
                // Só registra tentativa se o usuário EXISTE
                if (userFound != null)
                {
                    LoginLockoutHelper.RegistrarTentativa(username, tentativasPermitidas, tempoBloqueioMin, false);

                    var info = LoginLockoutHelper.GetInfo(username);
                    if (info.BloqueadoAte.HasValue && info.BloqueadoAte > DateTime.Now)
                    {
                        Logger.Log($"Usuário '{username}' foi bloqueado por excesso de tentativas no login.");
                        MessageBox.Show("Muitas tentativas erradas! Usuário bloqueado.", "Login Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Senha inválida! ({info.Tentativas}/{tentativasPermitidas})", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Usuário não existe → só mostra mensagem genérica (sem registrar tentativas)
                    MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveCurrentUserDetails(string username, string ip, DateTime loginTime)
        {
            try
            {
                string currentUserDetailsFilePath = Path.Combine(basePath, "currentUsers.json");
                var currentUserDetails = File.Exists(currentUserDetailsFilePath)
                    ? JsonConvert.DeserializeObject<List<UserLoginDetail>>(File.ReadAllText(currentUserDetailsFilePath))
                    : new List<UserLoginDetail>();

                currentUserDetails.Add(new UserLoginDetail
                {
                    Username = username,
                    IPAddress = ip,
                    LoginTime = loginTime.ToString("dd-MM-yyyy HH:mm:ss"),
                    OnlineTime = TimeSpan.Zero.ToString(@"hh\:mm\:ss")
                });

                File.WriteAllText(currentUserDetailsFilePath, JsonConvert.SerializeObject(currentUserDetails, Formatting.Indented));
                Logger.Log("Detalhes do usuário atual salvos com sucesso para o usuário: " + username);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar detalhes do usuário atual: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Erro ao salvar detalhes do usuário atual: " + ex.Message);
            }
        }

        private void SaveUserDetails(List<User> users)
        {
            try
            {
                string usersFilePath = Path.Combine(basePath, "users.json");
                File.WriteAllText(usersFilePath, JsonConvert.SerializeObject(users, Formatting.Indented));
                Logger.Log("Detalhes do usuário salvos com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar detalhes do usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Log("Erro ao salvar detalhes do usuário: " + ex.Message);
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "Local IP Address Not Found!";
            }
            catch (Exception ex)
            {
                Logger.Log("Erro ao obter endereço IP local: " + ex.Message);
                return "Erro ao obter IP!";
            }
        }

        private void LogActivity(string activity)
        {
            try
            {
                string logFilePath = Path.Combine(basePath, "activity_log.txt");
                string logMessage = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - {GetLocalIPAddress()} - {txtUsername.Text} - {activity}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar atividade: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void btnNotasVersao_Click(object sender, EventArgs e)
        //{
        //    string notasPath = Path.Combine(AppConfig.GetDatabasePath(), "notas_versao.txt");
        //    if (!File.Exists(notasPath))
        //    {
        //        MessageBox.Show("Arquivo de notas da versão não encontrado.", "Notas da Versão", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    string notas = File.ReadAllText(notasPath);
        //    MessageBox.Show(notas, "Notas da Versão", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        private void btnNotasVersao_Click(object sender, EventArgs e)
        {
            string notasPath = Path.Combine(AppConfig.GetDatabasePath(), "notas_versao.txt");
            if (!File.Exists(notasPath))
            {
                MessageBox.Show("Arquivo de notas da versão não encontrado.", "Notas da Versão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string notas = File.ReadAllText(notasPath);
            var frm = new NotasVersaoForm(notas);
            frm.ShowDialog();
        }

        private string ObterVersaoSistema()
        {
            try
            {
                string notasPath = Path.Combine(AppConfig.GetDatabasePath(), "notas_versao.txt");
                if (!File.Exists(notasPath))
                    return "Versão indefinida";

                // Lê só a primeira linha do arquivo
                string primeiraLinha = File.ReadLines(notasPath).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(primeiraLinha))
                {
                    // Exemplo de linha: v1.0.0 (26/06/2025)
                    // Exemplo de primeira linha: v1.0.0 (26/06/2025)
                    var match = System.Text.RegularExpressions.Regex.Match(primeiraLinha, @"v([\d\.]+)\s*\(([\d\/\.]+)\)");
                    if (match.Success)
                        return $"Versão {match.Groups[1].Value}   {match.Groups[2].Value}";
                }
            }
            catch { }
            return "Versão indefinida";
        }
        private void VerificarAtualizacao()
        {
            var config = AppConfig.Load();
            string updatePath = config.UpdatePath;

            string versaoLocalPath = Path.Combine(Application.StartupPath, "versao.txt");
            string versaoServidorPath = Path.Combine(updatePath, "versao.txt");

            if (!File.Exists(versaoLocalPath) || !File.Exists(versaoServidorPath))
                return;

            string versaoLocal = File.ReadAllText(versaoLocalPath).Trim();
            string versaoServidor = File.ReadAllText(versaoServidorPath).Trim();

            if (versaoLocal != versaoServidor)
            {
                string msg = $"Uma nova versão do DocsViewer está disponível!\n\n" +
                             $"Sua versão: {versaoLocal}\n" +
                             $"Nova versão: {versaoServidor}\n\n" +
                             $"Deseja atualizar agora?";

                var result = MessageBox.Show(msg, "Atualização disponível", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    string notasPath = Path.Combine(updatePath, "notas da versão.txt");
                    if (File.Exists(notasPath))
                    {
                        string notas = File.ReadAllText(notasPath);
                        MessageBox.Show(notas, "Novidades da atualização", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    string atualizadorPath = Path.Combine(Application.StartupPath, "Atualizador.exe");
                    if (File.Exists(atualizadorPath))
                    {
                        System.Diagnostics.Process.Start(atualizadorPath);
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("O arquivo Atualizador.exe não foi encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                        mensagem = "[MODO MANUTENÇÃO ATIVO] " + msgOriginal;
                        return false; // Não bloqueia!
                    }

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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
