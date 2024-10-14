using _21120127_Week04.Model;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System;

namespace _21120127_Week04.ModelView
{
    public class BookFormModelView
    {
        public Book CurrentBook { get; set; }

        public ICommand SaveBookCommand { get; }

        public BookFormModelView()
        {
            CurrentBook = DAO.BookDAO.GetBook().Result;
            SaveBookCommand = new RelayCommand(SaveBook, CanSaveBook);
        }

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
