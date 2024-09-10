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
        private readonly string _commandsFilePath = Path.Combine("Data", "bin", "CustomCommands.json");
        private readonly string _disabledCommandsFilePath = Path.Combine("Data", "bin", "DisabledCommands.json");
        private int mouseX;
        private int mouseY;

        public CommandBuilderMenu()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.CommandBuilderMenu_KeyDown);

            // Ensure the Data directory exists
            var directory = Path.GetDirectoryName(_commandsFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Load commands into the ListBoxes
            LoadCommandsIntoListBox();
            LoadDisabledCommandsIntoListBox();
            LoadUsageBox();

            // Register the SelectedIndexChanged event for the ListBox
            commandListBox.SelectedIndexChanged += commandListBox_SelectedIndexChanged;
            disabledcommandsListBox.SelectedIndexChanged += disabledcommandListBox_SelectedIndexChanged;

            // Wire up other event handlers
            removeCommandButton.Click += removeCommandButton_Click;
            disablecommandButton.Click += disablecommandButton_Click;
            restorecommandButton.Click += restorecommandButton_Click;

        }
        private void CommandBuilderMenu_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Shift key is pressed
            if (e.Shift)
            {
                // Get the current mouse position
                var mousePosition = Cursor.Position;
                mouseX = mousePosition.X;
                mouseY = mousePosition.Y;

                // Display the mouse position in the mouseposTextBox
                mouseposTextBox.Text = $"X: {mousePosition.X}, Y: {mousePosition.Y}";
            }
        }

        private void holdkeyButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" HoldKey=KeyIn(dur) ");
        }

        private void aholdkeyButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" HoldKeyAsync=KeyIn(dur) ");
        }

        private void hitkeyButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" HitKey=KeyIn ");
        }

        private void ahitkeyButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" HitKeyAsync=KeyIn ");
        }

        private void hitkeyloopButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" HitKeyLoop=KeyIn(dur,Speed) ");
        }

        private void ahitkeyloopButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" HitKeyLoopAsync=KeyIn(dur,Speed) ");
        }
        private void leftButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += " LeftClick ";
        }

        private void aleftButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += " LeftClickAsync ";
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += " RightClick ";
        }

        private void arightButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += " RightClickAsync ";
        }

        private void leftloopButton_Click(object sender, EventArgs e)
        {
            commandtextBox.Text += GetCommandText(" LeftClickLoop=KeyIn(dur,Speed) ");
        }

        private void aleftloopButton_Click(object sender, EventArgs e)
        {
            commandtextBox.Text += GetCommandText(" LeftClickLoopAsync=KeyIn(dur,Speed) ");
        }

        private void rightloopButton_Click(object sender, EventArgs e)
        {
            commandtextBox.Text += GetCommandText(" RightClickLoop=KeyIn(dur,Speed) ");
        }

        private void arightloopButton_Click(object sender, EventArgs e)
        {
            commandtextBox.Text += GetCommandText(" RightClickLoopAsync=KeyIn(dur,Speed) ");
        }

        private void turnButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" TurnMouse=Direction(dur,Speed) ");
        }

        private void aturnButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" TurnMouseAsync=Direction(dur,Speed) ");
        }

        private void playsoundButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += " PlaySoundClip=filename.wav ";
        }

        private void aplaysoundButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += " PlaySoundClipAsync=filename.wav ";
        }

        private void leftholdButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" LeftClickHold=dur ");
        }

        private void aleftholdButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" LeftClickHoldAsync=dur ");
        }

        private void rightholdButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" RightClickHold=dur ");
        }

        private void arightholdButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" RightClickHoldAsync=dur ");
        }

        private void delayButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" Delay=Speed ");
        }

        private void muteButton_Click(object sender, EventArgs e)
        {
            // Append the text to commandtextBox
            commandtextBox.Text += GetCommandText(" MuteVolume=dur ");
        }

        private void amuteButton_Click(object sender, EventArgs e)
        {
            commandtextBox.Text += GetCommandText(" MuteVolumeAsync=dur ");
        }

        private void mouseposButton_Click(object sender, EventArgs e)
        {
            commandtextBox.Text += GetCommandText($" MousePos({mouseX},{mouseY}) ");
        }

        private void disablecommandButton_Click(object sender, EventArgs e)
        {
            var selectedCommand = commandListBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCommand))
            {
                MessageBox.Show("Please select a command to disable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load existing commands and disabled commands
            var commands = LoadCommands();
            var disabledCommands = LoadDisabledCommands();

            if (commands.ContainsKey(selectedCommand))
            {
                // Move the command from active to disabled
                var command = commands[selectedCommand];
                commands.Remove(selectedCommand);
                disabledCommands[selectedCommand] = command;

                // Save the updated dictionaries back to their respective files
                SaveCommandsToFile(commands);
                SaveDisabledCommandsToFile(disabledCommands);

                // Update the ListBoxes
                LoadCommandsIntoListBox();
                LoadDisabledCommandsIntoListBox();

                // Clear the text boxes
                nametextBox.Clear();
                costtextBox.Clear();
                commandtextBox.Clear();

                MessageBox.Show("Command disabled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The selected command does not exist in the active commands list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void restorecommandButton_Click(object sender, EventArgs e)
        {
            var selectedCommand = disabledcommandsListBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCommand))
            {
                MessageBox.Show("Please select a command to restore.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load existing commands and disabled commands
            var commands = LoadCommands();
            var disabledCommands = LoadDisabledCommands();

            if (disabledCommands.ContainsKey(selectedCommand))
            {
                // Move the command from disabled to active
                var command = disabledCommands[selectedCommand];
                disabledCommands.Remove(selectedCommand);
                commands[selectedCommand] = command;

                // Save the updated dictionaries back to their respective files
                SaveCommandsToFile(commands);
                SaveDisabledCommandsToFile(disabledCommands);

                // Update the ListBoxes
                LoadCommandsIntoListBox();
                LoadDisabledCommandsIntoListBox();

                // Clear the text boxes
                nametextBox.Clear();
                costtextBox.Clear();
                commandtextBox.Clear();

                MessageBox.Show("Command restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The selected command does not exist in the disabled commands list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveDisabledCommandsToFile(Dictionary<string, Command> disabledCommands)
        {
            try
            {
                var json = JsonConvert.SerializeObject(disabledCommands, Formatting.Indented);
                File.WriteAllText(_disabledCommandsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving disabled commands: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // Split methods by spaces outside parentheses for saving
            var methods = SplitMethods(methodsText); // Split the text into individual methods

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

        // Helper method to split methods without breaking content inside parentheses
        private List<string> SplitMethods(string methodsText)
        {
            var methods = new List<string>();
            var currentMethod = "";
            int parenthesesDepth = 0;

            foreach (var c in methodsText)
            {
                if (c == '(')
                {
                    parenthesesDepth++;
                }
                else if (c == ')')
                {
                    parenthesesDepth--;
                }

                // Split only on spaces outside parentheses
                if (c == ' ' && parenthesesDepth == 0)
                {
                    if (!string.IsNullOrWhiteSpace(currentMethod))
                    {
                        methods.Add(currentMethod.Trim());
                        currentMethod = "";
                    }
                }
                else
                {
                    currentMethod += c;
                }
            }

            if (!string.IsNullOrWhiteSpace(currentMethod))
            {
                methods.Add(currentMethod.Trim());
            }

            return methods;
        }

        private string ReplaceCommasOutsideParentheses(string input, bool replaceWithSpace)
        {
            // Regex to match commas or spaces outside parentheses
            var pattern = @"(\s)(?=(?:[^\(\)]|\([^\(\)]*\))*$)";

            if (replaceWithSpace)
            {
                // Replace commas outside parentheses with spaces
                return System.Text.RegularExpressions.Regex.Replace(input, pattern, ",");
            }
            else
            {
                // Replace spaces outside parentheses with commas for proper saving
                return input.Replace(" ", ",");
            }
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
        private Dictionary<string, Command> LoadDisabledCommands()
        {
            if (!File.Exists(_disabledCommandsFilePath))
            {
                return new Dictionary<string, Command>();
            }

            var json = File.ReadAllText(_disabledCommandsFilePath);
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

        private void LoadDisabledCommandsIntoListBox()
        {
            disabledcommandsListBox.Items.Clear();
            var disabledCommands = LoadDisabledCommands();
            foreach (var DcommandName in disabledCommands.Keys)
            {
                disabledcommandsListBox.Items.Add(DcommandName);
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

                // Join the methods with spaces for display in the text box
                var methodsText = string.Join(" ", command.Methods);  // Join methods with spaces
                commandtextBox.Text = methodsText;
            }

            if (disabledcommandsListBox.SelectedIndex != -1)
            {
                disabledcommandsListBox.ClearSelected();

            }
        }
        private void disabledcommandListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedCommand = disabledcommandsListBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCommand))
            {
                return;
            }

            var commands = LoadDisabledCommands();
            if (commands.TryGetValue(selectedCommand, out Command command))
            {
                // Update text boxes with selected command details
                nametextBox.Text = selectedCommand;
                costtextBox.Text = command.BitCost.ToString();

                // Join the methods with spaces for display in the text box
                var methodsText = string.Join(" ", command.Methods);  // Join methods with spaces
                commandtextBox.Text = methodsText;
            }

            if (commandListBox.SelectedIndex != -1)
            {
                commandListBox.ClearSelected();

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

        private string GetCommandText(string commandTemplate)
        {
            string key = keyBox.Text.Trim();
            string dur = durBox.Text.Trim();
            string speed = speedBox.Text.Trim();
            string direction = directionBox.Text.Trim(); // Get the value from directionBox

            // Replace placeholders with actual values
            return commandTemplate
                .Replace("KeyIn", key)
                .Replace("dur", dur)
                .Replace("Speed", speed)
                .Replace("Direction", direction); // Replace Direction with the value from directionBox
        }

        private void LoadUsageBox()
        {
            string pathtoFile = Path.Combine("Data", "bin", "Custom Commands and Usage.txt");

            if (File.Exists(pathtoFile))
            {
                commanduseBox.Text = File.ReadAllText(pathtoFile);
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

    }
}
