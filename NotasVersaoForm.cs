using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DocsViewer
{
    public partial class NotasVersaoForm : Form
    {
        public NotasVersaoForm(string notasTexto)
        {
            InitializeComponent();
            txtNotas.Text = notasTexto;
            LogoHelper.AplicarLogoComoIcon(this);
            txtNotas.SelectionLength = 0;         // Remove qualquer seleção
            txtNotas.SelectionStart = txtNotas.TextLength;  // Coloca o cursor no final do texto
            txtNotas.DeselectAll();               // Garante que nada fique selecionado

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
    }
}

