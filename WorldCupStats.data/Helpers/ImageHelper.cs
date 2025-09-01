using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;


namespace WorldCupStats.data.Helpers
{
    public static class ImageHelper
    {
        private static readonly string ImageBasePath = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
               "WorldCupStats", "PlayerImages");

        static ImageHelper()
        {
            EnsureDirectoriesExist();
        }

        private static void EnsureDirectoriesExist()
        {
            if (!Directory.Exists(ImageBasePath))
                Directory.CreateDirectory(ImageBasePath);
        }

        public static string GetPlayerImagePath(string teamCode, string playerName)
        {
            var imagePath = Path.Combine(ImageBasePath, $"{teamCode}_{SanitizeFileName(playerName)}.jpg");
            return File.Exists(imagePath) ? imagePath : null;
        }

        // Overload for backward compatibility with shirt number
        public static string GetPlayerImagePath(string teamCode, string playerName, int shirtNumber)
        {
            // Try name first
            var imagePath = GetPlayerImagePath(teamCode, playerName);
            if (imagePath != null) return imagePath;

            // Fallback to shirt number
            var shirtPath = Path.Combine(ImageBasePath, $"{teamCode}_{shirtNumber}.jpg");
            return File.Exists(shirtPath) ? shirtPath : null;
        }
        public static string SavePlayerImage(string teamCode, string playerName, int shirtNumber, string sourceImagePath)
        {
            EnsureDirectoriesExist();

            // Use both name and number for unique identification
            var fileName = $"{teamCode}_{SanitizeFileName(playerName)}_{shirtNumber}.jpg";
            var destPath = Path.Combine(ImageBasePath, fileName);

            // Copy image to destination
            CopyImage(sourceImagePath, destPath);

            return destPath;
        }

        // Remove player image file
        public static bool DeletePlayerImageFile(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static void CopyImage(string sourceImagePath, string destPath)
        {
            // Copy and check format
            if (File.Exists(sourceImagePath))
            {
                // If it's already a JPG, just copy
                if (Path.GetExtension(sourceImagePath).ToLower() == ".jpg" ||
                    Path.GetExtension(sourceImagePath).ToLower() == ".jpeg")
                {
                    File.Copy(sourceImagePath, destPath, true);
                }
                else
                {
                    throw new NotSupportedException("Only JPG images are supported.");
                }
            }
        }

        // Helper to sanitize file names (remove invalid characters)
        private static string SanitizeFileName(string fileName)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var sanitized = fileName;

            foreach (char c in invalid)
            {
                sanitized = sanitized.Replace(c, '_');
            }

            // Also replace spaces with underscores for consistency
            sanitized = sanitized.Replace(' ', '_');

            return sanitized;
        }

        // Get default player image
        public static string GetDefaultPlayerImage()
        {
            // Check if default image exists in resources
            var defaultPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "default-player.png"
            );

            return File.Exists(defaultPath) ? defaultPath : null;
        }
    }
}
