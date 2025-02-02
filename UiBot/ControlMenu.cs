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
            bitcostButton.Checked = Properties.Settings.Default.isBitCostEnabled;
            sendkeyButton.Checked = Properties.Settings.Default.isSendKeyEnabled;
            enableSubBonusMulti.Checked = Properties.Settings.Default.isSubBonusMultiEnabled;
            modMake.Checked = Properties.Settings.Default.isModAddEnabled;
            modRemove.Checked = Properties.Settings.Default.isModRemoveEnabled;
            bitGambleCheck.Checked = Properties.Settings.Default.isBitGambleEnabled;
            blerpBox.Checked = Properties.Settings.Default.isblerpEnabled;
            subsweatbotBox.Checked = Properties.Settings.Default.isSubOnlySweatbotCommand;
            subgambleBox.Checked = Properties.Settings.Default.isSubOnlyGambleCommand;
            subbotBox.Checked = Properties.Settings.Default.isSubOnlyBotCommand;


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
            textBoxes["subbonusMultiplierBox"] = SubBonusMultiplierBox;
            textBoxes["bottoggleCostBox"] = BotToggleCostBox;
            textBoxes["sendkeyCostBox"] = SendKeyCostBox;
            textBoxes["sendkeyTimeBox"] = SendKeyTimeBox;
            textBoxes["bitChanceBox"] = BitChanceBox;
            textBoxes["bitGambleCDBox"] = BitGambleCDBox;
        }

        public TextBox BotToggleCostBox
        {
            get { return bottoggleCostBox; }
            set { bottoggleCostBox = value; }
        }
        public TextBox BitChanceBox
        {
            get { return bitChanceBox; }
            set { bitChanceBox = value; }
        }
        public TextBox BitGambleCDBox
        {
            get { return bitGambleCDBox; }
            set { bitGambleCDBox = value; }
        }
        public TextBox SendKeyCostBox
        {
            get { return sendkeyCostBox; }
            set { sendkeyCostBox = value; }
        }

        public TextBox SendKeyTimeBox
        {
            get { return sendkeyTimeBox; }
            set { sendkeyTimeBox = value; }
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

        public TextBox SubBonusMultiplierBox
        {
            get { return subbonusMultiplierBox; }
            set { subbonusMultiplierBox = value; }
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
        private void bitcostButton_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isBitCostEnabled = bitcostButton.Checked;
            Properties.Settings.Default.Save();
        }

        private void bonusMultiplierBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sendkeyButton_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSendKeyEnabled = sendkeyButton.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableSubBonusMulti_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSubBonusMultiEnabled = enableSubBonusMulti.Checked;
            Properties.Settings.Default.Save();
        }

        private void modMake_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isModAddEnabled = modMake.Checked;
            Properties.Settings.Default.Save();
        }

        private void modRemove_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isModRemoveEnabled = modRemove.Checked;
            Properties.Settings.Default.Save();
        }

        private void bitGambleCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isBitGambleEnabled = bitGambleCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void blerpcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isblerpEnabled = bitGambleCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void subsweatbotBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSubOnlySweatbotCommand = subsweatbotBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void subgambleBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSubOnlyGambleCommand = subgambleBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void subbotBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isSubOnlyBotCommand = subbotBox.Checked;
            Properties.Settings.Default.Save();
        }
    }
}