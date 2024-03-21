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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectMenu));
            messageTextBox = new TextBox();
            consoleTextBox = new TextBox();
            connectButton = new Button();
            disconnectButton = new Button();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            twitchOpen = new Button();
            pauseCommands = new CheckBox();
            stopGoose = new Button();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // messageTextBox
            // 
            messageTextBox.Location = new Point(63, 334);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.Size = new Size(574, 23);
            messageTextBox.TabIndex = 0;
            // 
            // consoleTextBox
            // 
            consoleTextBox.BackColor = Color.FromArgb(156, 154, 151);
            consoleTextBox.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
            consoleTextBox.Location = new Point(63, 28);
            consoleTextBox.Multiline = true;
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.ReadOnly = true;
            consoleTextBox.ScrollBars = ScrollBars.Horizontal;
            consoleTextBox.Size = new Size(574, 300);
            consoleTextBox.TabIndex = 1;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(63, 363);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(75, 23);
            connectButton.TabIndex = 2;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // disconnectButton
            // 
            disconnectButton.Location = new Point(144, 363);
            disconnectButton.Name = "disconnectButton";
            disconnectButton.Size = new Size(75, 23);
            disconnectButton.TabIndex = 3;
            disconnectButton.Text = "Disconnect";
            disconnectButton.UseVisualStyleBackColor = true;
            disconnectButton.Click += disconnectButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(274, 326);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(twitchOpen);
            panel1.Controls.Add(pauseCommands);
            panel1.Controls.Add(stopGoose);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(643, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(301, 329);
            panel1.TabIndex = 12;
            // 
            // twitchOpen
            // 
            twitchOpen.Location = new Point(161, 73);
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
            pauseCommands.Location = new Point(5, 44);
            pauseCommands.Name = "pauseCommands";
            pauseCommands.Size = new Size(146, 19);
            pauseCommands.TabIndex = 27;
            pauseCommands.Text = "Pause chat commands";
            pauseCommands.UseVisualStyleBackColor = true;
            pauseCommands.CheckedChanged += pauseCommands_CheckedChanged;
            // 
            // stopGoose
            // 
            stopGoose.Location = new Point(161, 44);
            stopGoose.Name = "stopGoose";
            stopGoose.Size = new Size(103, 23);
            stopGoose.TabIndex = 13;
            stopGoose.Text = "Kill Goose";
            stopGoose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(162, 123, 92);
            label1.Font = new Font("Constantia", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(5, 8);
            label1.Name = "label1";
            label1.Size = new Size(175, 23);
            label1.TabIndex = 12;
            label1.Text = "Command Toggles";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Red;
            label2.Location = new Point(344, 390);
            label2.Name = "label2";
            label2.Size = new Size(326, 15);
            label2.TabIndex = 13;
            label2.Text = "Remember this is unfinished software. Things will be broken!";
            // 
            // ConnectMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1058, 487);
            ControlBox = false;
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(disconnectButton);
            Controls.Add(connectButton);
            Controls.Add(consoleTextBox);
            Controls.Add(messageTextBox);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ConnectMenu";
            Text = "ConnectMenu";
            Load += ConnectMenu_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
    }
}