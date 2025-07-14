partial class MensagemManutencaoEditorForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.ListBox lstMensagens;
    private System.Windows.Forms.TextBox txtNovaMensagem;
    private System.Windows.Forms.Button btnAdicionar;
    private System.Windows.Forms.Button btnEditar;
    private System.Windows.Forms.Button btnRemover;
    private System.Windows.Forms.Button btnSalvar;
    private System.Windows.Forms.Label lblLista;
    private System.Windows.Forms.Label lblEditar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lstMensagens = new System.Windows.Forms.ListBox();
        this.txtNovaMensagem = new System.Windows.Forms.TextBox();
        this.btnAdicionar = new System.Windows.Forms.Button();
        this.btnEditar = new System.Windows.Forms.Button();
        this.btnRemover = new System.Windows.Forms.Button();
        this.btnSalvar = new System.Windows.Forms.Button();
        this.lblLista = new System.Windows.Forms.Label();
        this.lblEditar = new System.Windows.Forms.Label();
        this.SuspendLayout();

        // 
        // lstMensagens
        // 
        this.lstMensagens.FormattingEnabled = true;
        this.lstMensagens.Location = new System.Drawing.Point(12, 29);
        this.lstMensagens.Size = new System.Drawing.Size(340, 108);
        this.lstMensagens.TabIndex = 0;
        this.lstMensagens.SelectedIndexChanged += new System.EventHandler(this.lstMensagens_SelectedIndexChanged);
        // 
        // txtNovaMensagem
        // 
        this.txtNovaMensagem.Location = new System.Drawing.Point(12, 165);
        this.txtNovaMensagem.Multiline = true;
        this.txtNovaMensagem.Size = new System.Drawing.Size(340, 50);
        this.txtNovaMensagem.TabIndex = 1;
        // 
        // btnAdicionar
        // 
        this.btnAdicionar.Location = new System.Drawing.Point(12, 225);
        this.btnAdicionar.Size = new System.Drawing.Size(75, 30);
        this.btnAdicionar.Text = "Adicionar";
        this.btnAdicionar.TabIndex = 2;
        this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
        // 
        // btnEditar
        // 
        this.btnEditar.Location = new System.Drawing.Point(97, 225);
        this.btnEditar.Size = new System.Drawing.Size(75, 30);
        this.btnEditar.Text = "Editar";
        this.btnEditar.TabIndex = 3;
        this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
        // 
        // btnRemover
        // 
        this.btnRemover.Location = new System.Drawing.Point(182, 225);
        this.btnRemover.Size = new System.Drawing.Size(75, 30);
        this.btnRemover.Text = "Remover";
        this.btnRemover.TabIndex = 4;
        this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
        // 
        // btnSalvar
        // 
        this.btnSalvar.Location = new System.Drawing.Point(277, 225);
        this.btnSalvar.Size = new System.Drawing.Size(75, 30);
        this.btnSalvar.Text = "Salvar";
        this.btnSalvar.TabIndex = 5;
        this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
        // 
        // lblLista
        // 
        this.lblLista.Location = new System.Drawing.Point(12, 9);
        this.lblLista.Size = new System.Drawing.Size(340, 17);
        this.lblLista.Text = "Mensagens Cadastradas:";
        // 
        // lblEditar
        // 
        this.lblEditar.Location = new System.Drawing.Point(12, 145);
        this.lblEditar.Size = new System.Drawing.Size(340, 17);
        this.lblEditar.Text = "Editar/Adicionar Mensagem:";
        // 
        // MensagemManutencaoEditorForm
        // 
        this.ClientSize = new System.Drawing.Size(364, 271);
        this.Controls.Add(this.lstMensagens);
        this.Controls.Add(this.txtNovaMensagem);
        this.Controls.Add(this.btnAdicionar);
        this.Controls.Add(this.btnEditar);
        this.Controls.Add(this.btnRemover);
        this.Controls.Add(this.btnSalvar);
        this.Controls.Add(this.lblLista);
        this.Controls.Add(this.lblEditar);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Editor de Mensagens de Manutenção";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
