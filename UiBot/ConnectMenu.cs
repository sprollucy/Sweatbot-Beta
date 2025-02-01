﻿using Newtonsoft.Json;
using System.Diagnostics;
using TwitchLib.Client;
using TwitchLib.Communication.Interfaces;
using Timer = System.Windows.Forms.Timer;

namespace UiBot
{
    public partial class ConnectMenu : Form
    {
        private MainBot bot;
        private CommandHandler commandHandler; // Add this field
        private TextBoxWriter textBoxWriter;
        private Timer economyTimer;  // Timer for periodic updates
        private Timer memoryTimer;   // Timer for memory updates
        private int previousEconomyValue;  // Initialize a variable to hold the previous economy value
        private int totalSpent;  // Track the total amount spent
        private int lastSpent;
        string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");
        private bool isExpanded = false; // Track the state of the panel

        private FileSystemWatcher fileWatcher;

        private string logFilePath;
        private List<string> logEntries;
        public static Dictionary<string, int> userBits = new Dictionary<string, int>();
        private static readonly object fileLock = new object();

        public ConnectMenu()
        {
            ControlMenu controlMenu = new ControlMenu();
            SettingMenu settingMenu = new SettingMenu();
            bot = new MainBot();
            commandHandler = new CommandHandler();
            InitializeComponent();
            InitializeConsole();
            controlMenu.LoadSettings();
            this.TopLevel = false;

            pauseCommands.Checked = Properties.Settings.Default.isCommandsPaused;
            economyCheckBox.Checked = Properties.Settings.Default.isEconomyOn;

            // Initialize the Timer
            economyTimer = new Timer();
            economyTimer.Interval = 2000;
            economyTimer.Tick += EconomyTimer_Tick;
            memoryTimer = new Timer();
            memoryTimer.Interval = 2000;
            memoryTimer.Tick += MemoryTimer_Tick;
            lastSpent = LoadTotalSpentFromJson();
            economyLastLabel.Text = $"{lastSpent}";

            // Regular chat commands
            regComLabel.MouseClick += chatComPanel_MouseClick;
            chatComPanel.Height = 0;
            chatComPanel.Width = 0;

            regComLabel.Location = new Point(56, 583);
            if (Properties.Settings.Default.isEconomyOn)
            {
                economyTimer.Start();
            }
            LoadTotalSpentFromJson();

            DisplayTotalBitCost();
            RamDebug();

            // Hook up the KeyPress event for the messageTextBox
            messageTextBox.KeyPress += MessageTextBox_KeyPress;


            // Refund
            string date = DateTime.Now.ToString("M-d-yy");
            string logFileName = $"{date} bitlog.txt";
            logFilePath = Path.Combine("Logs", logFileName);
            userBits = new Dictionary<string, int>();

            LoadLogEntries();

            // Set up the file watcher to monitor changes
            SetUpFileWatcher();

        }

        private void chatComPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // Toggle the height based on the current state
            if (isExpanded)
            {
                chatComPanel.Height = 0; // Collapse
                chatComPanel.Width = 0;
                regComLabel.Text = "Regular Chat Commands (click to expand)";

                regComLabel.Location = new Point(56, 583);
                pictureBox9.BackColor = Color.FromArgb(37, 37, 37);
                regComLabel.BackColor = Color.FromArgb(37, 37, 37);
            }
            else
            {
                chatComPanel.Height = 96; // Expand
                chatComPanel.Width = 749;
                regComLabel.Text = "Regular Chat Commands (click to minimize)";
                regComLabel.Location = new Point(56, 485);
                pictureBox9.BackColor = Color.FromArgb(71, 83, 92);
                regComLabel.BackColor = Color.FromArgb(71, 83, 92);

            }

            isExpanded = !isExpanded; // Toggle the state
        }

        private void InitializeConsole()
        {
            // Redirect standard output to the consoleTextBox and optionally to the file if isDebugToFile is true
            textBoxWriter = new TextBoxWriter(consoleTextBox, Properties.Settings.Default.isDebugOn);
            Console.SetOut(textBoxWriter);
            Console.WriteLine($"[{timestamp}]Console initialized.");
        }

