namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;

internal interface IUserOptionsPrompt
{
    UserOptions GetOptions(FileInfo solution);
}