﻿using Moq;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Testing.Core;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

internal class GitHistoryWalkingTestsContext : IDisposable
{
    private readonly GitTool _gitTool;

    public GitHistoryWalkingTestsContext()
    {
        Logger = new NUnitLogger(false) { Level = LoggingLevel.Trace };
        Repository = new Mock<ICommitsRepository>();
        _gitTool = new GitTool(Logger);
    }

    private List<Commit> GetCommits(string gitLog)
    {
        var commits = new List<Commit>();
        foreach (var logLine in gitLog.Split('\n'))
        {
            _gitTool.ParseLogLine(logLine.Trim(), [], commits);
        }

        return commits;
    }

    public Mock<ICommitsRepository> Repository { get; }

    public NUnitLogger Logger { get; }

    public Dictionary<string, Commit> SetupGitRepository(LoggedScenario scenario)
    {
        var commits = GetCommits(scenario.ActualGitLog).ToDictionary(k => k.CommitId.Id, v => v);
        Repository.Setup(x => x.Get(It.IsAny<CommitId>())).Returns<CommitId>(id => commits[id.Id]);
        Repository.Setup(x => x.Head).Returns(commits[scenario.HeadCommitId]);
        return commits;
    }

    public void Dispose()
    {
        Logger.Dispose();
    }
}