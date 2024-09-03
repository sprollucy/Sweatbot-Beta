using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions; // For parsing command string
using TwitchLib.Client;

public class CustomCommandHandler
{
    private readonly Dictionary<string, List<string>> _commands;
    private readonly Dictionary<string, Action<TwitchClient, string, string>> _methodMap;
    // Import necessary methods from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    private const int KEYEVENTF_KEYDOWN = 0x0000; // Key down flag
    private const int KEYEVENTF_KEYUP = 0x0002; // Key up flag
    public CustomCommandHandler(string filePath)
    {
        _commands = LoadCommandsFromFile(filePath);
        _methodMap = new Dictionary<string, Action<TwitchClient, string, string>>
        {
            { "methoda", MethodA },
            { "methodb", MethodB },
            { "methodc", MethodC },
            { "holdkey", HoldKey },
            { "hitkey", HitKey }
        };
    }

    private Dictionary<string, List<string>> LoadCommandsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Commands file not found.", filePath);
        }

        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
    }

    public void HandleCommand(string message, TwitchClient client, string channel)
    {
        Console.WriteLine($"Received message: {message}");

        if (!message.StartsWith("!"))
        {
            Console.WriteLine("Message does not start with '!'");
            return;
        }

        string[] parts = message.Substring(1).Split(' ');
        string command = parts[0].ToLower();
        Console.WriteLine($"Parsed command: {command}");

        if (_commands.TryGetValue(command, out var methods))
        {
            Console.WriteLine($"Methods found for command '{command}': {string.Join(", ", methods)}");

            foreach (var methodName in methods)
            {
                Console.WriteLine($"Executing method: {methodName}");

                var commandParts = methodName.Split('=');
                var method = commandParts[0].ToLower();
                var parameters = commandParts.Length > 1 ? commandParts[1] : null;

                if (_methodMap.TryGetValue(method, out var action))
                {
                    action(client, channel, parameters);
                }
                else
                {
                    Console.WriteLine($"Method {method} not recognized.");
                    client.SendMessage(channel, $"Method {method} not recognized.");
                }
            }
        }
        else
        {
            Console.WriteLine($"Command '{command}' not recognized.");
            client.SendMessage(channel, "Command not recognized.");
        }
    }

    private void MethodA(TwitchClient client, string channel, string parameter = null)
    {
        client.SendMessage(channel, "Method A executed.");
    }

    private void MethodB(TwitchClient client, string channel, string parameter = null)
    {
        client.SendMessage(channel, "Method B executed.");
    }

    private void MethodC(TwitchClient client, string channel, string parameter = null)
    {
        client.SendMessage(channel, "Method C executed.");
    }

    private void HoldKey(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            client.SendMessage(channel, "No parameters specified for HoldButton.");
            return;
        }

        var match = Regex.Match(parameter, @"([a-zA-Z0-9])\((\d+)\)");
        if (!match.Success)
        {
            client.SendMessage(channel, "Invalid parameter format. Expected format: Button(Duration).");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();
        if (!int.TryParse(match.Groups[2].Value, out int duration))
        {
            client.SendMessage(channel, "Invalid duration specified.");
            return;
        }

        // Map the character to a virtual key code
        byte vkCode = (byte)ToVirtualKey(key);

        try
        {
            Console.WriteLine($"Holding button '{key}' for {duration} seconds.");
            client.SendMessage(channel, $"Holding button '{key}' for {duration} ms.");

            // Press the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
            System.Threading.Thread.Sleep(duration); // Hold for specified duration
            // Release the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error holding button: {ex.Message}");
            client.SendMessage(channel, $"Error holding button: {ex.Message}");
        }
    }

    private void HitKey(TwitchClient client, string channel, string parameter = null)
    {
        if (parameter == null)
        {
            client.SendMessage(channel, "No parameters specified for HitKey.");
            return;
        }

        // Remove duration-related code
        var match = Regex.Match(parameter, @"([a-zA-Z0-9])");
        if (!match.Success)
        {
            client.SendMessage(channel, "Invalid parameter format. Expected format: Button.");
            return;
        }

        string key = match.Groups[1].Value.ToUpper();

        // Map the character to a virtual key code
        byte vkCode = (byte)ToVirtualKey(key);

        try
        {
            Console.WriteLine($"Hitting button '{key}'");
            client.SendMessage(channel, $"Hitting button '{key}'");

            // Press and release the key
            keybd_event(vkCode, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error hitting button: {ex.Message}");
            client.SendMessage(channel, $"Error hitting button: {ex.Message}");
        }
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
