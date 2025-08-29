using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Models
{
    public class AppSettings
    {
        // Core settings
        public string Championship { get; set; } // "men" or "women"
        public string Language { get; set; } // "en" or "hr"
        public bool UseApi { get; set; } = false; // false = JSON files, true = API

        // Team and Player favorites
        public string FavoriteTeam { get; set; }
        public List<int> FavoritePlayers { get; set; } = new List<int>();

        // Window settings (for WPF)
        public string WindowMode { get; set; } = "windowed"; // "windowed" or "fullscreen"
        public int WindowWidth { get; set; } = 1024;
        public int WindowHeight { get; set; } = 768;

        // Display preferences
        public bool ShowPlayerImages { get; set; } = true;
        public bool AutoSave { get; set; } = true;
        public string Theme { get; set; } = "light"; // "light" or "dark"

        // Cache settings
        public DateTime LastCacheUpdate { get; set; }
        public bool UseCache { get; set; } = true;

        // First run flag
        public bool IsFirstRun { get; set; } = true;

        // Player image mappings (team_player -> imagePath)
        public Dictionary<string, string> PlayerImagePaths { get; set; } = new Dictionary<string, string>();

        // paths
        public static string SettingsPath = "settings.json";
        public static string ImagesFolder = "Images";
    }
}
