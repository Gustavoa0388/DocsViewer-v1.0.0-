using System.Drawing;
using System.Windows.Forms;
using NPOI.Util.Collections;
using PdfiumViewer;
using System.Linq;
using System.Collections.Generic;

namespace DocsViewer 
{ 

public static class ThemeManager
{
    private static bool _isDarkMode = false;
    public static bool ApplyToAllForms { get; set; } = true;
        private static Color _darkBackColor = Color.FromArgb(45, 45, 48);
    private static Color _darkForeColor = Color.White;
    private static Color _lightBackColor = SystemColors.ControlLightLight;
    private static Color _lightForeColor = SystemColors.ControlText;
        private static Color _darkTextBoxBackColor = Color.FromArgb(37, 37, 38);
        private static Color _darkTextBoxPlaceholderColor = Color.FromArgb(150, 150, 150);
        private static Color _lightTextBoxPlaceholderColor = SystemColors.GrayText;
        private static readonly List<ToolTip> _registeredToolTips = new List<ToolTip>();

        public static Color TextBoxPlaceholderColor =>
            _isDarkMode ? _darkTextBoxPlaceholderColor : _lightTextBoxPlaceholderColor;

        public static Color TextBoxBackColor =>
            _isDarkMode ? _darkTextBoxBackColor : SystemColors.Window;


        public static bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    UpdateAllForms(); // Isso vai atualizar todos os forms abertos

                    // Opcional: Salvar a preferência globalmente
                    Properties.Settings.Default.DarkMode = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static void Initialize(bool darkMode)
    {
        _isDarkMode = darkMode;
    }


        public static void ApplyTheme(Form form)
        {
            if (form is LoginForm) return;

            form.SuspendLayout();
            try
            {
                form.BackColor = _isDarkMode ? _darkBackColor : _lightBackColor;
                form.ForeColor = _isDarkMode ? _darkForeColor : _lightForeColor;

                foreach (Control control in form.Controls)
                {
                    SafeApplyThemeToControl(control);
                }
            }
            finally
            {
                form.ResumeLayout(true);
            }
        }
        private static void SafeApplyThemeToControl(Control control)
        {
            if (control == null || control.Tag as string == "ThemeProcessed")
                return;

            try
            {
                control.Tag = "ThemeProcessed";

                // Pular PDF Viewer durante a aplicação inicial do tema
                if (control.GetType().Name.Contains("PdfRenderer"))
                    return;

                // Aplicar tema normalmente para outros controles
                control.BackColor = _isDarkMode ? _darkBackColor : _lightBackColor;
                control.ForeColor = _isDarkMode ? _darkForeColor : _lightForeColor;

                // Tratamento especial para controles específicos
                if (control is Button button)
                {
                    button.BackColor = _isDarkMode ? Color.FromArgb(63, 63, 70) : SystemColors.ControlLightLight;
                    button.FlatStyle = _isDarkMode ? FlatStyle.Standard : FlatStyle.Standard;
                }
                // [...] outros controles específicos

                // Processar filhos de forma segura
                foreach (Control child in control.Controls)
                {
                    SafeApplyThemeToControl(child);
                }
            }
            finally
            {
                control.Tag = null;
            }
        }

        public static void ApplyDarkTheme(Form form)
    {
        form.BackColor = _darkBackColor;
        form.ForeColor = _darkForeColor;

        foreach (Control control in form.Controls)
        {
            ApplyDarkThemeToControl(control);
        }
    }

    public static void ApplyLightTheme(Form form)
    {
        form.BackColor = _lightBackColor;
        form.ForeColor = _lightForeColor;

        foreach (Control control in form.Controls)
        {
            ApplyLightThemeToControl(control);
        }
    }
        public static void ApplyThemeToDataGridView(DataGridView dgv, bool darkMode)
        {
            if (darkMode)
            {
                dgv.BackgroundColor = Color.FromArgb(45, 45, 48);
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
                dgv.DefaultCellStyle.ForeColor = Color.White;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(63, 63, 70);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 63, 70);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgv.EnableHeadersVisualStyles = false;
                dgv.GridColor = Color.FromArgb(80, 80, 80);
                dgv.BorderStyle = BorderStyle.Fixed3D;
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                dgv.BackgroundColor = SystemColors.Window;
                dgv.DefaultCellStyle.BackColor = SystemColors.Window;
                dgv.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                dgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgv.EnableHeadersVisualStyles = true;
                dgv.GridColor = SystemColors.ControlDark;
                dgv.BorderStyle = BorderStyle.Fixed3D;
                dgv.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dgv.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            }
        }

