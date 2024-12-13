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
            panel1 = new Panel();
            label4 = new Label();
            pictureBox4 = new PictureBox();
            customCommandBox = new RichTextBox();
            pictureBox3 = new PictureBox();
            label1 = new Label();
            pictureBox8 = new PictureBox();
            pauseCommands = new CheckBox();
            twitchOpen = new Button();
            backupButton = new Button();
            label2 = new Label();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            pictureBox5 = new PictureBox();
            panel2 = new Panel();
            economyCheckBox = new CheckBox();
            economyLabel = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // messageTextBox
            // 
            messageTextBox.BackColor = SystemColors.ControlLight;
            messageTextBox.BorderStyle = BorderStyle.None;
            messageTextBox.Location = new Point(53, 427);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.PlaceholderText = "Enter command to type to chat here. Press enter to send";
            messageTextBox.Size = new Size(574, 16);
            messageTextBox.TabIndex = 0;
            // 
            // consoleTextBox
            // 
            consoleTextBox.BackColor = Color.FromArgb(156, 155, 151);
            consoleTextBox.BorderStyle = BorderStyle.None;
            consoleTextBox.Font = new Font("Arial", 12F);
            consoleTextBox.Location = new Point(53, 62);
            consoleTextBox.Multiline = true;
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.ReadOnly = true;
            consoleTextBox.ScrollBars = ScrollBars.Vertical;
            consoleTextBox.Size = new Size(574, 359);
            consoleTextBox.TabIndex = 1;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(53, 456);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(75, 23);
            connectButton.TabIndex = 2;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // disconnectButton
            // 
            disconnectButton.Location = new Point(134, 456);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(75, 23);
            disconnectButton.TabIndex = 3;
            disconnectButton.Text = "Disconnect";
            disconnectButton.UseVisualStyleBackColor = true;
            disconnectButton.Click += disconnectButton_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label4);
            panel1.Controls.Add(pictureBox4);
            panel1.Controls.Add(customCommandBox);
            panel1.Controls.Add(pictureBox3);
            panel1.Location = new Point(633, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(403, 389);
            panel1.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(71, 83, 92);
            label4.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label4.ForeColor = SystemColors.ControlLight;
            label4.Location = new Point(173, 5);
            label4.Name = "label4";
            label4.Size = new Size(163, 19);
            label4.TabIndex = 30;
            label4.Text = "Enabled Commands";
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox4.BackgroundImageLayout = ImageLayout.None;
            pictureBox4.Location = new Point(170, 3);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(210, 24);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 32;
            pictureBox4.TabStop = false;
            // 
            // customCommandBox
            // 
            customCommandBox.BackColor = Color.FromArgb(156, 155, 151);
            customCommandBox.BorderStyle = BorderStyle.None;
            customCommandBox.ForeColor = SystemColors.ControlText;
            customCommandBox.Location = new Point(170, 27);
            customCommandBox.Name = "customCommandBox";
            customCommandBox.ReadOnly = true;
            customCommandBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            customCommandBox.Size = new Size(210, 357);
            customCommandBox.TabIndex = 30;
            customCommandBox.Text = "";
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(70, 70, 70);
            pictureBox3.Location = new Point(170, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(210, 383);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 31;
            pictureBox3.TabStop = false;
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
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(0, -2);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(164, 24);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 30;
            pictureBox8.TabStop = false;
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
            // twitchOpen
            // 
            twitchOpen.Location = new Point(33, 110);
            twitchOpen.Name = "twitchOpen";
            twitchOpen.Size = new Size(103, 23);
            twitchOpen.TabIndex = 28;
            twitchOpen.Text = "Open Twitch";
            twitchOpen.UseVisualStyleBackColor = true;
            twitchOpen.Click += twitchOpen_Click;
            // 
            // backupButton
            // 
            backupButton.Location = new Point(33, 139);
            backupButton.Name = "backupButton";
            backupButton.Size = new Size(103, 23);
            backupButton.TabIndex = 29;
            backupButton.Text = "Start Backup";
            backupButton.UseVisualStyleBackColor = true;
            backupButton.Click += backupButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Salmon;
            label2.Location = new Point(300, 455);
            label2.Name = "label2";
            label2.Size = new Size(432, 21);
            label2.TabIndex = 13;
            label2.Text = "Remember this is unfinished software. Things will be broken!";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(71, 83, 92);
            label3.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ControlLight;
            label3.Location = new Point(56, 40);
            label3.Name = "label3";
            label3.Size = new Size(71, 19);
            label3.TabIndex = 29;
            label3.Text = "Console";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox2.Location = new Point(53, 37);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(574, 25);
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
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(156, 155, 151);
            panel2.Controls.Add(economyCheckBox);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(backupButton);
            panel2.Controls.Add(twitchOpen);
            panel2.Controls.Add(pauseCommands);
            panel2.Controls.Add(pictureBox8);
            panel2.Location = new Point(633, 38);
            panel2.Name = "panel2";
            panel2.Size = new Size(163, 184);
            panel2.TabIndex = 33;
            // 
            // economyCheckBox
            // 
            economyCheckBox.AutoSize = true;
            economyCheckBox.BackColor = Color.FromArgb(156, 155, 151);
            economyCheckBox.ForeColor = SystemColors.ControlText;
            economyCheckBox.Location = new Point(5, 54);
            economyCheckBox.Name = "economyCheckBox";
            economyCheckBox.Size = new Size(131, 34);
            economyCheckBox.TabIndex = 123;
            economyCheckBox.Text = "Enable Bit Economy\r\nTracking";
            economyCheckBox.UseVisualStyleBackColor = false;
            economyCheckBox.CheckedChanged += economyCheckBox_CheckedChanged;
            // 
            // economyLabel
            // 
            economyLabel.AutoSize = true;
            economyLabel.ForeColor = Color.Salmon;
            economyLabel.Location = new Point(806, 428);
            economyLabel.Name = "economyLabel";
            economyLabel.Size = new Size(88, 15);
            economyLabel.TabIndex = 34;
            economyLabel.Text = "Total Economy:";
            // 
            // ConnectMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(37, 37, 37);
            ClientSize = new Size(1058, 596);
            ControlBox = false;
            Controls.Add(economyLabel);
            Controls.Add(panel2);
            Controls.Add(pictureBox5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(disconnectButton);
            Controls.Add(connectButton);
            Controls.Add(messageTextBox);
            Controls.Add(pictureBox2);
            Controls.Add(consoleTextBox);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ConnectMenu";
            Text = "ConnectMenu";
            Load += ConnectMenu_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox messageTextBox;
        private TextBox consoleTextBox;
        private Button connectButton;
        private Button disconnectButton;
        private Panel panel1;
        private Label label1;
        private CheckBox pauseCommands;
        private Button twitchOpen;
        private Label label2;
        private Label label3;
        private PictureBox pictureBox2;
        private Button backupButton;
        private RichTextBox customCommandBox;
        private Label label4;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox8;
        private PictureBox pictureBox5;
        private PictureBox pictureBox10;
        private Panel panel2;
        private Label economyLabel;
        private CheckBox economyCheckBox;
    }
}