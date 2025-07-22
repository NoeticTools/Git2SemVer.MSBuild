namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Remove;

internal interface IRemoveCommand
{
    bool HasError { get; }

    void Execute(string inputSolutionFile, bool unattended);
}