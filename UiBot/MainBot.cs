
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub;
using TwitchLib.Api.Helix.Models.Chat.GetChatters;
using UiBot.Properties;
using System.Text.RegularExpressions;
using System.Media;


namespace UiBot
{

    internal class MainBot : IDisposable
    {
        // References
        ChatCommandMethods chatCommandMethods = new ChatCommandMethods();
        ControlMenu controlMenu = new ControlMenu();
        private static CustomCommandHandler commandHandler;
        public static TwitchPubSub PubSub;

        // Dictionary 
        public static Dictionary<string, int> userBits = new Dictionary<string, int>();
        private Dictionary<string, DateTime> lastGambleTime = new Dictionary<string, DateTime>();
        private Dictionary<string, DateTime> lastExecutionTimes = new Dictionary<string, DateTime>();

        private static HashSet<string> bannedUsers = ChatCommandMethods.LoadBanList();  // Load the banned users on startup
        public Dictionary<string, string> commandConfigData;
        Dictionary<string, (int usageCount, int bitCost, int totalSpent)> commandUsageData = new Dictionary<string, (int, int, int)>();


        // Keyboard Events
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        // Handles connection
        private ConnectionCredentials creds;
        TwitchClient client;
        public bool isBotConnected = false;
        private static string channelId;
        string userBitName;

        // Property to get the connection status
        public bool IsConnected => isBotConnected;
        private bool creatorMessage = false;
        // spam command
        private DateTime lastTradersCommandTimer = DateTime.MinValue;
        private System.Threading.Timer timer;
        public int autoSendMessageCD;

        // Logging timestamp
        string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

        // File loading
        string commandsFilePath;
        string userBitsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "user_bits.json");

        internal MainBot()
        {
            commandHandler = new CustomCommandHandler(commandsFilePath);
            userBitName = controlMenu.CustombitnameBox.Text;
            string LastUsedProfile = Settings.Default.LastUsedProfile;
            // Construct the file path using string interpolation
            commandsFilePath = Path.Combine("Data", "Profiles", $"{LastUsedProfile}.json");
            LoadCredentialsFromJSON();
            LogHandler.LoadWhitelist();
            LogHandler.LoadUserBitsFromJson(userBitsFilePath);

        }

        public void Dispose()
        {
            // Dispose of any resources here

            Disconnect();
        }

        public void Disconnect()
        {
            if (isBotConnected)
            {
                // Disconnect and clean up resources here
                Console.WriteLine("[Sweatbot]: Disconnected");

                client.Disconnect();

                // Unsubscribe from events
                client.OnConnected -= Client_OnConnected;
                client.OnError -= Client_OnError;

                isBotConnected = false;
            }
            else
            {
                Console.WriteLine("[Sweatbot]: Not connected");
            }
        }

        internal void Connect()
        {
            if (!isBotConnected)
            {
                Console.WriteLine($"[Sweatbot]: Connecting to {channelId}...");
                InitializeTwitchClient();
                InitializePubSub();
                StartAutoMessage();
                isBotConnected = true;

            }
        }

