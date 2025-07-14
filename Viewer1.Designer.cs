using Org.BouncyCastle.Asn1.Crmf;
using System.Windows.Forms;

namespace DocsViewer 
{   
    partial class Viewer1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxSearch1;
        private System.Windows.Forms.TextBox textBoxSearch2;
        private System.Windows.Forms.Button buttonSearch1;
        private System.Windows.Forms.Button buttonSearch2;
        private System.Windows.Forms.ListBox listBoxFiles1;
        private System.Windows.Forms.ListBox listBoxFiles2;
        private PdfiumViewer.PdfViewer pdfViewer1;
        private PdfiumViewer.PdfViewer pdfViewer2;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnManagement;
        private System.Windows.Forms.ComboBox comboBoxCategory1;
        private System.Windows.Forms.ComboBox comboBoxCategory2;
        private System.Windows.Forms.ComboBox comboBoxSubCategory1;
        private System.Windows.Forms.ComboBox comboBoxSubCategory2;
        private System.Windows.Forms.Button btnToggleDarkMode;
        private System.Windows.Forms.Button btnVisualizacaoSimples;
        private System.Windows.Forms.Button btnChangePassword; // Novo botão para alterar senha
        private System.Windows.Forms.Button btnToggleListBoxes1;
        private ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPrev1;
        private System.Windows.Forms.ToolStripButton btnNext1;
        private System.Windows.Forms.ToolStripLabel lblPage1;
        private System.Windows.Forms.ToolStripLabel lblSignatureStatus1;
        private System.Windows.Forms.ToolStripButton btnRotateLeft1;
        private System.Windows.Forms.ToolStripButton btnRotateRight1;
        private System.Windows.Forms.ToolStripButton btnFitPage1;
        private System.Windows.Forms.ToolStripButton btnVerifySignatures1;
        private System.Windows.Forms.ToolStripButton btnVerOriginal1;
        private System.Windows.Forms.ToolStripButton btnPrintComCarimbo1;
        private System.Windows.Forms.ToolTip toolTip1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnPrev2;
        private System.Windows.Forms.ToolStripButton btnNext2;
        private System.Windows.Forms.ToolStripLabel lblPage2;
        private System.Windows.Forms.ToolStripLabel lblSignatureStatus2;
        private System.Windows.Forms.ToolStripButton btnRotateLeft2;
        private System.Windows.Forms.ToolStripButton btnRotateRight2;
        private System.Windows.Forms.ToolStripButton btnFitPage2;
        private System.Windows.Forms.ToolStripButton btnVerifySignatures2;
        private System.Windows.Forms.ToolStripButton btnVerOriginal2;
        private System.Windows.Forms.ToolStripButton btnPrintComCarimbo2;
        private System.Windows.Forms.ToolTip toolTip2;        
        private ToolStripSeparator toolStripSeparator2;


        

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
            this.textBoxSearch1 = new System.Windows.Forms.TextBox();
            this.textBoxSearch2 = new System.Windows.Forms.TextBox();
            this.buttonSearch1 = new System.Windows.Forms.Button();
            this.buttonSearch2 = new System.Windows.Forms.Button();
            this.listBoxFiles1 = new System.Windows.Forms.ListBox();
            this.listBoxFiles2 = new System.Windows.Forms.ListBox();
            this.pdfViewer1 = new PdfiumViewer.PdfViewer();
            this.pdfViewer2 = new PdfiumViewer.PdfViewer();
            this.btnLogout = new System.Windows.Forms.Button();
            this.comboBoxCategory1 = new System.Windows.Forms.ComboBox();
            this.comboBoxCategory2 = new System.Windows.Forms.ComboBox();
            this.comboBoxSubCategory1 = new System.Windows.Forms.ComboBox();
            this.comboBoxSubCategory2 = new System.Windows.Forms.ComboBox();
            this.btnToggleDarkMode = new System.Windows.Forms.Button();
            this.btnVisualizacaoSimples = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnToggleListBoxes1 = new System.Windows.Forms.Button();
            this.btnToggleAjust = new System.Windows.Forms.Button();
            this.btnToggleListBoxes2 = new System.Windows.Forms.Button();
            this.lblPage1 = new System.Windows.Forms.ToolStripLabel();
            this.lblSignatureStatus1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrintComCarimbo1 = new System.Windows.Forms.ToolStripButton();
            this.toolBtnFirstPage1 = new System.Windows.Forms.ToolStripButton();
            this.btnPrev1 = new System.Windows.Forms.ToolStripButton();
            this.btnNext1 = new System.Windows.Forms.ToolStripButton();
            this.btnRotateLeft1 = new System.Windows.Forms.ToolStripButton();
            this.toolBtnResetRotation1 = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRight1 = new System.Windows.Forms.ToolStripButton();
            this.btnFitPage1 = new System.Windows.Forms.ToolStripButton();
            this.btnVerifySignatures1 = new System.Windows.Forms.ToolStripButton();
            this.btnVerOriginal1 = new System.Windows.Forms.ToolStripButton();
            this.lblPage2 = new System.Windows.Forms.ToolStripLabel();
            this.lblSignatureStatus2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnPrintComCarimbo2 = new System.Windows.Forms.ToolStripButton();
            this.toolBtnFirstPage2 = new System.Windows.Forms.ToolStripButton();
            this.btnPrev2 = new System.Windows.Forms.ToolStripButton();
            this.btnNext2 = new System.Windows.Forms.ToolStripButton();
            this.btnRotateLeft2 = new System.Windows.Forms.ToolStripButton();
            this.toolBtnResetRotation2 = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRight2 = new System.Windows.Forms.ToolStripButton();
            this.btnFitPage2 = new System.Windows.Forms.ToolStripButton();
            this.btnVerifySignatures2 = new System.Windows.Forms.ToolStripButton();
            this.btnVerOriginal2 = new System.Windows.Forms.ToolStripButton();
            this.btnManagement = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSearch1
            // 
            this.textBoxSearch1.Location = new System.Drawing.Point(12, 36);
            this.textBoxSearch1.Name = "textBoxSearch1";
            this.textBoxSearch1.Size = new System.Drawing.Size(130, 20);
            this.textBoxSearch1.TabIndex = 0;
            // 
            // textBoxSearch2
            // 
            this.textBoxSearch2.Location = new System.Drawing.Point(947, 35);
            this.textBoxSearch2.Name = "textBoxSearch2";
            this.textBoxSearch2.Size = new System.Drawing.Size(130, 20);
            this.textBoxSearch2.TabIndex = 1;
            this.textBoxSearch2.TextChanged += new System.EventHandler(this.TextBoxSearch2_TextChanged);
            // 
            // buttonSearch1
            // 
            this.buttonSearch1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSearch1.Location = new System.Drawing.Point(151, 33);
            this.buttonSearch1.Name = "buttonSearch1";
            this.buttonSearch1.Size = new System.Drawing.Size(80, 28);
            this.buttonSearch1.TabIndex = 2;
            this.buttonSearch1.Text = "Procurar";
            this.buttonSearch1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSearch1.UseVisualStyleBackColor = true;
            this.buttonSearch1.Click += new System.EventHandler(this.ButtonSearch1_Click);
            // 
            // buttonSearch2
            // 
            this.buttonSearch2.BackColor = System.Drawing.Color.Transparent;
            this.buttonSearch2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSearch2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSearch2.Location = new System.Drawing.Point(1085, 33);
            this.buttonSearch2.Name = "buttonSearch2";
            this.buttonSearch2.Size = new System.Drawing.Size(80, 28);
            this.buttonSearch2.TabIndex = 3;
            this.buttonSearch2.Text = "Procurar";
            this.buttonSearch2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSearch2.UseVisualStyleBackColor = false;
            this.buttonSearch2.Click += new System.EventHandler(this.ButtonSearch2_Click);
            // 
            // listBoxFiles1
            // 
            this.listBoxFiles1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.listBoxFiles1.FormattingEnabled = true;
            this.listBoxFiles1.HorizontalScrollbar = true;
            this.listBoxFiles1.Location = new System.Drawing.Point(10, 97);
            this.listBoxFiles1.Name = "listBoxFiles1";
            this.listBoxFiles1.Size = new System.Drawing.Size(221, 888);
            this.listBoxFiles1.TabIndex = 4;
            this.listBoxFiles1.SelectedIndexChanged += new System.EventHandler(this.ListBoxFiles1_SelectedIndexChanged);
            // 
            // listBoxFiles2
            // 
            this.listBoxFiles2.FormattingEnabled = true;
            this.listBoxFiles2.HorizontalScrollbar = true;
            this.listBoxFiles2.Location = new System.Drawing.Point(946, 98);
            this.listBoxFiles2.Name = "listBoxFiles2";
            this.listBoxFiles2.Size = new System.Drawing.Size(213, 888);
            this.listBoxFiles2.TabIndex = 5;
            this.listBoxFiles2.SelectedIndexChanged += new System.EventHandler(this.ListBoxFiles2_SelectedIndexChanged);
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.AutoSize = true;
            this.pdfViewer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pdfViewer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pdfViewer1.Location = new System.Drawing.Point(237, 97);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.Size = new System.Drawing.Size(700, 886);
            this.pdfViewer1.TabIndex = 6;
            this.pdfViewer1.Load += new System.EventHandler(this.pdfViewer1_Load);
            // 
            // pdfViewer2
            // 
            this.pdfViewer2.AutoSize = true;
            this.pdfViewer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer2.Location = new System.Drawing.Point(1165, 97);
            this.pdfViewer2.Name = "pdfViewer2";
            this.pdfViewer2.Size = new System.Drawing.Size(700, 886);
            this.pdfViewer2.TabIndex = 7;
            // 
            // btnLogout
            // 
            this.btnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Location = new System.Drawing.Point(838, 8);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 36);
            this.btnLogout.TabIndex = 8;
            this.btnLogout.Text = "Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // comboBoxCategory1
            // 
            this.comboBoxCategory1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory1.FormattingEnabled = true;
            this.comboBoxCategory1.Location = new System.Drawing.Point(12, 8);
            this.comboBoxCategory1.Name = "comboBoxCategory1";
            this.comboBoxCategory1.Size = new System.Drawing.Size(130, 21);
            this.comboBoxCategory1.TabIndex = 10;
            this.comboBoxCategory1.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCategory_SelectedIndexChanged);
            // 
            // comboBoxCategory2
            // 
            this.comboBoxCategory2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory2.FormattingEnabled = true;
            this.comboBoxCategory2.Location = new System.Drawing.Point(947, 7);
            this.comboBoxCategory2.Name = "comboBoxCategory2";
            this.comboBoxCategory2.Size = new System.Drawing.Size(130, 21);
            this.comboBoxCategory2.TabIndex = 11;
            this.comboBoxCategory2.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCategory_SelectedIndexChanged);
            // 
            // comboBoxSubCategory1
            // 
            this.comboBoxSubCategory1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubCategory1.FormattingEnabled = true;
            this.comboBoxSubCategory1.Location = new System.Drawing.Point(151, 8);
            this.comboBoxSubCategory1.Name = "comboBoxSubCategory1";
            this.comboBoxSubCategory1.Size = new System.Drawing.Size(130, 21);
            this.comboBoxSubCategory1.TabIndex = 12;
            this.comboBoxSubCategory1.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSubCategory1_SelectedIndexChanged);
            // 
            // comboBoxSubCategory2
            // 
            this.comboBoxSubCategory2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubCategory2.FormattingEnabled = true;
            this.comboBoxSubCategory2.Location = new System.Drawing.Point(1085, 7);
            this.comboBoxSubCategory2.Name = "comboBoxSubCategory2";
            this.comboBoxSubCategory2.Size = new System.Drawing.Size(130, 21);
            this.comboBoxSubCategory2.TabIndex = 13;
            this.comboBoxSubCategory2.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSubCategory2_SelectedIndexChanged);
            // 
            // btnToggleDarkMode
            // 
            this.btnToggleDarkMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToggleDarkMode.Location = new System.Drawing.Point(621, 8);
            this.btnToggleDarkMode.Name = "btnToggleDarkMode";
            this.btnToggleDarkMode.Size = new System.Drawing.Size(100, 36);
            this.btnToggleDarkMode.TabIndex = 14;
            this.btnToggleDarkMode.Text = "Modo Escuro";
            this.btnToggleDarkMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToggleDarkMode.UseVisualStyleBackColor = true;
            this.btnToggleDarkMode.Click += new System.EventHandler(this.BtnToggleDarkMode_Click);
            // 
            // btnVisualizacaoSimples
            // 
            this.btnVisualizacaoSimples.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVisualizacaoSimples.Location = new System.Drawing.Point(537, 8);
            this.btnVisualizacaoSimples.Name = "btnVisualizacaoSimples";
            this.btnVisualizacaoSimples.Size = new System.Drawing.Size(80, 36);
            this.btnVisualizacaoSimples.TabIndex = 15;
            this.btnVisualizacaoSimples.Text = "Simples";
            this.btnVisualizacaoSimples.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVisualizacaoSimples.UseVisualStyleBackColor = true;
            this.btnVisualizacaoSimples.Click += new System.EventHandler(this.BtnVisualizacaoSimples_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChangePassword.Location = new System.Drawing.Point(423, 8);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(110, 36);
            this.btnChangePassword.TabIndex = 16;
            this.btnChangePassword.Text = "Alterar Senha";
            this.btnChangePassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += new System.EventHandler(this.BtnChangePassword_Click);
            // 
            // btnToggleListBoxes1
            // 
            this.btnToggleListBoxes1.Location = new System.Drawing.Point(12, 64);
            this.btnToggleListBoxes1.Name = "btnToggleListBoxes1";
            this.btnToggleListBoxes1.Size = new System.Drawing.Size(28, 28);
            this.btnToggleListBoxes1.TabIndex = 16;
            this.btnToggleListBoxes1.UseVisualStyleBackColor = true;
            this.btnToggleListBoxes1.Click += new System.EventHandler(this.btnToggleListBoxes1_Click);
            // 
            // btnToggleAjust
            // 
            this.btnToggleAjust.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnToggleAjust.Location = new System.Drawing.Point(725, 8);
            this.btnToggleAjust.Name = "btnToggleAjust";
            this.btnToggleAjust.Size = new System.Drawing.Size(110, 36);
            this.btnToggleAjust.TabIndex = 17;
            this.btnToggleAjust.Text = "Ajustar Página";
            this.btnToggleAjust.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToggleAjust.UseVisualStyleBackColor = true;
            this.btnToggleAjust.Click += new System.EventHandler(this.btnToggleAjust_Click);
            // 
            // btnToggleListBoxes2
            // 
            this.btnToggleListBoxes2.Location = new System.Drawing.Point(947, 64);
            this.btnToggleListBoxes2.Name = "btnToggleListBoxes2";
            this.btnToggleListBoxes2.Size = new System.Drawing.Size(28, 28);
            this.btnToggleListBoxes2.TabIndex = 21;
            this.btnToggleListBoxes2.UseVisualStyleBackColor = true;
            this.btnToggleListBoxes2.Click += new System.EventHandler(this.btnToggleListBoxes2_Click);
            // 
            // lblPage1
            // 
            this.lblPage1.Name = "lblPage1";
            this.lblPage1.Size = new System.Drawing.Size(49, 22);
            this.lblPage1.Text = "Página: ";
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
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrintComCarimbo1,
            this.toolBtnFirstPage1,
            this.btnPrev1,
            this.btnNext1,
            this.lblPage1,
            this.btnRotateLeft1,
            this.toolBtnResetRotation1,
            this.btnRotateRight1,
            this.btnFitPage1,
            this.lblSignatureStatus1,
            this.toolStripSeparator1,
            this.btnVerifySignatures1,
            this.btnVerOriginal1});
            this.toolStrip1.Location = new System.Drawing.Point(340, 101);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(297, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 22;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPrintComCarimbo1
            // 
            this.btnPrintComCarimbo1.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintComCarimbo1.Image = global::DocsViewer.Properties.Resources.print;
            this.btnPrintComCarimbo1.Name = "btnPrintComCarimbo1";
            this.btnPrintComCarimbo1.Size = new System.Drawing.Size(23, 22);
            this.btnPrintComCarimbo1.Click += new System.EventHandler(this.btnPrintComCarimbo1_Click);
            // 
            // toolBtnFirstPage1
            // 
            this.toolBtnFirstPage1.Image = global::DocsViewer.Properties.Resources.fist_page;
            this.toolBtnFirstPage1.Name = "toolBtnFirstPage1";
            this.toolBtnFirstPage1.Size = new System.Drawing.Size(23, 22);
            this.toolBtnFirstPage1.ToolTipText = "Ir para a Primeira Página";
            this.toolBtnFirstPage1.Click += new System.EventHandler(this.toolBtnFirstPage1_Click);
            // 
            // btnPrev1
            // 
            this.btnPrev1.Image = global::DocsViewer.Properties.Resources.previous_page;
            this.btnPrev1.Name = "btnPrev1";
            this.btnPrev1.Size = new System.Drawing.Size(23, 22);
            this.btnPrev1.ToolTipText = "Página Anterior";
            this.btnPrev1.Click += new System.EventHandler(this.btnPrev1_Click);
            // 
            // btnNext1
            // 
            this.btnNext1.Image = global::DocsViewer.Properties.Resources.next_page;
            this.btnNext1.Name = "btnNext1";
            this.btnNext1.Size = new System.Drawing.Size(23, 22);
            this.btnNext1.ToolTipText = "Próxima Página";
            this.btnNext1.Click += new System.EventHandler(this.btnNext1_Click);
            // 
            // btnRotateLeft1
            // 
            this.btnRotateLeft1.Image = global::DocsViewer.Properties.Resources.rotate_Left;
            this.btnRotateLeft1.Name = "btnRotateLeft1";
            this.btnRotateLeft1.Size = new System.Drawing.Size(23, 22);
            this.btnRotateLeft1.ToolTipText = "Girar Página para Esquerda";
            this.btnRotateLeft1.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // toolBtnResetRotation1
            // 
            this.toolBtnResetRotation1.Image = global::DocsViewer.Properties.Resources.rotate_reset;
            this.toolBtnResetRotation1.Name = "toolBtnResetRotation1";
            this.toolBtnResetRotation1.Size = new System.Drawing.Size(23, 22);
            this.toolBtnResetRotation1.ToolTipText = "Restaurar a Rotação Original da Página";
            this.toolBtnResetRotation1.Click += new System.EventHandler(this.toolBtnResetRotation1_Click);
            // 
            // btnRotateRight1
            // 
            this.btnRotateRight1.Image = global::DocsViewer.Properties.Resources.rotate_right;
            this.btnRotateRight1.Name = "btnRotateRight1";
            this.btnRotateRight1.Size = new System.Drawing.Size(23, 22);
            this.btnRotateRight1.ToolTipText = "Girar Página para Direita";
            this.btnRotateRight1.Click += new System.EventHandler(this.btnRotateRight_Click);
            // 
            // btnFitPage1
            // 
            this.btnFitPage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFitPage1.Image = global::DocsViewer.Properties.Resources.page_ajust2;
            this.btnFitPage1.Name = "btnFitPage1";
            this.btnFitPage1.Size = new System.Drawing.Size(23, 22);
            this.btnFitPage1.Text = "Ajustar Página";
            this.btnFitPage1.ToolTipText = "Ajustar a Visualização da Página";
            this.btnFitPage1.Click += new System.EventHandler(this.btnFitPage1_Click);
            // 
            // btnVerifySignatures1
            // 
            this.btnVerifySignatures1.Enabled = false;
            this.btnVerifySignatures1.Image = global::DocsViewer.Properties.Resources.digital_sign;
            this.btnVerifySignatures1.Name = "btnVerifySignatures1";
            this.btnVerifySignatures1.Size = new System.Drawing.Size(23, 22);
            this.btnVerifySignatures1.ToolTipText = "Ver Assinatura Digital(Alt+H)";
            this.btnVerifySignatures1.Click += new System.EventHandler(this.btnVerifySignatures_Click);
            // 
            // btnVerOriginal1
            // 
            this.btnVerOriginal1.Enabled = false;
            this.btnVerOriginal1.Image = global::DocsViewer.Properties.Resources.originalpdf;
            this.btnVerOriginal1.Name = "btnVerOriginal1";
            this.btnVerOriginal1.Size = new System.Drawing.Size(23, 22);
            this.btnVerOriginal1.ToolTipText = "Ver Documento Original(Alt+J)";
            this.btnVerOriginal1.Click += new System.EventHandler(this.btnVerOriginal_Click);
            // 
            // lblPage2
            // 
            this.lblPage2.Name = "lblPage2";
            this.lblPage2.Size = new System.Drawing.Size(49, 22);
            this.lblPage2.Text = "Página: ";
            // 
            // lblSignatureStatus2
            // 
            this.lblSignatureStatus2.Enabled = false;
            this.lblSignatureStatus2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignatureStatus2.Name = "lblSignatureStatus2";
            this.lblSignatureStatus2.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrintComCarimbo2,
            this.toolBtnFirstPage2,
            this.btnPrev2,
            this.btnNext2,
            this.lblPage2,
            this.btnRotateLeft2,
            this.toolBtnResetRotation2,
            this.btnRotateRight2,
            this.btnFitPage2,
            this.lblSignatureStatus2,
            this.toolStripSeparator2,
            this.btnVerifySignatures2,
            this.btnVerOriginal2});
            this.toolStrip2.Location = new System.Drawing.Point(1264, 100);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(297, 25);
            this.toolStrip2.Stretch = true;
            this.toolStrip2.TabIndex = 23;
            this.toolStrip2.TabStop = true;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnPrintComCarimbo2
            // 
            this.btnPrintComCarimbo2.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintComCarimbo2.Image = global::DocsViewer.Properties.Resources.print;
            this.btnPrintComCarimbo2.Name = "btnPrintComCarimbo2";
            this.btnPrintComCarimbo2.Size = new System.Drawing.Size(23, 22);
            this.btnPrintComCarimbo2.Click += new System.EventHandler(this.btnPrintComCarimbo2_Click);
            // 
            // toolBtnFirstPage2
            // 
            this.toolBtnFirstPage2.Image = global::DocsViewer.Properties.Resources.fist_page;
            this.toolBtnFirstPage2.Name = "toolBtnFirstPage2";
            this.toolBtnFirstPage2.Size = new System.Drawing.Size(23, 22);
            this.toolBtnFirstPage2.ToolTipText = "Ir para a Primeira Página";
            this.toolBtnFirstPage2.Click += new System.EventHandler(this.toolBtnFirstPage2_Click);
            // 
            // btnPrev2
            // 
            this.btnPrev2.Image = global::DocsViewer.Properties.Resources.previous_page;
            this.btnPrev2.Name = "btnPrev2";
            this.btnPrev2.Size = new System.Drawing.Size(23, 22);
            this.btnPrev2.ToolTipText = "Página Anterior";
            this.btnPrev2.Click += new System.EventHandler(this.btnPrev2_Click);
            // 
            // btnNext2
            // 
            this.btnNext2.Image = global::DocsViewer.Properties.Resources.next_page;
            this.btnNext2.Name = "btnNext2";
            this.btnNext2.Size = new System.Drawing.Size(23, 22);
            this.btnNext2.ToolTipText = "Próxima Página";
            this.btnNext2.Click += new System.EventHandler(this.btnNext2_Click);
            // 
            // btnRotateLeft2
            // 
            this.btnRotateLeft2.Image = global::DocsViewer.Properties.Resources.rotate_Left;
            this.btnRotateLeft2.Name = "btnRotateLeft2";
            this.btnRotateLeft2.Size = new System.Drawing.Size(23, 22);
            this.btnRotateLeft2.ToolTipText = "Girar Página para Esquerda";
            this.btnRotateLeft2.Click += new System.EventHandler(this.btnRotateLeft1_Click);
            // 
            // toolBtnResetRotation2
            // 
            this.toolBtnResetRotation2.Image = global::DocsViewer.Properties.Resources.rotate_reset;
            this.toolBtnResetRotation2.Name = "toolBtnResetRotation2";
            this.toolBtnResetRotation2.Size = new System.Drawing.Size(23, 22);
            this.toolBtnResetRotation2.ToolTipText = "Restaurar a Rotação Original da Página";
            this.toolBtnResetRotation2.Click += new System.EventHandler(this.toolBtnResetRotation2_Click);
            // 
            // btnRotateRight2
            // 
            this.btnRotateRight2.Image = global::DocsViewer.Properties.Resources.rotate_right;
            this.btnRotateRight2.Name = "btnRotateRight2";
            this.btnRotateRight2.Size = new System.Drawing.Size(23, 22);
            this.btnRotateRight2.ToolTipText = "Girar Página para Direita";
            this.btnRotateRight2.Click += new System.EventHandler(this.btnRotateRight1_Click);
            // 
            // btnFitPage2
            // 
            this.btnFitPage2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFitPage2.Image = global::DocsViewer.Properties.Resources.page_ajust;
            this.btnFitPage2.Name = "btnFitPage2";
            this.btnFitPage2.Size = new System.Drawing.Size(23, 22);
            this.btnFitPage2.Text = "Ajustar Página";
            this.btnFitPage2.ToolTipText = "Ajustar a Visualização da Página";
            this.btnFitPage2.Click += new System.EventHandler(this.btnFitPage2_Click);
            // 
            // btnVerifySignatures2
            // 
            this.btnVerifySignatures2.Enabled = false;
            this.btnVerifySignatures2.Image = global::DocsViewer.Properties.Resources.digital_sign;
            this.btnVerifySignatures2.Name = "btnVerifySignatures2";
            this.btnVerifySignatures2.Size = new System.Drawing.Size(23, 22);
            this.btnVerifySignatures2.ToolTipText = "Ver Assinatura Digital(Alt+H)";
            this.btnVerifySignatures2.Click += new System.EventHandler(this.btnVerifySignatures_Click);
            // 
            // btnVerOriginal2
            // 
            this.btnVerOriginal2.Enabled = false;
            this.btnVerOriginal2.Image = global::DocsViewer.Properties.Resources.originalpdf;
            this.btnVerOriginal2.Name = "btnVerOriginal2";
            this.btnVerOriginal2.Size = new System.Drawing.Size(23, 22);
            this.btnVerOriginal2.ToolTipText = "Ver Documento Original(Alt+G)";
            this.btnVerOriginal2.Click += new System.EventHandler(this.btnVerOriginal2_Click);
            // 
            // btnManagement
            // 
            this.btnManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManagement.Location = new System.Drawing.Point(299, 8);
            this.btnManagement.Name = "btnManagement";
            this.btnManagement.Size = new System.Drawing.Size(120, 36);
            this.btnManagement.TabIndex = 9;
            this.btnManagement.Text = "Gerenciamento";
            this.btnManagement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnManagement.UseVisualStyleBackColor = true;
            this.btnManagement.Click += new System.EventHandler(this.btnManagement_Click);
            // 
            // Viewer1
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1900, 1057);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnToggleListBoxes2);
            this.Controls.Add(this.btnToggleAjust);
            this.Controls.Add(this.btnToggleListBoxes1);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnVisualizacaoSimples);
            this.Controls.Add(this.btnToggleDarkMode);
            this.Controls.Add(this.comboBoxSubCategory2);
            this.Controls.Add(this.comboBoxSubCategory1);
            this.Controls.Add(this.comboBoxCategory2);
            this.Controls.Add(this.comboBoxCategory1);
            this.Controls.Add(this.btnManagement);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.pdfViewer1);
            this.Controls.Add(this.listBoxFiles2);
            this.Controls.Add(this.listBoxFiles1);
            this.Controls.Add(this.buttonSearch2);
            this.Controls.Add(this.buttonSearch1);
            this.Controls.Add(this.textBoxSearch2);
            this.Controls.Add(this.textBoxSearch1);
            this.Controls.Add(this.pdfViewer2);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Name = "Viewer1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visualizador de Documentos - Visualização Dupla";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Viewer1_KeyDown_1);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnToggleAjust;
        private System.Windows.Forms.Button btnToggleListBoxes2;
        private ToolStripButton toolBtnFirstPage1;
        private ToolStripButton toolBtnResetRotation1;
        private ToolStripButton toolBtnFirstPage2;
        private ToolStripButton toolBtnResetRotation2;      
        
    }
}