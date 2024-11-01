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

    public CommitMessageMetadata(string changeType, string changeDescription, string body, string footer)
    {
        ChangeDescription = changeDescription;
        ChangeType = ToChangeTypeId(changeType.ToLower());
        Body = body;
        Footer = footer;
        HasBreakingChange = false;
    }

    public bool HasBreakingChange { get; }

    public string Footer { get; }

    public CommitMessageMetadata() : this("", "", "", "")
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