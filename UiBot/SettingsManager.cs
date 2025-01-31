using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UiBot
{
    public static class SettingsManager
    {
        private static readonly string settingsFilePath = Path.Combine("Data", "bin", "settings.json");

        public static void SaveSettings()
        {
            try
            {
                // Get all boolean properties dynamically
                var boolSettings = Properties.Settings.Default.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof(bool))
                    .ToDictionary(p => p.Name, p => (bool)p.GetValue(Properties.Settings.Default));

                // Convert to JSON and save
                string json = JsonSerializer.Serialize(boolSettings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(settingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(settingsFilePath))
                {
                    string json = File.ReadAllText(settingsFilePath);
                    var loadedSettings = JsonSerializer.Deserialize<Dictionary<string, bool>>(json);

                    if (loadedSettings != null)
                    {
                        foreach (var setting in loadedSettings)
                        {
                            PropertyInfo prop = Properties.Settings.Default.GetType().GetProperty(setting.Key);
                            if (prop != null && prop.PropertyType == typeof(bool))
                            {
                                prop.SetValue(Properties.Settings.Default, setting.Value);
                            }
                        }

                        Console.WriteLine("Settings loaded successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Settings file not found. Using default values.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

    }
}
