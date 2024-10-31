using System.Text.RegularExpressions;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.Config;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

#pragma warning disable CS1591
internal sealed class ConventionalCommitParser
{
    private readonly ILogger _logger;
    private readonly Regex _regex;

    public ConventionalCommitParser(ILogger logger)
    {
        _logger = logger;
        _regex = new Regex(Git2SemVerConfiguration.Load().ConventionalCommitsPattern);
    }

    public ApiChanges Parse(Commit commit)
    {
        var changes = new ApiChanges();

        var matches = _regex.Matches(commit.Summary);
        foreach (Match match in matches)
        {
            var bumpsMajor = match.Groups["breakingChange"].Success || match.Groups["break"].Success;
            var bumpsMinor = match.Groups["feature"].Success;
            var bumpsPatch = match.Groups["fix"].Success;

            if (bumpsMajor)
            {
                _logger.LogDebug($"Major bump (breaking change) found on commit {commit.CommitId.ShortSha}");
            }

            if (bumpsMinor)
            {
                _logger.LogDebug($"Minor bump (feature) found on commit {commit.CommitId.ShortSha}");
            }

            if (bumpsPatch)
            {
                _logger.LogDebug($"Patch bump (fix) found on commit {commit.CommitId.ShortSha}");
            }

            changes.BreakingChange |= bumpsMajor;
            changes.FunctionalityChange |= bumpsMinor;
            changes.Patch |= bumpsPatch;

            var otherGroup = match.Groups["other"];
            if (otherGroup.Success)
            {
                _logger.LogDebug($"Conventional commit element {otherGroup.Value}");
            }
        }

        return changes;
    }
}