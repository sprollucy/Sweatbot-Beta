﻿using Newtonsoft.Json;

/* TODO **
 * Add extra customization into commands like how long the action goes on for
 * Custom scripting for custom commands?
 * Add multiple messages that can be sent at different times
 * rename all cooldown boxes to cost boxes 
*/
namespace UiBot
{
    public partial class ControlMenu : Form
    {
        private Dictionary<string, TextBox> textBoxes = new Dictionary<string, TextBox>();


        public ControlMenu()
        {
            InitializeComponent();

            // Initialize TextBoxes dictionary
            InitializeTextBoxes();

            // Load the settings from JSON on application startup
            LoadSettings();

            this.TopLevel = false;
            chkEnableGoose.Checked = Properties.Settings.Default.IsGooseEnabled;
            enableWiggle.Checked = Properties.Settings.Default.IsWiggleEnabled;
            enableRandomKey.Checked = Properties.Settings.Default.IsKeyEnabled;
            enableKitDrop.Checked = Properties.Settings.Default.IsDropEnabled;
            randomTurn.Checked = Properties.Settings.Default.IsTurnEnabled;
            oneClickCheck.Checked = Properties.Settings.Default.IsPopEnabled;
            enableAutoMessageCheck.Checked = Properties.Settings.Default.isAutoMessageEnabled;
            enableGrenade.Checked = Properties.Settings.Default.isGrenadeEnabled;
            enableBagDrop.Checked = Properties.Settings.Default.isDropBagEnabled;
            enableTradersCommand.Checked = Properties.Settings.Default.isTradersEnabled;
            enableGrenadeToss.Checked = Properties.Settings.Default.isGrenadeTossEnabled;
            crouchBox.Checked = Properties.Settings.Default.isCrouchEnabled;
            enableMagDump.Checked = Properties.Settings.Default.isMagDumpEnabled;
            enableHoldAim.Checked = Properties.Settings.Default.isHoldAimEnabled;
            enableChatBonus.Checked = Properties.Settings.Default.isChatBonusEnabled;
            enable360MagDump.Checked = Properties.Settings.Default.isMagDump360Enabled;
            enableProne.Checked = Properties.Settings.Default.isProneEnabled;
            enableVoiceLine.Checked = Properties.Settings.Default.isVoiceLineEnabled;
            enableReload.Checked = Properties.Settings.Default.isReloadEnabled;


        }

        private void ControlMenu_load(object sender, EventArgs e)
        {

        }

