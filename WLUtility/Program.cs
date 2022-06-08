using System;
using System.Threading;
using System.Windows.Forms;

namespace WLUtility
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var mutex = new Mutex(true, Application.ProductName, out var ret);
            if (ret)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
                mutex.ReleaseMutex();
            }
            else
            {
                Application.Exit();  
            }
        }
    }
}
