using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

public partial class MensagemManutencaoEditorForm : Form
{
    private string mensagensPath;

    public List<string> Mensagens { get; private set; }
    public string MensagemSelecionada => lstMensagens.SelectedItem?.ToString();

    public MensagemManutencaoEditorForm(string basePath)
    {
        InitializeComponent();
        mensagensPath = Path.Combine(basePath, "mensagens_manutencao.json");
        CarregarMensagens();
    }

    private void CarregarMensagens()
    {
        if (File.Exists(mensagensPath))
        {
            Mensagens = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(mensagensPath));
        }
        else
        {
            Mensagens = new List<string>
            {
                "Sistema em manutenção para atualização de documentos.",
                "Backup em andamento. Tente novamente em alguns minutos.",
                "Atualização urgente de segurança. Por favor, aguarde.",
                "Ambiente temporariamente indisponível.",
                "Manutenção preventiva programada.",
                "Problemas técnicos detectados, estamos trabalhando para resolver.",
                "Sistema fora do ar para upload de novos arquivos."
            };
        }
        AtualizarLista();
    }

    private void AtualizarLista()
    {
        lstMensagens.Items.Clear();
        lstMensagens.Items.AddRange(Mensagens.ToArray());
    }

    private void btnAdicionar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtNovaMensagem.Text))
        {
            Mensagens.Add(txtNovaMensagem.Text.Trim());
            txtNovaMensagem.Clear();
            AtualizarLista();
        }
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
        if (lstMensagens.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(txtNovaMensagem.Text))
        {
            Mensagens[lstMensagens.SelectedIndex] = txtNovaMensagem.Text.Trim();
            AtualizarLista();
        }
    }

    private void btnRemover_Click(object sender, EventArgs e)
    {
        if (lstMensagens.SelectedIndex >= 0)
        {
            Mensagens.RemoveAt(lstMensagens.SelectedIndex);
            AtualizarLista();
        }
    }

    private void btnSalvar_Click(object sender, EventArgs e)
    {
        File.WriteAllText(mensagensPath, JsonConvert.SerializeObject(Mensagens, Formatting.Indented));
        MessageBox.Show("Mensagens salvas com sucesso!");
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void lstMensagens_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstMensagens.SelectedIndex >= 0)
            txtNovaMensagem.Text = lstMensagens.SelectedItem.ToString();
    }
}
