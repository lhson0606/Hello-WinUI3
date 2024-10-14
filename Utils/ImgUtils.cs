using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace _21120127_Week04.Utils
{
    public class ImgUtils
    {
        public static BitmapImage GetBitmapFromPath(string path)
        {
            var bitmapImage = new BitmapImage(new Uri(path));
            return bitmapImage;
        }
    }
}
