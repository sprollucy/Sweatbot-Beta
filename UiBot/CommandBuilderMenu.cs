using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace UiBot
{
    public partial class CommandBuilderMenu : Form
    {
        private readonly string _commandsFilePath = Path.Combine("Data", "CustomCommands.json");

        public CommandBuilderMenu()
        {
            InitializeComponent();
            this.TopLevel = false;

            // Ensure the Data directory exists
            var directory = Path.GetDirectoryName(_commandsFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Load commands into the ListBox
            LoadCommandsIntoListBox();

            // Wire up the event handlers
            loadCommandButton.Click += loadCommandButton_Click;
            removeCommandButton.Click += removeCommandButton_Click;
        }

        private void QuestMenu_Load(object sender, EventArgs e)
        {
            LoadCommandsIntoListBox();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void commandtextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void costtextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void nametextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveCommand();
        }

        private void SaveCommand()
        {
            // Validate inputs
            string name = nametextBox.Text.Trim();
            string costText = costtextBox.Text.Trim();
            string methodsText = commandtextBox.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(costText) || string.IsNullOrEmpty(methodsText))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(costText, out int cost))
            {
                MessageBox.Show("Bit cost must be a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse methods (assuming comma-separated methods)
            var methods = methodsText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Create a new command
            var newCommand = new Command
            {
                BitCost = cost,
                Methods = new List<string>(methods)
            };

            // Load existing commands from file
            var commands = LoadCommands();

            // Add or update the command
            commands[name] = newCommand;

            // Save commands back to file
            SaveCommandsToFile(commands);
            
            MessageBox.Show("Command saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Update the ListBox
            LoadCommandsIntoListBox();
        }

        private Dictionary<string, Command> LoadCommands()
        {
            if (!File.Exists(_commandsFilePath))
            {
                return new Dictionary<string, Command>();
            }

            var json = File.ReadAllText(_commandsFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, Command>>(json) ?? new Dictionary<string, Command>();
        }

        private void SaveCommandsToFile(Dictionary<string, Command> commands)
        {
            try
            {
                var json = JsonConvert.SerializeObject(commands, Formatting.Indented);
                File.WriteAllText(_commandsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving commands: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadCommandsIntoListBox()
        {
            commandListBox.Items.Clear();
            var commands = LoadCommands();
            foreach (var commandName in commands.Keys)
            {
                commandListBox.Items.Add(commandName);
            }
        }

        private void commandListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCommand = commandListBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCommand))
            {
                return;
            }

            var commands = LoadCommands();
            if (commands.TryGetValue(selectedCommand, out Command command))
            {
                // Update text boxes with selected command details
                nametextBox.Text = selectedCommand;
                costtextBox.Text = command.BitCost.ToString();
                commandtextBox.Text = string.Join(",", command.Methods);
            }
        }


        private void openCustomJson_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(_commandsFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _commandsFilePath,
                        UseShellExecute = true // This will use the default application associated with the file type
                    });
                }
                else
                {
                    MessageBox.Show("The file 'CustomCommands.json' does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadCommandButton_Click(object sender, EventArgs e)
        {
            // Get the selected command from the ListBox
            var selectedCommand = commandListBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCommand))
            {
                return;
            }

            // Load the commands from the file
            var commands = LoadCommands();

            if (commands.TryGetValue(selectedCommand, out Command command))
            {
                // Populate text boxes with command details
                nametextBox.Text = selectedCommand;
                costtextBox.Text = command.BitCost.ToString();
                commandtextBox.Text = string.Join(",", command.Methods);
            }
            else
            {
                MessageBox.Show("The selected command does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void removeCommandButton_Click(object sender, EventArgs e)
        {
            var selectedCommand = commandListBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCommand))
            {
                return;
            }

            // Load existing commands from the file
            var commands = LoadCommands();

            if (commands.ContainsKey(selectedCommand))
            {
                // Remove the command from the dictionary
                commands.Remove(selectedCommand);

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Update the ListBox
                LoadCommandsIntoListBox();

                // Clear the text boxes
                nametextBox.Clear();
                costtextBox.Clear();
                commandtextBox.Clear();

                MessageBox.Show("Command removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The selected command does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
