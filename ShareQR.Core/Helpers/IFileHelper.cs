namespace ShareQR.Helpers
{
    public interface IFileHelper
    {
        string SharedDirectoryPath { get; }

        string GetSharedFilePath(string fileName);

        bool Save(byte[] byteArr, string path);
    }
}