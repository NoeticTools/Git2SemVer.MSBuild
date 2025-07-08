using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using Spectre.Console;


namespace NoeticTools.Git2SemVer.Core.Console;

[RegisterSingleton]
public class ConsoleIO(ILogger logger) : IConsoleIO
{
    private const string RegexHighlightMarkupPattern = @"\[(error|warn|em|code|bad|good)\]";

    public bool HasError => logger.HasError;

    public LoggingLevel Level { get; set; } = LoggingLevel.Info;

    public bool Unattended { get; set; }

    public T Ask<T>(string prompt, T defaultValue)
    {
        if (Unattended)
        {
            return defaultValue;
        }

        var result = AnsiConsole.Ask(prompt, defaultValue);
        WriteMarkupDebugLine($"User returned '{result}' to ask '{prompt}'.");
        return result;
    }

    public void WriteMarkupErrorLine(string message)
    {
        WriteMarkupLine(message);
        logger.LogError(RemoveNamedColoursMarkup(message));
    }

    public void WriteMarkupLine(string message)
    {
        var markupText = Regex.Replace(message, RegexHighlightMarkupPattern, NamedColoursEvaluator,
                                       RegexOptions.Compiled | RegexOptions.IgnoreCase);
        AnsiConsole.MarkupLine(markupText);
    }

    public void WriteMarkupWarningLine(string message)
    {
        WriteMarkupLine(message);
        logger.LogWarning(RemoveNamedColoursMarkup(message));
    }

    public T Prompt<T>(TextPrompt<T> prompt, T defaultValue)
    {
        Git2SemVerArgumentException.ThrowIfNull(prompt, nameof(prompt));

        if (Unattended)
        {
            if (prompt.Validator != null)
            {
                var result = prompt.Validator(defaultValue);
                if (!result.Successful)
                {
                    throw new
                        Git2SemVerConfigurationException($"No valid option in unattended mode for '{prompt}' default '{defaultValue}': {result.Message}");
                }
            }

            logger.LogDebug($"Unattended used default '{defaultValue}' for prompt '{prompt}'.");
            return defaultValue;
        }

        {
            var result = AnsiConsole.Prompt(prompt.DefaultValue(defaultValue));
            logger.LogDebug($"User returned '{result}' to prompt '{prompt}'.");
            return result;
        }
    }

    public bool PromptYesNo(string prompt, bool defaultValue = true)
    {
        if (Unattended)
        {
            logger.LogDebug($"Unattended used default '{defaultValue}' for yes/no prompt '{prompt}'.");
            return defaultValue;
        }

        var result = AnsiConsole.Prompt(new TextPrompt<bool>("Proceed?")
                                        .AddChoices([true, false])
                                        .DefaultValue(defaultValue)
                                        .WithConverter(choice => choice ? "y" : "n"));
        logger.LogDebug($"User returned '{result}' to prompt yes/no '{prompt}'.");
        return result;
    }

    public void WriteMarkupDebugLine(string message)
    {
        if (Level >= LoggingLevel.Debug)
        {
            AnsiConsole.MarkupLine("[grey]" + message + "[/]");
        }

        if (logger.Level >= LoggingLevel.Debug)
        {
            logger.LogDebug(message);
        }
    }

    public void WriteErrorLine(Exception exception)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (exception == null)
        {
            return;
        }

        WriteMarkupLine($"[error]Exception: {exception.Message}[/]\nStack Trace: {exception.StackTrace}");
        logger.LogError(exception);
    }

    public void WriteErrorLine(string message)
    {
        WriteMarkupLine("[error]" + message + "[/]");
        logger.LogError(message);
    }

    public void WriteMarkupInfoLine(string message)
    {
        WriteMarkupLine(message);
        logger.LogInfo(message);
    }

    public void WriteLine()
    {
        AnsiConsole.WriteLine();
    }

    public void WriteLine(string message)
    {
        AnsiConsole.WriteLine(message);
    }

    public string Escape(string message)
    {
        return Spectre.Console.Markup.Escape(message);
    }

    public void WriteWarningLine(string message)
    {
        AnsiConsole.WriteLine(message);

        if (logger.Level >= LoggingLevel.Warning)
        {
            logger.LogWarning(message);
        }
    }

    public void WriteCodeLine(string code)
    {
        WriteMarkupLine("[code]" + Escape(code) + "[/]");
    }

    public void WriteHorizontalLine()
    {
        AnsiConsole.Write(new Rule());
    }

    private static string NamedColoursEvaluator(Match match)
    {
        if ("[good]".Equals(match.Value, StringComparison.Ordinal))
        {
            return "[green1]";
        }

        if ("[error]".Equals(match.Value, StringComparison.Ordinal) ||
            "[bad]".Equals(match.Value, StringComparison.Ordinal))
        {
            return "[red]";
        }

        if ("[warn]".Equals(match.Value, StringComparison.Ordinal))
        {
            return "[lightsalmon1]";
        }

        if ("[em]".Equals(match.Value, StringComparison.Ordinal))
        {
            return "[aqua]";
        }

        if ("[code]".Equals(match.Value, StringComparison.Ordinal))
        {
            return "[teal]";
        }

        return match.Value;
    }

    private static string RemoveNamedColoursMarkup(string message)
    {
        var markupRemovedText = Regex.Replace(message, RegexHighlightMarkupPattern, "",
                                              RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return markupRemovedText;
    }
}