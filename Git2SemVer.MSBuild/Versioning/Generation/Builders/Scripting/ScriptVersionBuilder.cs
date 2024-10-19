using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.MSBuild.Tasking;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;

public sealed class ScriptVersionBuilder : IVersionBuilder
{
    private readonly ILogger _logger;

    public ScriptVersionBuilder(ILogger logger)
    {
        _logger = logger;
    }

    public void Build(IBuildHost host, IVersionGeneratorInputs inputs, IVersionOutputs outputs)
    {
        if (inputs.RunScript == false)
        {
            return;
        }

        var scriptRunner = new Git2SemVerScriptRunner(new MSBuildScriptRunner(_logger),
                                                      host,
                                                      inputs,
                                                      outputs,
                                                      _logger);
        var task = scriptRunner!.RunScript();
    }
}