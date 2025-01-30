using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TwitchLib.Client;

public class CustomCommandHandler
{
    private readonly Dictionary<string, Command> _commands;
    private readonly Dictionary<string, Action<TwitchClient, string, string>> _syncMethodMap;
    private readonly Dictionary<string, Func<TwitchClient, string, string, Task>> _asyncMethodMap;

    private Random random = new Random();  // Class-level Random object
    private DateTime _lastCommandExecutionTime = DateTime.Now;  // Tracks when the last command was executed

    string filePath = Path.Combine("Data", "bin", "CustomCommands.json");

    // User32.dll imports for mouse and keyboard events
    [DllImport("user32.dll", SetLastError = true)]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    // Event constants for mouse and keyboard actions
    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;
    public const int MOUSEEVENTF_MOVE = 0x0001;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const int MOUSEEVENTF_RIGHTUP = 0x0010;
    private const int KEYEVENTF_KEYDOWN = 0x0000;
    private const int KEYEVENTF_KEYUP = 0x0002;
    private const int VK_VOLUME_MUTE = 0xAD;
    private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;


    public CustomCommandHandler(string filePath)
    {
        _commands = LoadCommandsFromFile(filePath);

        // Sync method map
        _syncMethodMap = new Dictionary<string, Action<TwitchClient, string, string>>
    {
        { "kh", HoldKey },
        { "kp", HitKey },
        { "kploop", HitKeyLoop },
        { "lc", LeftClick },
        { "rc", RightClick },
        { "tm", TurnMouse },
        { "mpos", MousePos },
        { "psound", PlaySoundClip },
        { "rchold", RightClickHold },
        { "lchold", LeftClickHold },
        { "mutevol", MuteVolume },
        { "delay", Delay },
        { "lcloop", LeftClickLoop },
        { "rcloop", RightClickLoop }
    };

        // Async method map
        _asyncMethodMap = new Dictionary<string, Func<TwitchClient, string, string, Task>>
    {
        { "khasync", HoldKeyAsync },
        { "kpasync", HitKeyAsync },
        { "kploopasync", HitKeyLoopAsync },
        { "lcasync", LeftClickAsync },
        { "rcasync", RightClickAsync },
        { "tmasync", TurnMouseAsync },
        { "mposasync", MousePosAsync },
        { "psoundasync", PlaySoundClipAsync },
        { "rcholdasync", RightClickHoldAsync },
        { "lcholdasync", LeftClickHoldAsync },
        { "mutevolasync", MuteVolumeAsync },
        { "delayasync", DelayAsync },
        { "lcloopasync", LeftClickLoopAsync },
        { "rcloopasync", RightClickLoopAsync }
    };
    }

    public Dictionary<string, int> GetAllCommandsWithCosts()
    {
        return _commands.ToDictionary(
            cmd => cmd.Key,
            cmd => cmd.Value.BitCost
        );
    }

