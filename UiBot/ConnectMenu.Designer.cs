namespace UiBot
{
    partial class ConnectMenu
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
            messageTextBox = new TextBox();
            consoleTextBox = new TextBox();
            connectButton = new Button();
            disconnectButton = new Button();
            label4 = new Label();
            econoPanel = new Panel();
            economyLastLabel = new Label();
            label7 = new Label();
            pictureBox7 = new PictureBox();
            label6 = new Label();
            pictureBox6 = new PictureBox();
            economySpentLabel = new Label();
            label5 = new Label();
            economyLabel = new Label();
            pictureBox1 = new PictureBox();
            pictureBox4 = new PictureBox();
            customCommandBox = new RichTextBox();
            panel2 = new Panel();
            label12 = new Label();
            economyCheckBox = new CheckBox();
            label1 = new Label();
            backupButton = new Button();
            twitchOpen = new Button();
            pauseCommands = new CheckBox();
            pictureBox8 = new PictureBox();
            label2 = new Label();
            consoleTab = new Label();
            pictureBox2 = new PictureBox();
            pictureBox5 = new PictureBox();
            manramLabel = new Label();
            allocramLabel = new Label();
            ramLabel = new Label();
            debugGroup = new GroupBox();
            ramSnapButton = new Button();
            label20 = new Label();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            chatComPanel = new Panel();
            label22 = new Label();
            pictureBox9 = new PictureBox();
            regComLabel = new Label();
            panel1 = new Panel();
            logListPanel = new FlowLayoutPanel();
            refundPanel = new Panel();
            refundTab = new Label();
            econoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            debugGroup.SuspendLayout();
            chatComPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            panel1.SuspendLayout();
            refundPanel.SuspendLayout();
            SuspendLayout();
            // 
            // messageTextBox
            // 
            messageTextBox.BackColor = SystemColors.ControlLight;
            messageTextBox.BorderStyle = BorderStyle.None;
            messageTextBox.Location = new Point(53, 419);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.PlaceholderText = "Type here to send a message to chat. Enter to send or test a command with ! in front of its name";
            messageTextBox.Size = new Size(598, 16);
            messageTextBox.TabIndex = 0;
            // 
            // consoleTextBox
            // 
            consoleTextBox.BackColor = Color.FromArgb(156, 155, 151);
            consoleTextBox.BorderStyle = BorderStyle.None;
            consoleTextBox.Font = new Font("Arial", 12F);
            consoleTextBox.Location = new Point(53, 54);
            consoleTextBox.Multiline = true;
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.ReadOnly = true;
            consoleTextBox.ScrollBars = ScrollBars.Vertical;
            consoleTextBox.Size = new Size(598, 359);
            consoleTextBox.TabIndex = 1;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(53, 445);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(75, 23);
            connectButton.TabIndex = 2;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // disconnectButton
            // 
            disconnectButton.Location = new Point(134, 445);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(75, 23);
            disconnectButton.TabIndex = 3;
            disconnectButton.Text = "Disconnect";
            disconnectButton.UseVisualStyleBackColor = true;
            disconnectButton.Click += disconnectButton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(71, 83, 92);
            label4.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label4.ForeColor = SystemColors.ControlLight;
            label4.Location = new Point(2, 2);
            label4.Name = "label4";
            label4.Size = new Size(163, 19);
            label4.TabIndex = 30;
            label4.Text = "Enabled Commands";
            // 
            // econoPanel
            // 
            econoPanel.BackColor = Color.FromArgb(156, 155, 151);
            econoPanel.Controls.Add(economyLastLabel);
            econoPanel.Controls.Add(label7);
            econoPanel.Controls.Add(pictureBox7);
            econoPanel.Controls.Add(label6);
            econoPanel.Controls.Add(pictureBox6);
            econoPanel.Controls.Add(economySpentLabel);
            econoPanel.Controls.Add(label5);
            econoPanel.Controls.Add(economyLabel);
            econoPanel.Controls.Add(pictureBox1);
            econoPanel.Location = new Point(657, 228);
            econoPanel.Name = "econoPanel";
            econoPanel.Size = new Size(163, 148);
            econoPanel.TabIndex = 40;
            // 
            // economyLastLabel
            // 
            economyLastLabel.Anchor = AnchorStyles.None;
            economyLastLabel.AutoSize = true;
            economyLastLabel.Font = new Font("Segoe UI", 11F);
            economyLastLabel.ForeColor = SystemColors.ControlText;
            economyLastLabel.Location = new Point(5, 125);
            economyLastLabel.Name = "economyLastLabel";
            economyLastLabel.Size = new Size(17, 20);
            economyLastLabel.TabIndex = 131;
            economyLastLabel.Text = "0";
            economyLastLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(71, 83, 92);
            label7.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label7.ForeColor = SystemColors.ControlLight;
            label7.Location = new Point(3, 100);
            label7.Name = "label7";
            label7.Size = new Size(147, 19);
            label7.TabIndex = 129;
            label7.Text = "Spent Last Session";
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox7.BackgroundImageLayout = ImageLayout.None;
            pictureBox7.Location = new Point(0, 98);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(164, 24);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 130;
            pictureBox7.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(71, 83, 92);
            label6.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label6.ForeColor = SystemColors.ControlLight;
            label6.Location = new Point(2, 51);
            label6.Name = "label6";
            label6.Size = new Size(148, 19);
            label6.TabIndex = 127;
            label6.Text = "Spent This Session";
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox6.BackgroundImageLayout = ImageLayout.None;
            pictureBox6.Location = new Point(-1, 49);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(164, 24);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 128;
            pictureBox6.TabStop = false;
            // 
            // economySpentLabel
            // 
            economySpentLabel.Anchor = AnchorStyles.None;
            economySpentLabel.AutoSize = true;
            economySpentLabel.Font = new Font("Segoe UI", 11F);
            economySpentLabel.ForeColor = SystemColors.ControlText;
            economySpentLabel.Location = new Point(5, 76);
            economySpentLabel.Name = "economySpentLabel";
            economySpentLabel.Size = new Size(17, 20);
            economySpentLabel.TabIndex = 126;
            economySpentLabel.Text = "0";
            economySpentLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(71, 83, 92);
            label5.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label5.ForeColor = SystemColors.ControlLight;
            label5.Location = new Point(2, 2);
            label5.Name = "label5";
            label5.Size = new Size(151, 19);
            label5.TabIndex = 124;
            label5.Text = "Bits in Circulation ";
            // 
            // economyLabel
            // 
            economyLabel.Anchor = AnchorStyles.None;
            economyLabel.AutoSize = true;
            economyLabel.Font = new Font("Segoe UI", 11F);
            economyLabel.ForeColor = SystemColors.ControlText;
            economyLabel.Location = new Point(5, 27);
            economyLabel.Name = "economyLabel";
            economyLabel.Size = new Size(17, 20);
            economyLabel.TabIndex = 34;
            economyLabel.Text = "0";
            economyLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Location = new Point(-1, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(164, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 125;
            pictureBox1.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox4.BackgroundImageLayout = ImageLayout.None;
            pictureBox4.Location = new Point(-1, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(211, 24);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 32;
            pictureBox4.TabStop = false;
            // 
            // customCommandBox
            // 
            customCommandBox.BackColor = Color.FromArgb(156, 155, 151);
            customCommandBox.BorderStyle = BorderStyle.None;
            customCommandBox.Font = new Font("Segoe UI", 10F);
            customCommandBox.ForeColor = SystemColors.ControlText;
            customCommandBox.Location = new Point(0, 24);
            customCommandBox.Name = "customCommandBox";
            customCommandBox.ReadOnly = true;
            customCommandBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            customCommandBox.Size = new Size(210, 357);
            customCommandBox.TabIndex = 30;
            customCommandBox.Text = "";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(156, 155, 151);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(economyCheckBox);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(backupButton);
            panel2.Controls.Add(twitchOpen);
            panel2.Controls.Add(pauseCommands);
            panel2.Controls.Add(pictureBox8);
            panel2.Location = new Point(657, 30);
            panel2.Name = "panel2";
            panel2.Size = new Size(163, 192);
            panel2.TabIndex = 33;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = Color.Red;
            label12.Location = new Point(79, 70);
            label12.Name = "label12";
            label12.Size = new Size(57, 15);
            label12.TabIndex = 140;
            label12.Text = "In Testing";
            // 
            // economyCheckBox
            // 
            economyCheckBox.AutoSize = true;
            economyCheckBox.BackColor = Color.FromArgb(156, 155, 151);
            economyCheckBox.Checked = true;
            economyCheckBox.CheckState = CheckState.Checked;
            economyCheckBox.ForeColor = SystemColors.ControlText;
            economyCheckBox.Location = new Point(5, 54);
            economyCheckBox.Name = "economyCheckBox";
            economyCheckBox.Size = new Size(131, 34);
            economyCheckBox.TabIndex = 123;
            economyCheckBox.Text = "Enable Bit Economy\r\nTracking";
            economyCheckBox.UseVisualStyleBackColor = false;
            economyCheckBox.CheckedChanged += economyCheckBox_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(71, 83, 92);
            label1.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label1.ForeColor = SystemColors.ControlLight;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(103, 19);
            label1.TabIndex = 12;
            label1.Text = "Quick Menu";
            // 
            // backupButton
            // 
            backupButton.Location = new Point(33, 105);
            backupButton.Name = "backupButton";
            backupButton.Size = new Size(103, 23);
            backupButton.TabIndex = 29;
            backupButton.Text = "Start Backup";
            backupButton.UseVisualStyleBackColor = true;
            backupButton.Click += backupButton_Click;
            // 
            // twitchOpen
            // 
            twitchOpen.Location = new Point(33, 134);
            twitchOpen.Name = "twitchOpen";
            twitchOpen.Size = new Size(103, 23);
            twitchOpen.TabIndex = 28;
            twitchOpen.Text = "Open Twitch";
            twitchOpen.UseVisualStyleBackColor = true;
            twitchOpen.Click += twitchOpen_Click;
            // 
            // pauseCommands
            // 
            pauseCommands.AutoSize = true;
            pauseCommands.BackColor = Color.FromArgb(156, 155, 151);
            pauseCommands.ForeColor = SystemColors.ControlText;
            pauseCommands.Location = new Point(5, 29);
            pauseCommands.Name = "pauseCommands";
            pauseCommands.Size = new Size(146, 19);
            pauseCommands.TabIndex = 27;
            pauseCommands.Text = "Pause chat commands";
            pauseCommands.UseVisualStyleBackColor = false;
            pauseCommands.CheckedChanged += pauseCommands_CheckedChanged;
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(0, 0);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(164, 24);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 30;
            pictureBox8.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Red;
            label2.Location = new Point(215, 445);
            label2.Name = "label2";
            label2.Size = new Size(514, 21);
            label2.TabIndex = 13;
            label2.Text = "Remember this is unfinished software. Things will be broken or changed!";
            // 
            // consoleTab
            // 
            consoleTab.AutoSize = true;
            consoleTab.BackColor = Color.FromArgb(120, 132, 142);
            consoleTab.Font = new Font("Constantia", 14F, FontStyle.Bold);
            consoleTab.ForeColor = SystemColors.ControlLight;
            consoleTab.Location = new Point(53, 31);
            consoleTab.Name = "consoleTab";
            consoleTab.Size = new Size(82, 23);
            consoleTab.TabIndex = 29;
            consoleTab.Text = "Console";
            consoleTab.TextAlign = ContentAlignment.MiddleCenter;
            consoleTab.Click += consoleTab_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox2.Location = new Point(53, 30);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(598, 24);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 29;
            pictureBox2.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.Red;
            pictureBox5.Location = new Point(0, 27);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(47, 47);
            pictureBox5.TabIndex = 30;
            pictureBox5.TabStop = false;
            // 
            // manramLabel
            // 
            manramLabel.AutoSize = true;
            manramLabel.ForeColor = Color.Salmon;
            manramLabel.Location = new Point(6, 19);
            manramLabel.Name = "manramLabel";
            manramLabel.Size = new Size(108, 15);
            manramLabel.TabIndex = 35;
            manramLabel.Text = "Managed Memory:";
            // 
            // allocramLabel
            // 
            allocramLabel.AutoSize = true;
            allocramLabel.ForeColor = Color.Salmon;
            allocramLabel.Location = new Point(6, 49);
            allocramLabel.Name = "allocramLabel";
            allocramLabel.Size = new Size(136, 15);
            allocramLabel.TabIndex = 36;
            allocramLabel.Text = "Total Allocated Memory:";
            // 
            // ramLabel
            // 
            ramLabel.AutoSize = true;
            ramLabel.ForeColor = Color.Salmon;
            ramLabel.Location = new Point(6, 34);
            ramLabel.Name = "ramLabel";
            ramLabel.Size = new Size(106, 15);
            ramLabel.TabIndex = 37;
            ramLabel.Text = "Actual Ram Usage:";
            // 
            // debugGroup
            // 
            debugGroup.Controls.Add(ramSnapButton);
            debugGroup.Controls.Add(manramLabel);
            debugGroup.Controls.Add(ramLabel);
            debugGroup.Controls.Add(allocramLabel);
            debugGroup.ForeColor = Color.Salmon;
            debugGroup.Location = new Point(836, 503);
            debugGroup.Name = "debugGroup";
            debugGroup.Size = new Size(200, 100);
            debugGroup.TabIndex = 38;
            debugGroup.TabStop = false;
            debugGroup.Text = "Memory Debug";
            // 
            // ramSnapButton
            // 
            ramSnapButton.ForeColor = SystemColors.ControlText;
            ramSnapButton.Location = new Point(6, 69);
            ramSnapButton.Name = "ramSnapButton";
            ramSnapButton.Size = new Size(188, 23);
            ramSnapButton.TabIndex = 39;
            ramSnapButton.Text = "Snapshot ram use to file";
            ramSnapButton.UseVisualStyleBackColor = true;
            ramSnapButton.Click += ramSnapButton_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.ForeColor = SystemColors.ControlText;
            label20.Location = new Point(348, 93);
            label20.Name = "label20";
            label20.Size = new Size(359, 15);
            label20.TabIndex = 51;
            label20.Text = "!sbremove <commandname> - Remove a command through chat";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.ForeColor = SystemColors.ControlText;
            label19.Location = new Point(348, 78);
            label19.Name = "label19";
            label19.Size = new Size(384, 15);
            label19.TabIndex = 50;
            label19.Text = "!sbadd <commandname> <methods> - Add a command through chat";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.ForeColor = SystemColors.ControlText;
            label18.Location = new Point(348, 63);
            label18.Name = "label18";
            label18.Size = new Size(292, 15);
            label18.TabIndex = 49;
            label18.Text = "!refund @user – Refund the last command a user used";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = SystemColors.ControlText;
            label17.Location = new Point(348, 33);
            label17.Name = "label17";
            label17.Size = new Size(250, 15);
            label17.TabIndex = 48;
            label17.Text = "!addbits @user <amount> – Add bits to a user";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = SystemColors.ControlText;
            label16.Location = new Point(348, 18);
            label16.Name = "label16";
            label16.Size = new Size(341, 15);
            label16.TabIndex = 47;
            label16.Text = "!sendkey <key>  - Send any select key input to the streamers pc";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = SystemColors.ControlText;
            label15.Location = new Point(348, 3);
            label15.Name = "label15";
            label15.Size = new Size(206, 15);
            label15.TabIndex = 46;
            label15.Text = "!sweatbot - Turn Sweat Bot on and off";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = SystemColors.ControlText;
            label14.Location = new Point(3, 78);
            label14.Name = "label14";
            label14.Size = new Size(342, 15);
            label14.TabIndex = 45;
            label14.Text = "!sbgamble <amount>  - Gamble your bits in hopes to win more";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = SystemColors.ControlText;
            label13.Location = new Point(3, 63);
            label13.Name = "label13";
            label13.Size = new Size(263, 15);
            label13.TabIndex = 44;
            label13.Text = "!bitcost – List available commands and their cost";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = SystemColors.ControlText;
            label11.Location = new Point(3, 48);
            label11.Name = "label11";
            label11.Size = new Size(263, 15);
            label11.TabIndex = 43;
            label11.Text = "!mybits – Check how many bits a user has stored";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = SystemColors.ControlText;
            label10.Location = new Point(3, 33);
            label10.Name = "label10";
            label10.Size = new Size(191, 15);
            label10.TabIndex = 42;
            label10.Text = "!about – Information about the bot";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = SystemColors.ControlText;
            label9.Location = new Point(3, 18);
            label9.Name = "label9";
            label9.Size = new Size(250, 15);
            label9.TabIndex = 41;
            label9.Text = "!how2use – Instructions on how to use the bot";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.ControlText;
            label8.Location = new Point(3, 3);
            label8.Name = "label8";
            label8.Size = new Size(175, 15);
            label8.TabIndex = 40;
            label8.Text = "!help – List available commands";
            // 
            // chatComPanel
            // 
            chatComPanel.AutoScroll = true;
            chatComPanel.BackColor = Color.FromArgb(156, 155, 151);
            chatComPanel.Controls.Add(label22);
            chatComPanel.Controls.Add(label20);
            chatComPanel.Controls.Add(label19);
            chatComPanel.Controls.Add(label18);
            chatComPanel.Controls.Add(label8);
            chatComPanel.Controls.Add(label17);
            chatComPanel.Controls.Add(label9);
            chatComPanel.Controls.Add(label16);
            chatComPanel.Controls.Add(label10);
            chatComPanel.Controls.Add(label15);
            chatComPanel.Controls.Add(label11);
            chatComPanel.Controls.Add(label14);
            chatComPanel.Controls.Add(label13);
            chatComPanel.Location = new Point(53, 507);
            chatComPanel.Name = "chatComPanel";
            chatComPanel.Size = new Size(749, 96);
            chatComPanel.TabIndex = 141;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.ForeColor = SystemColors.ControlText;
            label22.Location = new Point(348, 48);
            label22.Name = "label22";
            label22.Size = new Size(287, 15);
            label22.TabIndex = 52;
            label22.Text = "!rembits @user <amount> – Remove bits from a user";
            // 
            // pictureBox9
            // 
            pictureBox9.BackColor = Color.FromArgb(37, 37, 37);
            pictureBox9.BackgroundImageLayout = ImageLayout.None;
            pictureBox9.Location = new Point(53, 485);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new Size(749, 22);
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.TabIndex = 30;
            pictureBox9.TabStop = false;
            // 
            // regComLabel
            // 
            regComLabel.AutoSize = true;
            regComLabel.BackColor = Color.FromArgb(37, 37, 37);
            regComLabel.Font = new Font("Constantia", 12F, FontStyle.Bold);
            regComLabel.ForeColor = SystemColors.ControlLight;
            regComLabel.Location = new Point(56, 485);
            regComLabel.Name = "regComLabel";
            regComLabel.Size = new Size(332, 19);
            regComLabel.TabIndex = 12;
            regComLabel.Text = "Regular Chat Commands (click to expand)";
            // 
            // panel1
            // 
            panel1.Controls.Add(label4);
            panel1.Controls.Add(customCommandBox);
            panel1.Controls.Add(pictureBox4);
            panel1.Location = new Point(827, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(210, 383);
            panel1.TabIndex = 142;
            // 
            // logListPanel
            // 
            logListPanel.AutoScroll = true;
            logListPanel.Location = new Point(0, 3);
            logListPanel.Name = "logListPanel";
            logListPanel.Size = new Size(597, 353);
            logListPanel.TabIndex = 31;
            // 
            // refundPanel
            // 
            refundPanel.BackColor = Color.FromArgb(156, 155, 151);
            refundPanel.Controls.Add(logListPanel);
            refundPanel.Location = new Point(53, 618);
            refundPanel.Name = "refundPanel";
            refundPanel.Size = new Size(598, 359);
            refundPanel.TabIndex = 142;
            // 
            // refundTab
            // 
            refundTab.AutoSize = true;
            refundTab.BackColor = Color.FromArgb(71, 83, 92);
            refundTab.Font = new Font("Constantia", 14F, FontStyle.Bold);
            refundTab.ForeColor = SystemColors.ControlLight;
            refundTab.Location = new Point(135, 31);
            refundTab.Name = "refundTab";
            refundTab.Size = new Size(85, 23);
            refundTab.TabIndex = 143;
            refundTab.Text = "Refunds";
            refundTab.TextAlign = ContentAlignment.MiddleCenter;
            refundTab.Click += refundTab_Click;
            // 
            // ConnectMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(37, 37, 37);
            ClientSize = new Size(1058, 1211);
            ControlBox = false;
            Controls.Add(consoleTab);
            Controls.Add(refundTab);
            Controls.Add(pictureBox2);
            Controls.Add(refundPanel);
            Controls.Add(panel1);
            Controls.Add(econoPanel);
            Controls.Add(panel2);
            Controls.Add(debugGroup);
            Controls.Add(pictureBox5);
            Controls.Add(label2);
            Controls.Add(disconnectButton);
            Controls.Add(connectButton);
            Controls.Add(messageTextBox);
            Controls.Add(consoleTextBox);
            Controls.Add(chatComPanel);
            Controls.Add(regComLabel);
            Controls.Add(pictureBox9);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ConnectMenu";
            Text = "ConnectMenu";
            Load += ConnectMenu_Load;
            econoPanel.ResumeLayout(false);
            econoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            debugGroup.ResumeLayout(false);
            debugGroup.PerformLayout();
            chatComPanel.ResumeLayout(false);
            chatComPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            refundPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox messageTextBox;
        private TextBox consoleTextBox;
        private Button connectButton;
        private Button disconnectButton;
        private Label label1;
        private CheckBox pauseCommands;
        private Button twitchOpen;
        private Label label2;
        private Label consoleTab;
        private PictureBox pictureBox2;
        private Button backupButton;
        private RichTextBox customCommandBox;
        private Label label4;
        private PictureBox pictureBox4;
        private PictureBox pictureBox8;
        private PictureBox pictureBox5;
        private PictureBox pictureBox10;
        private Panel panel2;
        private Label economyLabel;
        private CheckBox economyCheckBox;
        private Label manramLabel;
        private Label allocramLabel;
        private Label ramLabel;
        private GroupBox debugGroup;
        private Button ramSnapButton;
        private Panel econoPanel;
        private Label label5;
        private PictureBox pictureBox1;
        private Label economySpentLabel;
        private Label label6;
        private PictureBox pictureBox6;
        private Label economyLastLabel;
        private Label label7;
        private PictureBox pictureBox7;
        private Label label12;
        private GroupBox regGroupBox;
        private Label label18;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label20;
        private Label label19;
        private Label regComLabel;
        private PictureBox pictureBox9;
        public Panel chatComPanel;
        private Panel panel1;
        private FlowLayoutPanel logListPanel;
        public Panel refundPanel;
        private Label label22;
        private Label refundTab;
    }
}