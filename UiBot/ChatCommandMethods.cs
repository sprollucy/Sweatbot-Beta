using Newtonsoft.Json;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using TwitchLib.Client;
using TwitchLib.Client.Events;
/* TODO **

 */

namespace UiBot
{
    public class CounterData
    {
        public string ChannelName { get; set; }
        public string BotToken { get; set; }
    }

    public class ConfigData
    {
        public string bonusMultiplierBox { get; set; }
        public string subbonusMultiplierBox { get; set; }
    }

    internal class ChatCommandMethods
    {
        private static string userBitName;

        private static string banListFilePath = Path.Combine("Data", "BanList.json");

        private static readonly object fileLock = new object();

        //command dictionary
        public Dictionary<string, string> commandConfigData;
        public static Dictionary<string, int> userBits = new Dictionary<string, int>();

        ControlMenu controlMenu = new ControlMenu();

        // Handles connection
        TwitchClient client;
        public static string channelId;

        public Random random = new Random();

        //spam command
        public DateTime lastStatCommandTimer = DateTime.MinValue;
        public DateTime lastWipeStatCommandTimer = DateTime.MinValue;
        public DateTime lastHelpCommandTimer = DateTime.MinValue;
        public DateTime lastAboutCommandTimer = DateTime.MinValue;
        public DateTime lastBitcostCommandTimer = DateTime.MinValue;
        public DateTime lastHow2useTimer = DateTime.MinValue;


        public static int BitBonusMultiplier { get; private set; }
        public static int SubBonusMultiplier { get; private set; }

        internal ChatCommandMethods()
        {
            userBitName = controlMenu.CustombitnameBox.Text;
        }


