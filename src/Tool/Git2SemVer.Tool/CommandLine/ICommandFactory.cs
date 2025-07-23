using NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Remove;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;


namespace NoeticTools.Git2SemVer.Tool.CommandLine;

internal interface ICommandFactory
{
    ISetupCommand CreateAddCommand();

    IChangelogCommand CreateChangelogCommand();

    IRemoveCommand CreateRemoveCommand();

    IRunCommand CreateRunCommand();
}