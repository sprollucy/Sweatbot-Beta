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

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Load settings from file
            SettingsManager.LoadSettings();

            // Attach event handler to detect setting changes
            Properties.Settings.Default.PropertyChanged += (sender, e) =>
            {
                // Ensure the setting being changed is of type boolean
                PropertyInfo prop = Properties.Settings.Default.GetType().GetProperty(e.PropertyName);
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    if (Properties.Settings.Default.isDebugOn)
                    {
                        Console.WriteLine($"Setting changed: {e.PropertyName}");
                        SettingsManager.SaveSettings(); // Save settings after change
                    }
                    else
                    {
                        SettingsManager.SaveSettings(); // Save settings after change
                    }
                }
            };

            // Run the main form (or your application's main UI)
            Application.Run(new ModernMenu());
        }
    }
}
