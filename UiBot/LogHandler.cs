using Newtonsoft.Json;

namespace UiBot
{
    internal class LogHandler
    {

        // SemaphoreSlim to control access to the file
        private static SemaphoreSlim fileLock = new SemaphoreSlim(1, 1);

        public static async Task LogBits(string userName, int bits, string timestamp)
        {
            string logMessage = $"{timestamp} - {userName} gave {bits} bits";
            string date = DateTime.Now.ToString("M-d-yy");
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Acquire the semaphore before writing
            await fileLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(logFilePath, logMessage + Environment.NewLine);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public static async Task LogCommand(string userName, string command, int bitsCost, Dictionary<string, int> userBits, string timestamp)
        {
            string logMessage;
            if (userBits.ContainsKey(userName))
            {
                int bitsBeforeCommand = userBits[userName];
                int bitsAfterCommand = bitsBeforeCommand - bitsCost;
                logMessage = $"{timestamp} - {userName} had {bitsBeforeCommand} bits, used {command} command, costing {bitsCost} bits, now has {bitsAfterCommand} bits";
            }
            else
            {
                logMessage = $"{timestamp} - {userName} used {command} command, costing {bitsCost} bits";
            }

            string date = DateTime.Now.ToString("M-d-yy");
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Acquire the semaphore before writing
            await fileLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(logFilePath, logMessage + Environment.NewLine);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public static async Task LogAddbits(string commandUser, string command, int bitsAdded, string targetUser, Dictionary<string, int> userBits, string timestamp)
        {
            int currentTotalBits = userBits.ContainsKey(targetUser) ? userBits[targetUser] : 0;
            string logMessage = $"{timestamp} - {commandUser} used {command} command, added {bitsAdded} bits to {targetUser}, who had {currentTotalBits - bitsAdded} bits, now has {currentTotalBits} bits";
            string date = DateTime.Now.ToString("M-d-yy");
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Acquire the semaphore before writing
            await fileLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(logFilePath, logMessage + Environment.NewLine);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public static async Task LogRemovebits(string commandUser, string command, int bitsRemove, string targetUser, Dictionary<string, int> userBits, string timestamp)
        {
            int currentTotalBits = userBits.ContainsKey(targetUser) ? userBits[targetUser] : 0;
            string logMessage = $"{timestamp} - {commandUser} used {command} command, removed {bitsRemove} bits from {targetUser}, who had {currentTotalBits - bitsRemove} bits, now has {currentTotalBits} bits";
            string date = DateTime.Now.ToString("M-d-yy");
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Acquire the semaphore before writing
            await fileLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(logFilePath, logMessage + Environment.NewLine);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public static async Task FileBackup()
        {
            string[] jsonFilePaths = { Path.Combine("Data", "user_bits.json") };
            string timestamp = DateTime.Now.ToString("MM_dd_HH_mm");
            string backupDirectory = Path.Combine("Backup");

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            Console.WriteLine($"Backup triggered at {DateTime.Now}");

            try
            {
                for (int i = 0; i < jsonFilePaths.Length; i++)
                {
                    string backupFilePath = Path.Combine(backupDirectory, $"{Path.GetFileNameWithoutExtension(jsonFilePaths[i])}_backup_{timestamp}{Path.GetExtension(jsonFilePaths[i])}");

                    // Acquire the semaphore before reading and writing backup
                    await fileLock.WaitAsync();
                    try
                    {
                        string json = await File.ReadAllTextAsync(jsonFilePaths[i]);
                        await File.WriteAllTextAsync(backupFilePath, json);
                    }
                    finally
                    {
                        fileLock.Release();
                    }

                    Console.WriteLine($"Backup completed for {jsonFilePaths[i]} at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during backup: {ex.Message}");
            }
        }

        public static async Task LoadUserBitsFromJson(string fileName)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Data directory not found: {directoryPath}");
                MainBot.userBits = new Dictionary<string, int>();
                return;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine("User bits JSON file not found.");
                MainBot.userBits = new Dictionary<string, int>();
                return;
            }

            string json;
            // Acquire the semaphore before reading
            await fileLock.WaitAsync();
            try
            {
                json = await File.ReadAllTextAsync(filePath);
            }
            finally
            {
                fileLock.Release();
            }

            MainBot.userBits = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }

        public static async Task WriteUserBitsToJson(string fileName)
        {
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dataDirectory);
            string filePath = Path.Combine(dataDirectory, fileName);

            foreach (var user in MainBot.userBits)
            {
                if (user.Value < 0)
                {
                    MainBot.userBits[user.Key] = 0;
                    Console.WriteLine($"Negative bits detected for user {user.Key}. Set to 0.");
                }
            }

            string json = JsonConvert.SerializeObject(MainBot.userBits, Formatting.Indented);

            try
            {
                // Acquire the semaphore before writing
                await fileLock.WaitAsync();
                try
                {
                    await File.WriteAllTextAsync(filePath, json);
                }
                finally
                {
                    fileLock.Release();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing JSON file: {ex.Message}");
            }
        }

        public static async Task UpdateUserBits(string username, int bitsGiven)
        {
            if (MainBot.userBits.ContainsKey(username))
            {
                MainBot.userBits[username] += bitsGiven;
            }
            else
            {
                MainBot.userBits.Add(username, bitsGiven);
            }

            await WriteUserBitsToJson("user_bits.json");
        }

        public static async Task UpdateUserBitsRemoved(string username, int bitsGiven)
        {
            if (MainBot.userBits.ContainsKey(username))
            {
                MainBot.userBits[username] -= bitsGiven;
            }
            else
            {
                MainBot.userBits.Add(username, bitsGiven);
            }

            await WriteUserBitsToJson("user_bits.json");
        }
        public static void DebugToFile()
        {

        }

        //Mod Whitelist

        public static Dictionary<string, HashSet<string>> modWhitelist;

        public static void LoadWhitelist()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ModWhitelist.txt");
                string[] lines = File.ReadAllLines(filePath);
                modWhitelist = new Dictionary<string, HashSet<string>>();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(':'); // Format: username:refund,give,add_remove
                    if (parts.Length == 2)
                    {
                        string username = parts[0].Trim();
                        string[] perms = parts[1].Split(',');

                        modWhitelist[username] = new HashSet<string>(perms.Select(p => p.Trim()), StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, read errors)
                modWhitelist = new Dictionary<string, HashSet<string>>();
            }
        }

        public static bool HasPermission(string username, string permission)
        {
            return modWhitelist.TryGetValue(username, out var permissions) && permissions.Contains(permission);
        }


    }
}
