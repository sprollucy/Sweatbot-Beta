﻿using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


/* TODO **
 * 
 */

namespace UiBot
{

    internal class MainBot : IDisposable
    {
        //References
        Counter counter = new Counter();
        ChatCommandMethods chatCommandMethods = new ChatCommandMethods();
        ControlMenu controlMenu = new ControlMenu();

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
        private static TwitchPubSub pubSub;
        private static string channelId;

        //spam command
        private DateTime lastTradersCommandTimer = DateTime.MinValue;
        private System.Threading.Timer timer;
        public int autoSendMessageCD;

        internal MainBot()
        {
            LoadCredentialsFromJSON();
            LoadUserBitsFromJson("user_bits.json");
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
                pubSub.Disconnect();

                // Unsubscribe from events
                client.OnConnected -= Client_OnConnected;
                client.OnError -= Client_OnError;
                pubSub.OnFollow -= PubSub_OnFollow;

                isBotConnected = false;
            }
        }

        internal void Connect()
        {
            if (!isBotConnected)
            {
                Console.WriteLine($"[Sweat Bot]: Connecting to {channelId}...");
                InitializeTwitchClient();
                InitializePubSub();
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

                if(channelId == null) 
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

        private void InitializePubSub()
        {
            pubSub = new TwitchPubSub();
            pubSub.OnFollow += PubSub_OnFollow;
            pubSub.ListenToFollows(channelId);
            pubSub.Connect();
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            if (e.ChatMessage.Message.StartsWith("!"))
            {
                Console.WriteLine($"[{timestamp}] [{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");
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
                        WriteUserBitsToJson("user_bits.json");
                        LogHandler.LogBits(e.ChatMessage.DisplayName, bonusAmount, timestamp);
                    }
                    else
                    {
                        // Error parsing bonus amount, use a default value or handle the error accordingly
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName}, invalid bonus amount format. Using default value.");
                        bonusAmount = 100; // Set a default bonus amount
                        userBits.Add(e.ChatMessage.DisplayName, bonusAmount);
                        WriteUserBitsToJson("user_bits.json");
                    }
                }
            }

