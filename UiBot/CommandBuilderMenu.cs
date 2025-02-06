
using System.Diagnostics;

using Newtonsoft.Json;
using static UiBot.ConnectMenu;

namespace UiBot
{
    public partial class CommandBuilderMenu : Form
    {
        private static CustomCommandHandler commandHandler;

        private string _commandsFilePath;
        private readonly string _disabledCommandsFilePath = Path.Combine("Data", "bin", "DisabledCommands.json");
        private int mouseX;
        private int mouseY;
        private bool isExpanded;

        public CommandBuilderMenu()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.CommandBuilderMenu_KeyDown);
            isExpanded = false;
            inspectorPanel.Height = 0;
            usagePanel.Top = inspectorPanel.Bottom + 28;

            commandHandler = new CustomCommandHandler(_commandsFilePath);


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

            confCheckBox.Checked = Properties.Settings.Default.isConfirmationDisabled;
            // Load profiles into the ComboBox
            LoadProfilesIntoComboBox();

            // Load commands from the default profile
            LoadCommandsFromProfile(profileComboBox.SelectedItem.ToString());

            // Register the event for profile selection change
            profileComboBox.SelectedIndexChanged += ProfileComboBox_SelectedIndexChanged;
        }

        private void showhideInspector_Click(object sender, EventArgs e)
        {
            if (isExpanded)
            {
                inspectorPanel.Height = 0;
                usagePanel.Top = inspectorPanel.Bottom + 25;

            }
            else
            {
                inspectorPanel.Height = 283;
                usagePanel.Top = inspectorPanel.Bottom + 10;

            }
            isExpanded = !isExpanded;
        }


        private void LoadProfilesIntoComboBox()
        {
            profileComboBox.Items.Clear();
            string profilesDirectory = Path.Combine("Data", "Profiles");

            if (!Directory.Exists(profilesDirectory))
            {
                Directory.CreateDirectory(profilesDirectory);
            }

            // Get all the JSON files in the Profiles directory
            string[] profileFiles = Directory.GetFiles(profilesDirectory, "*.json");

            // Add the file names (without extension) to the ComboBox
            foreach (string file in profileFiles)
            {
                string profileName = Path.GetFileNameWithoutExtension(file);
                profileComboBox.Items.Add(profileName);
            }

            // Select the last used profile if available
            string lastUsedProfile = Properties.Settings.Default.LastUsedProfile;
            if (profileComboBox.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(lastUsedProfile) && profileComboBox.Items.Contains(lastUsedProfile))
                {
                    profileComboBox.SelectedItem = lastUsedProfile; // Set the last used profile
                }
                else
                {
                    // Default to the first profile if no saved preference
                    profileComboBox.SelectedIndex = 0;
                }

                string selectedProfile = profileComboBox.SelectedItem.ToString();

                // Load the commands from the selected profile
                LoadCommandsFromProfile(selectedProfile);
            }
        }

        private void ProfileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProfile = profileComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedProfile))
            {
                return;
            }
            Properties.Settings.Default.LastUsedProfile = profileComboBox.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            // Load commands from the selected profile
            LoadCommandsFromProfile(selectedProfile);
        }

        private void LoadCommandsFromProfile(string profileName)
        {
            // Set the _commandsFilePath dynamically based on profileName
            _commandsFilePath = Path.Combine("Data", "Profiles", $"{profileName}.json");

            if (!File.Exists(_commandsFilePath))
            {
                MessageBox.Show($"Profile '{profileName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load commands from the selected profile file
            var commands = LoadCommandsFromFile(_commandsFilePath);

            // Clear the current ListBox
            commandListBox.Items.Clear();

            // Populate the ListBox with the loaded commands
            foreach (var commandName in commands.Keys)
            {
                commandListBox.Items.Add(commandName);
            }

            // Clear the text boxes
            nametextBox.Clear();
            costtextBox.Clear();
            commandtextBox.Clear();
        }

        private Dictionary<string, Command> LoadCommandsFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, Command>>(json) ?? new Dictionary<string, Command>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new Dictionary<string, Command>();
            }
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

        private void AppendCommandText(string command)
        {
            commandtextBox.Text += GetCommandText(command);
        }

        // Synchronous commands
        private void holdkeyButton_Click(object sender, EventArgs e) => AppendCommandText(" KH=KeyIn(dur) ");
        private void hitkeyButton_Click(object sender, EventArgs e) => AppendCommandText(" KP=KeyIn ");
        private void hitkeyloopButton_Click(object sender, EventArgs e) => AppendCommandText(" KPLoop=KeyIn(dur,Speed) ");
        private void leftButton_Click(object sender, EventArgs e) => commandtextBox.Text += " LC ";
        private void rightButton_Click(object sender, EventArgs e) => commandtextBox.Text += " RC ";
        private void leftloopButton_Click(object sender, EventArgs e) => AppendCommandText(" LCLoop=(dur,Speed) ");
        private void rightloopButton_Click(object sender, EventArgs e) => AppendCommandText(" RCLoop=(dur,Speed) ");
        private void turnButton_Click(object sender, EventArgs e) => AppendCommandText(" TM=Direction(dur,Speed) ");
        private void playsoundButton_Click(object sender, EventArgs e) => commandtextBox.Text += " PSOUND=filename.wav ";
        private void leftholdButton_Click(object sender, EventArgs e) => AppendCommandText(" LCHOLD=dur ");
        private void rightholdButton_Click(object sender, EventArgs e) => AppendCommandText(" RCHOLD=dur ");
        private void delayButton_Click(object sender, EventArgs e) => AppendCommandText(" Delay=Speed ");
        private void muteButton_Click(object sender, EventArgs e) => AppendCommandText(" MuteVol=dur ");
        private void mouseposButton_Click(object sender, EventArgs e) => commandtextBox.Text += GetCommandText($" MPos({mouseX},{mouseY}) ");
        private void pixelateButton_Click(object sender, EventArgs e) => AppendCommandText(" PixelateScreen=dur ");

        // Asynchronous commands
        private void aholdkeyButton_Click(object sender, EventArgs e) => AppendCommandText(" KHAsync=KeyIn(dur) ");
        private void ahitkeyButton_Click(object sender, EventArgs e) => AppendCommandText(" KPAsync=KeyIn ");
        private void ahitkeyloopButton_Click(object sender, EventArgs e) => AppendCommandText(" KPLoopAsync=KeyIn(dur,Speed) ");
        private void aleftButton_Click(object sender, EventArgs e) => commandtextBox.Text += " LCAsync ";
        private void arightButton_Click(object sender, EventArgs e) => commandtextBox.Text += " RCAsync ";
        private void aleftloopButton_Click(object sender, EventArgs e) => AppendCommandText(" LCLoopAsync=(dur,Speed) ");
        private void arightloopButton_Click(object sender, EventArgs e) => AppendCommandText(" RCLoopAsync=(dur,Speed) ");
        private void aturnButton_Click(object sender, EventArgs e) => AppendCommandText(" TMAsync=Direction(dur,Speed) ");
        private void aplaysoundButton_Click(object sender, EventArgs e) => commandtextBox.Text += " PSOUNDAsync=filename.wav ";
        private void aleftholdButton_Click(object sender, EventArgs e) => AppendCommandText(" LCHoldAsync=dur ");
        private void arightholdButton_Click(object sender, EventArgs e) => AppendCommandText(" RCHoldAsync=dur ");
        private void adelayButton_Click(object sender, EventArgs e) => AppendCommandText(" DelayAsync=Speed ");
        private void amuteButton_Click(object sender, EventArgs e) => AppendCommandText(" MuteVolAsync=dur ");
        private void amouseposButton_Click(object sender, EventArgs e) => commandtextBox.Text += GetCommandText($" MPosAsync({mouseX},{mouseY}) ");


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

                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show("Command disabled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show("Command restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

            // Load existing commands from the correct profile file
            var commands = LoadCommands();  // _commandsFilePath should be set when a profile is selected

            // Add or update the command
            commands[name] = newCommand;

            // Save commands back to the correct file
            SaveCommandsToFile(commands);

            if (!Properties.Settings.Default.isConfirmationDisabled)
            {
                MessageBox.Show("Command saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Update the ListBox to reflect changes
            LoadCommandsIntoListBox();
        }


        private void SaveCommandsToProfile(string profileName, Dictionary<string, Command> commands)
        {
            string profileFilePath = Path.Combine("Data", "Profiles", $"{profileName}.json");

            try
            {
                var json = JsonConvert.SerializeObject(commands, Formatting.Indented);
                File.WriteAllText(profileFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                // Ensure _commandsFilePath is set by selecting a profile first
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
                    MessageBox.Show($"The file '{Path.GetFileName(_commandsFilePath)}' does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openDisabledJson_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(_disabledCommandsFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _disabledCommandsFilePath,
                        UseShellExecute = true // This will use the default application associated with the file type
                    });
                }
                else
                {
                    MessageBox.Show("The file 'DisabledCommands.json' does not exist.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show("Command removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private void confCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isConfirmationDisabled = confCheckBox.Checked;
            Properties.Settings.Default.Save();

        }

        private void walkButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickWalk(LoadCommandsIntoListBox);
        }

        private void spinButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickSpin(LoadCommandsIntoListBox);

        }

        private void wiggleButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickWiggle(LoadCommandsIntoListBox);

        }

        private void crouchButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickCrouch(LoadCommandsIntoListBox);

        }

        private void firemodeButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickFireMode(LoadCommandsIntoListBox);

        }

        private void mdumpButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickMDump(LoadCommandsIntoListBox);

        }

        private void hotmicButton_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickHotMic(LoadCommandsIntoListBox);

        }

        private void alftf4Button_Click(object sender, EventArgs e)
        {
            QuickAddCommands.QuickALTF4(LoadCommandsIntoListBox);

        }

        private void newProfileButton_Click(object sender, EventArgs e)
        {
            // Define the full path to the "Profiles" folder in your project
            string profilesDirectory = Path.Combine(Application.StartupPath, "Data", "Profiles");

            // Make sure the directory exists; if not, create it
            if (!Directory.Exists(profilesDirectory))
            {
                Directory.CreateDirectory(profilesDirectory);
            }

            // Open a SaveFileDialog to let the user select the file name for the new profile
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the initial directory to the Profiles folder
                saveFileDialog.InitialDirectory = profilesDirectory;

                saveFileDialog.Filter = "JSON Files (*.json)|*.json"; // Ensure only .json files are shown
                saveFileDialog.DefaultExt = ".json";  // Default extension is .json
                saveFileDialog.AddExtension = true;   // Add extension if not entered

                // Show the dialog and check if the user selected a file
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Ensure the file is saved within the 'Profiles' directory
                    string profileFilePath = saveFileDialog.FileName;

                    try
                    {
                        // Create an empty command list and save it to the new profile file
                        var emptyCommands = new Dictionary<string, Command>();  // Empty command list
                        File.WriteAllText(profileFilePath, JsonConvert.SerializeObject(emptyCommands, Formatting.Indented));

                        // Optionally, you can update the profileComboBox and select the newly created profile
                        profileComboBox.Items.Add(Path.GetFileNameWithoutExtension(profileFilePath));  // Add profile name to the ComboBox
                        profileComboBox.SelectedItem = Path.GetFileNameWithoutExtension(profileFilePath); // Select the new profile

                        MessageBox.Show("New profile created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while creating the profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void remProfileButton_Click(object sender, EventArgs e)
        {
            if (profileComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a profile to remove.", "No Profile Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedProfile = profileComboBox.SelectedItem.ToString();
            string profilesDirectory = Path.Combine(Application.StartupPath, "Data", "Profiles");
            string profileFilePath = Path.Combine(profilesDirectory, selectedProfile + ".json");

            // Confirm deletion
            DialogResult result = MessageBox.Show($"Are you sure you want to delete the profile '{selectedProfile}'?",
                                                  "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (File.Exists(profileFilePath))
                    {
                        File.Delete(profileFilePath);
                        profileComboBox.Items.Remove(selectedProfile); // Remove from ComboBox

                        MessageBox.Show("Profile deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Profile file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            var selectedCommand = commandListBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCommand))
            {
                MessageBox.Show("Please select a command first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userBits = 100000; // Simulate user bits

            if (commandHandler.CanExecuteCommand(selectedCommand, userBits))
            {
                var command = commandHandler.GetCommand(selectedCommand);
                if (command != null)
                {
                    // Simulate command execution
                    commandHandler.ExecuteCommandAsync(selectedCommand, null, "testChannel"); // Channel is not used here
                    Console.WriteLine($"Command executed locally: {selectedCommand}");
                }
            }
            else
            {
                Console.WriteLine($"Cannot execute command '{selectedCommand}'. Command does not exist or is broken.");
            }
        }

    }
}
