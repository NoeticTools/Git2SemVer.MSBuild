using System.Diagnostics;
using Injectio.Attributes;
using NoeticTools.Common.Exceptions;
using NoeticTools.Common.Logging;


namespace NoeticTools.Common.Tools;

[RegisterTransient]
public sealed class ProcessCli : IProcessCli
{
    public ProcessCli(ILogger logger)
    {
        WorkingDirectory = Environment.CurrentDirectory;
        Logger = logger;
    }

    public ILogger Logger { get; }

    public int TimeLimitMilliseconds { get; set; } = 30000;

    public string WorkingDirectory { get; set; }

    public (int returnCode, string stdOutput) Run(string application, string commandLineArguments)
    {
        var outWriter = new StringWriter();
        var errorWriter = new StringWriter();
        var returnCode = Run(application, commandLineArguments, outWriter, errorWriter);
        var output = outWriter.ToString();
        var errorOutput = errorWriter.ToString();
        if (!string.IsNullOrWhiteSpace(errorOutput) && returnCode == 0)
        {
            Logger.LogInfo(output);
            throw new Git2SemVerArgumentException($"ERROR: {errorOutput}\nOUTPUT:\n{output}");
        }

        return (returnCode, output);
    }

    /// <summary>
    ///     Run dotnet cli with provided command line arguments.
    /// </summary>
    public int Run(string application, string commandLineArguments,
                   TextWriter standardOut, TextWriter? errorOut = null)
    {
        Logger.LogTrace($"Running '{application} {commandLineArguments}'.");

        using var process = new Process();
        process.StartInfo.FileName = application;
        process.StartInfo.Arguments = commandLineArguments;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        if (WorkingDirectory.Length > 0)
        {
            process.StartInfo.WorkingDirectory = WorkingDirectory;
        }

        process.OutputDataReceived += (sender, data) => OnOutputDataReceived(data.Data, standardOut);

        if (errorOut != null)
        {
            process.ErrorDataReceived += (sender, data) => OnErrorDataReceived(data.Data, errorOut);
        }

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        var completed = process.WaitForExit(TimeLimitMilliseconds);
        if (completed)
        {
            process.WaitForExit();
        }

        if (!completed)
        {
            var message =
                $"ProcessCli.Run command timed out after {TimeLimitMilliseconds} milliseconds. Command was 'dotnet {commandLineArguments}'.";
            OnError(errorOut, message);
            process.Kill();
            process.WaitForExit(5000);
        }

        var exitCode = process.ExitCode;
        if (exitCode != 0)
        {
            var message = $"ProcessCli.Run command returned non-zero exit code {exitCode}.";
            OnError(errorOut, message);
        }

        standardOut.Flush();

        return exitCode;
    }

    private void OnError(TextWriter? errorOut, string message)
    {
        errorOut?.WriteLine(message);
        if (errorOut == null)
        {
            Logger.LogError(message);
        }
    }

    private static void OnErrorDataReceived(string? data, TextWriter errorOut)
    {
        if (data == null)
        {
            return;
        }

        errorOut.WriteLine(data);
    }

    private static void OnOutputDataReceived(string? data, TextWriter standardOut)
    {
        if (data == null)
        {
            return;
        }

        standardOut.WriteLine(data);
    }
}