        public static void BitMultiplier()
        {
            string bonusMultiplierBox;
            string configFilePath = Path.Combine("Data", "bin", "CommandConfigData.json");

            try
            {
                // Lock the file access section
                lock (fileLock)
                {
                    // Read the JSON file and parse it to extract the multiplier
                    string json = File.ReadAllText(configFilePath);
                    var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                    bonusMultiplierBox = configData?.bonusMultiplierBox;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(bonusMultiplierBox))
            {
                Console.WriteLine("No multiplier set.");
                return;
            }

            if (!int.TryParse(bonusMultiplierBox, out int durationSeconds))
            {
                Console.WriteLine("Invalid multiplier duration format.");
                return;
            }

            BitBonusMultiplier = durationSeconds;
        }

        public static void SubBitMultiplier()
        {
            string subbonusMultiplierBox;
            string configFilePath = Path.Combine("Data", "bin", "CommandConfigData.json");

            try
            {
                // Lock the file access section
                lock (fileLock)
                {
                    // Read the JSON file and parse it to extract the multiplier
                    string json = File.ReadAllText(configFilePath);
                    var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                    subbonusMultiplierBox = configData?.subbonusMultiplierBox;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(subbonusMultiplierBox))
            {
                Console.WriteLine("No Sub multiplier set.");
                return;
            }

            if (!int.TryParse(subbonusMultiplierBox, out int durationSeconds))
            {
                Console.WriteLine("Invalid multiplier duration format.");
                return;
            }

            SubBonusMultiplier = durationSeconds;
        }

        //Mod Commands

        public static void RefundLastCommand(OnChatCommandReceivedArgs e, string userName, Dictionary<string, int> userBits, TwitchClient client, string channelId)
        {
            lock (fileLock)
            {
                // Get the current date and timestamp for the log
                string date = DateTime.Now.ToString("M-d-yy");
                string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

                // Construct the log file path with the date in its name
                string logFileName = $"{date} bitlog.txt";
                string logFilePath = Path.Combine("Logs", logFileName);

                if (!File.Exists(logFilePath))
                {
                    Console.WriteLine("No log file found for today.");
                    return;
                }

                // Read all lines from the log file
                List<string> logLines = File.ReadAllLines(logFilePath).ToList();

                // Find the last relevant log line for the specified user (ignore refunded lines)
                string lastUserLogLine = logLines.LastOrDefault(line =>
                    line.Contains(userName) &&
                    line.Contains("had") &&
                    !line.Contains("refunded"));

                if (string.IsNullOrEmpty(lastUserLogLine))
                {
                    Console.WriteLine($"No valid log found for user {userName}.");
                    return;
                }

                // Extract information from the last log line for the user
                string[] parts = lastUserLogLine.Split(new[] { " - ", " had ", " bits, used ", " command, costing ", " bits, now has " }, StringSplitOptions.None);

                if (parts.Length < 6)
                {
                    Console.WriteLine("Log format is invalid.");
                    return;
                }

                string logUserName = parts[1];  // This is the userName from the log line
                int bitsCost = int.Parse(parts[4]);  // The amount of bits that were used (which will be refunded)

                // Refund the bits to the user
                if (userBits.ContainsKey(logUserName))
                {
                    userBits[logUserName] += bitsCost; // Add the refunded bits to the current balance

                    // Log the refund operation
                    int currentTotalBits = userBits[logUserName];
                    string logMessage = $"{timestamp} - {e.Command.ChatMessage.DisplayName} refunded {bitsCost} bits to {logUserName}, now has {currentTotalBits} bits";

                    // Remove the last log entry for the user from the list
                    logLines.Remove(lastUserLogLine);  // Remove the entry from the list

                    // Ensure the Logs directory exists
                    if (!Directory.Exists("Logs"))
                    {
                        Directory.CreateDirectory("Logs");
                    }

                    // Write the updated log file without the refunded entry
                    File.WriteAllLines(logFilePath, logLines);

                    // Append the refund log message to the file
                    File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

                    // Output to console
                    Console.WriteLine($"[{timestamp}] {e.Command.ChatMessage.DisplayName} refunded {bitsCost} bits to {logUserName}. They now have {userBits[logUserName]} bits.");

                    // Send message to chat
                    client.SendMessage(channelId, $"{bitsCost} {userBitName} were refunded to {logUserName}. They now have {userBits[logUserName]} {userBitName}.");
                    Task.Delay(500).ContinueWith(t =>
                    {
                        ConnectMenu.Instance.LoadLogEntries(); // Reload log entries from the updated file
                    });
                }
                else
                {
                    Console.WriteLine("User not found in the bits dictionary.");
                }
            }
        }

        public static void AddBitCommand(TwitchClient client, OnChatCommandReceivedArgs e)
        {
            string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

            string[] args = e.Command.ArgumentsAsString.Split(' ');
            if (args.Length == 2)
            {
                string username = args[0].StartsWith("@") ? args[0].Substring(1) : args[0]; // Remove "@" symbol if present
                int bitsToAdd;
                if (int.TryParse(args[1], out bitsToAdd))
                {
                    // Update user's bits
                    LogHandler.UpdateUserBits(username, bitsToAdd);

                    // Lock the log file to avoid concurrency issues when writing to it
                    lock (fileLock)
                    {
                        LogHandler.LogAddbits(e.Command.ChatMessage.DisplayName, "addbits", bitsToAdd, username, MainBot.userBits, timestamp);
                    }

                    // Notify about successful update
                    client.SendMessage(e.Command.ChatMessage.Channel, $"{bitsToAdd} {userBitName} added to {username}. New total: {MainBot.userBits[username]} {userBitName}");
                    Console.WriteLine($"[{timestamp}] User [{e.Command.ChatMessage.DisplayName}] added {bitsToAdd} bits to {username}. New total: {MainBot.userBits[username]} bits");

                }
                else
                {
                    client.SendMessage(e.Command.ChatMessage.Channel, $"Invalid number of {userBitName} specified.");
                }
            }
        }

        public static void RemoveBitCommand(TwitchClient client, OnChatCommandReceivedArgs e)
        {
            string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

            string[] args = e.Command.ArgumentsAsString.Split(' ');
            if (args.Length == 2)
            {
                string username = args[0].StartsWith("@") ? args[0].Substring(1) : args[0]; // Remove "@" symbol if present
                int bitsToRemove;
                if (int.TryParse(args[1], out bitsToRemove))
                {
                    // Update user's bits
                    LogHandler.UpdateUserBitsRemoved(username, bitsToRemove);

                    // Lock the log file to avoid concurrency issues when writing to it
                    lock (fileLock)
                    {
                        LogHandler.LogRemovebits(e.Command.ChatMessage.DisplayName, "rembits", bitsToRemove, username, MainBot.userBits, timestamp);
                    }

                    // Notify about successful update
                    client.SendMessage(e.Command.ChatMessage.Channel, $"{bitsToRemove} {userBitName} removed from {username}. New total: {MainBot.userBits[username]} {userBitName}");
                    Console.WriteLine($"[{timestamp}] User [{e.Command.ChatMessage.DisplayName}] removed {bitsToRemove} bits to {username}. New total: {MainBot.userBits[username]} bits");

                }
                else
                {
                    client.SendMessage(e.Command.ChatMessage.Channel, $"Invalid number of {userBitName} specified.");
                }
            }
        }

        public static HashSet<string> LoadBanList()
        {
            if (File.Exists(banListFilePath))
            {
                string json = File.ReadAllText(banListFilePath);
                return JsonConvert.DeserializeObject<HashSet<string>>(json) ?? new HashSet<string>();
            }
            else
            {
                return new HashSet<string>();
            }
        }

        // Saves the banned users to the BanList.json file
        public static void SaveBanList(HashSet<string> banList)
        {
            string json = JsonConvert.SerializeObject(banList, Formatting.Indented);
            File.WriteAllText(banListFilePath, json);
        }
    }
}
