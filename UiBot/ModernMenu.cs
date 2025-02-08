﻿using System.Drawing.Drawing2D;
using System.Reflection;
using Timer = System.Windows.Forms.Timer;

namespace UiBot
{
    public partial class ModernMenu : Form
    {
        private bool isEnlarged = false;
        public static bool isConnected;
        private int originalWidth;
        private int enlargedWidth;
        private TimeSpan transitionDuration = TimeSpan.FromMilliseconds(250); // Adjust the duration as needed
        private DateTime transitionStartTime;
        private Timer transitionTimer;
        private Color originalPictureBox2BackColor; // Store the original background color
        private Color originalSlideBackColor; // Store the original background color
        private bool isDragging = false;
        private Point offset;
        private ConnectMenu connectMenu;
        private SettingMenu settingMenu;
        private ControlMenu controlMenu;
        private TraderMenu traderMenu;
        private CommandBuilderMenu builderMenu;
        private bool isConnectMenuVisible = false;
        private bool isSettingMenuVisible = false;
        private bool isControlMenuVisible = false;
        private bool isTraderMenuVisible = false;
        private bool isCommandBuilderMenuVisible = false;
        private bool isLabelsSlidOut = false;
        private int labelsOriginalLeft;
        private int labelsTargetLeft;
        private Timer labelsSlideTimer;
        private MainBot bot;
        public static ModernMenu Instance { get; private set; }

        public ModernMenu()
        {
            InitializeComponent();
            bot = new MainBot();
            Instance = this; // Assign the static instance

            this.FormBorderStyle = FormBorderStyle.None;
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // Create a GraphicsPath with rounded corners
            GraphicsPath path = new GraphicsPath();
            int radius = 10; // Adjust the radius as needed
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            path.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - (radius * 2), rect.Top, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - (radius * 2), rect.Bottom - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            // Set the form's region to the rounded shape
            this.Region = new Region(path);
            // Mouse events for pictureBox10
            pictureBox10.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    offset = e.Location;
                }
            };

