namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
///     Conventional Commits parser.
/// </summary>
public interface IConventionalCommitsParser
{
    /// <summary>
    ///     Parse commit message.
    /// </summary>
    /// <param name="commitSummary">Commit summary or title.</param>
    /// <param name="commitMessageBody">Commit body. Does not include the summary.</param>
    /// <returns></returns>
    CommitMessageMetadata Parse(string commitSummary, string commitMessageBody);
}