
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub;


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

        public Dictionary<string, string> commandConfigData;

        // Keyboard Events
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        // Handles connection
        private ConnectionCredentials creds;
        TwitchClient client;
        public bool isBotConnected = false;
        private static string channelId;


        // Property to get the connection status
        public bool IsConnected => isBotConnected;

        // spam command
        private DateTime lastTradersCommandTimer = DateTime.MinValue;
        private System.Threading.Timer timer;
        public int autoSendMessageCD;

        // Logging timestamp
        string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");

        // File loading
        string commandsFilePath = Path.Combine("Data", "bin", "CustomCommands.json");
        string userBitsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "user_bits.json");

        internal MainBot()
        {
            commandHandler = new CustomCommandHandler(commandsFilePath);

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
                Console.WriteLine("[Sweat Bot]: Disconnected");

                client.Disconnect();

                // Unsubscribe from events
                client.OnConnected -= Client_OnConnected;
                client.OnError -= Client_OnError;

                isBotConnected = false;
            }
            else
            {
                Console.WriteLine("[SweatBot]: Not connected");
            }
        }

        internal void Connect()
        {
            if (!isBotConnected)
            {
                Console.WriteLine($"[SweatBot]: Connecting to {channelId}...");
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
                if (string.IsNullOrEmpty(Properties.Settings.Default.AccessToken) || string.IsNullOrEmpty(channelId))
                {
                    MessageBox.Show("Please enter token access and channel name in the Settings Menu");
                    Console.WriteLine("[SweatBot]: Disconnected");
                    return; // Don't proceed further
                }
                if (creds == null)
                {
                    MessageBox.Show("Twitch credentials are not set.");
                    Console.WriteLine("[SweatBot]: Disconnected");
                    return; // Don't proceed further
                }

                if (channelId == null)
                {
                    MessageBox.Show("Twitch channel are not set.");
                    Console.WriteLine("[SweatBot]: Disconnected");
                    return; // Don't proceed further
                }

                try
                {
                    client = new TwitchClient();
                    client.Initialize(creds, channelId);

                    // Subscribe to the OnNewSubscriber event
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

        private void InitializePubSub()
        {
            if (PubSub == null)
            {
                PubSub = new TwitchPubSub();
                PubSub.OnFollow += PubSub_OnFollow;


                // Ensure PubSub is connected
                PubSub.Connect();
            }
        }

        private void PubSub_OnFollow(object sender, OnFollowArgs e)
        {
            if (Properties.Settings.Default.isFollowBonusEnabled)
            {
                string followerName = e.Username;
                int bitsAwarded = 100; // Default value

                // Try to get the value from the FollowTextBox, similar to how you handle ChatBonus
                if (int.TryParse(controlMenu.FollowTextBox.Text, out int followBonus))
                {
                    bitsAwarded = followBonus;  // Update bitsAwarded with the value from FollowTextBox
                }
                else
                {
                    // Handle error if the value from the FollowTextBox is invalid
                    client.SendMessage(channelId, $"{followerName}, invalid follow bonus amount. Using default value.");
                    bitsAwarded = 100;  // Default value in case of invalid input
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
                client.SendMessage(channelId, $"{followerName} is now following! You have been awarded {bitsAwarded} bits. You now have {userBits[followerName]} bits.");

                // Optionally print to console
                Console.WriteLine($"[{timestamp}] {followerName} followed and was awarded {bitsAwarded} bits. Total bits: {userBits[followerName]}");
            }
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (Properties.Settings.Default.isSubBonusEnabled)
            {
                string subscriberName = e.Subscriber.DisplayName;
                int bitsAwarded = 500; // Default value

                // Try to get the value from the SubTextBox, similar to how you handle FollowTextBox
                if (int.TryParse(controlMenu.SubTextBox.Text, out int subBonus))
                {
                    bitsAwarded = subBonus;  // Update bitsAwarded with the value from SubTextBox
                }
                else
                {
                    // Handle error if the value from the SubTextBox is invalid
                    client.SendMessage(channelId, $"{subscriberName}, invalid subscription bonus amount. Using default value.");
                    bitsAwarded = 500;  // Default value in case of invalid input
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
                client.SendMessage(channelId, $"{subscriberName}, thank you for subscribing! You have been awarded {bitsAwarded} bits. You now have {userBits[subscriberName]} bits.");

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

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!Properties.Settings.Default.isCommandsPaused)
            {
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
                                // Execute the command
                                commandHandler.ExecuteCommandAsync(commandName, client, e.ChatMessage.Channel);

                                // Log the command execution
                                LogHandler.LogCommand(e.ChatMessage.DisplayName, commandName, command.BitCost, MainBot.userBits, timestamp);

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
                                Console.WriteLine($"[{timestamp}] [{e.ChatMessage.DisplayName}]: {commandName} Cost: {command.BitCost} Remaining bits: {MainBot.userBits[e.ChatMessage.DisplayName]}");
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
                                    // Execute the command
                                    commandHandler.ExecuteCommandAsync(commandName, client, e.ChatMessage.Channel);

                                    // Log the command execution
                                    LogHandler.LogCommand(e.ChatMessage.DisplayName, commandName, cheerAmount, MainBot.userBits, timestamp);

                                    // Send message with remaining bits
                                    string message = $"{e.ChatMessage.DisplayName}: {commandName} Cost: {cheerAmount}";
                                    client.SendMessage(e.ChatMessage.Channel, message);

                                    // Log command details to the console
                                    Console.WriteLine($"[{timestamp}] [{e.ChatMessage.DisplayName}]: {commandName} Cost: {cheerAmount}");
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
                                // No matching commands found, add the cheer amount to user's bit balance
                                if (MainBot.userBits.ContainsKey(e.ChatMessage.DisplayName))
                                {
                                    // Add cheer amount to the user's current bit balance
                                    MainBot.userBits[e.ChatMessage.DisplayName] += cheerAmount;

                                    // Log the addition of bits
                                    LogHandler.LogCommand(e.ChatMessage.DisplayName, "Added to balance", cheerAmount, MainBot.userBits, timestamp);
                                    client.SendMessage(e.ChatMessage.Channel, $"No command found for {cheerAmount} bits. Your balance has been updated by {cheerAmount} bits.");
                                    LogHandler.WriteUserBitsToJson(userBitsFilePath);
                                }
                                else
                                {
                                    // If the user doesn't have any bits, initialize their balance
                                    MainBot.userBits[e.ChatMessage.DisplayName] = cheerAmount;

                                    // Log the addition of bits
                                    LogHandler.LogCommand(e.ChatMessage.DisplayName, "Initialized balance", cheerAmount, MainBot.userBits, timestamp);
                                    client.SendMessage(e.ChatMessage.Channel, $"Your balance has been initialized with {cheerAmount} bits.");
                                }
                            }
                        }
                    }
                }
            }

            // Given Bits
            if (!e.ChatMessage.Message.StartsWith("!") && e.ChatMessage.Bits > 0)
            {
                int bitsGiven = e.ChatMessage.Bits;
                bool isSubscriber = e.ChatMessage.IsSubscriber;

                // Check if both bonus multipliers are enabled
                if (Properties.Settings.Default.isBonusMultiplierEnabled && Properties.Settings.Default.isSubBonusMultiEnabled && isSubscriber)
                {
                    // Apply Sub Bonus Multiplier first for subscribers
                    ChatCommandMethods.SubBitMultiplier(); // Update SubBonusMultiplier
                    int multiplier = ChatCommandMethods.SubBonusMultiplier; // Access SubBitBonusMultiplier

                    // Calculate bits after applying the subscriber multiplier
                    bitsGiven = (int)Math.Ceiling((double)bitsGiven * multiplier);

                    // Log the transaction
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    Console.WriteLine($"A {multiplier}x multiplier is active for Subs, resulting in {bitsGiven} bits given.");

                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {e.ChatMessage.Bits} bits! Sub Multiplier is active so it counts as {bitsGiven} bits. You now have {userBits[e.ChatMessage.DisplayName]} bits.");
                }
                else if (Properties.Settings.Default.isBonusMultiplierEnabled)
                {
                    // Apply normal Bonus Multiplier if not a subscriber or only the regular multiplier is enabled
                    ChatCommandMethods.BitMultiplier(); // Update BitBonusMultiplier
                    int multiplier = ChatCommandMethods.BitBonusMultiplier; // Access BitBonusMultiplier

                    // Calculate bits after applying the normal multiplier
                    bitsGiven = (int)Math.Ceiling((double)bitsGiven * multiplier);

                    // Log the transaction
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    Console.WriteLine($"A {multiplier}x multiplier is active, resulting in {bitsGiven} bits given.");

                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {e.ChatMessage.Bits} bits! Multiplier is active so it counts as {bitsGiven} bits. You now have {userBits[e.ChatMessage.DisplayName]} bits.");
                }
                else if (Properties.Settings.Default.isSubBonusMultiEnabled && isSubscriber)
                {
                    // Apply Sub Bonus Multiplier if only the subscriber bonus is enabled
                    ChatCommandMethods.SubBitMultiplier(); // Update SubBonusMultiplier
                    int multiplier = ChatCommandMethods.SubBonusMultiplier; // Access SubBitBonusMultiplier

                    // Calculate bits after applying the subscriber multiplier
                    bitsGiven = (int)Math.Ceiling((double)bitsGiven * multiplier);

                    // Log the transaction
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    Console.WriteLine($"A {multiplier}x multiplier is active for Subs, resulting in {bitsGiven} bits given.");

                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {e.ChatMessage.Bits} bits! Sub Multiplier is active so it counts as {bitsGiven} bits. You now have {userBits[e.ChatMessage.DisplayName]} bits.");
                }
                else
                {
                    // No multiplier is applied
                    LogHandler.UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                    LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);

                    // Thank the user and inform them of their new balance
                    client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, thank you for the {bitsGiven} bits! You now have {userBits[e.ChatMessage.DisplayName]} bits.");
                }
            }

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
            string Chatter = e.Command.ChatMessage.DisplayName;

            //antispam cooldowns
            int helpCooldownDuration = 30;
            int aboutCooldownDuration = 60;
            int tradersCooldownDuration = 90;
            int bitcostCooldownDuration = 30;
            int lastHow2useTimerDuration = 30;
            TimeSpan timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastStatCommandTimer;

            //Normal Commands
            switch (e.Command.CommandText.ToLower())
            {
                case "help":

                    if (timeSinceLastExecution.TotalSeconds >= helpCooldownDuration)
                    {
                        chatCommandMethods.lastHelpCommandTimer = DateTime.Now; // Update the last "help" execution time

                        // Construct the base message using StringBuilder
                        StringBuilder message = new StringBuilder();
                        message.Append("!how2use, !about, !mybits");

                        if (Properties.Settings.Default.isTradersEnabled)
                        {
                            message.Append(", !traders");
                        }

                        if (Properties.Settings.Default.isBitCostEnabled)
                        {
                            message.Append(", !bitcost");
                        }

                        if (Properties.Settings.Default.isBitGambleEnabled)
                        {
                            message.Append(", !sbgamble");
                        }

                        // Check if the user is a moderator
                        if (e.Command.ChatMessage.IsModerator)
                        {
                            if (Properties.Settings.Default.isModBitsEnabled)
                            {
                                message.Append(", !addbits, !rembits");
                            }

                            if (Properties.Settings.Default.isModRefundEnabled)
                            {
                                message.Append(", !refund");
                            }

                            if (Properties.Settings.Default.isModAddEnabled)
                            {
                                message.Append(", !sbadd");
                            }

                            if (Properties.Settings.Default.isModRemoveEnabled)
                            {
                                message.Append(", !sbremove");
                            }
                        }

                        // Check if the user is a broadcaster
                        if (e.Command.ChatMessage.IsBroadcaster)
                        {
                            message.Append(", !addbits, !refund, !sbadd, !rembits, !sbremove");
                        }

                        client.SendMessage(channelId, message.ToString());
                    }
                    break;

                case "how2use":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastHow2useTimer;

                    if (timeSinceLastExecution.TotalSeconds >= lastHow2useTimerDuration)
                    {
                        client.SendMessage(channelId, "To use Sweatbot, simply cheer Bits in the chat, and the bot will track how many you've given or do !{cheeramount} to directly run a command that matches that amount. Use `!bitcost` to see a list of available commands and their costs. When you have enough Bits, just type the command you want to use in the chat. You can also check your balance at any time with `!mybits`."
);
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
                    if (userBits.ContainsKey(Chatter))
                    {
                        client.SendMessage(channelId, $"{Chatter}, you have {userBits[Chatter]} bits");
                        Console.WriteLine($"{Chatter}, you have {userBits[Chatter]} bits");
                    }
                    else
                    {
                        client.SendMessage(channelId, $"{Chatter}, you have no bits");
                        Console.WriteLine($"{Chatter}, you have no bits");
                    }
                    break;

                case "bitcost":
                    if (Properties.Settings.Default.isBitCostEnabled)
                    {
                        commandHandler.ReloadCommands(commandsFilePath);
                        timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastBitcostCommandTimer;

                        if (timeSinceLastExecution.TotalSeconds >= bitcostCooldownDuration)
                        {
                            chatCommandMethods.lastBitcostCommandTimer = DateTime.Now;

                            // Define the mappings of textbox names to their corresponding labels and enabled states
                            var textBoxDetails = new Dictionary<string, (string Label, Func<bool> IsEnabled)>
                        {
                                { "bottoggleCostBox", ("sweatbot", () => Properties.Settings.Default.isSweatbotEnabled) },
                                { "sendkeyCostBox", ("sendkey", () => Properties.Settings.Default.isSendKeyEnabled) }

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
                    string currentPauseStateString = currentPauseState ? "on" : "off";

                    if (Properties.Settings.Default.isSweatbotEnabled)
                    {
                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(Chatter))
                        {
                            client.SendMessage(channelId, $"Sweatbot is {currentPauseStateString}. Use !sweatbot on or off to change that");

                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.BotToggleCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[Chatter] >= bitCost)
                                {
                                    // Determine the desired state based on the command message
                                    bool desiredState = e.Command.ChatMessage.Message.ToLower().Contains("on");

                                    // Toggle the value if the command is to turn it on or off
                                    if (e.Command.ChatMessage.Message.ToLower().Contains("on") || e.Command.ChatMessage.Message.ToLower().Contains("off"))
                                    {
                                        // Log the command usage
                                        LogHandler.LogCommand(Chatter, "sweatbot", bitCost, userBits, timestamp);

                                        // Deduct the cost of the command
                                        userBits[Chatter] -= bitCost;

                                        // Update the isChatCommandPaused value
                                        Properties.Settings.Default.isCommandsPaused = !desiredState;
                                        Properties.Settings.Default.Save();

                                        // Send confirmation message
                                        if (Properties.Settings.Default.isBitMsgEnabled)
                                        {
                                            client.SendMessage(channelId, $"{Chatter}, sweatbot turned {(desiredState ? "on" : "off")}! You have {userBits[Chatter]} bits remaining.");
                                        }
                                        Console.WriteLine($"[{timestamp}] [{Chatter}]: {e.Command.ChatMessage.Message} Cost:{bitCost} Remaining bits:{userBits[Chatter]}");

                                        // Save the updated bit data
                                        LogHandler.WriteUserBitsToJson("user_bits.json");
                                    }
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{Chatter}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
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
                            client.SendMessage(channelId, $"{Chatter}, your bit data is not found!");
                        }
                    }
                    else
                    {
                        client.SendMessage(channelId, $"Sweatbot is permanently {currentPauseStateString}");
                    }
                    break;

                case "sendkey":
                    if (Properties.Settings.Default.isSendKeyEnabled)
                    {
                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(Chatter))
                        {
                            if (int.TryParse(controlMenu.SendKeyCostBox.Text, out int bitCost))
                            {
                                if (int.TryParse(controlMenu.SendKeyTimeBox.Text, out int holdtime))
                                {
                                    // Check if the user has enough bits
                                    if (userBits[Chatter] >= bitCost)
                                    {
                                        // Extract the key to be sent from the command message
                                        string[] sendKeyCommandParts = e.Command.ChatMessage.Message.Split(' ');

                                        if (sendKeyCommandParts.Length > 1)
                                        {
                                            string keyToSend = sendKeyCommandParts[1].ToUpper();

                                            try
                                            {
                                                // Get the virtual key for the specified key
                                                int virtualKey = CustomCommandHandler.ToVirtualKey(keyToSend);

                                                // Simulate key press and hold asynchronously
                                                await Task.Run(async () =>
                                                {
                                                    keybd_event((byte)virtualKey, 0, 0, 0); // Key down
                                                    await Task.Delay(holdtime);          // Hold for the specified time
                                                    keybd_event((byte)virtualKey, 0, 2, 0); // Key up
                                                });

                                                // If successful, log the command usage and deduct bits
                                                LogHandler.LogCommand(Chatter, "sendkey", bitCost, userBits, timestamp);
                                                userBits[Chatter] -= bitCost;

                                                if (Properties.Settings.Default.isBitMsgEnabled)
                                                {
                                                    client.SendMessage(channelId, $"{Chatter}, You have {userBits[Chatter]} bits remaining.");
                                                }

                                                Console.WriteLine($"[{timestamp}] [{Chatter}]: {e.Command.ChatMessage.Message} Cost:{bitCost} Remaining bits:{userBits[Chatter]}");

                                                // Save the updated bit data
                                                LogHandler.WriteUserBitsToJson("user_bits.json");
                                            }
                                            catch (ArgumentException)
                                            {
                                                // Send an error message if the key is not supported
                                                client.SendMessage(channelId, $"{Chatter}, the key '{keyToSend}' is not supported.");
                                            }
                                            catch (Exception ex)
                                            {
                                                // Send a general error message
                                                client.SendMessage(channelId, $"{Chatter}, there was an error processing your command: {ex.Message}");
                                            }
                                        }
                                        else
                                        {
                                            // Notify the user to specify a key
                                            client.SendMessage(channelId, $"{Chatter}, please specify a key to send (e.g., !sendkey A or ESC).");
                                        }
                                    }
                                    else
                                    {
                                        // Send message indicating insufficient bits
                                        client.SendMessage(channelId, $"{Chatter}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                    }
                                }
                                else
                                {
                                    // Send message indicating invalid hold time value
                                    client.SendMessage(channelId, "Invalid hold time value.");
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
                            client.SendMessage(channelId, $"{Chatter}, your bit data is not found!");
                        }
                    }
                    break;

                case "sbgamble":
                    if (Properties.Settings.Default.isBitGambleEnabled)
                    {
                        // Check if the user has enough bits to gamble
                        if (userBits.ContainsKey(Chatter))
                        {
                            // Get the amount the user wants to gamble from the message
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
                                            client.SendMessage(channelId, $"{Chatter}, you don't have any bits to gamble.");
                                            break;
                                        }
                                    }
                                    // Check if the input contains a percentage
                                    else if (gambleInput.EndsWith("%"))
                                    {
                                        string percentageString = gambleInput.TrimEnd('%');
                                        if (int.TryParse(percentageString, out int percentage) && percentage > 0 && percentage <= 100)
                                        {
                                            // Calculate the gamble amount as a percentage of the user's available bits
                                            gambleAmount = (int)(userBits[Chatter] * (percentage / 100.0));

                                            // If the percentage results in zero, inform the user
                                            if (gambleAmount == 0)
                                            {
                                                client.SendMessage(channelId, $"{Chatter}, you can't gamble less than 1 bit.");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            // Invalid percentage input
                                            client.SendMessage(channelId, $"{Chatter}, please specify a valid percentage (1-100%).");
                                            break;
                                        }
                                    }
                                    else if (int.TryParse(gambleInput, out gambleAmount))
                                    {
                                        // If the input is a specific amount (not a percentage or 'all')
                                        if (gambleAmount <= 0)
                                        {
                                            client.SendMessage(channelId, $"{Chatter}, you can't gamble 0 or negative bits.");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // Invalid gamble amount format
                                        client.SendMessage(channelId, $"{Chatter}, please specify a valid amount or percentage to gamble (e.g., !sbgamble 100, !sbgamble 50%, or !sbgamble all).");
                                        break;
                                    }
                                }
                                else
                                {
                                    // Invalid command format
                                    client.SendMessage(channelId, $"{Chatter}, please specify a valid amount or percentage to gamble (e.g., !sbgamble 100, !sbgamble 50%, or !sbgamble all).");
                                    break;
                                }
                            }
                            else
                            {
                                // User didn't specify an amount
                                client.SendMessage(channelId, $"{Chatter}, please specify an amount or percentage to gamble (e.g., !sbgamble 100, !sbgamble 50%, or !sbgamble all).");
                                break;
                            }

                            // Check if the user has enough bits to gamble
                            if (userBits[Chatter] >= gambleAmount)
                            {
                                // Retrieve the cooldown time from BitGambleCDBox
                                int cooldownSeconds = 0;
                                if (int.TryParse(controlMenu.BitGambleCDBox.Text, out cooldownSeconds) && cooldownSeconds >= 0)
                                {
                                    // Check if the user is on cooldown
                                    if (lastGambleTime.ContainsKey(Chatter))
                                    {
                                        DateTime lastGamble = lastGambleTime[Chatter];
                                        TimeSpan timeSinceLastGamble = DateTime.Now - lastGamble;

                                        if (timeSinceLastGamble.TotalSeconds < cooldownSeconds)
                                        {
                                            int remainingCooldown = cooldownSeconds - (int)timeSinceLastGamble.TotalSeconds;
                                            break;
                                        }
                                    }

                                    // Get the win chance from the ControlMenu
                                    int winChance = 0;
                                    if (int.TryParse(controlMenu.BitChanceBox.Text, out winChance) && winChance >= 0 && winChance <= 100)
                                    {
                                        // Generate a random number between 1 and 100 to determine if the user wins
                                        Random rand = new Random();
                                        int outcome = rand.Next(1, 101);  // Random number between 1 and 100

                                        // Check if the user wins
                                        if (outcome <= winChance)
                                        {
                                            // User wins, they gain double the gamble amount
                                            int winnings = gambleAmount * 2;
                                            userBits[Chatter] += winnings;

                                            // Log the result and update the user's bits
                                            LogHandler.LogBits(Chatter, winnings, timestamp);
                                            LogHandler.WriteUserBitsToJson("user_bits.json");

                                            // Send a success message to the user
                                            client.SendMessage(channelId, $"{Chatter}, you won! You gambled {gambleAmount} bits and won {winnings} bits. You now have {userBits[Chatter]} bits.");
                                            Console.WriteLine($"[{timestamp}] [{Chatter}] Gambled {gambleAmount} bits, won {winnings} bits. Total bits: {userBits[Chatter]}");
                                        }
                                        else
                                        {
                                            // User loses, they lose the gamble amount
                                            userBits[Chatter] -= gambleAmount;

                                            // Log the result and update the user's bits
                                            LogHandler.LogBits(Chatter, -gambleAmount, timestamp);
                                            LogHandler.WriteUserBitsToJson("user_bits.json");

                                            // Send a failure message to the user
                                            client.SendMessage(channelId, $"{Chatter}, you lost! You gambled {gambleAmount} bits and lost. You now have {userBits[Chatter]} bits.");
                                            Console.WriteLine($"[{timestamp}] [{Chatter}] Gambled {gambleAmount} bits, lost. Total bits: {userBits[Chatter]}");
                                        }
                                    }
                                    else
                                    {
                                        // Invalid win chance value
                                        client.SendMessage(channelId, "Invalid win chance value. Please ensure it's between 0 and 100.");
                                    }

                                    // Update the last gamble time
                                    lastGambleTime[Chatter] = DateTime.Now;
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid cooldown time value in BitGambleCDBox.");
                                }
                            }
                            else
                            {
                                // Not enough bits
                                client.SendMessage(channelId, $"{Chatter}, you don't have enough bits to gamble {gambleAmount} bits! You currently have {userBits[Chatter]} bits.");
                            }
                        }
                        else
                        {
                            // User doesn't have any bits
                            client.SendMessage(channelId, $"{Chatter}, you don't have any bits to gamble.");
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
                        if (Properties.Settings.Default.isModBitsEnabled)
                        {
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(Chatter))
                            {
                                ChatCommandMethods.AddBitCommand(client, e);
                            }
                            else
                            {
                                client.SendMessage(e.Command.ChatMessage.Channel, "You are not authorized to use this command.");
                            }
                        }
                        break;

                    case "rembits":
                        if (Properties.Settings.Default.isModBitsEnabled)
                        {
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(Chatter))
                            {
                                ChatCommandMethods.RemoveBitCommand(client, e);
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
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(Chatter))
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

                    case "sbadd":
                        if (Properties.Settings.Default.isModAddEnabled)
                        {
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(Chatter))
                            {
                                // Parse the arguments for !sbadd
                                var sbaddArgs = e.Command.ArgumentsAsString.Split(' ');

                                if (sbaddArgs.Length >= 3)
                                {
                                    string commandName = sbaddArgs[0].ToLower();  // Command name
                                    if (int.TryParse(sbaddArgs[1], out int bitCost))
                                    {
                                        string methods = string.Join(" ", sbaddArgs.Skip(2));  // Join methods if there are multiple

                                        // Add the command and capture any error message or success message
                                        var commandHandler = new CustomCommandHandler("CustomCommands.json");
                                        string resultMessage = commandHandler.AddChatCommand(commandName, bitCost, methods);

                                        // Send the result message back to the channel
                                        client.SendMessage(channelId, resultMessage);
                                    }
                                    else
                                    {
                                        client.SendMessage(channelId, "Invalid bit cost. Please provide a valid number.");
                                    }
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid syntax. Usage: !sbadd {commandName} {cost} {methods}");
                                }
                            }
                        }
                        break;

                    case "sbremove":
                        if (Properties.Settings.Default.isModRemoveEnabled)
                        {
                            if (!Properties.Settings.Default.isModWhitelistEnabled || LogHandler.IsUserInWhitelist(Chatter))
                            {
                                // Parse the arguments for !sbremove
                                var sbremoveArgs = e.Command.ArgumentsAsString.Split(' ');

                                if (sbremoveArgs.Length == 1)
                                {
                                    string commandNameToRemove = sbremoveArgs[0].ToLower();  // Command name to remove

                                    // Remove the command
                                    var commandHandler = new CustomCommandHandler("CustomCommands.json");
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
                        // Parse the arguments for !sbadd
                        var sbaddArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (sbaddArgs.Length >= 3)
                        {
                            string commandName = sbaddArgs[0].ToLower();  // Command name
                            if (int.TryParse(sbaddArgs[1], out int bitCost))
                            {
                                string methods = string.Join(" ", sbaddArgs.Skip(2));  // Join methods if there are multiple

                                // Add the command and capture any error message or success message
                                var commandHandler = new CustomCommandHandler("CustomCommands.json");
                                string resultMessage = commandHandler.AddChatCommand(commandName, bitCost, methods);

                                // Send the result message back to the channel
                                client.SendMessage(channelId, resultMessage);
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid bit cost. Please provide a valid number.");
                            }
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid syntax. Usage: !sbadd {commandName} {cost} {methods}");
                        }
                        break;

                    case "sbremove":
                        // Parse the arguments for !sbremove
                        var sbremoveArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (sbremoveArgs.Length == 1)
                        {
                            string commandNameToRemove = sbremoveArgs[0].ToLower();  // Command name to remove

                            // Remove the command
                            var commandHandler = new CustomCommandHandler("CustomCommands.json");
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
            Thread.Sleep(2000);

            // Convert the interval to milliseconds
            int intervalMilliseconds = autoSendMessageCD * 1000;

            // Create a Timer object to run the method immediately and then reschedule it
            timer = new System.Threading.Timer(AutoMessageSender, null, 0, intervalMilliseconds);
        }
    }
}