    public static Dictionary<string, Command> LoadCommandsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "{}");
            Console.WriteLine("CustomCommands.json not found. Creating file");
        }

        var json = File.ReadAllText(filePath);
        var commands = JsonConvert.DeserializeObject<Dictionary<string, Command>>(json);

        return commands;
    }
    public void ReloadCommands(string filePath)
    {
        _commands.Clear();
        var updatedCommands = LoadCommandsFromFile(filePath);
        foreach (var cmd in updatedCommands)
        {
            _commands[cmd.Key] = cmd.Value;
        }
    }

    public Command GetCommand(string commandName)
    {
        _commands.TryGetValue(commandName.ToLower(), out var command);
        return command;
    }
    public List<Command> GetCommandsByBitCost(int bitCost)
    {
        // Filter commands whose BitCost matches the provided amount
        return _commands.Values.Where(cmd => cmd.BitCost == bitCost).ToList();
    }

    public bool CanExecuteCommand(string commandName, int userBits)
    {
        var command = GetCommand(commandName);
        return command != null && userBits >= command.BitCost;
    }

    public async Task ExecuteCommandAsync(string commandName, TwitchClient client, string channel)
    {
        var command = GetCommand(commandName);
        if (command == null)
        {
            Console.WriteLine($"Command '{commandName}' not found.");
            return;
        }

        if (command.Methods == null || command.Methods.Count == 0)
        {
            Console.WriteLine($"No methods defined for command '{commandName}'.");
            return;
        }

        List<Task> runningTasks = new List<Task>();  // List to track asynchronous tasks

        foreach (var methodName in command.Methods)
        {
            string method;
            string parameters = null;

            // Check for '=' format
            if (methodName.Contains("="))
            {
                var commandParts = methodName.Split(new[] { '=' }, 2);
                method = commandParts[0].Trim().ToLower();
                parameters = commandParts.Length > 1 ? commandParts[1].Trim() : null;
            }
            // Check for '()' format
            else
            {
                var methodMatch = Regex.Match(methodName, @"([a-zA-Z]+)\(([^)]*)\)");
                if (methodMatch.Success)
                {
                    method = methodMatch.Groups[1].Value.ToLower();
                    parameters = methodMatch.Groups[2].Value;
                }
                else
                {
                    method = methodName.Trim().ToLower();
                }
            }

            // Handle synchronous actions
            if (_syncMethodMap.TryGetValue(method, out var syncAction))
            {
                // Execute sync method as a blocking task
                await Task.Run(() => syncAction(client, channel, parameters));
            }
            // Handle asynchronous actions
            else if (_asyncMethodMap.TryGetValue(method, out var asyncAction))
            {
                // Handle delay explicitly
                if (method == "delayasync" && int.TryParse(parameters, out int delayMilliseconds))
                {
                    await Task.Delay(delayMilliseconds);
                }
                else
                {
                    // Add the async method to the list of running tasks (doesn't block the main thread)
                    runningTasks.Add(asyncAction(client, channel, parameters));
                }
            }
            else
            {
                Console.WriteLine($"Method {method} not recognized.");
            }
        }

        // Wait for all running asynchronous tasks to complete (if needed)
        await Task.WhenAll(runningTasks);
    }

    public string AddChatCommand(string commandName, int bitCost, string methods)
    {
        // Ensure the command name is unique
        if (_commands.ContainsKey(commandName.ToLower()))
        {
            return $"Command '{commandName}' already exists.";
        }

        // Split the methods into a list
        var methodList = methods.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        // Validate each method to ensure it's in either the sync or async method map
        foreach (var methodName in methodList)
        {
            string method;
            string parameters = null;

            // Check for '=' format (i.e., methodName=parameter)
            if (methodName.Contains("="))
            {
                var commandParts = methodName.Split(new[] { '=' }, 2);
                method = commandParts[0].Trim().ToLower();
                parameters = commandParts.Length > 1 ? commandParts[1].Trim() : null;
            }
            // Check for '()' format (i.e., methodName(parameters))
            else
            {
                var methodMatch = Regex.Match(methodName, @"([a-zA-Z]+)\(([^)]*)\)");
                if (methodMatch.Success)
                {
                    method = methodMatch.Groups[1].Value.ToLower();
                    parameters = methodMatch.Groups[2].Value;
                }
                else
                {
                    method = methodName.Trim().ToLower();
                }
            }

            // Check if the method is valid (exists in either sync or async map)
            if (!_syncMethodMap.ContainsKey(method) && !_asyncMethodMap.ContainsKey(method))
            {
                return $"Invalid method '{method}' for command '{commandName}'. Please use a valid method.";
            }
        }

        // Create a new command object
        var newCommand = new Command
        {
            BitCost = bitCost,
            Methods = methodList
        };

        // Add the new command to the dictionary
        _commands[commandName.ToLower()] = newCommand;

        // Save the updated dictionary back to the JSON file
        SaveChatCommandsToFile();
        return $"Command '{commandName}' added successfully.";
    }

    public bool RemoveChatCommand(string commandName)
    {

        // Load the existing commands from the file
        Dictionary<string, Command> currentCommands = new Dictionary<string, Command>();

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            currentCommands = JsonConvert.DeserializeObject<Dictionary<string, Command>>(json) ?? new Dictionary<string, Command>();
        }

        // Normalize the command name (remove '!' and case insensitive comparison)
        string normalizedCommandName = commandName.Replace("!", "").ToLower();

        // Check if the command exists and remove it
        if (currentCommands.ContainsKey(normalizedCommandName))
        {
            currentCommands.Remove(normalizedCommandName);

            // Save the updated commands to the file
            var updatedJson = JsonConvert.SerializeObject(currentCommands, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);

            return true;
        }

        // Command not found
        return false;
    }

    private void SaveChatCommandsToFile()
    {

        // Load the existing commands from the file if it exists
        Dictionary<string, Command> currentCommands = new Dictionary<string, Command>();

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            currentCommands = JsonConvert.DeserializeObject<Dictionary<string, Command>>(json) ?? new Dictionary<string, Command>();
        }

        // Normalize the command names to avoid duplicates like "!chatcom" and "chatcom"
        Dictionary<string, Command> normalizedCommands = new Dictionary<string, Command>();
        foreach (var cmd in currentCommands)
        {
            var normalizedKey = cmd.Key.Replace("!", "").ToLower();  // Remove any '!' and normalize case
            normalizedCommands[normalizedKey] = cmd.Value;
        }

        // Merge the new commands, ensuring no duplicates (by normalized name)
        foreach (var newCommand in _commands)
        {
            string normalizedKey = newCommand.Key.Replace("!", "").ToLower(); // Normalize the new command key

            // Only add the command if it doesn't already exist (normalized)
            if (!normalizedCommands.ContainsKey(normalizedKey))
            {
                normalizedCommands[normalizedKey] = newCommand.Value;
            }
        }

        // Serialize the updated dictionary to JSON
        var updatedJson = JsonConvert.SerializeObject(normalizedCommands, Formatting.Indented);

        // Instead of overwriting the entire file, append only new commands
        // This creates or updates the file with merged content
        File.WriteAllText(filePath, updatedJson); 
        Console.WriteLine("Commands have been successfully updated in CustomCommands.json.");
    }

    private void HoldKey(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for HoldKey.");
            return;
        }

        var match = Regex.Match(parameter, @"([a-zA-Z0-9]+)\((\d+)\)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Button(Duration).");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int duration))
        {
            Console.WriteLine("Invalid duration specified.");
            return;
        }

        int vkCode;
        try
        {
            vkCode = ToVirtualKey(key);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error mapping key: {ex.Message}");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Holding key '{key}' (VK Code: {vkCode:X}) for {duration} milliseconds.");
        }

        try
        {
            // Press the key
            keybd_event((byte)vkCode, 0, KEYEVENTF_KEYDOWN, 0);
            System.Threading.Thread.Sleep(duration); // Hold for specified duration
                                                     // Release the key
            keybd_event((byte)vkCode, 0, KEYEVENTF_KEYUP, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error holding key: {ex.Message}");
        }
    }

    private async Task HoldKeyAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for HoldKeyAsync.");
            return;
        }

        var match = Regex.Match(parameter, @"([a-zA-Z0-9]+)\((\d+)\)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Button(Duration).");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int duration))
        {
            Console.WriteLine("Invalid duration specified.");
            return;
        }

        byte vkCode;
        try
        {
            vkCode = (byte)ToVirtualKey(key);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error mapping key: {ex.Message}");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Async Holding key '{key}' (VK Code: {vkCode:X}) for {duration} milliseconds.");
        }

        try
        {
            // Press the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
            await Task.Delay(duration); // Hold for specified duration
                                        // Release the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error holding key asynchronously: {ex.Message}");
        }
    }

    private void HitKey(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for HitKey.");
            return;
        }

        // Remove duration-related code
        var match = Regex.Match(parameter, @"([a-zA-Z0-9]+)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Button.");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        byte vkCode;
        try
        {
            vkCode = (byte)ToVirtualKey(key);
        }
        catch (ArgumentException ex) { Console.WriteLine($"Error mapping key: {ex.Message}"); return; }
        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Hitting key '{key}' (VK Code: {vkCode:X})");
        }

        try
        {
            // Press the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
            // Release the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error hitting key: {ex.Message}");
        }
    }

    private async Task HitKeyAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for HitKeyAsync.");
            return;
        }

        var match = Regex.Match(parameter, @"([a-zA-Z0-9]+)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Button.");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        byte vkCode;
        try
        {
            vkCode = (byte)ToVirtualKey(key);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error mapping key: {ex.Message}");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Async Hitting key '{key}' (VK Code: {vkCode:X})");
        }

        try
        {
                                   // Press the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
            // Release the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error hitting key asynchronously: {ex.Message}");
        }
    }

    private void HitKeyLoop(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for HitKeyLoop.");
            return;
        }

        // Match format A(numberOfTimes, delayInMilliseconds)
        var match = Regex.Match(parameter, @"([a-zA-Z0-9]+)\((\d+),\s*(\d+)\)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Key(NumberOfTimes, DelayInMilliseconds).");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int times) || !int.TryParse(match.Groups[3].Value, out int delay))
        {
            Console.WriteLine("Invalid number of times or delay specified.");
            return;
        }

        byte vkCode;
        try
        {
            vkCode = (byte)ToVirtualKey(key);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error mapping key: {ex.Message}");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Hitting key '{key}' (VK Code: {vkCode:X}) {times} times with {delay}ms delay.");
        }

        try
        {
            for (int i = 0; i < times; i++)
            {
                // Press the key
                keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
                // Release the key
                keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);

                // Delay between key presses
                Thread.Sleep(delay);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in HitKeyLoop: {ex.Message}");
        }
    }

    private async Task HitKeyLoopAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for HitKeyLoopAsync.");
            return;
        }

        // Match format A(numberOfTimes, delayInMilliseconds)
        var match = Regex.Match(parameter, @"([a-zA-Z0-9]+)\((\d+),\s*(\d+)\)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Key(NumberOfTimes, DelayInMilliseconds).");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int times) || !int.TryParse(match.Groups[3].Value, out int delay))
        {
            Console.WriteLine("Invalid number of times or delay specified.");
            return;
        }

        byte vkCode;
        try
        {
            vkCode = (byte)ToVirtualKey(key);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error mapping key: {ex.Message}");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Async Hitting key '{key}' (VK Code: {vkCode:X}) {times} times with {delay}ms delay.");
        }

        try
        {
            for (int i = 0; i < times; i++)
            {
                // Press the key
                keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
                // Release the key
                keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);

                // Async delay between key presses
                await Task.Delay(delay);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in HitKeyLoopAsync: {ex.Message}");
        }
    }

    private void TurnMouse(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for TurnMouse.");
            return;
        }

        // Regex to match Direction(Duration,Speed)
        var match = Regex.Match(parameter, @"([UDLR]|RAND)\((\d+),(\d+)\)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Direction(Duration,Speed).");
            return;
        }

        string direction = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int duration) ||
            !int.TryParse(match.Groups[3].Value, out int speed))
        {
            Console.WriteLine("Invalid duration or speed specified.");
            return;
        }

        // Calculate move distance per iteration
        int pixelsPerSecond = speed;
        int moveDistance = pixelsPerSecond * 10; // Move distance every 10ms
        int interval = 10; // Move every 10ms
        int totalSteps = duration / interval;

        int dx = 0, dy = 0;

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Turning mouse '{direction}' for {duration} milliseconds at speed {speed}.");
            }

            if (direction == "RAND")
            {
                int randomNumber = random.Next(0, 2); // Generates 0 or 1 once
                if (UiBot.Properties.Settings.Default.isDebugOn)
                {
                    Console.WriteLine($"Random number: {randomNumber}");
                }

                switch (randomNumber)
                {
                    case 0:
                        dx = -moveDistance;
                        break;
                    case 1:
                        dx = moveDistance;
                        break;
                    default:
                        Console.WriteLine("Unexpected random number.");
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case "U":
                        dy = -moveDistance;
                        break;
                    case "D":
                        dy = moveDistance;
                        break;
                    case "R":
                        dx = moveDistance;
                        break;
                    case "L":
                        dx = -moveDistance;
                        break;
                    default:
                        Console.WriteLine("Invalid direction specified. Use U, D, L, R, or RAND.");
                        return;
                }
            }

            for (int i = 0; i < totalSteps; i++)
            {
                // Move the mouse
                mouse_event(MOUSEEVENTF_MOVE, dx, dy, 0, 0);

                // Sleep for the interval duration
                Thread.Sleep(interval);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error turning mouse: {ex.Message}");
        }
    }

    private async Task TurnMouseAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for TurnMouse.");
            return;
        }

        // Regex to match Direction(Duration,Speed) or RAND(Duration,Speed)
        var match = Regex.Match(parameter, @"([UDLR]|RAND)\((\d+),(\d+)\)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid parameter format. Expected format: Direction(Duration,Speed) or RAND(Duration,Speed).");
            return;
        }

        string direction = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int duration) ||
            !int.TryParse(match.Groups[3].Value, out int speed))
        {
            Console.WriteLine("Invalid duration or speed specified.");
            return;
        }

        // Calculate move distance per iteration
        int pixelsPerSecond = speed;
        int moveDistance = pixelsPerSecond * 10; // Move distance every 10ms
        int interval = 10; // Move every 10ms
        int totalSteps = duration / interval;

        int dx = 0, dy = 0;

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Turning mouse '{direction}' for {duration} milliseconds at speed {speed}.");
            }

            if (direction == "RAND")
            {
                int randomNumber = random.Next(0, 2); // Generates 0 or 1 once
                if (UiBot.Properties.Settings.Default.isDebugOn)
                {
                    Console.WriteLine($"Random number: {randomNumber}");
                }

                switch (randomNumber)
                {
                    case 0:
                        dx = -moveDistance;
                        break;
                    case 1:
                        dx = moveDistance;
                        break;
                    default:
                        Console.WriteLine("Unexpected random number.");
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case "U":
                        dy = -moveDistance;
                        break;
                    case "D":
                        dy = moveDistance;
                        break;
                    case "R":
                        dx = moveDistance;
                        break;
                    case "L":
                        dx = -moveDistance;
                        break;
                    default:
                        Console.WriteLine(channel, "Invalid direction specified. Use U, D, L, R, or RAND.");
                        return;
                }
            }

            for (int i = 0; i < totalSteps; i++)
            {
                // Move the mouse
                mouse_event(MOUSEEVENTF_MOVE, dx, dy, 0, 0);

                // Sleep for the interval duration
                await Task.Delay(interval);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error turning mouse: {ex.Message}");
        }
    }

    private void MousePos(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for MousePos.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+),(\d+)");
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out int x) || !int.TryParse(match.Groups[2].Value, out int y))
        {
            Console.WriteLine("Invalid parameter format. Expected format: x,y.");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Moving mouse to position ({x}, {y}).");
        }

        SetCursorPos(x, y);
    }

    private async Task MousePosAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for MousePos.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+),(\d+)");
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out int x) || !int.TryParse(match.Groups[2].Value, out int y))
        {
            Console.WriteLine("Invalid parameter format. Expected format: x,y.");
            return;
        }

        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine($"Moving mouse to position ({x}, {y}).");
        }

        // Run SetCursorPos on a separate thread to avoid blocking.
        await Task.Run(() => SetCursorPos(x, y));
    }

    private void LeftClick(TwitchClient client, string channel, string parameter = null)
    {
        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine("Left click");
        }
        // Simulate a left mouse button click
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    private async Task LeftClickAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine("Async Left click");
        }
        // Simulate a left mouse button click
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    private void RightClick(TwitchClient client, string channel, string parameter = null)
    {
        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine("Right click");
        }
        // Simulate pressing the right mouse button
        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        // Simulate releasing the right mouse button
        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
    }

    private async Task RightClickAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (UiBot.Properties.Settings.Default.isDebugOn)
        {
            Console.WriteLine("Async Right click");
        }
        // Simulate pressing the right mouse button
        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        // Simulate releasing the right mouse button
        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
    }

    private void LeftClickLoop(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for LeftClickLoop.");
            return;
        }

        var match = Regex.Match(parameter, @"\((\d+),\s*(\d+)\)");
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out int clickCount) || !int.TryParse(match.Groups[2].Value, out int delay))
        {
            Console.WriteLine("Invalid parameter format. Expected format: LeftClickLoop(ClickCount, DelayInMilliseconds).");
            return;
        }

        for (int i = 0; i < clickCount; i++)
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Left click {i + 1}/{clickCount}");
            }
            // Simulate a left mouse button click
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            // Sleep for the specified delay between clicks
            System.Threading.Thread.Sleep(delay);
        }
    }

    private void RightClickLoop(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for RightClickLoop.");
            return;
        }

        var match = Regex.Match(parameter, @"\((\d+),\s*(\d+)\)");
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out int clickCount) || !int.TryParse(match.Groups[2].Value, out int delay))
        {
            Console.WriteLine("Invalid parameter format. Expected format: RightClickLoop(ClickCount, DelayInMilliseconds).");
            return;
        }

        for (int i = 0; i < clickCount; i++)
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Right click {i + 1}/{clickCount}");
            }
            // Simulate pressing the right mouse button
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            // Simulate releasing the right mouse button
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

            // Sleep for the specified delay between clicks
            System.Threading.Thread.Sleep(delay);
        }
    }
    private async Task LeftClickLoopAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for LeftClickLoopAsync.");
            return;
        }

        var match = Regex.Match(parameter, @"\((\d+),\s*(\d+)\)");
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out int clickCount) || !int.TryParse(match.Groups[2].Value, out int delay))
        {
            Console.WriteLine("Invalid parameter format. Expected format: LeftClickLoopAsync(ClickCount, DelayInMilliseconds).");
            return;
        }

        for (int i = 0; i < clickCount; i++)
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Async Left click {i + 1}/{clickCount}");
            }
            // Simulate a left mouse button click
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            // Asynchronous delay between clicks
            await Task.Delay(delay);
        }
    }

    private async Task RightClickLoopAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for RightClickLoopAsync.");
            return;
        }

        var match = Regex.Match(parameter, @"\((\d+),\s*(\d+)\)");
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out int clickCount) || !int.TryParse(match.Groups[2].Value, out int delay))
        {
            Console.WriteLine("Invalid parameter format. Expected format: RightClickLoopAsync(ClickCount, DelayInMilliseconds).");
            return;
        }

        for (int i = 0; i < clickCount; i++)
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Async Right click {i + 1}/{clickCount}");
            }
            // Simulate pressing the right mouse button
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            // Simulate releasing the right mouse button
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

            // Asynchronous delay between clicks
            await Task.Delay(delay);
        }
    }

    private void RightClickHold(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for LeftClickHold.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Holding right mouse button for {duration} milliseconds.");
            }            
            // Simulate pressing the left mouse button
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            System.Threading.Thread.Sleep(duration); // Hold for specified duration

            // Simulate releasing the left mouse button
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LeftClickHold: {ex.Message}");
        }
    }

    private async Task RightClickHoldAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for RightClickHold.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Holding async right mouse button for {duration} milliseconds.");
            }
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            await Task.Delay(duration); // Use Task.Delay instead of Thread.Sleep
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RightClickHold: {ex.Message}");
        }
    }

    private void LeftClickHold(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for LeftClickHold.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Holding left mouse button for {duration} milliseconds.");
            }
            // Simulate pressing the left mouse button
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            System.Threading.Thread.Sleep(duration); // Hold for specified duration

            // Simulate releasing the left mouse button
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LeftClickHold: {ex.Message}");
        }
    }

    private async Task LeftClickHoldAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for LeftClickHold.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Holding Async left mouse button for {duration} milliseconds.");
            }
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(duration); // Use Task.Delay instead of Thread.Sleep
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LeftClickHold: {ex.Message}");
        }
    }

    private void MuteVolume(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for MuteVolume.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Muting Windows for {duration} milliseconds.");
            }
            // Simulate pressing the left mouse button
            keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            System.Threading.Thread.Sleep(duration); // Hold for specified duration

            // Simulate releasing the left mouse button
            keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MuteVolume: {ex.Message}");
        }
    }

    private async Task MuteVolumeAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for MuteVolume.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Muting Windows for {duration} milliseconds.");
            }

            // Simulate pressing the mute key
            keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);

            // Wait for the specified duration asynchronously
            await Task.Delay(duration);

            // Simulate releasing the mute key
            keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MuteVolume: {ex.Message}");
        }
    }

    private void Delay(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for Delay.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Delay for {duration} milliseconds.");
            }
            System.Threading.Thread.Sleep(duration); // Hold for specified duration

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Delay: {ex.Message}");
        }
    }

    private async Task DelayAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for Delay.");
            return;
        }

        var match = Regex.Match(parameter, @"(\d+)");
        if (!match.Success || !int.TryParse(match.Value, out int duration))
        {
            Console.WriteLine("Invalid parameter format. Expected format: Duration.");
            return;
        }

        try
        {
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                // Calculate time since last command was executed
                var now = DateTime.Now;
                var timeSinceLastCommand = (now - _lastCommandExecutionTime).TotalMilliseconds;

                // Print the delay information with time since last command
                Console.WriteLine($"Delay for {duration} milliseconds. Time since last command: {timeSinceLastCommand} ms.");

                // Update the last execution time to the current time
                _lastCommandExecutionTime = now;
            }

            await Task.Delay(duration);  // Asynchronously wait for the specified duration
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Delay: {ex.Message}");
        }
    }


    private void PlaySoundClip(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for PlaySoundClip.");
            return;
        }

        // Remove leading and trailing spaces and quotes from the parameter
        var fileName = parameter.Trim(' ', '(', ')', '"');

        // Get the file path
        string filePath = GetSoundFilePath(fileName);

        if (filePath == null)
        {
            Console.WriteLine($"Sound file not found: {fileName}");
            return;
        }

        try
        {
            // Play the sound file
            using (var player = new System.Media.SoundPlayer(filePath))
            {
                player.Play();
            }
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Playing sound for {fileName}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing sound: {ex.Message}");
            Console.WriteLine($"Error playing sound: {fileName}");
        }
    }

    private async Task PlaySoundClipAsync(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            Console.WriteLine("No parameters specified for PlaySoundClip.");
            return;
        }

        var fileName = parameter.Trim(' ', '(', ')', '"');
        string filePath = GetSoundFilePath(fileName);

        if (filePath == null)
        {
            Console.WriteLine($"Sound file not found: {fileName}");
            return;
        }

        try
        {
            // Play the sound file
            using (var player = new System.Media.SoundPlayer(filePath))
            {
                // SoundPlayer doesn't support async playback, but you can use a Task.Run to avoid blocking
                await Task.Run(() => player.Play());
            }
            if (UiBot.Properties.Settings.Default.isDebugOn)
            {
                Console.WriteLine($"Async Playing sound for {fileName}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing sound: {ex.Message}");
        }
    }

    private static string GetSoundFilePath(string soundFileName)
    {
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sound Clips");
        string filePath = Path.Combine(folderPath, soundFileName);

        // Check if file exists
        if (File.Exists(filePath))
        {
            return filePath;
        }
        return null; // Return null if file does not exist
    }

    // Helper method to convert key characters to virtual key codes
    public static int ToVirtualKey(string key)
    {
        switch (key.ToUpper())
        {
            // Letters
            case "A": return 0x41; // VK_A
            case "B": return 0x42; // VK_B
            case "C": return 0x43; // VK_C
            case "D": return 0x44; // VK_D
            case "E": return 0x45; // VK_E
            case "F": return 0x46; // VK_F
            case "G": return 0x47; // VK_G
            case "H": return 0x48; // VK_H
            case "I": return 0x49; // VK_I
            case "J": return 0x4A; // VK_J
            case "K": return 0x4B; // VK_K
            case "L": return 0x4C; // VK_L
            case "M": return 0x4D; // VK_M
            case "N": return 0x4E; // VK_N
            case "O": return 0x4F; // VK_O
            case "P": return 0x50; // VK_P
            case "Q": return 0x51; // VK_Q
            case "R": return 0x52; // VK_R
            case "S": return 0x53; // VK_S
            case "T": return 0x54; // VK_T
            case "U": return 0x55; // VK_U
            case "V": return 0x56; // VK_V
            case "W": return 0x57; // VK_W
            case "X": return 0x58; // VK_X
            case "Y": return 0x59; // VK_Y
            case "Z": return 0x5A; // VK_Z

            // Numbers
            case "0": return 0x30; // VK_0
            case "1": return 0x31; // VK_1
            case "2": return 0x32; // VK_2
            case "3": return 0x33; // VK_3
            case "4": return 0x34; // VK_4
            case "5": return 0x35; // VK_5
            case "6": return 0x36; // VK_6
            case "7": return 0x37; // VK_7
            case "8": return 0x38; // VK_8
            case "9": return 0x39; // VK_9

            // Function keys
            case "F1": return 0x70; // VK_F1
            case "F2": return 0x71; // VK_F2
            case "F3": return 0x72; // VK_F3
            case "F4": return 0x73; // VK_F4
            case "F5": return 0x74; // VK_F5
            case "F6": return 0x75; // VK_F6
            case "F7": return 0x76; // VK_F7
            case "F8": return 0x77; // VK_F8
            case "F9": return 0x78; // VK_F9
            case "F10": return 0x79; // VK_F10
            case "F11": return 0x7A; // VK_F11
            case "F12": return 0x7B; // VK_F12

            // Special keys
            case "ESC": return 0x1B; // VK_ESCAPE
            case "TAB": return 0x09; // VK_TAB
            case "ENTER": return 0x0D; // VK_RETURN
            case "SHIFT": return 0x10; // VK_SHIFT
            case "CTRL": return 0x11; // VK_CONTROL
            case "ALT": return 0x12; // VK_MENU
            case "CAPSLOCK": return 0x14; // VK_CAPITAL
            case "SPACE": return 0x20; // VK_SPACE
            case "PAGEUP": return 0x21; // VK_PRIOR
            case "PAGEDOWN": return 0x22; // VK_NEXT
            case "END": return 0x23; // VK_END
            case "HOME": return 0x24; // VK_HOME
            case "LEFT": return 0x25; // VK_LEFT
            case "UP": return 0x26; // VK_UP
            case "RIGHT": return 0x27; // VK_RIGHT
            case "DOWN": return 0x28; // VK_DOWN
            case "PRINTSCREEN": return 0x2C; // VK_SNAPSHOT
            case "INSERT": return 0x2D; // VK_INSERT
            case "DELETE": return 0x2E; // VK_DELETE
            case "WIN": return 0x5B; // VK_LWIN
            case "NUMLOCK": return 0x90; // VK_NUMLOCK
            case "SCROLLLOCK": return 0x91; // VK_SCROLL

            // Keypad keys
            case "NUMPAD0": return 0x60; // VK_NUMPAD0
            case "NUMPAD1": return 0x61; // VK_NUMPAD1
            case "NUMPAD2": return 0x62; // VK_NUMPAD2
            case "NUMPAD3": return 0x63; // VK_NUMPAD3
            case "NUMPAD4": return 0x64; // VK_NUMPAD4
            case "NUMPAD5": return 0x65; // VK_NUMPAD5
            case "NUMPAD6": return 0x66; // VK_NUMPAD6
            case "NUMPAD7": return 0x67; // VK_NUMPAD7
            case "NUMPAD8": return 0x68; // VK_NUMPAD8
            case "NUMPAD9": return 0x69; // VK_NUMPAD9
            case "NUMPADADD": return 0x6B; // VK_ADD
            case "NUMPADSUBTRACT": return 0x6D; // VK_SUBTRACT
            case "NUMPADMULTIPLY": return 0x6A; // VK_MULTIPLY
            case "NUMPADDIVIDE": return 0x6F; // VK_DIVIDE

            // Additional keys can be mapped here

            default:
                throw new ArgumentException("Unsupported key");
        }
    }
}
public class Command
{
    public int BitCost { get; set; }
    public List<string> Methods { get; set; }
}