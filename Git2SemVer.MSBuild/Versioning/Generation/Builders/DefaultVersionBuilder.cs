using System;
using System.Text.RegularExpressions;
using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Framework.Config;
using NoeticTools.Git2SemVer.MSBuild.Framework.Semver;
using NoeticTools.Git2SemVer.MSBuild.Scripting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistory;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;

/// <summary>
///     Git2SemVer's default outputs builder. This builder sets all MSBuild output properties.
/// </summary>
internal sealed class DefaultVersionBuilder : IVersionBuilder
{
    private readonly ILogger _logger;
    private readonly HistoryPaths _paths;

    public DefaultVersionBuilder(HistoryPaths paths, ILogger logger)
    {
        _paths = paths;
        _logger = logger;
    }

    public void Build(VersioningContext context)
    {
        var prereleaseLabel = GetPrereleaseLabel(context);

        var version = GetVersion(prereleaseLabel, context.Host);
        var informationalVersion = GetInformationalVersion(version, context);
        context.Outputs.SetAllVersionPropertiesFrom(informationalVersion);

        var buildSystemLabel = string.IsNullOrWhiteSpace(prereleaseLabel) ? version : version.WithPrerelease(prereleaseLabel, context.Host.BuildId.ToArray());
        context.Outputs.BuildSystemVersion = buildSystemLabel;

        var gitOutputs = context.Outputs.Git;
        var config = Git2SemVerConfiguration.Load();
        config.AddLogEntry(context.Host.BuildNumber,
                           gitOutputs.HasLocalChanges,
                           gitOutputs.BranchName,
                           gitOutputs.HeadCommit.CommitId.ShortSha,
                           context.Inputs.WorkingDirectory);
        config.Save();
    }

    private static SemVersion GetInformationalVersion(SemVersion version, IVersioningContext context)
    {
        var commitId = context.Outputs.Git.HeadCommit.CommitId.Id;
        var branchName = context.Outputs.Git.BranchName.ToNormalisedSemVerIdentifier();
        var host = context.Host;
        var metadata = new List<string>();
        if (version.IsRelease)
        {
            metadata.AddRange(host.BuildId);
        }
        metadata.AddRange([branchName, commitId]);
        return version.WithMetadata(metadata.ToArray());
    }

    private string GetPrereleaseLabel(IVersioningContext context)
    {
        var versionPrefix = _paths.BestPath.Version;
        var labelPrefix = "";
        if (versionPrefix.Major == 0)
        {
            labelPrefix = VersioningConstants.DefaultInitialDevelopmentLabel;
        }

        var inputs = context.Inputs;
        if (VersioningConstants.BranchMaturityPatternReleaseGroupName.Equals(inputs.VersionSuffix,
                                                                             StringComparison.CurrentCultureIgnoreCase))
        {
            return labelPrefix;
        }

        var prereleaseLabel = string.IsNullOrWhiteSpace(inputs.VersionSuffix)
            ? GetPrereleaseLabelFromBranchName(context)
            : inputs.VersionSuffix;

        if (labelPrefix.Length > 0)
        {
            prereleaseLabel = char.ToUpper(prereleaseLabel[0]) + prereleaseLabel.Substring(1);
        }

        return labelPrefix + prereleaseLabel;
    }

    private static string GetPrereleaseLabelFromBranchName(IVersioningContext context)
    {
        var inputs = context.Inputs;
        var branchName = context.Outputs.Git.BranchName;
        var pattern = string.IsNullOrWhiteSpace(inputs.BranchMaturityPattern)
            ? VersioningConstants.DefaultBranchMaturityPattern
            : inputs.BranchMaturityPattern;
        var regex = new Regex(pattern);

        var match = regex.Match(branchName);

        var groupNames = regex.GetGroupNames();
        foreach (var groupName in groupNames)
        {
            if (char.IsDigit(groupName[0]))
            {
                continue;
            }

            var group = match.Groups[groupName];
            if (group.Success)
            {
                return "release".Equals(groupName, StringComparison.InvariantCultureIgnoreCase) ? "" : groupName.ToNormalisedSemVerIdentifier();
            }
        }

        return "UNKNOWN_BRANCH";
    }

    private SemVersion GetVersion(string prereleaseLabel, IBuildHost host)
    {
        var versionPrefix = _paths.BestPath.Version;
        var isARelease = string.IsNullOrWhiteSpace(prereleaseLabel);
        if (isARelease)
        {
            return versionPrefix;
        }

        var prereleaseIdentifiers = new List<string> { prereleaseLabel };
        prereleaseIdentifiers.AddRange(host.BuildId);
        return versionPrefix.WithPrerelease(prereleaseIdentifiers.ToArray());
    }
}