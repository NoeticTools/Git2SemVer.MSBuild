using System.Reflection;


namespace NoeticTools.Git2SemVer.Core;

public sealed class EmbeddedResources<T> : IEmbeddedResources
{
    private readonly Assembly _assembly = typeof(T).Assembly;

    public string GetResourceFileContent(string filename)
    {
        return _assembly.GetResourceFileContent(filename);
    }

    public void WriteResourceFile(string resourceFilename, string destinationPath)
    {
        _assembly.WriteResourceFile(resourceFilename, destinationPath);
    }

    public void WriteResourceFile(string filename, DirectoryInfo destination)
    {
        var destinationPath = destination.WithFile(filename).FullName;
        _assembly.WriteResourceFile(filename, destinationPath);
    }
}