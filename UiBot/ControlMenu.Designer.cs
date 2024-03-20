namespace UiBot
{
    partial class ControlMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlMenu));
            wiggleCooldownTextBox = new TextBox();
            dropCooldownTextBox = new TextBox();
            gooseCooldownTextBox = new TextBox();
            randomKeyCooldownTextBox = new TextBox();
            turnCooldownTextBox = new TextBox();
            oneClickCooldownTextBox = new TextBox();
            saveButton = new Button();
            oneClickCheck = new CheckBox();
            randomTurn = new CheckBox();
            enableRandomKey = new CheckBox();
            chkEnableGoose = new CheckBox();
            enableKitDrop = new CheckBox();
            enableWiggle = new CheckBox();
            panel1 = new Panel();
            label4 = new Label();
            bonusTextBox = new TextBox();
            enableChatBonus = new CheckBox();
            holdAimCost = new TextBox();
            enableHoldAim = new CheckBox();
            magDumpCost = new TextBox();
            enableMagDump = new CheckBox();
            crouchBoxKey = new TextBox();
            crouchBoxCost = new TextBox();
            crouchBox = new CheckBox();
            grenadeKeyBox = new TextBox();
            grenadeCostBox = new TextBox();
            enableGrenadeToss = new CheckBox();
            enableTradersCommand = new CheckBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            dropKeyTextBox = new TextBox();
            textBox1 = new TextBox();
            label2 = new Label();
            enableBagDrop = new CheckBox();
            dropbagCooldownTextBox = new TextBox();
            enableGrenade = new CheckBox();
            grenadeCooldownTextBox = new TextBox();
            randomKeyInputs = new TextBox();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox10 = new PictureBox();
            autoMessageBox = new TextBox();
            autoSendMessageCD = new TextBox();
            autoMessageLabel = new Label();
            enableAutoMessageCheck = new CheckBox();
            pictureBox3 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // wiggleCooldownTextBox
            // 
            wiggleCooldownTextBox.Location = new Point(155, 70);
            wiggleCooldownTextBox.Name = "wiggleCooldownTextBox";
            wiggleCooldownTextBox.Size = new Size(100, 23);
            wiggleCooldownTextBox.TabIndex = 0;
            wiggleCooldownTextBox.Text = "300";
            // 
            // dropCooldownTextBox
            // 
            dropCooldownTextBox.Location = new Point(155, 101);
            dropCooldownTextBox.Name = "dropCooldownTextBox";
            dropCooldownTextBox.Size = new Size(100, 23);
            dropCooldownTextBox.TabIndex = 1;
            dropCooldownTextBox.Text = "300";
            // 
            // gooseCooldownTextBox
            // 
            gooseCooldownTextBox.Location = new Point(155, 159);
            gooseCooldownTextBox.Name = "gooseCooldownTextBox";
            gooseCooldownTextBox.Size = new Size(100, 23);
            gooseCooldownTextBox.TabIndex = 2;
            gooseCooldownTextBox.Text = "300";
            // 
            // randomKeyCooldownTextBox
            // 
            randomKeyCooldownTextBox.Location = new Point(155, 188);
            randomKeyCooldownTextBox.Name = "randomKeyCooldownTextBox";
            randomKeyCooldownTextBox.Size = new Size(100, 23);
            randomKeyCooldownTextBox.TabIndex = 3;
            randomKeyCooldownTextBox.Text = "300";
            // 
            // turnCooldownTextBox
            // 
            turnCooldownTextBox.Location = new Point(155, 247);
            turnCooldownTextBox.Name = "turnCooldownTextBox";
            turnCooldownTextBox.Size = new Size(100, 23);
            turnCooldownTextBox.TabIndex = 4;
            turnCooldownTextBox.Text = "300";
            // 
            // oneClickCooldownTextBox
            // 
            oneClickCooldownTextBox.Location = new Point(155, 276);
            oneClickCooldownTextBox.Name = "oneClickCooldownTextBox";
            oneClickCooldownTextBox.Size = new Size(100, 23);
            oneClickCooldownTextBox.TabIndex = 5;
            oneClickCooldownTextBox.Text = "300";
            oneClickCooldownTextBox.TextChanged += oneClickCooldownTextBox_TextChanged;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(820, 415);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // oneClickCheck
            // 
            oneClickCheck.AutoSize = true;
            oneClickCheck.BackColor = Color.FromArgb(181, 176, 163);
            oneClickCheck.Location = new Point(15, 276);
            oneClickCheck.Name = "oneClickCheck";
            oneClickCheck.Size = new Size(115, 19);
            oneClickCheck.TabIndex = 14;
            oneClickCheck.Text = "Enable One Click";
            oneClickCheck.UseVisualStyleBackColor = false;
            oneClickCheck.CheckedChanged += oneClickCheck_CheckedChanged;
            // 
            // randomTurn
            // 
            randomTurn.AutoSize = true;
            randomTurn.BackColor = Color.FromArgb(181, 176, 163);
            randomTurn.Location = new Point(15, 247);
            randomTurn.Name = "randomTurn";
            randomTurn.Size = new Size(85, 19);
            randomTurn.TabIndex = 15;
            randomTurn.Text = "EnableTurn";
            randomTurn.UseVisualStyleBackColor = false;
            randomTurn.CheckedChanged += randomTurn_CheckedChanged;
            // 
            // enableRandomKey
            // 
            enableRandomKey.AutoSize = true;
            enableRandomKey.BackColor = Color.FromArgb(181, 176, 163);
            enableRandomKey.Location = new Point(15, 188);
            enableRandomKey.Name = "enableRandomKey";
            enableRandomKey.Size = new Size(136, 19);
            enableRandomKey.TabIndex = 16;
            enableRandomKey.Text = "Enable Random Keys";
            enableRandomKey.UseVisualStyleBackColor = false;
            enableRandomKey.CheckedChanged += enableRandomKey_CheckedChanged;
            // 
            // chkEnableGoose
            // 
            chkEnableGoose.AutoSize = true;
            chkEnableGoose.BackColor = Color.FromArgb(181, 176, 163);
            chkEnableGoose.Location = new Point(15, 159);
            chkEnableGoose.Name = "chkEnableGoose";
            chkEnableGoose.Size = new Size(97, 19);
            chkEnableGoose.TabIndex = 17;
            chkEnableGoose.Text = "Enable Goose";
            chkEnableGoose.UseVisualStyleBackColor = false;
            chkEnableGoose.CheckedChanged += chkEnableGoose_CheckedChanged;
            // 
            // enableKitDrop
            // 
            enableKitDrop.AutoSize = true;
            enableKitDrop.BackColor = Color.FromArgb(181, 176, 163);
            enableKitDrop.Location = new Point(15, 101);
            enableKitDrop.Name = "enableKitDrop";
            enableKitDrop.Size = new Size(107, 19);
            enableKitDrop.TabIndex = 18;
            enableKitDrop.Text = "Enable Kit Drop";
            enableKitDrop.UseVisualStyleBackColor = false;
            enableKitDrop.CheckedChanged += enableKitDrop_CheckedChanged;
            // 
            // enableWiggle
            // 
            enableWiggle.AutoSize = true;
            enableWiggle.BackColor = Color.FromArgb(181, 176, 163);
            enableWiggle.Location = new Point(15, 72);
            enableWiggle.Name = "enableWiggle";
            enableWiggle.Size = new Size(101, 19);
            enableWiggle.TabIndex = 19;
            enableWiggle.Text = "Enable Wiggle";
            enableWiggle.UseVisualStyleBackColor = false;
            enableWiggle.CheckedChanged += enableWiggle_CheckedChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(180, 177, 163);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(bonusTextBox);
            panel1.Controls.Add(enableChatBonus);
            panel1.Controls.Add(holdAimCost);
            panel1.Controls.Add(enableHoldAim);
            panel1.Controls.Add(magDumpCost);
            panel1.Controls.Add(enableMagDump);
            panel1.Controls.Add(crouchBoxKey);
            panel1.Controls.Add(crouchBoxCost);
            panel1.Controls.Add(crouchBox);
            panel1.Controls.Add(grenadeKeyBox);
            panel1.Controls.Add(grenadeCostBox);
            panel1.Controls.Add(enableGrenadeToss);
            panel1.Controls.Add(enableTradersCommand);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(dropKeyTextBox);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(enableBagDrop);
            panel1.Controls.Add(dropbagCooldownTextBox);
            panel1.Controls.Add(enableGrenade);
            panel1.Controls.Add(grenadeCooldownTextBox);
            panel1.Controls.Add(randomKeyInputs);
            panel1.Controls.Add(oneClickCheck);
            panel1.Controls.Add(randomTurn);
            panel1.Controls.Add(enableRandomKey);
            panel1.Controls.Add(chkEnableGoose);
            panel1.Controls.Add(enableKitDrop);
            panel1.Controls.Add(enableWiggle);
            panel1.Controls.Add(wiggleCooldownTextBox);
            panel1.Controls.Add(dropCooldownTextBox);
            panel1.Controls.Add(gooseCooldownTextBox);
            panel1.Controls.Add(randomKeyCooldownTextBox);
            panel1.Controls.Add(turnCooldownTextBox);
            panel1.Controls.Add(oneClickCooldownTextBox);
            panel1.Controls.Add(pictureBox2);
            panel1.Location = new Point(51, 31);
            panel1.Name = "panel1";
            panel1.Size = new Size(514, 569);
            panel1.TabIndex = 20;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(279, 141);
            label4.Name = "label4";
            label4.Size = new Size(137, 15);
            label4.TabIndex = 61;
            label4.Text = "How many for first chat?";
            // 
            // bonusTextBox
            // 
            bonusTextBox.Location = new Point(280, 159);
            bonusTextBox.Name = "bonusTextBox";
            bonusTextBox.Size = new Size(100, 23);
            bonusTextBox.TabIndex = 60;
            bonusTextBox.Text = "100";
            // 
            // enableChatBonus
            // 
            enableChatBonus.AutoSize = true;
            enableChatBonus.BackColor = Color.FromArgb(181, 176, 163);
            enableChatBonus.Location = new Point(280, 119);
            enableChatBonus.Name = "enableChatBonus";
            enableChatBonus.Size = new Size(125, 19);
            enableChatBonus.TabIndex = 59;
            enableChatBonus.Text = "Enable Chat Bonus";
            enableChatBonus.UseVisualStyleBackColor = false;
            enableChatBonus.CheckedChanged += enableChatBonus_CheckedChanged;
            // 
            // holdAimCost
            // 
            holdAimCost.Location = new Point(154, 449);
            holdAimCost.Name = "holdAimCost";
            holdAimCost.Size = new Size(100, 23);
            holdAimCost.TabIndex = 58;
            holdAimCost.Text = "300";
            // 
            // enableHoldAim
            // 
            enableHoldAim.AutoSize = true;
            enableHoldAim.BackColor = Color.FromArgb(181, 176, 163);
            enableHoldAim.Location = new Point(14, 453);
            enableHoldAim.Name = "enableHoldAim";
            enableHoldAim.Size = new Size(115, 19);
            enableHoldAim.TabIndex = 57;
            enableHoldAim.Text = "Enable Hold Aim";
            enableHoldAim.UseVisualStyleBackColor = false;
            enableHoldAim.CheckedChanged += enableHoldAim_CheckedChanged;
            // 
            // magDumpCost
            // 
            magDumpCost.Location = new Point(154, 420);
            magDumpCost.Name = "magDumpCost";
            magDumpCost.Size = new Size(100, 23);
            magDumpCost.TabIndex = 56;
            magDumpCost.Text = "300";
            // 
            // enableMagDump
            // 
            enableMagDump.AutoSize = true;
            enableMagDump.BackColor = Color.FromArgb(181, 176, 163);
            enableMagDump.Location = new Point(14, 424);
            enableMagDump.Name = "enableMagDump";
            enableMagDump.Size = new Size(124, 19);
            enableMagDump.TabIndex = 55;
            enableMagDump.Text = "Enable Mag Dump";
            enableMagDump.UseVisualStyleBackColor = false;
            enableMagDump.CheckedChanged += enableMagDump_CheckedChanged;
            // 
            // crouchBoxKey
            // 
            crouchBoxKey.Location = new Point(260, 391);
            crouchBoxKey.Name = "crouchBoxKey";
            crouchBoxKey.Size = new Size(31, 23);
            crouchBoxKey.TabIndex = 54;
            crouchBoxKey.Text = "C";
            // 
            // crouchBoxCost
            // 
            crouchBoxCost.Location = new Point(154, 391);
            crouchBoxCost.Name = "crouchBoxCost";
            crouchBoxCost.Size = new Size(100, 23);
            crouchBoxCost.TabIndex = 53;
            crouchBoxCost.Text = "300";
            crouchBoxCost.TextChanged += crouchBoxCost_TextChanged;
            // 
            // crouchBox
            // 
            crouchBox.AutoSize = true;
            crouchBox.BackColor = Color.FromArgb(181, 176, 163);
            crouchBox.Location = new Point(14, 395);
            crouchBox.Name = "crouchBox";
            crouchBox.Size = new Size(65, 19);
            crouchBox.TabIndex = 52;
            crouchBox.Text = "Crouch";
            crouchBox.UseVisualStyleBackColor = false;
            crouchBox.CheckedChanged += crouchBox_CheckedChanged;
            // 
            // grenadeKeyBox
            // 
            grenadeKeyBox.Location = new Point(260, 362);
            grenadeKeyBox.Name = "grenadeKeyBox";
            grenadeKeyBox.Size = new Size(31, 23);
            grenadeKeyBox.TabIndex = 51;
            grenadeKeyBox.Text = "G";
            // 
            // grenadeCostBox
            // 
            grenadeCostBox.Location = new Point(154, 362);
            grenadeCostBox.Name = "grenadeCostBox";
            grenadeCostBox.Size = new Size(100, 23);
            grenadeCostBox.TabIndex = 50;
            grenadeCostBox.Text = "300";
            // 
            // enableGrenadeToss
            // 
            enableGrenadeToss.AutoSize = true;
            enableGrenadeToss.BackColor = Color.FromArgb(181, 176, 163);
            enableGrenadeToss.Location = new Point(14, 366);
            enableGrenadeToss.Name = "enableGrenadeToss";
            enableGrenadeToss.Size = new Size(159, 19);
            enableGrenadeToss.TabIndex = 49;
            enableGrenadeToss.Text = "Enable Grenade Toss(360)";
            enableGrenadeToss.UseVisualStyleBackColor = false;
            enableGrenadeToss.CheckedChanged += enableGrenadeToss_CheckedChanged;
            // 
            // enableTradersCommand
            // 
            enableTradersCommand.AutoSize = true;
            enableTradersCommand.BackColor = Color.FromArgb(181, 176, 163);
            enableTradersCommand.Location = new Point(279, 72);
            enableTradersCommand.Name = "enableTradersCommand";
            enableTradersCommand.Size = new Size(155, 19);
            enableTradersCommand.TabIndex = 48;
            enableTradersCommand.Text = "enableTradersCommand";
            enableTradersCommand.UseVisualStyleBackColor = false;
            enableTradersCommand.CheckedChanged += enableTradersCommand_CheckedChanged;
            // 
            // textBox3
            // 
            textBox3.BackColor = Color.FromArgb(181, 176, 163);
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Location = new Point(16, 220);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(133, 16);
            textBox3.TabIndex = 47;
            textBox3.Text = "Random buttons to press";
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(181, 176, 163);
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(49, 133);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 16);
            textBox2.TabIndex = 46;
            textBox2.Text = "Drop Item Button";
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // dropKeyTextBox
            // 
            dropKeyTextBox.Location = new Point(154, 130);
            dropKeyTextBox.Name = "dropKeyTextBox";
            dropKeyTextBox.Size = new Size(100, 23);
            dropKeyTextBox.TabIndex = 45;
            dropKeyTextBox.Text = "{DELETE}";
            dropKeyTextBox.TextChanged += dropKeyBox_TextChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(181, 176, 163);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(154, 49);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 16);
            textBox1.TabIndex = 44;
            textBox1.Text = "Cost";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(162, 123, 92);
            label2.Font = new Font("Cambria", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(15, 8);
            label2.Name = "label2";
            label2.Size = new Size(174, 25);
            label2.TabIndex = 29;
            label2.Text = "Command Toggles";
            // 
            // enableBagDrop
            // 
            enableBagDrop.AutoSize = true;
            enableBagDrop.BackColor = Color.FromArgb(181, 176, 163);
            enableBagDrop.Location = new Point(14, 337);
            enableBagDrop.Name = "enableBagDrop";
            enableBagDrop.Size = new Size(113, 19);
            enableBagDrop.TabIndex = 42;
            enableBagDrop.Text = "Enable Bag Drop";
            enableBagDrop.UseVisualStyleBackColor = false;
            enableBagDrop.CheckedChanged += enableBagDrop_CheckedChanged;
            // 
            // dropbagCooldownTextBox
            // 
            dropbagCooldownTextBox.Location = new Point(154, 333);
            dropbagCooldownTextBox.Name = "dropbagCooldownTextBox";
            dropbagCooldownTextBox.Size = new Size(100, 23);
            dropbagCooldownTextBox.TabIndex = 40;
            dropbagCooldownTextBox.Text = "300";
            // 
            // enableGrenade
            // 
            enableGrenade.AutoSize = true;
            enableGrenade.BackColor = Color.FromArgb(181, 176, 163);
            enableGrenade.Location = new Point(15, 308);
            enableGrenade.Name = "enableGrenade";
            enableGrenade.Size = new Size(108, 19);
            enableGrenade.TabIndex = 38;
            enableGrenade.Text = "Enable Grenade";
            enableGrenade.UseVisualStyleBackColor = false;
            enableGrenade.CheckedChanged += enableGrenade_CheckedChanged;
            // 
            // grenadeCooldownTextBox
            // 
            grenadeCooldownTextBox.Location = new Point(155, 304);
            grenadeCooldownTextBox.Name = "grenadeCooldownTextBox";
            grenadeCooldownTextBox.Size = new Size(100, 23);
            grenadeCooldownTextBox.TabIndex = 36;
            grenadeCooldownTextBox.Text = "300";
            // 
            // randomKeyInputs
            // 
            randomKeyInputs.Location = new Point(155, 217);
            randomKeyInputs.Name = "randomKeyInputs";
            randomKeyInputs.Size = new Size(206, 23);
            randomKeyInputs.TabIndex = 35;
            randomKeyInputs.Text = "W,A,S,D,E,Q,C,{TAB},G,2,3";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(0, 1);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(513, 429);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 21;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(586, 311);
            label1.Name = "label1";
            label1.Size = new Size(437, 90);
            label1.TabIndex = 28;
            label1.Text = resources.GetString("label1.Text");
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Red;
            pictureBox1.Location = new Point(-5, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 47);
            pictureBox1.TabIndex = 21;
            pictureBox1.TabStop = false;
            // 
            // pictureBox10
            // 
            pictureBox10.BackColor = Color.FromArgb(63, 78, 79);
            pictureBox10.Location = new Point(42, -3);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(757, 20);
            pictureBox10.TabIndex = 22;
            pictureBox10.TabStop = false;
            // 
            // autoMessageBox
            // 
            autoMessageBox.Location = new Point(734, 107);
            autoMessageBox.Multiline = true;
            autoMessageBox.Name = "autoMessageBox";
            autoMessageBox.Size = new Size(228, 106);
            autoMessageBox.TabIndex = 23;
            autoMessageBox.Text = "Send auto messages to chat! use \\\\ to send seperate messages";
            // 
            // autoSendMessageCD
            // 
            autoSendMessageCD.Location = new Point(657, 78);
            autoSendMessageCD.Name = "autoSendMessageCD";
            autoSendMessageCD.Size = new Size(54, 23);
            autoSendMessageCD.TabIndex = 24;
            autoSendMessageCD.Text = "300";
            // 
            // autoMessageLabel
            // 
            autoMessageLabel.AutoSize = true;
            autoMessageLabel.BackColor = Color.FromArgb(162, 123, 92);
            autoMessageLabel.Font = new Font("Cambria", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            autoMessageLabel.Location = new Point(596, 39);
            autoMessageLabel.Name = "autoMessageLabel";
            autoMessageLabel.Size = new Size(135, 25);
            autoMessageLabel.TabIndex = 25;
            autoMessageLabel.Text = "Auto Message";
            // 
            // enableAutoMessageCheck
            // 
            enableAutoMessageCheck.AutoSize = true;
            enableAutoMessageCheck.BackColor = Color.FromArgb(181, 176, 163);
            enableAutoMessageCheck.Location = new Point(735, 78);
            enableAutoMessageCheck.Name = "enableAutoMessageCheck";
            enableAutoMessageCheck.Size = new Size(139, 19);
            enableAutoMessageCheck.TabIndex = 26;
            enableAutoMessageCheck.Text = "Enable Auto Message";
            enableAutoMessageCheck.UseVisualStyleBackColor = false;
            enableAutoMessageCheck.CheckedChanged += enableAutoMessageCheck_CheckedChanged;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(586, 31);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(402, 236);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 27;
            pictureBox3.TabStop = false;
            // 
            // ControlMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1058, 700);
            ControlBox = false;
            Controls.Add(enableAutoMessageCheck);
            Controls.Add(autoMessageLabel);
            Controls.Add(autoSendMessageCD);
            Controls.Add(autoMessageBox);
            Controls.Add(pictureBox10);
            Controls.Add(pictureBox1);
            Controls.Add(saveButton);
            Controls.Add(panel1);
            Controls.Add(pictureBox3);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ControlMenu";
            Text = "ControlMenu";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox wiggleCooldownTextBox;
        private TextBox dropCooldownTextBox;
        private TextBox gooseCooldownTextBox;
        private TextBox randomKeyCooldownTextBox;
        private TextBox turnCooldownTextBox;
        private TextBox oneClickCooldownTextBox;
        private Button saveButton;
        private CheckBox oneClickCheck;
        private CheckBox randomTurn;
        private CheckBox enableRandomKey;
        private CheckBox chkEnableGoose;
        private CheckBox enableKitDrop;
        private CheckBox enableWiggle;
        private Panel panel1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox10;
        private TextBox autoMessageBox;
        private TextBox autoSendMessageCD;
        private Label autoMessageLabel;
        private CheckBox enableAutoMessageCheck;
        private PictureBox pictureBox3;
        private Label label1;
        private TextBox randomKeyInputs;
        private CheckBox enableGrenade;
        private TextBox grenadeCooldownTextBox;
        private CheckBox enableBagDrop;
        private TextBox dropbagCooldownTextBox;
        private Label label2;
        private TextBox textBox1;
        private TextBox dropKeyTextBox;
        private TextBox textBox2;
        private TextBox textBox3;
        private PictureBox pictureBox2;
        private CheckBox enableTradersCommand;
        private CheckBox enableMagDump;
        private TextBox grenadeCostBox;
        private CheckBox enableGrenadeToss;
        private TextBox grenadeKeyBox;
        private TextBox crouchBoxKey;
        private TextBox crouchBoxCost;
        private CheckBox crouchBox;
        private TextBox holdAimCost;
        private CheckBox enableHoldAim;
        private TextBox magDumpCost;
        private CheckBox enableChatBonus;
        private TextBox bonusTextBox;
        private Label label4;
    }
}