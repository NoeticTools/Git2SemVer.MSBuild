using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Testing.Core.ConventionalCommits;

public sealed class FixMessageMetadataStub : MessageMetadataStubBase
{
    public FixMessageMetadataStub()
        : base("fix", "", "", "", false, new FooterKeyValues(), new ConventionalCommitsSettings())
    {
    }

    public FixMessageMetadataStub(string scope,
                                  string changeDescription,
                                  string body,
                                  bool breakingChangeFlagged,
                                  FooterKeyValues footerKeyValues,
                                  ConventionalCommitsSettings convCommitsSettings)
        : base("fix", scope, changeDescription, body, breakingChangeFlagged, footerKeyValues, convCommitsSettings)
    {
    }
}