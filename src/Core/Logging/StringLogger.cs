using System.Text;


namespace NoeticTools.Git2SemVer.Core.Logging;

public sealed class StringLogger : LoggerShowingLevelBase, ILogger
{
    private readonly StringBuilder _stringBuilder = new();

    public void Dispose()
    {
    }

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }

    protected override void WriteLine(string message)
    {
        _stringBuilder.AppendLine(message);
    }
}