        public void InitializeTextBoxes()
        {
            // Add your TextBox controls to the dictionary with unique keys
            textBoxes["wiggleCooldown"] = WiggleCooldownTextBox;
            textBoxes["dropCooldown"] = DropCooldownTextBox;
            textBoxes["turnCooldown"] = TurnCooldownTextBox;
            textBoxes["gooseCooldown"] = GooseCooldownTextBox;
            textBoxes["randomKeyCooldown"] = RandomKeyCooldownTextBox;
            textBoxes["oneClickCooldown"] = OneClickCooldownTextBox;
            textBoxes["randomKeyInputs"] = RandomKeyInputs;
            textBoxes["autoMessageBox"] = AutoMessageBox;
            textBoxes["autoSendMessageCD"] = AutoSendMessageCD;
            textBoxes["grenadeCooldown"] = GrenadeCooldownTextBox;
            textBoxes["dropbagCooldown"] = dropbagCooldownTextBox;
            textBoxes["dropKey"] = DropKeyTextBox;
            textBoxes["granadeToss"] = GrenadeCostBox;
            textBoxes["grenadeTossKey"] = GrenadeKeyBox;
            textBoxes["crouchKey"] = CrouchKeyBox;
            textBoxes["magDumpCost"] = MagDumpBox;
            textBoxes["holdAimCost"] = HoldAimCost;
            textBoxes["bonusTextBox"] = BonusTextBox;
            textBoxes["crouchBoxCost"] = CrouchBoxCost;
            textBoxes["mag360Cost"] = Mag360Cost;
            textBoxes["proneCostBox"] = ProneCost;
            textBoxes["proneKey"] = ProneKeyBox;
            textBoxes["voicelineCostBox"] = VoicelineCostBox;
            textBoxes["reloadCostBox"] = ReloadCostBox;
            textBoxes["reloadKey"] = ReloadKeyBox;
        }
        public TextBox WiggleCooldownTextBox
        {
            get { return wiggleCooldownTextBox; }
            set { wiggleCooldownTextBox = value; }
        }
        public TextBox DropCooldownTextBox
        {
            get { return dropCooldownTextBox; }
            set { dropCooldownTextBox = value; }
        }
        public TextBox TurnCooldownTextBox
        {
            get { return turnCooldownTextBox; }
            set { turnCooldownTextBox = value; }
        }
        public TextBox GooseCooldownTextBox
        {
            get { return gooseCooldownTextBox; }
            set { gooseCooldownTextBox = value; }
        }
        public TextBox RandomKeyCooldownTextBox
        {
            get { return randomKeyCooldownTextBox; }
            set { randomKeyCooldownTextBox = value; }
        }
        public TextBox OneClickCooldownTextBox
        {
            get { return oneClickCooldownTextBox; }
            set { oneClickCooldownTextBox = value; }
        }
        public TextBox RandomKeyInputs
        {
            get { return randomKeyInputs; }
            set { randomKeyInputs = value; }
        }
        public TextBox GrenadeCooldownTextBox
        {
            get { return grenadeCooldownTextBox; }
            set { grenadeCooldownTextBox = value; }
        }
        public TextBox DropBagCooldownTextBox
        {
            get { return dropbagCooldownTextBox; }
            set { dropbagCooldownTextBox = value; }
        }
        public TextBox DropKeyTextBox
        {
            get { return dropKeyTextBox; }
            set { dropKeyTextBox = value; }
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

        public TextBox GrenadeCostBox
        {
            get { return grenadeCostBox; }
            set { grenadeCostBox = value; }
        }
        public TextBox GrenadeKeyBox
        {
            get { return grenadeKeyBox; }
            set { grenadeKeyBox = value; }
        }
        public TextBox CrouchKeyBox
        {
            get { return crouchBoxKey; }
            set { crouchBoxKey = value; }
        }
        public TextBox CrouchBoxCost
        {
            get { return crouchBoxCost; }
            set { crouchBoxCost = value; }
        }
        public TextBox MagDumpBox
        {
            get { return magDumpCost; }
            set { magDumpCost = value; }
        }
        public TextBox HoldAimCost
        {
            get { return holdAimCost; }
            set { holdAimCost = value; }
        }
        public TextBox BonusTextBox
        {
            get { return bonusTextBox; }
            set { bonusTextBox = value; }
        }
        public TextBox Mag360Cost
        {
            get { return mag360Cost; }
            set { mag360Cost = value; }
        }
        public TextBox ProneCost
        {
            get { return proneCostBox; }
            set { proneCostBox = value; }
        }
        public TextBox ProneKeyBox
        {
            get { return proneKeyBox; }
            set { proneKeyBox = value; }
        }
        public TextBox VoicelineCostBox
        {
            get { return voicelineCostBox; }
            set { voicelineCostBox = value; }
        }
        public TextBox ReloadCostBox
        {
            get { return reloadCostBox; }
            set { reloadCostBox = value; }
        }
        public TextBox ReloadKeyBox
        {
            get { return reloadKeyBox; }
            set { reloadKeyBox = value; }
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

            // Serialize and save the dictionary to a JSON file
            string json = JsonConvert.SerializeObject(textData);
            File.WriteAllText("CommandConfigData.json", json);

        }


        // Load settings from JSON on startup
        public void LoadSettings()
        {
            // Check if the JSON file exists
            if (File.Exists("CommandConfigData.json"))
            {
                // Deserialize and load the data from the JSON file into a dictionary
                string json = File.ReadAllText("CommandConfigData.json");
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

        public void chkEnableGoose_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsGooseEnabled = chkEnableGoose.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableWiggle_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsWiggleEnabled = enableWiggle.Checked;
            Properties.Settings.Default.Save();
        }


        private void enableRandomKey_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsKeyEnabled = enableRandomKey.Checked;
            Properties.Settings.Default.Save();

        }

        private void enableKitDrop_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsDropEnabled = enableKitDrop.Checked;
            Properties.Settings.Default.Save();
        }

        private void randomTurn_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsTurnEnabled = randomTurn.Checked;
            Properties.Settings.Default.Save();
        }

        private void oneClickCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsPopEnabled = oneClickCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableAutoMessageCheck_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isAutoMessageEnabled = enableAutoMessageCheck.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableGrenade_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isGrenadeEnabled = enableGrenade.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableBagDrop_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isDropBagEnabled = enableBagDrop.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableTradersCommand_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isTradersEnabled = enableTradersCommand.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableGrenadeToss_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isGrenadeTossEnabled = enableGrenadeToss.Checked;
            Properties.Settings.Default.Save();
        }

        private void crouchBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isCrouchEnabled = crouchBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableMagDump_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isMagDumpEnabled = enableMagDump.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableHoldAim_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isHoldAimEnabled = enableHoldAim.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableChatBonus_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isChatBonusEnabled = enableChatBonus.Checked;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();

        }

        private void enable360MagDump_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isMagDump360Enabled = enable360MagDump.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableProne_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isProneEnabled = enableProne.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableVoiceLine_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isVoiceLineEnabled = enableVoiceLine.Checked;
            Properties.Settings.Default.Save();
        }

        private void enableReload_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isReloadEnabled = enableReload.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
