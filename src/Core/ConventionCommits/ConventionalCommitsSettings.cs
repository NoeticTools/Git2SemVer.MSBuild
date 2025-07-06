using System.Text.Json.Serialization;

namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
/// Conventional Commits settings.
/// </summary>
public sealed class ConventionalCommitsSettings
{
    /// <summary>
    /// Commit message footer keys will comma-delimited list of issues.
    /// </summary>
    [JsonPropertyOrder(20)]
    public string[] IssueKeys { get; set; } =
    [
        "issues",
        "issue",
        "ref",
        "refs"
    ];
}