            if (e.ChatMessage.Bits > 0)
            {
                int bitsGiven = e.ChatMessage.Bits;
                UpdateUserBits(e.ChatMessage.DisplayName, bitsGiven);
                LogHandler.LogBits(e.ChatMessage.DisplayName, bitsGiven, timestamp);
            }
        }

        private void PubSub_OnFollow(object sender, OnFollowArgs e)
        {
            string followerUsername = e.DisplayName;
            Console.WriteLine($"New follower: {followerUsername}");
            client.SendMessage(channelId, $"{followerUsername} Thank you for following");
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
            Process[] gname = Process.GetProcessesByName("GooseDesktop");

//antispam cooldowns
            int wipecooldownDuration = 30;
            int wipestatcooldownDuration = 30;
            int helpCooldownDuration = 30;
            int aboutCooldownDuration = 10;
            int tradersCooldownDuration = 90;
            int bitcostCooldownDuration = 30;
            int lastHow2useTimerDuration = 30;
            TimeSpan timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastStatCommandTimer;
            string timestamp = DateTime.Now.ToString("HH:mm:ss");

//Normal Commands
            switch (e.Command.CommandText.ToLower())
            {
                case "help":
                    // Calculate the time elapsed since the last "help" command execution
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastHelpCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= helpCooldownDuration)
                    {
                        chatCommandMethods.lastHelpCommandTimer = DateTime.Now; // Update the last "help" execution time

                        client.SendMessage(channelId, "!how2use, !about, !traders, !mybits, !stats, !wipestats. Use !bitcost to check which commands are available and to see the prices");
                    }
                    break;

                case "how2use":
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastHow2useTimer;

                    if (timeSinceLastExecution.TotalSeconds >= lastHow2useTimerDuration)
                    {
                        client.SendMessage(channelId, "To use, just cheer Bits in the chat. The bot will keep track of how many you give. Type !bitcost to see what you can do with them. Then, just enter the command you want to use in the chat if you have enough Bits to spend. Use !mybits to check how many Bits you have stored!");
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

                case "stats":
                    // Calculate the time elapsed since the last execution
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastStatCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= wipecooldownDuration)
                    {
                        chatCommandMethods.lastStatCommandTimer = DateTime.Now; // Update the last execution time
                        client.SendMessage(channelId, $"@{channelId} has died {chatCommandMethods.deathCount} times today, escaped {chatCommandMethods.survivalCount} times, and killed {chatCommandMethods.killCount} players");
                    }

                    break;

                case "mybits":
                    string requester = e.Command.ChatMessage.DisplayName;
                    if (userBits.ContainsKey(requester))
                    {
                        client.SendMessage(channelId, $"{requester}, you have {userBits[requester]} bits");
                    }
                    else
                    {
                        client.SendMessage(channelId, $"{requester}, you have no bits");
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
                                { "WiggleCooldownTextBox", ("wiggle", () => Properties.Settings.Default.IsWiggleEnabled) },
                                { "DropCooldownTextBox", ("dropkit", () => Properties.Settings.Default.IsDropEnabled) },
                                { "GooseCooldownTextBox", ("goose", () => Properties.Settings.Default.IsGooseEnabled) },
                                { "RandomKeyCooldownTextBox", ("randomkeys", () => Properties.Settings.Default.IsKeyEnabled) },
                                { "TurnCooldownTextBox", ("turn", () => Properties.Settings.Default.IsTurnEnabled) },
                                { "OneClickCooldownTextBox", ("pop", () => Properties.Settings.Default.IsPopEnabled) },
                                { "GrenadeCooldownTextBox", ("grenadesound", () => Properties.Settings.Default.isGrenadeEnabled) },
                                { "DropBagCooldownTextBox", ("dropbag", () => Properties.Settings.Default.isDropBagEnabled) },
                                { "GrenadeCostBox", ("360grenade", () => Properties.Settings.Default.isGrenadeTossEnabled) },
                                { "CrouchBoxCost", ("crouch", () => Properties.Settings.Default.isCrouchEnabled) },
                                { "magDumpCost", ("magdump", () => Properties.Settings.Default.isMagDumpEnabled) },
                                { "HoldAimCost", ("holdaim", () => Properties.Settings.Default.isHoldAimEnabled) },
                                { "mag360Cost", ("360magdump", () => Properties.Settings.Default.isMagDump360Enabled) },
                                { "proneCostBox", ("prone", () => Properties.Settings.Default.isProneEnabled) },
                                { "voiceLineCostBox", ("voiceline", () => Properties.Settings.Default.isVoiceLineEnabled) },
                                { "reloadCostBox", ("reload", () => Properties.Settings.Default.isReloadEnabled) },
                                { "praisesunCostBox", ("praisesun", () => Properties.Settings.Default.isPraiseSunEnabled) },
                                { "touchgrassCostBox", ("touchgrass", () => Properties.Settings.Default.isTouchGrassEnabled) },
                                { "knifeoutCostBox", ("knifeout", () => Properties.Settings.Default.isKnifeOutEnabled) },
                                { "jumpCostBox", ("jump", () => Properties.Settings.Default.isJumpEnabled) },
                                { "windowsmuteCostBox", ("mutewindows", () => Properties.Settings.Default.isMuteWindowsEnabled) },
                                { "walkCostBox", ("walk", () => Properties.Settings.Default.isWalkEnabled) },
                                { "hotmicCostBox", ("hotmic", () => Properties.Settings.Default.isHotMicEnabled) },
                                { "normgrenadeCostBox", ("grenadetoss", () => Properties.Settings.Default.isNormGrenadeEnabled) },
                                { "weaponswapCostBox", ("weaponswap", () => Properties.Settings.Default.isWeaponSwapEnabled) },
                                { "firemodeCostBox", ("firemode", () => Properties.Settings.Default.isWeaponSwapEnabled) }
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
                        List<string> enabledCommandCosts = enabledCommandDetails.Select(detail => $"!{detail.Label} - {detail.Cost}").ToList();

                        if (enabledCommandCosts.Count > 0)
                        {
                            string message = $"{e.Command.ChatMessage.DisplayName}, these are the available commands: {string.Join(", ", enabledCommandCosts)}";
                            client.SendMessage(channelId, message);
                        }
                        else
                        {
                            string message = $"{e.Command.ChatMessage.DisplayName}, All sweatbot commands are disabled";
                            client.SendMessage(channelId, message);
                        }
                    }
                    break;

                case "wipestats":
                    // Calculate the time elapsed since the last execution
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastWipeStatCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= wipestatcooldownDuration)
                    {
                        chatCommandMethods.lastWipeStatCommandTimer = DateTime.Now; // Update the last execution time
                        client.SendMessage(channelId, $"@{channelId} has died {counter.AllDeath} times, killed {counter.AllKillCount} players, and escaped {counter.SurvivalCount} raids this wipe.");
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
                            var resetTimeData = traderResetInfoService.ReadJsonDataFromFile("Data/resetTime.json");

                            if (!string.IsNullOrEmpty(resetTimeData))
                            {
                                // Deserialize the JSON data
                                var traderResetResponse = JsonConvert.DeserializeObject<TraderResetInfoService.TraderResetResponse>(resetTimeData);

                                if (traderResetResponse != null && traderResetResponse.Data != null && traderResetResponse.Data.Traders != null)
                                {
                                    foreach (var trader in traderResetResponse.Data.Traders)
                                    {
                                        string traderName = trader.Name;
                                        string resetTime = trader.ResetTime;

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

                                            // Debugging: Print resetTime and timeRemaining
                                            Console.WriteLine($"Trader Name: {traderName}, Reset Time: {resetTime}");
                                            Console.WriteLine($"Time Remaining: {timeRemaining}");

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
                    else
                    {
                        // Send message indicating the command is disabled
                        client.SendMessage(channelId, "Traders command is currently disabled.");
                    }
                    break;


//Bit Commands

                case "dropbag":
                    if (Properties.Settings.Default.isDropBagEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.DropBagCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "dropbag", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    // Execute the command
                                    chatCommandMethods.BagDrop();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, dropbag used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        // Send message indicating the command is disabled
                        client.SendMessage(channelId, "Drop Bag command is currently disabled.");
                    }
                    break;

                case "goose":
                    if (!Properties.Settings.Default.IsGooseEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {
                        client.SendMessage(channelId, "Goose command is currently disabled.");
                    }
                    else if (gname.Length > 0)
                    {
                        client.SendMessage(channelId, "Goose is already running!");
                    }
                    else
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cost textbox value to an integer
                            if (int.TryParse(controlMenu.GooseCooldownTextBox.Text, out int gooseCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= gooseCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "goose", gooseCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= gooseCost;

                                    // Get the directory where the executable is located
                                    string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

                                    // Combine the directory with the "Goose" folder and the filename to get the full path to GooseDesktop.exe
                                    string gooseExePath = Path.Combine(exeDirectory, "Goose", "GooseDesktop.exe");

                                    if (File.Exists(gooseExePath))
                                    {
                                        // Generate a random runtime between 2 and 5 minutes.
                                        Random random = new Random();
                                        int runtimeMinutes = random.Next(2, 6); // Adjust the range as needed
                                        TimeSpan runtime = TimeSpan.FromMinutes(runtimeMinutes);

                                        // Start the Goose process and store it in the gooseProcess variable.
                                        chatCommandMethods.gooseProcess = Process.Start(gooseExePath);
                                        chatCommandMethods.lastGooseCommandTime = DateTime.Now;

                                        // Send a message indicating how long the Goose will run
                                        client.SendMessage(channelId, $"Goose is running for {runtimeMinutes} minutes!");

                                        // Save the updated bit data
                                        WriteUserBitsToJson("user_bits.json");
                                        // Schedule a task to stop the Goose process after the random runtime.
                                        Task.Run(() =>
                                        {
                                            Thread.Sleep(runtime);
                                            if (!chatCommandMethods.gooseProcess.HasExited)
                                            {
                                                chatCommandMethods.gooseProcess.Kill(); // Terminate the Goose process.
                                                client.SendMessage(channelId, "Goose has been terminated.");
                                            }
                                        });
                                    }
                                    else
                                    {
                                        client.SendMessage(channelId, "GooseDesktop.exe not found in the 'Goose' folder. Please make sure it's in the correct location.");
                                    }
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {gooseCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cost value
                                client.SendMessage(channelId, "Invalid cost value");
                            }
                        }
                        else
                        {
                            // Send message indicating user's bits data not found
                            client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, your bit data is not found!");
                        }
                    }
                    break;

                case "killgoose":
                    if (gname.Length > 0)
                    {
                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cost textbox value to an integer
                            if (int.TryParse(controlMenu.GooseCooldownTextBox.Text, out int killGooseCost))
                            {
                                LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "killgoose", killGooseCost, userBits, timestamp);

                                // Deduct the cost of the command
                                userBits[e.Command.ChatMessage.DisplayName] -= killGooseCost;

                                // Send a message indicating that the Goose is dead
                                client.SendMessage(channelId, "Goose is DEAD");

                                // Terminate the Goose process
                                foreach (Process process in Process.GetProcessesByName("GooseDesktop"))
                                {
                                    process.Kill();
                                }
                                // Save the updated bit data
                                WriteUserBitsToJson("user_bits.json");
                            }
                            else
                            {
                                // Send message indicating invalid cost value
                                client.SendMessage(channelId, "Invalid cost value");
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
                        // Send message indicating that the Goose is already dead
                        client.SendMessage(channelId, "Goose is already DEAD");
                    }
                    break;

                case "wiggle":
                    if (Properties.Settings.Default.IsWiggleEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.WiggleCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "wiggle", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    // Execute the command
                                    chatCommandMethods.WiggleMouse(4, 30, 50); //format is turns, distance in px, delay between move

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, wiggle used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "Wiggle command is currently disabled.");
                    }
                    break;

                case "turn":
                    if (Properties.Settings.Default.IsTurnEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.TurnCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "turn", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    // Randomly decide whether to move the mouse to the right or left
                                    bool moveRight = (new Random()).Next(2) == 0;

                                    chatCommandMethods.TurnRandom(2000);

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, turn used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "Turn command is currently disabled.");
                    }
                    break;

                case "randomkeys":
                    if (Properties.Settings.Default.IsKeyEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {
                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.RandomKeyCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "randomkeys", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.SendRandomKeyPresses();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, randomkeys used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "Random Keys command is currently disabled.");
                    }
                    break;

                case "360grenade":
                    if (Properties.Settings.Default.isGrenadeTossEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.GrenadeCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "360grenade", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.GrenadeTossTurn(5000);

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, 360grenade used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "360grenadetoss command is currently disabled.");
                    }
                    break;

                case "360magdump":
                    if (Properties.Settings.Default.isMagDump360Enabled && !Properties.Settings.Default.isCommandsPaused)
                    {
                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.Mag360Cost.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "360magdump", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.MagDump360(3000);

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, 360magdump used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "360magdump command is currently disabled.");
                    }
                    break;

                case "dropkit":
                    if (Properties.Settings.Default.IsDropEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.DropCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "dropkit", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.SimulateButtonPressAndMouseMovement();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, dropkit used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "Drop command is currently disabled.");
                    }
                    break;

                case "pop":
                    if (Properties.Settings.Default.IsPopEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.OneClickCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "pop", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.PopShot();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, pop used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "Pop command is currently disabled.");
                    }
                    break;

                case "grenadesound":
                    if (Properties.Settings.Default.isGrenadeEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.GrenadeCooldownTextBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "grenadesound", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.GrenadeSound();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, grenadesound used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
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
                        client.SendMessage(channelId, "Grenade Toss command is currently disabled.");
                    }
                    break;

                case "crouch":
                    if (Properties.Settings.Default.isCrouchEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.CrouchBoxCost.Text, out int bitCost))
                            {
                                LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "crouch", bitCost, userBits, timestamp);

                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.CrouchorStand();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, crouch used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Crouch command is currently disabled.");
                    }
                    break;

                case "voiceline":
                    if (Properties.Settings.Default.isVoiceLineEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.VoicelineCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "voiceline", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.VoiceLine();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, voiceline used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "VoiceLine command is currently disabled.");
                    }
                    break;

                case "reload":
                    if (Properties.Settings.Default.isReloadEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.ReloadCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "reload", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.Reload();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, reload used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Reload command is currently disabled.");
                    }
                    break;

                case "prone":
                    if (Properties.Settings.Default.isProneEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.ProneCost.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "prone", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.Prone();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, prone used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Prone command is currently disabled.");
                    }
                    break;

                case "magdump":
                    if (Properties.Settings.Default.isMagDumpEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.MagDumpBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "magdump", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.MagDump();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, magdump used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Magdump command is currently disabled.");
                    }
                    break;

                case "holdaim":
                    if (Properties.Settings.Default.isHoldAimEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.HoldAimCost.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "holdaim", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.HoldAim();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, holdaim used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Holdaim command is currently disabled.");
                    }
                    break;

                case "praisesun":
                    if (Properties.Settings.Default.isPraiseSunEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.PraisesunCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "praisesun", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.CrouchorStand();
                                    chatCommandMethods.LookUp(3000);
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, praisesun used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Praise Sun command is currently disabled.");
                    }
                    break;

                case "touchgrass":
                    if (Properties.Settings.Default.isTouchGrassEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.TouchGrassCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "touchgrass", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.CrouchorStand();
                                    chatCommandMethods.LookDown(3000);
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, touchgrass used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "touchgrass command is currently disabled.");
                    }
                    break;

                case "knifeout":
                    if (Properties.Settings.Default.isKnifeOutEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.KnifeoutCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "knifeout", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.KnivesOnly();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, knifeout used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Knifeout command is currently disabled.");
                    }
                    break;

                case "jump":
                    if (Properties.Settings.Default.isJumpEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.JumpCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "jump", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;
                                    SendKeys.SendWait("W");
                                    SendKeys.SendWait(" ");
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, jump used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Jump command is currently disabled.");
                    }
                    break;

                case "mutewindows":
                    if (Properties.Settings.Default.isMuteWindowsEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.WindowsmuteCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "mutewindows", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.MuteWindows();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, mutewindows used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Mute Windows command is currently disabled.");
                    }
                    break;

                case "walk":
                    if (Properties.Settings.Default.isWalkEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.WalkCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "walk", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.Walk();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, walk used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Walk command is currently disabled.");
                    }
                    break;

                case "hotmic":
                    if (Properties.Settings.Default.isHotMicEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.HotMicCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "hotmic", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.HotMic();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, hotmic used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Hotmic command is currently disabled.");
                    }
                    break;

                case "grenadetoss":
                    if (Properties.Settings.Default.isNormGrenadeEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.NormGrenadeCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "grenadetoss", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.GrenadeToss();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, grenadetoss used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Grenade Toss command is currently disabled.");
                    }
                    break;

                case "weaponswap":
                    if (Properties.Settings.Default.isWeaponSwapEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.WindowsmuteCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "weaponswap", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.SwapWeapon();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, weaponswap used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Weapon Swap command is currently disabled.");
                    }
                    break;

                case "firemode":
                    if (Properties.Settings.Default.isFireModeEnabled && !Properties.Settings.Default.isCommandsPaused)
                    {


                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.FireModeCostBox.Text, out int bitCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= bitCost)
                                {
                                    LogHandler.LogCommand(e.Command.ChatMessage.DisplayName, "firemode", bitCost, userBits, timestamp);

                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= bitCost;

                                    chatCommandMethods.FireMode();
                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, firemode used! You have {userBits[e.Command.ChatMessage.DisplayName]} bits");

                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {bitCost} bits.");
                                }
                            }
                            else
                            {
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
                        client.SendMessage(channelId, "Weapon Swap command is currently disabled.");
                    }
                    break;
            }

