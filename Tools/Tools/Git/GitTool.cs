using System.Text.RegularExpressions;
using Injectio.Attributes;
using NoeticTools.Common.Exceptions;
using NoeticTools.Common.Logging;
using Semver;
#pragma warning disable SYSLIB1045


namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
[RegisterTransient]
public class GitTool : IGitTool
{
    private const string GitLogParsingPattern =
        @"^(?<graph>[^\x1f$]*)(\x1f\.\|(?<sha>[^\|]+)?\|(?<parents>[^\|]*)?\|\x02(?<summary>[^\x03]*)?\x03\|\x02(?<body>[^\x03]*)?\x03\|(\s\((?<refs>.*?)\))?\|$)?";
    private readonly IGitProcessCli _inner;
    private readonly ILogger _logger;
    private readonly string _gitLogFormat;
    private const char RecordSeparator = ControlCharacterConstants.RS;

    public GitTool(ILogger logger)
    {
        _gitLogFormat = "%x1f.|%H|%P|%x02%s%x03|%x02%b%x03|%d|%x1e";
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

        var result = Run($"log --graph --skip={skipCount} --max-count={takeCount} --pretty=\"format:{_gitLogFormat}\"");

        var obfuscatedGitLog = new List<string>();
        var lines = result.stdOutput.Split(RecordSeparator); // todo - inadequate

        _logger.LogTrace($"Read {commits.Count} commits from git history. Skipped {skipCount}.");
        foreach (var line in lines)
        {
            ParseLogLine(line, obfuscatedGitLog, commits);
        }

        _logger.LogTrace($"Read {commits.Count} commits from git history. Skipped {skipCount}.");
        _logger.LogTrace("Partially obfuscated git log ({0} skipped):\n\n                .|Commit|Parents|Summary|Body|Refs|\n{1}", skipCount,
                         string.Join("\n", obfuscatedGitLog));

        return commits;
    }

    public void ParseLogLine(string line, List<string> obfuscatedGitLog, List<Commit> commits)
    {
        line = line.Trim();
        var regex = new Regex(GitLogParsingPattern, RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        var match = regex.Match(line);
        if (!match.Success)
        {
            throw new Git2SemVerGitLogParsingException($"Unable to parse Git log line {line}.");
        }

        var graph = GetGroupValue(match, "graph");
        var sha = GetGroupValue(match, "sha");
        var refs = GetGroupValue(match, "refs")!;
        var parents = GetGroupValue(match, "parents").Split(' ');
        var summary = GetGroupValue(match, "summary");
        var body = GetGroupValue(match, "body");

        var commit = line.Contains($"{ControlCharacterConstants.US}.|") ? new Commit(sha, parents, summary, body, refs): null;
        if (commit != null)
        {
            commits.Add(commit);
        }

        obfuscatedGitLog.Add(GetObfuscatedLogLine(graph, commit, refs));
    }

    private string GetObfuscatedLogLine(string graph, Commit? commit, string refs)
    {
        if (commit == null)
        {
            return $"{graph,-12}";
        }

        var redactedRefs = new Regex(@"HEAD -> \S+?(?=[,\)])").Replace(refs, "HEAD -> REDACTED_BRANCH");
        var redactedRefs2 = new Regex(@"origin\/\S+?(?=[,\)])").Replace(redactedRefs, "origin/REDACTED_BRANCH");
        var parentShas = commit.Parents.Length > 0 ? string.Join(" ", commit.Parents.Select(x => x.ObfuscatedSha)) : string.Empty;
        var sha = commit.CommitId.ObfuscatedSha;
        return $"{graph,-15} .|{sha}|{parentShas}|REDACTED|{redactedRefs2}|";
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

        return ParseStatusResponseBranchName(result.stdOutput);
    }

    public static string ParseStatusResponseBranchName(string stdOutput)
    {
        var regex = new Regex(@"^## (?<branchName>[a-zA-Z0-9!$*\._\/-]+?)(\.\.\..*)?\s*?$", RegexOptions.Multiline);
        var match = regex.Match(stdOutput);
        
        if (!match.Success)
        {
            throw new Git2SemVerGitOperationException($"Unable to read branch name from Git status response '{stdOutput}'.\n");
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