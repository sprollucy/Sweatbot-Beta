using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiBot
{
    internal class BanManager
    {
        private static string banListFilePath = Path.Combine("Data", "BanList.json");

        public static HashSet<string> LoadBanList()
        {
            if (File.Exists(banListFilePath))
            {
                string json = File.ReadAllText(banListFilePath);
                return JsonConvert.DeserializeObject<HashSet<string>>(json) ?? new HashSet<string>();
            }
            else
            {
                return new HashSet<string>();
            }
        }

        // Saves the banned users to the BanList.json file
        public static void SaveBanList(HashSet<string> banList)
        {
            string json = JsonConvert.SerializeObject(banList, Formatting.Indented);
            File.WriteAllText(banListFilePath, json);
        }
    }
}
