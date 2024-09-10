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
            nametextBox = new TextBox();
            costtextBox = new TextBox();
            commandtextBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            saveButton = new Button();
            commanduseBox = new RichTextBox();
            openCustomJson = new Button();
            label5 = new Label();
            commandListBox = new ListBox();
            removeCommandButton = new Button();
            label6 = new Label();
            pictureBox6 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox8 = new PictureBox();
            label7 = new Label();
            restartButton = new Button();
            holdkeyButton = new Button();
            panel1 = new Panel();
            label14 = new Label();
            mouseposTextBox = new TextBox();
            mouseposButton = new Button();
            arightloopButton = new Button();
            aleftloopButton = new Button();
            rightloopButton = new Button();
            leftloopButton = new Button();
            amuteButton = new Button();
            ahitkeyloopButton = new Button();
            hitkeyloopButton = new Button();
            directionBox = new TextBox();
            muteButton = new Button();
            aplaysoundButton = new Button();
            arightholdButton = new Button();
            speedBox = new TextBox();
            aleftholdButton = new Button();
            arightButton = new Button();
            keyBox = new TextBox();
            aleftButton = new Button();
            aturnButton = new Button();
            label11 = new Label();
            ahitkeyButton = new Button();
            delayButton = new Button();
            playsoundButton = new Button();
            durBox = new TextBox();
            rightholdButton = new Button();
            leftholdButton = new Button();
            rightButton = new Button();
            leftButton = new Button();
            turnButton = new Button();
            label10 = new Label();
            hitkeyButton = new Button();
            aholdkeyButton = new Button();
            label9 = new Label();
            label8 = new Label();
            pictureBox5 = new PictureBox();
            disabledcommandsListBox = new ListBox();
            disablecommandButton = new Button();
            restorecommandButton = new Button();
            label12 = new Label();
            label13 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // nametextBox
            // 
            nametextBox.Location = new Point(55, 55);
            nametextBox.Name = "nametextBox";
            nametextBox.Size = new Size(100, 23);
            nametextBox.TabIndex = 1;
            // 
            // costtextBox
            // 
            costtextBox.Location = new Point(161, 55);
            costtextBox.Name = "costtextBox";
            costtextBox.Size = new Size(51, 23);
            costtextBox.TabIndex = 2;
            // 
            // commandtextBox
            // 
            commandtextBox.Location = new Point(218, 55);
            commandtextBox.Name = "commandtextBox";
            commandtextBox.Size = new Size(812, 23);
            commandtextBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(162, 123, 92);
            label2.Location = new Point(55, 37);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(162, 123, 92);
            label3.Location = new Point(161, 37);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 5;
            label3.Text = "Cost";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(162, 123, 92);
            label4.Location = new Point(218, 37);
            label4.Name = "label4";
            label4.Size = new Size(282, 15);
            label4.TabIndex = 6;
            label4.Text = "Command input (Use space to separate commands)";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Location = new Point(55, 36);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(975, 42);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(689, 107);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(166, 40);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save Command";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // commanduseBox
            // 
            commanduseBox.BackColor = Color.FromArgb(220, 215, 201);
            commanduseBox.BorderStyle = BorderStyle.None;
            commanduseBox.Location = new Point(513, 265);
            commanduseBox.Name = "commanduseBox";
            commanduseBox.ReadOnly = true;
            commanduseBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            commanduseBox.Size = new Size(528, 300);
            commanduseBox.TabIndex = 10;
            commanduseBox.Text = "";
            // 
            // openCustomJson
            // 
            openCustomJson.Location = new Point(218, 310);
            openCustomJson.Name = "openCustomJson";
            openCustomJson.Size = new Size(108, 23);
            openCustomJson.TabIndex = 13;
            openCustomJson.Text = "Open json";
            openCustomJson.UseVisualStyleBackColor = true;
            openCustomJson.Click += openCustomJson_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(220, 215, 201);
            label5.Font = new Font("Segoe UI", 12F);
            label5.ForeColor = Color.Red;
            label5.Location = new Point(152, 489);
            label5.Name = "label5";
            label5.Size = new Size(238, 42);
            label5.TabIndex = 14;
            label5.Text = "Must restart application for \r\ncommands to show up on Twitch";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // commandListBox
            // 
            commandListBox.BackColor = Color.FromArgb(220, 215, 201);
            commandListBox.FormattingEnabled = true;
            commandListBox.ItemHeight = 15;
            commandListBox.Location = new Point(55, 281);
            commandListBox.Name = "commandListBox";
            commandListBox.Size = new Size(157, 154);
            commandListBox.TabIndex = 15;
            // 
            // removeCommandButton
            // 
            removeCommandButton.Location = new Point(58, 439);
            removeCommandButton.Name = "removeCommandButton";
            removeCommandButton.Size = new Size(74, 45);
            removeCommandButton.TabIndex = 17;
            removeCommandButton.Text = "Delete Command";
            removeCommandButton.UseVisualStyleBackColor = true;
            removeCommandButton.Click += removeCommandButton_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(162, 123, 92);
            label6.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label6.Location = new Point(506, 239);
            label6.Name = "label6";
            label6.Size = new Size(179, 19);
            label6.TabIndex = 18;
            label6.Text = "Commands and Usage";
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox6.BackgroundImageLayout = ImageLayout.None;
            pictureBox6.Location = new Point(503, 239);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(545, 23);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 19;
            pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.FromArgb(220, 215, 201);
            pictureBox7.Location = new Point(55, 238);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(434, 293);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 20;
            pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(162, 123, 92);
            pictureBox8.BackgroundImageLayout = ImageLayout.None;
            pictureBox8.Location = new Point(55, 238);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(434, 24);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 21;
            pictureBox8.TabStop = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(162, 123, 92);
            label7.Font = new Font("Constantia", 12F, FontStyle.Bold);
            label7.Location = new Point(58, 239);
            label7.Name = "label7";
            label7.Size = new Size(120, 19);
            label7.TabIndex = 22;
            label7.Text = "Command List";
            // 
            // restartButton
            // 
            restartButton.Location = new Point(218, 281);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(108, 23);
            restartButton.TabIndex = 23;
            restartButton.Text = "Restart";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += restartButton_Click;
            // 
            // holdkeyButton
            // 
            holdkeyButton.Location = new Point(186, 3);
            holdkeyButton.Name = "holdkeyButton";
            holdkeyButton.Size = new Size(75, 23);
            holdkeyButton.TabIndex = 24;
            holdkeyButton.Text = "Hold Key";
            holdkeyButton.UseVisualStyleBackColor = true;
            holdkeyButton.Click += holdkeyButton_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(220, 215, 201);
            panel1.Controls.Add(label14);
            panel1.Controls.Add(mouseposTextBox);
            panel1.Controls.Add(mouseposButton);
            panel1.Controls.Add(arightloopButton);
            panel1.Controls.Add(aleftloopButton);
            panel1.Controls.Add(rightloopButton);
            panel1.Controls.Add(leftloopButton);
            panel1.Controls.Add(amuteButton);
            panel1.Controls.Add(ahitkeyloopButton);
            panel1.Controls.Add(hitkeyloopButton);
            panel1.Controls.Add(directionBox);
            panel1.Controls.Add(muteButton);
            panel1.Controls.Add(aplaysoundButton);
            panel1.Controls.Add(arightholdButton);
            panel1.Controls.Add(speedBox);
            panel1.Controls.Add(aleftholdButton);
            panel1.Controls.Add(arightButton);
            panel1.Controls.Add(keyBox);
            panel1.Controls.Add(aleftButton);
            panel1.Controls.Add(aturnButton);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(ahitkeyButton);
            panel1.Controls.Add(delayButton);
            panel1.Controls.Add(playsoundButton);
            panel1.Controls.Add(durBox);
            panel1.Controls.Add(rightholdButton);
            panel1.Controls.Add(saveButton);
            panel1.Controls.Add(leftholdButton);
            panel1.Controls.Add(rightButton);
            panel1.Controls.Add(leftButton);
            panel1.Controls.Add(turnButton);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(hitkeyButton);
            panel1.Controls.Add(aholdkeyButton);
            panel1.Controls.Add(holdkeyButton);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Location = new Point(55, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(975, 154);
            panel1.TabIndex = 25;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(96, 127);
            label14.Name = "label14";
            label14.Size = new Size(230, 15);
            label14.TabIndex = 59;
            label14.Text = "Click box then hit Shift for Mouse Position";
            // 
            // mouseposTextBox
            // 
            mouseposTextBox.Location = new Point(3, 124);
            mouseposTextBox.Name = "mouseposTextBox";
            mouseposTextBox.Size = new Size(87, 23);
            mouseposTextBox.TabIndex = 58;
            mouseposTextBox.Text = "X: 0, Y: 0";
            // 
            // mouseposButton
            // 
            mouseposButton.Location = new Point(346, 78);
            mouseposButton.Name = "mouseposButton";
            mouseposButton.Size = new Size(85, 40);
            mouseposButton.TabIndex = 57;
            mouseposButton.Text = "Move Mouse to Position";
            mouseposButton.UseVisualStyleBackColor = true;
            mouseposButton.Click += mouseposButton_Click;
            // 
            // arightloopButton
            // 
            arightloopButton.Location = new Point(541, 107);
            arightloopButton.Name = "arightloopButton";
            arightloopButton.Size = new Size(98, 40);
            arightloopButton.TabIndex = 56;
            arightloopButton.Text = "Sync Loop Right Click";
            arightloopButton.UseVisualStyleBackColor = true;
            arightloopButton.Click += arightloopButton_Click;
            // 
            // aleftloopButton
            // 
            aleftloopButton.Location = new Point(437, 107);
            aleftloopButton.Name = "aleftloopButton";
            aleftloopButton.Size = new Size(98, 40);
            aleftloopButton.TabIndex = 55;
            aleftloopButton.Text = "Sync Loop Left Click";
            aleftloopButton.UseVisualStyleBackColor = true;
            aleftloopButton.Click += aleftloopButton_Click;
            // 
            // rightloopButton
            // 
            rightloopButton.Location = new Point(541, 78);
            rightloopButton.Name = "rightloopButton";
            rightloopButton.Size = new Size(98, 23);
            rightloopButton.TabIndex = 54;
            rightloopButton.Text = "Loop Right Click";
            rightloopButton.UseVisualStyleBackColor = true;
            rightloopButton.Click += rightloopButton_Click;
            // 
            // leftloopButton
            // 
            leftloopButton.Location = new Point(437, 78);
            leftloopButton.Name = "leftloopButton";
            leftloopButton.Size = new Size(98, 23);
            leftloopButton.TabIndex = 53;
            leftloopButton.Text = "Loop Left Click";
            leftloopButton.UseVisualStyleBackColor = true;
            leftloopButton.Click += leftloopButton_Click;
            // 
            // amuteButton
            // 
            amuteButton.Location = new Point(897, 78);
            amuteButton.Name = "amuteButton";
            amuteButton.Size = new Size(75, 40);
            amuteButton.TabIndex = 52;
            amuteButton.Text = "Sync Mute Windows";
            amuteButton.UseVisualStyleBackColor = true;
            amuteButton.Click += amuteButton_Click;
            // 
            // ahitkeyloopButton
            // 
            ahitkeyloopButton.Location = new Point(267, 78);
            ahitkeyloopButton.Name = "ahitkeyloopButton";
            ahitkeyloopButton.Size = new Size(73, 40);
            ahitkeyloopButton.TabIndex = 51;
            ahitkeyloopButton.Text = "Sync Hit Key Loop";
            ahitkeyloopButton.UseVisualStyleBackColor = true;
            ahitkeyloopButton.Click += ahitkeyloopButton_Click;
            // 
            // hitkeyloopButton
            // 
            hitkeyloopButton.Location = new Point(186, 78);
            hitkeyloopButton.Name = "hitkeyloopButton";
            hitkeyloopButton.Size = new Size(75, 40);
            hitkeyloopButton.TabIndex = 50;
            hitkeyloopButton.Text = "Hit Key Loop";
            hitkeyloopButton.UseVisualStyleBackColor = true;
            hitkeyloopButton.Click += hitkeyloopButton_Click;
            // 
            // directionBox
            // 
            directionBox.Location = new Point(2, 91);
            directionBox.Name = "directionBox";
            directionBox.Size = new Size(39, 23);
            directionBox.TabIndex = 45;
            // 
            // muteButton
            // 
            muteButton.Location = new Point(897, 33);
            muteButton.Name = "muteButton";
            muteButton.Size = new Size(75, 40);
            muteButton.TabIndex = 41;
            muteButton.Text = "Mute Windows";
            muteButton.UseVisualStyleBackColor = true;
            muteButton.Click += muteButton_Click;
            // 
            // aplaysoundButton
            // 
            aplaysoundButton.Location = new Point(816, 32);
            aplaysoundButton.Name = "aplaysoundButton";
            aplaysoundButton.Size = new Size(75, 40);
            aplaysoundButton.TabIndex = 40;
            aplaysoundButton.Text = "Sync Play Sound";
            aplaysoundButton.UseVisualStyleBackColor = true;
            aplaysoundButton.Click += aplaysoundButton_Click;
            // 
            // arightholdButton
            // 
            arightholdButton.Location = new Point(705, 32);
            arightholdButton.Name = "arightholdButton";
            arightholdButton.Size = new Size(105, 40);
            arightholdButton.TabIndex = 39;
            arightholdButton.Text = "Sync Right Click Hold";
            arightholdButton.UseVisualStyleBackColor = true;
            arightholdButton.Click += arightholdButton_Click;
            // 
            // speedBox
            // 
            speedBox.Location = new Point(2, 62);
            speedBox.Name = "speedBox";
            speedBox.Size = new Size(39, 23);
            speedBox.TabIndex = 44;
            speedBox.Text = "10";
            // 
            // aleftholdButton
            // 
            aleftholdButton.Location = new Point(599, 32);
            aleftholdButton.Name = "aleftholdButton";
            aleftholdButton.Size = new Size(100, 40);
            aleftholdButton.TabIndex = 38;
            aleftholdButton.Text = "Sync Left Click Hold";
            aleftholdButton.UseVisualStyleBackColor = true;
            aleftholdButton.Click += aleftholdButton_Click;
            // 
            // arightButton
            // 
            arightButton.Location = new Point(518, 32);
            arightButton.Name = "arightButton";
            arightButton.Size = new Size(75, 40);
            arightButton.TabIndex = 37;
            arightButton.Text = "Sync Right Click";
            arightButton.UseVisualStyleBackColor = true;
            arightButton.Click += arightButton_Click;
            // 
            // keyBox
            // 
            keyBox.Location = new Point(2, 33);
            keyBox.Name = "keyBox";
            keyBox.Size = new Size(39, 23);
            keyBox.TabIndex = 43;
            keyBox.Text = "A";
            // 
            // aleftButton
            // 
            aleftButton.Location = new Point(437, 32);
            aleftButton.Name = "aleftButton";
            aleftButton.Size = new Size(75, 40);
            aleftButton.TabIndex = 36;
            aleftButton.Text = "Sync Left Click";
            aleftButton.UseVisualStyleBackColor = true;
            aleftButton.Click += aleftButton_Click;
            // 
            // aturnButton
            // 
            aturnButton.Location = new Point(346, 32);
            aturnButton.Name = "aturnButton";
            aturnButton.Size = new Size(85, 40);
            aturnButton.TabIndex = 35;
            aturnButton.Text = "Sync Turn Mouse";
            aturnButton.UseVisualStyleBackColor = true;
            aturnButton.Click += aturnButton_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(45, 94);
            label11.Name = "label11";
            label11.Size = new Size(139, 15);
            label11.TabIndex = 49;
            label11.Text = "Direction(U,D,L,R, RAND)";
            // 
            // ahitkeyButton
            // 
            ahitkeyButton.Location = new Point(265, 32);
            ahitkeyButton.Name = "ahitkeyButton";
            ahitkeyButton.Size = new Size(75, 40);
            ahitkeyButton.TabIndex = 34;
            ahitkeyButton.Text = "Sync Hit Key";
            ahitkeyButton.UseVisualStyleBackColor = true;
            ahitkeyButton.Click += ahitkeyButton_Click;
            // 
            // delayButton
            // 
            delayButton.Location = new Point(897, 3);
            delayButton.Name = "delayButton";
            delayButton.Size = new Size(75, 23);
            delayButton.TabIndex = 33;
            delayButton.Text = "Delay";
            delayButton.UseVisualStyleBackColor = true;
            delayButton.Click += delayButton_Click;
            // 
            // playsoundButton
            // 
            playsoundButton.Location = new Point(816, 3);
            playsoundButton.Name = "playsoundButton";
            playsoundButton.Size = new Size(75, 23);
            playsoundButton.TabIndex = 32;
            playsoundButton.Text = "Play Sound Clip";
            playsoundButton.UseVisualStyleBackColor = true;
            playsoundButton.Click += playsoundButton_Click;
            // 
            // durBox
            // 
            durBox.Location = new Point(2, 4);
            durBox.Name = "durBox";
            durBox.Size = new Size(39, 23);
            durBox.TabIndex = 42;
            durBox.Text = "1000";
            // 
            // rightholdButton
            // 
            rightholdButton.Location = new Point(705, 3);
            rightholdButton.Name = "rightholdButton";
            rightholdButton.Size = new Size(105, 23);
            rightholdButton.TabIndex = 31;
            rightholdButton.Text = "Right Click Hold";
            rightholdButton.UseVisualStyleBackColor = true;
            rightholdButton.Click += rightholdButton_Click;
            // 
            // leftholdButton
            // 
            leftholdButton.Location = new Point(599, 3);
            leftholdButton.Name = "leftholdButton";
            leftholdButton.Size = new Size(100, 23);
            leftholdButton.TabIndex = 30;
            leftholdButton.Text = "Left Click Hold";
            leftholdButton.UseVisualStyleBackColor = true;
            leftholdButton.Click += leftholdButton_Click;
            // 
            // rightButton
            // 
            rightButton.Location = new Point(518, 3);
            rightButton.Name = "rightButton";
            rightButton.Size = new Size(75, 23);
            rightButton.TabIndex = 29;
            rightButton.Text = "Right Click";
            rightButton.UseVisualStyleBackColor = true;
            rightButton.Click += rightButton_Click;
            // 
            // leftButton
            // 
            leftButton.Location = new Point(437, 3);
            leftButton.Name = "leftButton";
            leftButton.Size = new Size(75, 23);
            leftButton.TabIndex = 28;
            leftButton.Text = "Left Click";
            leftButton.UseVisualStyleBackColor = true;
            leftButton.Click += leftButton_Click;
            // 
            // turnButton
            // 
            turnButton.Location = new Point(346, 3);
            turnButton.Name = "turnButton";
            turnButton.Size = new Size(85, 23);
            turnButton.TabIndex = 27;
            turnButton.Text = "Turn Mouse";
            turnButton.UseVisualStyleBackColor = true;
            turnButton.Click += turnButton_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(45, 67);
            label10.Name = "label10";
            label10.Size = new Size(73, 15);
            label10.TabIndex = 48;
            label10.Text = "Speed/Delay";
            // 
            // hitkeyButton
            // 
            hitkeyButton.Location = new Point(265, 3);
            hitkeyButton.Name = "hitkeyButton";
            hitkeyButton.Size = new Size(75, 23);
            hitkeyButton.TabIndex = 26;
            hitkeyButton.Text = "Hit Key";
            hitkeyButton.UseVisualStyleBackColor = true;
            hitkeyButton.Click += hitkeyButton_Click;
            // 
            // aholdkeyButton
            // 
            aholdkeyButton.Location = new Point(186, 32);
            aholdkeyButton.Name = "aholdkeyButton";
            aholdkeyButton.Size = new Size(75, 40);
            aholdkeyButton.TabIndex = 25;
            aholdkeyButton.Text = "Sync Hold Key";
            aholdkeyButton.UseVisualStyleBackColor = true;
            aholdkeyButton.Click += aholdkeyButton_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(45, 36);
            label9.Name = "label9";
            label9.Size = new Size(57, 15);
            label9.TabIndex = 47;
            label9.Text = "Input Key";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(45, 7);
            label8.Name = "label8";
            label8.Size = new Size(139, 15);
            label8.TabIndex = 46;
            label8.Text = "Amount/Duration(in ms)";
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.FromArgb(220, 215, 201);
            pictureBox5.Location = new Point(503, 255);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(558, 326);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 12;
            pictureBox5.TabStop = false;
            // 
            // disabledcommandsListBox
            // 
            disabledcommandsListBox.BackColor = Color.FromArgb(220, 215, 201);
            disabledcommandsListBox.FormattingEnabled = true;
            disabledcommandsListBox.ItemHeight = 15;
            disabledcommandsListBox.Location = new Point(332, 281);
            disabledcommandsListBox.Name = "disabledcommandsListBox";
            disabledcommandsListBox.Size = new Size(157, 154);
            disabledcommandsListBox.TabIndex = 26;
            // 
            // disablecommandButton
            // 
            disablecommandButton.Location = new Point(138, 439);
            disablecommandButton.Name = "disablecommandButton";
            disablecommandButton.Size = new Size(72, 45);
            disablecommandButton.TabIndex = 27;
            disablecommandButton.Text = "Disable Command";
            disablecommandButton.UseVisualStyleBackColor = true;
            // 
            // restorecommandButton
            // 
            restorecommandButton.Location = new Point(361, 439);
            restorecommandButton.Name = "restorecommandButton";
            restorecommandButton.Size = new Size(110, 45);
            restorecommandButton.TabIndex = 28;
            restorecommandButton.Text = "Enable Selected Command";
            restorecommandButton.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.BackColor = Color.FromArgb(220, 215, 201);
            label12.Location = new Point(55, 265);
            label12.Name = "label12";
            label12.Size = new Size(114, 15);
            label12.TabIndex = 29;
            label12.Text = "Enabled Commands";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.FromArgb(220, 215, 201);
            label13.Location = new Point(332, 265);
            label13.Name = "label13";
            label13.Size = new Size(117, 15);
            label13.TabIndex = 30;
            label13.Text = "Disabled Commands";
            // 
            // CommandBuilderMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(201, 198, 189);
            ClientSize = new Size(1042, 564);
            ControlBox = false;
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(restorecommandButton);
            Controls.Add(disablecommandButton);
            Controls.Add(disabledcommandsListBox);
            Controls.Add(panel1);
            Controls.Add(restartButton);
            Controls.Add(label7);
            Controls.Add(pictureBox8);
            Controls.Add(label6);
            Controls.Add(pictureBox6);
            Controls.Add(removeCommandButton);
            Controls.Add(commandListBox);
            Controls.Add(label5);
            Controls.Add(openCustomJson);
            Controls.Add(commanduseBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(commandtextBox);
            Controls.Add(costtextBox);
            Controls.Add(nametextBox);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox7);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CommandBuilderMenu";
            Text = "QuestMenu";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Button saveButton;
        private RichTextBox commanduseBox;
        private Button openCustomJson;
        private Label label5;
        private ListBox commandListBox;
        private Button removeCommandButton;
        private Label label6;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private Label label7;
        private Button restartButton;
        private Button holdkeyButton;
        private Panel panel1;
        private Button arightButton;
        private Button aleftButton;
        private Button aturnButton;
        private Button ahitkeyButton;
        private Button delayButton;
        private Button playsoundButton;
        private Button rightholdButton;
        private Button leftholdButton;
        private Button rightButton;
        private Button leftButton;
        private Button turnButton;
        private Button hitkeyButton;
        private Button aholdkeyButton;
        private Button aplaysoundButton;
        private Button arightholdButton;
        private Button aleftholdButton;
        private Button muteButton;
        private TextBox speedBox;
        private TextBox keyBox;
        private TextBox durBox;
        private TextBox directionBox;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private PictureBox pictureBox9;
        private PictureBox pictureBox5;
        private ListBox disabledcommandsListBox;
        private Button disablecommandButton;
        private Button restorecommandButton;
        private Label label12;
        private Label label13;
        private Button ahitkeyloopButton;
        private Button hitkeyloopButton;
        private Button amuteButton;
        private Button arightloopButton;
        private Button aleftloopButton;
        private Button rightloopButton;
        private Button leftloopButton;
        private Button mouseposButton;
        private TextBox mouseposTextBox;
        private Label label14;
    }
}