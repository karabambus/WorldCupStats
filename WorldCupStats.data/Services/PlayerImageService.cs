using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.data.Helpers;

namespace WorldCupStats.data.Services
{
    //sets up ijmages for players and manages paths in settings
    public class PlayerImageService
    {
        public static string SavePlayerImage(string teamCode, string playerName, int shirtNumber, string sourceImagePath)
        {
            //save image to app folder
            string savedPath = ImageHelper.SavePlayerImage(teamCode, playerName, shirtNumber, sourceImagePath);

            //create key
            string key = CreatePlayerKey(teamCode, playerName, shirtNumber);
            SettingsManager.UpdateSetting(settings =>
            {
                if (settings.PlayerImagePaths == null)
                    settings.PlayerImagePaths = new Dictionary<string, string>();

                settings.PlayerImagePaths[key] = savedPath;
            });
           

            return savedPath;
        }


        public static string GetPlayerImagePath(string teamCode, string playerName, int shirtNumber)
        {
            string key = CreatePlayerKey(teamCode, playerName, shirtNumber);

            var settings = SettingsManager.LoadSettings();
            if (settings?.PlayerImagePaths?.ContainsKey(key) == true)
            {
                string path = settings.PlayerImagePaths[key];

                // Verify file still exists
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    return path;
                else
                {
                    // Clean up invalid entry
                    RemoveImagePathFromSettings(key);
                    return null;
                }
            }

            return null;
        }

        public static void RemovePlayerImage(string teamCode, string playerName, int shirtNumber)
        {
            
            string imagePath = GetPlayerImagePath(teamCode, playerName, shirtNumber);

            if (!string.IsNullOrEmpty(imagePath))
            {
                ImageHelper.DeletePlayerImageFile(imagePath);
            }

            // Remove from settings
            string key = CreatePlayerKey(teamCode, playerName, shirtNumber);
            RemoveImagePathFromSettings(key);
        }

        private static void RemoveImagePathFromSettings(string key)
        {
            SettingsManager.UpdateSetting(settings =>
            {
                settings.PlayerImagePaths?.Remove(key);
            });
        }

        private static string CreatePlayerKey(string teamCode, string playerName, int shirtNumber)
        {
            return $"{teamCode}_{playerName}_{shirtNumber}";
        }
    }
}
