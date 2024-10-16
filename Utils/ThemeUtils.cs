using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace _21120127_Week04.Utils
{
    public class ThemeUtils
    {
        private readonly static string ThemeKey = "AppTheme";

        public static string GetStoredLocalTheme()
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey(ThemeKey))
                {
                    return localSettings.Values[ThemeKey].ToString();
                }
                return "Default"; // Default theme
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found)
                return "Default"; // Default theme
            }
        }

        public static void SetStoredLocalTheme(string theme)
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values[ThemeKey] = theme;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found)
            }
        }
    }
}