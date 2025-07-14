using System.Windows.Forms;

namespace DocsViewer 
{
    partial class MoveFileForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBoxCategory;
        private ComboBox comboBoxSubCategory;
        private Button btnMove;
        private Label label1;
        private Label label2;
        private ListBox listBoxFiles;  // no topo da classe

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.comboBoxSubCategory = new System.Windows.Forms.ComboBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory.Location = new System.Drawing.Point(12, 24);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(224, 21);
            this.comboBoxCategory.TabIndex = 0;
            this.comboBoxCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxCategory_SelectedIndexChanged);
            // 
            // comboBoxSubCategory
            // 
            this.comboBoxSubCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubCategory.Location = new System.Drawing.Point(12, 67);
            this.comboBoxSubCategory.Name = "comboBoxSubCategory";
            this.comboBoxSubCategory.Size = new System.Drawing.Size(224, 21);
            this.comboBoxSubCategory.TabIndex = 1;
            // 
            // btnMove
            // 
            this.btnMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMove.Location = new System.Drawing.Point(253, 39);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(80, 38);
            this.btnMove.TabIndex = 2;
            this.btnMove.Text = "Mover";
            this.btnMove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMove.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Categoria:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Subcategoria:";
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.HorizontalScrollbar = true;
            this.listBoxFiles.Location = new System.Drawing.Point(15, 94);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(318, 303);
            this.listBoxFiles.TabIndex = 0;
            // 
            // MoveFileForm
            // 
            this.ClientSize = new System.Drawing.Size(353, 429);
            this.Controls.Add(this.listBoxFiles);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.comboBoxSubCategory);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "MoveFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mover Arquivo";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentManagementForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
