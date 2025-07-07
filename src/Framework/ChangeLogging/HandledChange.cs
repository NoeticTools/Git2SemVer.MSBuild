namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class HandledChange
{
    public string ChangeType { get; set; } = "";

    public string Description { get; set; } = "";

    public List<string> Issues { get; set; } = [];

    public bool TryAddIssues(IEnumerable<string> issues)
    {
        var newIssues = issues.Where(x => !Issues.Contains(x)).ToList();
        Issues.AddRange(newIssues);
        return newIssues.Count > 0;
    }
}