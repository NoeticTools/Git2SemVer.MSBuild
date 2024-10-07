namespace NoeticTools.Git2SemVer.Tool.Commands.Remove;

internal interface IRemoveCommand
{
    bool HasError { get; }

    void Execute(string inputSolutionFile, bool unattended);
}