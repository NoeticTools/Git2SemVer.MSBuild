namespace NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;

public interface ICommitObfuscator
{
    string GetObfuscatedSha(string sha);
}