namespace DocsViewer 
{
    partial class CategoryManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxCategories;
        private System.Windows.Forms.ListBox listBoxSubcategories;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Button btnEditCategory;
        private System.Windows.Forms.Button btnDeleteCategory;
        private System.Windows.Forms.Button btnAddSubcategory;
        private System.Windows.Forms.Button btnEditSubcategory;
        private System.Windows.Forms.Button btnDeleteSubcategory;
        private System.Windows.Forms.Label labelCategories;
        private System.Windows.Forms.Label labelSubcategories;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryManagementForm));
            this.listBoxCategories = new System.Windows.Forms.ListBox();
            this.listBoxSubcategories = new System.Windows.Forms.ListBox();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.btnEditCategory = new System.Windows.Forms.Button();
            this.btnDeleteCategory = new System.Windows.Forms.Button();
            this.btnAddSubcategory = new System.Windows.Forms.Button();
            this.btnEditSubcategory = new System.Windows.Forms.Button();
            this.btnDeleteSubcategory = new System.Windows.Forms.Button();
            this.labelCategories = new System.Windows.Forms.Label();
            this.labelSubcategories = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxCategories
            // 
            this.listBoxCategories.Location = new System.Drawing.Point(30, 50);
            this.listBoxCategories.Name = "listBoxCategories";
            this.listBoxCategories.Size = new System.Drawing.Size(290, 290);
            this.listBoxCategories.TabIndex = 0;
            this.listBoxCategories.SelectedIndexChanged += new System.EventHandler(this.listBoxCategories_SelectedIndexChanged);
            // 
            // listBoxSubcategories
            // 
            this.listBoxSubcategories.Location = new System.Drawing.Point(355, 50);
            this.listBoxSubcategories.Name = "listBoxSubcategories";
            this.listBoxSubcategories.Size = new System.Drawing.Size(290, 290);
            this.listBoxSubcategories.TabIndex = 1;
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCategory.Location = new System.Drawing.Point(30, 346);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(90, 38);
            this.btnAddCategory.TabIndex = 2;
            this.btnAddCategory.Text = "Adicionar";
            this.btnAddCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // btnEditCategory
            // 
            this.btnEditCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditCategory.Location = new System.Drawing.Point(128, 346);
            this.btnEditCategory.Name = "btnEditCategory";
            this.btnEditCategory.Size = new System.Drawing.Size(90, 38);
            this.btnEditCategory.TabIndex = 3;
            this.btnEditCategory.Text = "Editar";
            this.btnEditCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEditCategory.UseVisualStyleBackColor = true;
            this.btnEditCategory.Click += new System.EventHandler(this.btnEditCategory_Click);
            // 
            // btnDeleteCategory
            // 
            this.btnDeleteCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteCategory.Location = new System.Drawing.Point(226, 346);
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.Size = new System.Drawing.Size(90, 38);
            this.btnDeleteCategory.TabIndex = 4;
            this.btnDeleteCategory.Text = "Excluir";
            this.btnDeleteCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteCategory.UseVisualStyleBackColor = true;
            this.btnDeleteCategory.Click += new System.EventHandler(this.btnDeleteCategory_Click);
            // 
            // btnAddSubcategory
            // 
            this.btnAddSubcategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddSubcategory.Location = new System.Drawing.Point(357, 346);
            this.btnAddSubcategory.Name = "btnAddSubcategory";
            this.btnAddSubcategory.Size = new System.Drawing.Size(90, 38);
            this.btnAddSubcategory.TabIndex = 5;
            this.btnAddSubcategory.Text = "Adicionar";
            this.btnAddSubcategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddSubcategory.UseVisualStyleBackColor = true;
            this.btnAddSubcategory.Click += new System.EventHandler(this.btnAddSubcategory_Click);
            // 
            // btnEditSubcategory
            // 
            this.btnEditSubcategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditSubcategory.Location = new System.Drawing.Point(455, 346);
            this.btnEditSubcategory.Name = "btnEditSubcategory";
            this.btnEditSubcategory.Size = new System.Drawing.Size(90, 38);
            this.btnEditSubcategory.TabIndex = 6;
            this.btnEditSubcategory.Text = "Editar";
            this.btnEditSubcategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEditSubcategory.UseVisualStyleBackColor = true;
            this.btnEditSubcategory.Click += new System.EventHandler(this.btnEditSubcategory_Click);
            // 
            // btnDeleteSubcategory
            // 
            this.btnDeleteSubcategory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSubcategory.Location = new System.Drawing.Point(553, 346);
            this.btnDeleteSubcategory.Name = "btnDeleteSubcategory";
            this.btnDeleteSubcategory.Size = new System.Drawing.Size(90, 38);
            this.btnDeleteSubcategory.TabIndex = 7;
            this.btnDeleteSubcategory.Text = "Excluir";
            this.btnDeleteSubcategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteSubcategory.UseVisualStyleBackColor = true;
            this.btnDeleteSubcategory.Click += new System.EventHandler(this.btnDeleteSubcategory_Click);
            // 
            // labelCategories
            // 
            this.labelCategories.AutoSize = true;
            this.labelCategories.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCategories.Location = new System.Drawing.Point(129, 20);
            this.labelCategories.Name = "labelCategories";
            this.labelCategories.Size = new System.Drawing.Size(93, 19);
            this.labelCategories.TabIndex = 8;
            this.labelCategories.Text = "Categorias";
            // 
            // labelSubcategories
            // 
            this.labelSubcategories.AutoSize = true;
            this.labelSubcategories.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubcategories.Location = new System.Drawing.Point(440, 20);
            this.labelSubcategories.Name = "labelSubcategories";
            this.labelSubcategories.Size = new System.Drawing.Size(120, 19);
            this.labelSubcategories.TabIndex = 9;
            this.labelSubcategories.Text = "Subcategorias";
            // 
            // CategoryManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(674, 398);
            this.Controls.Add(this.listBoxCategories);
            this.Controls.Add(this.listBoxSubcategories);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.btnEditCategory);
            this.Controls.Add(this.btnDeleteCategory);
            this.Controls.Add(this.btnAddSubcategory);
            this.Controls.Add(this.btnEditSubcategory);
            this.Controls.Add(this.btnDeleteSubcategory);
            this.Controls.Add(this.labelCategories);
            this.Controls.Add(this.labelSubcategories);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CategoryManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerenciar Categorias e Subcategorias";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}