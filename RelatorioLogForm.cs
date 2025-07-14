using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DocsViewer
{
    public partial class RelatorioLogForm : Form
    {
        private List<LogEntry> logs = new List<LogEntry>();
        private List<LogEntry> TodosOsLogs = new List<LogEntry>();
        private System.Windows.Forms.ToolTip toolTip3;
        private User loggedUser; // Receba o usuário no construtor, igual Viewer1

        public RelatorioLogForm()
        {
            InitializeComponent();
            ThemeManager.ApplyThemeToControl(this);
            LogoHelper.AplicarLogoComoIcon(this);
            UpdateAllButtonIcons();
            comboMes.SelectedIndexChanged += (s, e) => AtualizarDiasDoMes();
            comboAno.SelectedIndexChanged += (s, e) => AtualizarDiasDoMes();
            btnFiltrarLog.Click += FiltrarLogs;
        }

        private void RelatorioLogForm_Load(object sender, EventArgs e)
        {
            CarregarLogs();
            PreencherFiltrosLogs(logs);
            PreencherUsuarios(logs); // <<--- ADICIONE AQUI
            var hoje = DateTime.Now.Date;
            var logsDoDia = logs.Where(l => l.DataHora.Date == hoje).ToList();
            dataGridView1.DataSource = logsDoDia;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Atualiza comboAno, comboMes e comboDia para o dia atual:
            if (comboAno.Items.Contains(hoje.Year))
                comboAno.SelectedItem = hoje.Year;
            if (comboMes.Items.Contains(hoje.Month.ToString("D2")))
                comboMes.SelectedItem = hoje.Month.ToString("D2");
            if (comboDia.Items.Contains(hoje.Day.ToString("D2")))
                comboDia.SelectedItem = hoje.Day.ToString("D2");


            toolTip3 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip3);
            // Tooltips para botões comuns
            toolTip3.SetToolTip(btnExportarExcel, "Exporta o Relatório em Planilha Excel (F2)");
            toolTip3.SetToolTip(btnExportarPdf, "Exporta o Relatório em Arquivo PDF (F3)");
            toolTip3.SetToolTip(comboUsuario, "Filtra o Log por Usuário ou IP da Máquina");
            toolTip3.SetToolTip(comboDia, "Filtra o Log por Dia");
            toolTip3.SetToolTip(comboMes, "Filtra o Log por Mês");
            toolTip3.SetToolTip(comboAno, "Filtra o Log por Ano");
            toolTip3.SetToolTip(btnFiltrarLog, "Executa o Filtro do Log");
           

         
        }

  

        private void RelatorioForm_KeyDown(object sender, KeyEventArgs e)
        {

            {
                if (e.KeyCode == Keys.F2)
                {
                    btnExportarExcel.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F3)
                {
                    btnExportarPdf.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }
                if (e.KeyCode == Keys.F4)
                {
                    btnExportarPdf.PerformClick(); // Simula um clique no botão
                    e.Handled = true; // Indica que o evento foi tratado
                }

            }
        }

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnExportarExcel.Image = GetButtonImage("exportexcel");
                btnExportarPdf.Image = GetButtonImage("exportpdf");
                btnFiltrarLog.Image = GetButtonImage("filtro");
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
        private void PreencherUsuarios(List<LogEntry> logs)
        {
            comboUsuario.Items.Clear();
            comboUsuario.Items.Add("Todos");
            foreach (var user in logs.Select(l => l.Usuario).Distinct().OrderBy(u => u))
                comboUsuario.Items.Add(user);
            comboUsuario.SelectedIndex = 0; // Seleciona "Todos" por padrão
        }

        private void PreencherFiltrosLogs(List<LogEntry> logs)
        {
            // ANO
            var anos = logs.Select(l => l.DataHora.Year)
               .Distinct()
               .OrderBy(x => x)
               .ToArray();
            comboAno.Items.Clear();
            comboAno.Items.Add("Todos");
            foreach (var ano in anos)
                comboAno.Items.Add(ano);
            comboAno.SelectedIndex = 0;

            // MES
            comboMes.Items.Clear();
            comboMes.Items.Add("Todos");
            for (int m = 1; m <= 12; m++)
                comboMes.Items.Add(m.ToString("D2"));
            comboMes.SelectedIndex = 0;

            // DIA
            AtualizarDiasDoMes();
        }

        private void AtualizarDiasDoMes()
        {
            // Pegue o ano/mês selecionados para definir os dias possíveis
            int ano = (comboAno.SelectedIndex > 0 && comboAno.SelectedItem != null) ? Convert.ToInt32(comboAno.SelectedItem) : DateTime.Now.Year;
            int mes = (comboMes.SelectedIndex > 0 && comboMes.SelectedItem != null) ? Convert.ToInt32(comboMes.SelectedItem) : DateTime.Now.Month;
            int dias = DateTime.DaysInMonth(ano, mes);

            comboDia.Items.Clear();
            comboDia.Items.Add("Todos");
            for (int d = 1; d <= dias; d++)
                comboDia.Items.Add(d.ToString("D2"));
            comboDia.SelectedIndex = 0;
        }


        private void CarregarLogs()
        {
            string logFilePath = Path.Combine(AppConfig.GetDatabasePath(), "activity_log.txt");
            logs = new List<LogEntry>();

            if (File.Exists(logFilePath))
            {
                foreach (var linha in File.ReadAllLines(logFilePath))
                {
                    // Exemplo de linha: "15-06-2024 11:22:12 - 192.168.0.10 - maria - Documento adicionado: Doc1.pdf"
                    var partes = linha.Split(new[] { " - " }, StringSplitOptions.None);
                    if (partes.Length >= 4 && DateTime.TryParse(partes[0], out DateTime data))
                    {
                        logs.Add(new LogEntry
                        {
                            DataHora = data,
                            IP = partes[1],
                            Usuario = partes[2],
                            Atividade = partes[3]
                        });
                    }
                }
            }
        }

        private void FiltrarLogs(object sender, EventArgs e)
        {
            string usuarioFiltro = comboUsuario.SelectedItem?.ToString();
            string anoStr = comboAno.SelectedItem?.ToString();
            string mesStr = comboMes.SelectedItem?.ToString();
            string diaStr = comboDia.SelectedItem?.ToString();

            var filtrados = logs;

            // Filtro por usuário (se não for "Todos")
            if (!string.IsNullOrEmpty(usuarioFiltro) && usuarioFiltro != "Todos")
                filtrados = filtrados.Where(l => l.Usuario == usuarioFiltro).ToList();

            // Filtro por ano
            if (!string.IsNullOrEmpty(anoStr) && anoStr != "Todos" && int.TryParse(anoStr, out int ano))
                filtrados = filtrados.Where(l => l.DataHora.Year == ano).ToList();

            // Filtro por mês
            if (!string.IsNullOrEmpty(mesStr) && mesStr != "Todos" && int.TryParse(mesStr, out int mes))
                filtrados = filtrados.Where(l => l.DataHora.Month == mes).ToList();

            // Filtro por dia
            if (!string.IsNullOrEmpty(diaStr) && diaStr != "Todos" && int.TryParse(diaStr, out int dia))
                filtrados = filtrados.Where(l => l.DataHora.Day == dia).ToList();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = filtrados;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog() { Filter = "Excel Files|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Logs");

                        // Cabeçalhos
                        ws.Cell(1, 1).Value = "Data/Hora";
                        ws.Cell(1, 2).Value = "IP";
                        ws.Cell(1, 3).Value = "Usuário";
                        ws.Cell(1, 4).Value = "Atividade";

                         

                        ws.Range("A1:D1").Style.Font.Bold = true;
                        ws.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.LightBlue;

                        // Linhas
                        var logs = (List<LogEntry>)dataGridView1.DataSource;
                        for (int i = 0; i < logs.Count; i++)
                        {
                            ws.Cell(i + 2, 1).Value = logs[i].DataHora.ToString("dd/MM/yyyy HH:mm:ss");
                            ws.Cell(i + 2, 2).Value = logs[i].IP;
                            ws.Cell(i + 2, 3).Value = logs[i].Usuario;
                            ws.Cell(i + 2, 4).Value = logs[i].Atividade;
                        }

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(dialog.FileName);
                    }
                    MessageBox.Show("Relatório exportado para Excel com sucesso!");
                }
            }
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog() { Filter = "PDF Files|*.pdf" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());
                        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                        doc.Open();

                        var table = new iTextSharp.text.pdf.PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 3f, 2f, 2f, 7f });

                        var headerFont = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
                        var rowFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                        // Cabeçalho
                        string[] headers = { "Data/Hora", "IP", "Usuário", "Atividade" };
                        foreach (var header in headers)
                        {
                            var phrase = new iTextSharp.text.Phrase(header, headerFont); // <--- ORDEM CORRETA!
                            var cell = new iTextSharp.text.pdf.PdfPCell(phrase);
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(0, 162, 255);
                            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            cell.BorderWidth = 1f;
                            table.AddCell(cell);
                        }
                        table.HeaderRows = 1; // Faz o cabeçalho repetir

                        // Linhas de dados (pegando do DataGridView)
                        var logs = (List<LogEntry>)dataGridView1.DataSource;
                        foreach (var log in logs)
                        {
                            table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(log.DataHora.ToString("dd/MM/yyyy HH:mm:ss"), rowFont)));
                            table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(log.IP, rowFont)));
                            table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(log.Usuario, rowFont)));
                            table.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(log.Atividade, rowFont)));
                        }

                        doc.Add(table);
                        doc.Close();
                    }
                    MessageBox.Show("Relatório exportado para PDF com sucesso!");
                }
            }
        }

    }
}
