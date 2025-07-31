using System.Diagnostics;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.ChangeLogging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Generation.Builders;
using NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using NoeticTools.Git2SemVer.Framework.Persistence;


namespace NoeticTools.Git2SemVer.Framework.Generation;

internal sealed class VersioningEngine(
    IVersionGeneratorInputs inputs,
    IBuildHost host,
    IOutputsJsonIO generatedOutputsJsonFile,
    IGitTool gitTool,
    IGitHistoryWalker gitWalker,
    IDefaultVersionBuilderFactory defaultVersionBuilderFactory,
    IVersionBuilder scriptBuilder,
    IMSBuildGlobalProperties msBuildGlobalProperties,
    ILogger logger)
    : IVersioningEngine
{
    public void Dispose()
    {
        gitTool.Dispose();
    }

    public (VersionOutputs Outputs, SemanticVersionCalcResult Results) OutsideOfBuildRun()
    {
        return GetVersionOutputs();
    }

    public (IVersionOutputs versionOutputs, SemanticVersionCalcResult calcData) PrebuildRun()
    {
        var stopwatch = Stopwatch.StartNew();

        host.BumpBuildNumber();
        var (outputs, calcData) = GetVersionOutputs();

        SaveGeneratedVersions(outputs);

        stopwatch.Stop();

        logger.LogInfo($"Informational version: {outputs.InformationalVersion}");
        logger.LogDebug($"Version generation completed (in {stopwatch.Elapsed.TotalSeconds:F1} seconds).");
        host.ReportBuildStatistic("git2semver.runtime.seconds", stopwatch.Elapsed.TotalSeconds);

        return (outputs, calcData);
    }

    private (VersionOutputs Outputs, SemanticVersionCalcResult Results) GetVersionOutputs()
    {
        var calcResult = gitWalker.CalculateSemanticVersion();
        var outputs = new VersionOutputs(new GitOutputs(gitTool,
                                                        calcResult.PriorReleaseVersion,
                                                        calcResult.PriorReleaseCommitId,
                                                        calcResult.PriorVersions),
                                         calcResult.Version);
        RunBuilders(outputs);

        if (inputs.WriteConventionalCommitsInfo)
        {
            SaveConventionalCommitsInfo(outputs, calcResult.Contributing);
        }

        return (outputs, calcResult);
    }

    private void RunBuilders(VersionOutputs outputs)
    {
        logger.LogDebug("Running version builders.");
        using (logger.EnterLogScope())
        {
            var stopwatch = Stopwatch.StartNew();

            var defaultBuilder = defaultVersionBuilderFactory.Create(outputs.Version!);
            defaultBuilder.Build(host, gitTool, inputs, outputs, msBuildGlobalProperties);

            scriptBuilder.Build(host, gitTool, inputs, outputs, msBuildGlobalProperties);

            stopwatch.Stop();
            logger.LogDebug($"Version building completed (in {stopwatch.Elapsed.TotalSeconds:F1} sec).");
        }
    }

    private void SaveConventionalCommitsInfo(VersionOutputs outputs, ContributingCommits contributing)
    {
        var conventionalCommitsInfo = new ConventionalCommitsVersionInfo(outputs, contributing);
        var filePath = Path.Combine(inputs.IntermediateOutputDirectory, ChangelogConstants.DefaultConvCommitsInfoFilename);
        conventionalCommitsInfo.Save(filePath);
        if (inputs.VersioningMode == VersioningMode.StandAloneProject)
        {
            return;
        }

        logger.LogDebug("Saving conventional commits info file to '{0}'.", filePath);
        conventionalCommitsInfo.Save(Path.Combine(inputs.SolutionSharedDirectory, ChangelogConstants.DefaultConvCommitsInfoFilename));
    }

    private void SaveGeneratedVersions(VersionOutputs outputs)
    {
        generatedOutputsJsonFile.Write(inputs.IntermediateOutputDirectory, outputs);
        if (inputs.VersioningMode != VersioningMode.StandAloneProject)
        {
            generatedOutputsJsonFile.Write(inputs.SolutionSharedDirectory, outputs);
        }
    }
}