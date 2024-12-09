using Newtonsoft.Json;

namespace UiBot
{
    public static class QuickAddCommands
    {
        private static readonly string CommandsFilePath = Path.Combine("Data", "bin", "CustomCommands.json");

        public static void QuickWalk(Action updateListBoxCallback)
        {
            try
            {
                // Define the new walk command
                var newCommandName = "walk";
                var newCommand = new Command
                {
                    BitCost = 100,
                    Methods = new List<string> { "KH=W(5000)" }
                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the walk command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickWiggle(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "wiggle";
                var newCommand = new Command
                {
                    BitCost = 10,
                    Methods = new List<string>
                    {
                        "TM=L(20,5)",
                        "TM=R(20,5)",
                        "TM=L(20,5)",
                        "TM=R(20,5)",
                        "TM=L(20,5)",
                        "TM=R(20,5)",
                        "TM=L(20,5)",
                        "TM=R(20,5)"
                    }
                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickSpin(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "spin";
                var newCommand = new Command
                {
                    BitCost = 100,
                    Methods = new List<string> { "TM=RAND(5000,10)" }

                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickCrouch(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "crouch";
                var newCommand = new Command
                {
                    BitCost = 25,
                    Methods = new List<string> { "KP=C" }

                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickMDump(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "360magdump";
                var newCommand = new Command
                {
                    BitCost = 25,
                    Methods = new List<string>
                    {
                        "TMAsync=RAND(3000,10)",
                        "LCHoldAsync=3000"
                    }

                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickFireMode(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "firemode";
                var newCommand = new Command
                {
                    BitCost = 10,
                    Methods = new List<string>
                    {
                        "KP=B"
                    }

                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickHotMic(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "hotmic";
                var newCommand = new Command
                {
                    BitCost = 50,
                    Methods = new List<string>
                    {
                        "KH=K(5000)"
                    }

                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void QuickALTF4(Action updateListBoxCallback)
        {
            try
            {
                // Define the new wiggle command
                var newCommandName = "altf4";
                var newCommand = new Command
                {
                    BitCost = 2500,
                    Methods = new List<string>
                    {
                        "KHAsync=ALT(100)",
                        "KPAsync=F4"    
                    }

                };

                // Load existing commands from file
                var commands = LoadCommands();

                // Check if the command already exists
                if (commands.ContainsKey(newCommandName))
                {
                    MessageBox.Show($"The command '{newCommandName}' already exists. Command not added.",
                                    "Duplicate Command", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add or update the wiggle command
                commands[newCommandName] = newCommand;

                // Save the updated commands back to the file
                SaveCommandsToFile(commands);

                // Optional confirmation message
                if (!Properties.Settings.Default.isConfirmationDisabled)
                {
                    MessageBox.Show($"'{newCommandName}' command added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the ListBox via the provided callback
                updateListBoxCallback?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static Dictionary<string, Command> LoadCommands()
        {
            if (!File.Exists(CommandsFilePath))
            {
                return new Dictionary<string, Command>();
            }

            var json = File.ReadAllText(CommandsFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, Command>>(json) ?? new Dictionary<string, Command>();
        }

        private static void SaveCommandsToFile(Dictionary<string, Command> commands)
        {
            var json = JsonConvert.SerializeObject(commands, Formatting.Indented);
            File.WriteAllText(CommandsFilePath, json);
        }
    }
}
