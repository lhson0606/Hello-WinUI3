using _21120127_Week04.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
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
    }
}
