﻿using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Framework.Config;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

public sealed class BuildHostFactory(IConfiguration config, Action<string> buildOutput, ILogger logger)
{
    public IBuildHost Create(string hostType, string buildNumber, string buildContext, string inputsBuildIdFormat)
    {
        var host = new BuildHost(new BuildHostFinder(config, buildOutput, logger).Find(hostType), logger);

        if (!string.IsNullOrWhiteSpace(buildNumber))
        {
            host.BuildNumber = buildNumber;
        }

        if (!string.IsNullOrWhiteSpace(buildContext))
        {
            host.BuildContext = buildContext;
        }

        if (!string.IsNullOrWhiteSpace(inputsBuildIdFormat))
        {
            host.BuildIdFormat = inputsBuildIdFormat;
        }

        return host;
    }
}