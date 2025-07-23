using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Persistence;


namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;

internal class ReadOnlyOutputJsonIO : IOutputsJsonIO
{
    public IVersionOutputs Load(string directory)
    {
        return new OutputsJsonFileIO().Load(directory);
    }

    public void Write(string directory, IVersionOutputs outputs)
    {
        // do nothing
    }
}