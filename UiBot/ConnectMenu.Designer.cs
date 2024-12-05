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
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            label4 = new Label();
            pictureBox4 = new PictureBox();
            label1 = new Label();
            pictureBox8 = new PictureBox();
            pauseCommands = new CheckBox();
            stopGoose = new Button();
            twitchOpen = new Button();
            backupButton = new Button();
            customCommandBox = new RichTextBox();
            pictureBox3 = new PictureBox();
            label2 = new Label();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            pictureBox5 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // messageTextBox
            // 
            messageTextBox.Location = new Point(53, 369);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.Size = new Size(574, 23);
            messageTextBox.TabIndex = 0;
            // 
            // consoleTextBox
            // 
            consoleTextBox.BackColor = Color.FromArgb(156, 154, 151);
            consoleTextBox.Font = new Font("Arial", 12F);
            consoleTextBox.Location = new Point(53, 63);
            consoleTextBox.Multiline = true;
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.ReadOnly = true;
            consoleTextBox.ScrollBars = ScrollBars.Vertical;
            consoleTextBox.Size = new Size(574, 300);
            consoleTextBox.TabIndex = 1;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(53, 398);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(75, 23);
            connectButton.TabIndex = 2;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // disconnectButton
            // 
            disconnectButton.Location = new Point(134, 398);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(75, 23);
            disconnectButton.TabIndex = 3;
            disconnectButton.Text = "Disconnect";
            disconnectButton.UseVisualStyleBackColor = true;
            disconnectButton.Click += disconnectButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(220, 215, 201);
            pictureBox1.Location = new Point(1, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(163, 326);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(label4);
            panel1.Controls.Add(pictureBox4);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox8);
            panel1.Controls.Add(pauseCommands);
            panel1.Controls.Add(stopGoose);
            panel1.Controls.Add(twitchOpen);
            panel1.Controls.Add(backupButton);
            panel1.Controls.Add(customCommandBox);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(pictureBox3);
            panel1.Location = new Point(633, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(403, 339);
            panel1.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(162, 123, 92);
            label4.Font = new Font("Constantia", 14.25F, FontStyle.Bold);
            label4.Location = new Point(170, 3);
            label4.Name = "label4";
            label4.Size = new Size(191, 23);
            label4.TabIndex = 30;
            label4.Text = "Enabled Commands";
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox4.BackgroundImageLayout = ImageLayout.None;
            pictureBox4.Location = new Point(170, 3);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(210, 24);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 32;
            pictureBox4.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(162, 123, 92);
            label1.Font = new Font("Constantia", 14.25F, FontStyle.Bold);
            label1.Location = new Point(3, 3);
            label1.Name = "label1";
            label1.Size = new Size(136, 23);
            label1.TabIndex = 12;
            label1.Text = "Quick Toggles";
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(1, 2);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(163, 24);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 30;
            pictureBox8.TabStop = false;
            // 
            // pauseCommands
            // 
            pauseCommands.AutoSize = true;
            pauseCommands.BackColor = Color.FromArgb(220, 215, 201);
            pauseCommands.Location = new Point(5, 34);
            pauseCommands.Name = "pauseCommands";
            pauseCommands.Size = new Size(146, 19);
            pauseCommands.TabIndex = 27;
            pauseCommands.Text = "Pause chat commands";
            pauseCommands.UseVisualStyleBackColor = false;
            pauseCommands.CheckedChanged += pauseCommands_CheckedChanged;
            // 
            // stopGoose
            // 
            stopGoose.BackgroundImageLayout = ImageLayout.None;
            stopGoose.Location = new Point(31, 234);
            stopGoose.Name = "stopGoose";
            stopGoose.Size = new Size(103, 23);
            stopGoose.TabIndex = 13;
            stopGoose.Text = "Kill Goose";
            stopGoose.UseVisualStyleBackColor = true;
            stopGoose.Click += stopGoose_Click;
            // 
            // twitchOpen
            // 
            twitchOpen.Location = new Point(31, 263);
            twitchOpen.Name = "twitchOpen";
            twitchOpen.Size = new Size(103, 23);
            twitchOpen.TabIndex = 28;
            twitchOpen.Text = "Open Twitch";
            twitchOpen.UseVisualStyleBackColor = true;
            twitchOpen.Click += twitchOpen_Click;
            // 
            // backupButton
            // 
            backupButton.Location = new Point(31, 292);
            backupButton.Name = "backupButton";
            backupButton.Size = new Size(103, 23);
            backupButton.TabIndex = 29;
            backupButton.Text = "Start Backup";
            backupButton.UseVisualStyleBackColor = true;
            backupButton.Click += backupButton_Click;
            // 
            // customCommandBox
            // 
            customCommandBox.BackColor = Color.FromArgb(220, 215, 201);
            customCommandBox.BorderStyle = BorderStyle.None;
            customCommandBox.Location = new Point(174, 29);
            customCommandBox.Name = "customCommandBox";
            customCommandBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            customCommandBox.Size = new Size(205, 297);
            customCommandBox.TabIndex = 30;
            customCommandBox.Text = "";
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(220, 215, 201);
            pictureBox3.Location = new Point(170, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(210, 325);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 31;
            pictureBox3.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Red;
            label2.Location = new Point(292, 409);
            label2.Name = "label2";
            label2.Size = new Size(326, 15);
            label2.TabIndex = 13;
            label2.Text = "Remember this is unfinished software. Things will be broken!";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(162, 123, 92);
            label3.Font = new Font("Constantia", 14.25F, FontStyle.Bold);
            label3.Location = new Point(60, 40);
            label3.Name = "label3";
            label3.Size = new Size(156, 23);
            label3.TabIndex = 29;
            label3.Text = "Output Window";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox2.Location = new Point(53, 35);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(574, 37);
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
            // ConnectMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1058, 596);
            ControlBox = false;
            Controls.Add(pictureBox5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(disconnectButton);
            Controls.Add(connectButton);
            Controls.Add(consoleTextBox);
            Controls.Add(messageTextBox);
            Controls.Add(pictureBox2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ConnectMenu";
            Text = "ConnectMenu";
            Load += ConnectMenu_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox messageTextBox;
        private TextBox consoleTextBox;
        private Button connectButton;
        private Button disconnectButton;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Label label1;
        private Button stopGoose;
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
    }
}