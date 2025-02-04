using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using UiBot.Properties;

namespace UiBot
{
    internal class TarkovInRaidCheck
    {
        private static string _gameFolder = null;
        private static string _currentLogsFolder = null;

        public static string GameFolder
        {
            get
            {
                if (_gameFolder == null)
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\EscapeFromTarkov");
                    var installPath = key?.GetValue("InstallLocation")?.ToString();
                    key?.Dispose();

                    if (!String.IsNullOrEmpty(installPath))
                    {
                        _gameFolder = installPath;
                    }
                }

                return _gameFolder;
            }
        }

        public static string LogsFolder
        {
            get
            {
                if (_currentLogsFolder == null || !Directory.Exists(_currentLogsFolder))
                {
                    _currentLogsFolder = GetLatestLogFolder();
                }
                return _currentLogsFolder;
            }
        }

        private static string GetLatestLogFolder()
        {
            var logDirPath = Path.Combine(GameFolder, "Logs");
            var directories = Directory.GetDirectories(logDirPath);

            if (directories.Length == 0)
            {
                return null;
            }

            var latestDir = directories.OrderByDescending(dir => new DirectoryInfo(dir).CreationTime).First();
            return latestDir;
        }

        private readonly string folder;
        private readonly string searchPattern;
        private readonly int checkInterval;
        private volatile bool isStopping = false;
        private long lastFileSize = 0;
        private FileSystemWatcher fileCreateWatcher;
        private FileSystemWatcher folderWatcher; // Scope the watcher outside the method.

        public event EventHandler<FileChangedEventArgs> Created;
        public event EventHandler<FileChangedEventArgs> Changed;

        public TarkovInRaidCheck(string folder, string searchPattern, int checkInterval = 5000)
        {
            this.folder = folder;
            this.searchPattern = searchPattern;
            this.checkInterval = checkInterval;
        }

        private string TryGetFilePath()
        {
            string[] files = Directory.GetFiles(folder, searchPattern);
            if (files.Length > 0)
            {
                return files[0]; // We don't need to concatenate with the folder, `files[0]` already includes the path.
            }

            return null;
        }

        public void Start()
        {
            Reset();

            var filePath = TryGetFilePath();

            if (!string.IsNullOrEmpty(filePath))
            {
                if (Settings.Default.isDebugOn)
                {
                    Console.WriteLine($"LogFileWatcher: StartFileChangeMonitoring");

                }

                StartFileChangeMonitoring(filePath);
            }
            else
            {
                if (Settings.Default.isDebugOn)
                {
                    Console.WriteLine($"LogFileWatcher: No log file found, starting file creation watcher.");

                }
                fileCreateWatcher = new FileSystemWatcher(folder, searchPattern)
                {
                    EnableRaisingEvents = true
                };
                fileCreateWatcher.Created += OnLogFileCreated;
                fileCreateWatcher.Renamed += OnLogFileCreated;
            }

            StartFolderCreationMonitoring();
        }

        private void StartFileChangeMonitoring(string filePath)
        {
            Task.Run(() => CheckFile(filePath));
        }

        private void OnLogFileCreated(object sender, FileSystemEventArgs e)
        {
            StartFileChangeMonitoring(e.FullPath);
            StopFileCreationMonitoring();
            Created?.Invoke(this, new FileChangedEventArgs(e.FullPath));
        }

        private void CheckFile(string filePath)
        {
            while (!isStopping)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    long currentFileSize = fileInfo.Length;

                    // If the file size has increased, check for new lines
                    if (currentFileSize > lastFileSize)
                    {
                        // Open the log file for reading, allowing other processes to write to it
                        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var reader = new StreamReader(stream))
                        {
                            // Move the reader's position to the previous size so we only check new content
                            reader.BaseStream.Seek(lastFileSize, SeekOrigin.Begin);

                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                // Detect "GameStarted" → Resume chat commands
                                if (line.Contains("GameStarted"))
                                {
                                    if (Settings.Default.isCommandsPaused)
                                    {
                                        Console.WriteLine("Chat Commands are ACTIVE");
                                        // Trigger the pauseCommands checkbox change in ConnectMenu (on UI thread)
                                        if (ConnectMenu.Instance != null && ConnectMenu.Instance.pauseCommands.InvokeRequired)
                                        {
                                            ConnectMenu.Instance.pauseCommands.Invoke(new Action(() =>
                                            {
                                                ConnectMenu.Instance.pauseCommands.Checked = false;  // Ensure it reflects the state in the UI
                                            }));
                                        }

                                        if (Settings.Default.isDebugOn)
                                            Console.WriteLine($"Found 'GameStarted' in the log, resuming chat commands.");
                                    }
                                }

                                // Detect "BEClient exit" or "cancelled" → Pause chat commands
                                if (line.Contains("BEClient exit") || line.Contains("cancelled"))
                                {
                                    if (!Settings.Default.isCommandsPaused)
                                    {
                                        Console.WriteLine("Chat Commands are PAUSED");

                                        // Trigger the pauseCommands checkbox change in ConnectMenu (on UI thread)
                                        if (ConnectMenu.Instance != null && ConnectMenu.Instance.pauseCommands.InvokeRequired)
                                        {
                                            ConnectMenu.Instance.pauseCommands.Invoke(new Action(() =>
                                            {
                                                ConnectMenu.Instance.pauseCommands.Checked = true;  // Ensure it reflects the state in the UI
                                            }));
                                        }

                                        if (Settings.Default.isDebugOn)
                                            Console.WriteLine($"Detected '{line.Trim()}', pausing chat commands.");
                                    }
                                }
                            }
                        }

                        // Update lastFileSize to the current size after checking
                        lastFileSize = currentFileSize;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"LogFileWatcher: Error in CheckFile: {ex.Message}");
                }

                // Sleep for the specified check interval before checking the file again
                Thread.Sleep(checkInterval);
            }
        }

        public void Stop()
        {
            isStopping = true;
            StopFileCreationMonitoring();
            StopFolderCreationMonitoring(); // Ensure the folder monitoring stops too.
        }

        private void StopFileCreationMonitoring()
        {
            if (fileCreateWatcher != null)
            {
                fileCreateWatcher.Created -= OnLogFileCreated;
                fileCreateWatcher.Dispose();
                fileCreateWatcher = null;
            }
        }

        private void StopFolderCreationMonitoring()
        {
            if (folderWatcher != null)
            {
                folderWatcher.Created -= OnNewFolderCreated;
                folderWatcher.Dispose();
                folderWatcher = null;
            }
        }

        private void StartFolderCreationMonitoring()
        {
            folderWatcher = new FileSystemWatcher(Path.Combine(GameFolder, "Logs"))
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.DirectoryName
            };

            folderWatcher.Created += OnNewFolderCreated;
        }

        private void OnNewFolderCreated(object sender, FileSystemEventArgs e)
        {

            // Update to the newest folder after creation
            _currentLogsFolder = GetLatestLogFolder();            
            if (Settings.Default.isDebugOn)
            {
                Console.WriteLine($"New folder created: {e.FullPath}");
                Console.WriteLine($"Switched to latest folder: {_currentLogsFolder}");

            }
        }

        private void Reset()
        {
            isStopping = false;
            lastFileSize = 0;
        }
    }

    public class FileChangedEventArgs : EventArgs
    {
        public string FilePath { get; }

        public FileChangedEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
