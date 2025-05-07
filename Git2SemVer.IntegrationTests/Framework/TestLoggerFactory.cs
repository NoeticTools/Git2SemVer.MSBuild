using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Tools.CI;
using NoeticTools.Git2SemVer.Testing.Core;


namespace NoeticTools.Git2SemVer.IntegrationTests.Framework
{
    internal static class TestLoggerFactory
    {
        public static ILogger Create()
        {
            var variables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry entry in variables)
            {
                Console.WriteLine($"  === {entry.Key} = {entry.Value}");
            }

            //var teamcityHost = new TeamCityHost(new NullLogger());
            var teamCityVersion = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");
            Console.WriteLine("======= TC version: " + teamCityVersion);

            if (!string.IsNullOrWhiteSpace(teamCityVersion))
            {
                Console.WriteLine("===1===");

                Console.WriteLine("===RUNNING ON TEAMCITY===");

                var tempDir = Environment.GetEnvironmentVariable("TMPDIR");
                Console.WriteLine("== temp dir: " + tempDir);
                Console.WriteLine("== working directory: " + Directory.GetCurrentDirectory());

                //##teamcity[importData type='streamToBuildLog' filePath='path-to-file' wrapFileContentInBlock='false' charset='UTF-8']
                return new ConsoleLogger();
            }
            else
            {
                Console.WriteLine("===2===");
                return new NUnitLogger();
            }
        }
    }
}
