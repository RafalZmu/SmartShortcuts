using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShortcuts.Models
{
    public class SettingsManager
    {
        public Dictionary<string, string> Properties;
        public string Path { get; set; }

        public SettingsManager(string? path = null)
        {
            if (path is null)
                Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SmartShortcuts\\settings.json";
            else
                Path = path;

            CreateSettingFile();
            Properties = GetSettingsFromFile();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(Properties, Formatting.Indented);
            File.WriteAllText(Path, json);
        }

        private Dictionary<string, string> GetSettingsFromFile()
        {
            string json = File.ReadAllText(Path);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// Create settings file if it doesn't exist
        /// </summary>
        private void CreateSettingFile()
        {
            if (!File.Exists(Path))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SmartShortcuts");
                File.Create(Path);
                var settings = new Dictionary<string, string>
                {
                    { "AccentColor", "#066D08" }
                };
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                File.WriteAllText(Path, json);
            }
        }
    }
}