namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
/// Key value pair for JSON serialisation. Required for .NET Framework support.
/// </summary>
public sealed class KeyValue
{
    public string Keyword { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}