﻿using System;
using System.IO;
using Foundation;
using ShareQR.ShareAsQRExtension;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace ShareQR.ShareAsQRExtension
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
            if (!fileName.EndsWith(".db", StringComparison.Ordinal))
            {
                throw new Exception($"Please give a {fileName} ending with db.");
            }

            return Path.Combine(SharedDirectoryPath, fileName);
        }
    }
}
