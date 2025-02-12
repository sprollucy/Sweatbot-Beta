using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Media;
using System.Reflection;
using System.Windows.Forms;

namespace UiBot
{
    public partial class UpdateDialogForm : Form
    {
        public bool UserWantsToCheck { get; private set; }
        private bool isDragging = false;
        private Point offset;

        public UpdateDialogForm(string latestVersion)
        {

            InitializeComponent();
            SystemSounds.Exclamation.Play();
            GraphicsPath path = new GraphicsPath();
            int radius = 12; // Adjust the radius as needed
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            path.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - (radius * 2), rect.Top, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - (radius * 2), rect.Bottom - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            // Set the form's region to the rounded shape
            this.Region = new Region(path);
            string packageVersion = Assembly.GetExecutingAssembly()
.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0];

            messageLabel.Text = $"A new version {latestVersion} is available. Do you want to check it out?";
            versionLabel.Text = "Current Version: " + packageVersion;
            // Center the messageLabel
            messageLabel.Left = (this.ClientSize.Width - messageLabel.Width) / 2;

            // Center the versionLabel, ensuring there's a gap between the two labels
            versionLabel.Left = (this.ClientSize.Width - versionLabel.Width) / 2;
            panel1.Left = (this.ClientSize.Width - panel1.Width) / 2;
            updateLabel.Left = (this.ClientSize.Width - updateLabel.Width) / 2;

            versionLabel.Top = messageLabel.Bottom + 10; // Adjust the 10 as necessary for the gap

            titleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    offset = e.Location;
                }
            };

            titleBar.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point newLocation = this.PointToScreen(new Point(e.X - offset.X, e.Y - offset.Y));
                    this.Location = newLocation;
                }
            };

            titleBar.MouseUp += (s, e) =>
            {
                isDragging = false;
            };

            updateLabel.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    offset = e.Location;
                }
            };

            updateLabel.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point newLocation = this.PointToScreen(new Point(e.X - offset.X, e.Y - offset.Y));
                    this.Location = newLocation;
                }
            };

            updateLabel.MouseUp += (s, e) =>
            {
                isDragging = false;
            };
        }

        private void checkupdateButton_Click(object sender, EventArgs e)
        {
            UserWantsToCheck = true;
            Close();
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            UserWantsToCheck = false;
            Close();
        }

        private void autoupdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                string updaterPath = @"Sweatbot Updater.exe";
                Process.Start(new ProcessStartInfo
                {
                    FileName = updaterPath,
                    UseShellExecute = true
                });

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting Updater.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void closeBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
