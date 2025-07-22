namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;

internal interface IAddPreconditionValidator
{
    bool Validate(DirectoryInfo directory, bool unattended);
}