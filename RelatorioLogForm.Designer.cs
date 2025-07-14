namespace DocsViewer
{
    partial class RelatorioLogForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboUsuario;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblData;
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
            this.comboUsuario = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.comboMes = new System.Windows.Forms.ComboBox();
            this.comboDia = new System.Windows.Forms.ComboBox();
            this.comboAno = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFiltrarLog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboUsuario
            // 
            this.comboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUsuario.FormattingEnabled = true;
            this.comboUsuario.Location = new System.Drawing.Point(74, 46);
            this.comboUsuario.Name = "comboUsuario";
            this.comboUsuario.Size = new System.Drawing.Size(180, 21);
            this.comboUsuario.TabIndex = 0;
            this.comboUsuario.SelectedIndexChanged += new System.EventHandler(this.FiltrarLogs);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(22, 49);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "Usuário:";
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(22, 14);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(26, 13);
            this.lblData.TabIndex = 3;
            this.lblData.Text = "Dia:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(25, 84);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1143, 661);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportarExcel.Location = new System.Drawing.Point(917, 24);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(120, 38);
            this.btnExportarExcel.TabIndex = 5;
            this.btnExportarExcel.Text = "Exportar Excel";
            this.btnExportarExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportarExcel.UseVisualStyleBackColor = true;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // btnExportarPdf
            // 
            this.btnExportarPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportarPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportarPdf.Location = new System.Drawing.Point(1048, 24);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(120, 38);
            this.btnExportarPdf.TabIndex = 6;
            this.btnExportarPdf.Text = "Exportar PDF";
            this.btnExportarPdf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportarPdf.UseVisualStyleBackColor = true;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // comboMes
            // 
            this.comboMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMes.FormattingEnabled = true;
            this.comboMes.Location = new System.Drawing.Point(210, 10);
            this.comboMes.Name = "comboMes";
            this.comboMes.Size = new System.Drawing.Size(110, 21);
            this.comboMes.TabIndex = 7;
            // 
            // comboDia
            // 
            this.comboDia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDia.FormattingEnabled = true;
            this.comboDia.Location = new System.Drawing.Point(56, 10);
            this.comboDia.Name = "comboDia";
            this.comboDia.Size = new System.Drawing.Size(110, 21);
            this.comboDia.TabIndex = 8;
            // 
            // comboAno
            // 
            this.comboAno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAno.FormattingEnabled = true;
            this.comboAno.Location = new System.Drawing.Point(365, 10);
            this.comboAno.Name = "comboAno";
            this.comboAno.Size = new System.Drawing.Size(110, 21);
            this.comboAno.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(174, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Mês:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(330, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ano:";
            // 
            // btnFiltrarLog
            // 
            this.btnFiltrarLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFiltrarLog.Location = new System.Drawing.Point(269, 36);
            this.btnFiltrarLog.Name = "btnFiltrarLog";
            this.btnFiltrarLog.Size = new System.Drawing.Size(90, 38);
            this.btnFiltrarLog.TabIndex = 10;
            this.btnFiltrarLog.Text = "Filtrar Log";
            this.btnFiltrarLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFiltrarLog.UseVisualStyleBackColor = true;
            // 
            // RelatorioLogForm
            // 
            this.ClientSize = new System.Drawing.Size(1180, 757);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFiltrarLog);
            this.Controls.Add(this.comboAno);
            this.Controls.Add(this.comboDia);
            this.Controls.Add(this.comboMes);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.comboUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Name = "RelatorioLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório de Atividades (Log)";
            this.Load += new System.EventHandler(this.RelatorioLogForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RelatorioForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboMes;
        private System.Windows.Forms.ComboBox comboDia;
        private System.Windows.Forms.ComboBox comboAno;
        private System.Windows.Forms.Button btnFiltrarLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
