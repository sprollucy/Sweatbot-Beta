namespace UiBot
{
    internal class LogHandler
    {

        //Log given bits from chat
        public static void LogBits(string userName, int bits, string timestamp)
        {
            string logMessage = $"{timestamp} - {userName} gave {bits} bits";

            // Get the current date for the filename
            string date = DateTime.Now.ToString("M-d-yy");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        public static void LogCommand(string userName, string command, int bitsCost, Dictionary<string, int> userBits, string timestamp)
        {
            // Check if the user's bits information is available in the dictionary
            if (userBits.ContainsKey(userName))
            {
                int bitsBeforeCommand = userBits[userName];

                // Deduct the cost of the command from the user's bits
                int bitsAfterCommand = bitsBeforeCommand - bitsCost;

                // Create the log message
                string logMessage = $"{timestamp} - {userName} had {bitsBeforeCommand} bits, used {command} command, costing {bitsCost} bits, now has {bitsAfterCommand} bits";

                // Get the current date for the filename
                string date = DateTime.Now.ToString("M-d-yy");

                // Construct the log file path with the date in its name
                string logFileName = $"{date} bitlog.txt";
                string logFilePath = Path.Combine("Logs", logFileName);

                // Append the log message to the file
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            else
            {
                // If user's bits information is not available, log without the bit count information
                string logMessage = $"{timestamp} - {userName} used {command} command, costing {bitsCost} bits";

                // Get the current date for the filename
                string date = DateTime.Now.ToString("M-d-yy");

                // Construct the log file path with the date in its name
                string logFileName = $"{date} bitlog.txt";
                string logFilePath = Path.Combine("Logs", logFileName);

                // Append the log message to the file
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
        }

        public static void LogAddbits(string commandUser, string command, int bitsAdded, string targetUser, Dictionary<string, int> userBits, string timestamp)
        {
            // Get the current total bits of the target user
            int currentTotalBits = userBits.ContainsKey(targetUser) ? userBits[targetUser] : 0;

            // Create the log message showing the current total bits, bits added, and the new total
            string logMessage = $"{timestamp} - {commandUser} used {command} command, added {bitsAdded} bits to {targetUser}, who had {currentTotalBits - bitsAdded} bits, now has {currentTotalBits} bits";

            // Get the current date for the filename
            string date = DateTime.Now.ToString("M-d-yy");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }


        public static void LogRefundbits(string commandUser, string refundUsername, Dictionary<string, int> userBits, string timestamp)
        {
            // Get the current total bits of the target user
            int currentTotalBits = userBits.ContainsKey(refundUsername) ? userBits[refundUsername] : 0;

            // Create the log message showing the current total bits, bits added, and the new total
            string logMessage = $"{timestamp} - {commandUser} refunded bits to {refundUsername}, now has {currentTotalBits} bits";

            // Get the current date for the filename
            string date = DateTime.Now.ToString("M-d-yy");

            // Construct the log file path with the date in its name
            string logFileName = $"{date} bitlog.txt";
            string logFilePath = Path.Combine("Logs", logFileName);

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }


    }
}
