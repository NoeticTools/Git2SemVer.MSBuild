namespace NoeticTools.Git2SemVer.Core.Tools;

public interface IContentEditor
{
    string RemoveLinesWith(string signature, string content);
}