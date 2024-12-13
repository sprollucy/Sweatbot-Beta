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
            saveButton = new Button();
            panel1 = new Panel();
            bitcostButton = new CheckBox();
            enableBotToggle = new CheckBox();
            bottoggleCostBox = new TextBox();
            checkEnableBitMsg = new CheckBox();
            textBox1 = new TextBox();
            enableTradersCommand = new CheckBox();
            pictureBox2 = new PictureBox();
            restart_AppButton = new Button();
            label2 = new Label();
            label5 = new Label();
            pictureBox8 = new PictureBox();
            enableChatBonus = new CheckBox();
            label4 = new Label();
            bonusTextBox = new TextBox();
            enableBonusMulti = new CheckBox();
            bonusMultiplierBox = new TextBox();
            enableModBits = new CheckBox();
            pictureBox1 = new PictureBox();
            autoMessageBox = new TextBox();
            autoSendMessageCD = new TextBox();
            autoMessageLabel = new Label();
            enableAutoMessageCheck = new CheckBox();
            label3 = new Label();
            modRefund = new CheckBox();
            modWhitelistCheck = new CheckBox();
            openModWhitelist = new Button();
            panel2 = new Panel();
            subTextBox = new TextBox();
            label7 = new Label();
            enableSubBonus = new CheckBox();
            followTextBox = new TextBox();
            label6 = new Label();
            enableFollowBonus = new CheckBox();
            pictureBox7 = new PictureBox();
            panel3 = new Panel();
            label8 = new Label();
            panel4 = new Panel();
            pictureBox3 = new PictureBox();
            groupBox1 = new GroupBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // saveButton
            // 
            saveButton.ForeColor = SystemColors.ControlText;
            saveButton.Location = new Point(4, 24);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(160, 43);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(156, 155, 151);
            panel1.Controls.Add(bitcostButton);
            panel1.Controls.Add(enableBotToggle);
            panel1.Controls.Add(bottoggleCostBox);
            panel1.Controls.Add(checkEnableBitMsg);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(enableTradersCommand);
            panel1.Controls.Add(pictureBox2);
            panel1.ForeColor = SystemColors.ControlText;
            panel1.Location = new Point(53, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(514, 191);
            panel1.TabIndex = 20;
            // 
            // bitcostButton
            // 
            bitcostButton.AutoSize = true;
            bitcostButton.BackColor = Color.FromArgb(156, 155, 151);
            bitcostButton.Checked = true;
            bitcostButton.CheckState = CheckState.Checked;
            bitcostButton.ForeColor = SystemColors.ControlText;
            bitcostButton.Location = new Point(12, 88);
            bitcostButton.Name = "bitcostButton";
            bitcostButton.Size = new Size(205, 19);
            bitcostButton.TabIndex = 121;
            bitcostButton.Text = "Enable !bitcost command for chat";
            bitcostButton.UseVisualStyleBackColor = false;
            bitcostButton.CheckedChanged += bitcostButton_CheckedChanged;
            // 
            // enableBotToggle
            // 
            enableBotToggle.AutoSize = true;
            enableBotToggle.BackColor = Color.FromArgb(156, 155, 151);
            enableBotToggle.ForeColor = SystemColors.ControlText;
            enableBotToggle.Location = new Point(12, 36);
            enableBotToggle.Name = "enableBotToggle";
            enableBotToggle.Size = new Size(151, 19);
            enableBotToggle.TabIndex = 109;
            enableBotToggle.Text = "Enable Sweatbot Toggle";
            enableBotToggle.UseVisualStyleBackColor = false;
            enableBotToggle.CheckedChanged += enableBotToggle_CheckedChanged;
            // 
            // bottoggleCostBox
            // 
            bottoggleCostBox.BackColor = SystemColors.ControlLight;
            bottoggleCostBox.BorderStyle = BorderStyle.None;
            bottoggleCostBox.Location = new Point(216, 37);
            bottoggleCostBox.Name = "bottoggleCostBox";
            bottoggleCostBox.Size = new Size(34, 16);
            bottoggleCostBox.TabIndex = 108;
            bottoggleCostBox.Text = "300";
            bottoggleCostBox.TextAlign = HorizontalAlignment.Center;
            // 
            // checkEnableBitMsg
            // 
            checkEnableBitMsg.AutoSize = true;
            checkEnableBitMsg.BackColor = Color.FromArgb(156, 155, 151);
            checkEnableBitMsg.Checked = true;
            checkEnableBitMsg.CheckState = CheckState.Checked;
            checkEnableBitMsg.ForeColor = SystemColors.ControlText;
            checkEnableBitMsg.Location = new Point(12, 63);
            checkEnableBitMsg.Name = "checkEnableBitMsg";
            checkEnableBitMsg.Size = new Size(295, 19);
            checkEnableBitMsg.TabIndex = 115;
            checkEnableBitMsg.Text = "Enable remaining bits message after command use";
            checkEnableBitMsg.UseVisualStyleBackColor = false;
            checkEnableBitMsg.CheckedChanged += checkEnableBitMsg_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(156, 155, 151);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.ForeColor = SystemColors.ControlText;
            textBox1.Location = new Point(169, 37);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(63, 16);
            textBox1.TabIndex = 44;
            textBox1.Text = "Cost";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // enableTradersCommand
            // 
            enableTradersCommand.AutoSize = true;
            enableTradersCommand.BackColor = Color.FromArgb(156, 155, 151);
            enableTradersCommand.ForeColor = SystemColors.ControlText;
            enableTradersCommand.Location = new Point(12, 113);
            enableTradersCommand.Name = "enableTradersCommand";
            enableTradersCommand.Size = new Size(200, 19);
            enableTradersCommand.TabIndex = 48;
            enableTradersCommand.Text = "Enable Trader Command for chat";
            enableTradersCommand.UseVisualStyleBackColor = false;
            enableTradersCommand.CheckedChanged += enableTradersCommand_CheckedChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(514, 24);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 120;
            pictureBox2.TabStop = false;
            // 
            // restart_AppButton
            // 
            restart_AppButton.ForeColor = SystemColors.ControlText;
            restart_AppButton.Location = new Point(169, 24);
            restart_AppButton.Name = "restart_AppButton";
            restart_AppButton.Size = new Size(160, 43);
            restart_AppButton.TabIndex = 29;
            restart_AppButton.Text = "Restart App";
            restart_AppButton.UseVisualStyleBackColor = true;
            restart_AppButton.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(71, 83, 92);
            label2.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label2.ForeColor = SystemColors.ControlLight;
            label2.Location = new Point(53, 36);
            label2.Name = "label2";
            label2.Size = new Size(153, 19);
            label2.TabIndex = 29;
            label2.Text = "Command Settings";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(71, 83, 92);
            label5.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label5.ForeColor = SystemColors.ControlLight;
            label5.Location = new Point(0, 1);
            label5.Name = "label5";
            label5.Size = new Size(113, 19);
            label5.TabIndex = 120;
            label5.Text = "Chat Bonuses";
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(0, 0);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(514, 24);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 120;
            pictureBox8.TabStop = false;
            // 
            // enableChatBonus
            // 
            enableChatBonus.AutoSize = true;
            enableChatBonus.BackColor = Color.FromArgb(156, 155, 151);
            enableChatBonus.ForeColor = SystemColors.ControlText;
            enableChatBonus.Location = new Point(12, 31);
            enableChatBonus.Name = "enableChatBonus";
            enableChatBonus.Size = new Size(125, 19);
            enableChatBonus.TabIndex = 59;
            enableChatBonus.Text = "Enable Chat Bonus";
            enableChatBonus.UseVisualStyleBackColor = false;
            enableChatBonus.CheckedChanged += enableChatBonus_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(156, 155, 151);
            label4.ForeColor = SystemColors.ControlText;
            label4.Location = new Point(143, 32);
            label4.Name = "label4";
            label4.Size = new Size(137, 15);
            label4.TabIndex = 61;
            label4.Text = "How many for first chat?";
            // 
            // bonusTextBox
            // 
            bonusTextBox.BackColor = SystemColors.ControlLight;
            bonusTextBox.BorderStyle = BorderStyle.None;
            bonusTextBox.Location = new Point(298, 32);
            bonusTextBox.Name = "bonusTextBox";
            bonusTextBox.Size = new Size(33, 16);
            bonusTextBox.TabIndex = 60;
            bonusTextBox.Text = "100";
            bonusTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // enableBonusMulti
            // 
            enableBonusMulti.AutoSize = true;
            enableBonusMulti.BackColor = Color.FromArgb(156, 155, 151);
            enableBonusMulti.ForeColor = SystemColors.ControlText;
            enableBonusMulti.Location = new Point(12, 100);
            enableBonusMulti.Name = "enableBonusMulti";
            enableBonusMulti.Size = new Size(132, 19);
            enableBonusMulti.TabIndex = 86;
            enableBonusMulti.Text = "Enable Bit Multiplier";
            enableBonusMulti.UseVisualStyleBackColor = false;
            enableBonusMulti.CheckedChanged += enableBonusMulti_CheckedChanged;
            // 
            // bonusMultiplierBox
            // 
            bonusMultiplierBox.BackColor = SystemColors.ControlLight;
            bonusMultiplierBox.BorderStyle = BorderStyle.None;
            bonusMultiplierBox.Location = new Point(143, 101);
            bonusMultiplierBox.Name = "bonusMultiplierBox";
            bonusMultiplierBox.Size = new Size(20, 16);
            bonusMultiplierBox.TabIndex = 106;
            bonusMultiplierBox.Text = "2";
            bonusMultiplierBox.TextAlign = HorizontalAlignment.Center;
            bonusMultiplierBox.TextChanged += bonusMultiplierBox_TextChanged;
            // 
            // enableModBits
            // 
            enableModBits.AutoSize = true;
            enableModBits.BackColor = Color.FromArgb(156, 155, 151);
            enableModBits.ForeColor = SystemColors.ControlText;
            enableModBits.Location = new Point(10, 36);
            enableModBits.Name = "enableModBits";
            enableModBits.Size = new Size(181, 19);
            enableModBits.TabIndex = 76;
            enableModBits.Text = "Allow Moderators to give bits";
            enableModBits.UseVisualStyleBackColor = false;
            enableModBits.CheckedChanged += enableModBits_CheckedChanged;
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
            // autoMessageBox
            // 
            autoMessageBox.BackColor = SystemColors.ControlLight;
            autoMessageBox.Location = new Point(10, 55);
            autoMessageBox.Multiline = true;
            autoMessageBox.Name = "autoMessageBox";
            autoMessageBox.Size = new Size(228, 106);
            autoMessageBox.TabIndex = 23;
            autoMessageBox.Text = "Send auto messages to chat! use \\\\ to send seperate messages";
            // 
            // autoSendMessageCD
            // 
            autoSendMessageCD.BackColor = SystemColors.ControlLight;
            autoSendMessageCD.Location = new Point(380, 70);
            autoSendMessageCD.Name = "autoSendMessageCD";
            autoSendMessageCD.Size = new Size(43, 23);
            autoSendMessageCD.TabIndex = 24;
            autoSendMessageCD.Text = "300";
            autoSendMessageCD.TextAlign = HorizontalAlignment.Center;
            // 
            // autoMessageLabel
            // 
            autoMessageLabel.AutoSize = true;
            autoMessageLabel.BackColor = Color.FromArgb(71, 83, 92);
            autoMessageLabel.Font = new Font("Constantia", 12F, FontStyle.Bold);
            autoMessageLabel.ForeColor = SystemColors.ControlLight;
            autoMessageLabel.Location = new Point(0, 0);
            autoMessageLabel.Name = "autoMessageLabel";
            autoMessageLabel.Size = new Size(115, 19);
            autoMessageLabel.TabIndex = 25;
            autoMessageLabel.Text = "Auto Message";
            // 
            // enableAutoMessageCheck
            // 
            enableAutoMessageCheck.AutoSize = true;
            enableAutoMessageCheck.BackColor = Color.FromArgb(156, 155, 151);
            enableAutoMessageCheck.ForeColor = SystemColors.ControlText;
            enableAutoMessageCheck.Location = new Point(10, 30);
            enableAutoMessageCheck.Name = "enableAutoMessageCheck";
            enableAutoMessageCheck.Size = new Size(139, 19);
            enableAutoMessageCheck.TabIndex = 26;
            enableAutoMessageCheck.Text = "Enable Auto Message";
            enableAutoMessageCheck.UseVisualStyleBackColor = false;
            enableAutoMessageCheck.CheckedChanged += enableAutoMessageCheck_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(71, 83, 92);
            label3.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ControlLight;
            label3.Location = new Point(0, 1);
            label3.Name = "label3";
            label3.Size = new Size(161, 19);
            label3.TabIndex = 80;
            label3.Text = "Moderator Controls";
            // 
            // modRefund
            // 
            modRefund.AutoSize = true;
            modRefund.BackColor = Color.FromArgb(156, 155, 151);
            modRefund.Checked = true;
            modRefund.CheckState = CheckState.Checked;
            modRefund.ForeColor = SystemColors.ControlText;
            modRefund.Location = new Point(10, 59);
            modRefund.Name = "modRefund";
            modRefund.Size = new Size(202, 19);
            modRefund.TabIndex = 83;
            modRefund.Text = "Allow Moderators to give refunds";
            modRefund.UseVisualStyleBackColor = false;
            modRefund.CheckedChanged += modRefund_CheckedChanged;
            // 
            // modWhitelistCheck
            // 
            modWhitelistCheck.AutoSize = true;
            modWhitelistCheck.BackColor = Color.FromArgb(156, 155, 151);
            modWhitelistCheck.ForeColor = SystemColors.ControlText;
            modWhitelistCheck.Location = new Point(10, 84);
            modWhitelistCheck.Name = "modWhitelistCheck";
            modWhitelistCheck.Size = new Size(254, 19);
            modWhitelistCheck.TabIndex = 84;
            modWhitelistCheck.Text = "Enable Moderator Whitelist for bits/refunds";
            modWhitelistCheck.UseVisualStyleBackColor = false;
            modWhitelistCheck.CheckedChanged += modWhitelistCheck_CheckedChanged;
            // 
            // openModWhitelist
            // 
            openModWhitelist.Font = new Font("Segoe UI", 9F);
            openModWhitelist.Location = new Point(267, 81);
            openModWhitelist.Name = "openModWhitelist";
            openModWhitelist.Size = new Size(95, 23);
            openModWhitelist.TabIndex = 85;
            openModWhitelist.Text = "Open Whitelist";
            openModWhitelist.UseVisualStyleBackColor = true;
            openModWhitelist.Click += openModWhitelist_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(156, 155, 151);
            panel2.Controls.Add(subTextBox);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(enableSubBonus);
            panel2.Controls.Add(followTextBox);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(enableFollowBonus);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(pictureBox8);
            panel2.Controls.Add(bonusMultiplierBox);
            panel2.Controls.Add(enableBonusMulti);
            panel2.Controls.Add(bonusTextBox);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(enableChatBonus);
            panel2.ForeColor = SystemColors.ControlText;
            panel2.Location = new Point(53, 240);
            panel2.Name = "panel2";
            panel2.Size = new Size(514, 192);
            panel2.TabIndex = 121;
            // 
            // subTextBox
            // 
            subTextBox.BackColor = SystemColors.ControlLight;
            subTextBox.BorderStyle = BorderStyle.None;
            subTextBox.Location = new Point(298, 76);
            subTextBox.Name = "subTextBox";
            subTextBox.Size = new Size(33, 16);
            subTextBox.TabIndex = 125;
            subTextBox.Text = "100";
            subTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(156, 155, 151);
            label7.ForeColor = SystemColors.ControlText;
            label7.Location = new Point(143, 76);
            label7.Name = "label7";
            label7.Size = new Size(155, 15);
            label7.TabIndex = 126;
            label7.Text = "How many for subscribing? ";
            // 
            // enableSubBonus
            // 
            enableSubBonus.AutoSize = true;
            enableSubBonus.BackColor = Color.FromArgb(156, 155, 151);
            enableSubBonus.ForeColor = SystemColors.ControlText;
            enableSubBonus.Location = new Point(12, 75);
            enableSubBonus.Name = "enableSubBonus";
            enableSubBonus.Size = new Size(120, 19);
            enableSubBonus.TabIndex = 124;
            enableSubBonus.Text = "Enable Sub Bonus";
            enableSubBonus.UseVisualStyleBackColor = false;
            enableSubBonus.CheckedChanged += enableSubBonus_CheckedChanged;
            // 
            // followTextBox
            // 
            followTextBox.BackColor = SystemColors.ControlLight;
            followTextBox.BorderStyle = BorderStyle.None;
            followTextBox.Location = new Point(298, 54);
            followTextBox.Name = "followTextBox";
            followTextBox.Size = new Size(33, 16);
            followTextBox.TabIndex = 122;
            followTextBox.Text = "100";
            followTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(156, 155, 151);
            label6.ForeColor = SystemColors.ControlText;
            label6.Location = new Point(143, 54);
            label6.Name = "label6";
            label6.Size = new Size(141, 15);
            label6.TabIndex = 123;
            label6.Text = "How many for following?";
            // 
            // enableFollowBonus
            // 
            enableFollowBonus.AutoSize = true;
            enableFollowBonus.BackColor = Color.FromArgb(156, 155, 151);
            enableFollowBonus.ForeColor = SystemColors.ControlText;
            enableFollowBonus.Location = new Point(12, 53);
            enableFollowBonus.Name = "enableFollowBonus";
            enableFollowBonus.Size = new Size(135, 19);
            enableFollowBonus.TabIndex = 121;
            enableFollowBonus.Text = "Enable Follow Bonus";
            enableFollowBonus.UseVisualStyleBackColor = false;
            enableFollowBonus.CheckedChanged += enableFollowBonus_CheckedChanged;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox7.BackgroundImageLayout = ImageLayout.None;
            pictureBox7.Location = new Point(0, 0);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(442, 24);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 120;
            pictureBox7.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(156, 155, 151);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(autoMessageLabel);
            panel3.Controls.Add(enableAutoMessageCheck);
            panel3.Controls.Add(pictureBox7);
            panel3.Controls.Add(autoMessageBox);
            panel3.Controls.Add(autoSendMessageCD);
            panel3.ForeColor = SystemColors.ControlText;
            panel3.Location = new Point(585, 241);
            panel3.Name = "panel3";
            panel3.Size = new Size(434, 191);
            panel3.TabIndex = 122;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(244, 73);
            label8.Name = "label8";
            label8.Size = new Size(135, 15);
            label8.TabIndex = 121;
            label8.Text = "How often to send?(sec)";
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(156, 155, 151);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(pictureBox3);
            panel4.Controls.Add(enableModBits);
            panel4.Controls.Add(modRefund);
            panel4.Controls.Add(openModWhitelist);
            panel4.Controls.Add(modWhitelistCheck);
            panel4.ForeColor = SystemColors.ControlText;
            panel4.Location = new Point(585, 35);
            panel4.Name = "panel4";
            panel4.Size = new Size(434, 191);
            panel4.TabIndex = 123;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox3.BackgroundImageLayout = ImageLayout.None;
            pictureBox3.Location = new Point(0, 0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(442, 24);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 120;
            pictureBox3.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(saveButton);
            groupBox1.Controls.Add(restart_AppButton);
            groupBox1.Font = new Font("Segoe UI", 10F);
            groupBox1.ForeColor = Color.Salmon;
            groupBox1.Location = new Point(685, 438);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(334, 79);
            groupBox1.TabIndex = 124;
            groupBox1.TabStop = false;
            groupBox1.Text = "You must restart if changing any of the text boxes!";
            // 
            // ControlMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(37, 37, 37);
            ClientSize = new Size(1058, 609);
            ControlBox = false;
            Controls.Add(groupBox1);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ControlMenu";
            Text = "ControlMenu";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button saveButton;
        private Panel panel1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox10;
        private TextBox autoMessageBox;
        private TextBox autoSendMessageCD;
        private Label autoMessageLabel;
        private CheckBox enableAutoMessageCheck;
        private Label label2;
        private TextBox textBox1;
        private CheckBox enableTradersCommand;
        private CheckBox enableChatBonus;
        private TextBox bonusTextBox;
        private Label label4;
        private Button restart_AppButton;
        private CheckBox enableModBits;
        private Label label3;
        private CheckBox modRefund;
        private CheckBox modWhitelistCheck;
        private Button openModWhitelist;
        private CheckBox enableBonusMulti;
        private TextBox bonusMultiplierBox;
        private CheckBox enableBotToggle;
        private TextBox bottoggleCostBox;
        private CheckBox checkEnableBitMsg;
        private Label label5;
        private PictureBox pictureBox8;
        private PictureBox pictureBox2;
        private Panel panel2;
        private PictureBox pictureBox7;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pictureBox3;
        private TextBox subTextBox;
        private Label label7;
        private CheckBox enableSubBonus;
        private TextBox followTextBox;
        private Label label6;
        private CheckBox enableFollowBonus;
        private Label label8;
        private CheckBox bitcostButton;
        private GroupBox groupBox1;
    }
}