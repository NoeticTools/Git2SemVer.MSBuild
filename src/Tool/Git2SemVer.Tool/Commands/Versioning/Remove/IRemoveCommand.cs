namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Remove;

internal interface IRemoveCommand
{
    bool HasError { get; }

    void Execute(string inputSolutionFile, bool unattended);
}