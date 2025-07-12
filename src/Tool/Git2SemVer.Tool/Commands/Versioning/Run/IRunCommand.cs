namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Run;

internal interface IRunCommand
{
    bool HasError { get; }

    void Execute(RunCommandSettings unattended);
}