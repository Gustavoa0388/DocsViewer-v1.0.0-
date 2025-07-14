using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DocsViewer
{
    public static class LogoHelper
    {
        public static void AplicarLogoComoIcon(Form form)
        {
            var logoConfig = LogoConfig.LoadLogoConfig();
            if (logoConfig != null && !string.IsNullOrEmpty(logoConfig.LogoBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(logoConfig.LogoBase64);
                using (var ms = new MemoryStream(imageBytes))
                {
                    using (var bmp = new Bitmap(ms))
                    {
                        try
                        {
                            form.Icon = Icon.FromHandle(bmp.GetHicon());
                        }
                        catch { }
                    }
                }
            }
        }

    }
}
