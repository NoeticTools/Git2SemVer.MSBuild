using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;

internal interface IGeneratedOutputsJsonFile
{
    VersionOutputs Load(string directory);
    void Write(string directory, VersionOutputs outputs);
}