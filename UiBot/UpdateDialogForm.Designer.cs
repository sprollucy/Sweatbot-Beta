namespace UiBot
{
    partial class UpdateDialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDialogForm));
            checkupdateButton = new Button();
            noButton = new Button();
            autoupdateButton = new Button();
            messageLabel = new Label();
            versionLabel = new Label();
            closeBox = new PictureBox();
            titleBar = new PictureBox();
            panel1 = new Panel();
            updateLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)closeBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)titleBar).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // checkupdateButton
            // 
            checkupdateButton.BackColor = SystemColors.ButtonFace;
            checkupdateButton.FlatAppearance.BorderSize = 0;
            checkupdateButton.FlatStyle = FlatStyle.Flat;
            checkupdateButton.Location = new Point(0, 29);
            checkupdateButton.Name = "checkupdateButton";
            checkupdateButton.Size = new Size(75, 23);
            checkupdateButton.TabIndex = 0;
            checkupdateButton.Text = "Yes";
            checkupdateButton.UseVisualStyleBackColor = false;
            checkupdateButton.Click += checkupdateButton_Click;
            // 
            // noButton
            // 
            noButton.BackColor = SystemColors.ButtonFace;
            noButton.FlatAppearance.BorderSize = 0;
            noButton.FlatStyle = FlatStyle.Flat;
            noButton.Location = new Point(81, 29);
            noButton.Name = "noButton";
            noButton.Size = new Size(75, 23);
            noButton.TabIndex = 1;
            noButton.Text = "No";
            noButton.UseVisualStyleBackColor = false;
            noButton.Click += noButton_Click;
            // 
            // autoupdateButton
            // 
            autoupdateButton.BackColor = SystemColors.ButtonFace;
            autoupdateButton.FlatAppearance.BorderSize = 0;
            autoupdateButton.FlatStyle = FlatStyle.Flat;
            autoupdateButton.Location = new Point(0, 0);
            autoupdateButton.Name = "autoupdateButton";
            autoupdateButton.Size = new Size(156, 23);
            autoupdateButton.TabIndex = 2;
            autoupdateButton.Text = "Run Auto Updater";
            autoupdateButton.UseVisualStyleBackColor = false;
            autoupdateButton.Click += autoupdateButton_Click;
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.BackColor = Color.FromArgb(130, 129, 125);
            messageLabel.ForeColor = SystemColors.ControlText;
            messageLabel.Location = new Point(33, 23);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(334, 15);
            messageLabel.TabIndex = 3;
            messageLabel.Text = "A new version 0.0.0.0 is available. Do you want to check it out?";
            messageLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.BackColor = Color.FromArgb(130, 129, 125);
            versionLabel.Location = new Point(174, 47);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(38, 15);
            versionLabel.TabIndex = 4;
            versionLabel.Text = "label2";
            versionLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // closeBox
            // 
            closeBox.BackColor = Color.FromArgb(63, 78, 79);
            closeBox.Image = (Image)resources.GetObject("closeBox.Image");
            closeBox.Location = new Point(379, 0);
            closeBox.Name = "closeBox";
            closeBox.Size = new Size(20, 20);
            closeBox.SizeMode = PictureBoxSizeMode.Zoom;
            closeBox.TabIndex = 21;
            closeBox.TabStop = false;
            closeBox.Click += closeBox_Click;
            // 
            // titleBar
            // 
            titleBar.BackColor = Color.FromArgb(63, 78, 79);
            titleBar.Location = new Point(0, 0);
            titleBar.Name = "titleBar";
            titleBar.Size = new Size(400, 20);
            titleBar.TabIndex = 22;
            titleBar.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(autoupdateButton);
            panel1.Controls.Add(checkupdateButton);
            panel1.Controls.Add(noButton);
            panel1.Location = new Point(111, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(156, 53);
            panel1.TabIndex = 24;
            // 
            // updateLabel
            // 
            updateLabel.AutoSize = true;
            updateLabel.BackColor = Color.FromArgb(63, 78, 79);
            updateLabel.Font = new Font("Constantia", 12F, FontStyle.Bold);
            updateLabel.ForeColor = Color.Salmon;
            updateLabel.Location = new Point(132, 1);
            updateLabel.Name = "updateLabel";
            updateLabel.Size = new Size(136, 19);
            updateLabel.TabIndex = 121;
            updateLabel.Text = "UPDATE FOUND";
            // 
            // UpdateDialogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(130, 129, 125);
            ClientSize = new Size(400, 141);
            ControlBox = false;
            Controls.Add(updateLabel);
            Controls.Add(versionLabel);
            Controls.Add(messageLabel);
            Controls.Add(panel1);
            Controls.Add(closeBox);
            Controls.Add(titleBar);
            FormBorderStyle = FormBorderStyle.None;
            Name = "UpdateDialogForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "UpdateDialogForm";
            ((System.ComponentModel.ISupportInitialize)closeBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)titleBar).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button checkupdateButton;
        private Button noButton;
        private Button autoupdateButton;
        private Label messageLabel;
        private Label versionLabel;
        private PictureBox closeBox;
        private PictureBox titleBar;
        private Panel panel1;
        private Label updateLabel;
    }
}