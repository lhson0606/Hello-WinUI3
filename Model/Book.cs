using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace _21120127_Week04.Model
{

    public class Book : ObservableObject
    {
        private string _title;
        private string _author;
        private int _publishedYear;
        private string _isbn;
        private BitmapImage _cover;
        private byte[] _coverImageData;
        private int _price;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        public int PublishedYear
        {
            get => _publishedYear;
            set => SetProperty(ref _publishedYear, value);
        }

        public string ISBN
        {
            get => _isbn;
            set => SetProperty(ref _isbn, value);
        }

        public BitmapImage Cover
        {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public byte[] CoverImageData
        {
            get => _coverImageData;
            set
            {
                if (SetProperty(ref _coverImageData, value))
                {
                    UpdateCoverImage(value);
                }
            }
        }

        private void UpdateCoverImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                Cover = null;
                return;
            }

            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imageData);
                    writer.StoreAsync().GetResults();
                }

                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                Cover = bitmapImage;
            }
        }

        public Book()
        {
            Title = "";
            Author = "";
            PublishedYear = 0;
            ISBN = "";
            Cover = Utils.ImgUtils.GetBitmapFromPath("ms-appx:///Assets/img/imgPlaceholder.png");
            CoverImageData = Utils.FileUtils.OpenAnAssetFile("Assets/img/imgPlaceholder.png");
        }

    }
}
