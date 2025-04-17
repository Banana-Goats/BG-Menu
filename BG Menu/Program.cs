using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BG_Menu.Class.Sales_Summary;
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

            // Create a mutex to ensure only one instance of the application runs
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Application is already running.");
                return;
            }

            string hanaClientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HANA_Client_Dlls");
            string currentPath = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", $"{hanaClientPath};{currentPath}");

            string appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            Directory.SetCurrentDirectory(appDirectory);

 
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //GlobalInstances.InitializeAsync().GetAwaiter().GetResult();
            //GlobalInstances.TryLoadSalesDataAsync().GetAwaiter().GetResult();

            // Start the application
            Application.Run(new Login());
        }
    }
}
