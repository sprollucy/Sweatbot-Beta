using System.Runtime.InteropServices;
using Newtonsoft.Json;
using TwitchLib.Client;
using System.Diagnostics;
using System.Media;
/* TODO **
 * Add reload, lean, 360 mag dump
 */

namespace UiBot
{
    public class CounterData
    {
        public string ChannelName { get; set; }
        public string BotToken { get; set; }
    }
    public class ConfigData
    {
        public string RandomKeyInputs { get; set; }
        public string DropKey { get; set; }
        public Point[] MouseCursorPositions { get; set; }
        public string grenadeTossKey { get; set; }
        public string crouchKey { get; set; }
    }
    internal class ChatCommandMethods
    {
        //bit dictionary 
        public static Dictionary<string, int> userBits = new Dictionary<string, int>();
        //command dictionary
        public Dictionary<string, string> commandConfigData;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]

        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        // Mouse event constants
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_MOVE = 0x0001;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        ControlMenu controlMenu = new ControlMenu();

        // Handles connection
        TwitchClient client;
        public static string channelId;


        //wiggle
        public Random random = new Random();

        //random key press
        public DateTime lastRandomKeyPressesTime = DateTime.MinValue;

        Counter counter = new Counter();

        //death counter
        public int deathCount = 0;
        public int killCount = 0;
        public int survivalCount = 0;



        //goose
        public DateTime lastGooseCommandTime = DateTime.MinValue;
        public Process gooseProcess; // Declare a Process variable to store the Goose process.

        //grenade
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string soundFileName = Path.Combine("Sounds", "grenade.wav");


        //spam command
        public DateTime lastStatCommandTimer = DateTime.MinValue;
        public DateTime lastWipeStatCommandTimer = DateTime.MinValue;
        public DateTime lastHelpCommandTimer = DateTime.MinValue;
        public DateTime lastAboutCommandTimer = DateTime.MinValue;

