namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class HandledChange
{
    public string ChangeType { get; set; } = "";

    public string Description { get; set; } = "";

    public List<string> Issues { get; set; } = [];
}