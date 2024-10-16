using _21120127_Week04.Utils;
using Microsoft.UI;
using Microsoft.UI.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using WinUI3Localizer;

namespace _21120127_Week04
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            await DAO.BookDAO.Initialize();
            await LocalizationUtils.InitializeLocalizer();
            LanguageDictionary? dictionary = Localizer.Get()
                .GetLanguageDictionaries()
                .FirstOrDefault(x => x.Language is "en");

            m_window = new MainWindow();
            var storedTheme = ThemeUtils.GetStoredLocalTheme();
            //ChangeTheme(new Uri("ms-appx:///Themes/DarkTheme.xaml"));
            ChangeTheme(storedTheme);
            m_window.Activate();
        }


        private static Window m_window;

        public static Window MainWindow
        {
            get
            {
                return m_window;
            }
        }

        public async Task RestartAppAsync()
        {
            var result = await CoreApplication.RequestRestartAsync("Restarting application");
            LogUtils.Debug($"Failed to restart the app: {result}");
        }

        /*public void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = themeUri };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme); 
        }*/

        public async void ChangeTheme(String themeKey, bool willStoreSetting = false, bool willRestart = false)
        {
            ////ResourceDictionary Theme = new ResourceDictionary() { Source = themeUri };
            //ResourceDictionary temp = App.Current.Resources.ThemeDictionaries[themeKey] as ResourceDictionary;
            //if (temp == null)
            //{
            //    LogUtils.Debug("Theme not found");
            //    return;
            //}
            //ResourceDictionary Theme = new ResourceDictionary() { Source = themeUri };
            // Retrieve the theme dictionary from the application's resources
            ResourceDictionary originalTheme = App.Current.Resources.ThemeDictionaries[themeKey] as ResourceDictionary;

            if (originalTheme == null)
            {
                // Log or handle the case where the theme is not found
                LogUtils.Debug("Theme not found");
                return;
            }

            // Create a new ResourceDictionary and copy the resources from the original theme
            ResourceDictionary newTheme = new ResourceDictionary();
            foreach (var key in originalTheme.Keys)
            {
                newTheme[key] = originalTheme[key];
            }

            // Clear the current resources and add the new theme
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(newTheme);

            if (willStoreSetting)
            {
                ThemeUtils.SetStoredLocalTheme(themeKey);
            }

            if(willRestart)
            {
                await RestartAppAsync();
            }
        }
    }
}
