namespace ShareQR.Helpers
{
    public interface IFileHelper
    {
        string SharedDirectoryPath { get; }

        string GetSharedFilePath(string fileName);

        bool SaveByteArray(byte[] byteArr, string path);

		bool RemoveFile(string path);
    }
}