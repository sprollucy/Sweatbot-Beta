namespace UiBot
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
            pictureBox10 = new PictureBox();
            pictureBox1 = new PictureBox();
            channelOpen = new Button();
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
            restoreCommandButton = new Button();
            groupBox2 = new GroupBox();
            defaultCommandButton = new Button();
            enableUpdateCheck = new CheckBox();
            groupBox3 = new GroupBox();
            checkUpdateButton = new Button();
            pictureBox6 = new PictureBox();
            enableDebug = new CheckBox();
            groupBox4 = new GroupBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(accessBox);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(channelBox2);
            groupBox1.Font = new Font("Segoe UI", 9F);
            groupBox1.Location = new Point(54, 25);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(417, 140);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Login Info";
            // 
            // accessBox
            // 
            accessBox.Location = new Point(101, 22);
            accessBox.Name = "accessBox";
            accessBox.Size = new Size(100, 23);
            accessBox.TabIndex = 12;
            accessBox.TextChanged += accessBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Light", 9F);
            label1.Location = new Point(23, 108);
            label1.Name = "label1";
            label1.Size = new Size(228, 15);
            label1.TabIndex = 3;
            label1.Text = "Information will automatically reload on start";
            // 
            // button3
            // 
            button3.Location = new Point(207, 51);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 8;
            button3.Text = "Save";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
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
            label3.Location = new Point(9, 55);
            label3.Name = "label3";
            label3.Size = new Size(86, 15);
            label3.TabIndex = 5;
            label3.Text = "Channel Name";
            // 
            // button1
            // 
            button1.Location = new Point(207, 22);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // channelBox2
            // 
            channelBox2.Location = new Point(101, 52);
            channelBox2.Name = "channelBox2";
            channelBox2.Size = new Size(100, 23);
            channelBox2.TabIndex = 2;
            channelBox2.TextChanged += channelBox2_TextChanged;
            // 
            // pictureBox10
            // 
            pictureBox10.BackColor = Color.FromArgb(63, 78, 79);
            pictureBox10.Location = new Point(43, -1);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(757, 20);
            pictureBox10.TabIndex = 24;
            pictureBox10.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Red;
            pictureBox1.Location = new Point(0, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 47);
            pictureBox1.TabIndex = 23;
            pictureBox1.TabStop = false;
            // 
            // channelOpen
            // 
            channelOpen.Location = new Point(342, 77);
            channelOpen.Name = "channelOpen";
            channelOpen.Size = new Size(90, 23);
            channelOpen.TabIndex = 13;
            channelOpen.Text = "Open Twitch";
            channelOpen.UseVisualStyleBackColor = true;
            channelOpen.Click += channelOpen_Click;
            // 
            // changelogBox
            // 
            changelogBox.BackColor = Color.FromArgb(220, 215, 201);
            changelogBox.BorderStyle = BorderStyle.None;
            changelogBox.Location = new Point(635, 58);
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
            label5.BackColor = Color.FromArgb(156, 155, 151);
            label5.Location = new Point(72, 500);
            label5.Name = "label5";
            label5.Size = new Size(251, 15);
            label5.TabIndex = 30;
            label5.Text = "Created by Sprollucy with the help of ChatGPT";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // githubLink
            // 
            githubLink.AutoSize = true;
            githubLink.BackColor = Color.FromArgb(156, 155, 151);
            githubLink.Location = new Point(329, 500);
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
            versionNumber.BackColor = Color.FromArgb(156, 155, 151);
            versionNumber.Location = new Point(860, 538);
            versionNumber.Name = "versionNumber";
            versionNumber.RightToLeft = RightToLeft.No;
            versionNumber.Size = new Size(89, 15);
            versionNumber.TabIndex = 28;
            versionNumber.Text = "versionNumber";
            versionNumber.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(156, 155, 151);
            label7.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label7.Location = new Point(70, 530);
            label7.Name = "label7";
            label7.Size = new Size(551, 25);
            label7.TabIndex = 27;
            label7.Text = "Don't forget to check for updates and report any bugs you find";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(156, 155, 151);
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Location = new Point(0, 485);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(1046, 85);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 31;
            pictureBox2.TabStop = false;
            // 
            // changelogLabel
            // 
            changelogLabel.AutoSize = true;
            changelogLabel.BackColor = Color.FromArgb(162, 123, 92);
            changelogLabel.Font = new Font("Cambria", 15.75F);
            changelogLabel.Location = new Point(635, 32);
            changelogLabel.Name = "changelogLabel";
            changelogLabel.Size = new Size(105, 25);
            changelogLabel.TabIndex = 34;
            changelogLabel.Text = "Changelog";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(156, 155, 151);
            label4.Location = new Point(72, 515);
            label4.Name = "label4";
            label4.Size = new Size(324, 15);
            label4.TabIndex = 35;
            label4.Text = "If you wish to support this project here is a link to my paypal";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.FromArgb(156, 155, 151);
            linkLabel1.Location = new Point(402, 515);
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
            bitrestoreButton.Location = new Point(18, 22);
            bitrestoreButton.Name = "bitrestoreButton";
            bitrestoreButton.Size = new Size(201, 23);
            bitrestoreButton.TabIndex = 37;
            bitrestoreButton.Text = "Restore user bits from backup";
            bitrestoreButton.UseVisualStyleBackColor = true;
            bitrestoreButton.Click += bitrestoreButton_Click;
            // 
            // restoreCommandButton
            // 
            restoreCommandButton.Location = new Point(18, 51);
            restoreCommandButton.Name = "restoreCommandButton";
            restoreCommandButton.Size = new Size(201, 23);
            restoreCommandButton.TabIndex = 38;
            restoreCommandButton.Text = "Restore commands from backup";
            restoreCommandButton.UseVisualStyleBackColor = true;
            restoreCommandButton.Click += restoreCommandButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(defaultCommandButton);
            groupBox2.Controls.Add(restoreCommandButton);
            groupBox2.Controls.Add(bitrestoreButton);
            groupBox2.Location = new Point(54, 171);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(417, 96);
            groupBox2.TabIndex = 39;
            groupBox2.TabStop = false;
            groupBox2.Text = "Restore settings";
            // 
            // defaultCommandButton
            // 
            defaultCommandButton.Location = new Point(230, 51);
            defaultCommandButton.Name = "defaultCommandButton";
            defaultCommandButton.Size = new Size(165, 23);
            defaultCommandButton.TabIndex = 39;
            defaultCommandButton.Text = "Restore default commands";
            defaultCommandButton.UseVisualStyleBackColor = true;
            defaultCommandButton.Click += button2_Click;
            // 
            // enableUpdateCheck
            // 
            enableUpdateCheck.AutoSize = true;
            enableUpdateCheck.BackColor = Color.FromArgb(201, 198, 189);
            enableUpdateCheck.Location = new Point(198, 25);
            enableUpdateCheck.Name = "enableUpdateCheck";
            enableUpdateCheck.Size = new Size(197, 19);
            enableUpdateCheck.TabIndex = 40;
            enableUpdateCheck.Text = "Enable Automatic Update Check";
            enableUpdateCheck.UseVisualStyleBackColor = false;
            enableUpdateCheck.CheckedChanged += enableUpdateCheck_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(checkUpdateButton);
            groupBox3.Controls.Add(enableUpdateCheck);
            groupBox3.Location = new Point(54, 272);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(417, 55);
            groupBox3.TabIndex = 40;
            groupBox3.TabStop = false;
            groupBox3.Text = "Update Settings";
            // 
            // checkUpdateButton
            // 
            checkUpdateButton.Location = new Point(18, 22);
            checkUpdateButton.Name = "checkUpdateButton";
            checkUpdateButton.Size = new Size(136, 23);
            checkUpdateButton.TabIndex = 37;
            checkUpdateButton.Text = "Check For Update";
            checkUpdateButton.UseVisualStyleBackColor = true;
            checkUpdateButton.Click += checkUpdateButton_Click;
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox6.BackgroundImageLayout = ImageLayout.None;
            pictureBox6.Location = new Point(635, 32);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(411, 25);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 109;
            pictureBox6.TabStop = false;
            // 
            // enableDebug
            // 
            enableDebug.AutoSize = true;
            enableDebug.Location = new Point(18, 22);
            enableDebug.Name = "enableDebug";
            enableDebug.Size = new Size(133, 19);
            enableDebug.TabIndex = 110;
            enableDebug.Text = "Enable Debug Mode";
            enableDebug.UseVisualStyleBackColor = true;
            enableDebug.CheckedChanged += enableDebug_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(enableDebug);
            groupBox4.Location = new Point(54, 333);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(417, 55);
            groupBox4.TabIndex = 111;
            groupBox4.TabStop = false;
            groupBox4.Text = "Debug Settings";
            // 
            // SettingMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1041, 602);
            ControlBox = false;
            Controls.Add(groupBox4);
            Controls.Add(changelogLabel);
            Controls.Add(pictureBox6);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(linkLabel1);
            Controls.Add(label4);
            Controls.Add(changelogBox);
            Controls.Add(label5);
            Controls.Add(githubLink);
            Controls.Add(versionNumber);
            Controls.Add(label7);
            Controls.Add(channelOpen);
            Controls.Add(pictureBox10);
            Controls.Add(pictureBox1);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SettingMenu";
            Text = "SettingMenu";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
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
        private PictureBox pictureBox10;
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
        private Button restoreCommandButton;
        private GroupBox groupBox2;
        private Button defaultCommandButton;
        private CheckBox enableUpdateCheck;
        private GroupBox groupBox3;
        private Button button2;
        private Button button4;
        private Button checkUpdateButton;
        private PictureBox pictureBox6;
        private CheckBox enableDebug;
        private GroupBox groupBox4;
    }
}