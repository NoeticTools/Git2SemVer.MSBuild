using Microsoft.Build.Framework;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;
using NoeticTools.MSBuild.TaskLogging;


// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

/// <summary>
///     The Git2SemVer MSBuild task to get the package version from the properties file.
/// </summary>
public class Git2SemVerRestoreVersionPropertiesTask : Git2SemVerTaskBase
{
    private readonly MSBuildTaskLogger _logger;

    public Git2SemVerRestoreVersionPropertiesTask()
    {
        _logger = new MSBuildTaskLogger(Log);
    }

    /// <summary>
    ///     Path to the projects intermediate files directory (ob/).
    ///     Defaults to the MSBuild
    ///     <see
    ///         href="https://learn.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2022#list-of-common-properties-and-parameters">
    ///         BaseIntermediateOutputPath
    ///     </see>
    ///     property.
    /// </summary>
    [Required]
    public string Input_VersionCacheDirectory { get; set; } = "";

    /// <summary>
    ///     Called by MSBuild to execute the task.
    /// </summary>
    public override bool Execute()
    {
        try
        {
            _logger.LogDebug("Restoring version properties.");
            var cache = new GeneratedVersionsJsonFile().Load(Input_VersionCacheDirectory);
            SetOutputs(cache);
            return !Log.HasLoggedErrors;
        }
        catch (Exception exception)
        {
            Log.LogErrorFromException(exception);
            return false;
        }
    }
}