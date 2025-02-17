using Newtonsoft.Json;
using System.Diagnostics;
using UiBot.Properties;
namespace UiBot
{
    public partial class ControlMenu : Form
    {
        private Dictionary<string, TextBox> textBoxes = new Dictionary<string, TextBox>();
        private TarkovInRaidCheck tarkovMonitor;
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ModWhitelist.txt");
        private static readonly string excludefilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SendkeyExclude.txt");

        public ControlMenu()
        {
            InitializeComponent();
            InitializeTextBoxes();
            LoadSettings();
            LoadModWhitelist(); // Load data on startup

            this.TopLevel = false;
            Settings.Default.isCommandsPaused = false;
            enableAutoMessageCheck.Checked = Settings.Default.isAutoMessageEnabled;
            enableTradersCommand.Checked = Settings.Default.isTradersEnabled;
            enableChatBonus.Checked = Settings.Default.isChatBonusEnabled;
            modWhitelistCheck.Checked = Settings.Default.isModWhitelistEnabled;
            enableBonusMulti.Checked = Settings.Default.isBonusMultiplierEnabled;
            enableFollowBonus.Checked = Settings.Default.isFollowBonusEnabled;
            enableSubBonus.Checked = Settings.Default.isSubBonusEnabled;
            enableBotToggle.Checked = Settings.Default.isSweatbotEnabled;
            checkEnableBitMsg.Checked = Settings.Default.isBitMsgEnabled;
            bitcostButton.Checked = Settings.Default.isBitCostEnabled;
            sendkeyButton.Checked = Settings.Default.isSendKeyEnabled;
            enableSubBonusMulti.Checked = Settings.Default.isSubBonusMultiEnabled;
            bitGambleCheck.Checked = Settings.Default.isBitGambleEnabled;
            blerpBox.Checked = Settings.Default.isblerpEnabled;
            soundalertsBox.Checked = Settings.Default.isSoundAlertsEnabled;
            subsweatbotBox.Checked = Settings.Default.isSubOnlySweatbotCommand;
            subgambleBox.Checked = Settings.Default.isSubOnlyGambleCommand;
            subbotBox.Checked = Settings.Default.isSubOnlyBotCommand;
            enableRateDelayBox.Checked = Settings.Default.isRateDelayEnabled;
            pauseMessageBox.Checked = Settings.Default.isPausedMessage;
            enableCurrency.Checked = Settings.Default.isStoreCurrency;
            tangiaBox.Checked = Settings.Default.isTangiaEnabled;
            cheerComBox.Checked = Settings.Default.isDirectRunEnabled;

            InitializeToolTips();
            this.lstModWhitelist.SelectedIndexChanged += new System.EventHandler(this.lstModWhitelist_SelectedIndexChanged);

        }

        public void InitializeToolTips()
        {
            blerpTip.SetToolTip(this.blerpBox, "If a user sends a message with blerp, it will read how many bits they spent and add it to their wallet based on a set %");
            soundalertTip.SetToolTip(this.blerpBox, "If a user sends a message with SoundAlerts, it will read how many bits they spent and add it to their wallet based on a set %");
            traderTip.SetToolTip(this.enableTradersCommand, "Allows chat users to use !trader command to print out their restock timers to chat");
            sweatbottogTip.SetToolTip(this.enableBotToggle, "Allows chat users to use !sweatbot to pause and unpause the commands");
            chatbonusTip.SetToolTip(this.enableChatBonus, "if a user sends a message in chat, it will automatically give them free currency and add it to their wallet");
            bitMultiTip.SetToolTip(this.enableBonusMulti, "Percentage based multiplier for when a user cheers bits in chat");
            subMultiTip.SetToolTip(this.enableSubBonusMulti, "Percentage based multiplier for when a sub cheers bits in chat");
            modWhitelistTip.SetToolTip(this.modWhitelistCheck, "Add Moderators to the whitelist with the permisions you want them to have\nExample Sprollucy:refund,give,remove,add_remove_command");
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
            textBoxes["custombitnameBox"] = CustombitnameBox;
            textBoxes["rateDelayBox"] = RateDelayBox;
            textBoxes["blerpReturnBox"] = BlerpReturnBox;
            textBoxes["soundAlertsReturnBox"] = SoundAlertsReturnBox;
            textBoxes["tangiaTextBox"] = TangiaTextBox;
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
        public TextBox CustombitnameBox
        {
            get { return custombitnameBox; }
            set { custombitnameBox = value; }
        }
        public TextBox RateDelayBox
        {
            get { return rateDelayBox; }
            set { rateDelayBox = value; }
        }
        public TextBox BlerpReturnBox
        {
            get { return blerpReturnBox; }
            set { blerpReturnBox = value; }
        }
        public TextBox SoundAlertsReturnBox
        {
            get { return soundAlertsReturnBox; }
            set { soundAlertsReturnBox = value; }
        }
        public TextBox TangiaTextBox
        {
            get { return tangiaTextBox; }
            set { tangiaTextBox = value; }
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
            Settings.Default.isAutoMessageEnabled = enableAutoMessageCheck.Checked;
            Settings.Default.Save();
        }

        private void enableTradersCommand_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isTradersEnabled = enableTradersCommand.Checked;
            Settings.Default.Save();
        }

