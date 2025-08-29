using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Helpers
{
    public static class LocalizationManager
    {
        private static ResourceManager resourceManager;

        static LocalizationManager()
        {
            resourceManager = new ResourceManager("WorldCupStats.data.Resources.WorldCupRes", typeof(LocalizationManager).Assembly);
        }

        public static string GetString(string key)
        {
            return resourceManager.GetString(key) ?? key;
        }   

        public static void SetLanguage(String language)
        {
            var culture = new CultureInfo(language == "hr" ? "hr-HR" : "en-US");
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
