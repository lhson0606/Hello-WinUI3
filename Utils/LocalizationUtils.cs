using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using WinUI3Localizer;

namespace _21120127_Week04.Utils
{
    public class LocalizationUtils
    {
        public static readonly string RESOURCE_FILE_NAME = "lang";
        public static readonly string RESOURCE_FILE_EXTENSION = "resx";
        public static readonly string RESOURCE_ROOT_FOLDER = "Resources";
        public static readonly string RESOURCE_LANG_FOLDER = "Languages";
        public static readonly string RESOURCE_FILE_DEFAULT = "default";

        public static async Task InitializeLocalizer()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            //Create Resources folder if not exists
            StorageFolder resourcesFolder = await localFolder.CreateFolderAsync(
                RESOURCE_ROOT_FOLDER,
                CreationCollisionOption.OpenIfExists);
            //Create Languages folder inside Resources folder if not exists
            StorageFolder languagesFolder = await resourcesFolder.CreateFolderAsync(
                RESOURCE_LANG_FOLDER,
                CreationCollisionOption.OpenIfExists);

            await CreateStringResourceFileIfNotExists(languagesFolder, RESOURCE_FILE_DEFAULT);
            await CreateStringResourceFileIfNotExists(languagesFolder, "en");
            await CreateStringResourceFileIfNotExists(languagesFolder, "vn");

            ILocalizer localizer = await new LocalizerBuilder()
                .AddStringResourcesFolderForLanguageDictionaries(stringsFolder.Path)
                .SetOptions(options =>
                {
                    options.DefaultLanguage = "en";
                })
                .Build();

            // Debug: Check if dictionaries are loaded
            var dictionaries = localizer.GetLanguageDictionaries();
            foreach (var dictionary in dictionaries)
            {
                if (dictionary.GetItemsCount() > 0)
                {
                    LogUtils.Debug($"Language: {dictionary.Language}, Keys: {string.Join(", ", dictionary.GetItems())}");
                }
            }
        }

        private static async Task CreateFolderIfNotExists(StorageFolder parentFolder, string folderName)
        {
            await parentFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
        }

        private static async Task CreateStringResourceFileIfNotExists(StorageFolder langFolder, string language)
        {
            string languageFileName = GetResourceFileName(language);

            StorageFile? existingFile = await languageFolder.TryGetItemAsync(resourceFileName) as StorageFile;
            string resourceFilePath = System.IO.Path.Combine(stringsFolder.Name, language, resourceFileName);
            StorageFile sourceFile = await LoadStringResourcesFileFromAppResource(resourceFilePath);

            bool shouldCopy = false;

            if (existingFile is null)
            {
                shouldCopy = true;
            }
            else
            {
                var sourceProperties = await sourceFile.GetBasicPropertiesAsync();
                var destinationProperties = await existingFile.GetBasicPropertiesAsync();

                if (sourceProperties.DateModified > destinationProperties.DateModified)
                {
                    shouldCopy = true;
                }
            }

            if (shouldCopy)
            {
                _ = await sourceFile.CopyAsync(languageFolder, resourceFileName, NameCollisionOption.ReplaceExisting);
            }
        }

        private static async Task<StorageFile> LoadStringResourcesFileFromAppResource(string filePath)
        {
            Uri resourcesFileUri = new($"ms-appx:///{filePath}");
            return await StorageFile.GetFileFromApplicationUriAsync(resourcesFileUri);
        }

        private static string GetResourceFileName(string language)
        {
            if (language == RESOURCE_FILE_DEFAULT)
            {
                return $"{RESOURCE_FILE_NAME}.{RESOURCE_FILE_EXTENSION}";
            }
            else
            {
                return $"{RESOURCE_FILE_NAME}.{language}.{RESOURCE_FILE_EXTENSION}";
            }
        }

    }
}
