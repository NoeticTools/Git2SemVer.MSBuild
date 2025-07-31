using NoeticTools.Git2SemVer.Core.Logging;
using Spectre.Console;


namespace NoeticTools.Git2SemVer.Core.Console;

public interface IConsoleIO
{
    bool HasError { get; }

    LoggingLevel Level { get; set; }

    bool Unattended { get; set; }

    T Ask<T>(string prompt, T defaultValue);

    string Escape(string message);

    T Prompt<T>(TextPrompt<T> prompt, T defaultValue);

    bool PromptYesNo(string prompt, bool defaultValue = true);

    /// <summary>
    ///     Write message to both console and the logger depending on level thresholds.
    ///     The message sent to the console has added markup to format as code.
    /// </summary>
    void WriteCodeLine(string code);

    /// <summary>
    ///     Write message to both console and the logger depending on level thresholds.
    ///     The message sent to the console has added markup to format as an error message.
    /// </summary>
    void WriteErrorLine(string message);

    void WriteErrorLine(Exception exception);

    void WriteHorizontalLine();

    /// <summary>
    ///     Write newline to console.
    /// </summary>
    void WriteLine();

    /// <summary>
    ///     Write message to console without formating.
    /// </summary>
    void WriteLine(string message);

    /// <summary>
    ///     Write message to both console and the logger depending on level thresholds.
    ///     The message sent to the console has added markup to format as a debug message.
    /// </summary>
    void WriteMarkupDebugLine(string message);

    /// <summary>
    ///     Writes markup error message to console.
    ///     Markup is added to the message to format as an error message.
    /// </summary>
    void WriteMarkupErrorLine(string message);

    /// <summary>
    ///     Write message to both console and the logger depending on level thresholds.
    ///     The message sent to the console has added markup to format as an info message.
    /// </summary>
    void WriteMarkupInfoLine(string message);

    /// <summary>
    ///     Writes markup message to console.
    /// </summary>
    void WriteMarkupLine(string message);

    /// <summary>
    ///     Writes markup warning message to console.
    ///     Markup is added to the message to format as a warning message.
    /// </summary>
    void WriteMarkupWarningLine(string message);

    /// <summary>
    ///     Write message to both console to console without formating and to the logger as a warning.
    /// </summary>
    void WriteWarningLine(string message);
}