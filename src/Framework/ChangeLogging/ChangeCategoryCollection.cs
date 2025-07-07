using System.Collections;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeCategoryCollection : IReadOnlyList<ChangeCategory>
{
    private readonly IReadOnlyList<ChangeCategory> _inner;

    public ChangeCategoryCollection(IReadOnlyList<Commit> commits, LastRunData lastRunData, bool incremental,
                                    ChangelogLocalSettings localSettings)
    {
        var remainingCommits = new List<Commit>(commits.Where(x => x.MessageMetadata.ChangeTypeText.Length > 0));
        if (incremental)
        {
            ReconcileHandledChanges(remainingCommits, lastRunData.HandledChanges);
        }

        var orderedCategories = localSettings.Categories.OrderBy(x => x.Order);
        _inner = orderedCategories.Select(category => GetChangeCategory(category, remainingCommits)).ToList();
    }

    public int Count => _inner.Count;

    public ChangeCategory this[int index] => _inner[index];

    public IEnumerator<ChangeCategory> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    private static ChangeCategory GetChangeCategory(CategorySettings categorySettings,
                                                    List<Commit> remainingCommits)
    {
        var changeCategory = new ChangeCategory(categorySettings);
        changeCategory.ExtractFrom(remainingCommits);
        return changeCategory;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static void ReconcileHandledChanges(List<Commit> commits, List<HandledChange> handledChanges)
    {
        var handledChangesLookup = new HandledChangeLookup(handledChanges);
        foreach (var commit in commits.ToArray())
        {
            var messageMetadata = commit.MessageMetadata;
            if (handledChangesLookup.TryGet(messageMetadata, out var handledChange))
            {
                if (!handledChange!.TryAddIssues(messageMetadata.Issues))
                {
                    commits.Remove(commit);
                }
            }
            else
            {
                var newHandledChange = new HandledChange
                {
                    ChangeType = messageMetadata.ChangeTypeText,
                    Description = messageMetadata.ChangeDescription,
                    Issues = messageMetadata.Issues.ToList()
                };
                handledChangesLookup.Add(newHandledChange);
                handledChanges.Add(newHandledChange);
            }
        }
    }
}