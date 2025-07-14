using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DocsViewer
{
    public partial class RelatorioUsuariosForm : Form
    {
        private System.Windows.Forms.ToolTip toolTip1;
        private List<User> users;
        private Dictionary<string, List<string>> userPermissions;

        public RelatorioUsuariosForm()
        {
            InitializeComponent();
            CarregarUsuarios();
            CarregarPermissoes();
            PreencherComboUsuarios();
            AtualizarGrid();
            ThemeManager.ApplyThemeToControl(this);
            LogoHelper.AplicarLogoComoIcon(this);
            UpdateAllButtonIcons();


            toolTip1 = new System.Windows.Forms.ToolTip();
            ThemeManager.ApplyToolTipTheme(toolTip1);
            // Tooltips para botões comuns
            toolTip1.SetToolTip(btnExportarExcel, "Exporta o Relatório em Planilha Excel (F2)");
            toolTip1.SetToolTip(btnExportarPdf, "Exporta o Relatório em Arquivo PDF (F3)");
            toolTip1.SetToolTip(comboUsuario, "Filtrar o Relatório por Usuário");
            
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

        private void CarregarUsuarios()
        {
            string usersFilePath = Path.Combine(AppConfig.GetDatabasePath(), "users.json");
            if (File.Exists(usersFilePath))
            {
                users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersFilePath));
            }
            else
            {
                users = new List<User>();
            }
        }

        private void CarregarPermissoes()
        {
            string permissionsFilePath = Path.Combine(AppConfig.GetDatabasePath(), "userPermissions.json");
            if (File.Exists(permissionsFilePath))
            {
                userPermissions = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(permissionsFilePath));
            }
            else
            {
                userPermissions = new Dictionary<string, List<string>>();
            }
        }

        private void PreencherComboUsuarios()
        {
            comboUsuario.Items.Clear();
            comboUsuario.Items.Add("Todos");
            foreach (var user in users)
            {
                if (user.Username != "dbadmin")
                    comboUsuario.Items.Add(user.Username);
            }
            comboUsuario.SelectedIndex = 0;
        }

        //private void AtualizarGrid()
        //{
        //    string usuarioSelecionado = comboUsuario.SelectedItem?.ToString();
        //
        //    var lista = users
        //        .Where(u => usuarioSelecionado == "Todos" || u.Username == usuarioSelecionado)
        //        .Select(u => new
        //        {
        //            Nome = u.Username,
        //            Classe = u.Role,
        //            Permissões = userPermissions.ContainsKey(u.Username) ? string.Join(", ", userPermissions[u.Username]) : "",
        //            // Adapte para mostrar subcategorias se desejar
        //        })
        //        .ToList();
        //
        //    foreach (DataGridViewColumn col in dataGridView1.Columns)
        //    {
        //        col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        //    }
        //    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        //
        //
        //    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        //    dataGridView1.DataSource = null;
        //    dataGridView1.DataSource = lista;
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        //}

        private void AtualizarGrid()
        {
            string usuarioSelecionado = comboUsuario.SelectedItem?.ToString();

            var lista = users
                .Where(u => u.Username != "dbadmin") // <--- FILTRA aqui!
                .Where(u => usuarioSelecionado == "Todos" || u.Username == usuarioSelecionado)
                .Select(u => new
                {
                    Nome = u.Username,
                    Classe = u.Role,
                    UltimoLogin = u.LastLogin == DateTime.MinValue ? "" : u.LastLogin.ToString("dd/MM/yyyy HH:mm:ss"),
                    Permissões = userPermissions.ContainsKey(u.Username) ? string.Join(", ", userPermissions[u.Username]) : "",
                })
                .ToList();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }



        private void comboUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog() { Filter = "Excel Files|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Relatório de Usuários");

                        // Cabeçalho
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            ws.Cell(1, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                            ws.Cell(1, i + 1).Style.Font.Bold = true;
                            ws.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 162, 255);
                            ws.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                        }

                        // Dados
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].IsNewRow) continue;
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                ws.Cell(i + 2, j + 1).Value = dataGridView1.Rows[i].Cells[j].Value?.ToString() ?? "";
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
                        var doc = new Document(PageSize.A4.Rotate());
                        var writer = PdfWriter.GetInstance(doc, fs);
                        doc.Open();

                        var table = new PdfPTable(dataGridView1.Columns.Count);
                        table.WidthPercentage = 100;

                        var headerFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                        var rowFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            var cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                            cell.BackgroundColor = new BaseColor(0, 162, 255);
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }
                        table.HeaderRows = 1;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;
                            foreach (DataGridViewCell gridCell in row.Cells)
                            {
                                var cell = new PdfPCell(new Phrase(gridCell.Value?.ToString() ?? "", rowFont));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(cell);
                            }
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
