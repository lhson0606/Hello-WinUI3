using _21120127_Week04.Model;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Provider;
using Microsoft.UI.Xaml.Controls;
using _21120127_Week04.Utils;

namespace _21120127_Week04.Db
{
    public interface IBookDb
    {
        public async Task Initialize()
        {
            throw new NotImplementedException();
        }
        Task<Book> GetBookAsync();
        Task<Book> SaveBook(Book book);
    }

    public class MockBookDb : IBookDb
    {
        public readonly static string PATH = "db_book.txt";
        private static StorageFile _file = null;

        public async Task Initialize()
        {
            if (_file != null)
            {
                return;
            }

            // Get the app's installation folder
            StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            /*// Create or get a subfolder within the installation folder
            StorageFolder subFolder = await installedLocation.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);*/

            // Create or open the file within the subfolder
            _file = await installedLocation.CreateFileAsync(PATH, CreationCollisionOption.OpenIfExists);
        }

        public async Task<Book> GetBookAsync()
        {
            await Initialize();
            string jsonString;

            try
            {
                using (var stream = await _file.OpenStreamForReadAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        jsonString = await reader.ReadToEndAsync();
                    }
                }

                return JsonSerializer.Deserialize<Book>(jsonString);
            }
            catch (Exception e)
            {
                //await Utils.DialogUtils.ShowDialogAsync("Get Book", e.Message, App.MainWindow.Content.XamlRoot);
            
                return new Book();
            }
        }

        public async Task<Book> SaveBook(Book book)
        {
            await Initialize();
            CachedFileManager.DeferUpdates(_file);

            try
            {
                string jsonString = JsonSerializer.Serialize(book);
                LogUtils.Debug("Serialized JSON: " + jsonString);

                // Write the JSON string to the file
                using (var stream = await _file.OpenStreamForWriteAsync())
                {
                    stream.SetLength(0); // Clear the file before writing
                    using (var writer = new StreamWriter(stream))
                    {
                        await writer.WriteAsync(jsonString);
                    }
                }
            }
            catch (Exception e)
            {
                await Utils.DialogUtils.ShowDialogAsync("Save Book", e.Message, App.MainWindow.Content.XamlRoot);
            }

            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(_file);
            string message;
            if (status == FileUpdateStatus.Complete)
            {
                message = "File " + _file.Name + " was saved at " + _file.Path + "!";
            }
            else if (status == FileUpdateStatus.CompleteAndRenamed)
            {
                message = "File " + _file.Name + " was renamed and saved.";
            }
            else
            {
                message = "File " + _file.Name + " couldn't be saved.";
            }

            await Utils.DialogUtils.ShowDialogAsync("Save Book", message, App.MainWindow.Content.XamlRoot);

            return book;
        }

        public void Dispose()
        {
            // No need to dispose StorageFile
        }

        ~MockBookDb()
        {
            Dispose();
        }
    }
}
