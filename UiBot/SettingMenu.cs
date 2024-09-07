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
            string jsonFilePath = Path.Combine("Data", "Logon.json");

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
            string jsonFilePath = Path.Combine("Data", "Logon.json");

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


        private void groupBox1_Enter(object sender, EventArgs e)
        {

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

        private void bitrestoreButton_Click(object sender, EventArgs e)
        {
            string backupFilePath = Path.Combine("Data", "user_bits_backup.txt");
            string jsonFilePath = Path.Combine("Data", "user_bits.json");

            Console.WriteLine($"Restoration triggered at {DateTime.Now}");

            try
            {
                // Read the content from the backup file
                string backupContent = File.ReadAllText(backupFilePath);

                // Write the backup content back to the JSON file
                File.WriteAllText(jsonFilePath, backupContent);

                MessageBox.Show($"Restoration completed for {jsonFilePath} at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during restoration: {ex.Message}");
            }
        }


        private void restoreCommandButton_Click(object sender, EventArgs e)
        {
            // Define the list of backup file paths
            string[] backupFilePaths = { Path.Combine("Data", "CommandConfigData_backup.txt"), Path.Combine("Data", "DropPositionData_backup.txt") };

            // Define the list of corresponding JSON file paths
            string[] jsonFilePaths = { Path.Combine("Data", "CommandConfigData.json"), Path.Combine("Data", "DropPositionData.json") };

            Console.WriteLine($"Restoration triggered at {DateTime.Now}");

            try
            {
                for (int i = 0; i < backupFilePaths.Length; i++)
                {
                    // Read the content from the backup file
                    string backupContent = File.ReadAllText(backupFilePaths[i]);

                    // Write the backup content back to the JSON file
                    File.WriteAllText(jsonFilePaths[i], backupContent);

                    MessageBox.Show($"Restoration completed for {jsonFilePaths[i]} at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during restoration: {ex.Message}");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string backupFilePath = Path.Combine("Data", "Default_Commands.txt");
            string jsonFilePath = Path.Combine("Data", "CommandConfigData.json");

            Console.WriteLine($"Restoration triggered at {DateTime.Now}");

            try
            {
                // Read the content from the backup file
                string backupContent = File.ReadAllText(backupFilePath);

                // Write the backup content back to the JSON file
                File.WriteAllText(jsonFilePath, backupContent);

                MessageBox.Show($"Restoration completed for {jsonFilePath} at {DateTime.Now}");
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
        }
    }
}
