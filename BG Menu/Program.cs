using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BG_Menu.Forms.Sub_Forms;

namespace BG_Menu
{
    internal static class Program
    {
        private static Mutex mutex = null;

        [STAThread]
        static void Main()
        {
            const string appName = "BG Menu";
            bool createdNew;

            // Create a mutex and check if it's already existing
            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // Application is already running, so we exit.
                MessageBox.Show("Application is already running.");
                return;
            }

            // Set the current working directory to the directory where the executable is located
            string appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            Directory.SetCurrentDirectory(appDirectory);

            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
