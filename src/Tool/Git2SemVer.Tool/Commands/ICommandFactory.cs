using NoeticTools.Git2SemVer.Tool.Commands.Changelog;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Remove;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Run;


namespace NoeticTools.Git2SemVer.Tool.Commands;

internal interface ICommandFactory
{
    ISetupCommand CreateAddCommand();

    IChangelogCommand CreateChangelogCommand();

    IRemoveCommand CreateRemoveCommand();

    IRunCommand CreateRunCommand();
}