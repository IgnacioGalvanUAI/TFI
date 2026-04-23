using System;
using System.Windows.Forms;
using BarberiaTurnos.UI;

namespace BarberiaTurnos
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
