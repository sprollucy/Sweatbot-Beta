using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace UiBot
{
    public static class SettingsManager
    {
        private static readonly string settingsFilePath = Path.Combine("Data", "bin", "settings.json");

        public static void SaveSettings()
        {
            try
            {
                // Get all boolean and string properties dynamically
                var settings = Properties.Settings.Default.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof(bool) || p.PropertyType == typeof(string))
                    .ToDictionary(p => p.Name, p =>
                    {
                        if (p.PropertyType == typeof(bool))
                        {
                            return (object)(bool)p.GetValue(Properties.Settings.Default);
                        }
                        else
                        {
                            return (object)(string)p.GetValue(Properties.Settings.Default);
                        }
                    });

                // Convert to JSON and save
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
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
                    var loadedSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    if (loadedSettings != null)
                    {
                        foreach (var setting in loadedSettings)
                        {
                            PropertyInfo prop = Properties.Settings.Default.GetType().GetProperty(setting.Key);
                            if (prop != null)
                            {
                                // Check if the property is a boolean or string and set the value accordingly
                                if (prop.PropertyType == typeof(bool) && setting.Value is bool)
                                {
                                    prop.SetValue(Properties.Settings.Default, setting.Value);
                                }
                                else if (prop.PropertyType == typeof(string) && setting.Value is string)
                                {
                                    prop.SetValue(Properties.Settings.Default, setting.Value);
                                }
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
