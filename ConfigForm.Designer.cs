using System.Windows.Forms;
using NPOI.OpenXmlFormats.Wordprocessing;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace DocsViewer 
{
    partial class ConfigForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos em uso.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer do Windows Forms

        private void InitializeComponent()
        {
            this.btnSalvar = new System.Windows.Forms.Button();
            this.lblTentativas = new System.Windows.Forms.Label();
            this.numTentativas = new System.Windows.Forms.NumericUpDown();
            this.lblMinutos = new System.Windows.Forms.Label();
            this.numMinutos = new System.Windows.Forms.NumericUpDown();
            this.lblResetarUsuario = new System.Windows.Forms.Label();
            this.comboResetUsuario = new System.Windows.Forms.ComboBox();
            this.btnResetarUsuario = new System.Windows.Forms.Button();
            this.lblResetarSenha = new System.Windows.Forms.Label();
            this.comboResetSenhaUsuario = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblResetBloqUsuario = new System.Windows.Forms.Label();
            this.lblEspacoDisco = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblResetSenha = new System.Windows.Forms.Label();
            this.btnResetarSenha = new System.Windows.Forms.Button();
            this.lblDatabasePath = new System.Windows.Forms.Label();
            this.txtDatabasePath = new System.Windows.Forms.TextBox();
            this.btnProcurarBanco = new System.Windows.Forms.Button();
            this.lblDocumentsPath = new System.Windows.Forms.Label();
            this.txtDocumentsPath = new System.Windows.Forms.TextBox();
            this.btnProcurarDocs = new System.Windows.Forms.Button();
            this.btnTrocarLogo = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblMetadadosPath = new System.Windows.Forms.Label();
            this.txtMetadataPath = new System.Windows.Forms.TextBox();
            this.btnProcurarMetadados = new System.Windows.Forms.Button();
            this.lblConfigBanco = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnEditarMensagens = new System.Windows.Forms.Button();
            this.cmbMensagensManutencao = new System.Windows.Forms.ComboBox();
            this.btnSalvarManutencao = new System.Windows.Forms.Button();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.chkManutencaoAtiva = new System.Windows.Forms.CheckBox();
            this.lblManutencao = new System.Windows.Forms.Label();
            this.txtMensagemManutencao = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numTentativas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinutos)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSalvar
            // 
            this.btnSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalvar.Location = new System.Drawing.Point(19, 223);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(80, 38);
            this.btnSalvar.TabIndex = 6;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // lblTentativas
            // 
            this.lblTentativas.AutoSize = true;
            this.lblTentativas.Location = new System.Drawing.Point(36, 44);
            this.lblTentativas.Name = "lblTentativas";
            this.lblTentativas.Size = new System.Drawing.Size(147, 13);
            this.lblTentativas.TabIndex = 20;
            this.lblTentativas.Text = "Tentativas antes do bloqueio:";
            // 
            // numTentativas
            // 
            this.numTentativas.Location = new System.Drawing.Point(187, 40);
            this.numTentativas.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTentativas.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTentativas.Name = "numTentativas";
            this.numTentativas.Size = new System.Drawing.Size(50, 20);
            this.numTentativas.TabIndex = 21;
            this.numTentativas.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblMinutos
            // 
            this.lblMinutos.AutoSize = true;
            this.lblMinutos.Location = new System.Drawing.Point(36, 68);
            this.lblMinutos.Name = "lblMinutos";
            this.lblMinutos.Size = new System.Drawing.Size(126, 13);
            this.lblMinutos.TabIndex = 22;
            this.lblMinutos.Text = "Tempo de bloqueio (min):";
            // 
            // numMinutos
            // 
            this.numMinutos.Location = new System.Drawing.Point(187, 66);
            this.numMinutos.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numMinutos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinutos.Name = "numMinutos";
            this.numMinutos.Size = new System.Drawing.Size(50, 20);
            this.numMinutos.TabIndex = 23;
            this.numMinutos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblResetarUsuario
            // 
            this.lblResetarUsuario.AutoSize = true;
            this.lblResetarUsuario.Location = new System.Drawing.Point(36, 91);
            this.lblResetarUsuario.Name = "lblResetarUsuario";
            this.lblResetarUsuario.Size = new System.Drawing.Size(142, 13);
            this.lblResetarUsuario.TabIndex = 24;
            this.lblResetarUsuario.Text = "Resetar bloqueio do usuário:";
            // 
            // comboResetUsuario
            // 
            this.comboResetUsuario.Location = new System.Drawing.Point(36, 107);
            this.comboResetUsuario.Name = "comboResetUsuario";
            this.comboResetUsuario.Size = new System.Drawing.Size(180, 21);
            this.comboResetUsuario.TabIndex = 25;
            // 
            // btnResetarUsuario
            // 
            this.btnResetarUsuario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetarUsuario.Location = new System.Drawing.Point(93, 151);
            this.btnResetarUsuario.Name = "btnResetarUsuario";
            this.btnResetarUsuario.Size = new System.Drawing.Size(120, 38);
            this.btnResetarUsuario.TabIndex = 26;
            this.btnResetarUsuario.Text = "Liberar Usuário";
            this.btnResetarUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnResetarUsuario.UseVisualStyleBackColor = true;
            this.btnResetarUsuario.Click += new System.EventHandler(this.btnResetarUsuario_Click);
            // 
            // lblResetarSenha
            // 
            this.lblResetarSenha.AutoSize = true;
            this.lblResetarSenha.Location = new System.Drawing.Point(27, 43);
            this.lblResetarSenha.Name = "lblResetarSenha";
            this.lblResetarSenha.Size = new System.Drawing.Size(131, 13);
            this.lblResetarSenha.TabIndex = 30;
            this.lblResetarSenha.Text = "Resetar senha do usuário:";
            // 
            // comboResetSenhaUsuario
            // 
            this.comboResetSenhaUsuario.Location = new System.Drawing.Point(27, 59);
            this.comboResetSenhaUsuario.Name = "comboResetSenhaUsuario";
            this.comboResetSenhaUsuario.Size = new System.Drawing.Size(180, 21);
            this.comboResetSenhaUsuario.TabIndex = 31;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblResetBloqUsuario);
            this.panel1.Controls.Add(this.lblTentativas);
            this.panel1.Controls.Add(this.numTentativas);
            this.panel1.Controls.Add(this.lblMinutos);
            this.panel1.Controls.Add(this.numMinutos);
            this.panel1.Controls.Add(this.lblResetarUsuario);
            this.panel1.Controls.Add(this.comboResetUsuario);
            this.panel1.Controls.Add(this.btnResetarUsuario);
            this.panel1.Location = new System.Drawing.Point(12, 288);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 221);
            this.panel1.TabIndex = 33;
            // 
            // lblResetBloqUsuario
            // 
            this.lblResetBloqUsuario.AutoSize = true;
            this.lblResetBloqUsuario.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResetBloqUsuario.Location = new System.Drawing.Point(34, 10);
            this.lblResetBloqUsuario.Name = "lblResetBloqUsuario";
            this.lblResetBloqUsuario.Size = new System.Drawing.Size(222, 18);
            this.lblResetBloqUsuario.TabIndex = 34;
            this.lblResetBloqUsuario.Text = "DESBLOQUEAR  USUÁRIO";
            this.lblResetBloqUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEspacoDisco
            // 
            this.lblEspacoDisco.AutoSize = true;
            this.lblEspacoDisco.Font = new System.Drawing.Font("Consolas", 8F);
            this.lblEspacoDisco.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblEspacoDisco.Location = new System.Drawing.Point(119, 209);
            this.lblEspacoDisco.Name = "lblEspacoDisco";
            this.lblEspacoDisco.Size = new System.Drawing.Size(0, 13);
            this.lblEspacoDisco.TabIndex = 36;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lblResetSenha);
            this.panel2.Controls.Add(this.btnResetarSenha);
            this.panel2.Controls.Add(this.lblResetarSenha);
            this.panel2.Controls.Add(this.comboResetSenhaUsuario);
            this.panel2.Location = new System.Drawing.Point(12, 515);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(307, 145);
            this.panel2.TabIndex = 34;
            // 
            // lblResetSenha
            // 
            this.lblResetSenha.AutoSize = true;
            this.lblResetSenha.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResetSenha.Location = new System.Drawing.Point(27, 14);
            this.lblResetSenha.Name = "lblResetSenha";
            this.lblResetSenha.Size = new System.Drawing.Size(259, 18);
            this.lblResetSenha.TabIndex = 33;
            this.lblResetSenha.Text = "RESETAR SENHA DO USUÁRIO";
            this.lblResetSenha.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnResetarSenha
            // 
            this.btnResetarSenha.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetarSenha.Location = new System.Drawing.Point(93, 96);
            this.btnResetarSenha.Name = "btnResetarSenha";
            this.btnResetarSenha.Size = new System.Drawing.Size(120, 38);
            this.btnResetarSenha.TabIndex = 32;
            this.btnResetarSenha.Text = "Resetar Senha";
            this.btnResetarSenha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnResetarSenha.UseVisualStyleBackColor = true;
            this.btnResetarSenha.Click += new System.EventHandler(this.btnResetarSenha_Click);
            // 
            // lblDatabasePath
            // 
            this.lblDatabasePath.AutoSize = true;
            this.lblDatabasePath.Location = new System.Drawing.Point(16, 26);
            this.lblDatabasePath.Name = "lblDatabasePath";
            this.lblDatabasePath.Size = new System.Drawing.Size(100, 13);
            this.lblDatabasePath.TabIndex = 0;
            this.lblDatabasePath.Text = "Caminho do Banco:";
            // 
            // txtDatabasePath
            // 
            this.txtDatabasePath.Location = new System.Drawing.Point(102, 50);
            this.txtDatabasePath.Multiline = true;
            this.txtDatabasePath.Name = "txtDatabasePath";
            this.txtDatabasePath.Size = new System.Drawing.Size(348, 22);
            this.txtDatabasePath.TabIndex = 1;
            this.txtDatabasePath.TextChanged += new System.EventHandler(this.txtDatabasePath_TextChanged);
            // 
            // btnProcurarBanco
            // 
            this.btnProcurarBanco.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcurarBanco.Location = new System.Drawing.Point(15, 42);
            this.btnProcurarBanco.Name = "btnProcurarBanco";
            this.btnProcurarBanco.Size = new System.Drawing.Size(80, 38);
            this.btnProcurarBanco.TabIndex = 2;
            this.btnProcurarBanco.Text = "Procurar";
            this.btnProcurarBanco.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProcurarBanco.UseVisualStyleBackColor = true;
            this.btnProcurarBanco.Click += new System.EventHandler(this.btnProcurarBanco_Click);
            // 
            // lblDocumentsPath
            // 
            this.lblDocumentsPath.AutoSize = true;
            this.lblDocumentsPath.Location = new System.Drawing.Point(16, 89);
            this.lblDocumentsPath.Name = "lblDocumentsPath";
            this.lblDocumentsPath.Size = new System.Drawing.Size(134, 13);
            this.lblDocumentsPath.TabIndex = 3;
            this.lblDocumentsPath.Text = "Caminho dos Documentos:";
            // 
            // txtDocumentsPath
            // 
            this.txtDocumentsPath.Location = new System.Drawing.Point(102, 113);
            this.txtDocumentsPath.Multiline = true;
            this.txtDocumentsPath.Name = "txtDocumentsPath";
            this.txtDocumentsPath.Size = new System.Drawing.Size(348, 22);
            this.txtDocumentsPath.TabIndex = 4;
            this.txtDocumentsPath.TextChanged += new System.EventHandler(this.txtDocumentsPath_TextChanged);
            // 
            // btnProcurarDocs
            // 
            this.btnProcurarDocs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcurarDocs.Location = new System.Drawing.Point(16, 105);
            this.btnProcurarDocs.Name = "btnProcurarDocs";
            this.btnProcurarDocs.Size = new System.Drawing.Size(80, 38);
            this.btnProcurarDocs.TabIndex = 5;
            this.btnProcurarDocs.Text = "Procurar";
            this.btnProcurarDocs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProcurarDocs.UseVisualStyleBackColor = true;
            this.btnProcurarDocs.Click += new System.EventHandler(this.btnProcurarDocs_Click);
            // 
            // btnTrocarLogo
            // 
            this.btnTrocarLogo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTrocarLogo.Location = new System.Drawing.Point(487, 206);
            this.btnTrocarLogo.Name = "btnTrocarLogo";
            this.btnTrocarLogo.Size = new System.Drawing.Size(110, 38);
            this.btnTrocarLogo.TabIndex = 0;
            this.btnTrocarLogo.Text = "Trocar Logo";
            this.btnTrocarLogo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTrocarLogo.Click += new System.EventHandler(this.btnTrocarLogo_Click);
            // 
            // picLogo
            // 
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLogo.Location = new System.Drawing.Point(459, 43);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(156, 150);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 7;
            this.picLogo.TabStop = false;
            // 
            // lblMetadadosPath
            // 
            this.lblMetadadosPath.AutoSize = true;
            this.lblMetadadosPath.Location = new System.Drawing.Point(19, 154);
            this.lblMetadadosPath.Name = "lblMetadadosPath";
            this.lblMetadadosPath.Size = new System.Drawing.Size(124, 13);
            this.lblMetadadosPath.TabIndex = 8;
            this.lblMetadadosPath.Text = "Caminho dos Metadados";
            // 
            // txtMetadataPath
            // 
            this.txtMetadataPath.Location = new System.Drawing.Point(105, 178);
            this.txtMetadataPath.Multiline = true;
            this.txtMetadataPath.Name = "txtMetadataPath";
            this.txtMetadataPath.Size = new System.Drawing.Size(348, 22);
            this.txtMetadataPath.TabIndex = 9;
            // 
            // btnProcurarMetadados
            // 
            this.btnProcurarMetadados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcurarMetadados.Location = new System.Drawing.Point(19, 170);
            this.btnProcurarMetadados.Name = "btnProcurarMetadados";
            this.btnProcurarMetadados.Size = new System.Drawing.Size(80, 38);
            this.btnProcurarMetadados.TabIndex = 10;
            this.btnProcurarMetadados.Text = "Procurar";
            this.btnProcurarMetadados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProcurarMetadados.UseVisualStyleBackColor = true;
            this.btnProcurarMetadados.Click += new System.EventHandler(this.btnProcurarMetadados_Click);
            // 
            // lblConfigBanco
            // 
            this.lblConfigBanco.AutoSize = true;
            this.lblConfigBanco.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigBanco.Location = new System.Drawing.Point(170, 5);
            this.lblConfigBanco.Name = "lblConfigBanco";
            this.lblConfigBanco.Size = new System.Drawing.Size(334, 18);
            this.lblConfigBanco.TabIndex = 35;
            this.lblConfigBanco.Text = "CONFIGURAÇÕES  GERAIS DO SISTEMA";
            this.lblConfigBanco.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.lblConfigBanco);
            this.panel3.Controls.Add(this.btnProcurarMetadados);
            this.panel3.Controls.Add(this.txtMetadataPath);
            this.panel3.Controls.Add(this.btnSalvar);
            this.panel3.Controls.Add(this.lblEspacoDisco);
            this.panel3.Controls.Add(this.lblMetadadosPath);
            this.panel3.Controls.Add(this.picLogo);
            this.panel3.Controls.Add(this.btnTrocarLogo);
            this.panel3.Controls.Add(this.btnProcurarDocs);
            this.panel3.Controls.Add(this.txtDocumentsPath);
            this.panel3.Controls.Add(this.lblDocumentsPath);
            this.panel3.Controls.Add(this.btnProcurarBanco);
            this.panel3.Controls.Add(this.txtDatabasePath);
            this.panel3.Controls.Add(this.lblDatabasePath);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(630, 267);
            this.panel3.TabIndex = 35;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.txtMensagemManutencao);
            this.panel4.Controls.Add(this.btnEditarMensagens);
            this.panel4.Controls.Add(this.cmbMensagensManutencao);
            this.panel4.Controls.Add(this.btnSalvarManutencao);
            this.panel4.Controls.Add(this.lblMensagem);
            this.panel4.Controls.Add(this.chkManutencaoAtiva);
            this.panel4.Controls.Add(this.lblManutencao);
            this.panel4.Location = new System.Drawing.Point(337, 288);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(307, 273);
            this.panel4.TabIndex = 36;
            // 
            // btnEditarMensagens
            // 
            this.btnEditarMensagens.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditarMensagens.Location = new System.Drawing.Point(91, 86);
            this.btnEditarMensagens.Name = "btnEditarMensagens";
            this.btnEditarMensagens.Size = new System.Drawing.Size(130, 38);
            this.btnEditarMensagens.TabIndex = 39;
            this.btnEditarMensagens.Text = "Editar Mensagens";
            this.btnEditarMensagens.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEditarMensagens.UseVisualStyleBackColor = true;
            this.btnEditarMensagens.Click += new System.EventHandler(this.btnEditarMensagens_Click);
            // 
            // cmbMensagensManutencao
            // 
            this.cmbMensagensManutencao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMensagensManutencao.FormattingEnabled = true;
            this.cmbMensagensManutencao.Location = new System.Drawing.Point(16, 55);
            this.cmbMensagensManutencao.Name = "cmbMensagensManutencao";
            this.cmbMensagensManutencao.Size = new System.Drawing.Size(270, 21);
            this.cmbMensagensManutencao.TabIndex = 38;
            // 
            // btnSalvarManutencao
            // 
            this.btnSalvarManutencao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalvarManutencao.Location = new System.Drawing.Point(111, 217);
            this.btnSalvarManutencao.Name = "btnSalvarManutencao";
            this.btnSalvarManutencao.Size = new System.Drawing.Size(80, 38);
            this.btnSalvarManutencao.TabIndex = 35;
            this.btnSalvarManutencao.Text = "Salvar";
            this.btnSalvarManutencao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalvarManutencao.UseVisualStyleBackColor = true;
            this.btnSalvarManutencao.Click += new System.EventHandler(this.btnSalvarManutencao_Click);
            // 
            // lblMensagem
            // 
            this.lblMensagem.AutoSize = true;
            this.lblMensagem.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.Location = new System.Drawing.Point(70, 37);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(151, 15);
            this.lblMensagem.TabIndex = 37;
            this.lblMensagem.Text = "MENSAGEM DE AVISO";
            this.lblMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkManutencaoAtiva
            // 
            this.chkManutencaoAtiva.AutoSize = true;
            this.chkManutencaoAtiva.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkManutencaoAtiva.Location = new System.Drawing.Point(16, 187);
            this.chkManutencaoAtiva.Name = "chkManutencaoAtiva";
            this.chkManutencaoAtiva.Size = new System.Drawing.Size(167, 20);
            this.chkManutencaoAtiva.TabIndex = 35;
            this.chkManutencaoAtiva.Text = "Sistema em Manutenção";
            this.chkManutencaoAtiva.UseVisualStyleBackColor = true;
            // 
            // lblManutencao
            // 
            this.lblManutencao.AutoSize = true;
            this.lblManutencao.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManutencao.Location = new System.Drawing.Point(41, 9);
            this.lblManutencao.Name = "lblManutencao";
            this.lblManutencao.Size = new System.Drawing.Size(233, 18);
            this.lblManutencao.TabIndex = 34;
            this.lblManutencao.Text = "MANUTENÇÃO DO SISTEMA";
            this.lblManutencao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMensagemManutencao
            // 
            this.txtMensagemManutencao.Location = new System.Drawing.Point(16, 130);
            this.txtMensagemManutencao.Multiline = true;
            this.txtMensagemManutencao.Name = "txtMensagemManutencao";
            this.txtMensagemManutencao.Size = new System.Drawing.Size(270, 51);
            this.txtMensagemManutencao.TabIndex = 40;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 668);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuração do Sistema";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManagementForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numTentativas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinutos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Label lblTentativas;
        private System.Windows.Forms.NumericUpDown numTentativas;
        private System.Windows.Forms.Label lblMinutos;
        private System.Windows.Forms.NumericUpDown numMinutos;
        private System.Windows.Forms.Label lblResetarUsuario;
        private System.Windows.Forms.ComboBox comboResetUsuario;
        private System.Windows.Forms.Button btnResetarUsuario;
        private System.Windows.Forms.Button btnResetarSenha;
        private System.Windows.Forms.Label lblResetarSenhaUsuario;
        private System.Windows.Forms.ComboBox comboResetSenhaUsuario;
        private System.Windows.Forms.Label lblResetarSenha;
        private System.Windows.Forms.Label lblEspacoDisco;
        private Panel panel1;
        private Panel panel2;
        private Label lblResetSenha;
        private Label lblResetBloqUsuario;
        private Label lblDatabasePath;
        private TextBox txtDatabasePath;
        private Button btnProcurarBanco;
        private Label lblDocumentsPath;
        private TextBox txtDocumentsPath;
        private Button btnProcurarDocs;
        private Button btnTrocarLogo;
        private PictureBox picLogo;
        private Label lblMetadadosPath;
        private TextBox txtMetadataPath;
        private Button btnProcurarMetadados;
        private Label lblConfigBanco;        
        private Panel panel3;
        private Panel panel4;
        private CheckBox chkManutencaoAtiva;
        private Label lblManutencao;
        private Button btnSalvarManutencao;
        private Label lblMensagem;
        private ComboBox cmbMensagensManutencao;
        private Button btnEditarMensagens;
        private TextBox txtMensagemManutencao;
    }
}
