using System.Windows.Forms;
using NPOI.OpenXmlFormats.Wordprocessing;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace DocsViewer 
{
    partial class DocumentViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxSearch3;
        private System.Windows.Forms.Button buttonSearch3;
        private System.Windows.Forms.ListBox listBoxFiles3;
        private PdfiumViewer.PdfViewer pdfViewer3;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnManagement;
        private System.Windows.Forms.ComboBox comboBoxCategory3;
        private System.Windows.Forms.ComboBox comboBoxSubCategory3;
        private System.Windows.Forms.Button btnToggleDarkMode;
        private System.Windows.Forms.Button btnVisualizacaoDupla;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnPrev;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripLabel lblPage;        
        private System.Windows.Forms.ToolStripLabel lblSignatureStatus1;        
        private System.Windows.Forms.ToolStripButton btnRotateLeft;
        private System.Windows.Forms.ToolStripButton btnRotateRight;
        private System.Windows.Forms.ToolStripButton btnFitPage1;
        private System.Windows.Forms.ToolStripButton btnVerifySignatures;
        private System.Windows.Forms.ToolStripButton btnVerOriginal;
        private System.Windows.Forms.ToolStripButton btnPrintComCarimbo;
        private ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolTip toolTip1;
        private ToolStripButton toolBtnFirstPage1;
        private ToolStripButton toolBtnResetRotation1;


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
            this.components = new System.ComponentModel.Container();
            this.textBoxSearch3 = new System.Windows.Forms.TextBox();
            this.listBoxFiles3 = new System.Windows.Forms.ListBox();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnManagement = new System.Windows.Forms.Button();
            this.comboBoxCategory3 = new System.Windows.Forms.ComboBox();
            this.comboBoxSubCategory3 = new System.Windows.Forms.ComboBox();
            this.btnToggleDarkMode = new System.Windows.Forms.Button();
            this.btnVisualizacaoDupla = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnToggleAjust1 = new System.Windows.Forms.Button();
            this.btnToggleListBoxes1 = new System.Windows.Forms.Button();
            this.buttonSearch3 = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnPrintComCarimbo = new System.Windows.Forms.ToolStripButton();
            this.toolBtnFirstPage1 = new System.Windows.Forms.ToolStripButton();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.lblPage = new System.Windows.Forms.ToolStripLabel();
            this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.toolBtnResetRotation1 = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
            this.btnFitPage1 = new System.Windows.Forms.ToolStripButton();
            this.lblSignatureStatus1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnVerifySignatures = new System.Windows.Forms.ToolStripButton();
            this.btnVerOriginal = new System.Windows.Forms.ToolStripButton();
            this.pdfViewer3 = new PdfiumViewer.PdfViewer();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSearch3
            // 
            this.textBoxSearch3.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxSearch3.Location = new System.Drawing.Point(12, 43);
            this.textBoxSearch3.Name = "textBoxSearch3";
            this.textBoxSearch3.Size = new System.Drawing.Size(126, 20);
            this.textBoxSearch3.TabIndex = 0;
            // 
            // listBoxFiles3
            // 
            this.listBoxFiles3.FormattingEnabled = true;
            this.listBoxFiles3.HorizontalScrollbar = true;
            this.listBoxFiles3.Location = new System.Drawing.Point(12, 97);
            this.listBoxFiles3.Name = "listBoxFiles3";
            this.listBoxFiles3.Size = new System.Drawing.Size(300, 875);
            this.listBoxFiles3.TabIndex = 4;
            this.listBoxFiles3.SelectedIndexChanged += new System.EventHandler(this.ListBoxFiles3_SelectedIndexChanged);
            // 
            // btnLogout
            // 
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Location = new System.Drawing.Point(849, 6);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 36);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // btnManagement
            // 
            this.btnManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManagement.Location = new System.Drawing.Point(299, 6);
            this.btnManagement.Name = "btnManagement";
            this.btnManagement.Size = new System.Drawing.Size(120, 36);
            this.btnManagement.TabIndex = 10;
            this.btnManagement.Text = "Gerenciamento";
            this.btnManagement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnManagement.UseVisualStyleBackColor = true;
            this.btnManagement.Click += new System.EventHandler(this.btnManagement_Click);
            // 
            // comboBoxCategory3
            // 
            this.comboBoxCategory3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory3.FormattingEnabled = true;
            this.comboBoxCategory3.Location = new System.Drawing.Point(12, 12);
            this.comboBoxCategory3.Name = "comboBoxCategory3";
            this.comboBoxCategory3.Size = new System.Drawing.Size(126, 21);
            this.comboBoxCategory3.TabIndex = 11;
            this.comboBoxCategory3.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCategory_SelectedIndexChanged);
            // 
            // comboBoxSubCategory3
            // 
            this.comboBoxSubCategory3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubCategory3.FormattingEnabled = true;
            this.comboBoxSubCategory3.Location = new System.Drawing.Point(144, 12);
            this.comboBoxSubCategory3.Name = "comboBoxSubCategory3";
            this.comboBoxSubCategory3.Size = new System.Drawing.Size(126, 21);
            this.comboBoxSubCategory3.TabIndex = 12;
            // 
            // btnToggleDarkMode
            // 
            this.btnToggleDarkMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToggleDarkMode.Location = new System.Drawing.Point(621, 6);
            this.btnToggleDarkMode.Name = "btnToggleDarkMode";
            this.btnToggleDarkMode.Size = new System.Drawing.Size(110, 36);
            this.btnToggleDarkMode.TabIndex = 15;
            this.btnToggleDarkMode.Text = "Modo Escuro";
            this.btnToggleDarkMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToggleDarkMode.UseVisualStyleBackColor = true;
            this.btnToggleDarkMode.Click += new System.EventHandler(this.BtnToggleDarkMode_Click);
            // 
            // btnVisualizacaoDupla
            // 
            this.btnVisualizacaoDupla.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVisualizacaoDupla.Location = new System.Drawing.Point(537, 6);
            this.btnVisualizacaoDupla.Name = "btnVisualizacaoDupla";
            this.btnVisualizacaoDupla.Size = new System.Drawing.Size(80, 36);
            this.btnVisualizacaoDupla.TabIndex = 16;
            this.btnVisualizacaoDupla.Text = "Dupla";
            this.btnVisualizacaoDupla.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVisualizacaoDupla.UseVisualStyleBackColor = true;
            this.btnVisualizacaoDupla.Click += new System.EventHandler(this.BtnVisualizacaoDupla_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChangePassword.Location = new System.Drawing.Point(423, 6);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(110, 36);
            this.btnChangePassword.TabIndex = 17;
            this.btnChangePassword.Text = "Alterar Senha";
            this.btnChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click_1);
            // 
            // btnToggleAjust1
            // 
            this.btnToggleAjust1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToggleAjust1.Location = new System.Drawing.Point(735, 6);
            this.btnToggleAjust1.Name = "btnToggleAjust1";
            this.btnToggleAjust1.Size = new System.Drawing.Size(110, 36);
            this.btnToggleAjust1.TabIndex = 19;
            this.btnToggleAjust1.Text = "Ajustar Página";
            this.btnToggleAjust1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToggleAjust1.UseVisualStyleBackColor = true;
            this.btnToggleAjust1.Click += new System.EventHandler(this.btnToggleAjust1_Click_1);
            // 
            // btnToggleListBoxes1
            // 
            this.btnToggleListBoxes1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToggleListBoxes1.Location = new System.Drawing.Point(12, 67);
            this.btnToggleListBoxes1.Name = "btnToggleListBoxes1";
            this.btnToggleListBoxes1.Size = new System.Drawing.Size(28, 28);
            this.btnToggleListBoxes1.TabIndex = 18;
            this.btnToggleListBoxes1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToggleListBoxes1.UseVisualStyleBackColor = true;
            this.btnToggleListBoxes1.Click += new System.EventHandler(this.btnToggleListBoxes1_Click);
            // 
            // buttonSearch3
            // 
            this.buttonSearch3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSearch3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSearch3.Location = new System.Drawing.Point(144, 39);
            this.buttonSearch3.Name = "buttonSearch3";
            this.buttonSearch3.Size = new System.Drawing.Size(80, 28);
            this.buttonSearch3.TabIndex = 2;
            this.buttonSearch3.Text = "Procurar";
            this.buttonSearch3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSearch3.UseVisualStyleBackColor = true;
            this.buttonSearch3.Click += new System.EventHandler(this.ButtonSearch3_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrintComCarimbo,
            this.toolBtnFirstPage1,
            this.btnPrev,
            this.btnNext,
            this.lblPage,
            this.btnRotateLeft,
            this.toolBtnResetRotation1,
            this.btnRotateRight,
            this.btnFitPage1,
            this.lblSignatureStatus1,
            this.toolStripSeparator1,
            this.btnVerifySignatures,
            this.btnVerOriginal});
            this.toolStrip.Location = new System.Drawing.Point(413, 100);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(328, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // btnPrintComCarimbo
            // 
            this.btnPrintComCarimbo.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintComCarimbo.Image = global::DocsViewer.Properties.Resources.print;
            this.btnPrintComCarimbo.Name = "btnPrintComCarimbo";
            this.btnPrintComCarimbo.Size = new System.Drawing.Size(23, 22);
            this.btnPrintComCarimbo.Click += new System.EventHandler(this.btnPrintComCarimbo_Click);
            // 
            // toolBtnFirstPage1
            // 
            this.toolBtnFirstPage1.Image = global::DocsViewer.Properties.Resources.fist_page;
            this.toolBtnFirstPage1.Name = "toolBtnFirstPage1";
            this.toolBtnFirstPage1.Size = new System.Drawing.Size(23, 22);
            this.toolBtnFirstPage1.ToolTipText = "Ir para a Primeira Página";
            this.toolBtnFirstPage1.Click += new System.EventHandler(this.toolBtnFirstPage1_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Image = global::DocsViewer.Properties.Resources.previous_page;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 22);
            this.btnPrev.ToolTipText = "Página Anterior";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = global::DocsViewer.Properties.Resources.next_page;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 22);
            this.btnNext.ToolTipText = "Próxima Página";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblPage
            // 
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(49, 22);
            this.lblPage.Text = "Página: ";
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Image = global::DocsViewer.Properties.Resources.rotate_Left;
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(23, 22);
            this.btnRotateLeft.ToolTipText = "Girar Página para Esquerda";
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // toolBtnResetRotation1
            // 
            this.toolBtnResetRotation1.Image = global::DocsViewer.Properties.Resources.rotate_reset;
            this.toolBtnResetRotation1.Name = "toolBtnResetRotation1";
            this.toolBtnResetRotation1.Size = new System.Drawing.Size(23, 22);
            this.toolBtnResetRotation1.ToolTipText = "Restaurar a Rotação Original da Página";
            this.toolBtnResetRotation1.Click += new System.EventHandler(this.toolBtnResetRotation1_Click);
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Image = global::DocsViewer.Properties.Resources.rotate_right;
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(23, 22);
            this.btnRotateRight.ToolTipText = "Girar Página para Direita";
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click_1);
            // 
            // btnFitPage1
            // 
            this.btnFitPage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFitPage1.Image = global::DocsViewer.Properties.Resources.page_ajust;
            this.btnFitPage1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnFitPage1.Name = "btnFitPage1";
            this.btnFitPage1.Size = new System.Drawing.Size(23, 22);
            this.btnFitPage1.Text = "Ajustar Página";
            this.btnFitPage1.ToolTipText = "Ajustar a Visualização da Página";
            this.btnFitPage1.Click += new System.EventHandler(this.btnFitPage1_Click);
            // 
            // lblSignatureStatus1
            // 
            this.lblSignatureStatus1.Enabled = false;
            this.lblSignatureStatus1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignatureStatus1.Name = "lblSignatureStatus1";
            this.lblSignatureStatus1.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnVerifySignatures
            // 
            this.btnVerifySignatures.Enabled = false;
            this.btnVerifySignatures.Image = global::DocsViewer.Properties.Resources.digital_sign;
            this.btnVerifySignatures.Name = "btnVerifySignatures";
            this.btnVerifySignatures.Size = new System.Drawing.Size(23, 22);
            this.btnVerifySignatures.ToolTipText = "Ver Assinatura Digital(Alt+H)";
            this.btnVerifySignatures.Click += new System.EventHandler(this.btnVerifySignatures_Click);
            // 
            // btnVerOriginal
            // 
            this.btnVerOriginal.Enabled = false;
            this.btnVerOriginal.Image = global::DocsViewer.Properties.Resources.originalpdf;
            this.btnVerOriginal.Name = "btnVerOriginal";
            this.btnVerOriginal.Size = new System.Drawing.Size(23, 22);
            this.btnVerOriginal.ToolTipText = "Ver Documento Original(Alt+J)";
            this.btnVerOriginal.Click += new System.EventHandler(this.btnVerOriginal_Click);
            // 
            // pdfViewer3
            // 
            this.pdfViewer3.AutoSize = true;
            this.pdfViewer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pdfViewer3.Location = new System.Drawing.Point(317, 97);
            this.pdfViewer3.Name = "pdfViewer3";
            this.pdfViewer3.Size = new System.Drawing.Size(1300, 875);
            this.pdfViewer3.TabIndex = 6;
            // 
            // DocumentViewerForm
            // 
            this.AcceptButton = this.buttonSearch3;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1900, 1057);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.btnToggleAjust1);
            this.Controls.Add(this.btnToggleListBoxes1);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnVisualizacaoDupla);
            this.Controls.Add(this.btnToggleDarkMode);
            this.Controls.Add(this.comboBoxSubCategory3);
            this.Controls.Add(this.comboBoxCategory3);
            this.Controls.Add(this.btnManagement);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.pdfViewer3);
            this.Controls.Add(this.listBoxFiles3);
            this.Controls.Add(this.buttonSearch3);
            this.Controls.Add(this.textBoxSearch3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Name = "DocumentViewerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visualizador de Documentos - Visualização Simples";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentViewerForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentViewerForm_KeyDown);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Button btnToggleAjust1;
        private System.Windows.Forms.Button btnToggleListBoxes1;
        
    }
}