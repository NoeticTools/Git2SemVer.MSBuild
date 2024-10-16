using Moq;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;
using NoeticTools.Testing.Common;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

internal abstract class GitHistoryWalkingTestsBase
{
    protected Mock<ICommitsRepository> _repository;
    protected NUnitTaskLogger _logger;

    protected List<Commit> GetCommits(string gitLog)
    {
        var commits = new List<Commit>();
        foreach (var logLine in gitLog.Split('\n'))
        {
            if (!logLine.Contains(" .|"))
            {
                continue;
            }

            var commit = GitTool.ParseLogLine(logLine.Trim(), _logger);
            commits.Add(commit);
        }
        return commits;
    }

    protected Dictionary<string, Commit> SetupGitRepository(LoggedScenario scenario)
    {
        var commits = GetCommits(scenario.ActualGitLog).ToDictionary(k => k.CommitId.Id, v => v);
        _repository.Setup(x => x.Get(It.IsAny<CommitId>())).Returns<CommitId>(id => commits[id.Id]);
        _repository.Setup(x => x.Head).Returns(commits[scenario.HeadCommitId]);
        return commits;
    }

    protected void SetupBase()
    {
        VersionHistorySegment.Reset();
        GitObfuscation.Reset();

        _logger = new NUnitTaskLogger(false) { Level = LoggingLevel.Trace };
        _repository = new Mock<ICommitsRepository>();
    }
}