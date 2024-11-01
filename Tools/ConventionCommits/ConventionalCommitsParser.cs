using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace NoeticTools.Common.ConventionCommits
{
    public sealed class ConventionalCommitsParser
    {
        private readonly Regex _regex = new Regex("""
                                                  \A
                                                    (?<ChangeType>(fix|feat|build|chore|ci|docs|style|refactor|perf|test))
                                                      (\((?<scope>[\w\-\.]+)\))?(!)?: \s+(?<desc>\w+[^(\n|\r\n)]*)
                                                    ( (\n|\r\n){2} (?<body>.*?) )?
                                                    ( (\n|\r\n) 
                                                      ( 
                                                        (\n|\r\n) 
                                                        (?<footer> 
                                                          (BREAKING(\s|-)CHANGE | \w(\w|-)* )
                                                          :\s+ 
                                                          (\w|\#)(\w|\s)*?
                                                          (\n|\r\n)?
                                                        )*
                                                      )? 
                                                    )?
                                                    (\n|\r\n)?
                                                  \Z
                                                  """,
                                                  RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

        public CommitMessageMetadata Parse(string commitMessage)
        {
            var match = _regex.Match(commitMessage);

            if (!match.Success)
            {
                return new CommitMessageMetadata();
            }

            var changeType = match.GetGroupValue("ChangeType");
            var changeDescription = match.GetGroupValue("desc");
            var body = match.GetGroupValue("body");
            var keyValuePairs = GetFooterKeyValuePairs(match);
            return new CommitMessageMetadata(changeType, changeDescription, body, keyValuePairs);
        }

        private static List<(string key, string value)> GetFooterKeyValuePairs(Match match)
        {
            var keyValuePairs = new List<(string key, string value)>();
            var footerGroup = match.Groups["footer"];
            if (!footerGroup.Success)
            {
                return keyValuePairs;
            }

            foreach (Capture capture in footerGroup.Captures)
            {
                var line = capture.Value;
                var elements = line.Split(':');
                keyValuePairs.Add((key: elements[0], value: elements[1].Trim()));
            }

            return keyValuePairs;
        }
    }
}
