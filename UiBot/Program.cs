using System;
using System.Reflection;

namespace UiBot
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SettingsManager.LoadSettings();
            // Ensure necessary directories exist by calling the IntegrityCheck method
            IntegrityCheck.EnsureDirectoriesExist();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Run the main form (or your application's main UI)
            Application.Run(new ModernMenu());
        }
    }
}
