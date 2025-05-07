using System.Diagnostics;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Testing.Core;


namespace NoeticTools.Git2SemVer.IntegrationTests.Framework;

internal static class TestLoggerFactory
{
    private static int _loggerId = 1;

    /// <summary>
    /// Create a logger appropriate for the host environment.
    /// </summary>
    public static ILogger Create()
    {
        var teamCityVersion = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");
        return !string.IsNullOrWhiteSpace(teamCityVersion) ? CreateTeamCityFileStreamLogger() : new NUnitLogger();
    }

    /// <summary>
    /// Log to file and stream file to TeamCity's build log. This mitigates risk of collision of standard output and TC service messages. 
    /// </summary>
#pragma warning disable CA1859
    private static ILogger CreateTeamCityFileStreamLogger()
#pragma warning restore CA1859
    {
        var outputFileDir = Path.Combine(Environment.GetEnvironmentVariable("TMPDIR")!, "TestResults");
        if (!Directory.Exists(outputFileDir))
        {
            Directory.CreateDirectory(outputFileDir);
            Wait(() => Directory.Exists(outputFileDir));
        }

        var outputFilePath = Path.Combine(outputFileDir, $"test{_loggerId++:D3}.txt");
        if (!File.Exists(outputFilePath))
        {
            File.Delete(outputFilePath);
            Wait(() => !File.Exists(outputFilePath));
        }

        var logger = new FileLogger(outputFilePath);
        logger.LogInfo("Logging started.");
        Wait(() => File.Exists(outputFilePath));

        Console.Out.WriteLine($"##teamcity[importData type='streamToBuildLog' filePath='{outputFilePath}' wrapFileContentInBlock='false' charset='UTF-8']");
        Console.Out.Flush();
        return logger;
    }

    private static void Wait(Func<bool> predicate)
    {
        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.ElapsedMilliseconds < 3000)
        {
            if (predicate())
            {
                break;
            }

            Thread.Sleep(1);
        }
    }
}