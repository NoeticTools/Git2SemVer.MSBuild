using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Testing.Core.ConventionalCommits;

public abstract class MessageMetadataStubBase(
    string changeType,
    string scope,
    string changeDescription,
    string body,
    bool breakingChangeFlagged,
    FooterKeyValues footerKeyValues,
    ConventionalCommitsSettings convCommitsSettings)
    : ICommitMessageMetadata
{
    private readonly CommitMessageMetadata _inner = new(changeType, scope, changeDescription, body, breakingChangeFlagged, footerKeyValues,
                                                        convCommitsSettings);

    protected MessageMetadataStubBase(string changeType)
        : this(changeType, "", "", "", false, new FooterKeyValues(), new ConventionalCommitsSettings())
    {
    }

    public ApiChangeFlags ApiChangeFlags => _inner.ApiChangeFlags;

    public string Body => _inner.Body;

    public string ChangeType => _inner.ChangeType;

    public string Description => _inner.Description;

    public FooterKeyValues FooterKeyValues => _inner.FooterKeyValues;

    public IReadOnlyList<string> Issues => _inner.Issues;

    public string Scope => _inner.Scope;
}