        public static void ApplyDarkThemeToControl(Control control)  // Mude "private" para "public"
        {
            if (control == null || control.Tag?.ToString() == "ThemeApplied")
                return;

            try
            {
                control.Tag = "ThemeApplied"; // Marca o controle como processado

                control.BackColor = _darkBackColor;
                control.ForeColor = _darkForeColor;

                if (control is Button button)
                {
                    button.BackColor = Color.FromArgb(63, 63, 70);
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Standard;
                    button.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = _darkTextBoxBackColor;
                    textBox.ForeColor = Color.White;
                    textBox.BorderStyle = BorderStyle.FixedSingle;

                    // Verifica se é um TextBox com placeholder
                    if (textBox.Tag?.ToString() == "HasPlaceholder")
                    {
                        if (textBox.Text == textBox.Tag.ToString().Replace("HasPlaceholder_", ""))
                        {
                            textBox.ForeColor = _darkTextBoxPlaceholderColor;
                        }
                    }
                }
                else if (control is ComboBox comboBox)
        {
            comboBox.BackColor = Color.FromArgb(37, 37, 38);
            comboBox.ForeColor = Color.White;
            comboBox.FlatStyle = FlatStyle.Standard;
        }
        else if (control is ListBox listBox)
        {
            listBox.BackColor = Color.FromArgb(37, 37, 38);
            listBox.ForeColor = Color.White;
            listBox.BorderStyle = BorderStyle.Fixed3D;
        }
        else if (control is CheckedListBox checkedListBox)
        {
            checkedListBox.BackColor = Color.FromArgb(37, 37, 38);
            checkedListBox.ForeColor = Color.White;
            checkedListBox.BorderStyle = BorderStyle.Fixed3D;
        }
        else if (control is Label label)
        {
            label.ForeColor = Color.White;
        }
        else if (control is GroupBox groupBox)
        {
            groupBox.ForeColor = Color.White;
            foreach (Control subControl in groupBox.Controls)
            {
                ApplyDarkThemeToControl(subControl);
            }
        }
        else if (control is Panel panel)
        {
            panel.BackColor = Color.FromArgb(45, 45, 48);
            foreach (Control subControl in panel.Controls)
            {
                ApplyDarkThemeToControl(subControl);
            }
        }
                        
        else if (control is ToolStrip toolStrip)
        {
            toolStrip.BackColor = Color.FromArgb(45, 45, 48);
            toolStrip.ForeColor = Color.White;
            toolStrip.Renderer = new DarkToolStripRenderer();
        }
                else if (control is MenuStrip menuStrip)
                {
                    menuStrip.BackColor = Color.FromArgb(45, 45, 48); // Corrigido para a cor escura
                    menuStrip.ForeColor = Color.White;
                    menuStrip.Renderer = new DarkMenuStripRenderer();
                }


                if (control is PdfRenderer pdfRenderer)
                {
                    // Tentativa de ajustar o PDF viewer
                    try
                    {
                        control.BackColor = _darkBackColor;
                        var toolStrip = control.Controls.OfType<ToolStrip>().FirstOrDefault();
                        if (toolStrip != null)
                        {
                            toolStrip.BackColor = Color.FromArgb(63, 63, 70);
                            toolStrip.ForeColor = Color.White;
                        }
                    }
                    catch { }
                }
                else if (control is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.FromArgb(45, 45, 48);
                    dgv.DefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
                    dgv.DefaultCellStyle.ForeColor = Color.White;
                    dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(63, 63, 70);
                    dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 63, 70);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.GridColor = Color.FromArgb(80, 80, 80);
                    dgv.BorderStyle = BorderStyle.Fixed3D;
                    dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                    dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                }

                // Aplica recursivamente a todos os controles filhos
                foreach (Control childControl in control.Controls)
                {
                    ApplyDarkThemeToControl(childControl);
                }

            }
            finally
            {
                control.Tag = null; // Remove a marca após processar
            }
        }



        private static void ApplyLightThemeToControl(Control control)
    {
        control.BackColor = _lightBackColor;
        control.ForeColor = _lightForeColor;

        if (control is Button button)
        {
            button.BackColor = SystemColors.ControlLightLight;
            button.ForeColor = SystemColors.ControlText;
            button.FlatStyle = FlatStyle.Standard;
        }
        else if (control is TextBox textBox)
        {
            textBox.BackColor = SystemColors.Window;
            textBox.ForeColor = SystemColors.WindowText;
            textBox.BorderStyle = BorderStyle.Fixed3D;
        }
        else if (control is ComboBox comboBox)
        {
            comboBox.BackColor = SystemColors.Window;
            comboBox.ForeColor = SystemColors.WindowText;
            comboBox.FlatStyle = FlatStyle.Standard;
        }
        else if (control is ListBox listBox)
        {
            listBox.BackColor = SystemColors.Window;
            listBox.ForeColor = SystemColors.WindowText;
            listBox.BorderStyle = BorderStyle.Fixed3D;
        }
        else if (control is CheckedListBox checkedListBox)
        {
            checkedListBox.BackColor = SystemColors.Window;
            checkedListBox.ForeColor = SystemColors.WindowText;
            checkedListBox.BorderStyle = BorderStyle.Fixed3D;
        }
        else if (control is Label label)
        {
            label.ForeColor = SystemColors.ControlText;
        }
        else if (control is GroupBox groupBox)
        {
            groupBox.ForeColor = SystemColors.ControlText;
            foreach (Control subControl in groupBox.Controls)
            {
                ApplyLightThemeToControl(subControl);
            }
        }
        else if (control is Panel panel)
        {
            panel.BackColor = SystemColors.Control;
            foreach (Control subControl in panel.Controls)
            {
                ApplyLightThemeToControl(subControl);
            }
        }
        else if (control is ToolStrip toolStrip)
        {
            toolStrip.BackColor = Color.FromArgb(63, 63, 70);
            toolStrip.ForeColor = Color.White;
            toolStrip.Renderer = new ToolStripProfessionalRenderer();
        }
            else if (control is DataGridView dgv)
            {
                dgv.BackgroundColor = SystemColors.Window;
                dgv.DefaultCellStyle.BackColor = SystemColors.Window;
                dgv.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                dgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgv.EnableHeadersVisualStyles = true;
                dgv.GridColor = SystemColors.ControlDark;
                dgv.BorderStyle = BorderStyle.Fixed3D;
                dgv.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                dgv.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            }

            // Aplica recursivamente a todos os controles filhos
            foreach (Control childControl in control.Controls)
        {
            ApplyLightThemeToControl(childControl);
        }

    }

        public static void ApplyThemeToControl(Control control)
        {
            if (IsDarkMode)
                ApplyDarkThemeToControl(control);
            else
                ApplyLightThemeToControl(control);
        }

        public static Image GetButtonImage(string baseName, bool darkMode)
    {
        try
        {
            var resourceManager = new System.Resources.ResourceManager("DocsViewer.Properties.Resources",
                System.Reflection.Assembly.GetExecutingAssembly());
            var resourceName = darkMode ? $"{baseName}_dark" : $"{baseName}_light";
            var image = (Image)resourceManager.GetObject(resourceName);
            return image ?? GetFallbackIcon();
        }
        catch
        {
            return GetFallbackIcon();
        }
    }
    private static Image GetFallbackIcon()
    {
        // Ícone padrão caso o recurso não seja encontrado
        try
        {
            return SystemIcons.Information.ToBitmap();
        }
        catch
        {
            // Fallback extremo - cria um bitmap simples
            var bmp = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Red); // Cor visível para indicar falta de ícone
            }
            return bmp;
        }
    }
    private static void UpdateAllForms()
    {
        foreach (Form form in Application.OpenForms)
        {
            ApplyTheme(form);
        }
        foreach (var tip in _registeredToolTips)
        {
           ApplyToolTipTheme(tip);
        }
    }

        public class DarkMenuStripRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuStripRenderer() : base(new DarkColorTable()) { }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = Color.White;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb  (45, 45, 48)), //(63, 63, 70)),
                                          e.Item.ContentRectangle);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
        

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                // Garante que a seta do submenu seja branca
                e.ArrowColor = Color.White;
                base.OnRenderArrow(e);
            }
        }

        public static void ApplyToolTipTheme(ToolTip toolTip)
        {
            if (toolTip == null)
                return;

            if (!_registeredToolTips.Contains(toolTip))
                _registeredToolTips.Add(toolTip);

            toolTip.OwnerDraw = true;
            toolTip.BackColor = _isDarkMode ? SystemColors.GrayText : Color.WhiteSmoke;
            toolTip.ForeColor = _isDarkMode ? Color.White : Color.Black;

            toolTip.Popup -= ToolTip_Popup;
            toolTip.Draw -= ToolTip_Draw;
            toolTip.Popup += ToolTip_Popup;
            toolTip.Draw += ToolTip_Draw;
        }

        private static void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            if (sender is ToolTip toolTip)
            {
                Font font = new Font("Segoe UI", 9, FontStyle.Regular);
                string text = toolTip.GetToolTip(e.AssociatedControl);
                Size size = TextRenderer.MeasureText(text, font);
                e.ToolTipSize = new Size(size.Width + 4, size.Height + 4);
                // 2. Definir a posição personalizada
                Point desiredPosition = CalculateToolTipPosition(e.AssociatedControl, e.ToolTipSize);
               
            }
        }

        private static Point CalculateToolTipPosition(Control associatedControl, Size tooltipSize)
        {
            // Exemplo 1: Mostrar acima do controle centralizado
            //return new Point(
            //associatedControl.Left + (associatedControl.Width - tooltipSize.Width) / 2,
            //associatedControl.Top - tooltipSize.Height - 5
            //);

            // Exemplo 2: Mostrar à direita do controle alinhado ao topo
            //return new Point(
            //associatedControl.Right + (associatedControl.Width - tooltipSize.Width) / + 20,
            //associatedControl.Left + 10
            //);

            // Exemplo 3: Posição relativa ao cursor
            return Control.MousePosition + new Size(10, 20); // Offset do cursor
        }

        private static void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (sender is ToolTip toolTip)
            {
                Font font = new Font("Segoe UI", 9, FontStyle.Regular);

                // Fundo e borda
                e.Graphics.FillRectangle(new SolidBrush(toolTip.BackColor), e.Bounds);
                e.Graphics.DrawRectangle(Pens.Gray, e.Bounds);

                // Texto
                TextRenderer.DrawText(
                    e.Graphics,
                    e.ToolTipText,
                    font,
                    e.Bounds,
                    toolTip.ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                );
            }
        }
        public static Font ToolTipFont { get; set; } = new Font("Segoe UI", 10, FontStyle.Bold);


        private class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer() : base(new DarkColorTable()) { }
    }

        private class DarkColorTable : ProfessionalColorTable
    {

         public override Color MenuItemPressedGradientBegin => Color.FromArgb(45, 45, 48);
         public override Color MenuItemPressedGradientEnd => Color.FromArgb(45, 45, 48);
         public override Color MenuItemSelected => Color.FromArgb(63, 63, 70);
         public override Color MenuItemBorder => Color.FromArgb(80, 80, 80);
        public override Color ToolStripBorder => Color.FromArgb(80, 80, 80);
        public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 48);
        public override Color ToolStripGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color ToolStripGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color ToolStripGradientMiddle => Color.FromArgb(45, 45, 48);
        public override Color ToolStripContentPanelGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color ToolStripContentPanelGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color ToolStripPanelGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color ToolStripPanelGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color MenuBorder => Color.FromArgb(80, 80, 80);
        public override Color MenuStripGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color MenuStripGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color ImageMarginGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(45, 45, 48);
        public override Color ImageMarginGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(63, 63, 70);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(63, 63, 70); 
        }
}
    }