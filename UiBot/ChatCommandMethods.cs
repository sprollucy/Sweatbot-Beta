using Newtonsoft.Json;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using TwitchLib.Client;
using TwitchLib.Client.Events;
/* TODO **

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
        public string dropKey { get; set; }
        public string dropbagKey { get; set; }
        public Point[] MouseCursorPositions { get; set; }
        public string grenadeTossKey { get; set; }
        public string crouchKey { get; set; }
        public string proneKey { get; set; }
        public string reloadKey { get; set; }
        public string knifeKey { get; set; }
        public string jumpKey { get; set; }
        public string muteTime { get; set; }
        public string firemodeKey { get; set; }
        public string swapKey { get; set; }
        public string micTime { get; set; }
        public string micKey { get; set; }
        public string walkTime { get; set; }
        public string walkKey { get; set; }
        public string bonusMultiplierBox { get; set; }



    }
    internal class ChatCommandMethods
    {
        //command dictionary
        public Dictionary<string, string> commandConfigData;
        public static Dictionary<string, int> userBits = new Dictionary<string, int>();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]

        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern int BlockInput(int fBlockIt);

        // event constants
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_MOVE = 0x0001;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int VK_VOLUME_MUTE = 0xAD;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_KEYDOWN = 0x0000; // Key down flag
        const byte VK_W = 0x57; // Virtual-key code for the "W" key

        ControlMenu controlMenu = new ControlMenu();

        // Handles connection
        TwitchClient client;
        public static string channelId;

        public Random random = new Random();

        //random key press
        public DateTime lastRandomKeyPressesTime = DateTime.MinValue;

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
        public DateTime lastBitcostCommandTimer = DateTime.MinValue;
        public DateTime lastHow2useTimer = DateTime.MinValue;

        public string configFilePath = Path.Combine("Data", "CommandConfigData.json"); // Adjust the file path as needed
        string dropFilePath = Path.Combine("Data", "DropPositionData.json"); // Adjust the file path as needed

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
                return TimeSpan.Zero;
            }
        }

        public void SendRandomKeyPresses()
        {
            // Load the keys from CommandConfigData.json
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

            // Shuffle the keys array
            keys = keys.OrderBy(x => random.Next()).ToArray();

            foreach (string key in keys)
            {
                string cleanedKey = key.Trim(); // Remove any leading/trailing spaces
                SendKeys.SendWait(cleanedKey);

                // Apply a random hold duration between 250ms and 1000ms
                int holdDuration = random.Next(250, 1000);
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
                int distance = random.Next(200) * direction;

                // Move the mouse
                mouse_event(MOUSEEVENTF_MOVE, distance, 0, 0, 0);

                // Sleep for a short duration before the next movement (e.g., 100-300ms)
                Thread.Sleep(random.Next(10));
            }
        }

        public void GrenadeTossTurn(int durationMilliseconds)
        {
            // Assuming CommandConfigData.json contains the correct key mapping
            string grenadeKey;

            try
            {
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                grenadeKey = configData?.grenadeTossKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(grenadeKey))
            {
                Console.WriteLine("Grenade key not configured.");
                return;
            }

            // Start spinning
            Thread spinThread = new Thread(() => TurnRandom(durationMilliseconds));
            spinThread.Start();

            SendKeys.SendWait("S");
            Thread.Sleep(100);

            // Equip grenade
            SendKeys.SendWait(grenadeKey);

            // Wait 1.5 seconds before throwing the grenade
            Thread.Sleep(3000);

            // Simulate mouse left click to throw the grenade
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public void GrenadeToss()
        {
            // Assuming CommandConfigData.json contains the correct key mapping
            string grenadeKey;

            try
            {
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                grenadeKey = configData?.grenadeTossKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(grenadeKey))
            {
                Console.WriteLine("Grenade key not configured.");
                return;
            }

            SendKeys.SendWait(grenadeKey);

            Thread.Sleep(3000);

            // Simulate mouse left click to throw the grenade
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void MagDump360(int durationMilliseconds)
        {

            // Start a new thread for pulling out the grenade
            Thread magThread = new Thread(() =>
            {
                MagDump();
            });

            // Start a new thread for spinning
            Thread spinThread = new Thread(() =>
            {
                TurnRandom(durationMilliseconds);
            });

            // Start both threads
            magThread.Start();
            spinThread.Start();

            // Wait for the grenadeThread to complete
            magThread.Join();

        }

        public void CrouchorStand()
        {
            // Load the keys from CommandConfigData.json
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

        public void Prone()
        {
            // Load the keys from CommandConfigData.json
            string proneKey;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                proneKey = configData?.proneKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(proneKey))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            SendKeys.SendWait(proneKey);
        }

        public void Reload()
        {
            // Load the keys from CommandConfigData.json
            string reloadKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                reloadKeyBox = configData?.reloadKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(reloadKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            SendKeys.SendWait(reloadKeyBox);
        }
        public void DropMag()
        {
            // Load the keys from CommandConfigData.json
            string reloadKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                reloadKeyBox = configData?.reloadKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(reloadKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            SendKeys.SendWait(reloadKeyBox);
            Thread.Sleep(25);
            SendKeys.SendWait(reloadKeyBox);
        }

        public void VoiceLine()
        {
            int randomNumber = random.Next(1, 3); // Generates 1 or 2

            if (randomNumber == 1)
            {
                SendKeys.SendWait("{F1}");
            }
            else if (randomNumber == 2)
            {
                SendKeys.SendWait("{F1}");
                Thread.Sleep(50); // Adjust delay time as needed
                SendKeys.SendWait("{F1}");
            }
        }

        public void SimulateButtonPressAndMouseMovement()
        {
            // Disable keyboard and mouse inputs
            BlockInput(1);

            string dropKey;
            string dropbagKeyBox;


            System.Threading.Timer timer = new System.Threading.Timer(state =>
            {
                // Enable keyboard and mouse inputs after 5 seconds
                BlockInput(0);
                Console.WriteLine("Input is now unblocked.");
            }, null, 5000, Timeout.Infinite);

            try
            {
                // Read the JSON file and parse it to extract the keys and mouse positions
                string json = File.ReadAllText(dropFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                string json2 = File.ReadAllText(configFilePath);
                var configData2 = JsonConvert.DeserializeObject<ConfigData>(json2); // Corrected: use json2
                dropbagKeyBox = configData?.dropbagKey;
                dropKey = configData2?.dropKey;

                // Load the recorded mouse positions
                Point[] mousePositions = configData?.MouseCursorPositions;

                if (mousePositions == null)
                {
                    Console.WriteLine("Invalid or missing mouse positions data in the configuration.");
                    return;
                }

                // Simulate button presses
                string[] keyPresses = new string[] { dropbagKeyBox, dropbagKeyBox, "{TAB}" };
                int[] sleepDurations = new int[] { 150, 200, 0 }; // Corresponding sleep durations in milliseconds

                for (int i = 0; i < keyPresses.Length; i++)
                {
                    SendKeys.SendWait(keyPresses[i]);

                    // Sleep only if necessary
                    if (sleepDurations[i] > 0)
                        Thread.Sleep(sleepDurations[i]);
                }

                foreach (Point newPosition in mousePositions)
                {
                    Thread.Sleep(200);
                    // Store the original mouse position
                    Point originalMousePosition = Cursor.Position;

                    // Move the mouse to the new position
                    Cursor.Position = newPosition;
                    Thread.Sleep(100);
                    SendKeys.SendWait(dropKey);
                }
            }
            finally
            {
                // Ensure that input is re-enabled in case of exceptions
                BlockInput(0);
            }
        }

        public void BagDrop()
        {
            string dropbagKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                dropbagKeyBox = configData?.dropbagKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(dropbagKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }
            // Simulate button presses
            string[] keyPresses = new string[] { dropbagKeyBox, dropbagKeyBox };
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
            Thread.Sleep(100);
            // Simulate a left mouse button click
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void MagDump()
        {
            Thread.Sleep(100);
            // Simulate pressing the left mouse button
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

            // Wait for the specified duration
            Thread.Sleep(3000);

            // Simulate releasing the left mouse button
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public void HoldAim()
        {
            Thread.Sleep(100);
            // Simulate pressing the right mouse button
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);

            // Wait for the specified duration
            Thread.Sleep(2500);

            // Simulate releasing the right mouse button
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static bool PlaySound(string soundFileName, string channelId, TwitchClient client)
        {
            string soundFilePath = GetSoundFilePath(soundFileName);

            if (soundFilePath == null)
            {
                return false; // Return false if sound file not found
            }

            try
            {
                // Create a new SoundPlayer instance and play the sound file
                using (var player = new SoundPlayer(soundFilePath))
                {
                    player.Play();
                }
                return true; // Return true if played successfully
            }
            catch (Exception ex)
            {
                client.SendMessage(channelId, $"Error playing sound: {ex.Message}");
                return false; // Return false if there was an error
            }
        }

        public static string GetSoundFilePath(string soundFileName)
        {
            string soundClipPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sound Clips");
            string soundFilePath = Path.Combine(soundClipPath, $"{soundFileName}.wav"); // assuming .wav as default extension

            return File.Exists(soundFilePath) ? soundFilePath : null; // Return path if exists, otherwise null
        }

        public static List<string> GetAvailableSounds(string soundDirectory)
        {
            if (!Directory.Exists(soundDirectory))
            {
                return new List<string> { "No sounds available" };
            }

            // Define the supported sound file extensions and get all sound files in the directory
            string[] soundExtensions = { "*.mp3", "*.wav", "*.ogg", "*.flac", "*.aac" };
            var files = soundExtensions.SelectMany(ext => Directory.GetFiles(soundDirectory, ext)).ToList();

            // Extract and return the file names without extension
            return files.Select(Path.GetFileNameWithoutExtension).ToList();
        }


        public void KnivesOnly()
        {
            // Load the keys from CommandConfigData.json
            string knifeKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                knifeKeyBox = configData?.knifeKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(knifeKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            SendKeys.SendWait(knifeKeyBox);
        }

        //Name PraiseSun
        public void LookUp(int durationMilliseconds)
        {
            Random random = new Random();
            DateTime endTime = DateTime.Now.AddMilliseconds(durationMilliseconds);

            while (DateTime.Now < endTime)
            {
                // Generate a random Y-coordinate for looking up (e.g., between -20 and -5 pixels)
                int deltaY = -200;

                // Move the mouse vertically
                mouse_event(MOUSEEVENTF_MOVE, 0, deltaY, 0, 0);

                // Sleep for a short duration before the next movement (e.g., 100-300ms)
                Thread.Sleep(random.Next(10));
            }
        }

        public void LookDown(int durationMilliseconds)
        {
            Random random = new Random();
            DateTime endTime = DateTime.Now.AddMilliseconds(durationMilliseconds);

            while (DateTime.Now < endTime)
            {
                // Generate a random Y-coordinate for looking up (e.g., between -20 and -5 pixels)
                int deltaY = 200;

                // Move the mouse vertically
                mouse_event(MOUSEEVENTF_MOVE, 0, deltaY, 0, 0);

                // Sleep for a short duration before the next movement (e.g., 100-300ms)
                Thread.Sleep(random.Next(10));
            }
        }

        private void pressAndHoldKey(string key, int durationMilliseconds)
        {
            // Press the key down
            SendKeys.SendWait(key);
            Thread.Sleep(durationMilliseconds);
            // Release the key
            SendKeys.SendWait("{" + key + " up}");

        }

        public void MuteWindows()
        {
            // Load the keys from CommandConfigData.json
            string muteDuration;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                muteDuration = configData?.muteTime;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(muteDuration))
            {
                Console.WriteLine("No time set.");
                return;
            }

            if (!int.TryParse(muteDuration, out int muteDurationSeconds))
            {
                Console.WriteLine("Invalid mute duration format.");
                return;
            }

            int muteDurationMilliseconds = muteDurationSeconds * 1000; // Convert seconds to milliseconds

            // Mute the sound
            keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);

            // Block input (to prevent accidental volume changes)
            BlockInput(1);

            // Wait for the specified duration
            System.Threading.Thread.Sleep(muteDurationMilliseconds);

            // Release the mute key
            keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);

            // Unblock input
            BlockInput(0);
        }

        public void FireMode()
        {
            // Load the keys from CommandConfigData.json
            string firemodeKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                firemodeKeyBox = configData?.firemodeKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(firemodeKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            SendKeys.SendWait(firemodeKeyBox);
        }

        public void SwapWeapon()
        {
            // Load the keys from CommandConfigData.json
            string weaponKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                weaponKeyBox = configData?.swapKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(weaponKeyBox))
            {
                Console.WriteLine("No keys to send.");
                return;
            }

            string[] keys = weaponKeyBox.Split(',').Select(k => k.Trim()).ToArray();

            // Shuffle the keys array
            Random rnd = new Random();
            keys = keys.OrderBy(k => rnd.Next()).ToArray();

            foreach (string key in keys)
            {
                SendKeys.SendWait(key);
            }
        }

        public void HotMic()
        {
            // Load the keys from CommandConfigData.json
            string micDuration, micKeyBox;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                micDuration = configData?.micTime;
                micKeyBox = configData?.micKey;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(micDuration))
            {
                Console.WriteLine("No time set.");
                return;
            }

            if (!int.TryParse(micDuration, out int durationSeconds))
            {
                Console.WriteLine("Invalid hotmic duration format.");
                return;
            }

            int durationMilliseconds = durationSeconds * 1000; // Convert seconds to milliseconds
                                                               // Simulate key down
            keybd_event((byte)micKeyBox[0], 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);

            // Wait for the specified duration
            Thread.Sleep(durationMilliseconds);

            // Simulate key up
            keybd_event((byte)micKeyBox[0], 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public void Walk()
        {
            string walkDuration;

            try
            {
                // Read the JSON file and parse it to extract the keys
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                walkDuration = configData?.walkTime;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(walkDuration))
            {
                Console.WriteLine("No time set.");
                return;
            }

            if (!int.TryParse(walkDuration, out int durationSeconds))
            {
                Console.WriteLine("Invalid walk duration format.");
                return;
            }

            int durationMilliseconds = durationSeconds * 1000; // Convert seconds to milliseconds

            // Simulate key down
            keybd_event(VK_W, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);

            // Wait for the specified duration
            Thread.Sleep(durationMilliseconds);

            // Simulate key up
            keybd_event(VK_W, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public void TouchGrass(int durationMilliseconds)
        {

            // Start a new thread for pulling out the grenade
            Thread crouchorStandThread = new Thread(() =>
            {
                CrouchorStand();
            });

            // Start a new thread for spinning
            Thread lookDownThread = new Thread(() =>
            {
                LookDown(durationMilliseconds);
            });

            // Start both threads
            crouchorStandThread.Start();
            lookDownThread.Start();

            // Wait for the grenadeThread to complete
            crouchorStandThread.Join();

        }

        public static int BitBonusMultiplier { get; private set; }

        public static void BitMultiplier()
        {
            string bonusMultiplierBox;
            string configFilePath = Path.Combine("Data", "CommandConfigData.json");

            try
            {
                // Read the JSON file and parse it to extract the multiplier
                string json = File.ReadAllText(configFilePath);
                var configData = JsonConvert.DeserializeObject<ConfigData>(json);
                bonusMultiplierBox = configData?.bonusMultiplierBox;
            }
            catch (Exception ex)
            {
                // Handle any errors, such as file not found or JSON parsing issues
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return;
            }

            if (string.IsNullOrEmpty(bonusMultiplierBox))
            {
                Console.WriteLine("No time set.");
                return;
            }

            if (!int.TryParse(bonusMultiplierBox, out int durationSeconds))
            {
                Console.WriteLine("Invalid multiplier duration format.");
                return;
            }

            BitBonusMultiplier = durationSeconds;
        }

        //Mod Commands

        public static void RefundLastCommand(OnChatCommandReceivedArgs e, string userName, Dictionary<string, int> userBits, TwitchClient client, string channelId)
        {
            // Get the current date and timestamp for the log
            string date = DateTime.Now.ToString("M-d-yy");
            string timestamp = DateTime.Now.ToString("HH:mm:ss");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            if (!File.Exists(logFilePath))
            {
                Console.WriteLine("No log file found for today.");
                return;
            }

            // Read all lines from the log file
            string[] logLines = File.ReadAllLines(logFilePath);

            // Find the last log line for the specified user
            string lastUserLogLine = logLines.LastOrDefault(line => line.Contains($" - {userName} had "));

            if (string.IsNullOrEmpty(lastUserLogLine))
            {
                Console.WriteLine($"No log found for user {userName}.");
                return;
            }

            // Extract information from the last log line for the user
            string[] parts = lastUserLogLine.Split(new[] { " - ", " had ", " bits, used ", " command, costing ", " bits, now has " }, StringSplitOptions.None);

            if (parts.Length < 6)
            {
                Console.WriteLine("Log format is invalid.");
                return;
            }

            string logUserName = parts[1];
            int bitsCost = int.Parse(parts[4]);

            // Refund the bits to the user
            if (userBits.ContainsKey(logUserName))
            {
                userBits[logUserName] += bitsCost; // Add the refunded bits to the current balance

                // Log the refund operation
                int currentTotalBits = userBits[logUserName];
                string logMessage = $"{timestamp} - {e.Command.ChatMessage.DisplayName} refunded {bitsCost} bits to {logUserName}, now has {currentTotalBits} bits";

                // Ensure the Logs directory exists
                if (!Directory.Exists("Logs"))
                {
                    Directory.CreateDirectory("Logs");
                }

                // Append the log message to the file
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

                // Output to console
                Console.WriteLine($"[{timestamp}] {e.Command.ChatMessage.DisplayName} refunded {bitsCost} bits to {logUserName}. They now have {userBits[logUserName]} bits.");

                // Send message to chat
                client.SendMessage(channelId, $"{bitsCost} bits were refunded to {logUserName}. They now have {userBits[logUserName]} bits.");
            }
            else
            {
                Console.WriteLine("User not found in the bits dictionary.");
            }
        }

        public static void AddBitCommand(TwitchClient client, OnChatCommandReceivedArgs e)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");

            string[] args = e.Command.ArgumentsAsString.Split(' ');
            if (args.Length == 2)
            {
                string username = args[0].StartsWith("@") ? args[0].Substring(1) : args[0]; // Remove "@" symbol if present
                int bitsToAdd;
                if (int.TryParse(args[1], out bitsToAdd))
                {
                    // Update user's bits
                    LogHandler.UpdateUserBits(username, bitsToAdd);
                    LogHandler.LogAddbits(e.Command.ChatMessage.DisplayName, "addbits", bitsToAdd, username, MainBot.userBits, timestamp);

                    // Notify about successful update
                    client.SendMessage(e.Command.ChatMessage.Channel, $"{bitsToAdd} bits added to {username}. New total: {MainBot.userBits[username]} bits");
                    Console.WriteLine($"[{timestamp}] User [{e.Command.ChatMessage.DisplayName}] added {bitsToAdd} bits to {username}. New total: {MainBot.userBits[username]} bits");

                }
                else
                {
                    client.SendMessage(e.Command.ChatMessage.Channel, "Invalid number of bits specified.");
                }
            }
        }

    }
}