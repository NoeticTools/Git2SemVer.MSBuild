using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Core.Tools.DotnetCli;

/// <summary>
///     Help for executing dotnet cli commands.
/// </summary>
[RegisterTransient]
public sealed class DotNetTool : IDotNetTool
{
    private readonly ProcessCli _inner;

    public DotNetTool() : this(new ProcessCli(new NullTaskLogger()))
    {
    }

    public DotNetTool(ProcessCli inner)
    {
        _inner = inner;
    }

    public IProjectCommands Projects => new ProjectCommands(this);

    public ISolutionCommands Solution => new SolutionCommands(this);

    /// <summary>
    ///     Build solution with build caching disabled.
    /// </summary>
    /// <remarks>
    ///     See: <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build">dotnet build</see>
    /// </remarks>
    public int Build(string solution, string configuration, params string[] arguments)
    {
        _inner.Logger.LogInfo($"Building solution {solution}.");
        var dotNetCommandLine =
            $"build {solution} --configuration {configuration} --disable-build-servers {string.Join(" ", arguments)}";
        return Run(dotNetCommandLine);
    }

    /// <summary>
    ///     Pack project. Generates NuGet package.
    /// </summary>
    public int Pack(string project, string configuration, params string[] arguments)
    {
        _inner.Logger.LogInfo($"Packing project {project}.");
        var dotNetCommandLine = $"pack {project} --configuration {configuration} {string.Join(" ", arguments)}";
        return Run(dotNetCommandLine);
    }

    /// <summary>
    ///     Run dotnet cli with provided command line arguments.
    /// </summary>
    public int Run(string commandLineArguments)
    {
        return _inner.Run("dotnet", commandLineArguments);
    }

    public int RunReturningStdOut(string commandLineArguments, out string standardOutput)
    {
        return _inner.Run("dotnet", commandLineArguments, out standardOutput);
    }
}