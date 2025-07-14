using System.Windows.Forms;
using System;

namespace DocsViewer 
{
    partial class DocumentManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.Button btnSaveFiles;
        private System.Windows.Forms.Button btnDeleteFiles;
        private System.Windows.Forms.Button btnClearFiles;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.ComboBox cmbSubCategory;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox comboBoxCategorySearch;
        private System.Windows.Forms.ComboBox comboBoxSubCategorySearch;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Button btnRenameFile1;
        private System.Windows.Forms.Button btnMoveFile;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem categoriasMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criarCategoriaMenuItem;
        

        //private System.Windows.Forms.Button btnUpdateCategories;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentManagementForm));
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.btnSaveFiles = new System.Windows.Forms.Button();
            this.btnDeleteFiles = new System.Windows.Forms.Button();
            this.btnClearFiles = new System.Windows.Forms.Button();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.cmbSubCategory = new System.Windows.Forms.ComboBox();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.comboBoxCategorySearch = new System.Windows.Forms.ComboBox();
            this.comboBoxSubCategorySearch = new System.Windows.Forms.ComboBox();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRenameFile1 = new System.Windows.Forms.Button();
            this.btnMoveFile = new System.Windows.Forms.Button();
            this.pdfViewer1 = new PdfiumViewer.PdfViewer();
            this.pdfViewer2 = new PdfiumViewer.PdfViewer();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.categoriasMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criarCategoriaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLoadFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadFiles.Location = new System.Drawing.Point(14, 81);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(125, 38);
            this.btnLoadFiles.TabIndex = 0;
            this.btnLoadFiles.Text = "Carregar Arquivos";
            this.btnLoadFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.BtnLoadFiles_Click);
            // 
            // btnSaveFiles
            // 
            this.btnSaveFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveFiles.Location = new System.Drawing.Point(52, 789);
            this.btnSaveFiles.Name = "btnSaveFiles";
            this.btnSaveFiles.Size = new System.Drawing.Size(80, 38);
            this.btnSaveFiles.TabIndex = 4;
            this.btnSaveFiles.Text = "Salvar";
            this.btnSaveFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveFiles.UseVisualStyleBackColor = true;
            this.btnSaveFiles.Click += new System.EventHandler(this.BtnSaveFiles_Click);
            // 
            // btnDeleteFiles
            // 
            this.btnDeleteFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteFiles.Location = new System.Drawing.Point(908, 791);
            this.btnDeleteFiles.Name = "btnDeleteFiles";
            this.btnDeleteFiles.Size = new System.Drawing.Size(80, 38);
            this.btnDeleteFiles.TabIndex = 5;
            this.btnDeleteFiles.Text = "Excluir ";
            this.btnDeleteFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteFiles.UseVisualStyleBackColor = true;
            this.btnDeleteFiles.Click += new System.EventHandler(this.BtnDeleteFiles_Click);
            // 
            // btnClearFiles
            // 
            this.btnClearFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearFiles.Location = new System.Drawing.Point(136, 789);
            this.btnClearFiles.Name = "btnClearFiles";
            this.btnClearFiles.Size = new System.Drawing.Size(110, 38);
            this.btnClearFiles.TabIndex = 7;
            this.btnClearFiles.Text = "Limpar Lista";
            this.btnClearFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearFiles.UseVisualStyleBackColor = true;
            this.btnClearFiles.Click += new System.EventHandler(this.BtnClearFiles_Click);
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(161, 75);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(150, 21);
            this.cmbCategory.TabIndex = 1;
            // 
            // cmbSubCategory
            // 
            this.cmbSubCategory.Location = new System.Drawing.Point(161, 102);
            this.cmbSubCategory.Name = "cmbSubCategory";
            this.cmbSubCategory.Size = new System.Drawing.Size(150, 21);
            this.cmbSubCategory.TabIndex = 2;
            // 
            // lstFiles
            // 
            this.lstFiles.AllowDrop = true;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.Location = new System.Drawing.Point(14, 138);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(297, 641);
            this.lstFiles.TabIndex = 3;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(976, 126);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(211, 20);
            this.textBoxSearch.TabIndex = 8;
            // 
            // btnSearch
            // 
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(890, 120);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 32);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Pesquisar";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // comboBoxCategorySearch
            // 
            this.comboBoxCategorySearch.FormattingEnabled = true;
            this.comboBoxCategorySearch.Location = new System.Drawing.Point(890, 70);
            this.comboBoxCategorySearch.Name = "comboBoxCategorySearch";
            this.comboBoxCategorySearch.Size = new System.Drawing.Size(297, 21);
            this.comboBoxCategorySearch.TabIndex = 10;
            // 
            // comboBoxSubCategorySearch
            // 
            this.comboBoxSubCategorySearch.FormattingEnabled = true;
            this.comboBoxSubCategorySearch.Location = new System.Drawing.Point(890, 96);
            this.comboBoxSubCategorySearch.Name = "comboBoxSubCategorySearch";
            this.comboBoxSubCategorySearch.Size = new System.Drawing.Size(297, 21);
            this.comboBoxSubCategorySearch.TabIndex = 11;
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.HorizontalScrollbar = true;
            this.listBoxFiles.Location = new System.Drawing.Point(890, 157);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxFiles.Size = new System.Drawing.Size(297, 628);
            this.listBoxFiles.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(218, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 26);
            this.label1.TabIndex = 22;
            this.label1.Text = "Carregamento de Novos Arquivos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(1141, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(314, 26);
            this.label2.TabIndex = 23;
            this.label2.Text = "Gerenciamento de Arquivos";
            // 
            // btnRenameFile1
            // 
            this.btnRenameFile1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenameFile1.Location = new System.Drawing.Point(1074, 791);
            this.btnRenameFile1.Name = "btnRenameFile1";
            this.btnRenameFile1.Size = new System.Drawing.Size(90, 38);
            this.btnRenameFile1.TabIndex = 26;
            this.btnRenameFile1.Text = "Renomear";
            this.btnRenameFile1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRenameFile1.UseVisualStyleBackColor = true;
            this.btnRenameFile1.Click += new System.EventHandler(this.btnRenameFile1_Click);
            // 
            // btnMoveFile
            // 
            this.btnMoveFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoveFile.Location = new System.Drawing.Point(991, 791);
            this.btnMoveFile.Name = "btnMoveFile";
            this.btnMoveFile.Size = new System.Drawing.Size(80, 38);
            this.btnMoveFile.TabIndex = 27;
            this.btnMoveFile.Text = "Mover ";
            this.btnMoveFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMoveFile.UseVisualStyleBackColor = true;
            this.btnMoveFile.Click += new System.EventHandler(this.btnMoveFile_Click);
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.AutoSize = true;
            this.pdfViewer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pdfViewer1.Location = new System.Drawing.Point(332, 75);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.Size = new System.Drawing.Size(536, 708);
            this.pdfViewer1.TabIndex = 28;
            // 
            // pdfViewer2
            // 
            this.pdfViewer2.AutoSize = true;
            this.pdfViewer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pdfViewer2.Location = new System.Drawing.Point(1218, 69);
            this.pdfViewer2.Name = "pdfViewer2";
            this.pdfViewer2.Size = new System.Drawing.Size(536, 716);
            this.pdfViewer2.TabIndex = 29;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.categoriasMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1900, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // categoriasMenuItem
            // 
            this.categoriasMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.criarCategoriaMenuItem});
            this.categoriasMenuItem.Name = "categoriasMenuItem";
            this.categoriasMenuItem.Size = new System.Drawing.Size(75, 20);
            this.categoriasMenuItem.Text = "&Categorias";
            // 
            // criarCategoriaMenuItem
            // 
            this.criarCategoriaMenuItem.Name = "criarCategoriaMenuItem";
            this.criarCategoriaMenuItem.Size = new System.Drawing.Size(234, 22);
            this.criarCategoriaMenuItem.Text = "Criar Ca&tegoria e Subcategoria";
            this.criarCategoriaMenuItem.Click += new System.EventHandler(this.CriarCategoriaMenuItem_Click);
            // 
            // DocumentManagementForm
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(1900, 857);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.pdfViewer2);
            this.Controls.Add(this.pdfViewer1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxFiles);
            this.Controls.Add(this.comboBoxSubCategorySearch);
            this.Controls.Add(this.comboBoxCategorySearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.btnLoadFiles);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.cmbSubCategory);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.btnSaveFiles);
            this.Controls.Add(this.btnDeleteFiles);
            this.Controls.Add(this.btnClearFiles);
            this.Controls.Add(this.btnRenameFile1);
            this.Controls.Add(this.btnMoveFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocumentManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerenciamento de Documentos";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentManagementForm_KeyDown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private PdfiumViewer.PdfViewer pdfViewer1;
        private PdfiumViewer.PdfViewer pdfViewer2;
    }
}