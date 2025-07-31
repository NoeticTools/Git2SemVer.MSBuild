using Injectio.Attributes;
using NoeticTools.Git2SemVer.Core;


namespace NoeticTools.Git2SemVer.IntegrationTests.Framework;

[RegisterSingleton]
internal sealed class Git2SemverEmbeddedResources : IEmbeddedResources
{
    private readonly EmbeddedResources<Git2SemverEmbeddedResources> _inner = new();

    public string GetResourceFileContent(string filename)
    {
        return _inner.GetResourceFileContent(filename);
    }

    public void WriteResourceFile(string resourceFilename, string destinationPath)
    {
        _inner.WriteResourceFile(resourceFilename, destinationPath);
    }

    public void WriteResourceFile(string filename, DirectoryInfo destination)
    {
        _inner.WriteResourceFile(filename, destination);
    }
}