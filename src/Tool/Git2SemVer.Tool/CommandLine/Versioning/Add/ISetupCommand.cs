namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;

internal interface ISetupCommand
{
    bool HasError { get; }

    void Execute(string inputSolutionFile, bool unattended);
}