namespace ShareQR
{
    public interface IFileHelper
    {
        string SharedDirectoryPath { get; }

        string GetSharedFilePath(string fileName);
    }
}
