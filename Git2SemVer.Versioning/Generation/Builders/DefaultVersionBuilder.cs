﻿using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Versioning.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Versioning.Framework.Semver;
using NoeticTools.Git2SemVer.Versioning.Generation;
using NoeticTools.Git2SemVer.Versioning.Generation.GitHistoryWalking;
using Semver;


namespace NoeticTools.Git2SemVer.Versioning.Generation.Builders;

/// <summary>
///     Git2SemVer's default outputs builder. This builder sets all MSBuild output properties.
/// </summary>
internal sealed class DefaultVersionBuilder : IVersionBuilder
{
    private readonly ILogger _logger;
    private readonly IHistoryPaths _paths;

    public DefaultVersionBuilder(IHistoryPaths paths, ILogger logger)
    {
        _paths = paths;
        _logger = logger;
    }

    public void Build(IBuildHost host, IGitTool gitTool, IVersionGeneratorInputs inputs, IVersionOutputs outputs)
    {
        _logger.LogDebug("Running default version builder.");
        using (_logger.EnterLogScope())
        {
            var prereleaseLabel = GetPrereleaseLabel(inputs, outputs);

            var version = GetVersion(prereleaseLabel, host);
            _logger.LogDebug("Version: {0}", version.ToString());
            var informationalVersion = GetInformationalVersion(version, host, outputs);
            _logger.LogDebug("Informational version: {0}", informationalVersion.ToString());
            outputs.SetAllVersionPropertiesFrom(informationalVersion,
                                                host.BuildNumber,
                                                host.BuildContext);

            outputs.BuildSystemVersion = GetBuildSystemLabel(host, prereleaseLabel, version);
            _logger.LogTrace($"BuildSystemVersion = {outputs.BuildSystemVersion}");

            var gitOutputs = outputs.Git;
            //var config = Git2SemVerConfiguration.Load();
            //config.AddLogEntry(host.BuildNumber,
            //                   gitOutputs.HasLocalChanges,
            //                   gitOutputs.BranchName,
            //                   gitOutputs.HeadCommit.CommitId.ShortSha,
            //                   inputs.WorkingDirectory);
            //config.Save();
        }
    }

    private SemVersion GetBuildSystemLabel(IBuildHost host, string prereleaseLabel, SemVersion version)
    {
        var buildSystemLabel = version.IsRelease
            ? version.WithMetadata(host.BuildNumber)
            : version.WithPrerelease(prereleaseLabel, host.BuildId.ToArray());
        return buildSystemLabel;
    }

    private static SemVersion GetInformationalVersion(SemVersion version, IBuildHost host, IVersionOutputs outputs)
    {
        var commitId = outputs.Git.HeadCommit.CommitId.Sha;
        var branchName = outputs.Git.BranchName.ToNormalisedSemVerIdentifier();
        var metadata = new List<string>();
        if (version.IsRelease)
        {
            metadata.AddRange(host.BuildId);
        }

        metadata.AddRange([branchName, commitId]);
        return version.WithMetadata(metadata.ToArray());
    }

    private string GetPrereleaseLabel(IVersionGeneratorInputs inputs, IVersionOutputs outputs)
    {
        var versionPrefix = _paths.BestPath.Version;
        var initialDevSuffix = "";
        if (versionPrefix.Major == 0)
        {
            initialDevSuffix = VersioningConstants.InitialDevelopmentLabel;
        }

        if (VersioningConstants.ReleaseGroupName.Equals(inputs.VersionSuffix,
                                                        StringComparison.Ordinal))
        {
            return initialDevSuffix;
        }

        var prereleaseLabel = string.IsNullOrWhiteSpace(inputs.VersionSuffix)
            ? GetPrereleaseLabelFromBranchName(inputs, outputs)
            : inputs.VersionSuffix;
        if (!string.IsNullOrWhiteSpace(prereleaseLabel) &&
            !string.IsNullOrWhiteSpace(initialDevSuffix))
        {
            initialDevSuffix = "-" + initialDevSuffix;
        }

        return prereleaseLabel + initialDevSuffix;
    }

    private static string GetPrereleaseLabelFromBranchName(IVersionGeneratorInputs inputs, IVersionOutputs outputs)
    {
        var branchName = outputs.Git.BranchName;
        var pattern = string.IsNullOrWhiteSpace(inputs.BranchMaturityPattern)
            ? VersioningConstants.DefaultBranchMaturityPattern
            : inputs.BranchMaturityPattern;
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

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
                return "release".Equals(groupName, StringComparison.Ordinal) ? "" : groupName.ToNormalisedSemVerIdentifier();
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