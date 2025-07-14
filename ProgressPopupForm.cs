using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DocsViewer 
{
    public partial class ProgressPopupForm : Form
    {
        public ProgressPopupForm()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this); // Adicione esta linha
            LogoHelper.AplicarLogoComoIcon(this);
        }

        public void UpdateProgress(int value, int max, string message)
        {
            progressBar.Minimum = 0; // sempre inicializa o mínimo
            progressBar.Maximum = Math.Max(1, max); // nunca deixa o máximo menor que 1

            // Garante que value está no intervalo permitido
            value = Math.Min(Math.Max(value, progressBar.Minimum), progressBar.Maximum);

            progressBar.Value = value;
            labelStatus.Text = message;
            labelStatus.Refresh();
        }
    }
}
