using System.Diagnostics;


namespace NoeticTools.Git2SemVer.Core.Logging;

[RegisterTransient]
public sealed class FileLogger : LoggerShowingLevelBase, ILogger
{
    private readonly StreamWriter _stream;

    public FileLogger(string filePath)
    {
        Level = LoggingLevel.Trace;

        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                _stream = new StreamWriter(filePath, false) { AutoFlush = true };
                break;
            }
            catch (IOException)
            {
                if (stopwatch.ElapsedMilliseconds < 3000)
                {
                    Thread.Sleep(25);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public void Dispose()
    {
        _stream.Flush();
        _stream.Close();
        _stream.Dispose();
    }

    protected override void WriteLine(string message)
    {
        _stream.WriteLine(message);
    }
}