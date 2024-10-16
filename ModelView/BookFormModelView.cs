using _21120127_Week04.Model;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System;
using _21120127_Week04.Utils;
using Microsoft.UI.Xaml;

namespace _21120127_Week04.ModelView
{
    public class BookFormModelView: INotifyPropertyChanged
    {
        private bool _isDarkMode = false;

        public bool _isInitialized = false;

        public bool IsInitialized
        {
            get; set;
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                _isDarkMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDarkMode)));
            }
        }

        public Book CurrentBook { get; set; }

        public ICommand SaveBookCommand { get; }

        public BookFormModelView()
        {
            CurrentBook = DAO.BookDAO.GetBook().Result;
            SaveBookCommand = new RelayCommand(SaveBook, CanSaveBook);
            IsDarkMode = ThemeUtils.GetStoredLocalTheme() == "Dark";
        }

        private void UpdateTheme()
        {
            var app = Application.Current as App;
            if (app != null)
            {
                app.ChangeTheme(IsDarkMode ? "Dark" : "Light", true, false);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void SaveBook()
        {
            await DAO.BookDAO.SaveBook(CurrentBook);
        }

        private bool CanSaveBook()
        {
            //return !string.IsNullOrEmpty(CurrentBook.Title);
            return true;
        }
    }
}
