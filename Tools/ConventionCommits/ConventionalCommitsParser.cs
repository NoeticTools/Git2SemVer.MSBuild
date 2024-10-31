using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace NoeticTools.Common.ConventionCommits
{
    public sealed class ConventionalCommitsParser
    {
        private readonly Regex _regex = new Regex(@"\A(?<ChangeType>(fix|feat|build|chore|ci|docs|style|refactor|perf|test))(\([\w\-\.]+\))?(!)?:\s([\w\s])+([\s\S]*)",
                                                  RegexOptions.IgnorePatternWhitespace);

        public CommitMessageMetadata Parse(string commitMessage)
        {
            var matches = _regex.Match(commitMessage);
            if (!matches.Success)
            {
                return new CommitMessageMetadata();
            }

            var changeType = matches.Groups["ChangeType"].Value;
            return new CommitMessageMetadata(changeType);
        }
    }
}
