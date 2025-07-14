using System;
using System.Threading;
using System.Windows.Forms;


namespace DocsViewer 
{
    static class Program
    {

        private static Mutex mutex = null;

        [STAThread]

        static void Main()
        {
            const string mutexName = "DocsViewerMutex";
            mutex = new Mutex(true, mutexName, out bool createdNew);

            if (createdNew)
            {
                // (Seu código atual permanece aqui)
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (sender, args) =>
                {
                    MessageBox.Show($"Erro não tratado: {args.Exception.Message}");
                    Logger.Log($"Unhandled UI exception: {args.Exception.ToString()}");
                };

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    var ex = (Exception)args.ExceptionObject;
                    MessageBox.Show($"Erro fatal: {ex.Message}");
                    Logger.Log($"Fatal error: {ex.ToString()}");
                };

                try
                {
                    Application.Run(new LoginForm());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao iniciar aplicação: {ex.Message}");
                    Logger.Log($"Application start failed: {ex.ToString()}");
                }
            }
            else
            {
                // NOVO: Avisa e fecha se tentar abrir de novo!
                MessageBox.Show("O DocsViewer já está aberto nesta máquina!", "Já em execução", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0); // Fecha o app imediatamente
            }
        }

    }
}