            pictureBox10.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point newLocation = this.PointToScreen(new Point(e.X - offset.X, e.Y - offset.Y));
                    this.Location = newLocation;
                }
            };

            pictureBox10.MouseUp += (s, e) =>
            {
                isDragging = false;
            };

            originalWidth = slideBar.Width;
            enlargedWidth = originalWidth + 130; // Adjust the enlarged width as needed
            transitionTimer = new Timer { Interval = 1 }; // Adjust the interval for smoother transition
            transitionTimer.Tick += TransitionTimer_Tick;
            originalPictureBox2BackColor = pictureBox2.BackColor; // Store the original background color
            originalSlideBackColor = slidebarHighlight1.BackColor; // Store the original background color
            labelsOriginalLeft = label1.Left;
            labelsTargetLeft = label1.Left + 130; // Adjust this value as needed
            labelsSlideTimer = new Timer { Interval = 1 }; // Adjust the interval for smoother animation
            labelsSlideTimer.Tick += LabelsSlideTimer_Tick;

            Action<PictureBox, PictureBox, Label> setMouseEventsSlide = (pictureBox, pictureBoxside, label) =>
            {
                EventHandler mouseEnter = (s, e) =>
                {
                    pictureBox.BackColor = Color.FromArgb(108, 108, 108);
                    pictureBoxside.BackColor = Color.FromArgb(156, 155, 151);
                    label.BackColor = Color.FromArgb(108, 108, 108);
                };

                EventHandler mouseLeave = (s, e) =>
                {
                    pictureBox.BackColor = originalSlideBackColor;
                    pictureBoxside.BackColor = originalPictureBox2BackColor;
                    label.BackColor = originalSlideBackColor;
                };

                pictureBox.MouseEnter += mouseEnter;
                pictureBox.MouseLeave += mouseLeave;
                pictureBoxside.MouseEnter += mouseEnter;
                pictureBoxside.MouseLeave += mouseLeave;
                label.MouseEnter += mouseEnter;
                label.MouseLeave += mouseLeave;
            };

            // Apply to your sidebar items
            setMouseEventsSlide(slidebarHighlight1, connectButton, connectLabel);
            setMouseEventsSlide(slidebarHighlight2, commandMenu, chatsettingLabel);
            setMouseEventsSlide(slidebarHighlight3, commandBuilder, commandBulderLabel);
            setMouseEventsSlide(slidebarHighlight4, eftTrader, traderLabel);
            setMouseEventsSlide(slidebarHighlight5, settingsButton, settingsLabel);

            connectLabel.Click += (s, e) => connectMenu_Click(s, e);
            slidebarHighlight1.Click += (s, e) => connectMenu_Click(s, e);
            chatsettingLabel.Click += (s, e) => commandMenu_Click(s, e);
            slidebarHighlight2.Click += (s, e) => commandMenu_Click(s, e);
            traderLabel.Click += (s, e) => pictureBox5_Click(s, e);
            slidebarHighlight4.Click += (s, e) => pictureBox5_Click(s, e);
            commandBulderLabel.Click += (s, e) => commandBuilder_Click(s, e);
            slidebarHighlight3.Click += (s, e) => commandBuilder_Click(s, e);
            settingsLabel.Click += (s, e) => settingsButton_Click(s, e);
            slidebarHighlight5.Click += (s, e) => settingsButton_Click(s, e);

            CheckStart();
            BetaLabel();
            // Attach event handler to detect setting changes
            PropertySaver();
        }

        private void BetaLabel()
        {
            string packageVersion = Assembly.GetExecutingAssembly()
.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0];
            versionLabel.Text = packageVersion;
            // Position the label at the bottom-right corner of the form
            versionLabel.Location = new Point(
                this.ClientSize.Width - versionLabel.Width,  // 10 is padding from the right edge
                this.ClientSize.Height - versionLabel.Height  // 10 is padding from the bottom edge
            );
        }

        public void CheckStart()
        {
            if (Properties.Settings.Default.isTraderMenuEnabled)
            {
                eftTrader.Visible = true; // Ensure the picture box is visible (optional, depending on your requirement)
                traderLabel.Visible = true;
                var traderResetInfoService = new TraderResetInfoService();
            }
            else
            {
                eftTrader.Visible = false; // Optionally hide the picture box if it's disabled
                traderLabel.Visible = false;
            }

        }

        public void PropertySaver()
        {
            Properties.Settings.Default.PropertyChanged += (sender, e) =>
            {
                // Ensure the setting being changed is of type boolean
                PropertyInfo prop = Properties.Settings.Default.GetType().GetProperty(e.PropertyName);
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    if (Properties.Settings.Default.isDebugOn)
                    {
                        Console.WriteLine($"Setting changed: {e.PropertyName}");
                        SettingsManager.SaveSettings(); // Save settings after change
                    }
                    else
                    {
                        SettingsManager.SaveSettings(); // Save settings after change
                    }
                }
            };
        }

        public void UpdateConnection()
        {
            if (isConnected)
            {
                conStatus.Text = "Connected";
                conStatus.ForeColor = Color.Green;

            }
            else
            {
                conStatus.Text = "Not Connected";
                conStatus.ForeColor = Color.Salmon;
            }

        }


        private void TransitionTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - transitionStartTime;
            double progress = elapsed.TotalMilliseconds / transitionDuration.TotalMilliseconds;
            int newWidth = isEnlarged
                ? (int)(originalWidth + (enlargedWidth - originalWidth) * progress)
                : (int)(enlargedWidth - (enlargedWidth - originalWidth) * progress);

            newWidth = Math.Min(Math.Max(newWidth, originalWidth), enlargedWidth);
            slideBar.Width = newWidth;
            slidebarHighlight1.Width = newWidth;
            slidebarHighlight2.Width = newWidth;
            slidebarHighlight3.Width = newWidth;
            slidebarHighlight4.Width = newWidth;
            slidebarHighlight5.Width = newWidth;

            if (progress >= 1.0)
            {
                transitionTimer.Stop();
            }
        }

        private void LabelsSlideTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - transitionStartTime;
            double progress = elapsed.TotalMilliseconds / transitionDuration.TotalMilliseconds;
            int newLeft = isLabelsSlidOut
                ? (int)(labelsOriginalLeft + (labelsTargetLeft - labelsOriginalLeft) * progress)
                : (int)(labelsTargetLeft - (labelsTargetLeft - labelsOriginalLeft) * progress);

            foreach (Control control in Controls)
            {
                if (control is Label label)
                {
                    if (label.Name == "label1" || label.Name == "connectLabel" || label.Name == "chatsettingLabel" ||  label.Name == "traderLabel" || label.Name == "label5" || label.Name == "label6" || label.Name == "label6" || label.Name == "label7" || label.Name == "settingsLabel" || label.Name == "commandBulderLabel")
                    {
                        label.Left = newLeft;
                    }
                }
            }

            if (progress >= 1.0)
            {
                labelsSlideTimer.Stop();
            }
        }


        private void menuButton_Click(object sender, EventArgs e)
        {
            if (!transitionTimer.Enabled)
            {
                transitionStartTime = DateTime.Now;
                transitionTimer.Start();
                isEnlarged = !isEnlarged;
            }
            if (!labelsSlideTimer.Enabled)
            {
                transitionStartTime = DateTime.Now;
                labelsSlideTimer.Start();
                isLabelsSlidOut = !isLabelsSlidOut;
            }
        }

        private void closeBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void connectMenu_Click(object sender, EventArgs e)
        {

            HideOpenMenu(); // Hide the current open menu, if any
            ShowConnectMenu();
            if (isEnlarged)
            {
                menuButton_Click(sender, e);
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            if (isEnlarged)
            {
                menuButton_Click(sender, e);
            }

            HideOpenMenu(); // Hide the current open menu, if any
            ShowSettingMenu();
        }

        // Define methods to show/hide ConnectMenu and SettingMenu
        private void ShowConnectMenu()
        {
            if (!isConnectMenuVisible)
            {
                if (connectMenu == null || connectMenu.IsDisposed)
                {
                    connectMenu = new ConnectMenu();
                    connectMenu.Dock = DockStyle.Fill;
                    connectMenu.Location = new Point(-connectMenu.Width, 0);
                }
                this.Controls.Add(connectMenu);
                connectMenu.Show();
                isConnectMenuVisible = true;
                currentTab.Text = "Connection Menu";
            }
        }

        private void HideConnectMenu()
        {
            if (isConnectMenuVisible)
            {
                this.Controls.Remove(connectMenu);
                connectMenu.Hide();
                isConnectMenuVisible = false;
            }
        }

        private void ShowSettingMenu()
        {
            if (!isSettingMenuVisible)
            {
                if (settingMenu == null || settingMenu.IsDisposed)
                {
                    settingMenu = new SettingMenu();
                    settingMenu.Dock = DockStyle.Fill;
                    settingMenu.Location = new Point(-settingMenu.Width, 0);
                }
                this.Controls.Add(settingMenu);
                settingMenu.Show();
                isSettingMenuVisible = true;
                currentTab.Text = "Settings";
            }
        }

        private void HideSettingMenu()
        {
            if (isSettingMenuVisible)
            {
                this.Controls.Remove(settingMenu);
                settingMenu.Hide();
                isSettingMenuVisible = false;
            }
        }
        private void ShowTraderMenu()
        {
            if (!isTraderMenuVisible)
            {
                if (traderMenu == null || traderMenu.IsDisposed)
                {
                    traderMenu = new TraderMenu();
                    traderMenu.Dock = DockStyle.Fill;
                    traderMenu.Location = new Point(-traderMenu.Width, 0);
                }
                this.Controls.Add(traderMenu);
                traderMenu.Show();
                isTraderMenuVisible = true;
                currentTab.Text = "Trader Menu";

            }
        }

        private void HideTraderMenu()
        {
            if (isTraderMenuVisible)
            {
                this.Controls.Remove(traderMenu);
                traderMenu.Hide();
                isTraderMenuVisible = false;
            }
        }

        private void ShowControlMenu()
        {
            if (!isControlMenuVisible)
            {
                if (controlMenu == null || controlMenu.IsDisposed)
                {
                    controlMenu = new ControlMenu();
                    controlMenu.Dock = DockStyle.Fill;
                    controlMenu.Location = new Point(-controlMenu.Width, 0);
                }
                this.Controls.Add(controlMenu);
                controlMenu.Show();
                isControlMenuVisible = true;
                currentTab.Text = "Chat Settings";

            }
        }

        private void HideControlMenu()
        {
            if (isControlMenuVisible)
            {
                this.Controls.Remove(controlMenu); // Change 'commandMenu' to 'controlMenu'
                controlMenu.Hide(); // Change 'commandMenu' to 'controlMenu'
                isControlMenuVisible = false;
            }
        }
        private void ShowCommandBuilderMenu()
        {
            if (!isCommandBuilderMenuVisible)
            {
                if (builderMenu == null || builderMenu.IsDisposed)
                {
                    builderMenu = new CommandBuilderMenu();
                    builderMenu.Dock = DockStyle.Fill;
                    builderMenu.Location = new Point(-builderMenu.Width, 0);
                }
                this.Controls.Add(builderMenu);
                builderMenu.Show();
                isCommandBuilderMenuVisible = true;
                currentTab.Text = "Command Builder";

            }
        }

        private void HideCommandBuilderMenu()
        {
            if (isCommandBuilderMenuVisible)
            {
                this.Controls.Remove(builderMenu); // Change 'commandMenu' to 'controlMenu'
                builderMenu.Hide(); // Change 'commandMenu' to 'controlMenu'
                builderMenu.Dispose();
                isCommandBuilderMenuVisible = false;
            }
        }

        // Method to hide the currently open menu
        private void HideOpenMenu()
        {
            if (isConnectMenuVisible)
            {
                HideConnectMenu();
            }
            else if (isSettingMenuVisible)
            {
                HideSettingMenu();
            }
            else if (isControlMenuVisible)
            {
                HideControlMenu();
            }
            else if (isTraderMenuVisible)
            {
                HideTraderMenu();
            }
            else if (isCommandBuilderMenuVisible)
            {
                HideCommandBuilderMenu();
            }

        }

        private void commandMenu_Click(object sender, EventArgs e)
        {
            if (isEnlarged)
            {
                menuButton_Click(sender, e);
            }

            HideOpenMenu(); // Hide the current open menu, if any
            ShowControlMenu();
        }


        private async void ModernMenu_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isUpdateCheckEnabled)
            {
                UpdateCheck updateChecker = new UpdateCheck();
                await updateChecker.CheckForUpdatesAsync();
            }
            ShowConnectMenu();
            // Preload ControlMenu but keep it hidden
            if (controlMenu == null || controlMenu.IsDisposed)
            {
                controlMenu = new ControlMenu();
                controlMenu.Dock = DockStyle.Fill;
                controlMenu.Visible = false; // Keep it hidden on load
                this.Controls.Add(controlMenu);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (isEnlarged)
            {
                menuButton_Click(sender, e);
            }

            HideOpenMenu();
            ShowTraderMenu();
        }

        private void commandBuilder_Click(object sender, EventArgs e)
        {
            if (isEnlarged)
            {
                menuButton_Click(sender, e);
            }
            HideOpenMenu();
            ShowCommandBuilderMenu();
        }
    }
}

