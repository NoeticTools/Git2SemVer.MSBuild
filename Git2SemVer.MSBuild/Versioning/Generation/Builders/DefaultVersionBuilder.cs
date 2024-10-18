using System.Text.RegularExpressions;
using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Framework.Config;
using NoeticTools.Git2SemVer.MSBuild.Framework.Semver;
using NoeticTools.Git2SemVer.MSBuild.Scripting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;

/// <summary>
///     Git2SemVer's default outputs builder. This builder sets all MSBuild output properties.
/// </summary>
internal sealed class DefaultVersionBuilder : IVersionBuilder
{
    private readonly IHistoryPaths _paths;

    public DefaultVersionBuilder(IHistoryPaths paths, ILogger logger)
    {
        _paths = paths;
    }

    public void Build(IBuildHost host, IVersionGeneratorInputs inputs, IVersionOutputs outputs)
    {
        var prereleaseLabel = GetPrereleaseLabel(inputs, outputs);

        var version = GetVersion(prereleaseLabel, host);
        var informationalVersion = GetInformationalVersion(version, host, outputs);
        outputs.SetAllVersionPropertiesFrom(informationalVersion,
                                                    host.BuildNumber,
                                                    host.BuildContext);

        var buildSystemLabel = string.IsNullOrWhiteSpace(prereleaseLabel)
            ? version
            : version.WithPrerelease(prereleaseLabel, host.BuildId.ToArray());
        outputs.BuildSystemVersion = buildSystemLabel;

        var gitOutputs = outputs.Git;
        var config = Git2SemVerConfiguration.Load();
        config.AddLogEntry(host.BuildNumber,
                           gitOutputs.HasLocalChanges,
                           gitOutputs.BranchName,
                           gitOutputs.HeadCommit.CommitId.ShortSha,
                           inputs.WorkingDirectory);
        config.Save();
    }

    private static SemVersion GetInformationalVersion(SemVersion version, IBuildHost host, IVersionOutputs outputs)
    {
        var commitId = outputs.Git.HeadCommit.CommitId.Id;
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
                                                        StringComparison.CurrentCultureIgnoreCase))
        {
            return initialDevSuffix;
        }

        var prereleaseLabel = string.IsNullOrWhiteSpace(inputs.VersionSuffix)
            ? GetPrereleaseLabelFromBranchName(inputs, outputs)
            : inputs.VersionSuffix;
        if (!string.IsNullOrWhiteSpace(prereleaseLabel))
        {
            prereleaseLabel += "-";
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