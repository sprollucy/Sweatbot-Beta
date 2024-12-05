
﻿using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;




/* TODO **
 * 
 */

namespace UiBot
{

    internal class MainBot : IDisposable
    {
        //References
        ChatCommandMethods chatCommandMethods = new ChatCommandMethods();
        ControlMenu controlMenu = new ControlMenu();
        private static CustomCommandHandler commandHandler;

        //dictionary 
        public static Dictionary<string, int> userBits = new Dictionary<string, int>();
        public Dictionary<string, string> commandConfigData;

        //Console import/kill
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        // Handles connection
        private ConnectionCredentials creds;
        TwitchClient client;
        private bool isBotConnected = false;
        private static string channelId;

        //spam command
        private DateTime lastTradersCommandTimer = DateTime.MinValue;
        private System.Threading.Timer timer;
        public int autoSendMessageCD;

        internal MainBot()
        {
            LoadCredentialsFromJSON();
            LogHandler.LoadWhitelist();
            LogHandler.LoadUserBitsFromJson("user_bits.json");
            string commandsFilePath = Path.Combine("Data", "bin",  "CustomCommands.json");
            commandHandler = new CustomCommandHandler(commandsFilePath);

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
                Console.WriteLine("[Sweat Bot]: Disconnected");

                client.Disconnect();

                // Unsubscribe from events
                client.OnConnected -= Client_OnConnected;
                client.OnError -= Client_OnError;

                isBotConnected = false;
            }
            else
            {
                Console.WriteLine("[Sweat Bot]: Not connected");
            }
        }

        internal void Connect()
        {
            if (!isBotConnected)
            {
                Console.WriteLine($"[Sweat Bot]: Connecting to {channelId}...");
                InitializeTwitchClient();
                StartAutoMessage();

            }
        }