        private void MessageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Process the command when Enter key is pressed
                ProcessCommand();
                e.Handled = true; // Prevent the Enter key from being added to the TextBox
            }
        }

        private void ProcessCommand()
        {
            string message = messageTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(message))
            {
                if (message.StartsWith("!"))
                {
                    // Extract command name without '!'
                    string commandName = message.TrimStart('!').ToLower();

                    if (bot.IsConnected)
                    {
                        // Send command to bot if connected
                        bot.SendMessage(message);
                    }
                    else
                    {
                        // Process command locally if not connected
                        bot.ProcessLocalCommand(commandName);
                    }
                }
                else
                {
                    // Send message as chat message
                    bot.SendMessage(message);
                }

                Console.WriteLine($"Message: {message}"); // Print the message to the console
                messageTextBox.Clear(); // Clear the TextBox after sending the message
            }
        }

        private void ConnectMenu_Load(object sender, EventArgs e)
        {
            // Load and display commands on form load
            LoadCommands();
            DisplayCommands();
            UpdateButtonVisibility(false);
            UpdateConnection();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            bot.Connect(); // Attempt to connect
            UpdateButtonVisibility(true); // Show the Disconnect button
            UpdateConnection();
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            bot.Disconnect(); // Attempt to disconnect
            UpdateButtonVisibility(false); // Hide the Disconnect button
            UpdateConnection();
        }

        private void UpdateButtonVisibility(bool isConnected)
        {
            disconnectButton.Visible = isConnected;
            connectButton.Enabled = !isConnected; // Optionally disable the Connect button while connected
        }

        private void UpdateConnection()
        {
            if (bot.IsConnected)
            {
                ModernMenu.isConnected = true;
                ModernMenu.Instance.UpdateConnection();
            }
            else
            {
                ModernMenu.isConnected = false;
                ModernMenu.Instance.UpdateConnection();
            }
        }

        private void pauseCommands_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isCommandsPaused = pauseCommands.Checked;
            if (pauseCommands.Checked)
            {
                Console.WriteLine("Commands are paused.");
            }
            else
            {
                Console.WriteLine("Commands are not paused.");
            }
            Properties.Settings.Default.Save();
        }

        private void twitchOpen_Click(object sender, EventArgs e)
        {
            // Get the channel name from channelBox2
            string channelName = Properties.Settings.Default.ChannelName;

            // Construct the Twitch URL with the channel name
            string twitchUrl = $"https://www.twitch.tv/{channelName}";

            // Open the Twitch URL in the default web browser
            Process.Start(new ProcessStartInfo
            {
                FileName = twitchUrl,
                UseShellExecute = true
            });
        }

        private void backupButton_Click(object sender, EventArgs e)
        {
            LogHandler.FileBackup();
        }

        private void LoadCommands()
        {
            string commandsFilePath = Path.Combine("Data", "bin", "CustomCommands.json");

            if (File.Exists(commandsFilePath))
            {
                var jsonData = File.ReadAllText(commandsFilePath);

                // Deserialize JSON into a dictionary
                var commandDictionary = JsonConvert.DeserializeObject<Dictionary<string, Command>>(jsonData);

                // Pass the dictionary to the command handler
                commandHandler.LoadCommands(commandDictionary);
            }
            else
            {
                MessageBox.Show("Commands file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCommands()
        {
            var allCommandsWithCosts = commandHandler.GetAllCommandsWithCosts();
            if (allCommandsWithCosts.Any())
            {
                var commandList = allCommandsWithCosts
                    .Select(cmd => $"{cmd.Key} ( {cmd.Value} )")
                    .ToList();

                // Join each command with a newline character
                string commandListMessage = string.Join(Environment.NewLine, commandList);
                customCommandBox.Text = $"{commandListMessage}";
            }
            else
            {
                customCommandBox.Text = "No commands available.";
            }
        }

        // Method to calculate the total bit cost from user_bits.json
        private int CalculateTotalBitCost()
        {
            int totalBitCost = 0;

            // Load the user_bits.json file
            string userBitsFilePath = Path.Combine("Data", "user_bits.json");

            if (File.Exists(userBitsFilePath))
            {
                var jsonData = File.ReadAllText(userBitsFilePath);

                // Deserialize JSON into a dictionary or list depending on the format
                var userBitsData = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);

                if (userBitsData != null)
                {
                    // Sum up the bit costs
                    foreach (var item in userBitsData)
                    {
                        totalBitCost += item.Value; // Assuming the JSON has a format like {"command": bitCost}
                    }
                }
            }
            else
            {
                MessageBox.Show("user_bits.json file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalBitCost;
        }

        public void DisplayTotalBitCost()
        {
            if (economyLabel != null && economySpentLabel != null)
            {
                int totalBitCost = CalculateTotalBitCost();

                // Display the current economy value (total bit cost)
                economyLabel.Text = $"{totalBitCost}";

                // If the total bit cost is less than the previous economy value, update the totalSpent
                if (totalBitCost < previousEconomyValue)
                {
                    int amountSpent = previousEconomyValue - totalBitCost;  // Calculate the amount removed
                    totalSpent += amountSpent;  // Accumulate the total spent amount

                    economySpentLabel.Text = $"{totalSpent}";  // Update the spent label
                }

                if (totalSpent < lastSpent + 1)
                {
                    economySpentLabel.ForeColor = Color.Black;
                }
                else
                {
                    economySpentLabel.ForeColor = Color.Green;
                }

                // Update the previous economy value for the next calculation
                previousEconomyValue = totalBitCost;
                SaveTotalSpentToJson(totalSpent);
            }
            else
            {
                Console.WriteLine("economyLabel or economySpentLabel is null.");
            }
        }


        private int LoadTotalSpentFromJson()
        {
            string filePath = Path.Combine("Data", "bin", "CommandConfigData.json");

            // Check if the file exists
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);

                // Return the lastSpent value from the JSON file, defaulting to 0 if not found
                return jsonObj.lastSpent ?? 0;
            }
            else
            {
                // If the file doesn't exist, return 0 by default
                Console.WriteLine("File not found. Returning default totalSpent value of 0.");
                return 0;
            }
        }
        private void SaveTotalSpentToJson(int totalSpent)
        {
            string filePath = Path.Combine("Data", "bin", "CommandConfigData.json");

            // Read the existing data from the JSON file
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);

                // Update the lastSpent field with the new totalSpent value
                jsonObj.lastSpent = totalSpent;

                // Serialize the updated object and save it back to the file
                string updatedJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(filePath, updatedJson);
            }
            else
            {
                // If the file doesn't exist, create it and save the new value
                var newData = new { lastSpent = totalSpent };
                string newJson = JsonConvert.SerializeObject(newData, Formatting.Indented);
                File.WriteAllText(filePath, newJson);
            }
        }

        private void economyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isEconomyOn = economyCheckBox.Checked;
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.isEconomyOn)
            {
                econoPanel.Visible = true;
            }
            else
            {
                econoPanel.Visible = false;
            }
        }

        public void RamDebug()
        {
            if (Properties.Settings.Default.isDebugOn)
            {
                memoryTimer.Start(); // Start the memory timer
                RamUsage();

                debugGroup.Visible = true;
            }
            else
            {
                debugGroup.Visible = false;
            }
        }

        private void RamUsage()
        {
            // Get the current process
            Process currentProcess = Process.GetCurrentProcess();

            // Get total physical memory usage (includes managed and unmanaged memory)
            long allocmemoryUsed = currentProcess.WorkingSet64;
            long memoryUsed = currentProcess.PrivateMemorySize64;
            long gcmemoryUsed = GC.GetTotalMemory(false);

            // Update the label with memory usage in MB
            ramLabel.Text = $"Actual Memory Usage: {memoryUsed / 1024 / 1024} MB";
            manramLabel.Text = $"Managed GC Memory: {gcmemoryUsed / 1024 / 1024} MB";
            allocramLabel.Text = $"Total Allocated Memory: {allocmemoryUsed / 1024 / 1024} MB";
        }

        private void MemoryTimer_Tick(object sender, EventArgs e)
        {
            RamUsage(); // Update the RAM usage display
        }

        private void EconomyTimer_Tick(object sender, EventArgs e)
        {
            // Check if economy is enabled
            if (Properties.Settings.Default.isEconomyOn)
            {
                DisplayTotalBitCost(); // Update the bit cost display
            }
        }

        private void ramSnapButton_Click(object sender, EventArgs e)
        {
            // Get the current process
            Process currentProcess = Process.GetCurrentProcess();

            // Get memory usage details
            long allocmemoryUsed = currentProcess.WorkingSet64;
            long memoryUsed = currentProcess.PrivateMemorySize64;
            long gcmemoryUsed = GC.GetTotalMemory(false);

            // Write memory usage details to the console
            Console.WriteLine($"[{timestamp}] Ram snapshot writen to 'Debug File.txt' in 'Logs' folder");
            Console.WriteLine($"[RamSnap] Managed GC Memory: {gcmemoryUsed / 1024 / 1024} MB");
            Console.WriteLine($"[RamSnap] Actual Memory Usage: {memoryUsed / 1024 / 1024} MB");
            Console.WriteLine($"[RamSnap] Total Allocated Memory: {allocmemoryUsed / 1024 / 1024} MB");
        }

        // Custom TextWriter to redirect Console output to a TextBox
        private class TextBoxWriter : System.IO.TextWriter
        {
            private TextBox textBox;
            private StreamWriter fileWriter;
            private bool isDebugToFile;

            public TextBoxWriter(TextBox textBox, bool isDebugToFile)
            {
                this.textBox = textBox;
                this.isDebugToFile = isDebugToFile;

                if (isDebugToFile && Properties.Settings.Default.isDebugOn)
                {
                    // Ensure the Log folder exists
                    Directory.CreateDirectory("Logs");
                    // Open the file in append mode
                    fileWriter = new StreamWriter(Path.Combine("Logs", "Debug File.txt"), true)
                    {
                        AutoFlush = true // Ensure the buffer is flushed to the file immediately
                    };
                }
            }

            public override void Write(char value)
            {
                if (textBox.IsHandleCreated)
                {
                    textBox.Invoke(new Action(() => textBox.AppendText(value.ToString())));
                }

                if (isDebugToFile)
                {
                    fileWriter?.Write(value); // Write to file if enabled
                }
            }

            public override void Write(string value)
            {
                if (textBox.IsHandleCreated)
                {
                    textBox.Invoke(new Action(() => textBox.AppendText(value)));
                }

                if (isDebugToFile)
                {
                    fileWriter?.Write(value); // Write to file if enabled
                }
            }

            public override void WriteLine(string value)
            {
                if (textBox.IsHandleCreated)
                {
                    textBox.Invoke(new Action(() => textBox.AppendText(value + Environment.NewLine)));
                }

                if (isDebugToFile)
                {
                    fileWriter?.WriteLine(value); // Write to file if enabled
                }
            }

            public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;

            // Close the file stream when done
            public void Close()
            {
                fileWriter?.Close();
            }
        }

        public class Command
        {
            public int BitCost { get; set; }
            public List<string> Methods { get; set; }
        }

        // This class would handle loading commands and their bit costs
        public class CommandHandler
        {
            private Dictionary<string, Command> commands = new Dictionary<string, Command>();

            public void LoadCommands(Dictionary<string, Command> commands)
            {
                this.commands = commands;
            }

            public Dictionary<string, int> GetAllCommandsWithCosts()
            {
                return commands.ToDictionary(
                    cmd => cmd.Key,
                    cmd => cmd.Value.BitCost);
            }
        }

        private void refundwinButton_Click(object sender, EventArgs e)
        {
                refundPanel.Location = new Point(53, 38); // Reset the position when collapsed
        }
        private void closeRefundButton_Click(object sender, EventArgs e)
        {
            refundPanel.Location = new Point(53, 626); // Reset the position when collapsed
        }

        /*
         * Refund
         */

        private void LoadLogEntries()
        {
            if (!File.Exists(logFilePath)) return;

            // Lock the section of the code that accesses the file
            lock (fileLock)
            {
                logEntries = File.ReadAllLines(logFilePath).ToList();
            }

            // Ensure this runs on the UI thread
            if (logListPanel.InvokeRequired)
            {
                logListPanel.Invoke(new Action(LoadLogEntries));
                return;
            }

            logListPanel.Controls.Clear(); // Clear previous controls

            // Load each log entry with a Refund button
            foreach (var entry in logEntries)
            {
                if (string.IsNullOrWhiteSpace(entry)) continue; // Ignore empty lines

                var panel = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Width = logListPanel.Width };
                var label = new Label { Text = entry, AutoSize = true, Width = 500 };
                var refundButton = new Button { Text = "Refund", Tag = entry, AutoSize = true, FlatStyle = FlatStyle.Flat };
                refundButton.Click += RefundButton_Click;
                panel.Controls.Add(label);
                panel.Controls.Add(refundButton);
                logListPanel.Controls.Add(panel); // Add the panel to the FlowLayoutPanel
            }
        }

        private void RefundButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is string logEntry)
            {
                ProcessRefund(logEntry);
                logEntries.Remove(logEntry);

                // Lock the section of the code that accesses the file
                lock (fileLock)
                {
                    File.WriteAllLines(logFilePath, logEntries); // Save updated log
                }

                // Remove refunded entry from UI
                RemoveLogEntryFromUI(logEntry);
            }
        }

        private void ProcessRefund(string logEntry)
        {
            string[] parts = logEntry.Split(new[] { " - ", " had ", " bits, used ", " command, costing ", " bits, now has " }, StringSplitOptions.None);
            if (parts.Length < 6) return;

            string userName = parts[1]; // Extracted from log
            int bitsCost;

            // Ensure correct parsing
            if (!int.TryParse(parts[4], out bitsCost))
            {
                Console.WriteLine($"Failed to parse bits cost for {userName}.");
                return;
            }

            // Update user's bits
            LogHandler.UpdateUserBits(userName, bitsCost);

            // Notify about successful update
            string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");
            bot.SendMessage($"{bitsCost} bits refunded to {userName}. New total: {MainBot.userBits[userName]} bits");
            Console.WriteLine($"[{timestamp}] Refunded {bitsCost} bits to {userName}. New total: {MainBot.userBits[userName]} bits");
        }


        public void RefunderMessge(string userName, int bitsCost)
        {
            // Ensure the latest balance is fetched after the refund
            if (userBits.ContainsKey(userName))
            {
                // Send the updated message with the correct balance
                bot.SendMessage($"Refunded {bitsCost} bits to {userName}. They now have {userBits[userName]} bits.");
            }
            else
            {
                bot.SendMessage($"Error: User {userName} not found for refund.");
            }

            // Reload the updated balances and log entries on the UI thread
            if (logListPanel.InvokeRequired)
            {
                logListPanel.Invoke(new Action(() =>
                {
                    LoadLogEntries(); // Reload log entries from the file
                }));
            }
            else
            {
                LoadLogEntries();
            }
        }

        private void RemoveLogEntryFromUI(string logEntry)
        {
            foreach (Control control in logListPanel.Controls)
            {
                if (control is FlowLayoutPanel panel && panel.Controls[0] is Label label && label.Text == logEntry)
                {
                    logListPanel.Controls.Remove(panel); // Remove the panel for the refunded entry
                    break; // Exit once the entry is found and removed
                }
            }
        }

        private void SetUpFileWatcher()
        {
            fileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(logFilePath),
                Filter = Path.GetFileName(logFilePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            fileWatcher.Changed += OnLogFileChanged;
            fileWatcher.Renamed += OnLogFileChanged; // in case the file is renamed
        }

        private void OnLogFileChanged(object sender, FileSystemEventArgs e)
        {
            // Ensure this runs on the UI thread
            if (logListPanel.InvokeRequired)
            {
                // Use a lambda to pass the correct parameters
                logListPanel.Invoke(new Action(() => OnLogFileChanged(sender, e)));
                return;
            }

            // Lock the section of the code that accesses the file
            lock (fileLock)
            {
                // Reload the log entries when the file changes
                LoadLogEntries();
            }
        }

    }
}