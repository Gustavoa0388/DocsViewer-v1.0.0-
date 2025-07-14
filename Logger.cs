using System;
using System.IO;
using System.Windows.Forms;

namespace DocsViewer
{
    public static class Logger
    {
        private static string GetLogFilePath()
        {
            string databasePath = AppConfig.GetDatabasePath();
            return Path.Combine(databasePath, "log.txt");
        }

        public static void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(GetLogFilePath(), true))
                {
                    writer.WriteLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                // Se der erro no log, tenta mostrar mensagem
                MessageBox.Show($"Failed to write log: {ex.Message}", "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}