using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoC;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer;
using Newtonsoft.Json.Linq;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Tools.CI;
using NoeticTools.Git2SemVer.Testing.Core;


namespace NoeticTools.Git2SemVer.IntegrationTests.Framework
{
    internal static class TestLoggerFactory
    {
        private static int _loggerId = 1;

        public static ILogger Create()
        {
            //var variables = Environment.GetEnvironmentVariables();
            //foreach (DictionaryEntry entry in variables)
            //{
            //    Console.WriteLine($"  === {entry.Key} = {entry.Value}");
            //}

            var teamCityVersion = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");
            if (!string.IsNullOrWhiteSpace(teamCityVersion))
            {
                Console.WriteLine("===RUNNING ON TEAMCITY===");

                var logFilePath = Path.Combine(Environment.GetEnvironmentVariable("TMPDIR")!, "TestResults", $"test{_loggerId++:D3}.txt");
                var logger = new FileLogger(logFilePath);
                logger.LogInfo("Logging started.");
                Console.WriteLine("== temp dir: " + logFilePath);

                Console.WriteLine($"##teamcity[importData type='streamToBuildLog' filePath='{logFilePath}' wrapFileContentInBlock='false' charset='UTF-8']");
                return logger;
            }
            else
            {
                Console.WriteLine("===2===");
                return new NUnitLogger();
            }
        }
    }
}
