using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.data.Models;

namespace WorldCupStats.data.Helpers
{
    public static class SettingsManager
    {
        private static readonly string AppDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "WorldCupStats"
        );


        private static readonly string SettingsFile = Path.Combine(AppDataFolder, "settings.json");

        // Cache the settings in memory
        private static AppSettings _cachedSettings;
        private static DateTime _lastLoadTime;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

        // Events for settings changes
        public static event EventHandler<SettingsChangedEventArgs> SettingsChanged;

        static SettingsManager()
        {
            EnsureDirectoryExists();
        }

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);
        }

        public static AppSettings LoadSettings(bool forceReload = false)
        {
            try
            {
                // Return cached settings if valid
                if (!forceReload && _cachedSettings != null &&
                    DateTime.Now - _lastLoadTime < CacheExpiration)
                {
                    return _cachedSettings;
                }
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    _cachedSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                    _lastLoadTime = DateTime.Now;

                    // Validate and fix if needed
                    ValidateSettings(_cachedSettings);

                    return _cachedSettings;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load settings", ex);
            }

            // Return default settings
            _cachedSettings = new AppSettings();
            _lastLoadTime = DateTime.Now;
            return _cachedSettings;
        }


        public static void SaveSettings(AppSettings settings)
        {
            try
            {
                EnsureDirectoryExists();

                //Validate settings before saving
                ValidateSettings(settings);

                // Save new settings
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsFile, json);

                // Update cache
                _cachedSettings = settings;
                _lastLoadTime = DateTime.Now;

                // Raise event
                SettingsChanged?.Invoke(null, new SettingsChangedEventArgs(settings));

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save settings", ex);
            }


        }

        public static void UpdateSetting(Action<AppSettings> updateAction)
        {
            var settings = LoadSettings();
            updateAction(settings);
            SaveSettings(settings);
        }

        public static string GetChampionship() => LoadSettings().Championship;
        public static string GetLanguage() => LoadSettings().Language;
        public static bool IsApiMode() => LoadSettings().UseApi;
        public static string GetFavoriteTeam() => LoadSettings().FavoriteTeam;
        public static string GetWindowMode() => LoadSettings().WindowMode;

        public static void SetWindowMode(string windowMode)
        {
            if (windowMode != "normal" && windowMode != "large" && windowMode != "xlarge")
            {
                windowMode = "normal"; // Default to normal if invalid
            }

            UpdateSetting(s => s.WindowMode = windowMode);
        }

        // Set championship
        public static void SetChampionship(string championship)
        {
            if (championship != "men" && championship != "women")
                throw new ArgumentException("Championship must be 'men' or 'women'");

            UpdateSetting(s => s.Championship = championship);
        }

        // Set language
        public static void SetLanguage(string language)
        {
            if (language != "en" && language != "hr")
                throw new ArgumentException("Language must be 'en' or 'hr'");

            UpdateSetting(s => s.Language = language);
        }

        // Set API/File mode
        public static void SetDataMode(bool useApi)
        {
            UpdateSetting(s => s.UseApi = useApi);
        }

        // Favorite team management
        public static void SetFavoriteTeam(string teamCode)
        {
            UpdateSetting(s => s.FavoriteTeam = teamCode);
        }

        // Favorite players management
        public static void SetFavoritePlayers(string teamCode, List<int> playerNumbers)
        {
            if (playerNumbers.Count > 3)
                throw new ArgumentException("Maximum 3 favorite players allowed");

            UpdateSetting(s =>
            {
                s.FavoriteTeam = teamCode;
                s.FavoritePlayers = playerNumbers;
            });
        }

        public static List<int> GetFavoritePlayers(string teamCode)
        {
            var settings = LoadSettings();
            if (settings.FavoriteTeam == teamCode)
                return settings.FavoritePlayers ?? new List<int>();
            return new List<int>();
        }

        // Player image management
        public static void SavePlayerImagePath(string teamCode, string playerName, int shirtNumber, string imagePath)
        {
            UpdateSetting(s =>
            {
                var key = $"{teamCode}_{playerName}_{shirtNumber}";
                s.PlayerImagePaths[key] = imagePath;
            });
        }

        public static string GetPlayerImagePath(string teamCode, string playerName, int shirtNumber)
        {
            var settings = LoadSettings();
            var key = $"{teamCode}_{playerName}_{shirtNumber}";
            return settings.PlayerImagePaths.ContainsKey(key) ? settings.PlayerImagePaths[key] : null;
        }

        public static void RemovePlayerImagePath(string teamCode, string playerName, int shirtNumber)
        {
            UpdateSetting(s =>
            {
                var key = $"{teamCode}_{playerName}_{shirtNumber}";
                s.PlayerImagePaths.Remove(key);
            });
        }

        // First run handling
        public static bool IsFirstRun()
        {
            return LoadSettings().IsFirstRun;
        }

        public static void SetFirstRunComplete()
        {
            UpdateSetting(s => s.IsFirstRun = false);
        }

        // Reset settings
        public static bool ResetSettings()
        {
            try
            {
                if (File.Exists(SettingsFile))
                    File.Delete(SettingsFile);

                _cachedSettings = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Export settings (for backup)
        public static bool ExportSettings(string exportPath)
        {
            try
            {
                var settings = LoadSettings();
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(exportPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Import settings
        public static void ImportSettings(string importPath)
        {
            try
            {
                if (!File.Exists(importPath))
                    throw new FileNotFoundException("Import file not found");

                var json = File.ReadAllText(importPath);
                var settings = JsonConvert.DeserializeObject<AppSettings>(json);

                ValidateSettings(settings);
                SaveSettings(settings);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to import settings", ex);
            }
        }

        // Validate settings
        private static void ValidateSettings(AppSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            // Ensure valid championship
            if (settings.Championship != "men" && settings.Championship != "women")
                settings.Championship = "men";

            // Ensure valid language
            if (settings.Language != "en" && settings.Language != "hr")
                settings.Language = "en";

            // Ensure favorite players list exists
            settings.FavoritePlayers ??= new List<int>();

            // Limit favorite players to 3
            if (settings.FavoritePlayers.Count > 3)
                settings.FavoritePlayers = settings.FavoritePlayers.Take(3).ToList();

            // Ensure image paths dictionary exists
            settings.PlayerImagePaths ??= new Dictionary<string, string>();

            // Validate window dimensions
            if (settings.WindowWidth < 800)
                settings.WindowWidth = 1024;
            if (settings.WindowHeight < 600)
                settings.WindowHeight = 768;
        }

        
    }

    // Event args for settings changes
    public class SettingsChangedEventArgs : EventArgs
    {
        public AppSettings NewSettings { get; }

        public SettingsChangedEventArgs(AppSettings settings)
        {
            NewSettings = settings;
        }
    }

    // Localization helper that uses settings
    public static class LocalizationHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            ["en"] = new Dictionary<string, string>
            {
                ["app.title"] = "World Cup Statistics",
                ["team.select"] = "Select Team:",
                ["players.all"] = "All Players",
                ["players.favorites"] = "Favorite Players",
                ["btn.save"] = "Save",
                ["btn.cancel"] = "Cancel",
                ["msg.saved"] = "Settings saved successfully!",
                ["msg.error"] = "An error occurred",
                ["settings.championship"] = "Championship",
                ["settings.language"] = "Language",
                ["settings.dataMode"] = "Data Source"
            },
            ["hr"] = new Dictionary<string, string>
            {
                ["app.title"] = "Statistike Svjetskog Prvenstva",
                ["team.select"] = "Odaberite Tim:",
                ["players.all"] = "Svi Igrači",
                ["players.favorites"] = "Omiljeni Igrači",
                ["btn.save"] = "Spremi",
                ["btn.cancel"] = "Odustani",
                ["msg.saved"] = "Postavke uspješno spremljene!",
                ["msg.error"] = "Dogodila se greška",
                ["settings.championship"] = "Prvenstvo",
                ["settings.language"] = "Jezik",
                ["settings.dataMode"] = "Izvor Podataka"
            }
        };

        public static string Get(string key)
        {
            var language = SettingsManager.GetLanguage();

            if (Translations.ContainsKey(language) && Translations[language].ContainsKey(key))
                return Translations[language][key];

            // Fallback to English
            if (Translations["en"].ContainsKey(key))
                return Translations["en"][key];

            // Return key if translation not found
            return key;
        }

        // Quick access
        public static string AppTitle => Get("app.title");
        public static string TeamSelect => Get("team.select");
        public static string AllPlayers => Get("players.all");
        public static string FavoritePlayers => Get("players.favorites");
        public static string SaveButton => Get("btn.save");
        public static string CancelButton => Get("btn.cancel");
    }

}
