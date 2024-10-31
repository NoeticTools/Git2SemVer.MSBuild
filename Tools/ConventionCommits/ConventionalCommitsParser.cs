using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace NoeticTools.Common.ConventionCommits
{
    public sealed class ConventionalCommitsParser
    {
        private readonly Regex _regex = new Regex(@"\A(?<ChangeType>(fix|feat|build|chore|ci|docs|style|refactor|perf|test))(\((?<scope>[\w\-\.]+)\))?(!)?: \s+(?<desc>\w+[\s\S]*)",
                                                  RegexOptions.IgnorePatternWhitespace);

        public CommitMessageMetadata Parse(string commitMessage)
        {
            var match = _regex.Match(commitMessage);
            if (!match.Success)
            {
                return new CommitMessageMetadata();
            }

            var changeType = match.GetGroupValue("ChangeType");
            var changeDescription = match.GetGroupValue("desc");
            return new CommitMessageMetadata(changeType, changeDescription);
        }
    }
}
