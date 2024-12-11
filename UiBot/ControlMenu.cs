using Newtonsoft.Json;
using System.Diagnostics;
namespace UiBot
{
    public partial class ControlMenu : Form
    {
        private Dictionary<string, TextBox> textBoxes = new Dictionary<string, TextBox>();


        public ControlMenu()
        {
            InitializeComponent();
            InitializeTextBoxes();
            LoadSettings();

            this.TopLevel = false;
            enableAutoMessageCheck.Checked = Properties.Settings.Default.isAutoMessageEnabled;
            enableTradersCommand.Checked = Properties.Settings.Default.isTradersEnabled;
            enableChatBonus.Checked = Properties.Settings.Default.isChatBonusEnabled;
            enableModBits.Checked = Properties.Settings.Default.isModBitsEnabled;
            modRefund.Checked = Properties.Settings.Default.isModRefundEnabled;
            modWhitelistCheck.Checked = Properties.Settings.Default.isModWhitelistEnabled;
            enableBonusMulti.Checked = Properties.Settings.Default.isBonusMultiplierEnabled;
            enableFollowBonus.Checked = Properties.Settings.Default.isFollowBonusEnabled;
            enableSubBonus.Checked = Properties.Settings.Default.isSubBonusEnabled;
            enableBotToggle.Checked = Properties.Settings.Default.isSweatbotEnabled;
            checkEnableBitMsg.Checked = Properties.Settings.Default.isBitMsgEnabled;

            if (Properties.Settings.Default.isTraderMenuEnabled)
            {
                enableTradersCommand.Visible = true;
            }
            else
            {
                enableTradersCommand.Visible = false;
            }

        }

        public void InitializeTextBoxes()
        {
            // Add your TextBox controls to the dictionary with unique keys
            textBoxes["autoMessageBox"] = AutoMessageBox;
            textBoxes["autoSendMessageCD"] = AutoSendMessageCD;
            textBoxes["bonusTextBox"] = BonusTextBox;
            textBoxes["followTextBox"] = FollowTextBox;
            textBoxes["subTextBox"] = SubTextBox;
            textBoxes["bonusMultiplierBox"] = BonusMultiplierBox;
            textBoxes["bottoggleCostBox"] = BotToggleCostBox;
        }

        public TextBox BotToggleCostBox
        {
            get { return bottoggleCostBox; }
            set { bottoggleCostBox = value; }
        }

        public TextBox AutoMessageBox
        {
            get { return autoMessageBox; }
            set { autoMessageBox = value; }
        }
        public TextBox AutoSendMessageCD
        {
            get { return autoSendMessageCD; }
            set { autoSendMessageCD = value; }
        }

        public TextBox BonusTextBox
        {
            get { return bonusTextBox; }
            set { bonusTextBox = value; }
        }

        public TextBox FollowTextBox
        {
            get { return followTextBox; }
            set { followTextBox = value; }
        }

        public TextBox SubTextBox
        {
            get { return subTextBox; }
            set { subTextBox = value; }
        }

        public TextBox BonusMultiplierBox
        {
            get { return bonusMultiplierBox; }
            set { bonusMultiplierBox = value; }
        }

        //TODO make save reload on save so app doesnt have to restart
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Create a dictionary to store the text from all TextBox controls
            Dictionary<string, string> textData = new Dictionary<string, string>();

            // Iterate through the TextBoxes dictionary and save the text from each TextBox
            foreach (var textBoxEntry in textBoxes)
            {
                string textBoxKey = textBoxEntry.Key;
                TextBox textBox = textBoxEntry.Value;
                string textBoxText = textBox.Text;

                // Add the TextBox key and text to the dictionary
                textData[textBoxKey] = textBoxText;
            }

            // Serialize and save the dictionary to a JSON file in the "Data" folder
            string json = JsonConvert.SerializeObject(textData);
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "bin");
            Directory.CreateDirectory(dataDirectory); // Ensure the directory exists
            string filePath = Path.Combine(dataDirectory, "CommandConfigData.json");
            File.WriteAllText(filePath, json);
        }

        // Load settings from JSON on startup
        public void LoadSettings()
        {
            // Construct the file path to the JSON file in the "Data" folder
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "bin");
            string filePath = Path.Combine(dataDirectory, "CommandConfigData.json");

            // Check if the JSON file exists
            if (File.Exists(filePath))
            {
                // Deserialize and load the data from the JSON file into a dictionary
                string json = File.ReadAllText(filePath);
                Dictionary<string, string> textData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                // Iterate through the dictionary and set the text for each TextBox
                foreach (var textBoxEntry in textData)
                {
                    string textBoxKey = textBoxEntry.Key;
                    string textBoxText = textBoxEntry.Value;

                    // Check if the key exists in the TextBoxes dictionary
                    if (textBoxes.ContainsKey(textBoxKey))
                    {
                        TextBox textBox = textBoxes[textBoxKey];
                        textBox.Text = textBoxText;
                    }
                }
            }
        }

        private void enableAutoMessageCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isAutoMessageEnabled = enableAutoMessageCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableTradersCommand_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isTradersEnabled = enableTradersCommand.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableChatBonus_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isChatBonusEnabled = enableChatBonus.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableFollowBonus_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isFollowBonusEnabled = enableFollowBonus.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableSubBonus_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSubBonusEnabled = enableSubBonus.Checked;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();

        }

        private void enableModBits_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isModBitsEnabled = enableModBits.Checked;
            Properties.Settings.Default.Save();
        }

        private void modRefund_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isModRefundEnabled = modRefund.Checked;
            Properties.Settings.Default.Save();
        }

        private void modWhitelistCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isModWhitelistEnabled = modWhitelistCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void openModWhitelist_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ModWhitelist.txt");
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        private void enableBonusMulti_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isBonusMultiplierEnabled = enableBonusMulti.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableBotToggle_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSweatbotEnabled = enableBotToggle.Checked;
            Properties.Settings.Default.Save();
        }


        private void checkEnableBitMsg_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isBitMsgEnabled = checkEnableBitMsg.Checked;
            Properties.Settings.Default.Save();
        }

        private void saveMessageButton_Click(object sender, EventArgs e)
        {
            // Create a dictionary to store the text from all TextBox controls
            Dictionary<string, string> textData = new Dictionary<string, string>();

            // Iterate through the TextBoxes dictionary and save the text from each TextBox
            foreach (var textBoxEntry in textBoxes)
            {
                string textBoxKey = textBoxEntry.Key;
                TextBox textBox = textBoxEntry.Value;
                string textBoxText = textBox.Text;

                // Add the TextBox key and text to the dictionary
                textData[textBoxKey] = textBoxText;
            }

            // Serialize and save the dictionary to a JSON file in the "Data" folder
            string json = JsonConvert.SerializeObject(textData);
            string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "bin");
            Directory.CreateDirectory(dataDirectory); // Ensure the directory exists
            string filePath = Path.Combine(dataDirectory, "CommandConfigData.json");
            File.WriteAllText(filePath, json);
        }

        private void bonusMultiplierBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}