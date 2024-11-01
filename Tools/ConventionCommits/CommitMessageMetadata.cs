using System.Collections.Generic;

namespace NoeticTools.Common.ConventionCommits;

public class CommitMessageMetadata
{
    public string ChangeDescription { get; }

    private readonly Dictionary<string, CommitChangeTypeId> _changeTypeIdLookup = new()
    {
        {"feat", CommitChangeTypeId.Feature},
        {"fix", CommitChangeTypeId.Fix},
        {"build", CommitChangeTypeId.Build},
        {"chore", CommitChangeTypeId.Chore},
        {"ci", CommitChangeTypeId.ContinuousIntegration},
        {"docs", CommitChangeTypeId.Documentation},
        {"style", CommitChangeTypeId.Style},
        {"refactor", CommitChangeTypeId.Refactoring},
        {"perf", CommitChangeTypeId.Performance},
        {"test", CommitChangeTypeId.Testing},
    };

    public CommitMessageMetadata(string changeType, bool breakingChangeFlagged, string changeDescription, string body,
                                 List<(string key, string value)> footerKeyValues)
    {
        ChangeType = ToChangeTypeId(changeType.ToLower());
        ChangeDescription = changeDescription;
        Body = body;
        FooterKeyValues = footerKeyValues.ToLookup(k => k.key, v => v.value);

        HasBreakingChange = breakingChangeFlagged ||
                            FooterKeyValues.Contains("BREAKING-CHANGE") || 
                            FooterKeyValues.Contains("BREAKING CHANGE");
    }

    public bool HasBreakingChange { get; }

    public ILookup<string, string> FooterKeyValues { get; }

    public CommitMessageMetadata() : this("", false, "", "", [])
    {
    }

    public CommitChangeTypeId ChangeType { get; }

    private CommitChangeTypeId ToChangeTypeId(string value)
    {
        if (_changeTypeIdLookup.TryGetValue(value, out var changeTypeId))
        {
            return changeTypeId;
        }
        return CommitChangeTypeId.None;
    }

    public string Body { get; }
}