namespace NoeticTools.Git2SemVer.Framework.Versioning;

public interface ICommonTaskOptions
{
    /// <summary>
    ///     The working directory.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         Default is the project's directory.
    ///     </para>
    /// </remarks>
    string WorkingDirectory { get; }
}