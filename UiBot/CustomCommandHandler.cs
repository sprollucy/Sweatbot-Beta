﻿using Newtonsoft.Json;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions; // For parsing command string
using System.Windows.Forms;
using TwitchLib.Client;
using UiBot;

public class CustomCommandHandler
{
    private readonly Dictionary<string, Command> _commands;
    private readonly Dictionary<string, Action<TwitchClient, string, string>> _methodMap;
    private readonly Dictionary<string, Action<TwitchClient, string, string>> _syncMethodMap;
    private readonly Dictionary<string, Func<TwitchClient, string, string, Task>> _asyncMethodMap;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    private Random random = new Random(); // Class-level Random object

    // event constants
    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int KEYEVENTF_KEYDOWN = 0x0000; 
    private const int KEYEVENTF_KEYUP = 0x0002;
    public const int MOUSEEVENTF_MOVE = 0x0001;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const int MOUSEEVENTF_RIGHTUP = 0x0010;
    const int VK_VOLUME_MUTE = 0xAD;
    const uint KEYEVENTF_EXTENDEDKEY = 0x0001;


    public CustomCommandHandler(string filePath)
    {
        _commands = LoadCommandsFromFile(filePath);

        _syncMethodMap = new Dictionary<string, Action<TwitchClient, string, string>>
    {
        { "holdkey", HoldKey },
        { "hitkey", HitKey },
        { "leftclick", LeftClick },
        { "rightclick", RightClick },
        { "turnmouse", TurnMouse },
        { "playsoundclip", PlaySoundClip },
        { "rightclickhold", RightClickHold },
        { "leftclickhold", LeftClickHold },
        { "mutevolume", MuteVolume },
        { "delay", Delay }
    };

        _asyncMethodMap = new Dictionary<string, Func<TwitchClient, string, string, Task>>
    {
        { "holdkeyasync", HoldKeyAsync },
        { "hitkeyasync", HitKeyAsync },
        { "leftclickasync", LeftClickAsync },
        { "rightclickasync", RightClickAsync },
        { "turnmouseasync", TurnMouseAsync },
        { "playsoundclipasync", PlaySoundClipAsync },
        { "rightclickholdasync", RightClickHoldAsync },
        { "leftclickholdasync", LeftClickHoldAsync }

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
            throw new FileNotFoundException("Commands file not found.", filePath);
        }

        var json = File.ReadAllText(filePath);
        var commands = JsonConvert.DeserializeObject<Dictionary<string, Command>>(json);

        // Debugging: Print out the loaded commands
        foreach (var cmd in commands)
        {
            Console.WriteLine($"Command: {cmd.Key}, BitCost: {cmd.Value.BitCost}, Methods: {string.Join(", ", cmd.Value.Methods ?? new List<string>())}");
        }

        return commands;
    }


    public Command GetCommand(string commandName)
    {
        _commands.TryGetValue(commandName.ToLower(), out var command);
        return command;
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

        var sequentialTasks = new List<Func<Task>>();
        var asyncTasks = new List<Func<Task>>();

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

            if (_syncMethodMap.TryGetValue(method, out var syncAction))
            {
                sequentialTasks.Add(async () =>
                {
                    await Task.Run(() => syncAction(client, channel, parameters));
                });
            }
            else if (_asyncMethodMap.TryGetValue(method, out var asyncAction))
            {
                asyncTasks.Add(() => asyncAction(client, channel, parameters));
            }
            else
            {
                Console.WriteLine($"Method {method} not recognized.");
            }
        }

        // Execute synchronous methods in sequence
        foreach (var task in sequentialTasks)
        {
            await task();
        }

        // Execute asynchronous methods in parallel
        await Task.WhenAll(asyncTasks.Select(t => t()));
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
            await Task.Delay(100); // Small delay before sending key events
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

    private void Delay(TwitchClient client, string channel, string parameter = null)
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
                Console.WriteLine($"Delay for {duration} milliseconds.");
            }
            System.Threading.Thread.Sleep(duration); // Hold for specified duration

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
    private static int ToVirtualKey(string key)
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