        private void InitializeTwitchClient()
        {
            if (!isBotConnected)
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.AccessToken) || string.IsNullOrEmpty(channelId))
                {
                    MessageBox.Show("Please enter token access and channel name in the Settings Menu");
                    Console.WriteLine("[Sweat Bot]: Disconnected");
                    return; // Don't proceed further
                }
                if (creds == null)
                {
                    MessageBox.Show("Twitch credentials are not set.");
                    Console.WriteLine("[Sweat Bot]: Disconnected");
                    return; // Don't proceed further
                }

                if (channelId == null)
                {
                    MessageBox.Show("Twitch channel are not set.");
                    Console.WriteLine("[Sweat Bot]: Disconnected");
                    return; // Don't proceed further
                }

                try
                {
                    client = new TwitchClient();
                    client.Initialize(creds, channelId);
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

        // Property to get the connection status
        public bool IsConnected => isBotConnected;

        // Method to process commands locally
        public void ProcessLocalCommand(string commandName)
        {
            if (commandHandler == null)
            {
                string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                string commandsFilePath = Path.Combine("CustomCommands.json");
                commandHandler = new CustomCommandHandler(commandsFilePath);
            }

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
                Console.WriteLine($"Cannot execute command '{commandName}'. Not enough bits or command does not exist.");
            }
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!Properties.Settings.Default.isCommandsPaused)
            {
                // Ensure the command handler is initialized
                if (commandHandler == null)
                {
                    string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                    string commandsFilePath = Path.Combine(dataFolderPath, "CustomCommands.json");
                    commandHandler = new CustomCommandHandler(commandsFilePath);
                }

                // Load user bits
                string userBitsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "user_bits.json");

                // Check if the message is a command
                if (e.ChatMessage.Message.StartsWith("!"))
                {
                    string commandName = e.ChatMessage.Message.TrimStart('!').ToLower();
                    LogHandler.LoadUserBitsFromJson(userBitsFilePath);

                    // Check if the command is enabled
                    if (commandHandler.GetCommand(commandName) != null)
                    {
                        var command = commandHandler.GetCommand(commandName);

                        // Check if the command is enabled
                        //if (commandName == "test" && !Properties.Settings.Default.IsTurnEnabled)
                        //{
                            // Inform the user that the command is disabled
                          //  client.SendMessage(e.ChatMessage.Channel, $"The command '{commandName}' is currently disabled.");
                           // return;
                        //}

                        // Process other commands
                        int userBits = MainBot.userBits.ContainsKey(e.ChatMessage.DisplayName)
                            ? MainBot.userBits[e.ChatMessage.DisplayName]
                            : 0;

                        if (commandHandler.CanExecuteCommand(commandName, userBits))
                        {
                            try
                            {
                                // Execute the command
                                commandHandler.ExecuteCommandAsync(commandName, client, e.ChatMessage.Channel);

                                // Log the command execution
                                string timestamp1 = DateTime.Now.ToString("HH:mm:ss");
                                LogHandler.LogCommand(e.ChatMessage.DisplayName, commandName, command.BitCost, MainBot.userBits, timestamp1);

                                // Deduct the bit cost of the command
                                MainBot.userBits[e.ChatMessage.DisplayName] -= command.BitCost;

                                // Save the updated bit data
                                LogHandler.WriteUserBitsToJson(userBitsFilePath);

                                string message = "";

                                // Only add remaining bits info if BitCost is greater than 0
                                if (command.BitCost > 0 && Properties.Settings.Default.isBitMsgEnabled)
                                {
                                    message += $" You have {MainBot.userBits[e.ChatMessage.DisplayName]} bits remaining.";
                                }

                                // Inform the user that the command was executed
                                client.SendMessage(e.ChatMessage.Channel, message);

                                // Log command details to the console
                                Console.WriteLine($"[{timestamp1}] [{e.ChatMessage.DisplayName}]: {commandName} Cost: {command.BitCost} Remaining bits: {MainBot.userBits[e.ChatMessage.DisplayName]}");
                            }
                            catch (Exception ex)
                            {
                                // Log and inform the user of the error
                                Console.WriteLine($"Error executing command '{commandName}': {ex.Message}");
                                client.SendMessage(e.ChatMessage.Channel, $"An error occurred while executing the command '{commandName}'. Please try again later.");
                            }
                        }
                        else
                        {
                            // Inform the user they do not have enough bits
                            client.SendMessage(e.ChatMessage.Channel, $"You don't have enough bits to execute the command '{commandName}'.");
                        }
                    }
                }
            }

            string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

            if (Properties.Settings.Default.isChatBonusEnabled)
            {
                if (!userBits.ContainsKey(e.ChatMessage.DisplayName))
                {
                    int bonusAmount;
                    if (int.TryParse(controlMenu.BonusTextBox.Text, out bonusAmount))
                    {
                        // User is chatting for the first time, give them the specified bonus amount
                        userBits.Add(e.ChatMessage.DisplayName, bonusAmount);
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName} welcome to the stream! Here is {bonusAmount} bits on the house, use !help for more info");
                        LogHandler.WriteUserBitsToJson("user_bits.json");
                        LogHandler.LogBits(e.ChatMessage.DisplayName, bonusAmount, timestamp);
                    }
                    else
                    {
                        // Error parsing bonus amount, use a default value or handle the error accordingly
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, invalid bonus amount format. Using default value.");
                        bonusAmount = 100; // Set a default bonus amount
                        userBits.Add(e.ChatMessage.DisplayName, bonusAmount);
                        LogHandler.WriteUserBitsToJson("user_bits.json");
                    }
                }
            }

            if (e.ChatMessage.Bits > 0)
            {
                int bitsGiven = e.ChatMessage.Bits;

                if (Properties.Settings.Default.isBonusMultiplierEnabled)
                {
                    ChatCommandMethods.BitMultiplier(); // Call BitMultiplier to update BitBonusMultiplier
                    int multiplier = ChatCommandMethods.BitBonusMultiplier; // Access BitBonusMultiplier

                    // Calculate bitsGiven after applying multiplier and rounding up
                    bitsGiven = (int)Math.Ceiling((double)bitsGiven * multiplier);

                    // Update user bits and log the transaction
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    Console.WriteLine($"Applied multiplier {multiplier}, resulting in {bitsGiven} bits given.");

                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {e.ChatMessage.Bits} bits! Multiplier is active so it counts as {bitsGiven} bits. You now have {userBits[e.ChatMessage.DisplayName]} bits.");
                }
                else
                {
                    // Update user bits and log the transaction
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    // Thank the user for giving bits and inform them of their new balance
                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {bitsGiven} bits! You now have {userBits[e.ChatMessage.DisplayName]} bits.");
                }
            }

        }

        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("[Sweat Bot]: Connected");
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
                Console.WriteLine("Sweat Bot is not connected to Twitch. Cannot send message.");
            }
        }

        //Chat 
        private async void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {

            var traderResetInfoService = new TraderResetInfoService();
            Process[] pname = Process.GetProcessesByName("notepad");

            //antispam cooldowns
            int helpCooldownDuration = 30;
            int aboutCooldownDuration = 10;
            int tradersCooldownDuration = 90;
            int bitcostCooldownDuration = 30;
            int lastHow2useTimerDuration = 30;
            TimeSpan timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastStatCommandTimer;
            string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

            //Normal Commands
            switch (e.Command.CommandText.ToLower())
            {
                case "help":

                    if (timeSinceLastExecution.TotalSeconds >= helpCooldownDuration)
                    {
                        chatCommandMethods.lastHelpCommandTimer = DateTime.Now; // Update the last "help" execution time

                        // Construct the base message using StringBuilder
                        StringBuilder message = new StringBuilder();
                        message.Append("!how2use, !about, !mybits !bitcost");

                        if (Properties.Settings.Default.isTradersEnabled)
                        {
                            message.Append(" !traders");
                        }

                        // Check if the user is a moderator
                        if (e.Command.ChatMessage.IsModerator)
                        {
                            if (Properties.Settings.Default.isModBitsEnabled)
                            {
                                message.Append(", !addbits");
                            }

                            if (Properties.Settings.Default.isModRefundEnabled)
                            {
                                message.Append(", !refund");
                            }
                        }

                        // Check if the user is a broadcaster
                        if (e.Command.ChatMessage.IsBroadcaster)
                        {
                            message.Append(", !addbits, !refund");
                        }

                        client.SendMessage(channelId, message.ToString());
                    }
                    break;

                case "how2use":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastHow2useTimer;

                    if (timeSinceLastExecution.TotalSeconds >= lastHow2useTimerDuration)
                    {
                        client.SendMessage(channelId, "To use, just cheer Bits in the chat. The bot will keep track of how many you give. Type !bitcost or !ccommand to see what you can do with them. Then, just enter the command you want to use in the chat if you have enough Bits to spend. Use !mybits to check how many Bits you have stored!");
                    }
                    break;

                case "about":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastAboutCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= aboutCooldownDuration)
                    {
                        chatCommandMethods.lastAboutCommandTimer = DateTime.Now; // Update the last "help" execution time
                        client.SendMessage(channelId, $"I am a bot created by Sprollucy. This is a small project that was inspired by bitbot and to help practice my coding. Many features may be incomplete or missing at this time.");
                        client.SendMessage(channelId, $"If you want to learn more about this project, visit https://github.com/sprollucy/Tarkov-Twitch-Bot-Working for more information, bug reporting, and suggestions");
                    }
                    break;

                case "mybits":
                    string requester = e.Command.ChatMessage.DisplayName;
                    if (userBits.ContainsKey(requester))
                    {
                        client.SendMessage(channelId, $"{requester}, you have {userBits[requester]} bits");
                        Console.WriteLine($"{requester}, you have {userBits[requester]} bits");
                    }
                    else
                    {
                        client.SendMessage(channelId, $"{requester}, you have no bits");
                        Console.WriteLine($"{requester}, you have no bits");
                    }
                    break;

                case "bitcost":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastBitcostCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= bitcostCooldownDuration)
                    {
                        chatCommandMethods.lastBitcostCommandTimer = DateTime.Now; // Update the last "help" execution time

                        // Define the mappings of textbox names to their corresponding labels and enabled states
                        var textBoxDetails = new Dictionary<string, (string Label, Func<bool> IsEnabled)>
                        {
                                { "bottoggleCostBox", ("sweatbot", () => Properties.Settings.Default.isSweatbotEnabled) }
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

                        if (!Properties.Settings.Default.isCommandsPaused)
                        {
                            if (enabledCommandCosts.Count > 0)
                            {
                                standardCommandsMessage = $"{e.Command.ChatMessage.DisplayName}, Available commands: {string.Join(", ", enabledCommandCosts)}";
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
                            standardCommandsMessage = $"{e.Command.ChatMessage.DisplayName}, All commands are paused";
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
                    break;

                case "traders":
                    if (Properties.Settings.Default.isTradersEnabled)
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
                        { "Prapor", Properties.Settings.Default.isTraderPraporEnabled },
                        { "Therapist", Properties.Settings.Default.isTraderTherapistEnabled },
                        { "Mechanic", Properties.Settings.Default.isTraderMechanicEnabled },
                        { "Peacekeeper", Properties.Settings.Default.isTraderPeacekeeperEnabled },
                        { "Fence", Properties.Settings.Default.isTraderFenceEnabled },
                        { "Ragman", Properties.Settings.Default.isTraderRagmanEnabled },
                        { "Skier", Properties.Settings.Default.isTraderSkierEnabled },
                        { "Jaeger", Properties.Settings.Default.isTraderJaegerEnabled },
                        { "Lightkeeper", Properties.Settings.Default.isTraderLightkeeperEnabled }
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
                    // Check the current state of isChatCommandPaused
                    bool currentPauseState = Properties.Settings.Default.isCommandsPaused;
                    string currentPauseStateString = currentPauseState ? "off" : "on";

                    if (Properties.Settings.Default.isSweatbotEnabled)
                    {
                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            client.SendMessage(channelId, $"Sweatbot is {currentPauseStateString}. Use !sweatbot on or off to change that");

                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.BotToggleCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    // Determine the desired state based on the command message
                                    bool desiredState = e.Command.ChatMessage.Message.ToLower().Contains("on");

                                    // Toggle the value if the command is to turn it on or off
                                    if (e.Command.ChatMessage.Message.ToLower().Contains("on") || e.Command.ChatMessage.Message.ToLower().Contains("off"))
                                    {
                                        // Log the command usage
                                        LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "sweatbot", bitCost, userBits, timestamp);

                                        // Deduct the cost of the command
                                        userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                        // Update the isChatCommandPaused value
                                        Properties.Settings.Default.isCommandsPaused = !desiredState;
                                        Properties.Settings.Default.Save();

                                        // Send confirmation message
                                        if (Properties.Settings.Default.isBitMsgEnabled)
                                        {
                                            client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, sweatbot turned {(desiredState ? "on" : "off")}! You have {userBits[e.Command.ChatMessage.DisplayName]} bits remaining.");
                                        }
                                        Console.WriteLine($"[{timestamp}] [{e.Command.ChatMessage.DisplayName}]: {e.Command.ChatMessage.Message} Cost:{bitCost} Remaining bits:{userBits[e.Command.ChatMessage.DisplayName]}");

                                        // Save the updated bit data
                                        LogHandler.WriteUserBitsToJson("user_bits.json");
                                    }
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cost value
                                client.SendMessage(channelId, "Invalid cost value.");
                            }
                        }
                        else
                        {
                            // Send message indicating user's bits data not found
                            client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, your bit data is not found!");
                        }
                    }
                    else
                    {
                        client.SendMessage(channelId, $"Sweatbot is permanently {currentPauseStateString}");
                    }
                    break;

            }

            //Mod Commands
            if (e.Command.ChatMessage.IsModerator)
            {
                switch (e.Command.CommandText.ToLower())
                {

                    case "addbits":
                        if (Properties.Settings.Default.isModBitsEnabled)
                        {
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(e.Command.ChatMessage.DisplayName))
                            {
                                ChatCommandMethods.AddBitCommand(client, e);
                            }
                            else
                            {
                                client.SendMessage(e.Command.ChatMessage.Channel, "You are not authorized to use this command.");
                            }
                        }
                        break;

                    case "refund":
                        if (Properties.Settings.Default.isModRefundEnabled)
                        {
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(e.Command.ChatMessage.DisplayName))
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
                        }
                        break;

                }
            }

            //Channel owner chat
            if (e.Command.ChatMessage.IsBroadcaster)
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "help":
                        client.SendMessage(channelId, "!hi, !addbits, !refund");
                        break;
                    case "hi":
                        client.SendMessage(channelId, "Hi");
                        break;
                    case "note":
                        if (pname.Length > 0)
                        {
                            break;
                        }
                        else
                        {
                            Process notePad = new Process();
                            notePad.StartInfo.FileName = "notepad.exe";
                            notePad.Start();
                        }
                        break;

                    case "addbits":
                        ChatCommandMethods.AddBitCommand(client, e);
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
                }
            }
        }

        //Loading and logging
        public void LoadCredentialsFromJSON()
        {
            // Correct the path to be relative
            string jsonFilePath = Path.Combine("Data", "bin","Logon.json");

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                CounterData data = JsonConvert.DeserializeObject<CounterData>(json);

                if (data != null && !string.IsNullOrEmpty(data.ChannelName))
                {
                    creds = new ConnectionCredentials(data.ChannelName, data.BotToken);
                    channelId = data.ChannelName;
                }
                else
                {
                    // Handle the case where the ChannelName is empty or invalid.
                    // You can show a message to the user or take appropriate action.
                }
            }
            else
            {
                // Handle the absence of the JSON file as needed.
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
                if (Properties.Settings.Default.isAutoMessageEnabled)
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
            Thread.Sleep(1000);

            // Convert the interval to milliseconds
            int intervalMilliseconds = autoSendMessageCD * 1000;

            // Create a Timer object to run the method immediately and then reschedule it
            timer = new System.Threading.Timer(AutoMessageSender, null, 0, intervalMilliseconds);
        }

    }

}