        public void LoadCredentialsFromJSON()
        {
            string jsonFilePath = "Logon.json";

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                CounterData data = JsonConvert.DeserializeObject<CounterData>(json);

                if (data != null && !string.IsNullOrEmpty(data.ChannelName))
                {
                    channelId = data.ChannelName;
                }
                else
                {
                    // Handle the case where the ChannelName is empty or invalid.
                    // You can show a message to the user or take appropriate action.
                }
            }
            else
            {
                // Handle the absence of the JSON file as needed.
            }
        }

        public TimeSpan GetRemainingRandomKeyPressesCooldown()
        {
            // Instantiate ControlMenu class to access the text box's text
            TextBox randomCooldownTextBox = controlMenu.RandomKeyCooldownTextBox;


            if (int.TryParse(randomCooldownTextBox.Text, out int cooldownSeconds))
            {
                TimeSpan cooldownDuration = TimeSpan.FromSeconds(cooldownSeconds);
                DateTime cooldownEndTime = lastRandomKeyPressesTime.Add(cooldownDuration);
                TimeSpan remainingCooldown = cooldownEndTime - DateTime.Now;

                return (remainingCooldown.TotalSeconds > 0) ? remainingCooldown : TimeSpan.Zero;
            }
            else
            {
                // Invalid input from the text box, use a default value or handle the error
                return TimeSpan.Zero; // You can return a default value or handle the error as needed
            }
        }

        public void SendRandomKeyPresses()
        {
            // Load the keys from CommandConfigData.json
            string configFilePath = "CommandConfigData.json"; // Adjust the file path as needed
            string keysInput;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                keysInput = configData?.RandomKeyInputs;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(keysInput))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            Random random = new Random();
            string[] keys = keysInput.Split(',');

            foreach (string key in keys)
            {
                string cleanedKey = key.Trim(); // Remove any leading/trailing spaces
                SendKeys.SendWait(cleanedKey);

                // Apply a random hold duration between 250ms and 1000ms
                int holdDuration = random.Next(250, 1001);
                Thread.Sleep(holdDuration);
            }
        }

        public void WiggleMouse(int numWiggles, int wiggleDistance, int delayBetweenWiggles)
        {
            for (int i = 0; i < numWiggles; i++)
            {
                // Move the mouse to the right
                mouse_event(MOUSEEVENTF_MOVE, wiggleDistance, 0, 0, 0);
                Thread.Sleep(delayBetweenWiggles);

                // Move the mouse back to the left (negative distance)
                mouse_event(MOUSEEVENTF_MOVE, -wiggleDistance, 0, 0, 0);
                Thread.Sleep(delayBetweenWiggles);
            }
        }

        public void TurnRandom(int durationMilliseconds)
        {
            Random random = new Random();
            int direction = random.Next(2) * 2 - 1; // -1 for left, 1 for right
            DateTime endTime = DateTime.Now.AddMilliseconds(durationMilliseconds);

            while (DateTime.Now < endTime)
            {
                // Generate a random distance to move (e.g., between 5 and 20 pixels)
                int distance = random.Next(5, 100) * direction;

                // Move the mouse
                mouse_event(MOUSEEVENTF_MOVE, distance, 0, 0, 0);

                // Sleep for a short duration before the next movement (e.g., 100-300ms)
                Thread.Sleep(random.Next(10));
            }
        }

        public void GrenadeToss(int durationMilliseconds)
        {
            // Load the keys from CommandConfigData.json
            string configFilePath = "CommandConfigData.json"; // Adjust the file path as needed
            string grenadeKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                grenadeKeyBox = configData?.grenadeTossKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(grenadeKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            // Start a new thread for pulling out the grenade
            Thread grenadeThread = new Thread(() =>
            {
                SendKeys.SendWait(grenadeKeyBox);
            });

            // Start a new thread for spinning
            Thread spinThread = new Thread(() =>
            {
                TurnRandom(durationMilliseconds);
            });

            // Start both threads
            grenadeThread.Start();
            spinThread.Start();

            // Wait for the grenadeThread to complete
            grenadeThread.Join();

            // Wait for 2 seconds after pulling the grenade
            Thread.Sleep(2000);

            // After 2 seconds, simulate left-click to throw the grenade
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void CrouchorStand()
        {
            // Load the keys from CommandConfigData.json
            string configFilePath = "CommandConfigData.json"; // Adjust the file path as needed
            string crouchKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                crouchKeyBox = configData?.crouchKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(crouchKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }


                SendKeys.SendWait(crouchKeyBox);


        }

        public void SimulateButtonPressAndMouseMovement()
        {
            // Disable keyboard and mouse inputs
            BlockInput(true);

            string configFilePath = "DropPositionData.json"; // Adjust the file path as needed
            string dropKey;

            System.Threading.Timer timer = new System.Threading.Timer(state =>
            {
                // Enable keyboard and mouse inputs after 5 seconds
                BlockInput(false);
                Console.WriteLine("Input is now unblocked.");
            }, null, 5000, Timeout.Infinite);

            try
            {
                // Read the JSON file and parse it to extract the keys and mouse positions
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                dropKey = configData?.DropKey;

                // Load the recorded mouse positions
                Point[] mousePositions = configData?.MouseCursorPositions;

                if (mousePositions == null)
                {
                    Console.WriteLine("Invalid or missing mouse positions data in the configuration.");
                    return;
                }

                // Simulate button presses
                string[] keyPresses = new string[] { "{Z}", "{Z}", "{TAB}", "{DELETE}" };
                int[] sleepDurations = new int[] { 150, 200, 0, 300 }; // Corresponding sleep durations in milliseconds

                for (int i = 0; i < keyPresses.Length; i++)
                {
                    SendKeys.SendWait(keyPresses[i]);

                    // Sleep only if necessary
                    if (sleepDurations[i] > 0)
                        Thread.Sleep(sleepDurations[i]);
                }

                foreach (Point newPosition in mousePositions)
                {
                    // Store the original mouse position
                    Point originalMousePosition = Cursor.Position;

                    // Move the mouse to the new position
                    Cursor.Position = newPosition;

                    // Simulate a mouse click (DELETE key in this case)
                    SendKeys.SendWait(dropKey);

                    // Sleep for a short duration
                    Thread.Sleep(300);

                    // Restore the original mouse position
                    Cursor.Position = originalMousePosition;
                }
            }
            finally
            {
                // Ensure that input is re-enabled in case of exceptions
                BlockInput(false);
            }
        }


        public void BagDrop()
        {
            // Simulate button presses
            string[] keyPresses = new string[] { "{Z}", "{Z}" };
            int[] sleepDurations = new int[] { 150, 150 }; // Corresponding sleep durations in milliseconds

            for (int i = 0; i < keyPresses.Length; i++)
            {
                SendKeys.SendWait(keyPresses[i]);

                // Sleep only if necessary
                if (sleepDurations[i] > 0)
                    Thread.Sleep(sleepDurations[i]);
            }
        }

        public void PopShot()
        {
            // Simulate a left mouse button click
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void MagDump()
        {
            // Simulate pressing the left mouse button
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

            // Wait for the specified duration
            Thread.Sleep(2500);

            // Simulate releasing the left mouse button
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public void HoldAim()
        {
                // Simulate pressing the right mouse button
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);

                // Wait for the specified duration
                Thread.Sleep(2500);

                // Simulate releasing the right mouse button
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);    
        }


        public void GrenadeSound()
        {
            // Create a SoundPlayer and specify the notification sound file path
            string notificationSoundFilePath = Path.Combine(appDirectory, soundFileName);
            SoundPlayer player = new SoundPlayer(notificationSoundFilePath);

            // Play the notification sound
            Thread.Sleep(1000);
            player.Play();

        }

    }
}