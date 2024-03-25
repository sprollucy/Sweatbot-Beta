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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingMenu));
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
            textBox3 = new TextBox();
            label5 = new Label();
            githubLink = new LinkLabel();
            versionNumber = new Label();
            label7 = new Label();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            changelogLabel = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
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
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
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
            label1.Font = new Font("Segoe UI Light", 9F, FontStyle.Regular, GraphicsUnit.Point);
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
            checkBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
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
            // textBox3
            // 
            textBox3.BackColor = Color.FromArgb(222, 208, 182);
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Location = new Point(647, 76);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.ScrollBars = ScrollBars.Vertical;
            textBox3.Size = new Size(386, 306);
            textBox3.TabIndex = 1;
            textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(156, 155, 151);
            label5.Location = new Point(77, 398);
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
            githubLink.Location = new Point(334, 398);
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
            versionNumber.Location = new Point(872, 426);
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
            label7.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(75, 413);
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
            pictureBox2.Location = new Point(0, 390);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(629, 63);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 31;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(201, 198, 189);
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(635, 28);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(470, 385);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 32;
            pictureBox3.TabStop = false;
            // 
            // changelogLabel
            // 
            changelogLabel.AutoSize = true;
            changelogLabel.BackColor = Color.FromArgb(162, 123, 92);
            changelogLabel.Font = new Font("Cambria", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            changelogLabel.Location = new Point(647, 40);
            changelogLabel.Name = "changelogLabel";
            changelogLabel.Size = new Size(105, 25);
            changelogLabel.TabIndex = 34;
            changelogLabel.Text = "Changelog";
            // 
            // SettingMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1058, 487);
            ControlBox = false;
            Controls.Add(changelogLabel);
            Controls.Add(textBox3);
            Controls.Add(pictureBox3);
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
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
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
        private TextBox textBox3;
        private Label label5;
        private LinkLabel githubLink;
        private Label versionNumber;
        private Label label7;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Label changelogLabel;
    }
}