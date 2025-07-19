using NoeticTools.Git2SemVer.Framework.ChangeLogging;


namespace NoeticTools.Git2SemVer.Framework.Generation;

public interface IVersioningEngine : IDisposable
{
    /// <summary>
    ///     Get information including contributing conventional commits since last (direct) releases.
    /// </summary>
    ConventionalCommitsVersionInfo GetConventionalCommitsInfo();

    /// <summary>
    ///     Perform a prebuild versioning run. Depending on the host may bump the build number.
    /// </summary>
    IVersionOutputs PrebuildRun();
}