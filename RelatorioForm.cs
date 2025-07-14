using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DocsViewer
{
    public partial class RelatorioForm : Form
    {
        private System.Windows.Forms.ToolTip toolTip3;
        private List<string> categorias = new List<string>();
        private List<string> subcategorias = new List<string>();
        private List<Documento> documentos = new List<Documento>(); // Adapte para sua classe real
        private string documentsPath => AppConfig.GetDocumentsPath();
        private string basePath => AppConfig.GetDatabasePath();
        private Dictionary<string, List<string>> userPermissions; // Se precisar controlar por usuário
        private User loggedUser; // Receba o usuário no construtor, igual Viewer1
        


        private bool UserCanAccessCategory(string category)
        {
            return userPermissions.ContainsKey(loggedUser.Username) && userPermissions[loggedUser.Username].Contains(category);
        }
        private bool UserCanAccessSubCategory(string category, string subCategory)
        {
            return userPermissions.ContainsKey(loggedUser.Username) && userPermissions[loggedUser.Username].Contains(subCategory);
        }
        private void RelatorioForm_Load(object sender, EventArgs e)
        {
            // Aqui você inicializa a tela, por exemplo:
            CarregarCategorias();
            CarregarDocumentos();
            AtualizarGrid();
            ThemeManager.ApplyThemeToControl(this);
            LogoHelper.AplicarLogoComoIcon(this);
            UpdateAllButtonIcons();
        }

        public RelatorioForm(User user, Dictionary<string, List<string>> userPermissions)
        {
            InitializeComponent();
            this.loggedUser = user;
            this.userPermissions = userPermissions;


            toolTip3 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip3);
            // Tooltips para botões comuns
            toolTip3.SetToolTip(btnExportarExcel, "Exporta o Relatório em Planilha Excel (F2)");
            toolTip3.SetToolTip(btnExportarPdf, "Exporta o Relatório em Arquivo PDF (F3)");
            toolTip3.SetToolTip(comboCategoria, "Filtrar Categoria de Arquivos");
            toolTip3.SetToolTip(comboSubcategoria, "Filtrar Sub Categoria de Arquivos");
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

            }
        }
                

        private void UpdateAllButtonIcons()
        {
            try
            {
                // Botões de busca
                btnExportarExcel.Image = GetButtonImage("exportexcel");
                btnExportarPdf.Image = GetButtonImage("exportpdf");
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

        private void CarregarCategorias()
        {
            comboCategoria.Items.Clear();
            comboCategoria.Items.Add("Todas");

            if (Directory.Exists(documentsPath))
            {
                var categories = Directory.GetDirectories(documentsPath).Select(Path.GetFileName);
                foreach (var category in categories)
                {
                    // Se quiser filtrar por permissão:
                    if (userPermissions == null || UserCanAccessCategory(category))
                    {
                        comboCategoria.Items.Add(category);
                    }
                }
            }
            comboCategoria.SelectedIndex = 0;
        }

        private void CarregarSubcategorias()
        {
            comboSubcategoria.Items.Clear();
            comboSubcategoria.Items.Add("Todas");
            string selectedCategory = comboCategoria.SelectedItem?.ToString();

            if (selectedCategory != "Todas" && !string.IsNullOrEmpty(selectedCategory))
            {
                var subCategoryPath = Path.Combine(documentsPath, selectedCategory);
                if (Directory.Exists(subCategoryPath))
                {
                    var subCategories = Directory.GetDirectories(subCategoryPath).Select(Path.GetFileName);
                    foreach (var subCategory in subCategories)
                    {
                        // Se quiser filtrar por permissão:
                        if (userPermissions == null || UserCanAccessSubCategory(selectedCategory, subCategory))
                        {
                            comboSubcategoria.Items.Add(subCategory);
                        }
                    }
                }
            }
            comboSubcategoria.SelectedIndex = 0;
        }


        private void CarregarDocumentos()
        {
            documentos = DocumentRepository.GetAll(); // Adapte para seu projeto!
        }

        private void comboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarSubcategorias();
            AtualizarGrid();
        }

        private void comboSubcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void AtualizarGrid()
        {
            string categoria = comboCategoria.SelectedItem?.ToString();
            string subcategoria = comboSubcategoria.SelectedItem?.ToString();

            var docs = new List<Documento>();
            string metadataDir = AppConfig.GetMetadataPath();

            if (Directory.Exists(documentsPath))
            {
                var categories = (categoria == "Todas")
                    ? Directory.GetDirectories(documentsPath).Select(Path.GetFileName)
                    : new List<string> { categoria };

                foreach (var cat in categories)
                {
                    if (cat == "Todas") continue;
                    var subCategories = (subcategoria == "Todas")
                        ? Directory.GetDirectories(Path.Combine(documentsPath, cat)).Select(Path.GetFileName)
                        : new List<string> { subcategoria };

                    foreach (var sub in subCategories)
                    {
                        if (sub == "Todas") continue;
                        var filesPath = Path.Combine(documentsPath, cat, sub);
                        if (!Directory.Exists(filesPath)) continue;

                        var files = Directory.GetFiles(filesPath, "*.pdf");
                        foreach (var file in files)
                        {
                            // Tenta buscar metadado
                            string nomeJson = Path.GetFileNameWithoutExtension(file) + ".json";
                            string jsonPath = Path.Combine(metadataDir, nomeJson);

                            Documento doc = null;
                            if (File.Exists(jsonPath))
                            {
                                try
                                {
                                    doc = JsonConvert.DeserializeObject<Documento>(File.ReadAllText(jsonPath));
                                }
                                catch
                                {
                                    // Se JSON estiver corrompido, ignora e monta básico
                                }
                            }

                            if (doc == null)
                            {
                                doc = new Documento
                                {
                                    Nome = Path.GetFileName(file),
                                    Categoria = cat,
                                    Subcategoria = sub,
                                    Data = File.GetCreationTime(file),
                                    Usuario = "", // ou "N/A"
                                    TamanhoMb = Math.Round(new FileInfo(file).Length / 1024.0 / 1024.0, 2)
                                };
                            }

                            docs.Add(doc);
                        }
                    }
                }
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = docs;

            if (dataGridView1.Columns.Contains("Data"))
                dataGridView1.Columns["Data"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (dataGridView1.Columns.Contains("TamanhoMb"))
            {
                dataGridView1.Columns["TamanhoMb"].HeaderText = "Tamanho (MB)";
                dataGridView1.Columns["TamanhoMb"].DefaultCellStyle.Format = "N2";
            }
            if (dataGridView1.Columns.Contains("Usuario"))
                dataGridView1.Columns["Usuario"].HeaderText = "Usuário";
                dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Confere se é a coluna do tamanho e o valor não é nulo
            if (dataGridView1.Columns[e.ColumnIndex].Name == "TamanhoMb" && e.Value != null)
            {
                double valor;
                if (double.TryParse(e.Value.ToString(), out valor))
                {
                    e.Value = valor.ToString("N2") + " MB";
                    e.FormattingApplied = true;
                }
            }
        }


        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog() { Filter = "Excel Files|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Relatório");

                        // Cabeçalhos
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            ws.Cell(1, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                            ws.Cell(1, i + 1).Style.Font.FontName = "Tahoma";
                            ws.Cell(1, i + 1).Style.Font.FontSize = 12;
                            ws.Cell(1, i + 1).Style.Font.Bold = true;
                            ws.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                            ws.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 162, 255);
                            ws.Cell(1, i + 1).Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                            ws.Cell(1, i + 1).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Medium;
                        }

                        // Dados
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].IsNewRow) continue;
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                var column = dataGridView1.Columns[j];
                                var valor = dataGridView1.Rows[i].Cells[j].Value;
                                string texto = valor?.ToString() ?? "";

                                if (column.Name == "TamanhoMb" && double.TryParse(texto, out double tamanho))
                                    texto = tamanho.ToString("N2") + " MB";

                                var cell = ws.Cell(i + 2, j + 1);
                                cell.Value = texto;
                                cell.Style.Font.FontName = "Tahoma";
                                cell.Style.Font.FontSize = 10;
                                cell.Style.Font.FontColor = XLColor.Black;
                                cell.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                cell.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Left;
                            }
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
                        // Página em paisagem (horizontal)
                        var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());
                        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                        doc.Open();

                        // Cria a tabela com o mesmo número de colunas do grid
                        var table = new iTextSharp.text.pdf.PdfPTable(dataGridView1.Columns.Count);
                        table.WidthPercentage = 100;

                        // Define larguras relativas (opcional, ajuste se quiser)
                        float[] widths = new float[] { 7f, 2f, 2f, 2f};
                        table.SetWidths(widths);

                        // Estilo de cabeçalho
                        var headerFont = iTextSharp.text.FontFactory.GetFont("Tahoma", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);

                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(column.HeaderText, headerFont));
                            cell.BackgroundColor = new BaseColor(0, 162, 255); 
                            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            cell.BorderWidth = 2f;
                            table.AddCell(cell);
                        }

                        // Estilo de dados
                        var rowFont = iTextSharp.text.FontFactory.GetFont("Tahoma", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;
                            foreach (DataGridViewCell gridCell in row.Cells)
                            {
                                var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(gridCell.Value?.ToString() ?? "", rowFont));
                                cell.BorderWidth = 0.8f;
                                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                                table.AddCell(cell);

                                table.HeaderRows = 1;
                            }
                        }

                        doc.Add(table);
                        doc.Close();
                    }
                    MessageBox.Show("Relatório exportado para PDF com sucesso!");
                }
            }
        }

        // Exemplo de repositório (você vai adaptar para sua fonte real de dados)
        public static class DocumentRepository
        {
            public static List<string> GetCategorias()
            {
                // Retorne lista de categorias do banco
                return new List<string> { "RH", "Financeiro", "Técnico" };
            }
            public static List<string> GetSubcategorias()
            {
                // Retorne lista de subcategorias do banco
                return new List<string> { "Admissão", "Pagamento", "Projetos" };
            }
            public static List<string> GetSubcategoriasPorCategoria(string categoria)
            {
                // Retorne subcategorias filtradas
                if (categoria == "RH") return new List<string> { "Admissão", "Demissão" };
                if (categoria == "Financeiro") return new List<string> { "Pagamento", "Contas" };
                return new List<string> { "Projetos", "Relatórios" };
            }
            public static List<Documento> GetAll()
            {
                // Retorne lista de documentos do banco
                return new List<Documento>
            {
                new Documento { Nome="Doc1", Categoria="RH", Subcategoria="Admissão", Data=DateTime.Now },
                new Documento { Nome="Doc2", Categoria="Financeiro", Subcategoria="Pagamento", Data=DateTime.Now }
            };
            }
        }
    }
}
