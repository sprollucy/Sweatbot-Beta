using System.Runtime.InteropServices;
using Newtonsoft.Json;
using TwitchLib.Client.Events;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using TwitchLib.Api.Helix;


/* TODO **
 * Move commands to seperate script. Its slightly overwhelming.
 * - Moved methods to ChatCommandMethods
 * Remove all cooldowns and convert to "bit" usage
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
                Console.WriteLine("[Bot]: Connecting...");
                InitializeTwitchClient();
                InitializePubSub();
                StartAutoMessage();

            }
        }


        private void InitializeTwitchClient()
        {
            if (!isBotConnected)
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.AccessToken) || string.IsNullOrEmpty(Properties.Settings.Default.ChannelName))
                {
                    MessageBox.Show("Please enter token access and channel name in the Settings Menu");
                    Console.WriteLine("[Bot]: Disconnected");
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
            Console.WriteLine($"[{timestamp}] [{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");

            if (Properties.Settings.Default.isChatBonusEnabled)
            {
                if (!userBits.ContainsKey(e.ChatMessage.DisplayName))
                {
                    int bonusAmount;
                    if (int.TryParse(controlMenu.BonusTextBox.Text, out bonusAmount))
                    {
                        // User is chatting for the first time, give them the specified bonus amount
                        userBits.Add(e.ChatMessage.DisplayName, bonusAmount);
                        client.SendMessage(channelId, $"{e.ChatMessage.DisplayName} welcome to the chat! Here is {bonusAmount} bits on the house, use !help for more info");
                        WriteUserBitsToJson("user_bits.json");
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

        static void LoadUserBitsFromJson(string filePath)
        {
            // Check if JSON file exists
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

        // Write user bits to JSON file
        static void WriteUserBitsToJson(string filePath)
        {
            // Serialize dictionary to JSON
            string json = JsonConvert.SerializeObject(userBits, Formatting.Indented);

            // Write JSON to file
            File.WriteAllText(filePath, json);
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
            Console.WriteLine("[Bot]: Connected");
        }

        public void SendMessage(string message)
        {
            if (client.IsConnected)
            {
                string channelName = channelId;

                // Send the message to the specified channel
                client.SendMessage(channelName, message);
            }
            else
            {
                Console.WriteLine("Bot is not connected to Twitch. Cannot send message.");
            }
        }


        //Chat 
        private async void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var traderResetInfoService = new TraderResetInfoService();

            Process[] pname = Process.GetProcessesByName("notepad");
            Process[] gname = Process.GetProcessesByName("GooseDesktop");

            //antispam cooldowns
            int wipecooldownDuration = 10;
            int wipestatcooldownDuration = 10;
            int helpCooldownDuration = 10;
            int aboutCooldownDuration = 10;
            int tradersCooldownDuration = 60;

            TimeSpan timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastStatCommandTimer;

            //Normal Commands
            switch (e.Command.CommandText.ToLower())
            {
                case "help":
                    // Calculate the time elapsed since the last "help" command execution
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastHelpCommandTimer;
                    if (timeSinceLastExecution.TotalSeconds >= helpCooldownDuration)
                    {
                        chatCommandMethods.lastHelpCommandTimer = DateTime.Now; // Update the last "help" execution time
                        client.SendMessage(channelId, "!about, !traders, !mybits, !drop, !goose, !help, !stats, !wipestats. These Commands cost bits to use !killgoose, !randomkeys,  !turn, !wiggle, !pop, !grenade, !dropbag, !grenadetoss, !magdump, !holdaim, !crouch, !dropkit,  use !bitprice to check the cost!");
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
                    LoadUserBitsFromJson("user_bits.json");
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
                    string wigCost = controlMenu.WiggleCooldownTextBox.Text;
                    string kitdropCost = controlMenu.DropCooldownTextBox.Text;
                    string gooseCost = controlMenu.GooseCooldownTextBox.Text;
                    string randomkeyCost = controlMenu.RandomKeyCooldownTextBox.Text;
                    string turnCost = controlMenu.TurnCooldownTextBox.Text;
                    string popCost = controlMenu.OneClickCooldownTextBox.Text;
                    string grenadeCost = controlMenu.GrenadeCooldownTextBox.Text;
                    string dropbagCost = controlMenu.DropBagCooldownTextBox.Text;
                    string grenadeTossCost = controlMenu.GrenadeCostBox.Text;  
                    string crouchCost = controlMenu.CrouchBoxCost.Text;
                    string magCost = controlMenu.MagDumpBox.Text;
                    string aimCost = controlMenu.HoldAimCost.Text;
                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, These are the cost of each command !randomkeys - {randomkeyCost},  !turn - {turnCost}, !wiggle - {wigCost}, !pop - {popCost}, !grenade - {grenadeCost}, !dropbag - {dropbagCost}, !grenadetoss - {grenadeTossCost}, !magdump - {magCost}, !holdaim - {aimCost}, !crouch - {crouchCost}, !dropkit - {kitdropCost}, !goose - {gooseCost} ");
                    break;


                case "wipestats":
                    // Calculate the time elapsed since the last execution
                    timeSinceLastExecution = DateTime.Now - chatCommandMethods.lastWipeStatCommandTimer;

                    if (timeSinceLastExecution.TotalSeconds >= wipestatcooldownDuration)
                    {
                        chatCommandMethods.lastWipeStatCommandTimer = DateTime.Now; // Update the last execution time
                        client.SendMessage(channelId, $"@{channelId} has died {counter.AllDeath} times, killed {counter.AllKillCount} players, and escaped {counter.SurvivalCount} raids this wipe.");
                    }
                    else
                    {
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
                            var resetTimeData = traderResetInfoService.ReadJsonDataFromFile("resetTime.json");

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

                    if(Properties.Settings.Default.IsCommandsPaused = true)
                    {

                    }


                case "dropbag":
                    if (Properties.Settings.Default.isDropBagEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.DropBagCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    // Execute the command
                                    chatCommandMethods.BagDrop();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {cooldownCost} bits.");
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
                    if (!Properties.Settings.Default.IsGooseEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        client.SendMessage(channelId, "Goose command is currently disabled.");
                    }
                    else if (gname.Length > 0)
                    {
                        client.SendMessage(channelId, "Goose is already running!");
                    }
                    else
                    {
                        // Parse the cooldown time from the gooseCooldownTextBox
                        if (int.TryParse(controlMenu.GooseCooldownTextBox.Text, out int cooldownSeconds))
                        {
                            TimeSpan gooseCooldown = TimeSpan.FromSeconds(cooldownSeconds);

                            if (DateTime.Now - chatCommandMethods.lastGooseCommandTime < gooseCooldown)
                            {
                                TimeSpan remainingCooldown = gooseCooldown - (DateTime.Now - chatCommandMethods.lastGooseCommandTime);
                                client.SendMessage(channelId, $"Goose command is on cooldown. You can use it again in {remainingCooldown.TotalSeconds:F0} seconds.");
                            }
                            else
                            {
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
                        }
                        else
                        {
                            client.SendMessage(channelId, "Invalid cost time");
                        }
                    }
                    break;

                case "killgoose":
                    if (gname.Length > 0)
                    {
                        client.SendMessage(channelId, "Goose is DEAD");
                        foreach (Process process in Process.GetProcessesByName("GooseDesktop"))
                        {
                            process.Kill();
                        }
                        break;
                    }
                    else
                    {
                        client.SendMessage(channelId, "Goose is already DEAD");
                    }

                    break;

                case "wiggle":
                    if (Properties.Settings.Default.IsWiggleEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.WiggleCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    // Execute the command
                                    chatCommandMethods.WiggleMouse(4, 30, 50); //format is turns, distance in px, delay between move

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is {cooldownCost} bits.");
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
                    if (Properties.Settings.Default.IsTurnEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.TurnCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    // Randomly decide whether to move the mouse to the right or left
                                    bool moveRight = (new Random()).Next(2) == 0;

                                    chatCommandMethods.TurnRandom(2000);

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is  {cooldownCost}  bits.");
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
                    if (Properties.Settings.Default.IsKeyEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.RandomKeyCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.SendRandomKeyPresses();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");3
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is  {cooldownCost}  bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
                                client.SendMessage(channelId, "Invalid cooldown value.");
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

                case "grenadetoss":
                    if (Properties.Settings.Default.isGrenadeTossEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.GrenadeCostBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.GrenadeToss(5000);

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is  {cooldownCost}  bits.");
                                }
                            }
                            else
                            {
                                // Send message indicating invalid cooldown value
                                client.SendMessage(channelId, "Invalid cooldown value.");
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

                case "dropkit":
                    if (Properties.Settings.Default.IsDropEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.DropCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.SimulateButtonPressAndMouseMovement();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is   {cooldownCost}   bits.");
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
                    if (Properties.Settings.Default.IsPopEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.OneClickCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.PopShot();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is   {cooldownCost}   bits.");
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

                case "grenade":
                    if (Properties.Settings.Default.isGrenadeEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.GrenadeCooldownTextBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.GrenadeSound();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is   {cooldownCost}   bits.");
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
                    if (Properties.Settings.Default.isCrouchEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.CrouchBoxCost.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.CrouchorStand();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is   {cooldownCost}   bits.");
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

                case "magdump":
                    if (Properties.Settings.Default.isMagDumpEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.MagDumpBox.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.MagDump();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is   {cooldownCost}   bits.");
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
                    if (Properties.Settings.Default.isHoldAimEnabled && !Properties.Settings.Default.IsCommandsPaused)
                    {
                        // Load user bits data
                        LoadUserBitsFromJson("user_bits.json");

                        // Check if the user's bits are loaded
                        if (userBits.ContainsKey(e.Command.ChatMessage.DisplayName))
                        {
                            // Convert the cooldown textbox value to an integer
                            if (int.TryParse(controlMenu.HoldAimCost.Text, out int cooldownCost))
                            {
                                // Check if the user has enough bits
                                if (userBits[e.Command.ChatMessage.DisplayName] >= cooldownCost)
                                {
                                    // Deduct the cost of the command
                                    userBits[e.Command.ChatMessage.DisplayName] -= cooldownCost;

                                    chatCommandMethods.HoldAim();

                                    // Save the updated bit data
                                    WriteUserBitsToJson("user_bits.json");
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you have {userBits[e.Command.ChatMessage.DisplayName]} bits");
                                }
                                else
                                {
                                    // Send message indicating insufficient bits
                                    client.SendMessage(channelId, $"{e.Command.ChatMessage.DisplayName}, you don't have enough bits to use this command! The cost is   {cooldownCost}   bits.");
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
            }

            //Mod Commands
            if (e.Command.ChatMessage.IsModerator)
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "help":
                        client.SendMessage(channelId, "!death, !escape, !kill");
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
                }
            }

            //Channel owner chat
            if (e.Command.ChatMessage.IsBroadcaster)
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "help":
                        client.SendMessage(channelId, "!hi, !goose, !killgoose, !death, !escape, !resettoday, !resetallstats, !addbits");
                        break;
                    case "hi":
                        client.SendMessage(channelId, "Hi Boss");
                        break;
                    //start windows proccesses 
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


                }
            }
        }

        public void LoadCredentialsFromJSON()
        {
            string jsonFilePath = "Logon.json";

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

        public void LoadAutoMessageData()
        {
            try
            {
                string jsonFilePath = "CommandConfigData.json";

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

        private System.Threading.Timer timer;

        public void StartAutoMessage()
        {
            // Convert the interval to milliseconds
            int intervalMilliseconds = autoSendMessageCD * 1000;

            // Create a Timer object to run the method immediately and then reschedule it
            timer = new System.Threading.Timer(AutoMessageSender, null, 0, intervalMilliseconds);
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
    }

}
