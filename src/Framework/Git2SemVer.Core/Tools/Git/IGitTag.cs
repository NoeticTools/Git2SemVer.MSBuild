namespace NoeticTools.Git2SemVer.Core.Tools.Git;

public interface IGitTag
{
    string CanonicalName { get; }

    string FriendlyName { get; }

    bool IsAnnotated { get; }
}