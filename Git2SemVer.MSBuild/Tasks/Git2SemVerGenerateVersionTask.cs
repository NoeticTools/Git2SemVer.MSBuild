using Microsoft.Build.Framework;
using NoeticTools.Common.Exceptions;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Framework.Config;
using NoeticTools.Git2SemVer.MSBuild.Tools.CI;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;
using NoeticTools.MSBuild.Tasking.Logging;
using ILogger = NoeticTools.Common.Logging.ILogger;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

/// <summary>
///     The Git2SemVer MSBuild task.
/// </summary>
/// <remarks>
///     <para>
///         This class exposed properties for inputs from the MSBuild environment and outputs to the environment.
///     </para>
/// </remarks>
public class Git2SemVerGenerateVersionTask : Git2SemVerTaskBase, IVersionGeneratorInputs
{
    /// <summary>
    ///     Optional case-insensitive regular expression that maps branch name to build maturity such as "release" or "beta".
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    /// </remarks>
    public string BranchMaturityPattern { get; set; } = "";

    /// <summary>
    ///     The optional MSBuild <c>Git2SemVer_BuildContext</c> property value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         If set, used to override the <see cref="IBuildHost.BuildContext" /> property.
    ///     </para>
    /// </remarks>
    public string BuildContext { get; set; } = "";

    /// <summary>
    ///     Optional build ID format.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         If set, overrides the host object's <see cref="IBuildHost.BuildIdFormat">BuildIdFormat</see> property to set it
    ///         formats the value of <see cref="IBuildHost.BuildId">BuildId</see>.
    ///         Intended to be used with <c>Custom</c> build host type but can be used with any host.
    ///     </para>
    /// </remarks>
    public string BuildIdFormat { get; set; } = "";

    /// <summary>
    ///     Optional input MSBuild <c>Git2SemVer_BuildContext</c> property value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         If set, used to override the <see cref="IBuildHost.BuildNumber" /> property.
    ///     </para>
    ///     <para>
    ///         <b>TeamCity</b>
    ///     </para>
    ///     <para>
    ///         TeamCity provides the build number in an environment variable <b>BUILD_NUMBER</b> which is is automatically
    ///         read.
    ///     </para>
    ///     <para>
    ///         <b>GitHub</b>
    ///     </para>
    ///     <para>
    ///         Currently GitHub does not provide a build number. However, a unique build ID can be constructed from
    ///         <c>github.run_number</c> and <c>github.run_attempt</c>:
    ///     </para>
    ///     <code>
    ///     Unique build ID = "&lt;github.run_number&gt;-&lt;github.run_attempt&gt;"
    /// </code>
    ///     This can be passed to the build in the GitHub workflow yml like this:
    ///     <code>
    ///    - name: Build NetVersionBuilder
    ///      env:
    ///        BUILD_NUMBER: ${{ github.run_number }}-${{ github.run_attempt }}
    ///      run: |
    ///        dotnet build -p:BuildNumber=${{ env.BUILD_NUMBER }}
    /// </code>
    /// </remarks>
    public string BuildNumber { get; set; } = "";

    /// <summary>
    ///     Required path the C# script file to run.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    /// </remarks>
    [Required]
    public string BuildScriptPath { get; set; } = "";

    /// <summary>
    ///     Path to directory to write versioning report to.
    ///     Defaults to the MSBuild
    ///     <see
    ///         href="https://learn.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2022#list-of-common-properties-and-parameters">
    ///         BaseIntermediateOutputPath
    ///     </see>
    ///     property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    /// </remarks>
    [Required]
    public string IntermediateOutputDirectory { get; set; } = "";

    /// <summary>
    ///     This project's versioning mode. For internal use only.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         If Git2SemVer was installed using the dotnet tool then solution versioning
    ///         is being used and this property is set to either <c>SolutionClientProject</c> or
    ///         <c>SolutionVersioningProject</c>.
    ///     </para>
    ///     <para>
    ///         Otherwise, this project is versioning itself independent of any other solution projects and
    ///         this property is set to <c>StandAloneProject</c>.
    ///     </para>
    ///     <para>
    ///         See also: <seealso cref="VersioningMode" />
    ///     </para>
    /// </remarks>
    public string Mode { get; set; } = "";

    /// <summary>
    ///     When using solution versioning the shared directory that holds generated version properties.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    /// <para>
    ///     Not used if the <see cref="Mode" /> is <c>"StandAloneProject"</c>. Otherwise, required.
    /// </para>
    /// </remarks>
    public string SolutionSharedDirectory { get; set; } = "";

