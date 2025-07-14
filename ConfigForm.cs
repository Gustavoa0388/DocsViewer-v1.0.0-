using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq; 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO; // <--- ESSA LINHA!
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DocsViewer 
{
    public partial class ConfigForm : Form
    {
        private System.Windows.Forms.ToolTip toolTip1;
        private readonly string loggedInUser;
        public ConfigForm()   
      {        
        
            
            InitializeComponent();
            LoadConfig();
            ThemeManager.ApplyTheme(this);
            ThemeManager.ApplyToolTipTheme(toolTip1);
            UpdateAllButtonIcons();
            LogoHelper.AplicarLogoComoIcon(this);
            AtualizarLabelEspacoDiscoViaArquivo();
            CarregarManutencao();


            toolTip1 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip1);
            // Tooltips para botões comuns
            toolTip1.SetToolTip(btnSalvar, "Salva os Caminhos Selecionado(F2)");
            toolTip1.SetToolTip(btnProcurarBanco, "Procura o Caminho do Banco de Dados(F3)");
            toolTip1.SetToolTip(btnProcurarDocs, "Procura o Caminho do Banco de Documentos(F4)");
            toolTip1.SetToolTip(btnProcurarMetadados, "Procura o Caminho do Banco de Documentos(F5)");
            toolTip1.SetToolTip(btnTrocarLogo, "Procura o Caminho do Banco de Documentos(F6)");
            toolTip1.SetToolTip(btnSalvarManutencao, "Salva as configurações de Manutenção do Sistema(F7)");

            // Preenche comboResetSenhaUsuario com todos os usuários
            comboResetSenhaUsuario.Items.Clear();
            string usersFilePath = Path.Combine(AppConfig.GetDatabasePath(), "users.json");
            if (File.Exists(usersFilePath))
            {
                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
                foreach (var user in users.Select(u => u.Username).OrderBy(u => u))
                    comboResetSenhaUsuario.Items.Add(user);
            }
            if (comboResetSenhaUsuario.Items.Count > 0)
                comboResetSenhaUsuario.SelectedIndex = 0;


            var mensagens = CarregarMensagens();
            cmbMensagensManutencao.Items.Clear();
            cmbMensagensManutencao.Items.AddRange(mensagens.ToArray());

            if (cmbMensagensManutencao.Items.Count > 0)
                cmbMensagensManutencao.SelectedIndex = 0;
        }
        private void cmbMensagensManutencao_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMensagemManutencao.Text = cmbMensagensManutencao.SelectedItem?.ToString();
        }
        private string GetMensagensFilePath()
        {
            return Path.Combine(AppConfig.GetDatabasePath(), "mensagens_manutencao.json");
        }

        private List<string> CarregarMensagens()
        {
            var file = GetMensagensFilePath();
            if (File.Exists(file))
            {
                return JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(file));
            }
            else
            {
                // Mensagens padrão se arquivo não existe
                return new List<string>
        {
            "Sistema em manutenção para atualização de documentos.",
            "Backup em andamento. Tente novamente em alguns minutos.",
            "Atualização urgente de segurança. Por favor, aguarde."
        };
            }
        }


        private string GetManutencaoFilePath()
        {
            return Path.Combine(AppConfig.GetDatabasePath(), "manutencao.json");
        }

        private void CarregarManutencao()
        {
            string file = GetManutencaoFilePath();
            if (File.Exists(file))
            {
                var json = JObject.Parse(File.ReadAllText(file));
                chkManutencaoAtiva.Checked = json["status"]?.Value<bool>() ?? false;
                txtMensagemManutencao.Text = json["mensagem"]?.ToString() ?? "";
            }
            else
            {
                chkManutencaoAtiva.Checked = false;
                txtMensagemManutencao.Text = "";
            }
        }
        
        private void btnSalvarManutencao_Click(object sender, EventArgs e)
        {
            var json = new JObject
            {
                ["status"] = chkManutencaoAtiva.Checked,
                ["mensagem"] = txtMensagemManutencao.Text
            };

            File.WriteAllText(GetManutencaoFilePath(), json.ToString());
            // === REGISTRO NO LOG DE ATIVIDADE ===
            string statusStr = chkManutencaoAtiva.Checked ? "ATIVOU" : "DESATIVOU";
            string username = loggedInUser ?? "Desconhecido";
            ActivityLogger.Log($"[{statusStr} o modo de manutenção] Mensagem: \"{txtMensagemManutencao.Text}\"", username);

            MessageBox.Show("Status de manutenção salvo com sucesso!", "Manutenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnSalvar.Image = GetButtonImage("save");
                btnProcurarBanco.Image = GetButtonImage("search");
                btnProcurarDocs.Image = GetButtonImage("search");
                btnProcurarMetadados.Image = GetButtonImage("search");
                btnTrocarLogo.Image = GetButtonImage("change_logo");
                btnResetarUsuario.Image = GetButtonImage("unlockuser");
                btnResetarSenha.Image = GetButtonImage("passwordreset");
                btnSalvarManutencao.Image = GetButtonImage("save");
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


        private void ManagementForm_KeyDown(object sender, KeyEventArgs e)
        {
            {
                if (e.KeyCode == Keys.F2)
                {
                    btnSalvar.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F3)
                {
                    btnProcurarBanco.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F4)
                {
                    btnProcurarDocs.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F5)
                {
                    btnProcurarBanco.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F6)
                {
                    btnTrocarLogo.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F7)
                {
                    btnSalvarManutencao.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }

            }
        }

        private void LoadConfig()
        {
            var config = AppConfig.Load();
            txtDatabasePath.Text = config.DatabasePath;
            txtDocumentsPath.Text = config.DocumentsPath;
            txtMetadataPath.Text = config.MetadataPath;
            numTentativas.Value = config.LoginLockoutTentativas;
            numMinutos.Value = config.LoginLockoutTempo;
           

            // Preenche os usuários bloqueados para resetar
            comboResetUsuario.Items.Clear();
            var bloqueios = LoginLockoutHelper.GetAllLockedUsers();
            foreach (var user in bloqueios)
                comboResetUsuario.Items.Add(user);
            if (comboResetUsuario.Items.Count > 0)
                comboResetUsuario.SelectedIndex = 0;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // VALIDAÇÃO: obriga preencher os campos!
            {
                if (string.IsNullOrWhiteSpace(txtDatabasePath.Text) ||
                    string.IsNullOrWhiteSpace(txtDocumentsPath.Text) ||
                    string.IsNullOrWhiteSpace(txtMetadataPath.Text))
                {
                    MessageBox.Show("Os caminhos do Banco de Dados, Documentos e Metadados são obrigatórios!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var config = new AppConfig
                {
                    DatabasePath = txtDatabasePath.Text,
                    DocumentsPath = txtDocumentsPath.Text,
                    MetadataPath = txtMetadataPath.Text,
                    LoginLockoutTentativas = (int)numTentativas.Value,
                    LoginLockoutTempo = (int)numMinutos.Value

                };


                // Validação básica
                if (!Directory.Exists(txtDatabasePath.Text))
                {
                    MessageBox.Show("Caminho do banco inválido!");
                    return;
                }
                if (!Directory.Exists(txtDocumentsPath.Text))
                {
                    MessageBox.Show("Caminho dos documentos inválido!");
                    return;
                }
                if (!Directory.Exists(txtMetadataPath.Text))
                {
                    MessageBox.Show("Caminho dos metadados inválido!");
                    return;
                }

                config.Save();
                MessageBox.Show("Caminhos salvos com sucesso!", "Configuração", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnProcurarBanco_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selecione a pasta do banco de dados";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtDatabasePath.Text = dialog.SelectedPath;
                }
            }
        }

        private void AtualizarLabelEspacoDiscoViaArquivo()
        {
            try
            {
                // Busca o caminho do banco de dados do AppConfig
                string txtPath = Path.Combine(AppConfig.GetDatabasePath(), "espaco_disco.txt");
                if (File.Exists(txtPath))
                {
                    string texto = File.ReadAllText(txtPath, Encoding.UTF8).Trim();
                    lblEspacoDisco.Text = texto;

                }
                else
                {
                    lblEspacoDisco.Text = "Arquivo de espaço em disco não encontrado.";
                }
            }
            catch (Exception ex)
            {
                lblEspacoDisco.Text = "Erro ao ler espaço em disco: " + ex.Message;
            }
        }
        private void txtDatabasePath_TextChanged(object sender, EventArgs e)
        {
            AtualizarLabelEspacoDiscoViaArquivo();
        }

        private void txtDocumentsPath_TextChanged(object sender, EventArgs e)
        {
            AtualizarLabelEspacoDiscoViaArquivo();
        }

        private void btnProcurarDocs_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selecione a pasta dos documentos";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtDocumentsPath.Text = dialog.SelectedPath;
                }
            }
        }

        
        private void btnTrocarLogo_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Imagens|*.png;*.jpg;*.jpeg;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Salva a imagem como Base64 no JSON no DatabasePath
                    LogoConfig.SaveLogo(dialog.FileName);

                    // Atualiza o PictureBox com a nova imagem
                    var logoConfig = LogoConfig.LoadLogoConfig();
                    if (logoConfig != null && !string.IsNullOrEmpty(logoConfig.LogoBase64))
                    {
                        byte[] imageBytes = Convert.FromBase64String(logoConfig.LogoBase64);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            picLogo.Image?.Dispose();
                            picLogo.Image = new Bitmap(ms);
                        }
                    }

                    MessageBox.Show("Logo trocada com sucesso!");
                }
            }
        }
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            var config = AppConfig.Load();
            if (!string.IsNullOrEmpty(config.LogoPath) && File.Exists(config.LogoPath))
                picLogo.Image = Image.FromFile(config.LogoPath);
        }

        private void btnProcurarMetadados_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selecione a pasta dos metadados";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtMetadataPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnResetarUsuario_Click(object sender, EventArgs e)
        {
            if (comboResetUsuario.SelectedItem != null)
            {
                string user = comboResetUsuario.SelectedItem.ToString();

                if (user == "dbadmin")
                {
                    MessageBox.Show("O usuário 'dbadmin' não pode ser resetado.");
                    return;
                }

                LoginLockoutHelper.ResetUser(user);

                // Log da ação
                ActivityLogger.Log($"Bloqueio do usuário '{user}' foi liberado pelo usuário '{loggedInUser}'.", loggedInUser);

                MessageBox.Show($"Bloqueio do usuário '{user}' foi liberado!");
                comboResetUsuario.Items.Remove(user);
            }
        }


        private void btnResetarSenha_Click(object sender, EventArgs e)
        {
            if (comboResetSenhaUsuario.SelectedItem != null)
            {
                string user = comboResetSenhaUsuario.SelectedItem.ToString();

                if (user == "dbadmin")
                {
                    MessageBox.Show("A senha do usuário 'dbadmin' não pode ser resetada.");
                    return;
                }

                string usersFilePath = Path.Combine(AppConfig.GetDatabasePath(), "users.json");
                if (!File.Exists(usersFilePath))
                {
                    MessageBox.Show("Arquivo de usuários não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
                var u = users.FirstOrDefault(x => x.Username == user);

                if (u == null)
                {
                    MessageBox.Show("Usuário não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                u.Password = "123"; // senha padrão
                u.FirstLogin = true;

                File.WriteAllText(usersFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented));

                // Log da ação
                ActivityLogger.Log($"Senha do usuário '{user}' foi resetada para senha padrão pelo usuário '{loggedInUser}'.", loggedInUser);

                MessageBox.Show($"Senha do usuário '{user}' foi resetada para senha padrão. Ele será obrigado a trocar a senha no próximo acesso.");
            }
        }

        private void btnEditarMensagens_Click(object sender, EventArgs e)
        {
            var form = new MensagemManutencaoEditorForm(AppConfig.GetDatabasePath());
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Recarrega mensagens após salvar
                CarregarMensagens(); // método seu que atualiza o ComboBox
            }
        }
    }
}
