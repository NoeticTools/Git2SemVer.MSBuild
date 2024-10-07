namespace NoeticTools.Git2SemVer.Tool.Commands.Add;

internal interface ISetupCommand
{
    bool HasError { get; }

    void Execute(string inputSolutionFile, bool unattended);
}