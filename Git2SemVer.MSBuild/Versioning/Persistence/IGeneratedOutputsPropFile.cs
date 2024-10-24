using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;

internal interface IGeneratedOutputsPropFile
{
    void Write(string directory, VersionOutputs outputs);
}