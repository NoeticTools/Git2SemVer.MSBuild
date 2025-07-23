namespace NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;

internal interface IChangelogCommand
{
    bool HasError { get; }

    void Execute(ChangelogCommandSettings cmdLineSettings);
}