using _21120127_Week04.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Windows.Storage;
using System.IO;

namespace _21120127_Week04.Utils
{
    public class FileUtils
    {
        public static string GetRelativePath(string absolutePath, string basePath)
        {
            Uri baseUri = new Uri(basePath);
            Uri fileUri = new Uri(absolutePath);
            return baseUri.MakeRelativeUri(fileUri).ToString();
        }

        public static byte[] GetBytes(string filePath) {
            return System.IO.File.ReadAllBytes(filePath);
        }

        public static byte[] OpenAnAssetFile(string path)
        {
            // Get the app's installation folder
            StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            // Combine the assets path with the file path
            string filePath = Path.Combine(installedLocation.Path, path);

            // Read the binary file contents
            byte[] fileData = File.ReadAllBytes(filePath);

            return fileData;

        }
    }
}
