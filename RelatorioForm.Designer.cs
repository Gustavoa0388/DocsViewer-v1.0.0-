using System.Windows.Forms;
using System.Drawing;

namespace DocsViewer
{
    partial class RelatorioForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboCategoria;
        private System.Windows.Forms.ComboBox comboSubcategoria;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.Label lblSubcategoria;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnExportarPdf;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboCategoria = new System.Windows.Forms.ComboBox();
            this.comboSubcategoria = new System.Windows.Forms.ComboBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.lblSubcategoria = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboCategoria
            // 
            this.comboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCategoria.FormattingEnabled = true;
            this.comboCategoria.Location = new System.Drawing.Point(90, 18);
            this.comboCategoria.Name = "comboCategoria";
            this.comboCategoria.Size = new System.Drawing.Size(180, 21);
            this.comboCategoria.TabIndex = 0;
            this.comboCategoria.UseWaitCursor = true;
            this.comboCategoria.SelectedIndexChanged += new System.EventHandler(this.comboCategoria_SelectedIndexChanged);
            // 
            // comboSubcategoria
            // 
            this.comboSubcategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSubcategoria.FormattingEnabled = true;
            this.comboSubcategoria.Location = new System.Drawing.Point(400, 18);
            this.comboSubcategoria.Name = "comboSubcategoria";
            this.comboSubcategoria.Size = new System.Drawing.Size(180, 21);
            this.comboSubcategoria.TabIndex = 1;
            this.comboSubcategoria.UseWaitCursor = true;
            this.comboSubcategoria.SelectedIndexChanged += new System.EventHandler(this.comboSubcategoria_SelectedIndexChanged);
            // 
            // lblCategoria
            // 
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(20, 21);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(55, 13);
            this.lblCategoria.TabIndex = 2;
            this.lblCategoria.Text = "Categoria:";
            this.lblCategoria.UseWaitCursor = true;
            // 
            // lblSubcategoria
            // 
            this.lblSubcategoria.AutoSize = true;
            this.lblSubcategoria.Location = new System.Drawing.Point(310, 21);
            this.lblSubcategoria.Name = "lblSubcategoria";
            this.lblSubcategoria.Size = new System.Drawing.Size(73, 13);
            this.lblSubcategoria.TabIndex = 3;
            this.lblSubcategoria.Text = "Subcategoria:";
            this.lblSubcategoria.UseWaitCursor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.LightGray;
            this.dataGridView1.Location = new System.Drawing.Point(23, 65);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1066, 715);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnExportarPdf
            // 
            this.btnExportarPdf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportarPdf.Location = new System.Drawing.Point(962, 12);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(120, 38);
            this.btnExportarPdf.TabIndex = 6;
            this.btnExportarPdf.Text = "Exportar PDF";
            this.btnExportarPdf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportarPdf.UseVisualStyleBackColor = true;
            this.btnExportarPdf.UseWaitCursor = true;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportarExcel.Location = new System.Drawing.Point(835, 12);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(120, 38);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Exportar Excel";
            this.btnExportarExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportarExcel.UseVisualStyleBackColor = true;
            this.btnExportarExcel.UseWaitCursor = true;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // RelatorioForm
            // 
            this.ClientSize = new System.Drawing.Size(1101, 792);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblSubcategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.comboSubcategoria);
            this.Controls.Add(this.comboCategoria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RelatorioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório de Documentos";
            this.Load += new System.EventHandler(this.RelatorioForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RelatorioForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
