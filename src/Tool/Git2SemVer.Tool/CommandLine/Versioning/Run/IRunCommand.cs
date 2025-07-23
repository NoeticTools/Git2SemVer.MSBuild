namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;

internal interface IRunCommand
{
    bool HasError { get; }

    void Execute(RunCommandSettings unattended);
}