//Mod Commands
            if (e.Command.ChatMessage.IsModerator)
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "help":
                        StringBuilder message = new StringBuilder("!death, !escape, !kill");

                        if (Properties.Settings.Default.isModBitsEnabled)
                        {
                            message.Append(", !addbits");
                        }

                        if (Properties.Settings.Default.isModRefundEnabled)
                        {
                            message.Append(", !refund");
                        }

                        client.SendMessage(channelId, message.ToString());
                        break;

                    case "death":
                        chatCommandMethods.deathCount = chatCommandMethods.deathCount + 1;
                        counter.IncrementAllDeath();
                        client.SendMessage(channelId, $"@{channelId} has died {chatCommandMethods.deathCount} times");
                        break;

                    case "kill":
                        chatCommandMethods.killCount = chatCommandMethods.killCount + 1;
                        counter.IncrementKillCount();
                        client.SendMessage(channelId, $"You got a kill! thats {chatCommandMethods.killCount} kills today!");
                        break;

                    case "escape":
                        chatCommandMethods.survivalCount = chatCommandMethods.survivalCount + 1;
                        counter.IncrementSurvivalCount();
                        client.SendMessage(channelId, $"@{channelId} has escaped {counter.SurvivalCount} times");
                        break;


                    case "addbits":
                        if (Properties.Settings.Default.isModBitsEnabled)
                        {
                            string[] args = e.Command.ArgumentsAsString.Split(' ');
                            if (args.Length == 2)
                            {
                                string username = args[0].StartsWith("@") ? args[0].Substring(1) : args[0]; // Remove "@" symbol if present
                                int bitsToAdd;
                                if (int.TryParse(args[1], out bitsToAdd))
                                {
                                    // Update user's bits
                                    if (userBits.ContainsKey(username))
                                    {
                                        userBits[username] += bitsToAdd;
                                    }
                                    else
                                    {
                                        userBits[username] = bitsToAdd;
                                    }
                                    WriteUserBitsToJson("user_bits.json"); // Write changes to JSON file
                                    LogHandler.LogAddbits(e.Command.ChatMessage.DisplayName, "addbits", bitsToAdd, username, userBits, timestamp);

                                    // Notify about successful update
                                    client.SendMessage(channelId, $"{bitsToAdd} bits added to {username}. New total: {userBits[username]} bits");
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid number of bits specified.");
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid syntax. Usage: !addbits [username] [bits]");
                            }
                        }
                        break;

                    case "refund":

                        if (Properties.Settings.Default.isModRefundEnabled)
                        {
                            string[] refundArgs = e.Command.ArgumentsAsString.Split(' ');

                            if (refundArgs.Length == 1)
                            {
                                string refundUsername = refundArgs[0].StartsWith("@") ? refundArgs[0].Substring(1) : refundArgs[0]; // Remove "@" symbol if present

                                ChatCommandMethods.RefundLastCommand(refundUsername, userBits, client, channelId);
                                LogHandler.LogRefundbits(e.Command.ChatMessage.DisplayName, refundUsername, userBits, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                WriteUserBitsToJson("user_bits.json"); // Write changes to JSON file
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid syntax. Usage: !refund [username]");
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
                        client.SendMessage(channelId, "!hi, !goose, !killgoose, !death, !escape, !resettoday, !resetallstats, !addbits, !refund");
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

                    case "death":
                        chatCommandMethods.deathCount = chatCommandMethods.deathCount + 1;
                        counter.IncrementAllDeath();
                        client.SendMessage(channelId, $"@{channelId} has died {chatCommandMethods.deathCount} times");
                        break;

                    case "resettoday":
                        client.SendMessage(channelId, $"Stream stats reset!");
                        chatCommandMethods.deathCount = 0;
                        chatCommandMethods.killCount = 0;
                        break;

                    case "escape":
                        counter.IncrementSurvivalCount();
                        client.SendMessage(channelId, $"@{channelId} has escaped {counter.SurvivalCount} times");
                        break;

                    case "kill":
                        chatCommandMethods.killCount = chatCommandMethods.killCount + 1;
                        counter.IncrementKillCount();
                        client.SendMessage(channelId, $"You got a kill! thats {chatCommandMethods.killCount} kills today!");
                        break;

                    case "resetallstats":
                        client.SendMessage(channelId, "All stats reset!");
                        counter.ResetAllDeath();
                        counter.ResetSurvivalCount();
                        chatCommandMethods.deathCount = 0;
                        break;

                    case "addbits":
                            string[] args = e.Command.ArgumentsAsString.Split(' ');
                            if (args.Length == 2)
                            {
                                string username = args[0].StartsWith("@") ? args[0].Substring(1) : args[0]; // Remove "@" symbol if present
                                int bitsToAdd;
                                if (int.TryParse(args[1], out bitsToAdd))
                                {
                                    // Update user's bits
                                    if (userBits.ContainsKey(username))
                                    {
                                        userBits[username] += bitsToAdd;
                                    }
                                    else
                                    {
                                        userBits[username] = bitsToAdd;
                                    }
                                    WriteUserBitsToJson("user_bits.json"); // Write changes to JSON file
                                    LogHandler.LogAddbits(e.Command.ChatMessage.DisplayName, "addbits", bitsToAdd, username, userBits, timestamp);

                                    // Notify about successful update
                                    client.SendMessage(channelId, $"{bitsToAdd} bits added to {username}. New total: {userBits[username]} bits");
                                }
                                else
                                {
                                    client.SendMessage(channelId, "Invalid number of bits specified.");
                                }
                            }
                            else
                            {
                                client.SendMessage(channelId, "Invalid syntax. Usage: !addbits [username] [bits]");
                            }
                            break;

                    case "refund":
                        string[] refundArgs = e.Command.ArgumentsAsString.Split(' ');

                        if (refundArgs.Length == 1)
                        {
                            string refundUsername = refundArgs[0].StartsWith("@") ? refundArgs[0].Substring(1) : refundArgs[0]; // Remove "@" symbol if present

                            ChatCommandMethods.RefundLastCommand(refundUsername, userBits, client, channelId);
                            LogHandler.LogRefundbits(e.Command.ChatMessage.DisplayName, refundUsername, userBits, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            WriteUserBitsToJson("user_bits.json"); // Write changes to JSON file
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
            string jsonFilePath = Path.Combine("Data", "Logon.json");

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

        private static void UpdateUserBits(string username, int bitsGiven)
        {
            if (userBits.ContainsKey(username))
            {
                userBits[username] += bitsGiven;
            }
            else
            {
                userBits.Add(username, bitsGiven);
            }

            WriteUserBitsToJson("user_bits.json");
        }

        static void LoadUserBitsFromJson(string fileName)
        {
            // Construct the file path to the "Data" folder
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string filePath = Path.Combine(directoryPath, fileName);

            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Data directory not found: {directoryPath}");
                userBits = new Dictionary<string, int>();
                return;
            }

            // Check if the JSON file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("User bits JSON file not found.");
                userBits = new Dictionary<string, int>();
                return;
            }

            // Deserialize JSON file to dictionary
            string json = File.ReadAllText(filePath);
            userBits = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        }

        static void WriteUserBitsToJson(string fileName)
        {
            // Construct the file path to the JSON file in the "Data" folder
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dataDirectory); // Ensure the directory exists
            string filePath = Path.Combine(dataDirectory, fileName);

            // Validate bits to ensure they are not negative
            foreach (var user in userBits)
            {
                if (user.Value < 0)
                {
                    // Handle negative bits (e.g., set to zero or log the issue)
                    userBits[user.Key] = 0;
                    Console.WriteLine($"Negative bits detected for user {user.Key}. Set to 0.");
                }
            }

            // Serialize dictionary to JSON
            string json = JsonConvert.SerializeObject(userBits, Formatting.Indented);

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

        public void FileBackupTimer()
        {
            // Convert the interval to milliseconds
            int intervalMilliseconds = 600000;

            // Create a Timer object to run the method immediately and then reschedule it
            timer = new System.Threading.Timer(AutoMessageSender, null, 0, intervalMilliseconds);
        }

        public void FileBackup()
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

//Auto Message
        public void LoadAutoMessageData()
        {
            try
            {
                string jsonFilePath = Path.Combine("Data", "CommandConfigData.json");

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
            // Convert the interval to milliseconds
            int intervalMilliseconds = autoSendMessageCD * 1000;

            // Create a Timer object to run the method immediately and then reschedule it
            timer = new System.Threading.Timer(AutoMessageSender, null, 0, intervalMilliseconds);
        }

    }

}
