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
            removeCommandButton = new Button();
            label6 = new Label();
            pictureBox6 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox8 = new PictureBox();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            SuspendLayout();
            // 
            // nametextBox
            // 
            nametextBox.Location = new Point(53, 49);
            nametextBox.Name = "nametextBox";
            nametextBox.Size = new Size(100, 23);
            nametextBox.TabIndex = 1;
            // 
            // costtextBox
            // 
            costtextBox.Location = new Point(159, 49);
            costtextBox.Name = "costtextBox";
            costtextBox.Size = new Size(51, 23);
            costtextBox.TabIndex = 2;
            // 
            // commandtextBox
            // 
            commandtextBox.Location = new Point(216, 49);
            commandtextBox.Name = "commandtextBox";
            commandtextBox.Size = new Size(713, 23);
            commandtextBox.TabIndex = 3;
            commandtextBox.TextChanged += commandtextBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(162, 123, 92);
            label2.Location = new Point(53, 31);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(162, 123, 92);
            label3.Location = new Point(159, 31);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 5;
            label3.Text = "Cost";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(162, 123, 92);
            label4.Location = new Point(216, 31);
            label4.Name = "label4";
            label4.Size = new Size(95, 15);
            label4.TabIndex = 6;
            label4.Text = "Command input";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Location = new Point(53, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Location = new Point(216, 30);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(713, 24);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox3.BackgroundImageLayout = ImageLayout.None;
            pictureBox3.Location = new Point(159, 30);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(51, 24);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 8;
            pictureBox3.TabStop = false;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(935, 49);
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
            richTextBox1.Location = new Point(514, 239);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(527, 322);
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
            pictureBox5.BackColor = Color.FromArgb(220, 215, 201);
            pictureBox5.Location = new Point(503, 219);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(558, 362);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 12;
            pictureBox5.TabStop = false;
            // 
            // openCustomJson
            // 
            openCustomJson.Location = new Point(216, 236);
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
            label5.Location = new Point(404, 31);
            label5.Name = "label5";
            label5.Size = new Size(100, 15);
            label5.TabIndex = 14;
            label5.Text = "THIS IS WORKING";
            // 
            // commandListBox
            // 
            commandListBox.BackColor = Color.FromArgb(220, 215, 201);
            commandListBox.FormattingEnabled = true;
            commandListBox.ItemHeight = 15;
            commandListBox.Location = new Point(53, 207);
            commandListBox.Name = "commandListBox";
            commandListBox.Size = new Size(157, 154);
            commandListBox.TabIndex = 15;
            // 
            // removeCommandButton
            // 
            removeCommandButton.Location = new Point(216, 207);
            removeCommandButton.Name = "removeCommandButton";
            removeCommandButton.Size = new Size(108, 23);
            removeCommandButton.TabIndex = 17;
            removeCommandButton.Text = "Delete Command";
            removeCommandButton.UseVisualStyleBackColor = true;
            removeCommandButton.Click += removeCommandButton_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(162, 123, 92);
            label6.Font = new Font("Constantia", 14.25F, FontStyle.Bold);
            label6.Location = new Point(503, 207);
            label6.Name = "label6";
            label6.Size = new Size(211, 23);
            label6.TabIndex = 18;
            label6.Text = "Commands and Usage";
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox6.BackgroundImageLayout = ImageLayout.None;
            pictureBox6.Location = new Point(503, 207);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(545, 26);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 19;
            pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.FromArgb(220, 215, 201);
            pictureBox7.Location = new Point(53, 180);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(280, 184);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 20;
            pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(53, 180);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(280, 24);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 21;
            pictureBox8.TabStop = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(162, 123, 92);
            label7.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label7.Location = new Point(56, 181);
            label7.Name = "label7";
            label7.Size = new Size(134, 19);
            label7.TabIndex = 22;
            label7.Text = "Command input";
            // 
            // CommandBuilderMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1042, 570);
            ControlBox = false;
            Controls.Add(label7);
            Controls.Add(pictureBox8);
            Controls.Add(label6);
            Controls.Add(pictureBox6);
            Controls.Add(removeCommandButton);
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
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox7);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CommandBuilderMenu";
            Text = "QuestMenu";
            Load += QuestMenu_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
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
        private Button removeCommandButton;
        private Label label6;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private Label label7;
    }
}