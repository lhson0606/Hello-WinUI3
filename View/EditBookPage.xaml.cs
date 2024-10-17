using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using _21120127_Week04.ModelView;
using Windows.Storage.Pickers;
using WinUI3Localizer;
using _21120127_Week04.Utils;
using System.Resources;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace _21120127_Week04.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditBookPage : Page
    {

        public BookFormModelView BookFormModelView { get; set; } = new BookFormModelView();

        public EditBookPage()
        {
            this.InitializeComponent();
            //DataContext = BookFormModelView;
        }

        private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = App.MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Get the absolute path of the selected file
                string absolutePath = file.Path;
                LogUtils.Debug($"Absolute Path: {absolutePath}");

                // Compute the relative path based on a known base directory
                string baseDirectory = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
                string relativePath = Utils.FileUtils.GetRelativePath(absolutePath, baseDirectory);
                LogUtils.Debug($"Relative Path: {relativePath}");

                // Set the cover image
                BookFormModelView.CurrentBook.CoverImageData = Utils.FileUtils.GetBytes(absolutePath);
            }
        }

        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string selectedLanguage = e.AddedItems[0].ToString();
                LogUtils.Debug($"Selected Language: {selectedLanguage}");

                if (selectedLanguage == "EN")
                {
                    Localizer.Get().SetLanguage("en");
                }
                else if (selectedLanguage == "VN")
                {
                    Localizer.Get().SetLanguage("vn");
                }
            }
        }

        private async void tsDarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            if(!BookFormModelView.IsInitialized)
            {
                BookFormModelView.IsInitialized = true;
                return;
            }

            // Get the toggle switch state
            var app = App.Current as App;
            Uri uriTheme = tsDarkMode.IsOn ? new Uri("ms-appx:///Themes/DarkTheme.xaml") : new Uri("ms-appx:///Themes/LightTheme.xaml");
            String themeKey = tsDarkMode.IsOn ? "Dark" : "Light";
            // Apply the theme
            app.ChangeTheme(themeKey, true);
            //await app.RestartAppAsync();
            //SolidColorBrush foreground = Resources["ButtonForegroundThemeBrush"] as SolidColorBrush;
            //btnSave.Foreground = foreground;
            await DialogUtils.ShowDialogAsync("Change Theme", "The theme has been changed successfully! You will need to restart the app for the action to take effects.",
                XamlRoot
                );
        }
    }
}