        private void enableChatBonus_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isChatBonusEnabled = enableChatBonus.Checked;
            Settings.Default.Save();
        }

        private void enableFollowBonus_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isFollowBonusEnabled = enableFollowBonus.Checked;
            Settings.Default.Save();
        }

        private void enableSubBonus_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSubBonusEnabled = enableSubBonus.Checked;
            Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();

        }

        private void modWhitelistCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isModWhitelistEnabled = modWhitelistCheck.Checked;
            Settings.Default.Save();
        }

        private void openModWhitelist_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        private void enableBonusMulti_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isBonusMultiplierEnabled = enableBonusMulti.Checked;
            Settings.Default.Save();
        }

        private void enableBotToggle_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSweatbotEnabled = enableBotToggle.Checked;
            Settings.Default.Save();
        }


        private void checkEnableBitMsg_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isBitMsgEnabled = checkEnableBitMsg.Checked;
            Settings.Default.Save();
        }
        private void bitcostButton_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isBitCostEnabled = bitcostButton.Checked;
            Settings.Default.Save();
        }

        private void bonusMultiplierBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sendkeyButton_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSendKeyEnabled = sendkeyButton.Checked;
            Settings.Default.Save();
        }

        private void enableSubBonusMulti_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSubBonusMultiEnabled = enableSubBonusMulti.Checked;
            Settings.Default.Save();
        }

        private void bitGambleCheck_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isBitGambleEnabled = bitGambleCheck.Checked;
            Settings.Default.Save();
        }

        private void blerpcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isblerpEnabled = blerpBox.Checked;
            Settings.Default.Save();
        }

        private void soundalertsBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSoundAlertsEnabled = soundalertsBox.Checked;
            Settings.Default.Save();
        }

        private void subsweatbotBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSubOnlySweatbotCommand = subsweatbotBox.Checked;
            Settings.Default.Save();
        }

        private void subgambleBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSubOnlyGambleCommand = subgambleBox.Checked;
            Settings.Default.Save();
        }

        private void subbotBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isSubOnlyBotCommand = subbotBox.Checked;
            Settings.Default.Save();
        }
        private void enableRateDelayBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isRateDelayEnabled = enableRateDelayBox.Checked;
            Settings.Default.Save();
        }

        private void enableInRaid_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isInRaid = enableInRaid.Checked;
            StartInRaidFileCheck();
        }

        private void tangiaBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isTangiaEnabled = tangiaBox.Checked;
            Settings.Default.Save();
        }

        private void openSendkeyButton_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = excludefilePath,
                UseShellExecute = true
            });
        }

        private void pauseMessageBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isPausedMessage = pauseMessageBox.Checked;
            Settings.Default.Save();
        }
        private void cheerComBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isDirectRunEnabled = cheerComBox.Checked;
            Settings.Default.Save();
        }

        private void traderConfigButton_Click(object sender, EventArgs e)
        {
            TraderMenuConfig traderMenuConfig = new TraderMenuConfig();

            traderMenuConfig.Show();
        }

        private void enableCurrency_CheckedChanged(object sender, EventArgs e)
        {
            // Save the current checked state
            Settings.Default.isStoreCurrency = enableCurrency.Checked;
            Settings.Default.Save();

            // Disable and uncheck other checkboxes when enableCurrency is unchecked
            bool isEnabled = enableCurrency.Checked;

            enableFollowBonus.Enabled = isEnabled;

            enableChatBonus.Enabled = isEnabled;

            enableBonusMulti.Enabled = isEnabled;

            blerpBox.Enabled = isEnabled;

            soundalertsBox.Enabled = isEnabled;

            tangiaBox.Enabled = isEnabled;

            enableSubBonus.Enabled = isEnabled;

            enableSubBonusMulti.Enabled = isEnabled;

            subsweatbotBox.Enabled = isEnabled;

            subgambleBox.Enabled = isEnabled;

            bitGambleCheck.Enabled = isEnabled;

            sendkeyButton.Enabled = isEnabled;

            enableBotToggle.Enabled = isEnabled;

            checkEnableBitMsg.Enabled = isEnabled;
        }


        private void StartInRaidFileCheck()
        {
            if (Settings.Default.isInRaid)
            {
                if (tarkovMonitor == null) // Only start if it's not already running
                {
                    string logFolder = TarkovInRaidCheck.LogsFolder;
                    string searchPattern = "*application*.log"; // Match any log file with 'application' in the name

                    if (Directory.Exists(logFolder))
                    {
                        tarkovMonitor = new TarkovInRaidCheck(logFolder, searchPattern);

                        tarkovMonitor.Created += (sender, e) => Console.WriteLine($"File Created: {e.FilePath}");
                        tarkovMonitor.Changed += (sender, e) => Console.WriteLine($"File Changed: {e.FilePath}");
                        if (Settings.Default.isDebugOn)
                        {
                            Console.WriteLine("Starting log monitoring...");

                        }

                        tarkovMonitor.Start();
                    }
                    else
                    {
                        Console.WriteLine("Tarkov log folder not found.");
                    }
                }
            }
            else
            {
                if (tarkovMonitor != null)
                {
                    if (Settings.Default.isDebugOn)
                    {
                        Console.WriteLine("Stopping log monitoring...");
                    }
                    tarkovMonitor.Stop();
                    tarkovMonitor = null; // Clear reference to allow re-initialization
                    ConnectMenu.Instance.pauseCommands.Invoke(new Action(() =>
                    {
                        ConnectMenu.Instance.pauseCommands.Checked = false;  // Ensure it reflects the state in the UI
                    }));
                }
            }
        }

        private void LoadModWhitelist()
        {
            lstModWhitelist.Items.Clear();
            LogHandler.LoadWhitelist(); // Load into modWhitelist dictionary

            foreach (var mod in LogHandler.modWhitelist)
            {
                lstModWhitelist.Items.Add($"{mod.Key}: {string.Join(", ", mod.Value)}");
            }
        }

        // Add or update a user in the mod whitelist
        private async void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();

            // Collect permissions from checkboxes
            HashSet<string> permissions = new HashSet<string>();

            if (chkRefund.Checked) permissions.Add("refund");
            if (chkGive.Checked) permissions.Add("give");
            if (chkRemove.Checked) permissions.Add("remove");
            if (chkAdd.Checked) permissions.Add("add_remove_command");
            if (chkBan.Checked) permissions.Add("ban");

            if (string.IsNullOrEmpty(username) || permissions.Count == 0)
            {
                MessageBox.Show("Username and at least one permission must be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If the user doesn't exist in the whitelist, add them
            if (!LogHandler.modWhitelist.ContainsKey(username))
            {
                LogHandler.modWhitelist[username] = new HashSet<string>();
            }

            // Update the user's permissions
            LogHandler.modWhitelist[username] = permissions;

            await SaveModWhitelist();
            LoadModWhitelist(); // Refresh the list
            MessageBox.Show("User added/updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Remove a user from the mod whitelist
        private async void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstModWhitelist.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedEntry = lstModWhitelist.SelectedItem.ToString();
            string username = selectedEntry.Split(':')[0].Trim();

            if (LogHandler.modWhitelist.ContainsKey(username))
            {
                LogHandler.modWhitelist.Remove(username);
                await SaveModWhitelist();
                LoadModWhitelist(); // Refresh the list
                MessageBox.Show("User removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Save the whitelist back to the file
        private async Task SaveModWhitelist()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ModWhitelist.txt");

            List<string> lines = LogHandler.modWhitelist
                .Select(kvp => $"{kvp.Key}:{string.Join(",", kvp.Value)}")
                .ToList();

            await File.WriteAllLinesAsync(filePath, lines);
        }

        // When selecting an item in the ListBox, fill the text fields and checkboxes for editing
        private void lstModWhitelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstModWhitelist.SelectedItem != null)
            {
                string selectedEntry = lstModWhitelist.SelectedItem.ToString();
                string[] parts = selectedEntry.Split(':');

                if (parts.Length == 2)
                {
                    txtUsername.Text = parts[0].Trim();
                    string[] perms = parts[1].Trim().Split(',');

                    // Reset checkboxes
                    chkRefund.Checked = false;
                    chkGive.Checked = false;
                    chkAdd.Checked = false;
                    chkRemove.Checked = false;

                    // Set checkboxes based on user permissions
                    foreach (var perm in perms)
                    {
                        if (perm.Trim().Equals("refund", StringComparison.OrdinalIgnoreCase)) chkRefund.Checked = true;
                        if (perm.Trim().Equals("give", StringComparison.OrdinalIgnoreCase)) chkGive.Checked = true;
                        if (perm.Trim().Equals("add_remove_command", StringComparison.OrdinalIgnoreCase)) chkAdd.Checked = true;
                        if (perm.Trim().Equals("remove", StringComparison.OrdinalIgnoreCase)) chkRemove.Checked = true;
                    }
                }
            }
        }
    }
}