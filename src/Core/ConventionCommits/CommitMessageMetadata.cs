using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

[JsonDerivedType(typeof(ICommitMessageMetadata))]
public sealed class CommitMessageMetadata : ICommitMessageMetadata
{
    private readonly ConventionalCommitsSettings _convCommitsSettings;

    public CommitMessageMetadata(string changeType,
                                 string changeDescription,
                                 string body,
                                 bool breakingChangeFlagged,
                                 Dictionary<string, List<string>> footerKeyValues,
                                 ConventionalCommitsSettings convCommitsSettings)
    {
        _convCommitsSettings = convCommitsSettings;
        ChangeType = changeType.ToLower();
        ChangeDescription = changeDescription;
        Body = body;
        FooterKeyValues = footerKeyValues;

        var functionalityChange = string.Equals(ChangeType, "feat", StringComparison.InvariantCultureIgnoreCase);
        var fix = string.Equals(ChangeType, "fix", StringComparison.InvariantCultureIgnoreCase);
        var breakingChange = breakingChangeFlagged ||
                             FooterKeyValues.ContainsKey("BREAKING-CHANGE") ||
                             FooterKeyValues.ContainsKey("BREAKING CHANGE");

        ApiChangeFlags = new ApiChangeFlags(breakingChange, functionalityChange, fix);
    }

    public CommitMessageMetadata(ConventionalCommitsSettings convCommitsSettings)
        : this("", "", "", false, [], convCommitsSettings)
    {
    }

    public ApiChangeFlags ApiChangeFlags { get; }

    public string Body { get; }

    public string ChangeDescription { get; }

    public string ChangeType { get; }

    public Dictionary<string, List<string>> FooterKeyValues { get; }

    public IReadOnlyList<string> Issues
    {
        get
        {
            var issues = new List<string>();
            foreach (var issueKey in _convCommitsSettings.IssueKeys)
            {
                // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
                if (!FooterKeyValues.ContainsKey(issueKey))
                {
                    continue;
                }
                issues.AddRange(FooterKeyValues[issueKey]);
            }

            return issues;
        }
    }

    public static CommitMessageMetadata Null => new(new ConventionalCommitsSettings());
}