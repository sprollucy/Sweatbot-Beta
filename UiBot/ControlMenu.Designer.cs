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
            restart_AppButton = new Button();
            enableBotToggle = new CheckBox();
            label2 = new Label();
            bottoggleCostBox = new TextBox();
            checkEnableBitMsg = new CheckBox();
            textBox1 = new TextBox();
            enableTradersCommand = new CheckBox();
            label1 = new Label();
            pictureBox2 = new PictureBox();
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
            textBox4 = new TextBox();
            modRefund = new CheckBox();
            modWhitelistCheck = new CheckBox();
            openModWhitelist = new Button();
            saveMessageButton = new Button();
            panel2 = new Panel();
            pictureBox7 = new PictureBox();
            panel3 = new Panel();
            panel4 = new Panel();
            pictureBox3 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // saveButton
            // 
            saveButton.Location = new Point(355, 8);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(220, 215, 201);
            panel1.Controls.Add(restart_AppButton);
            panel1.Controls.Add(saveButton);
            panel1.Controls.Add(enableBotToggle);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(bottoggleCostBox);
            panel1.Controls.Add(checkEnableBitMsg);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(enableTradersCommand);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox2);
            panel1.Location = new Point(53, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(514, 184);
            panel1.TabIndex = 20;
            // 
            // restart_AppButton
            // 
            restart_AppButton.Location = new Point(436, 8);
            restart_AppButton.Name = "restart_AppButton";
            restart_AppButton.Size = new Size(75, 23);
            restart_AppButton.TabIndex = 29;
            restart_AppButton.Text = "Restart App";
            restart_AppButton.UseVisualStyleBackColor = true;
            restart_AppButton.Click += button1_Click;
            // 
            // enableBotToggle
            // 
            enableBotToggle.AutoSize = true;
            enableBotToggle.BackColor = Color.FromArgb(220, 215, 201);
            enableBotToggle.Location = new Point(12, 85);
            enableBotToggle.Name = "enableBotToggle";
            enableBotToggle.Size = new Size(151, 19);
            enableBotToggle.TabIndex = 109;
            enableBotToggle.Text = "Enable Sweatbot Toggle";
            enableBotToggle.UseVisualStyleBackColor = false;
            enableBotToggle.CheckedChanged += enableBotToggle_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(162, 123, 92);
            label2.Font = new Font("Cambria", 15.75F);
            label2.Location = new Point(12, 8);
            label2.Name = "label2";
            label2.Size = new Size(179, 25);
            label2.TabIndex = 29;
            label2.Text = "Command Settings";
            // 
            // bottoggleCostBox
            // 
            bottoggleCostBox.Location = new Point(217, 83);
            bottoggleCostBox.Name = "bottoggleCostBox";
            bottoggleCostBox.Size = new Size(100, 23);
            bottoggleCostBox.TabIndex = 108;
            bottoggleCostBox.Text = "300";
            // 
            // checkEnableBitMsg
            // 
            checkEnableBitMsg.AutoSize = true;
            checkEnableBitMsg.BackColor = Color.FromArgb(220, 215, 201);
            checkEnableBitMsg.Location = new Point(12, 139);
            checkEnableBitMsg.Name = "checkEnableBitMsg";
            checkEnableBitMsg.Size = new Size(295, 19);
            checkEnableBitMsg.TabIndex = 115;
            checkEnableBitMsg.Text = "Enable remaining bits message after command use";
            checkEnableBitMsg.UseVisualStyleBackColor = false;
            checkEnableBitMsg.CheckedChanged += checkEnableBitMsg_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(220, 215, 201);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(169, 86);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(63, 16);
            textBox1.TabIndex = 44;
            textBox1.Text = "Cost";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // enableTradersCommand
            // 
            enableTradersCommand.AutoSize = true;
            enableTradersCommand.BackColor = Color.FromArgb(220, 215, 201);
            enableTradersCommand.Location = new Point(12, 114);
            enableTradersCommand.Name = "enableTradersCommand";
            enableTradersCommand.Size = new Size(200, 19);
            enableTradersCommand.TabIndex = 48;
            enableTradersCommand.Text = "Enable Trader Command for chat";
            enableTradersCommand.UseVisualStyleBackColor = false;
            enableTradersCommand.CheckedChanged += enableTradersCommand_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(220, 215, 201);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(68, 47);
            label1.Name = "label1";
            label1.Size = new Size(375, 30);
            label1.TabIndex = 28;
            label1.Text = "Currently you need to save and restart the app after changing the cost\r\nof the commands. ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(514, 33);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 120;
            pictureBox2.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(162, 123, 92);
            label5.Font = new Font("Cambria", 15.75F);
            label5.Location = new Point(12, 9);
            label5.Name = "label5";
            label5.Size = new Size(134, 25);
            label5.TabIndex = 120;
            label5.Text = "Chat Bonuses";
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(0, 1);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(514, 33);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 120;
            pictureBox8.TabStop = false;
            // 
            // enableChatBonus
            // 
            enableChatBonus.AutoSize = true;
            enableChatBonus.BackColor = Color.FromArgb(220, 215, 201);
            enableChatBonus.Location = new Point(12, 50);
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
            label4.BackColor = Color.FromArgb(220, 215, 201);
            label4.Location = new Point(143, 52);
            label4.Name = "label4";
            label4.Size = new Size(137, 15);
            label4.TabIndex = 61;
            label4.Text = "How many for first chat?";
            // 
            // bonusTextBox
            // 
            bonusTextBox.Location = new Point(286, 50);
            bonusTextBox.Name = "bonusTextBox";
            bonusTextBox.Size = new Size(55, 23);
            bonusTextBox.TabIndex = 60;
            bonusTextBox.Text = "100";
            // 
            // enableBonusMulti
            // 
            enableBonusMulti.AutoSize = true;
            enableBonusMulti.BackColor = Color.FromArgb(220, 215, 201);
            enableBonusMulti.Location = new Point(12, 75);
            enableBonusMulti.Name = "enableBonusMulti";
            enableBonusMulti.Size = new Size(132, 19);
            enableBonusMulti.TabIndex = 86;
            enableBonusMulti.Text = "Enable bit multiplier";
            enableBonusMulti.UseVisualStyleBackColor = false;
            enableBonusMulti.CheckedChanged += enableBonusMulti_CheckedChanged;
            // 
            // bonusMultiplierBox
            // 
            bonusMultiplierBox.Location = new Point(150, 73);
            bonusMultiplierBox.Name = "bonusMultiplierBox";
            bonusMultiplierBox.Size = new Size(23, 23);
            bonusMultiplierBox.TabIndex = 106;
            bonusMultiplierBox.Text = "2";
            // 
            // enableModBits
            // 
            enableModBits.AutoSize = true;
            enableModBits.BackColor = Color.FromArgb(220, 215, 201);
            enableModBits.Location = new Point(10, 49);
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
            autoMessageBox.Location = new Point(12, 71);
            autoMessageBox.Multiline = true;
            autoMessageBox.Name = "autoMessageBox";
            autoMessageBox.Size = new Size(228, 106);
            autoMessageBox.TabIndex = 23;
            autoMessageBox.Text = "Send auto messages to chat! use \\\\ to send seperate messages";
            // 
            // autoSendMessageCD
            // 
            autoSendMessageCD.Location = new Point(307, 93);
            autoSendMessageCD.Name = "autoSendMessageCD";
            autoSendMessageCD.Size = new Size(54, 23);
            autoSendMessageCD.TabIndex = 24;
            autoSendMessageCD.Text = "300";
            // 
            // autoMessageLabel
            // 
            autoMessageLabel.AutoSize = true;
            autoMessageLabel.BackColor = Color.FromArgb(162, 123, 92);
            autoMessageLabel.Font = new Font("Cambria", 15.75F);
            autoMessageLabel.Location = new Point(12, 9);
            autoMessageLabel.Name = "autoMessageLabel";
            autoMessageLabel.Size = new Size(135, 25);
            autoMessageLabel.TabIndex = 25;
            autoMessageLabel.Text = "Auto Message";
            // 
            // enableAutoMessageCheck
            // 
            enableAutoMessageCheck.AutoSize = true;
            enableAutoMessageCheck.BackColor = Color.FromArgb(220, 215, 201);
            enableAutoMessageCheck.Location = new Point(12, 46);
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
            label3.BackColor = Color.FromArgb(162, 123, 92);
            label3.Font = new Font("Cambria", 15.75F);
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(301, 25);
            label3.TabIndex = 80;
            label3.Text = "Moderator Control and Bonuses";
            // 
            // textBox4
            // 
            textBox4.BackColor = Color.FromArgb(220, 215, 201);
            textBox4.BorderStyle = BorderStyle.None;
            textBox4.Location = new Point(246, 71);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(182, 16);
            textBox4.TabIndex = 82;
            textBox4.Text = "How often to send message?";
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // modRefund
            // 
            modRefund.AutoSize = true;
            modRefund.BackColor = Color.FromArgb(220, 215, 201);
            modRefund.Location = new Point(10, 72);
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
            modWhitelistCheck.BackColor = Color.FromArgb(220, 215, 201);
            modWhitelistCheck.Location = new Point(10, 97);
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
            openModWhitelist.Location = new Point(267, 94);
            openModWhitelist.Name = "openModWhitelist";
            openModWhitelist.Size = new Size(95, 23);
            openModWhitelist.TabIndex = 85;
            openModWhitelist.Text = "Open Whitelist";
            openModWhitelist.UseVisualStyleBackColor = true;
            openModWhitelist.Click += openModWhitelist_Click;
            // 
            // saveMessageButton
            // 
            saveMessageButton.Location = new Point(246, 133);
            saveMessageButton.Name = "saveMessageButton";
            saveMessageButton.Size = new Size(182, 44);
            saveMessageButton.TabIndex = 119;
            saveMessageButton.Text = "Save";
            saveMessageButton.UseVisualStyleBackColor = true;
            saveMessageButton.Click += saveMessageButton_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(220, 215, 201);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(pictureBox8);
            panel2.Controls.Add(bonusMultiplierBox);
            panel2.Controls.Add(enableBonusMulti);
            panel2.Controls.Add(bonusTextBox);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(enableChatBonus);
            panel2.Location = new Point(53, 240);
            panel2.Name = "panel2";
            panel2.Size = new Size(514, 192);
            panel2.TabIndex = 121;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox7.BackgroundImageLayout = ImageLayout.None;
            pictureBox7.Location = new Point(0, 1);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(514, 33);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 120;
            pictureBox7.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(220, 215, 201);
            panel3.Controls.Add(autoMessageLabel);
            panel3.Controls.Add(enableAutoMessageCheck);
            panel3.Controls.Add(saveMessageButton);
            panel3.Controls.Add(pictureBox7);
            panel3.Controls.Add(autoMessageBox);
            panel3.Controls.Add(textBox4);
            panel3.Controls.Add(autoSendMessageCD);
            panel3.Location = new Point(585, 241);
            panel3.Name = "panel3";
            panel3.Size = new Size(434, 191);
            panel3.TabIndex = 122;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(220, 215, 201);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(pictureBox3);
            panel4.Controls.Add(enableModBits);
            panel4.Controls.Add(modRefund);
            panel4.Controls.Add(openModWhitelist);
            panel4.Controls.Add(modWhitelistCheck);
            panel4.Location = new Point(585, 35);
            panel4.Name = "panel4";
            panel4.Size = new Size(434, 191);
            panel4.TabIndex = 123;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox3.BackgroundImageLayout = ImageLayout.None;
            pictureBox3.Location = new Point(0, 1);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(514, 33);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 120;
            pictureBox3.TabStop = false;
            // 
            // ControlMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1058, 508);
            ControlBox = false;
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
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
            ResumeLayout(false);
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
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private CheckBox enableTradersCommand;
        private CheckBox enableChatBonus;
        private TextBox bonusTextBox;
        private Label label4;
        private Button restart_AppButton;
        private CheckBox enableModBits;
        private Label label3;
        private TextBox textBox4;
        private CheckBox modRefund;
        private CheckBox modWhitelistCheck;
        private Button openModWhitelist;
        private CheckBox enableBonusMulti;
        private TextBox bonusMultiplierBox;
        private CheckBox enableBotToggle;
        private TextBox bottoggleCostBox;
        private CheckBox checkEnableBitMsg;
        private Button saveMessageButton;
        private Label label5;
        private PictureBox pictureBox8;
        private PictureBox pictureBox2;
        private Panel panel2;
        private PictureBox pictureBox7;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pictureBox3;
    }
}