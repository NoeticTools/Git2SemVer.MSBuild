namespace NoeticTools.Git2SemVer.Framework.Versioning;

public interface IVersioningEngine : IDisposable
{
    /// <summary>
    ///     Get information including contributing conventional commits since last (direct) releases.
    /// </summary>
    VersioningOutputs OutsideOfBuildRun();

    /// <summary>
    ///     Perform a prebuild versioning run. Depending on the host may bump the build number.
    /// </summary>
    VersioningOutputs PrebuildRun();
}