    /// <summary>
    ///     When using solution versioning the path to the shared generated version properties file.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    /// <para>
    ///     Not used if the <see cref="Mode" /> is <c>"StandAloneProject"</c>. Otherwise, required.
    /// </para>
    /// </remarks>
    public string SolutionSharedVersioningPropsFile { get; set; } = "";

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
    [Required]
    public string WorkingDirectory { get; set; } = "";

    /// <summary>
    ///     Optional input MSBuild <c>Git2SemVer_HostType</c> property.
    ///     If set overrides automatic host type detection.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         See <see cref="IBuildHost.HostTypeId" />.
    ///     </para>
    /// </remarks>
    public string HostType { get; set; } = "";

    /// <summary>
    ///     Optional MSBuild <c>Git2SemVer_RunScript</c> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         If set to <c>false</c> prevents the script from being executed.
    ///         If set to <c>true</c> ensure the script is executed and fail the build if the script is not present.
    ///         If <c>null</c> (not set) the script will execute if present but the build will not fail if not present.
    ///         The default is <c>null</c>.
    ///     </para>
    /// </remarks>
    public bool? RunScript { get; set; }

    /// <summary>
    ///     The optional MSBuild <c>Git2SemVer_ScriptArg</c> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         Arguments that are passed on to the user's optional C# script.
    ///         Not used by Git2SemVer.
    ///         Default is empty string.
    ///     </para>
    /// </remarks>
    public string ScriptArgs { get; set; } = "";

    /// <summary>
    ///     The optional MSBuild <c>Git2SemVer_UpdateHostBuildLabel</c> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input.
    ///     </para>
    ///     <para>
    ///         If set to true, Git2SemVer.MSBuild will update the host's build label.
    ///         Default is false.
    ///     </para>
    /// </remarks>
    public bool UpdateHostBuildLabel { get; set; }

    /// <summary>
    ///     Called by MSBuild to execute the task.
    /// </summary>
    public override bool Execute()
    {
        var logger = new CompositeLogger { Level = LoggingLevel.Trace };
        try
        {
#pragma warning disable CA2000
            logger.Add(new MSBuildTaskLogger(Log) { Level = LoggingLevel.Trace });
            var logFilePath = Path.Combine(IntermediateOutputDirectory, "Git2SemVer.MSBuild.log");
            logger.Add(new FileLogger(logFilePath) { Level = LoggingLevel.Trace });
#pragma warning restore CA2000

            logger.LogDebug("Executing Git2SemVer.MSBuild task to generate version.");

            try
            {
                VersioningMode = (VersioningMode)Enum.Parse(typeof(VersioningMode), Mode);
            }
            catch (Exception exception)
            {
                throw new Git2SemVerConfigurationException($"Invalid Git2SemVer_Mode value '{Mode}'.", exception);
            }

            var config = Git2SemVerConfiguration.Load();
            var inputs = this;
            var host = new BuildHostFactory(config, logger).Create(inputs.HostType,
                                                                   inputs.BuildNumber,
                                                                   inputs.BuildContext,
                                                                   inputs.BuildIdFormat);
            var gitTool = new GitTool(logger)
            {
                WorkingDirectory = inputs.WorkingDirectory
            };
            var commitsRepo = new CommitsRepository(gitTool);
            var gitPathsFinder = new PathsFromLastReleasesFinder(commitsRepo, gitTool, logger);

            var defaultBuilderFactory = new DefaultVersionBuilderFactory(logger);
            var scriptBuilder = new ScriptVersionBuilder(logger);
            var versionGenerator = new VersionGenerator(inputs, host,
                                                        new GeneratedVersionsJsonFile(),
                                                        new GeneratedVersionsPropsFile(),
                                                        gitTool, gitPathsFinder, defaultBuilderFactory,
                                                        scriptBuilder, logger);
            SetOutputs(versionGenerator.Run());
            return !Log.HasLoggedErrors;
        }
#pragma warning disable CA1031
        catch (Exception exception)
#pragma warning restore CA1031
        {
            logger.LogError(exception);
            return false;
        }
        finally
        {
            logger.Dispose();
        }
    }

    public VersioningMode VersioningMode { get; private set; }

    public bool ValidateScriptInputs(ILogger logger)
    {
        if (logger == null)
        {
            throw new ArgumentNullException(nameof(logger), "Logger is required.");
        }

        if (string.IsNullOrWhiteSpace(BuildScriptPath))
        {
            logger.LogError($"The script file path (property {nameof(BuildScriptPath)}) is required.");
            return false;
        }

        if (RunScript is not true || File.Exists(BuildScriptPath))
        {
            return true;
        }

        logger.LogError($"The required build script file '{BuildScriptPath}' was not found.");
        return false;
    }
}