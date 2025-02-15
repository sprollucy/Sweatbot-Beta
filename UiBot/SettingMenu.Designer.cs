﻿namespace UiBot
{
    partial class SettingMenu
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
            groupBox1 = new GroupBox();
            accessBox = new TextBox();
            label1 = new Label();
            button3 = new Button();
            label2 = new Label();
            checkBox1 = new CheckBox();
            label3 = new Label();
            button1 = new Button();
            channelBox2 = new TextBox();
            channelOpen = new Button();
            pictureBox1 = new PictureBox();
            changelogBox = new TextBox();
            label5 = new Label();
            githubLink = new LinkLabel();
            versionNumber = new Label();
            label7 = new Label();
            pictureBox2 = new PictureBox();
            changelogLabel = new Label();
            label4 = new Label();
            linkLabel1 = new LinkLabel();
            bitrestoreButton = new Button();
            groupBox2 = new GroupBox();
            enableUpdateCheck = new CheckBox();
            groupBox3 = new GroupBox();
            checkUpdateButton = new Button();
            pictureBox6 = new PictureBox();
            enableDebug = new CheckBox();
            groupBox4 = new GroupBox();
            enableDebugCommands = new CheckBox();
            cashappLink = new LinkLabel();
            discordLink = new LinkLabel();
            panel5 = new Panel();
            label19 = new Label();
            pictureBox13 = new PictureBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            groupBox4.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(156, 155, 151);
            groupBox1.Controls.Add(accessBox);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(channelBox2);
            groupBox1.Controls.Add(channelOpen);
            groupBox1.Font = new Font("Segoe UI", 9F);
            groupBox1.ForeColor = SystemColors.ControlText;
            groupBox1.Location = new Point(5, 29);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(417, 140);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Login Info";
            // 
            // accessBox
            // 
            accessBox.BackColor = SystemColors.ControlLight;
            accessBox.Location = new Point(101, 22);
            accessBox.Name = "accessBox";
            accessBox.PlaceholderText = "Token";
            accessBox.Size = new Size(100, 23);
            accessBox.TabIndex = 12;
            accessBox.TextChanged += accessBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(12, 108);
            label1.Name = "label1";
            label1.Size = new Size(394, 15);
            label1.TabIndex = 3;
            label1.Text = "Information will automatically reload on start. Must restart after changing";
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ButtonFace;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = SystemColors.ControlText;
            button3.Location = new Point(207, 51);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 8;
            button3.Text = "Save";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ControlText;
            label2.Location = new Point(18, 25);
            label2.Name = "label2";
            label2.Size = new Size(77, 15);
            label2.TabIndex = 4;
            label2.Text = "Access Token";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 9F);
            checkBox1.ForeColor = SystemColors.ControlText;
            checkBox1.Location = new Point(288, 25);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(121, 19);
            checkBox1.TabIndex = 11;
            checkBox1.Text = "Show information";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ControlText;
            label3.Location = new Point(9, 55);
            label3.Name = "label3";
            label3.Size = new Size(86, 15);
            label3.TabIndex = 5;
            label3.Text = "Channel Name";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ButtonFace;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = SystemColors.ControlText;
            button1.Location = new Point(207, 22);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // channelBox2
            // 
            channelBox2.BackColor = SystemColors.ControlLight;
            channelBox2.Location = new Point(101, 52);
            channelBox2.Name = "channelBox2";
            channelBox2.PlaceholderText = "Channel";
            channelBox2.Size = new Size(100, 23);
            channelBox2.TabIndex = 2;
            channelBox2.TextChanged += channelBox2_TextChanged;
            // 
            // channelOpen
            // 
            channelOpen.BackColor = SystemColors.ButtonFace;
            channelOpen.FlatAppearance.BorderSize = 0;
            channelOpen.FlatStyle = FlatStyle.Flat;
            channelOpen.ForeColor = SystemColors.ControlText;
            channelOpen.Location = new Point(288, 51);
            channelOpen.Name = "channelOpen";
            channelOpen.Size = new Size(90, 23);
            channelOpen.TabIndex = 13;
            channelOpen.Text = "Open Twitch";
            channelOpen.UseVisualStyleBackColor = false;
            channelOpen.Click += channelOpen_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(37, 37, 37);
            pictureBox1.Location = new Point(0, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 47);
            pictureBox1.TabIndex = 23;
            pictureBox1.TabStop = false;
            // 
            // changelogBox
            // 
            changelogBox.BackColor = Color.FromArgb(156, 155, 151);
            changelogBox.BorderStyle = BorderStyle.None;
            changelogBox.Location = new Point(635, 56);
            changelogBox.Multiline = true;
            changelogBox.Name = "changelogBox";
            changelogBox.ReadOnly = true;
            changelogBox.ScrollBars = ScrollBars.Vertical;
            changelogBox.Size = new Size(406, 345);
            changelogBox.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(135, 135, 135);
            label5.ForeColor = SystemColors.ControlText;
            label5.Location = new Point(72, 547);
            label5.Name = "label5";
            label5.Size = new Size(280, 15);
            label5.TabIndex = 30;
            label5.Text = "Created by Sprollucy with the help of AI and Friends";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // githubLink
            // 
            githubLink.AutoSize = true;
            githubLink.BackColor = Color.FromArgb(135, 135, 135);
            githubLink.Location = new Point(351, 547);
            githubLink.Name = "githubLink";
            githubLink.Size = new Size(43, 15);
            githubLink.TabIndex = 29;
            githubLink.TabStop = true;
            githubLink.Text = "Github";
            githubLink.TextAlign = ContentAlignment.MiddleCenter;
            githubLink.LinkClicked += linkLabel1_LinkClicked;
            // 
            // versionNumber
            // 
            versionNumber.AutoSize = true;
            versionNumber.BackColor = Color.FromArgb(135, 135, 135);
            versionNumber.ForeColor = SystemColors.ControlText;
            versionNumber.Location = new Point(897, 585);
            versionNumber.Name = "versionNumber";
            versionNumber.RightToLeft = RightToLeft.No;
            versionNumber.Size = new Size(89, 15);
            versionNumber.TabIndex = 28;
            versionNumber.Text = "versionNumber";
            versionNumber.TextAlign = ContentAlignment.MiddleRight;
            versionNumber.Click += versionNumber_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(135, 135, 135);
            label7.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label7.ForeColor = SystemColors.ControlText;
            label7.Location = new Point(70, 577);
            label7.Name = "label7";
            label7.Size = new Size(551, 25);
            label7.TabIndex = 27;
            label7.Text = "Don't forget to check for updates and report any bugs you find";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(135, 135, 135);
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Location = new Point(0, 532);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(1046, 85);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 31;
            pictureBox2.TabStop = false;
            // 
            // changelogLabel
            // 
            changelogLabel.AutoSize = true;
            changelogLabel.BackColor = Color.FromArgb(71, 83, 92);
            changelogLabel.Font = new Font("Cambria", 15.75F);
            changelogLabel.Location = new Point(635, 31);
            changelogLabel.Name = "changelogLabel";
            changelogLabel.Size = new Size(105, 25);
            changelogLabel.TabIndex = 34;
            changelogLabel.Text = "Changelog";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(135, 135, 135);
            label4.ForeColor = SystemColors.ControlText;
            label4.Location = new Point(72, 562);
            label4.Name = "label4";
            label4.Size = new Size(302, 15);
            label4.TabIndex = 35;
            label4.Text = "If you wish to support this project here is are some links!\r\n";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.FromArgb(135, 135, 135);
            linkLabel1.Location = new Point(373, 562);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(42, 15);
            linkLabel1.TabIndex = 36;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Paypal";
            linkLabel1.TextAlign = ContentAlignment.MiddleCenter;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked_1;
            // 
            // bitrestoreButton
            // 
            bitrestoreButton.BackColor = SystemColors.ButtonFace;
            bitrestoreButton.FlatAppearance.BorderSize = 0;
            bitrestoreButton.FlatStyle = FlatStyle.Flat;
            bitrestoreButton.ForeColor = SystemColors.ControlText;
            bitrestoreButton.Location = new Point(18, 22);
            bitrestoreButton.Name = "bitrestoreButton";
            bitrestoreButton.Size = new Size(201, 23);
            bitrestoreButton.TabIndex = 37;
            bitrestoreButton.Text = "Restore user bits from backup";
            bitrestoreButton.UseVisualStyleBackColor = false;
            bitrestoreButton.Click += bitrestoreButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.FromArgb(156, 155, 151);
            groupBox2.Controls.Add(bitrestoreButton);
            groupBox2.ForeColor = SystemColors.ControlText;
            groupBox2.Location = new Point(5, 175);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(417, 58);
            groupBox2.TabIndex = 39;
            groupBox2.TabStop = false;
            groupBox2.Text = "Restore settings";
            // 
            // enableUpdateCheck
            // 
            enableUpdateCheck.AutoSize = true;
            enableUpdateCheck.BackColor = Color.FromArgb(156, 155, 151);
            enableUpdateCheck.Checked = true;
            enableUpdateCheck.CheckState = CheckState.Checked;
            enableUpdateCheck.ForeColor = SystemColors.ControlText;
            enableUpdateCheck.Location = new Point(209, 24);
            enableUpdateCheck.Name = "enableUpdateCheck";
            enableUpdateCheck.Size = new Size(197, 19);
            enableUpdateCheck.TabIndex = 40;
            enableUpdateCheck.Text = "Enable Automatic Update Check";
            enableUpdateCheck.UseVisualStyleBackColor = false;
            enableUpdateCheck.CheckedChanged += enableUpdateCheck_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.FromArgb(156, 155, 151);
            groupBox3.Controls.Add(checkUpdateButton);
            groupBox3.Controls.Add(enableUpdateCheck);
            groupBox3.ForeColor = SystemColors.ControlText;
            groupBox3.Location = new Point(5, 239);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(417, 55);
            groupBox3.TabIndex = 40;
            groupBox3.TabStop = false;
            groupBox3.Text = "Update Settings";
            // 
            // checkUpdateButton
            // 
            checkUpdateButton.BackColor = SystemColors.ButtonFace;
            checkUpdateButton.FlatAppearance.BorderSize = 0;
            checkUpdateButton.FlatStyle = FlatStyle.Flat;
            checkUpdateButton.ForeColor = SystemColors.ControlText;
            checkUpdateButton.Location = new Point(18, 21);
            checkUpdateButton.Name = "checkUpdateButton";
            checkUpdateButton.Size = new Size(136, 23);
            checkUpdateButton.TabIndex = 37;
            checkUpdateButton.Text = "Check For Update";
            checkUpdateButton.UseVisualStyleBackColor = false;
            checkUpdateButton.Click += checkUpdateButton_Click;
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox6.BackgroundImageLayout = ImageLayout.None;
            pictureBox6.Location = new Point(635, 31);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(411, 25);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 109;
            pictureBox6.TabStop = false;
            // 
            // enableDebug
            // 
            enableDebug.AutoSize = true;
            enableDebug.BackColor = Color.FromArgb(156, 155, 151);
            enableDebug.ForeColor = SystemColors.ControlText;
            enableDebug.Location = new Point(9, 22);
            enableDebug.Name = "enableDebug";
            enableDebug.Size = new Size(133, 19);
            enableDebug.TabIndex = 110;
            enableDebug.Text = "Enable Debug Mode";
            enableDebug.UseVisualStyleBackColor = false;
            enableDebug.CheckedChanged += enableDebug_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.FromArgb(156, 155, 151);
            groupBox4.Controls.Add(enableDebugCommands);
            groupBox4.Controls.Add(enableDebug);
            groupBox4.ForeColor = SystemColors.ControlText;
            groupBox4.Location = new Point(5, 294);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(417, 55);
            groupBox4.TabIndex = 111;
            groupBox4.TabStop = false;
            groupBox4.Text = "Debug Settings";
            // 
            // enableDebugCommands
            // 
            enableDebugCommands.AutoSize = true;
            enableDebugCommands.BackColor = Color.FromArgb(156, 155, 151);
            enableDebugCommands.ForeColor = SystemColors.ControlText;
            enableDebugCommands.Location = new Point(166, 22);
            enableDebugCommands.Name = "enableDebugCommands";
            enableDebugCommands.Size = new Size(184, 19);
            enableDebugCommands.TabIndex = 111;
            enableDebugCommands.Text = "Enable Debug For Commands";
            enableDebugCommands.UseVisualStyleBackColor = false;
            enableDebugCommands.CheckedChanged += enableDebugCommands_CheckedChanged;
            // 
            // cashappLink
            // 
            cashappLink.AutoSize = true;
            cashappLink.BackColor = Color.FromArgb(135, 135, 135);
            cashappLink.Location = new Point(415, 562);
            cashappLink.Name = "cashappLink";
            cashappLink.Size = new Size(58, 15);
            cashappLink.TabIndex = 113;
            cashappLink.TabStop = true;
            cashappLink.Text = "Cash App";
            cashappLink.LinkClicked += cashappLink_LinkClicked;
            // 
            // discordLink
            // 
            discordLink.AutoSize = true;
            discordLink.BackColor = Color.FromArgb(135, 135, 135);
            discordLink.Location = new Point(394, 547);
            discordLink.Name = "discordLink";
            discordLink.Size = new Size(47, 15);
            discordLink.TabIndex = 114;
            discordLink.TabStop = true;
            discordLink.Text = "Discord";
            discordLink.TextAlign = ContentAlignment.MiddleCenter;
            discordLink.LinkClicked += discordLink_LinkClicked;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(156, 155, 151);
            panel5.Controls.Add(label19);
            panel5.Controls.Add(pictureBox13);
            panel5.Controls.Add(groupBox1);
            panel5.Controls.Add(groupBox2);
            panel5.Controls.Add(groupBox4);
            panel5.Controls.Add(groupBox3);
            panel5.Location = new Point(53, 31);
            panel5.Name = "panel5";
            panel5.Size = new Size(428, 358);
            panel5.TabIndex = 115;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.BackColor = Color.FromArgb(71, 83, 92);
            label19.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label19.ForeColor = SystemColors.ControlLight;
            label19.Location = new Point(3, 0);
            label19.Name = "label19";
            label19.Size = new Size(69, 19);
            label19.TabIndex = 51;
            label19.Text = "Settings";
            // 
            // pictureBox13
            // 
            pictureBox13.BackColor = Color.FromArgb(71, 83, 92);
            pictureBox13.BackgroundImageLayout = ImageLayout.None;
            pictureBox13.Location = new Point(0, -1);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new Size(428, 24);
            pictureBox13.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox13.TabIndex = 50;
            pictureBox13.TabStop = false;
            // 
            // SettingMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(37, 37, 37);
            ClientSize = new Size(1041, 616);
            ControlBox = false;
            Controls.Add(panel5);
            Controls.Add(discordLink);
            Controls.Add(cashappLink);
            Controls.Add(changelogLabel);
            Controls.Add(pictureBox6);
            Controls.Add(linkLabel1);
            Controls.Add(label4);
            Controls.Add(changelogBox);
            Controls.Add(label5);
            Controls.Add(githubLink);
            Controls.Add(versionNumber);
            Controls.Add(label7);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox2);
            ForeColor = SystemColors.ControlLight;
            FormBorderStyle = FormBorderStyle.None;
            Name = "SettingMenu";
            Text = "SettingMenu";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private Button button3;
        private Label label2;
        private CheckBox checkBox1;
        private Label label3;
        private Button button1;
        private TextBox channelBox2;
        private PictureBox pictureBox1;
        private Button helpButton;
        private TextBox accessBox;
        private Button channelOpen;
        private TextBox changelogBox;
        private Label label5;
        private LinkLabel githubLink;
        private Label versionNumber;
        private Label label7;
        private PictureBox pictureBox2;
        private Label changelogLabel;
        private Label label4;
        private LinkLabel linkLabel1;
        private Button bitrestoreButton;
        private GroupBox groupBox2;
        private CheckBox enableUpdateCheck;
        private GroupBox groupBox3;
        private Button button2;
        private Button button4;
        private Button checkUpdateButton;
        private PictureBox pictureBox6;
        private CheckBox enableDebug;
        private GroupBox groupBox4;
        private LinkLabel cashappLink;
        private LinkLabel discordLink;
        private CheckBox enableDebugCommands;
        private Panel panel5;
        private Label label19;
        private PictureBox pictureBox13;
    }
}