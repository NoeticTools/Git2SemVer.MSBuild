using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;

internal interface IGeneratedOutputsFile
{
    VersionOutputs Load(string directory);
    void Save(string directory, VersionOutputs outputs);
}