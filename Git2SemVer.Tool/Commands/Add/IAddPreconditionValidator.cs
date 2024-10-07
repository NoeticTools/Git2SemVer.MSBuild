namespace NoeticTools.Git2SemVer.Tool.Commands.Add;

internal interface IAddPreconditionValidator
{
    bool Validate(DirectoryInfo directory, bool unattended);
}