using System.Text.RegularExpressions;
using Injectio.Attributes;
using NoeticTools.Common.Exceptions;
using NoeticTools.Common.Logging;
using Semver;


namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
[RegisterTransient]
public class GitTool : IGitTool
{
    private readonly IGitProcessCli _inner;
    private readonly ILogger _logger;

    public GitTool(ILogger logger)
    {
        _logger = logger;
        _inner = new GitProcessCli(logger);
        BranchName = GetBranchName();
        HasLocalChanges = GetHasLocalChanges();
    }

    public string BranchName { get; }

    public bool HasLocalChanges { get; }

    public string WorkingDirectory
    {
        get => _inner.WorkingDirectory;
        set => _inner.WorkingDirectory = value;
    }

    public IReadOnlyList<Commit> GetCommits(int skipCount, int takeCount)
    {
        var commits = new List<Commit>();

        var result = Run($"log --graph --skip={skipCount} --max-count={takeCount} --pretty=\"format:.|%H|%P|%<(30,trunc)%s|%d|\"");

        var obfuscatedGitLog = new List<string>();
        var lines = result.stdOutput.Split('\n');
        foreach (var line in lines)
        {
            _logger.LogTrace($" == {line}.");//>>>
            obfuscatedGitLog.Add(GitObfuscation.ObfuscateLogLine(line));

            if (!line.Contains(" .|"))
            {
                continue;
            }

            var commit = ParseLogLine(line, _logger);
            commits.Add(commit);
        }

        _logger.LogTrace($"Read {commits.Count} commits from git history. Skipped {skipCount}.");
        _logger.LogTrace("Partially obfuscated git log ({0} skipped):\n\n                .|Commit|Parents|Summary|Refs|\n{1}", skipCount,
                         string.Join("\n", obfuscatedGitLog));

        return commits;
    }

    public static Commit ParseLogLine(string line, ILogger logger)
    {
        var regex =
            new
                Regex(@"^(?<graph>[^\.]*)(\.\|(?<sha>[^\|]*)?\|(?<parents>[^\|]*)?\|(?<summary>[^\|]*)?\|(( \(tag: (?<tags>[^\|]+)*\))|([^\|]*))\|)?$",
                      RegexOptions.Multiline);
        var match = regex.Match(line.Trim());
        if (!match.Success)
        {
            logger.LogWarning($"Unexpected git log line: {line}.");
        }

        var sha = GetGroupValue(match, "sha");
        var tags = GetGroupValue(match, "tags")!;
        var parents = GetGroupValue(match, "parents").Split(' ');
        var summary = GetGroupValue(match, "summary");

        var commit = new Commit(sha, parents, summary, tags);
        return commit;
    }

    public (int returnCode, string stdOutput) Run(string arguments)
    {
        var outWriter = new StringWriter();
        var errorWriter = new StringWriter();

        var returnCode = _inner.Run(arguments, outWriter, errorWriter);

        if (returnCode != 0)
        {
            throw new Git2SemVerGitOperationException($"Git command '{arguments}' returned non-zero return code: {returnCode}");
        }

        var errorOutput = errorWriter.ToString();
        if (!string.IsNullOrWhiteSpace(errorOutput))
        {
            _logger.LogError($"Git command '{arguments}' returned error: {errorOutput}");
        }

        return (returnCode, outWriter.ToString());
    }

    private string GetBranchName()
    {
        var result = Run("status -b -s --porcelain");

        var regex = new Regex(@"^## (?<branchName>\S+?)(\.\.\..*)?$", RegexOptions.Multiline);
        var match = regex.Match(result.stdOutput);

        if (!match.Success)
        {
            _logger.LogError($"Unable to read branch name from Git status response '{result.stdOutput}'.");
        }

        return match.Groups["branchName"].Value;
    }

    private string GetVersion()
    {
        var process = new ProcessCli(_logger);
        var result = process.Run("git", "--version");
        if (result.returnCode != 0)
        {
            _logger.LogError($"Unable to read git version. Return code was '{result.returnCode}'.");
        }

        return result.stdOutput;
    }

    private static string GetGroupValue(Match match, string groupName)
    {
        var group = match.Groups[groupName];
        return group.Success ? group.Value : "";
    }

    private bool GetHasLocalChanges()
    {
        var result = Run("status -u -s --porcelain");
        return result.stdOutput.Length > 0;
    }
}