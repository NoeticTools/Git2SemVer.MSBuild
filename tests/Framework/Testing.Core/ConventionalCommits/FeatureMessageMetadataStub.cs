using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Testing.Core.ConventionalCommits;

public sealed class FeatureMessageMetadataStub : MessageMetadataStubBase
{
    public FeatureMessageMetadataStub()
        : this("", "", "")
    {
    }

    public FeatureMessageMetadataStub(string scope,
                                      string changeDescription,
                                      string body)
        : this(scope, changeDescription, body, false, new FooterKeyValues(), new ConventionalCommitsSettings())
    {
    }

    public FeatureMessageMetadataStub(string scope,
                                      string changeDescription,
                                      string body,
                                      bool breakingChangeFlagged,
                                      FooterKeyValues footerKeyValues,
                                      ConventionalCommitsSettings convCommitsSettings)
        : base("feat", scope, changeDescription, body, breakingChangeFlagged, footerKeyValues, convCommitsSettings)
    {
    }
}