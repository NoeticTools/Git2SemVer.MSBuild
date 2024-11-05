namespace NoeticTools.Common;

public interface IEmbeddedResources
{
    string GetResourceFileContent(string filename);
    void WriteResourceFile(string resourceFilename, string destinationPath);
    void WriteResourceFile(string filename, DirectoryInfo destination);
}