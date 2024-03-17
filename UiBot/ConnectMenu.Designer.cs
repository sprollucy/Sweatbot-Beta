﻿namespace UiBot
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
            enableBagDrop = new CheckBox();
            enableGrenade = new CheckBox();
            toggleAll = new Button();
            oneClickCheck = new CheckBox();
            stopGoose = new Button();
            randomTurn = new CheckBox();
            label1 = new Label();
            enableRandomKey = new CheckBox();
            chkEnableGoose = new CheckBox();
            enableKitDrop = new CheckBox();
            enableWiggle = new CheckBox();
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
            messageTextBox.TextChanged += messageTextBox_TextChanged;
            // 
            // consoleTextBox
            // 
            consoleTextBox.BackColor = Color.FromArgb(181, 176, 163);
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
            pictureBox1.Size = new Size(254, 326);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(enableBagDrop);
            panel1.Controls.Add(enableGrenade);
            panel1.Controls.Add(toggleAll);
            panel1.Controls.Add(oneClickCheck);
            panel1.Controls.Add(stopGoose);
            panel1.Controls.Add(randomTurn);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(enableRandomKey);
            panel1.Controls.Add(chkEnableGoose);
            panel1.Controls.Add(enableKitDrop);
            panel1.Controls.Add(enableWiggle);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(643, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(301, 329);
            panel1.TabIndex = 12;
            // 
            // enableBagDrop
            // 
            enableBagDrop.AutoSize = true;
            enableBagDrop.BackColor = Color.FromArgb(181, 176, 163);
            enableBagDrop.Location = new Point(9, 249);
            enableBagDrop.Name = "enableBagDrop";
            enableBagDrop.Size = new Size(113, 19);
            enableBagDrop.TabIndex = 28;
            enableBagDrop.Text = "Enable Bag Drop";
            enableBagDrop.UseVisualStyleBackColor = false;
            enableBagDrop.CheckedChanged += enableBagDrop_CheckedChanged;
            // 
            // enableGrenade
            // 
            enableGrenade.AutoSize = true;
            enableGrenade.BackColor = Color.FromArgb(181, 176, 163);
            enableGrenade.Location = new Point(9, 224);
            enableGrenade.Name = "enableGrenade";
            enableGrenade.Size = new Size(108, 19);
            enableGrenade.TabIndex = 27;
            enableGrenade.Text = "Enable Grenade";
            enableGrenade.UseVisualStyleBackColor = false;
            enableGrenade.CheckedChanged += enableGrenade_CheckedChanged;
            // 
            // toggleAll
            // 
            toggleAll.Location = new Point(133, 54);
            toggleAll.Name = "toggleAll";
            toggleAll.Size = new Size(103, 23);
            toggleAll.TabIndex = 26;
            toggleAll.Text = "Toggle All Off";
            toggleAll.UseVisualStyleBackColor = true;
            toggleAll.Click += toggleAll_Click;
            // 
            // oneClickCheck
            // 
            oneClickCheck.AutoSize = true;
            oneClickCheck.BackColor = Color.FromArgb(181, 176, 163);
            oneClickCheck.Location = new Point(9, 199);
            oneClickCheck.Name = "oneClickCheck";
            oneClickCheck.Size = new Size(129, 19);
            oneClickCheck.TabIndex = 20;
            oneClickCheck.Text = "One Left Click(pop)";
            oneClickCheck.UseVisualStyleBackColor = false;
            oneClickCheck.CheckedChanged += oneClickCheck_CheckedChanged;
            // 
            // stopGoose
            // 
            stopGoose.Location = new Point(133, 83);
            stopGoose.Name = "stopGoose";
            stopGoose.Size = new Size(103, 23);
            stopGoose.TabIndex = 13;
            stopGoose.Text = "Kill Goose";
            stopGoose.UseVisualStyleBackColor = true;
            // 
            // randomTurn
            // 
            randomTurn.AutoSize = true;
            randomTurn.BackColor = Color.FromArgb(181, 176, 163);
            randomTurn.Location = new Point(9, 170);
            randomTurn.Name = "randomTurn";
            randomTurn.Size = new Size(88, 19);
            randomTurn.TabIndex = 21;
            randomTurn.Text = "Enable Turn";
            randomTurn.UseVisualStyleBackColor = false;
            randomTurn.CheckedChanged += randomTurn_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(162, 123, 92);
            label1.Font = new Font("Constantia", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(0, 6);
            label1.Name = "label1";
            label1.Size = new Size(175, 23);
            label1.TabIndex = 12;
            label1.Text = "Command Toggles";
            // 
            // enableRandomKey
            // 
            enableRandomKey.AutoSize = true;
            enableRandomKey.BackColor = Color.FromArgb(181, 176, 163);
            enableRandomKey.Location = new Point(9, 139);
            enableRandomKey.Name = "enableRandomKey";
            enableRandomKey.Size = new Size(131, 19);
            enableRandomKey.TabIndex = 22;
            enableRandomKey.Text = "Enable Random Key";
            enableRandomKey.UseVisualStyleBackColor = false;
            enableRandomKey.CheckedChanged += enableRandomKey_CheckedChanged;
            // 
            // chkEnableGoose
            // 
            chkEnableGoose.AutoSize = true;
            chkEnableGoose.BackColor = Color.FromArgb(181, 176, 163);
            chkEnableGoose.Location = new Point(9, 112);
            chkEnableGoose.Name = "chkEnableGoose";
            chkEnableGoose.Size = new Size(97, 19);
            chkEnableGoose.TabIndex = 23;
            chkEnableGoose.Text = "Enable Goose";
            chkEnableGoose.UseVisualStyleBackColor = false;
            chkEnableGoose.CheckedChanged += chkEnableGoose_CheckedChanged;
            // 
            // enableKitDrop
            // 
            enableKitDrop.AutoSize = true;
            enableKitDrop.BackColor = Color.FromArgb(181, 176, 163);
            enableKitDrop.Location = new Point(9, 83);
            enableKitDrop.Name = "enableKitDrop";
            enableKitDrop.Size = new Size(107, 19);
            enableKitDrop.TabIndex = 24;
            enableKitDrop.Text = "Enable Kit Drop";
            enableKitDrop.UseVisualStyleBackColor = false;
            enableKitDrop.CheckedChanged += enableKitDrop_CheckedChanged;
            // 
            // enableWiggle
            // 
            enableWiggle.AutoSize = true;
            enableWiggle.BackColor = Color.FromArgb(181, 176, 163);
            enableWiggle.Location = new Point(9, 54);
            enableWiggle.Name = "enableWiggle";
            enableWiggle.Size = new Size(101, 19);
            enableWiggle.TabIndex = 25;
            enableWiggle.Text = "Enable Wiggle";
            enableWiggle.UseVisualStyleBackColor = false;
            enableWiggle.CheckedChanged += enableWiggle_CheckedChanged;
            // 
            // ConnectMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1058, 487);
            ControlBox = false;
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
        private CheckBox oneClickCheck;
        private CheckBox randomTurn;
        private CheckBox enableRandomKey;
        private CheckBox chkEnableGoose;
        private CheckBox enableKitDrop;
        private CheckBox enableWiggle;
        private Button toggleAll;
        private CheckBox enableBagDrop;
        private CheckBox enableGrenade;
    }
}