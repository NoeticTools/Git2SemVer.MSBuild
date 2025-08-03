using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
///     Conventional Commits settings.
/// </summary>
public sealed class ConventionalCommitsSettings : JsonSettingsFileBase<ConventionalCommitsSettings>
{
    /// <summary>
    ///     Commit message footer tokens that may contain a comma-delimited list of issues.
    /// </summary>
    [JsonPropertyOrder(20)]
    public string[] FooterIssueTokens { get; set; } =
    [
        "issues",
        "issue",
        "ref",
        "refs"
    ];
}