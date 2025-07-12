namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;

internal interface IAddPreconditionValidator
{
    bool Validate(DirectoryInfo directory, bool unattended);
}