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
            // Ensure necessary directories exist by calling the IntegrityCheck method
            IntegrityCheck.EnsureDirectoriesExist();
            SettingsManager.LoadSettings();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Load settings from file



            // Run the main form (or your application's main UI)
            Application.Run(new ModernMenu());
        }
    }
}
