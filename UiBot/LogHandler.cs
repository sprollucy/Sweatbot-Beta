using Newtonsoft.Json;

namespace UiBot
{
    internal class LogHandler
    {

        //Log given bits from chat
        public static void LogBits(string userName, int bits, string timestamp)
        {
            string logMessage = $"{timestamp} - {userName} gave {bits} bits";

            // Get the current date for the filename
            string date = DateTime.Now.ToString("M-d-yy");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        public static void LogCommand(string userName, string command, int bitsCost, Dictionary<string, int> userBits, string timestamp)
        {
            // Check if the user's bits information is available in the dictionary
            if (userBits.ContainsKey(userName))
            {
                int bitsBeforeCommand = userBits[userName];

                // Deduct the cost of the command from the user's bits
                int bitsAfterCommand = bitsBeforeCommand - bitsCost;

                // Create the log message
                string logMessage = $"{timestamp} - {userName} had {bitsBeforeCommand} bits, used {command} command, costing {bitsCost} bits, now has {bitsAfterCommand} bits";

                // Get the current date for the filename
                string date = DateTime.Now.ToString("M-d-yy");

                // Construct the log file path with the date in its name
                string logFileName = $"{date} bitlog.txt";
                string logFilePath = Path.Combine("Logs", logFileName);

                // Append the log message to the file
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            else
            {
                // If user's bits information is not available, log without the bit count information
                string logMessage = $"{timestamp} - {userName} used {command} command, costing {bitsCost} bits";

                // Get the current date for the filename
                string date = DateTime.Now.ToString("M-d-yy");

                // Construct the log file path with the date in its name
                string logFileName = $"{date} bitlog.txt";
                string logFilePath = Path.Combine("Logs", logFileName);

                // Append the log message to the file
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
        }

        public static void LogAddbits(string commandUser, string command, int bitsAdded, string targetUser, Dictionary<string, int> userBits, string timestamp)
        {
            // Get the current total bits of the target user
            int currentTotalBits = userBits.ContainsKey(targetUser) ? userBits[targetUser] : 0;

            // Create the log message showing the current total bits, bits added, and the new total
            string logMessage = $"{timestamp} - {commandUser} used {command} command, added {bitsAdded} bits to {targetUser}, who had {currentTotalBits - bitsAdded} bits, now has {currentTotalBits} bits";

            // Get the current date for the filename
            string date = DateTime.Now.ToString("M-d-yy");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }


        public static void LogRefundbits(string commandUser, string refundUsername, Dictionary<string, int> userBits, string timestamp)
        {
            // Get the current total bits of the target user
            int currentTotalBits = userBits.ContainsKey(refundUsername) ? userBits[refundUsername] : 0;

            // Create the log message showing the current total bits, bits added, and the new total
            string logMessage = $"{timestamp} - {commandUser} refunded bits to {refundUsername}, now has {currentTotalBits} bits";

            // Get the current date for the filename
            string date = DateTime.Now.ToString("M-d-yy");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        public static void FileBackup()
        {
            // Define the list of file paths to backup
            string[] jsonFilePaths = { Path.Combine("Data", "user_bits.json"), Path.Combine("Data", "CommandConfigData.json"), Path.Combine("Data", "DropPositionData.json") };

            // Define the list of backup file paths
            string[] backupFilePaths = { Path.Combine("Backup", "user_bits_backup.txt"), Path.Combine("Backup", "CommandConfigData_backup.txt"), Path.Combine("Backup", "DropPositionData_backup.txt") };

            Console.WriteLine($"Backup triggered at {DateTime.Now}");

            try
            {
                for (int i = 0; i < jsonFilePaths.Length; i++)
                {
                    // Read the JSON content
                    string json = File.ReadAllText(jsonFilePaths[i]);

                    // Write JSON content to the backup file
                    File.WriteAllText(backupFilePaths[i], json);

                    Console.WriteLine($"Backup completed for {jsonFilePaths[i]} at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during backup: {ex.Message}");
            }
        }

        public static void LoadUserBitsFromJson(string fileName)
        {
            // Construct the file path to the "Data" folder
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string filePath = Path.Combine(directoryPath, fileName);

            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Data directory not found: {directoryPath}");
                MainBot.userBits = new Dictionary<string, int>();
                return;
            }

            // Check if the JSON file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("User bits JSON file not found.");
                MainBot.userBits = new Dictionary<string, int>();
                return;
            }

            // Deserialize JSON file to dictionary
            string json = File.ReadAllText(filePath);
            MainBot.userBits = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }

        public static void WriteUserBitsToJson(string fileName)
        {
            // Construct the file path to the JSON file in the "Data" folder
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dataDirectory); // Ensure the directory exists
            string filePath = Path.Combine(dataDirectory, fileName);

            // Validate bits to ensure they are not negative
            foreach (var user in MainBot.userBits)
            {
                if (user.Value < 0)
                {
                    // Handle negative bits (e.g., set to zero or log the issue)
                    MainBot.userBits[user.Key] = 0;
                    Console.WriteLine($"Negative bits detected for user {user.Key}. Set to 0.");
                }
            }

            // Serialize dictionary to JSON
            string json = JsonConvert.SerializeObject(MainBot.userBits, Formatting.Indented);

            try
            {
                // Write JSON to file
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing JSON file: {ex.Message}");
            }
        }

        public static void UpdateUserBits(string username, int bitsGiven)
        {
            if (MainBot.userBits.ContainsKey(username))
            {
                MainBot.userBits[username] += bitsGiven;
            }
            else
            {
                MainBot.userBits.Add(username, bitsGiven);
            }

            LogHandler.WriteUserBitsToJson("user_bits.json");
        }

        //Mod Whitelist

        private static HashSet<string> modWhitelist;

        public static void LoadWhitelist()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ModWhitelist.txt");
                string[] lines = File.ReadAllLines(filePath);
                modWhitelist = new HashSet<string>(lines, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, read errors)
                modWhitelist = new HashSet<string>();
            }
        }

        public static bool IsUserInWhitelist(string username)
        {
            return modWhitelist.Contains(username);
        }

    }
}
