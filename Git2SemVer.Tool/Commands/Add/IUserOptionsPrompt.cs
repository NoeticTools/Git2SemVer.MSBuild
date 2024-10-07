namespace NoeticTools.Git2SemVer.Tool.Commands.Add;

internal interface IUserOptionsPrompt
{
    UserOptions GetOptions(FileInfo solution);
}