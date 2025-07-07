using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

[JsonDerivedType(typeof(ICommitMessageMetadata))]
public sealed class CommitMessageMetadata : ICommitMessageMetadata
{
    private static readonly Dictionary<string, CommitChangeTypeId> ChangeTypeIdLookup = new()
    {
        { "feat", CommitChangeTypeId.Feature },
        { "fix", CommitChangeTypeId.Fix },
        { "change", CommitChangeTypeId.Change },
        { "deprecate", CommitChangeTypeId.Deprecate },
        { "remove", CommitChangeTypeId.Remove },
        { "security", CommitChangeTypeId.Security }
    };

    private readonly ConventionalCommitsSettings _convCommitsSettings;

    public CommitMessageMetadata(string changeType,
                                 string changeDescription,
                                 string body,
                                 bool breakingChangeFlagged,
                                 List<(string key, string value)> footerKeyValues,
                                 ConventionalCommitsSettings convCommitsSettings)
    {
        _convCommitsSettings = convCommitsSettings;
        ChangeType = ToChangeTypeId(changeType.ToLower());
        ChangeTypeText = changeType;
        ChangeDescription = changeDescription;
        Body = body;
        FooterKeyValues = footerKeyValues.ToLookup(k => k.key, v => v.value);

        var functionalityChange = ChangeType == CommitChangeTypeId.Feature;
        var fix = ChangeType == CommitChangeTypeId.Fix;
        var breakingChange = breakingChangeFlagged ||
                             FooterKeyValues.Contains("BREAKING-CHANGE") ||
                             FooterKeyValues.Contains("BREAKING CHANGE");

        ApiChangeFlags = new ApiChangeFlags(breakingChange, functionalityChange, fix);
    }

    public CommitMessageMetadata(ConventionalCommitsSettings convCommitsSettings)
        : this("", "", "", false, [], convCommitsSettings)
    {
    }

    [JsonPropertyOrder(11)]
    public ApiChangeFlags ApiChangeFlags { get; }

    [JsonPropertyOrder(12)]
    public string Body { get; }

    [JsonPropertyOrder(13)]
    public string ChangeDescription { get; }

    [JsonPropertyOrder(14)]
    public CommitChangeTypeId ChangeType { get; }

    [JsonPropertyOrder(15)]
    public string ChangeTypeText { get; }

    [JsonPropertyOrder(20)]
    public ILookup<string, string> FooterKeyValues { get; }

    [JsonIgnore]
    public IReadOnlyList<string> Issues
    {
        get
        {
            var issues = new List<string>();
            foreach (var issueKey in _convCommitsSettings.IssueKeys)
            {
                issues.AddRange(FooterKeyValues[issueKey]);
            }

            return issues;
        }
    }

    public static CommitMessageMetadata Null => new(new ConventionalCommitsSettings());

    private static CommitChangeTypeId ToChangeTypeId(string value)
    {
        // ReSharper disable once CanSimplifyDictionaryTryGetValueWithGetValueOrDefault
        if (ChangeTypeIdLookup.TryGetValue(value, out var changeTypeId))
        {
            return changeTypeId;
        }

        return string.IsNullOrWhiteSpace(value) ? CommitChangeTypeId.None : CommitChangeTypeId.Custom;
    }
}