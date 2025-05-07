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
            Console.WriteLine("===0===");
            //Console.WriteLine("==== " + Environment.GetEnvironmentVariable("TMPDIR"));
            var variables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry entry in variables)
            {
                Console.WriteLine($"  === {entry.Key} = {entry.Value}");
            }

            var teamcityHost = new TeamCityHost(new NullLogger());
            if (teamcityHost.MatchesHostSignature())
            {

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