        private void InitializeTwitchClient()
        {
            if (!isBotConnected)
            {
                if (string.IsNullOrEmpty(Settings.Default.AccessToken) || string.IsNullOrEmpty(channelId))
                {
                    MessageBox.Show("Please enter token access and channel name in the Settings Menu");
                    Console.WriteLine("[Sweatbot]: Disconnected");
                    return;
                }
                if (creds == null)
                {
                    MessageBox.Show("Twitch credentials are not set.");
                    Console.WriteLine("[Sweatbot]: Disconnected");
                    return;
                }

                if (channelId == null)
                {
                    MessageBox.Show("Twitch channel are not set.");
                    Console.WriteLine("[Sweatbot]: Disconnected");
                    return;
                }

                try
                {
                    client = new TwitchClient();
                    client.Initialize(creds, channelId);

                    client.OnNewSubscriber += Client_OnNewSubscriber;

                    client.OnConnected += Client_OnConnected;
                    client.OnError += Client_OnError;
                    client.OnMessageReceived += Client_OnMessageReceived;
                    client.OnChatCommandReceived += Client_OnChatCommandReceived;

                    client.AddChatCommandIdentifier('$');
                    client.OnConnected += (sender, e) => isBotConnected = true;
                    client.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during Twitch client initialization: {ex.Message}");
                }
            }
        }
        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("[Sweatbot]: Connected");
        }

        public void SendMessage(string message)
        {
            if (client != null && client.IsConnected)
            {
                // Send the message to the specified channel
                client.SendMessage(channelId, message);
            }
            else
            {
                Console.WriteLine("Sweatbot is not connected to Twitch. Cannot send message.");
            }
        }

        private void InitializePubSub()
        {
            if (PubSub == null)
            {
                PubSub = new TwitchPubSub();
                PubSub.OnFollow += PubSub_OnFollow;
                PubSub.Connect();
            }
        }

        private void PubSub_OnFollow(object sender, OnFollowArgs e)
        {
            if (Settings.Default.isFollowBonusEnabled)
            {
                string followerName = e.Username;
                int bitsAwarded = 1; // Default value

                // Try to get the value from the FollowTextBox, similar to how you handle ChatBonus
                if (int.TryParse(controlMenu.FollowTextBox.Text, out int followBonus))
                {
                    bitsAwarded = followBonus;  // Update bitsAwarded with the value from FollowTextBox
                }
                else
                {
                    // Handle error if the value from the FollowTextBox is invalid
                    client.SendMessage(channelId, $"{followerName}, invalid follow bonus amount. Using default value.");
                    bitsAwarded = 1;  // Default value in case of invalid input
                }

                // Check if the user already exists in the bits dictionary
                if (!userBits.ContainsKey(followerName))
                {
                    userBits[followerName] = bitsAwarded;
                }
                else
                {
                    userBits[followerName] += bitsAwarded;
                }

                // Save updated bits data to the file
                LogHandler.WriteUserBitsToJson("user_bits.json");

                // Log the follow event and awarded bits
                LogHandler.LogBits(followerName, bitsAwarded, timestamp);

                // Send a thank-you message to the chat
                client.SendMessage(channelId, $"{followerName} is now following! You have been awarded {bitsAwarded} {userBitName}. You now have {userBits[followerName]} {userBitName}.");

                // Optionally print to console
                Console.WriteLine($"[{timestamp}] {followerName} followed and was awarded {bitsAwarded} bits. Total bits: {userBits[followerName]}");
            }
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (Settings.Default.isSubBonusEnabled)
            {
                string subscriberName = e.Subscriber.DisplayName;
                int bitsAwarded = 1; // Default value

                // Try to get the value from the SubTextBox, similar to how you handle FollowTextBox
                if (int.TryParse(controlMenu.SubTextBox.Text, out int subBonus))
                {
                    bitsAwarded = subBonus;  // Update bitsAwarded with the value from SubTextBox
                }
                else
                {
                    // Handle error if the value from the SubTextBox is invalid
                    client.SendMessage(channelId, $"{subscriberName}, invalid subscription bonus amount. Using default value.");
                    bitsAwarded = 1;  // Default value in case of invalid input
                }

                // Check if the user already exists in the bits dictionary
                if (!userBits.ContainsKey(subscriberName))
                {
                    userBits[subscriberName] = bitsAwarded;
                }
                else
                {
                    userBits[subscriberName] += bitsAwarded;
                }

                // Save updated bits data to the file
                LogHandler.WriteUserBitsToJson("user_bits.json");

                // Log the subscription and awarded bits
                LogHandler.LogBits(subscriberName, bitsAwarded, timestamp);

                // Send a thank-you message to the chat
                client.SendMessage(channelId, $"{subscriberName}, thank you for subscribing! You have been awarded {bitsAwarded} {userBitName}. You now have {userBits[subscriberName]} {userBitName}.");

                // Optionally print to console
                Console.WriteLine($"[{timestamp}] {subscriberName} subscribed and was awarded {bitsAwarded} bits. Total bits: {userBits[subscriberName]}");
            }
        }

        public void ProcessLocalCommand(string commandName)
        {
            int userBits = 100000; // Simulate user bits

            if (commandHandler.CanExecuteCommand(commandName, userBits))
            {
                var command = commandHandler.GetCommand(commandName);
                if (command != null)
                {
                    // Simulate command execution
                    commandHandler.ExecuteCommandAsync(commandName, null, "testChannel"); // Channel is not used here
                    Console.WriteLine($"Command executed locally: {commandName}");
                }
            }
            else
            {
                Console.WriteLine($"Cannot execute command '{commandName}'. Command does not exist or is broken.");
            }
        }

        public void PrintSortedCommandUsageCounts()
        {
            var sortedCommands = commandUsageData.OrderByDescending(entry => entry.Value.usageCount);

            Console.WriteLine("Popularity========================================================");

            int index = 1;

            foreach (var entry in sortedCommands)
            {
                Console.WriteLine($" {index}. {entry.Key,-20} Used: {entry.Value.usageCount,-5} Total Spent: {entry.Value.totalSpent}");

                index++;
            }
            Console.WriteLine("===============================================================");
        }

        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            DateTime currentTime = DateTime.Now;
            bool isSubscriber = e.ChatMessage.IsSubscriber;
            bool isSubOnly = Settings.Default.isSubOnlyBotCommand;

            if (e.ChatMessage.DisplayName.Equals("Sprollucy") && !creatorMessage && Settings.Default.isEasterEgg)
            {
                try
                {
                    // Path to Tada.wav sound file (Windows default location)
                    string soundFilePath = @"C:\Windows\Media\Tada.wav";

                    // Load and play the sound
                    using (SoundPlayer player = new SoundPlayer(soundFilePath))
                    {
                        player.Play();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing sound: {ex.Message}");
                }
                creatorMessage = true;
                //Thanks ace
                client.SendMessage(channelId, $"Father is here!");
            }

            if (Settings.Default.isRateDelayEnabled && e.ChatMessage.Message.StartsWith("!"))
            {
                int delayS;
                if (!int.TryParse(controlMenu.RateDelayBox.Text, out delayS))
                {
                    delayS = 1;
                }

                if (lastExecutionTimes.ContainsKey(e.ChatMessage.DisplayName))
                {
                    TimeSpan timeDifference = currentTime - lastExecutionTimes[e.ChatMessage.DisplayName];

                    if (timeDifference < TimeSpan.FromSeconds(delayS))
                    {
                        int remainingDelayS = (int)(delayS - timeDifference.TotalSeconds);
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, You can use another command in {remainingDelayS} seconds!");
                        await Task.Delay(remainingDelayS);
                    }
                }

                lastExecutionTimes[e.ChatMessage.DisplayName] = currentTime;
            }

            if (bannedUsers.Contains(e.ChatMessage.DisplayName))
            {
                client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, you are banned from Sweatbot!");
                return;
            }

            if (Settings.Default.isPausedMessage)
            {
                if (Settings.Default.isCommandsPaused)
                {
                    client.SendMessage(channelId, "Commands are currently paused");
                    return;
                }
            }

            if (isSubOnly && !isSubscriber)
            {
                client.SendMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName}, Sorry! Sweatbot is for subscribers only!");
                return;
            }

            if (!Settings.Default.isCommandsPaused)
            {
                if (!Settings.Default.isStoreCurrency)
                {
                    // If currency is disabled, skip this block
                    return;
                }

                commandHandler.ReloadCommands(commandsFilePath);

                // Check if the message starts with "!" (for regular commands)
                if (e.ChatMessage.Message.StartsWith("!"))
                {
                    string commandName = e.ChatMessage.Message.TrimStart('!').ToLower();

                    // Check if the command is enabled
                    if (commandHandler.GetCommand(commandName) != null)
                    {
                        var command = commandHandler.GetCommand(commandName);

                        // Process other commands
                        int userBits = MainBot.userBits.ContainsKey(e.ChatMessage.DisplayName)
                            ? MainBot.userBits[e.ChatMessage.DisplayName]
                            : 0;

                        if (commandHandler.CanExecuteCommand(commandName, userBits))
                        {
                            try
                            {
                                await commandHandler.ExecuteCommandAsync(commandName, client, e.ChatMessage.Channel);

                                if (!commandUsageData.ContainsKey(commandName))
                                {
                                    commandUsageData[commandName] = (0, command.BitCost, 0);
                                }

                                var commandData = commandUsageData[commandName];
                                commandData.usageCount++;
                                commandData.totalSpent += command.BitCost;
                                commandUsageData[commandName] = commandData;

                                // Log the command execution
                                LogHandler.LogCommand(e.ChatMessage.DisplayName, commandName, command.BitCost, MainBot.userBits, timestamp);

                                // Deduct the bit cost of the command
                                MainBot.userBits[e.ChatMessage.DisplayName] -= command.BitCost;

                                // Save the updated bit data
                                LogHandler.WriteUserBitsToJson(userBitsFilePath);

                                string message = "";

                                // Only add remaining bits info if BitCost is greater than 0
                                if (command.BitCost > 0 && Settings.Default.isBitMsgEnabled)
                                {
                                    message += $" You have {MainBot.userBits[e.ChatMessage.DisplayName]} {userBitName} remaining.";
                                }

                                // Inform the user that the command was executed
                                client.SendMessage(channelId, message);

                                // Log command details to the console
                                Console.WriteLine($"[{timestamp}] [{e.ChatMessage.DisplayName}]: {commandName} Cost: {command.BitCost} Remaining {userBitName}: {MainBot.userBits[e.ChatMessage.DisplayName]}");
                            }
                            catch (Exception ex)
                            {
                                // Log and inform the user of the error
                                Console.WriteLine($"Error executing command '{commandName}': {ex.Message}");
                                client.SendMessage(channelId, $"An error occurred while executing the command '{commandName}'. Please try again later.");
                            }
                        }
                        else
                        {
                            // Inform the user they do not have enough bits
                            client.SendMessage(channelId, $"You don't have enough {userBitName} to execute the command '{commandName}'.");
                        }
                    }
                }

                // Check if the message contains a cheer amount for bit-based commands
                // Add the check here to prevent processing cheers if the message starts with "!"
                if (e.ChatMessage.Message.StartsWith("!") && e.ChatMessage.Bits > 0)
                {
                    int cheerAmount = e.ChatMessage.Bits;

                    // Get all commands with bit costs
                    var commandsWithCosts = commandHandler.GetAllCommandsWithCosts();

                    // Find commands that match the cheer amount
                    var matchingCommands = commandsWithCosts.Where(kv => kv.Value == cheerAmount).ToList();

                    // If matching commands are found, execute them
                    if (matchingCommands.Any())
                    {
                        foreach (var matchingCommand in matchingCommands)
                        {
                            string commandName = matchingCommand.Key.ToLower();  // Get the command name

                            // Check if the user has enough bits, if not, add the missing bits
                            if (MainBot.userBits.ContainsKey(e.ChatMessage.DisplayName))
                            {
                                int currentBits = MainBot.userBits[e.ChatMessage.DisplayName];
                                int bitsRequired = cheerAmount;

                                // Execute the command since user now has enough bits
                                try
                                {
                                    await commandHandler.ExecuteCommandAsync(commandName, client, e.ChatMessage.Channel);

                                    if (!commandUsageData.ContainsKey(commandName))
                                    {
                                        commandUsageData[commandName] = (0, cheerAmount, 0);
                                    }

                                    var commandData = commandUsageData[commandName];
                                    commandData.usageCount++;
                                    commandData.totalSpent += cheerAmount;
                                    commandUsageData[commandName] = commandData;


                                    // Log the command execution
                                    LogHandler.LogCommand(e.ChatMessage.DisplayName, commandName, cheerAmount, MainBot.userBits, timestamp);

                                    // Send message with remaining bits
                                    string message = $"{e.ChatMessage.DisplayName}: {commandName} Cost: {cheerAmount}";
                                    client.SendMessage(channelId, message);

                                    // Log command details to the console
                                    Console.WriteLine($"[{timestamp}] [{e.ChatMessage.DisplayName}]: {commandName} Cost: {cheerAmount}");
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    // Log and inform the user of the error
                                    Console.WriteLine($"Error executing command '{commandName}': {ex.Message}");
                                    client.SendMessage(channelId, $"An error occurred while executing the command '{commandName}'. Please try again later.");
                                }
                            }
                            else
                            {
                                // No matching commands found, add the cheer amount to user's bit balance
                                if (MainBot.userBits.ContainsKey(e.ChatMessage.DisplayName))
                                {
                                    // Add cheer amount to the user's current bit balance
                                    MainBot.userBits[e.ChatMessage.DisplayName] += cheerAmount;

                                    // Log the addition of bits
                                    LogHandler.LogCommand(e.ChatMessage.DisplayName, "Added to balance", cheerAmount, MainBot.userBits, timestamp);
                                    client.SendMessage(channelId, $"No command found for {cheerAmount} {userBitName}. Your balance has been updated by {cheerAmount} {userBitName}.");
                                    LogHandler.WriteUserBitsToJson(userBitsFilePath);
                                    return;

                                }
                                else
                                {
                                    // If the user doesn't have any bits, initialize their balance
                                    MainBot.userBits[e.ChatMessage.DisplayName] = cheerAmount;

                                    // Log the addition of bits
                                    LogHandler.LogCommand(e.ChatMessage.DisplayName, "Initialized balance", cheerAmount, MainBot.userBits, timestamp);
                                    client.SendMessage(channelId, $"Your balance has been initialized with {cheerAmount} {userBitName}.");
                                    return;

                                }
                            }
                        }
                    }
                }
            }

            // Given Bits
            if (e.ChatMessage.Bits > 0)
            {
                int bitsGiven = e.ChatMessage.Bits;
                bool bonusApplied = false;

                if (Settings.Default.isBonusMultiplierEnabled && !(Settings.Default.isSubBonusMultiEnabled && isSubscriber) && Settings.Default.isStoreCurrency)
                {
                    // Apply normal Bonus Multiplier only if subscriber bonus is not enabled
                    ChatCommandMethods.BitMultiplier();
                    int bonusMultiplierPercentage = ChatCommandMethods.BitBonusMultiplier;
                    double bonusMultiplier = 1 + (bonusMultiplierPercentage / 100.0);
                    bitsGiven = (int)Math.Ceiling(bitsGiven * bonusMultiplier);

                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    Console.WriteLine($"A {bonusMultiplierPercentage}% Multiplier is active, resulting in {bitsGiven} bits given.");
                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {e.ChatMessage.Bits} {userBitName}! Multiplier is active so it counts as {bitsGiven} {userBitName}. You now have {userBits[e.ChatMessage.DisplayName]} {userBitName}.");

                    bonusApplied = true;
                }
                else if (Settings.Default.isSubBonusMultiEnabled && isSubscriber)
                {
                    // Apply Sub Bonus Multiplier
                    ChatCommandMethods.SubBitMultiplier();
                    int subMultiplierPercentage = ChatCommandMethods.SubBonusMultiplier;
                    double subMultiplier = 1 + (subMultiplierPercentage / 100.0);
                    bitsGiven = (int)Math.Ceiling(bitsGiven * subMultiplier);

                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    Console.WriteLine($"A {subMultiplierPercentage}% Sub Multiplier is active, resulting in {bitsGiven} bits given.");
                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {e.ChatMessage.Bits} {userBitName}! Sub Multiplier is active so it counts as {bitsGiven} {userBitName}. You now have {userBits[e.ChatMessage.DisplayName]} {userBitName}.");

                    bonusApplied = true;
                }

                if (!bonusApplied)
                {
                    // No multiplier applied, log and thank the user normally
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {bitsGiven} {userBitName}! You now have {userBits[e.ChatMessage.DisplayName]} {userBitName}.");
                }
            }

            if (Settings.Default.isChatBonusEnabled && Settings.Default.isStoreCurrency)
            {
                if (!userBits.ContainsKey(e.ChatMessage.DisplayName))
                {
                    int bonusAmount;
                    if (int.TryParse(controlMenu.BonusTextBox.Text, out bonusAmount))
                    {
                        // User is chatting for the first time, give them the specified bonus amount
                        userBits.Add(e.ChatMessage.DisplayName, bonusAmount);
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName} welcome to the stream! Here is {bonusAmount} {userBitName} on the house, use !help for more info");
                        Console.WriteLine($"[{e.ChatMessage.DisplayName}]: First Chat Bonus: {bonusAmount}");

                        LogHandler.WriteUserBitsToJson("user_bits.json");
                        LogHandler.LogBits(e.ChatMessage.DisplayName, bonusAmount, timestamp);
                    }
                    else
                    {
                        // Error parsing bonus amount, use a default value or handle the error accordingly
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, invalid bonus amount format. Using default value.");
                        userBits.Add(e.ChatMessage.DisplayName, bonusAmount);
                        LogHandler.WriteUserBitsToJson("user_bits.json");
                    }
                }
            }

            MessageIntegrations(e);
        }

        private void MessageIntegrations(OnMessageReceivedArgs e)
        {
            string message = e.ChatMessage.Message;

            if (Settings.Default.isStoreCurrency)
            {
                if (Settings.Default.isblerpEnabled)
                {
                    if (e.ChatMessage.DisplayName.Equals("blerp", StringComparison.OrdinalIgnoreCase))
                    {
                        string pattern = @"^(\S+)\s+used\s+(\d+)\s+bits";
                        Match match = Regex.Match(message, pattern);
                        if (match.Success)
                        {
                            string user = match.Groups[1].Value;
                            int bitsUsed = int.Parse(match.Groups[2].Value);

                            int percentage = int.TryParse(controlMenu.BlerpReturnBox.Text, out int blerpPercentage) ? blerpPercentage : 100;
                            int adjustedBits = (bitsUsed * percentage) / 100;

                            if (!userBits.ContainsKey(user))
                            {
                                userBits[user] = adjustedBits;
                            }
                            else
                            {
                                userBits[user] += adjustedBits;
                            }

                            client.SendMessage(channelId, $"{user}, you've received {adjustedBits} {userBitName} for using blerp! Your total is now {userBits[user]}.");
                            Console.WriteLine($"Blerp Bonus for [{user}]: Used {bitsUsed} bits, Adjusted: {adjustedBits}. Total: {userBits[user]}");

                            LogHandler.WriteUserBitsToJson("user_bits.json");
                            LogHandler.LogBits(user, adjustedBits, timestamp);
                        }
                        else
                        {
                            Console.WriteLine("No match found in the message. Expected '{User} used {bits} in blerp message");
                        }
                    }
                }

                if (Settings.Default.isSoundAlertsEnabled)
                {
                    if (e.ChatMessage.DisplayName.Equals("SoundAlerts", StringComparison.OrdinalIgnoreCase))
                    {
                        string pattern = @"^(\S+)\s+played\s+.*!\s+for\s+(\d+)\s+Bits";
                        Match match = Regex.Match(message, pattern);

                        if (match.Success)
                        {
                            string user = match.Groups[1].Value;
                            int bitsUsed = int.Parse(match.Groups[2].Value);

                            if (bitsUsed > 1)
                            {
                                int percentage = int.TryParse(controlMenu.SoundAlertsReturnBox.Text, out int soundAlertPercentage) ? soundAlertPercentage : 100;
                                int adjustedBits = (bitsUsed * percentage) / 100;

                                if (!userBits.ContainsKey(user))
                                {
                                    userBits[user] = adjustedBits;
                                }
                                else
                                {
                                    userBits[user] += adjustedBits;
                                }

                                client.SendMessage(channelId, $"{user}, you've received {adjustedBits} {userBitName} using SoundAlerts! Your total is now {userBits[user]}.");
                                Console.WriteLine($"SoundAlerts Bonus for [{user}]: Used {bitsUsed} bits, Adjusted: {adjustedBits}. Total: {userBits[user]}");

                                LogHandler.WriteUserBitsToJson("user_bits.json");
                                LogHandler.LogBits(user, adjustedBits, timestamp);
                            }
                            else
                            {
                                Console.WriteLine($"{user} played a sound alert for {bitsUsed} bits, but only amounts over 1 bit count.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No match found in the message. Expected format: '{User} played {Sound}! for {bits} Bits'");
                        }
                    }
                }

                if (Settings.Default.isTangiaEnabled)
                {
                    if (e.ChatMessage.DisplayName.Equals("TangiaBot", StringComparison.OrdinalIgnoreCase))
                    {
                        string pattern = @"^(\S+)";
                        Match match = Regex.Match(message, pattern);

                        if (match.Success)
                        {
                            string user = match.Groups[1].Value; // Extract username

                            // Attempt to parse the bits used
                            int bitsUsed = 0;
                            if (int.TryParse(controlMenu.TangiaTextBox.Text, out bitsUsed))
                            {
                                if (!userBits.ContainsKey(user))
                                {
                                    userBits[user] = bitsUsed; // Initialize the user bits
                                }
                                else
                                {
                                    userBits[user] += bitsUsed; // Add bits if the user already exists in the dictionary
                                }

                                // Send the message to the channel
                                client.SendMessage(channelId, $"{user}, you've received {bitsUsed} {userBitName} for using Tangia! Your total is now {userBits[user]}.");

                                Console.WriteLine($"Tangia Bonus for [{user}]: Used {bitsUsed} bits. Total: {userBits[user]}");

                                // Log the data to the JSON file
                                LogHandler.WriteUserBitsToJson("user_bits.json");

                                // Log the individual transaction
                                LogHandler.LogBits(user, bitsUsed, timestamp);
                            }
                            else
                            {
                                Console.WriteLine("Invalid bits value in TangiaBox.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No match found in the message.");
                        }
                    }
                }
            }
        }

        //Chat 
        private async void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var traderResetInfoService = new TraderResetInfoService();
            string Chatter = e.Command.ChatMessage.DisplayName;

            //antispam cooldowns
            int helpCooldownDuration = 15;
            int aboutCooldownDuration = 30;
            int tradersCooldownDuration = 90;
            int bitcostCooldownDuration = 15;
            int lastHow2useTimerDuration = 15;
            TimeSpan timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastStatCommandTimer;
            bool isSubscriber = e.Command.ChatMessage.IsSubscriber;

            //Normal Commands
            switch (e.Command.CommandText.ToLower())
            {
                case "help":

                    if (timeSinceLastExecution.TotalSeconds >= helpCooldownDuration)
                    {
                        chatCommandMethods.lastHelpCommandTimer = DateTime.Now;

                        StringBuilder message = new StringBuilder();
                        message.Append("!how2use, !about");

                        if (Settings.Default.isStoreCurrency)
                        {
                            message.Append(", !mybits");
                        }

                        if (Settings.Default.isTradersEnabled)
                        {
                            message.Append(", !traders");
                        }

                        if (Settings.Default.isBitCostEnabled && Settings.Default.isStoreCurrency)
                        {
                            message.Append(", !bitcost");
                        }

                        if (Settings.Default.isBitGambleEnabled && Settings.Default.isStoreCurrency)
                        {
                            message.Append(", !sbgamble");
                        }

                        if (e.Command.ChatMessage.IsModerator)
                        {
                            if (LogHandler.HasPermission(Chatter, "give"))
                            {
                                message.Append(", !addbits");
                            }
                            if (LogHandler.HasPermission(Chatter, "remove"))
                            {
                                message.Append(", !rembits");
                            }

                            if (LogHandler.HasPermission(Chatter, "refund"))
                            {
                                message.Append(", !refund");
                            }

                            if (LogHandler.HasPermission(Chatter, "add_remove_command"))
                            {
                                message.Append(", !sbadd, !sbremove, !cdebug");
                            }

                            if (LogHandler.HasPermission(Chatter, "ban"))
                            {
                                message.Append(", !sbban, !sbunban");
                            }
                        }

                        if (e.Command.ChatMessage.IsBroadcaster)
                        {
                            message.Append(", !addbits, !refund, !rembits, !sbadd, !sbremove, !cdebug, !sbban, !sbunban");
                        }

                        client.SendMessage(channelId, message.ToString());
                    }
                    break;

                case "how2use":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastHow2useTimer;

                    if (timeSinceLastExecution.TotalSeconds >= lastHow2useTimerDuration && Settings.Default.isStoreCurrency)
                    {
                        client.SendMessage(channelId, $"To use Sweatbot, simply cheer Bits in the chat, and the bot will track how many you've given or do ![cheeramount] to directly run a command that matches that amount. Use `!bitcost` to see a list of available commands and their costs. When you have enough {userBitName}, just type the command you want to use in the chat. You can also check your balance at any time with `!mybits`.");
                    }
                    else if (timeSinceLastExecution.TotalSeconds >= lastHow2useTimerDuration)
                    {
                        client.SendMessage(channelId, $"To use Sweatbot, simply put a '!' in front of your cheer amount like ![cheeramount] to directly run a command that matches that amount. Use `!bitcost` to see a list of available commands and their costs.");

                    }
                    break;

                case "about":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastAboutCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= aboutCooldownDuration)
                    {
                        chatCommandMethods.lastAboutCommandTimer = DateTime.Now;
                        client.SendMessage(channelId, $"I am a bot created by Sprollucy. This is a small project that was inspired by bitbot and to help practice my coding. Many features may be incomplete or missing at this time.");
                        client.SendMessage(channelId, $"If you want to learn more about this project, visit https://github.com/sprollucy/Sweatbot-Beta for more information, bug reporting, and suggestions");
                    }
                    break;

                case "mybits":
                    if (Settings.Default.isStoreCurrency)
                    {
                        if (userBits.ContainsKey(Chatter))
                        {
                            client.SendMessage(channelId, $"{Chatter}, you have {userBits[Chatter]} {userBitName}");
                            Console.WriteLine($"{Chatter}, you have {userBits[Chatter]} bits");
                        }
                        else
                        {
                            client.SendMessage(channelId, $"{Chatter}, you have no {userBitName}");
                            Console.WriteLine($"{Chatter}, you have no bits");
                        }
                    }
                    break;

                case "bitcost":
                    if (Settings.Default.isBitCostEnabled)
                    {
                        commandHandler.ReloadCommands(commandsFilePath);
                        timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastBitcostCommandTimer;

                        if (timeSinceLastExecution.TotalSeconds >= bitcostCooldownDuration)
                        {
                            chatCommandMethods.lastBitcostCommandTimer = DateTime.Now;

                            // Define the mappings of textbox names to their corresponding labels and enabled states
                            var textBoxDetails = new Dictionary<string, (string Label, Func<bool> IsEnabled)>
                        {
                                { "bottoggleCostBox", ("sweatbot", () => Settings.Default.isSweatbotEnabled) },
                                { "sendkeyCostBox", ("sendkey", () => Settings.Default.isSendKeyEnabled) }

                        };

                            // Define a list to hold enabled command details
                            List<(string Label, int Cost)> enabledCommandDetails = new List<(string Label, int Cost)>();

                            // Retrieve enabled command details and add them to the list
                            foreach (var detail in textBoxDetails)
                            {
                                if (detail.Value.IsEnabled())
                                {
                                    var textBox = controlMenu.Controls.Find(detail.Key, true).FirstOrDefault() as TextBox;
                                    if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))
                                    {
                                        string label = detail.Value.Label;
                                        int cost = 0;
                                        int.TryParse(textBox.Text, out cost); // Parsing cost as integer
                                        enabledCommandDetails.Add((label, cost));
                                    }
                                }
                            }

                            // Order the enabled commands by their costs from cheap to expensive
                            enabledCommandDetails.Sort((x, y) => x.Cost.CompareTo(y.Cost));

                            // Construct the message dynamically with ordered enabled commands
                            List<string> enabledCommandCosts = enabledCommandDetails.Select(detail => $"!{detail.Label}({detail.Cost})").ToList();

                            // Create messages based on the conditions
                            string standardCommandsMessage = string.Empty;
                            string customCommandsMessage = string.Empty;

                            if (!Settings.Default.isCommandsPaused)
                            {
                                if (enabledCommandCosts.Count > 0)
                                {
                                    standardCommandsMessage = $"{Chatter}, Available commands: {string.Join(", ", enabledCommandCosts)}";
                                }

                                var allCommandsWithCosts = commandHandler.GetAllCommandsWithCosts();
                                if (allCommandsWithCosts.Any())
                                {
                                    var commandList = allCommandsWithCosts
                                        .Select(cmd => $"!{cmd.Key}({cmd.Value})")
                                        .OrderBy(cmd => int.Parse(cmd.Split('(')[1].Trim(')'))) // Ensure commands are ordered by cost
                                        .ToList();

                                    customCommandsMessage = $"{string.Join(", ", commandList)}";
                                }
                            }
                            else
                            {
                                standardCommandsMessage = $"{Chatter}, All commands are paused";
                            }

                            // Combine the two messages into a single message
                            if (!string.IsNullOrEmpty(standardCommandsMessage) || !string.IsNullOrEmpty(customCommandsMessage))
                            {
                                // Combine the messages with a separator (e.g., " | ")
                                string combinedMessage = $"{standardCommandsMessage} {customCommandsMessage}".Trim(' ', '|');

                                // Send the combined message
                                client.SendMessage(channelId, combinedMessage);
                            }
                        }
                    }
                    break;

                case "traders":
                    if (Settings.Default.isTradersEnabled)
                    {
                        timeSinceLastExecution = DateTime.Now - lastTradersCommandTimer;

                        if (timeSinceLastExecution.TotalSeconds >= tradersCooldownDuration)
                        {
                            // Update the resetTime.json file with the latest reset info
                            await traderResetInfoService.GetAndSaveTraderResetInfoWithLatest();

                            // Read the reset time data from resetTime.json
                            var resetTimeData = traderResetInfoService.ReadJsonDataFromFile("Data/bin/resetTime.json");

                            if (!string.IsNullOrEmpty(resetTimeData))
                            {
                                // Deserialize the JSON data
                                var traderResetResponse = JsonConvert.DeserializeObject<TraderResetInfoService.TraderResetResponse>(resetTimeData);

                                if (traderResetResponse != null && traderResetResponse.Data != null && traderResetResponse.Data.Traders != null)
                                {
                                    // Define a dictionary for trader enable statuses
                                    var traderEnabledStatus = new Dictionary<string, bool>
                    {
                        { "Prapor", Settings.Default.isTraderPraporEnabled },
                        { "Therapist", Settings.Default.isTraderTherapistEnabled },
                        { "Mechanic", Settings.Default.isTraderMechanicEnabled },
                        { "Peacekeeper", Settings.Default.isTraderPeacekeeperEnabled },
                        { "Fence", Settings.Default.isTraderFenceEnabled },
                        { "Ragman", Settings.Default.isTraderRagmanEnabled },
                        { "Skier", Settings.Default.isTraderSkierEnabled },
                        { "Jaeger", Settings.Default.isTraderJaegerEnabled },
                        { "Lightkeeper", Settings.Default.isTraderLightkeeperEnabled }
                    };

                                    // Iterate through the traders
                                    foreach (var trader in traderResetResponse.Data.Traders)
                                    {
                                        string traderName = trader.Name;
                                        string resetTime = trader.ResetTime;

                                        // Check if the trader is enabled
                                        if (traderEnabledStatus.TryGetValue(traderName, out bool isEnabled) && isEnabled)
                                        {
                                            // Parse the reset time as a DateTime
                                            if (DateTime.TryParse(resetTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime resetDateTime))
                                            {
                                                // Get the local time zone
                                                TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

                                                // Convert the reset time from UTC to local time
                                                DateTime localResetTime = TimeZoneInfo.ConvertTimeFromUtc(resetDateTime, localTimeZone);

                                                // Calculate the time remaining until the reset time
                                                TimeSpan timeRemaining = localResetTime - DateTime.Now;

                                                // Check if the time remaining is negative
                                                if (timeRemaining < TimeSpan.Zero)
                                                {
                                                    // The reset time has passed; set the time remaining to zero
                                                    timeRemaining = TimeSpan.Zero;
                                                }

                                                // Format the time difference as hours and minutes
                                                string formattedTimeRemaining = $"{(int)timeRemaining.TotalHours} hours {timeRemaining.Minutes} minutes";

                                                // Send a separate alert if there are 5 minutes or less remaining
                                                if (timeRemaining <= TimeSpan.FromMinutes(5))
                                                {
                                                    client.SendMessage(channelId, $"@{channelId} {traderName} has 5 minutes or less remaining! Countdown: {formattedTimeRemaining}");
                                                }
                                                else
                                                {
                                                    // Send the regular countdown message
                                                    client.SendMessage(channelId, $"Trader Name: {traderName}, Countdown: {formattedTimeRemaining}");
                                                }
                                            }
                                            else
                                            {
                                                // Handle the case where the reset time cannot be parsed
                                                client.SendMessage(channelId, $"Failed to parse reset time for trader '{traderName}'.");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Handle the case where the JSON data is not as expected
                                    client.SendMessage(channelId, "Failed to fetch trader reset info.");
                                }
                            }
                            else
                            {
                                // Handle the case where resetTime.json is empty or not found
                                client.SendMessage(channelId, "Reset time data not available.");
                            }

                            // Update the last execution time for the "traders" command
                            lastTradersCommandTimer = DateTime.Now;
                        }
                    }
                    break;

                //Bit Commands

                case "sweatbot":
                    if (Settings.Default.isStoreCurrency)
                    {
                        // Get the current state of isChatCommandPaused
                        bool isPaused = Settings.Default.isCommandsPaused;
                        string currentState = isPaused ? "off" : "on";

                        // Get the bit cost from the settings
                        if (!int.TryParse(controlMenu.BotToggleCostBox.Text, out int bitCostBot))
                        {
                            client.SendMessage(channelId, "Error: Invalid cost value.");
                            break;
                        }

                        // Check if the command is sub-only
                        bool isSubOnly = Settings.Default.isSubOnlySweatbotCommand;

                        if (isSubOnly && !isSubscriber)
                        {
                            client.SendMessage(channelId, $"{Chatter}, this command is for Twitch subscribers only!");
                            break;
                        }

                        // If the command is just "!sweatbot", show the current state and cost
                        if (e.Command.ChatMessage.Message.Trim().ToLower() == "!sweatbot")
                        {
                            client.SendMessage(channelId, $"Sweatbot is currently {currentState}. You can change that with !sweatbot on or off for {bitCostBot} {userBitName}.");
                            break;
                        }

                        if (Settings.Default.isSweatbotEnabled)
                        {
                            // Check if the user's bits are loaded
                            if (userBits.ContainsKey(Chatter))
                            {
                                // Determine the desired state based on the command message
                                bool desiredState = e.Command.ChatMessage.Message.ToLower().Contains("on");

                                // If they are trying to set it to the same state, notify and do nothing
                                if ((desiredState && currentState == "on") || (!desiredState && currentState == "off"))
                                {
                                    client.SendMessage(channelId, $"{Chatter}, sweatbot is already {currentState}. No {userBitName} were taken.");
                                    break;
                                }

                                // Check if the user has enough bits
                                if (userBits[Chatter] >= bitCostBot)
                                {
                                    // Deduct the cost of the command
                                    userBits[Chatter] -= bitCostBot;

                                    // Update the isChatCommandPaused value
                                    Settings.Default.isCommandsPaused = !desiredState;

                                    // Log the command usage
                                    LogHandler.LogCommand(Chatter, "sweatbot", bitCostBot, userBits, timestamp);

                                    // Send confirmation message with the correct updated state
                                    if (Settings.Default.isBitMsgEnabled)
                                    {
                                        client.SendMessage(channelId, $"{Chatter}, sweatbot turned {(desiredState ? "on" : "off")}! You have {userBits[Chatter]} {userBitName} remaining.");
                                    }
                                    Console.WriteLine($"[{timestamp}] [{Chatter}]: {e.Command.ChatMessage.Message} Cost:{bitCostBot} Remaining bits:{userBits[Chatter]}");

                                    // Save the updated bit data
                                    LogHandler.WriteUserBitsToJson("user_bits.json");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{Chatter}, you don't have enough {userBitName} to use this command! The cost is {bitCostBot} {userBitName}.");
                                }
                            }
                            else
                            {
                                // Send message indicating user's bits data not found
                                client.SendMessage(channelId, $"{Chatter}, your {userBitName} data is not found!");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Sweatbot is permanently off.");
                        }
                    }
                    break;

                case "sendkey":
                    if (Settings.Default.isSendKeyEnabled && Settings.Default.isStoreCurrency)
                    {
                        if (userBits.ContainsKey(Chatter))
                        {
                            if (int.TryParse(controlMenu.SendKeyCostBox.Text, out int bitCost))
                            {
                                if (int.TryParse(controlMenu.SendKeyTimeBox.Text, out int holdtime))
                                {
                                    if (userBits[Chatter] >= bitCost)
                                    {
                                        string[] sendKeyCommandParts = e.Command.ChatMessage.Message.Split(' ');

                                        if (sendKeyCommandParts.Length > 1)
                                        {
                                            string keyToSend = sendKeyCommandParts[1].ToUpper();

                                            try
                                            {
                                                string filePath = @"Data\SendKeyExclude.txt";
                                                string[] excludedKeys = File.ReadAllText(filePath)
                                                                             .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                                             .Select(k => k.Trim().ToUpper())
                                                                             .ToArray();

                                                Console.WriteLine($"Excluded keys: {string.Join(", ", excludedKeys)}");

                                                if (excludedKeys.Contains(keyToSend))
                                                {
                                                    client.SendMessage(channelId, $"{Chatter}, the key '{keyToSend}' is excluded from being sent.");
                                                    return;
                                                }

                                                int virtualKey = CustomCommandHandler.ToVirtualKey(keyToSend);

                                                await Task.Run(async () =>
                                                {
                                                    keybd_event((byte)virtualKey, 0, 0, 0);
                                                    await Task.Delay(holdtime);
                                                    keybd_event((byte)virtualKey, 0, 2, 0);
                                                });

                                                LogHandler.LogCommand(Chatter, "sendkey", bitCost, userBits, timestamp);
                                                userBits[Chatter] -= bitCost;

                                                if (Settings.Default.isBitMsgEnabled)
                                                {
                                                    client.SendMessage(channelId, $"{Chatter}, You have {userBits[Chatter]} {userBitName} remaining.");
                                                }

                                                Console.WriteLine($"[{timestamp}] [{Chatter}]: {e.Command.ChatMessage.Message} Cost:{bitCost} Remaining bits:{userBits[Chatter]}");

                                                LogHandler.WriteUserBitsToJson("user_bits.json");
                                            }
                                            catch (ArgumentException)
                                            {
                                                client.SendMessage(channelId, $"{Chatter}, the key '{keyToSend}' is not supported.");
                                            }
                                            catch (Exception ex)
                                            {
                                                client.SendMessage(channelId, $"{Chatter}, there was an error processing your command: {ex.Message}");
                                            }
                                        }
                                        else
                                        {
                                            client.SendMessage(channelId, $"{Chatter}, please specify a key to send (e.g., !sendkey A or ESC).");
                                        }
                                    }
                                    else
                                    {
                                        client.SendMessage(channelId, $"{Chatter}, you don't have enough {userBitName} to use this command! The cost is {bitCost} {userBitName}.");
                                    }
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid hold time value.");
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid cost value.");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, $"{Chatter}, your {userBitName} data is not found!");
                        }
                    }
                    break;

                case "sbgamble":
                    if (Settings.Default.isBitGambleEnabled && Settings.Default.isStoreCurrency)
                    {
                        // Check if the command is sub-only
                        bool isSubGambleOnly = Settings.Default.isSubOnlyGambleCommand;

                        if (isSubGambleOnly && !isSubscriber)
                        {
                            client.SendMessage(channelId, $"{Chatter}, this command is for Twitch subscribers only!");
                            break;
                        }

                        // Check if the user has enough bits to gamble
                        if (userBits.ContainsKey(Chatter))
                        {
                            string message = e.Command.ChatMessage.Message.ToLower();
                            int gambleAmount = 0;

                            if (message.Contains(" "))
                            {
                                string[] commandParts = message.Split(' ');

                                if (commandParts.Length > 1)
                                {
                                    string gambleInput = commandParts[1];

                                    // Check if the user wants to gamble all their bits
                                    if (gambleInput == "all")
                                    {
                                        gambleAmount = userBits[Chatter];
                                        if (gambleAmount <= 0)
                                        {
                                            client.SendMessage(channelId, $"{Chatter}, you don't have any {userBitName} to gamble.");
                                            break;
                                        }
                                    }
                                    // Check if the input contains a percentage
                                    else if (gambleInput.EndsWith("%"))
                                    {
                                        string percentageString = gambleInput.TrimEnd('%');
                                        if (int.TryParse(percentageString, out int percentage) && percentage > 0 && percentage <= 100)
                                        {
                                            gambleAmount = (int)(userBits[Chatter] * (percentage / 100.0));

                                            if (gambleAmount == 0)
                                            {
                                                client.SendMessage(channelId, $"{Chatter}, you can't gamble less than 1 {userBitName}.");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            client.SendMessage(channelId, $"{Chatter}, please specify a valid percentage (1-100%).");
                                            break;
                                        }
                                    }
                                    else if (int.TryParse(gambleInput, out gambleAmount))
                                    {
                                        if (gambleAmount <= 0)
                                        {
                                            client.SendMessage(channelId, $"{Chatter}, you can't gamble 0 or negative {userBitName}.");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        client.SendMessage(channelId, $"{Chatter}, please specify a valid amount or percentage to gamble (e.g., !sbgamble 100, !sbgamble 50%, or !sbgamble all).");
                                        break;
                                    }
                                }
                                else
                                {
                                    client.SendMessage(channelId, $"{Chatter}, please specify a valid amount or percentage to gamble.");
                                    break;
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, $"{Chatter}, please specify an amount or percentage to gamble.");
                                break;
                            }

                            // Check if the user has enough bits to gamble
                            if (userBits[Chatter] >= gambleAmount)
                            {
                                int cooldownSeconds = 0;
                                if (int.TryParse(controlMenu.BitGambleCDBox.Text, out cooldownSeconds) && cooldownSeconds >= 0)
                                {
                                    if (lastGambleTime.ContainsKey(Chatter))
                                    {
                                        DateTime lastGamble = lastGambleTime[Chatter];
                                        TimeSpan timeSinceLastGamble = DateTime.Now - lastGamble;

                                        if (timeSinceLastGamble.TotalSeconds < cooldownSeconds)
                                        {
                                            int remainingCooldown = cooldownSeconds - (int)timeSinceLastGamble.TotalSeconds;
                                            client.SendMessage(channelId, $"{Chatter}, you must wait {remainingCooldown} seconds before gambling again.");
                                            break;
                                        }
                                    }

                                    int winChance = 0;
                                    if (int.TryParse(controlMenu.BitChanceBox.Text, out winChance) && winChance >= 0 && winChance <= 100)
                                    {
                                        Random rand = new Random();
                                        int outcome = rand.Next(1, 101);

                                        if (outcome <= winChance)
                                        {
                                            int winnings = gambleAmount * 2;
                                            userBits[Chatter] += winnings;

                                            LogHandler.LogBits(Chatter, winnings, timestamp);
                                            LogHandler.WriteUserBitsToJson("user_bits.json");

                                            client.SendMessage(channelId, $"{Chatter}, you won! You gambled {gambleAmount} {userBitName} and won {winnings} {userBitName}. You now have {userBits[Chatter]} {userBitName}.");
                                            Console.WriteLine($"[{timestamp}] [{Chatter}] Gambled {gambleAmount} bits, won {winnings} bits. Total bits: {userBits[Chatter]}");
                                        }
                                        else
                                        {
                                            userBits[Chatter] -= gambleAmount;

                                            LogHandler.LogBits(Chatter, -gambleAmount, timestamp);
                                            LogHandler.WriteUserBitsToJson("user_bits.json");

                                            client.SendMessage(channelId, $"{Chatter}, you lost! You gambled {gambleAmount} {userBitName} and lost. You now have {userBits[Chatter]} {userBitName}.");
                                            Console.WriteLine($"[{timestamp}] [{Chatter}] Gambled {gambleAmount} bits, lost. Total bits: {userBits[Chatter]}");
                                        }
                                    }
                                    else
                                    {
                                        client.SendMessage(channelId, "Invalid win chance value. Please ensure it's between 0 and 100.");
                                    }

                                    lastGambleTime[Chatter] = DateTime.Now;
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid cooldown time value in BitGambleCDBox.");
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, $"{Chatter}, you don't have enough {userBitName} to gamble {gambleAmount} {userBitName}!");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, $"{Chatter}, you don't have any {userBitName} to gamble.");
                        }
                    }
                    break;
            }

            //Mod Commands
            if (e.Command.ChatMessage.IsModerator)
            {
                switch (e.Command.CommandText.ToLower())
                {

                    case "addbits":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "give"))
                        {
                            ChatCommandMethods.AddBitCommand(client, e);
                        }
                        else
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "You are not authorized to use this command.");
                        }
                        break;

                    case "rembits":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "remove"))
                        {
                            ChatCommandMethods.RemoveBitCommand(client, e);
                        }
                        else
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "You are not authorized to use this command.");
                        }
                        break;

                    case "refund":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "refund"))
                        {
                            string[] refundArgs = e.Command.ArgumentsAsString.Split(' ');

                            if (refundArgs.Length == 1)
                            {
                                string refundUsername = refundArgs[0].StartsWith("@") ? refundArgs[0].Substring(1) : refundArgs[0]; // Remove "@" symbol if present

                                ChatCommandMethods.RefundLastCommand(e, refundUsername, userBits, client, channelId);
                                LogHandler.WriteUserBitsToJson("user_bits.json"); // Write changes to JSON file
                            }
                            else
                            {
                                client.SendMessage(e.Command.ChatMessage.Channel, "Invalid syntax. Usage: !refund [username]");
                            }
                        }
                        else
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "You are not authorized to use this command.");
                        }
                        break;

                    case "sbadd":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "add_remove_command"))
                        {
                            var sbaddArgs = e.Command.ArgumentsAsString.Split(' ');

                            if (sbaddArgs.Length >= 3)
                            {
                                string commandName = sbaddArgs[0].ToLower();
                                if (int.TryParse(sbaddArgs[1], out int bitCost))
                                {
                                    string methods = string.Join(" ", sbaddArgs.Skip(2));
                                    var commandHandler = new CustomCommandHandler(commandsFilePath);
                                    string resultMessage = commandHandler.AddChatCommand(commandName, bitCost, methods);
                                    client.SendMessage(channelId, resultMessage);
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid {userBitName} cost. Please provide a valid number.");
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid syntax. Usage: !sbadd {commandName} {cost} {methods}");
                            }
                        }
                        break;

                    case "sbremove":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "add_remove_command"))
                        {
                            var sbremoveArgs = e.Command.ArgumentsAsString.Split(' ');

                            if (sbremoveArgs.Length == 1)
                            {
                                string commandNameToRemove = sbremoveArgs[0].ToLower();
                                var commandHandler = new CustomCommandHandler(commandsFilePath);
                                bool isRemoved = commandHandler.RemoveChatCommand(commandNameToRemove);

                                if (isRemoved)
                                {
                                    client.SendMessage(channelId, $"Command '!{commandNameToRemove}' has been removed successfully.");
                                }
                                else
                                {
                                    client.SendMessage(channelId, $"Command '!{commandNameToRemove}' not found.");
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid syntax. Usage: !sbremove {commandName}");
                            }
                        }
                        break;

                    case "sbban":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "ban"))
                        {
                            var sbbanArgs = e.Command.ArgumentsAsString.Split(' ');

                            if (sbbanArgs.Length == 1)
                            {
                                string bannedUsername = sbbanArgs[0].StartsWith("@") ? sbbanArgs[0].Substring(1) : sbbanArgs[0];

                                if (!bannedUsers.Contains(bannedUsername))
                                {
                                    bannedUsers.Add(bannedUsername);
                                    ChatCommandMethods.SaveBanList(bannedUsers);
                                    client.SendMessage(channelId, $"{bannedUsername} has been banned from using the bot.");
                                }
                                else
                                {
                                    client.SendMessage(channelId, $"{bannedUsername} is already banned.");
                                }
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbban [username]");
                        }
                        break;

                    // Command Debug
                    case "cdebug":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "add_remove_command"))
                        {
                            if (e.Command.ArgumentsAsList.Count == 0)
                            {
                                client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, please specify a command to print to chat. Example: !cdebug pop");
                                break;
                            }

                            string debugCommandName = e.Command.ArgumentsAsList[0];
                            string debugResponse = commandHandler.DebugCommand(debugCommandName);

                            client.SendMessage(channelId, debugResponse);
                        }
                        break;

                    case "sbunban":
                        if (Settings.Default.isModWhitelistEnabled || LogHandler.HasPermission(Chatter, "ban"))
                        {
                            var sbunbanArgs = e.Command.ArgumentsAsString.Split(' ');

                            if (sbunbanArgs.Length == 1)
                            {
                                string unbannedUsername = sbunbanArgs[0].StartsWith("@") ? sbunbanArgs[0].Substring(1) : sbunbanArgs[0];

                                if (bannedUsers.Contains(unbannedUsername))
                                {
                                    bannedUsers.Remove(unbannedUsername);
                                    ChatCommandMethods.SaveBanList(bannedUsers);
                                    client.SendMessage(channelId, $"{unbannedUsername} has been unbanned and can use the bot again.");
                                }
                                else
                                {
                                    client.SendMessage(channelId, $"{unbannedUsername} is not on the ban list.");
                                }
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbunban [username]");
                        }
                        break;
                }
            }

            //Channel owner chat
            if (e.Command.ChatMessage.IsBroadcaster)
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "hi":
                        client.SendMessage(channelId, "Hi");
                        break;

                    case "addbits":
                        ChatCommandMethods.AddBitCommand(client, e);
                        break;

                    case "rembits":
                        ChatCommandMethods.RemoveBitCommand(client, e);
                        break;

                    case "refund":
                        string[] refundArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (refundArgs.Length == 1)
                        {
                            string refundUsername = refundArgs[0].StartsWith("@") ? refundArgs[0].Substring(1) : refundArgs[0]; // Remove "@" symbol if present

                            ChatCommandMethods.RefundLastCommand(e, refundUsername, userBits, client, channelId);
                            LogHandler.WriteUserBitsToJson("user_bits.json"); // Write changes to JSON file
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !refund [username]");
                        }
                        break;

                    case "sbadd":
                        var sbaddArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (sbaddArgs.Length >= 3)
                        {
                            string commandName = sbaddArgs[0].ToLower();
                            if (int.TryParse(sbaddArgs[1], out int bitCost))
                            {
                                string methods = string.Join(" ", sbaddArgs.Skip(2));
                                var commandHandler = new CustomCommandHandler(commandsFilePath);
                                string resultMessage = commandHandler.AddChatCommand(commandName, bitCost, methods);
                                client.SendMessage(channelId, resultMessage);
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid {userBitName} cost. Please provide a valid number.");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbadd {commandName} {cost} {methods}");
                        }
                        break;

                    case "sbremove":

                        var sbremoveArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (sbremoveArgs.Length == 1)
                        {
                            string commandNameToRemove = sbremoveArgs[0].ToLower();
                            var commandHandler = new CustomCommandHandler(commandsFilePath);
                            bool isRemoved = commandHandler.RemoveChatCommand(commandNameToRemove);

                            if (isRemoved)
                            {
                                client.SendMessage(channelId, $"Command '!{commandNameToRemove}' has been removed successfully.");
                            }
                            else
                            {
                                client.SendMessage(channelId, $"Command '!{commandNameToRemove}' not found.");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbremove {commandName}");
                        }

                        break;

                    case "sbban":
                        var sbbanArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (sbbanArgs.Length == 1)
                        {
                            string bannedUsername = sbbanArgs[0].StartsWith("@") ? sbbanArgs[0].Substring(1) : sbbanArgs[0];

                            if (!bannedUsers.Contains(bannedUsername))
                            {
                                bannedUsers.Add(bannedUsername);
                                ChatCommandMethods.SaveBanList(bannedUsers);
                                client.SendMessage(channelId, $"{bannedUsername} has been banned from using the bot.");
                            }
                            else
                            {
                                client.SendMessage(channelId, $"{bannedUsername} is already banned.");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbban [username]");
                        }
                        break;

                    case "sbunban":
                        var sbunbanArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (sbunbanArgs.Length == 1)
                        {
                            string unbannedUsername = sbunbanArgs[0].StartsWith("@") ? sbunbanArgs[0].Substring(1) : sbunbanArgs[0];

                            if (bannedUsers.Contains(unbannedUsername))
                            {
                                bannedUsers.Remove(unbannedUsername);
                                ChatCommandMethods.SaveBanList(bannedUsers);
                                client.SendMessage(channelId, $"{unbannedUsername} has been unbanned and can use the bot again.");
                            }
                            else
                            {
                                client.SendMessage(channelId, $"{unbannedUsername} is not on the ban list.");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbunban [username]");
                        }
                        break;

                    // Command Debug
                    case "cdebug":
                        if (e.Command.ArgumentsAsList.Count == 0)
                        {
                            client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, please specify a command to print to chat. Example: !cdebug pop");
                            break;
                        }

                        string debugCommandName = e.Command.ArgumentsAsList[0];
                        string debugResponse = commandHandler.DebugCommand(debugCommandName);

                        client.SendMessage(channelId, debugResponse);
                        break;
                }
            }
        }

        //Loading and logging
        public void LoadCredentialsFromJSON()
        {
            // Correct the path to be relative
            string jsonFilePath = Path.Combine("Data", "bin", "Logon.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                CounterData data = JsonConvert.DeserializeObject<CounterData>(json);

                if (data != null && !string.IsNullOrEmpty(data.ChannelName))
                {
                    creds = new ConnectionCredentials(data.ChannelName, data.BotToken);
                    channelId = data.ChannelName;
                }
            }
        }

        //Auto Message
        public void LoadAutoMessageData()
        {
            try
            {
                string jsonFilePath = Path.Combine("Data", "bin", "CommandConfigData.json");

                if (File.Exists(jsonFilePath))
                {
                    string jsonData = File.ReadAllText(jsonFilePath);
                    commandConfigData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

                    if (commandConfigData.ContainsKey("autoSendMessageCD") && int.TryParse(commandConfigData["autoSendMessageCD"], out int cd))
                    {
                        autoSendMessageCD = cd;
                    }
                    else
                    {
                        Console.WriteLine("Invalid or missing 'autoSendMessageCD' value in CommandConfigData.json.");
                    }
                }
                else
                {
                    Console.WriteLine("CommandConfigData.json not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CommandConfigData: {ex.Message}");
            }
        }

        public void AutoMessageSender(object state)
        {
            try
            {
                if (Settings.Default.isAutoMessageEnabled)
                {
                    // Reload auto message data before sending
                    LoadAutoMessageData();

                    if (commandConfigData != null && commandConfigData.ContainsKey("autoMessageBox"))
                    {
                        string autoMessageBox = commandConfigData["autoMessageBox"];

                        // Split the message by '\\'
                        string[] messageParts = autoMessageBox.Split(new string[] { "\\\\" }, StringSplitOptions.None);

                        // Send each part as a separate message
                        foreach (string messagePart in messageParts)
                        {
                            if (!string.IsNullOrEmpty(messagePart.Trim()))
                            {
                                client.SendMessage(channelId, messagePart);
                                Console.WriteLine(messagePart);
                            }
                        }

                        // Update the timer interval
                        int intervalMilliseconds = autoSendMessageCD * 1000;
                        timer.Change(intervalMilliseconds, Timeout.Infinite);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AutoMessageSender: {ex.Message}");
            }
        }

        public void StartAutoMessage()
        {
            Thread.Sleep(2000);

            // Convert the interval to milliseconds
            int intervalMilliseconds = autoSendMessageCD * 1000;

            // Create a Timer object to run the method immediately and then reschedule it
            timer = new System.Threading.Timer(AutoMessageSender, null, 0, intervalMilliseconds);
        }
    }
}


