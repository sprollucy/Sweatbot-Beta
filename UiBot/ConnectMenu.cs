using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UiBot
{
    public partial class ConnectMenu : Form
    {
        private MainBot bot;
        private CommandHandler commandHandler; // Add this field
        private TextBoxWriter textBoxWriter;
        string timestamp = DateTime.Now.ToString("MM/dd HH:mm:ss");


        public ConnectMenu()
        {
            ControlMenu controlMenu = new ControlMenu();
            SettingMenu settingMenu = new SettingMenu();
            bot = new MainBot();
            commandHandler = new CommandHandler(); // Initialize the command handler
            InitializeComponent();
            InitializeConsole();
            controlMenu.LoadSettings();
            this.TopLevel = false;

            // Hook up the KeyPress event for the messageTextBox
            messageTextBox.KeyPress += MessageTextBox_KeyPress;
            pauseCommands.Checked = Properties.Settings.Default.isCommandsPaused;
        }

        private void InitializeConsole()
        {
            // Redirect standard output to the consoleTextBox and optionally to the file if isDebugToFile is true
            textBoxWriter = new TextBoxWriter(consoleTextBox, Properties.Settings.Default.isWriteDebugOn);
            Console.SetOut(textBoxWriter);
            Console.WriteLine($"[{timestamp}]Console initialized.");
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

        private void connectButton_Click(object sender, EventArgs e)
        {
            bot.Connect();
        }

        private void ConnectMenu_Load(object sender, EventArgs e)
        {
            // Load and display commands on form load
            LoadCommands();
            DisplayCommands();
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            bot.Disconnect();
        }

        private void stopGoose_Click(object sender, EventArgs e)
        {
            foreach (Process process in Process.GetProcessesByName("GooseDesktop"))
            {
                process.Kill();
            }
        }

        private void pauseCommands_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isCommandsPaused = pauseCommands.Checked;
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
            string commandsFilePath = Path.Combine("Data" ,"bin" , "CustomCommands.json");

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
                customCommandBox.Text = $"Available commands:\n{commandListMessage}";
            }
            else
            {
                customCommandBox.Text = "No commands available.";
            }
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
}
