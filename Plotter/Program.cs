using System;
using System.Windows.Forms;

namespace Plotter
{
    // Class contains the Main method that defines the entry point for the program
    // The Main method creates and runs the MainForm class to start the application
    internal static class Program
    {
        // The main entry point for the application
        // Creates and runs the MainForm class

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}