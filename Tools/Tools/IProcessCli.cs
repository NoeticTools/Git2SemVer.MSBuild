using NoeticTools.Common.Logging;


namespace NoeticTools.Common.Tools;

public interface IProcessCli
{
    ILogger Logger { get; }

    /// <summary>
    ///     Process run time limit in milliseconds.
    /// </summary>
    int TimeLimitMilliseconds { get; set; }

    string WorkingDirectory { get; set; }

    (int returnCode, string stdOutput) Run(string application, string commandLineArguments);

    /// <summary>
    ///     Run dotnet cli with provided command line arguments.
    /// </summary>
    int Run(string application, string commandLineArguments,
            TextWriter standardOut, TextWriter? errorOut = null);
}