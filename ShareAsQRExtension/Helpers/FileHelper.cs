using System;
using System.IO;
using Foundation;
using ShareAsQRExtension;
using ShareQR.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace ShareAsQRExtension
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

        public bool SaveByteArray(byte[] byteArr, string path)
        {
            var hasSaved = true;

            try
            {
                var bw = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate));
                bw.Write(byteArr);
                bw.Flush();
                bw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
                hasSaved = false;
            }

            return hasSaved;
        }

        public bool RemoveFile(string path)
        {
            var hasRemoved = true;

            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
                hasRemoved = false;
            }

            return hasRemoved;
        }
    }
}
