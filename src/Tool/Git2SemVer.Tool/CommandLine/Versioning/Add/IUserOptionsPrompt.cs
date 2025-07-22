namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;

internal interface IUserOptionsPrompt
{
    UserOptions GetOptions(FileInfo solution);
}