using System;
using System.IO;

namespace UiBot
{
    public static class IntegrityCheck
    {
        // Method to create the required directories and ensure the user_bits.json file exists
        public static void EnsureDirectoriesExist()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Define the required directories
            string[] directories = new string[]
            {
                Path.Combine(appDirectory, "Backup"),
                Path.Combine(appDirectory, "Logs"),
                Path.Combine(appDirectory, "Sound Clips"),
                Path.Combine(appDirectory, "Sounds"),
                Path.Combine(appDirectory, "Data", "bin"),
                Path.Combine(appDirectory, "Data", "Profiles")
            };

            // Check if each directory exists, if not, create it
            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    Console.WriteLine($"Created directory: {dir}");
                }
            }

            // Ensure the user_bits.json file exists in the Data directory
            string userBitsFile = Path.Combine(appDirectory, "Data", "user_bits.json");
            if (!File.Exists(userBitsFile))
            {
                // Create an empty JSON object and write it to the file
                File.WriteAllText(userBitsFile, "{}");
                Console.WriteLine($"Created missing file: {userBitsFile}");
            }

            // Check if CustomCommands.json exists in the Data\bin directory and move it to Data\Profiles
            string customCommandsFile = Path.Combine(appDirectory, "Data", "bin", "CustomCommands.json");
            if (File.Exists(customCommandsFile))
            {
                string targetFile = Path.Combine(appDirectory, "Data", "Profiles", "CustomCommands.json");
                try
                {
                    // Move the file from bin to Profiles
                    File.Move(customCommandsFile, targetFile);
                    Console.WriteLine($"Moved CustomCommands.json from 'bin' to 'Profiles'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving CustomCommands.json: {ex.Message}");
                }
            }

            // Ensure the SendKeyExclude file exists in the Data directory
            string sendKeyExcludeFile = Path.Combine(appDirectory, "Data", "SendKeyExclude.txt");
            if (!File.Exists(sendKeyExcludeFile))
            {
                string defaultContent = "";
                File.WriteAllText(sendKeyExcludeFile, defaultContent);
                Console.WriteLine($"Created missing file: {sendKeyExcludeFile}");
            }

            // Ensure the ModWhitelist file exists in the Data directory
            string modWhitelistFile = Path.Combine(appDirectory, "Data", "ModWhitelist.txt");
            if (!File.Exists(modWhitelistFile))
            {
                string defaultContent = "Example:refund,give,remove,add_remove_command"; 
                File.WriteAllText(modWhitelistFile, defaultContent);
                Console.WriteLine($"Created missing file: {modWhitelistFile}");
            }
        }
    }
}
