namespace UiBot
{
    partial class CommandBuilderMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandBuilderMenu));
            label1 = new Label();
            nametextBox = new TextBox();
            costtextBox = new TextBox();
            commandtextBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            saveButton = new Button();
            richTextBox1 = new RichTextBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            openCustomJson = new Button();
            label5 = new Label();
            commandListBox = new ListBox();
            loadCommandButton = new Button();
            removeCommandButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(48, 23);
            label1.Name = "label1";
            label1.Size = new Size(770, 28);
            label1.TabIndex = 0;
            label1.Text = "UNSTABLE. IF YOU BREAK SOMETHING, THERE IS A BACK UP FILE IN THE DATA FOLDER";
            // 
            // nametextBox
            // 
            nametextBox.Location = new Point(53, 104);
            nametextBox.Name = "nametextBox";
            nametextBox.Size = new Size(100, 23);
            nametextBox.TabIndex = 1;
            // 
            // costtextBox
            // 
            costtextBox.Location = new Point(159, 104);
            costtextBox.Name = "costtextBox";
            costtextBox.Size = new Size(51, 23);
            costtextBox.TabIndex = 2;
            // 
            // commandtextBox
            // 
            commandtextBox.Location = new Point(216, 104);
            commandtextBox.Name = "commandtextBox";
            commandtextBox.Size = new Size(713, 23);
            commandtextBox.TabIndex = 3;
            commandtextBox.TextChanged += commandtextBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(162, 123, 92);
            label2.Location = new Point(53, 86);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(162, 123, 92);
            label3.Location = new Point(159, 86);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 5;
            label3.Text = "Cost";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(162, 123, 92);
            label4.Location = new Point(216, 86);
            label4.Name = "label4";
            label4.Size = new Size(95, 15);
            label4.TabIndex = 6;
            label4.Text = "Command input";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(53, 85);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(216, 85);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(713, 24);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImageLayout = ImageLayout.None;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(159, 85);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(51, 24);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 8;
            pictureBox3.TabStop = false;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(935, 104);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(220, 215, 201);
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Location = new Point(523, 239);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(518, 322);
            richTextBox1.TabIndex = 10;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(503, 219);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(667, 397);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 11;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(503, 219);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(667, 397);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 12;
            pictureBox5.TabStop = false;
            // 
            // openCustomJson
            // 
            openCustomJson.Location = new Point(253, 218);
            openCustomJson.Name = "openCustomJson";
            openCustomJson.Size = new Size(108, 47);
            openCustomJson.TabIndex = 13;
            openCustomJson.Text = "Open custom json";
            openCustomJson.UseVisualStyleBackColor = true;
            openCustomJson.Click += openCustomJson_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(162, 123, 92);
            label5.Font = new Font("Segoe UI", 9F);
            label5.ForeColor = Color.Red;
            label5.Location = new Point(404, 86);
            label5.Name = "label5";
            label5.Size = new Size(100, 15);
            label5.TabIndex = 14;
            label5.Text = "THIS IS WORKING";
            // 
            // commandListBox
            // 
            commandListBox.FormattingEnabled = true;
            commandListBox.ItemHeight = 15;
            commandListBox.Location = new Point(53, 160);
            commandListBox.Name = "commandListBox";
            commandListBox.Size = new Size(194, 289);
            commandListBox.TabIndex = 15;
            // 
            // loadCommandButton
            // 
            loadCommandButton.Location = new Point(253, 160);
            loadCommandButton.Name = "loadCommandButton";
            loadCommandButton.Size = new Size(108, 23);
            loadCommandButton.TabIndex = 16;
            loadCommandButton.Text = "Edit Command";
            loadCommandButton.UseVisualStyleBackColor = true;
            loadCommandButton.Click += loadCommandButton_Click;
            // 
            // removeCommandButton
            // 
            removeCommandButton.Location = new Point(253, 189);
            removeCommandButton.Name = "removeCommandButton";
            removeCommandButton.Size = new Size(108, 23);
            removeCommandButton.TabIndex = 17;
            removeCommandButton.Text = "Delete Command";
            removeCommandButton.UseVisualStyleBackColor = true;
            removeCommandButton.Click += removeCommandButton_Click;
            // 
            // CommandBuilderMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1042, 570);
            ControlBox = false;
            Controls.Add(removeCommandButton);
            Controls.Add(loadCommandButton);
            Controls.Add(commandListBox);
            Controls.Add(label5);
            Controls.Add(openCustomJson);
            Controls.Add(richTextBox1);
            Controls.Add(saveButton);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(commandtextBox);
            Controls.Add(costtextBox);
            Controls.Add(nametextBox);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CommandBuilderMenu";
            Text = "QuestMenu";
            Load += QuestMenu_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private TextBox nametextBox;
        private TextBox costtextBox;
        private TextBox commandtextBox;
        private Label label2;
        private Label label3;
        private Label label4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Button saveButton;
        private RichTextBox richTextBox1;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private Button openCustomJson;
        private Label label5;
        private ListBox commandListBox;
        private Button loadCommandButton;
        private Button removeCommandButton;
    }
}