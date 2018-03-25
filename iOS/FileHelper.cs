using System;
using System.IO;
using Foundation;
using ShareQR.Helpers;
using ShareQR.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]

namespace ShareQR.iOS
{
    public class FileHelper : IFileHelper
    {
        public string SharedDirectoryPath { get; private set; }

        public FileHelper()
        {
            var fileManager = new NSFileManager();
            var appGroupContainer = fileManager.GetContainerUrl("group.com.ettech.ShareQR") ?? throw new Exception("Please check whether set up App Groups properly.");
            var appGroupContainerPath = appGroupContainer.Path;

            SharedDirectoryPath = appGroupContainerPath;
        }

        public string GetSharedFilePath(string fileName)
        {
            return Path.Combine(SharedDirectoryPath, fileName);
        }

        public bool Save(byte[] byteArr, string path)
        {
            var hasSaved = NSData.FromArray(byteArr).Save(path, false, out NSError error);
            if (!hasSaved)
            {
                Console.WriteLine("Cannot save because " + error.LocalizedDescription + ".");
            }

            return hasSaved;
        }
    }
}