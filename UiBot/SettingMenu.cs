using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace UiBot
{
    public partial class SettingMenu : Form
    {
        public SettingMenu()
        {
            string packageVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0];

            InitializeComponent();
            this.TopLevel = false;
            accessBox.UseSystemPasswordChar = true;

            // Load existing data from the JSON file
            LoginData existingData = LoadCounterData();

            // Set the textBox1 and textBox2 controls with the loaded data
            accessBox.Text = existingData.BotToken;
            channelBox2.Text = existingData.ChannelName;
            versionNumber.Text = "Version: " + packageVersion;

            enableUpdateCheck.Checked = Properties.Settings.Default.isUpdateCheckEnabled;
            enableDebug.Checked = Properties.Settings.Default.isDebugOn;
            enableEFTtrade.Checked = Properties.Settings.Default.isTraderMenuEnabled;

            LoadChangelog();
        }

        // Load the changelog from the file and display it in changelogBox
        private void LoadChangelog()
        {
            string changelogFilePath = Path.Combine("Changelog.txt");

            if (File.Exists(changelogFilePath))
            {
                changelogBox.Text = File.ReadAllText(changelogFilePath);
            }
        }

        private void SettingMenu_Load(object sender, EventArgs e)
        {

        }

        private void accessBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AccessToken = accessBox.Text;
            Properties.Settings.Default.Save();
        }

        public void channelBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ChannelName = channelBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userInput1 = accessBox.Text;

            // Load existing data from the JSON file
            LoginData existingData = LoadCounterData();

            // Update the existing data with the new value
            existingData.BotToken = userInput1;

            // Specify the path to the JSON file
            string jsonFilePath = Path.Combine("Data", "bin", "Logon.json");

            // Serialize and save the updated data to the JSON file
            string jsonData = JsonConvert.SerializeObject(existingData, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonData);

            label1.Text += "\nAccess Token saved to Logon.json";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string userInput2 = channelBox2.Text;

            // Load existing data from the JSON file
            LoginData existingData = LoadCounterData();

            // Update the existing data with the new value
            existingData.ChannelName = userInput2;

            // Specify the path to the JSON file
            string jsonFilePath = Path.Combine("Data", "bin", "Logon.json");

            // Serialize and save the updated data to the JSON file
            string jsonData = JsonConvert.SerializeObject(existingData, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonData);

            label1.Text += "\nChannel saved to Logon.json";
        }

        // Load existing data from the JSON file
        private LoginData LoadCounterData()
        {
            string jsonFilePath = Path.Combine("Data", "bin", "Logon.json");
            LoginData existingData = new LoginData();

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                existingData = JsonConvert.DeserializeObject<LoginData>(json) ?? new LoginData();
            }

            return existingData;
        }

        public class LoginData
        {
            public string ChannelName { get; set; }
            public string BotToken { get; set; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                accessBox.UseSystemPasswordChar = false;
            }
            else
            {
                accessBox.UseSystemPasswordChar = true;
            }
        }

        public void channelOpen_Click(object sender, EventArgs e)
        {
            // Get the channel name from channelBox2
            string channelName = channelBox2.Text;

            // Construct the Twitch URL with the channel name
            string twitchUrl = $"https://www.twitch.tv/{channelName}";

            // Open the Twitch URL in the default web browser
            Process.Start(new ProcessStartInfo
            {
                FileName = twitchUrl,
                UseShellExecute = true
            });
        }

        public void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify the URL you want to open
            string url = "https://github.com/sprollucy/Tarkov-Twitch-Bot-Working";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify the URL you want to open
            string url = "https://www.paypal.com/donate/?business=FK2ZHM73QW3FA&no_recurring=0&item_name=Thank+you+for+helping+support+my+projects%21&currency_code=USD";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }

        private void cashappLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify the URL you want to open
            string url = "http://cash.app/$sprollucy";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }

        private void bitrestoreButton_Click(object sender, EventArgs e)
        {
            string backupDirectory = Path.Combine("Backup");
            string backupFileNamePattern = "user_bits_backup_*.json"; // Wildcard pattern for the backup files
            string jsonFilePath = Path.Combine("Data", "user_bits.json");

            Console.WriteLine($"Restoration triggered at {DateTime.Now}");

            try
            {
                // Get all matching backup files
                string[] backupFiles = Directory.GetFiles(backupDirectory, backupFileNamePattern);

                if (backupFiles.Length == 0)
                {
                    MessageBox.Show("No backup files found.");
                    return;
                }

                // Sort files by creation time descending and select the latest one
                string latestBackupFile = backupFiles.OrderByDescending(f => f).First();

                // Read the content from the latest backup file
                string backupContent = File.ReadAllText(latestBackupFile);

                // Write the backup content back to the JSON file
                File.WriteAllText(jsonFilePath, backupContent);

                MessageBox.Show($"Restoration completed from {latestBackupFile} to {jsonFilePath} Please restart for changes to take effect.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during restoration: {ex.Message}");
            }
        }

        private void enableUpdateCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isUpdateCheckEnabled = enableUpdateCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private async void checkUpdateButton_Click(object sender, EventArgs e)
        {
            UpdateCheck updateChecker = new UpdateCheck();
            await updateChecker.ButtonCheckForUpdatesAsync();
        }

        private void enableDebug_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isDebugOn = enableDebug.Checked;
            Properties.Settings.Default.Save();
            if (Application.OpenForms["ConnectMenu"] is ConnectMenu connectMenu)
            {
                connectMenu.RamDebug();
            }
        }

        private void enableEFTtrade_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isTraderMenuEnabled = enableEFTtrade.Checked;
            Properties.Settings.Default.Save();
        }

        private void discordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify the URL you want to open
            string url = "https://discord.gg/k4uH6WZTS4";

            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }
    }
}
